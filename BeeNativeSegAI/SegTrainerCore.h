#pragma once

#include "SegFeatureCore.h"

#include <opencv2/ml.hpp>

#include <cstdint>
#include <functional>
#include <string>
#include <vector>

namespace BeeSegAI {

struct SegTrainingSample {
    cv::Mat bgr;
    cv::Rect roi;
    std::vector<cv::Point> defectPixels;
    std::vector<cv::Point> normalPixels;
};

class SegTrainer {
public:
    void Configure(const FeatureConfig& cfg);
    int AddSample(const cv::Mat& bgr, const cv::Mat& mask, const cv::Rect& roi);
    int Train(int numTrees,
              int maxDepth,
              int minSampleCount,
              const std::function<void(int)>& progressCb = nullptr,
              const volatile int* cancelFlag = nullptr);

    bool SaveModel(const std::wstring& path, float threshold, uint32_t minArea) const;
    bool LoadModel(const std::wstring& path, float& outThreshold, uint32_t& outMinArea);
    float PredictPixel(const cv::Mat& bgr, const cv::Point& pixel) const;

    const cv::Ptr<cv::ml::RTrees>& Model() const { return model_; }
    const FeatureConfig& Config() const { return cfg_; }
    const std::vector<SegTrainingSample>& Samples() const { return samples_; }

private:
    FeatureConfig cfg_;
    SegFeatureExtractor extractor_;
    std::vector<SegTrainingSample> samples_;
    cv::Ptr<cv::ml::RTrees> model_;
};

} // namespace BeeSegAI
