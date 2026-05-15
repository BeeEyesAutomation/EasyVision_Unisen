#include "pch.h"

#include "SegInferCore.h"

#include "SegAIFileFormat.h"

#include <opencv2/core/ocl.hpp>
#include <opencv2/imgproc.hpp>

#include <algorithm>
#include <mutex>

namespace BeeSegAI {
namespace {

std::mutex g_openCvRuntimeMutex;

struct ScopedOpenCvRuntimeLock
{
    std::unique_lock<std::mutex> lock;
    bool restoreOpenCl = false;
    bool oldOpenCl = false;

    void Lock()
    {
        if (!lock.owns_lock()) {
            lock = std::unique_lock<std::mutex>(g_openCvRuntimeMutex);
        }
    }

    void EnableOpenCl()
    {
        Lock();
        oldOpenCl = cv::ocl::useOpenCL();
        cv::ocl::setUseOpenCL(true);
        restoreOpenCl = true;
    }

    ~ScopedOpenCvRuntimeLock()
    {
        if (restoreOpenCl) {
            cv::ocl::setUseOpenCL(oldOpenCl);
        }
    }
};

cv::Rect EffectiveRoi(const cv::Rect& roi, const cv::Size& size)
{
    const cv::Rect imageRect(0, 0, size.width, size.height);
    if (roi.width <= 0 || roi.height <= 0) {
        return imageRect;
    }

    const cv::Rect clipped = roi & imageRect;
    return clipped.area() > 0 ? clipped : imageRect;
}

bool CanUseGpu(bool enable)
{
    if (!enable) {
        return false;
    }

    try {
        return cv::ocl::haveOpenCL();
    } catch (...) {
        return false;
    }
}

void PredictRows(const cv::Ptr<cv::ml::RTrees>& model, const cv::Mat& samples, cv::Mat& predRaw)
{
    predRaw.create(samples.rows, 1, CV_32F);
    cv::parallel_for_(cv::Range(0, samples.rows), [&](const cv::Range& range) {
        for (int row = range.start; row < range.end; ++row) {
            predRaw.at<float>(row, 0) = model->predict(samples.row(row));
        }
    });
}

void FilterConnectedComponents(const cv::Mat& roiMask, uint32_t minDefectArea, cv::Mat& filtered)
{
    cv::Mat labels;
    cv::Mat stats;
    cv::Mat centroids;
    const int componentCount = cv::connectedComponentsWithStats(roiMask, labels, stats, centroids, 8);

    filtered = cv::Mat::zeros(roiMask.size(), CV_8U);
    for (int label = 1; label < componentCount; ++label) {
        if (stats.at<int>(label, cv::CC_STAT_AREA) >= static_cast<int>(minDefectArea)) {
            filtered.setTo(255, labels == label);
        }
    }
}

} // namespace

bool SegInferer::LoadModel(const std::wstring& path)
{
    float threshold = 0.0f;
    uint32_t minArea = 0;
    cv::Ptr<cv::ml::RTrees> loaded;
    FeatureConfig loadedCfg;
    if (!LoadSegai(path, loaded, loadedCfg, threshold, minArea)) {
        return false;
    }

    model_ = loaded;
    cfg_ = loadedCfg;
    minDefectArea_ = minArea;
    extractor_.Configure(cfg_);
    return true;
}

int SegInferer::Predict(const cv::Mat& bgr,
                        const cv::Rect& roi,
                        float threshold,
                        cv::Mat& outMask,
                        float& outScore)
{
    if (CanUseGpu(useGpu_)) {
        const int rc = PredictGpu(bgr, roi, threshold, outMask, outScore);
        if (rc == 0) {
            return 0;
        }
    }

    return PredictCpu(bgr, roi, threshold, outMask, outScore);
}

int SegInferer::PredictCpu(const cv::Mat& bgr,
                           const cv::Rect& roi,
                           float threshold,
                           cv::Mat& outMask,
                           float& outScore)
{
    outScore = 0.0f;
    if (!model_ || model_->empty()) {
        return -1;
    }
    if (bgr.empty() || bgr.type() != CV_8UC3) {
        return -2;
    }

    const cv::Rect r = EffectiveRoi(roi, bgr.size());
    if (r.area() <= 0) {
        return -2;
    }

    cv::Mat roiView = bgr(r);

    FeatureConfig predictCfg = cfg_;
    predictCfg.roi = cv::Rect(0, 0, roiView.cols, roiView.rows);
    extractor_.Configure(predictCfg);

    std::vector<cv::Mat> planes;
    extractor_.ExtractCPU(roiView, planes);

    cv::Mat samples;
    extractor_.PlanesToInterleaved(planes, predictCfg.roi, samples);
    if (samples.empty()) {
        return -2;
    }

    cv::Mat predRaw;
    PredictRows(model_, samples, predRaw);

    cv::Mat roiPred = predRaw.reshape(1, roiView.rows);
    cv::Mat roiMask;
    cv::threshold(roiPred, roiMask, threshold, 255, cv::THRESH_BINARY);
    roiMask.convertTo(roiMask, CV_8U);

    cv::Mat filtered;
    FilterConnectedComponents(roiMask, minDefectArea_, filtered);

    outMask = cv::Mat::zeros(bgr.size(), CV_8U);
    filtered.copyTo(outMask(r));
    outScore = static_cast<float>(cv::countNonZero(filtered)) / static_cast<float>(roiView.total());
    return 0;
}

int SegInferer::PredictGpu(const cv::Mat& bgr,
                           const cv::Rect& roi,
                           float threshold,
                           cv::Mat& outMask,
                           float& outScore)
{
    outScore = 0.0f;
    if (!model_ || model_->empty()) {
        return -1;
    }
    if (bgr.empty() || bgr.type() != CV_8UC3) {
        return -2;
    }

    const cv::Rect r = EffectiveRoi(roi, bgr.size());
    if (r.area() <= 0) {
        return -2;
    }

    ScopedOpenCvRuntimeLock runtimeLock;
    runtimeLock.EnableOpenCl();

    cv::Mat roiView = bgr(r);

    FeatureConfig predictCfg = cfg_;
    predictCfg.roi = cv::Rect(0, 0, roiView.cols, roiView.rows);
    extractor_.Configure(predictCfg);

    cv::UMat srcGpu;
    roiView.copyTo(srcGpu);

    std::vector<cv::UMat> planesGpu;
    extractor_.ExtractGpu(srcGpu, planesGpu);

    std::vector<cv::UMat> roiPlanes;
    roiPlanes.reserve(planesGpu.size());
    for (size_t i = 0; i < planesGpu.size(); ++i) {
        roiPlanes.push_back(planesGpu[i](predictCfg.roi));
    }

    cv::UMat mergedGpu;
    cv::merge(roiPlanes, mergedGpu);
    cv::UMat samplesGpu = mergedGpu.reshape(1, predictCfg.roi.area());

    cv::Mat samples = samplesGpu.getMat(cv::ACCESS_READ);
    if (samples.empty()) {
        return -2;
    }

    cv::Mat predRaw;
    PredictRows(model_, samples, predRaw);

    cv::Mat roiPred = predRaw.reshape(1, roiView.rows);
    cv::Mat roiMask;
    cv::threshold(roiPred, roiMask, threshold, 255, cv::THRESH_BINARY);
    roiMask.convertTo(roiMask, CV_8U);

    cv::Mat filtered;
    FilterConnectedComponents(roiMask, minDefectArea_, filtered);

    outMask = cv::Mat::zeros(bgr.size(), CV_8U);
    filtered.copyTo(outMask(r));
    outScore = static_cast<float>(cv::countNonZero(filtered)) / static_cast<float>(roiView.total());
    return 0;
}

} // namespace BeeSegAI
