#pragma once

#include "SegFeatureCore.h"

#include <opencv2/ml.hpp>

#include <cstdint>
#include <string>

namespace BeeSegAI {

class SegInferer {
public:
    bool LoadModel(const std::wstring& path);
    void SetGpuEnabled(bool enabled) { useGpu_ = enabled; }
    bool GpuEnabled() const { return useGpu_; }

    int Predict(const cv::Mat& bgr,
                const cv::Rect& roi,
                float threshold,
                cv::Mat& outMask,
                float& outScore);

private:
    int PredictCpu(const cv::Mat& bgr,
                   const cv::Rect& roi,
                   float threshold,
                   cv::Mat& outMask,
                   float& outScore);

    int PredictGpu(const cv::Mat& bgr,
                   const cv::Rect& roi,
                   float threshold,
                   cv::Mat& outMask,
                   float& outScore);

    FeatureConfig cfg_;
    SegFeatureExtractor extractor_;
    cv::Ptr<cv::ml::RTrees> model_;
    uint32_t minDefectArea_ = 50;
    bool useGpu_ = false;
};

} // namespace BeeSegAI
