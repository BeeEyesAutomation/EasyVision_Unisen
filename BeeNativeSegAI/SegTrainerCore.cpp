#include "pch.h"

#include "SegTrainerCore.h"

#include "SegAIFileFormat.h"

#include <algorithm>
#include <numeric>
#include <stdexcept>

namespace BeeSegAI {
namespace {

cv::Rect EffectiveRoi(const cv::Rect& roi, const cv::Size& size)
{
    const cv::Rect imageRect(0, 0, size.width, size.height);
    if (roi.width <= 0 || roi.height <= 0) {
        return imageRect;
    }

    const cv::Rect clipped = roi & imageRect;
    return clipped.area() > 0 ? clipped : imageRect;
}

std::vector<cv::Point> SelectEvenly(const std::vector<cv::Point>& points, int count)
{
    if (count <= 0 || points.empty()) {
        return {};
    }
    if (count >= static_cast<int>(points.size())) {
        return points;
    }

    std::vector<cv::Point> selected;
    selected.reserve(count);
    const double step = static_cast<double>(points.size()) / static_cast<double>(count);
    for (int i = 0; i < count; ++i) {
        const int index = std::min(static_cast<int>(points.size()) - 1, static_cast<int>(i * step));
        selected.push_back(points[index]);
    }
    return selected;
}

void AppendRows(const std::vector<cv::Mat>& planes,
                const std::vector<cv::Point>& pixels,
                int label,
                cv::Mat& features,
                cv::Mat& labels,
                int& row)
{
    for (const cv::Point& pixel : pixels) {
        float* dst = features.ptr<float>(row);
        for (int feature = 0; feature < SegFeatureExtractor::kNumFeatures; ++feature) {
            dst[feature] = planes[feature].at<float>(pixel);
        }
        labels.at<int>(row, 0) = label;
        ++row;
    }
}

} // namespace

void SegTrainer::Configure(const FeatureConfig& cfg)
{
    cfg_ = cfg;
    extractor_.Configure(cfg_);
}

int SegTrainer::AddSample(const cv::Mat& bgr, const cv::Mat& mask, const cv::Rect& roi)
{
    if (bgr.empty() || bgr.type() != CV_8UC3 || mask.empty() || mask.type() != CV_8UC1 || mask.size() != bgr.size()) {
        return -2;
    }

    SegTrainingSample sample;
    bgr.copyTo(sample.bgr);
    sample.roi = EffectiveRoi(roi, bgr.size());

    for (int y = sample.roi.y; y < sample.roi.y + sample.roi.height; ++y) {
        const unsigned char* maskRow = mask.ptr<unsigned char>(y);
        for (int x = sample.roi.x; x < sample.roi.x + sample.roi.width; ++x) {
            const unsigned char value = maskRow[x];
            if (value == 1 || value >= 200) {
                sample.defectPixels.emplace_back(x, y);
            } else if (value == 2 || (value >= 100 && value < 200)) {
                sample.normalPixels.emplace_back(x, y);
            }
        }
    }

    if (sample.defectPixels.empty() && sample.normalPixels.empty()) {
        return -3;
    }

    samples_.push_back(std::move(sample));
    return 0;
}

int SegTrainer::Train(int numTrees,
                      int maxDepth,
                      int minSampleCount,
                      const std::function<void(int)>& progressCb,
                      const volatile int* cancelFlag)
{
    if (samples_.empty()) {
        return -3;
    }

    int totalDefect = 0;
    int totalNormal = 0;
    for (const SegTrainingSample& sample : samples_) {
        totalDefect += static_cast<int>(sample.defectPixels.size());
        totalNormal += static_cast<int>(sample.normalPixels.size());
    }

    if (totalDefect == 0 || totalNormal == 0) {
        return -2;
    }

    const int maxPerClass = 50000;
    const int useDefect = std::min(totalDefect, maxPerClass);
    const int useNormal = std::min(totalNormal, maxPerClass);

    cv::Mat allFeatures(useDefect + useNormal, SegFeatureExtractor::kNumFeatures, CV_32F);
    cv::Mat allLabels(useDefect + useNormal, 1, CV_32S);

    int row = 0;
    int remainingDefect = useDefect;
    int remainingNormal = useNormal;
    int remainingDefectPool = totalDefect;
    int remainingNormalPool = totalNormal;

    for (size_t i = 0; i < samples_.size(); ++i) {
        if (cancelFlag && *cancelFlag) {
            return -10;
        }

        const SegTrainingSample& sample = samples_[i];
        std::vector<cv::Mat> planes;

        FeatureConfig sampleCfg = cfg_;
        sampleCfg.roi = sample.roi;
        extractor_.Configure(sampleCfg);
        extractor_.ExtractCPU(sample.bgr, planes);

        const int defectTake = (i + 1 == samples_.size())
            ? remainingDefect
            : std::min(remainingDefect, static_cast<int>(
                  (static_cast<int64_t>(sample.defectPixels.size()) * useDefect) / std::max(1, totalDefect)));
        const int normalTake = (i + 1 == samples_.size())
            ? remainingNormal
            : std::min(remainingNormal, static_cast<int>(
                  (static_cast<int64_t>(sample.normalPixels.size()) * useNormal) / std::max(1, totalNormal)));

        const std::vector<cv::Point> defectSelected = SelectEvenly(sample.defectPixels, defectTake);
        const std::vector<cv::Point> normalSelected = SelectEvenly(sample.normalPixels, normalTake);

        AppendRows(planes, defectSelected, 1, allFeatures, allLabels, row);
        AppendRows(planes, normalSelected, 0, allFeatures, allLabels, row);

        remainingDefect -= static_cast<int>(defectSelected.size());
        remainingNormal -= static_cast<int>(normalSelected.size());
        remainingDefectPool -= static_cast<int>(sample.defectPixels.size());
        remainingNormalPool -= static_cast<int>(sample.normalPixels.size());
        (void)remainingDefectPool;
        (void)remainingNormalPool;

        if (progressCb) {
            progressCb(static_cast<int>((i + 1) * 40 / samples_.size()));
        }
    }

    if (row <= 1) {
        return -2;
    }

    cv::Mat trainFeatures = allFeatures.rowRange(0, row);
    cv::Mat trainLabels = allLabels.rowRange(0, row);

    model_ = cv::ml::RTrees::create();
    model_->setMaxDepth(std::max(1, maxDepth));
    model_->setMinSampleCount(std::max(1, minSampleCount));
    model_->setRegressionAccuracy(0.0f);
    model_->setUseSurrogates(false);
    model_->setMaxCategories(2);
    model_->setCalculateVarImportance(true);
    model_->setActiveVarCount(0);
    model_->setTermCriteria(cv::TermCriteria(
        cv::TermCriteria::MAX_ITER + cv::TermCriteria::EPS,
        std::max(1, numTrees),
        0.01));

    if (progressCb) {
        progressCb(50);
    }

    cv::Ptr<cv::ml::TrainData> trainData = cv::ml::TrainData::create(trainFeatures, cv::ml::ROW_SAMPLE, trainLabels);
    if (!model_->train(trainData)) {
        return -4;
    }

    if (progressCb) {
        progressCb(100);
    }

    return 0;
}

bool SegTrainer::SaveModel(const std::wstring& path, float threshold, uint32_t minArea) const
{
    return SaveSegai(path, model_, cfg_, threshold, minArea);
}

bool SegTrainer::LoadModel(const std::wstring& path, float& outThreshold, uint32_t& outMinArea)
{
    cv::Ptr<cv::ml::RTrees> loaded;
    FeatureConfig loadedCfg;
    if (!LoadSegai(path, loaded, loadedCfg, outThreshold, outMinArea)) {
        return false;
    }

    model_ = loaded;
    Configure(loadedCfg);
    return true;
}

float SegTrainer::PredictPixel(const cv::Mat& bgr, const cv::Point& pixel) const
{
    if (!model_ || model_->empty() || bgr.empty() || bgr.type() != CV_8UC3) {
        return -1.0f;
    }

    const cv::Rect bounds(0, 0, bgr.cols, bgr.rows);
    if (!bounds.contains(pixel)) {
        return -1.0f;
    }

    SegFeatureExtractor extractor = extractor_;
    FeatureConfig cfg = cfg_;
    cfg.roi = EffectiveRoi(cfg.roi, bgr.size());
    extractor.Configure(cfg);

    std::vector<cv::Mat> planes;
    extractor.ExtractCPU(bgr, planes);

    cv::Mat sample;
    extractor.PackSamples(planes, std::vector<cv::Point>{pixel}, sample);
    return model_->predict(sample);
}

} // namespace BeeSegAI
