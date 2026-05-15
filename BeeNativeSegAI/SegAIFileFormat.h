#pragma once

#include "SegFeatureCore.h"

#include <opencv2/ml.hpp>

#include <cstdint>
#include <string>

namespace BeeSegAI {

#pragma pack(push, 1)
struct SegAIHeader {
    char magic[8];
    uint32_t version;
    uint32_t featureFlags;
    uint32_t numClasses;
    uint32_t featureCount;
    float defectThreshold;
    uint32_t minDefectArea;
    uint32_t lbpRadius;
    uint32_t hsvWindow;
    uint32_t gaborSize;
    float gaborSigma;
    float gaborLambda;
    uint32_t reservedA;
    uint64_t payloadSize;
    uint32_t crc32;
    uint32_t reservedB;
};
#pragma pack(pop)

static_assert(sizeof(SegAIHeader) == 72, "SegAIHeader must stay binary-compatible.");

bool SaveSegai(const std::wstring& path,
               const cv::Ptr<cv::ml::RTrees>& model,
               const FeatureConfig& cfg,
               float threshold,
               uint32_t minArea);

bool LoadSegai(const std::wstring& path,
               cv::Ptr<cv::ml::RTrees>& outModel,
               FeatureConfig& outCfg,
               float& outThreshold,
               uint32_t& outMinArea);

} // namespace BeeSegAI
