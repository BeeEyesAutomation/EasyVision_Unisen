#include "pch.h"

#include "SegAINativeExport.h"

#include "SegInferCore.h"
#include "SegTrainerCore.h"

#include <opencv2/core/ocl.hpp>

#include <algorithm>
#include <cstring>
#include <memory>

namespace {

struct TrainerHandle {
    BeeSegAI::FeatureConfig cfg;
    BeeSegAI::SegTrainer trainer;
    cv::Rect roi;
};

struct InferHandle {
    BeeSegAI::SegInferer inferer;
};

cv::Mat WrapBgr(const uint8_t* bgr, int w, int h, int step)
{
    if (!bgr || w <= 0 || h <= 0 || step < w * 3) {
        return {};
    }
    return cv::Mat(h, w, CV_8UC3, const_cast<uint8_t*>(bgr), static_cast<size_t>(step));
}

cv::Mat WrapMask(const uint8_t* mask, int w, int h, int step)
{
    if (!mask || w <= 0 || h <= 0 || step < w) {
        return {};
    }
    return cv::Mat(h, w, CV_8UC1, const_cast<uint8_t*>(mask), static_cast<size_t>(step));
}

} // namespace

SAPI void* SEGAI_TrainerCreate()
{
    try {
        std::unique_ptr<TrainerHandle> handle(new TrainerHandle());
        handle->cfg.roi = cv::Rect();
        handle->trainer.Configure(handle->cfg);
        return handle.release();
    } catch (...) {
        return nullptr;
    }
}

SAPI void SEGAI_TrainerDestroy(void* handle)
{
    delete static_cast<TrainerHandle*>(handle);
}

SAPI int SEGAI_TrainerSetROI(void* handle, int x, int y, int w, int h)
{
    TrainerHandle* trainer = static_cast<TrainerHandle*>(handle);
    if (!trainer || w <= 0 || h <= 0) {
        return -1;
    }

    trainer->roi = cv::Rect(x, y, w, h);
    trainer->cfg.roi = trainer->roi;
    trainer->trainer.Configure(trainer->cfg);
    return 0;
}

SAPI int SEGAI_TrainerAddSample(void* handle,
                                const uint8_t* bgr,
                                int w,
                                int h,
                                int step,
                                const uint8_t* mask,
                                int maskStep)
{
    TrainerHandle* trainer = static_cast<TrainerHandle*>(handle);
    if (!trainer) {
        return -1;
    }

    cv::Mat bgrMat = WrapBgr(bgr, w, h, step);
    cv::Mat maskMat = WrapMask(mask, w, h, maskStep);
    if (bgrMat.empty() || maskMat.empty()) {
        return -2;
    }

    const cv::Rect roi = trainer->roi.area() > 0 ? trainer->roi : cv::Rect(0, 0, w, h);
    return trainer->trainer.AddSample(bgrMat, maskMat, roi);
}

SAPI void SEGAI_TrainerClearSamples(void* handle)
{
    TrainerHandle* trainer = static_cast<TrainerHandle*>(handle);
    if (!trainer) {
        return;
    }

    trainer->trainer = BeeSegAI::SegTrainer();
    trainer->trainer.Configure(trainer->cfg);
}

SAPI int SEGAI_TrainerTrain(void* handle,
                            int numTrees,
                            int maxDepth,
                            int minSampleCount,
                            void(*progressCb)(int, void*),
                            void* userData,
                            const volatile int* cancelFlag)
{
    TrainerHandle* trainer = static_cast<TrainerHandle*>(handle);
    if (!trainer) {
        return -1;
    }

    return trainer->trainer.Train(
        numTrees,
        maxDepth,
        minSampleCount,
        [progressCb, userData](int progress) {
            if (progressCb) {
                progressCb(progress, userData);
            }
        },
        cancelFlag);
}

SAPI int SEGAI_TrainerSave(void* handle, const wchar_t* path, float defectThreshold, uint32_t minDefectArea)
{
    TrainerHandle* trainer = static_cast<TrainerHandle*>(handle);
    if (!trainer || !path) {
        return -1;
    }

    return trainer->trainer.SaveModel(path, defectThreshold, minDefectArea) ? 0 : -5;
}

SAPI int SEGAI_TrainerSampleCount(void* handle, int* outDefectPixels, int* outNormalPixels)
{
    TrainerHandle* trainer = static_cast<TrainerHandle*>(handle);
    if (!trainer || !outDefectPixels || !outNormalPixels) {
        return -1;
    }

    int defect = 0;
    int normal = 0;
    for (const BeeSegAI::SegTrainingSample& sample : trainer->trainer.Samples()) {
        defect += static_cast<int>(sample.defectPixels.size());
        normal += static_cast<int>(sample.normalPixels.size());
    }

    *outDefectPixels = defect;
    *outNormalPixels = normal;
    return 0;
}

SAPI void* SEGAI_InferCreate()
{
    try {
        return new InferHandle();
    } catch (...) {
        return nullptr;
    }
}

SAPI void SEGAI_InferDestroy(void* handle)
{
    delete static_cast<InferHandle*>(handle);
}

SAPI int SEGAI_InferLoad(void* handle, const wchar_t* path)
{
    InferHandle* infer = static_cast<InferHandle*>(handle);
    if (!infer || !path) {
        return -1;
    }

    return infer->inferer.LoadModel(path) ? 0 : -5;
}

SAPI int SEGAI_InferSetGpu(void* handle, int enable)
{
    InferHandle* infer = static_cast<InferHandle*>(handle);
    if (!infer) {
        return -1;
    }

    infer->inferer.SetGpuEnabled(enable != 0);
    return 0;
}

SAPI int SEGAI_InferGetGpuAvailable()
{
    try {
        return cv::ocl::haveOpenCL() ? 1 : 0;
    } catch (...) {
        return 0;
    }
}

SAPI int SEGAI_InferPredict(void* handle,
                            const uint8_t* bgr,
                            int w,
                            int h,
                            int step,
                            int roiX,
                            int roiY,
                            int roiW,
                            int roiH,
                            float threshold,
                            uint8_t** outMaskPtr,
                            int* outMaskW,
                            int* outMaskH,
                            float* outScore)
{
    InferHandle* infer = static_cast<InferHandle*>(handle);
    if (!infer || !outMaskPtr || !outMaskW || !outMaskH || !outScore) {
        return -1;
    }

    *outMaskPtr = nullptr;
    *outMaskW = 0;
    *outMaskH = 0;
    *outScore = 0.0f;

    cv::Mat bgrMat = WrapBgr(bgr, w, h, step);
    if (bgrMat.empty()) {
        return -2;
    }

    cv::Mat mask;
    float score = 0.0f;
    const int rc = infer->inferer.Predict(bgrMat, cv::Rect(roiX, roiY, roiW, roiH), threshold, mask, score);
    if (rc != 0) {
        return rc;
    }

    const size_t byteCount = static_cast<size_t>(mask.rows) * static_cast<size_t>(mask.cols);
    uint8_t* buffer = new uint8_t[byteCount];
    std::memcpy(buffer, mask.data, byteCount);

    *outMaskPtr = buffer;
    *outMaskW = mask.cols;
    *outMaskH = mask.rows;
    *outScore = score;
    return 0;
}

SAPI void SEGAI_FreeBuffer(void* ptr)
{
    delete[] static_cast<uint8_t*>(ptr);
}
