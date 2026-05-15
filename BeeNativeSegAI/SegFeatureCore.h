#pragma once

#include <opencv2/core.hpp>

#include <vector>

namespace BeeSegAI {

struct FeatureConfig {
    int lbpRadius = 1;
    int hsvWindow = 5;
    int gaborSize = 7;
    float gaborSigma = 2.0f;
    float gaborLambda = 4.0f;
    cv::Rect roi;
};

class SegFeatureExtractor {
public:
    static constexpr int kNumFeatures = 24;

    SegFeatureExtractor();

    void Configure(const FeatureConfig& cfg);
    void ExtractCPU(const cv::Mat& srcBgr, std::vector<cv::Mat>& outPlanes) const;
    void ExtractGpu(const cv::UMat& srcBgr, std::vector<cv::UMat>& outPlanes) const;

    void PackSamples(const std::vector<cv::Mat>& planes,
                     const std::vector<cv::Point>& pixels,
                     cv::Mat& outSamples) const;

    void PlanesToInterleaved(const std::vector<cv::Mat>& planes,
                             const cv::Rect& roi,
                             cv::Mat& outRows) const;

private:
    FeatureConfig cfg_;
    cv::Mat gaborKernels_[4];
    cv::Mat lbpLookup_;
};

} // namespace BeeSegAI
