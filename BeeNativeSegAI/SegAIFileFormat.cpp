#include "pch.h"

#include "SegAIFileFormat.h"

#include <algorithm>
#include <array>
#include <filesystem>
#include <fstream>
#include <vector>

namespace BeeSegAI {
namespace {

constexpr std::array<char, 8> kMagic = {'S', 'E', 'G', 'A', 'I', '\0', '\0', '\0'};
constexpr uint32_t kVersion = 1;
constexpr uint32_t kFeatureFlags = 0x00FFFFFFu;
constexpr uint32_t kNumClasses = 2;

uint32_t Crc32(const std::vector<unsigned char>& data)
{
    uint32_t crc = 0xFFFFFFFFu;
    for (unsigned char byte : data) {
        crc ^= byte;
        for (int bit = 0; bit < 8; ++bit) {
            crc = (crc & 1u) ? ((crc >> 1) ^ 0xEDB88320u) : (crc >> 1);
        }
    }
    return ~crc;
}

std::string PathToUtf8(const std::filesystem::path& path)
{
    return path.u8string();
}

bool ReadAllBytes(const std::filesystem::path& path, std::vector<unsigned char>& out)
{
    std::ifstream stream(path, std::ios::binary);
    if (!stream) {
        return false;
    }

    stream.seekg(0, std::ios::end);
    const std::streamoff size = stream.tellg();
    if (size < 0) {
        return false;
    }

    out.resize(static_cast<size_t>(size));
    stream.seekg(0, std::ios::beg);
    if (!out.empty()) {
        stream.read(reinterpret_cast<char*>(out.data()), size);
    }

    return stream.good() || stream.eof();
}

bool WriteAllBytes(const std::filesystem::path& path, const std::vector<unsigned char>& data)
{
    std::ofstream stream(path, std::ios::binary | std::ios::trunc);
    if (!stream) {
        return false;
    }

    if (!data.empty()) {
        stream.write(reinterpret_cast<const char*>(data.data()), static_cast<std::streamsize>(data.size()));
    }

    return stream.good();
}

std::filesystem::path MakePayloadTempPath(const std::filesystem::path& segaiPath)
{
    std::filesystem::path temp = segaiPath;
    temp += L".rtree.yml";
    return temp;
}

SegAIHeader BuildHeader(const FeatureConfig& cfg, float threshold, uint32_t minArea, const std::vector<unsigned char>& payload)
{
    SegAIHeader header = {};
    std::copy(kMagic.begin(), kMagic.end(), header.magic);
    header.version = kVersion;
    header.featureFlags = kFeatureFlags;
    header.numClasses = kNumClasses;
    header.featureCount = SegFeatureExtractor::kNumFeatures;
    header.defectThreshold = threshold;
    header.minDefectArea = minArea;
    header.lbpRadius = static_cast<uint32_t>(cfg.lbpRadius);
    header.hsvWindow = static_cast<uint32_t>(cfg.hsvWindow);
    header.gaborSize = static_cast<uint32_t>(cfg.gaborSize);
    header.gaborSigma = cfg.gaborSigma;
    header.gaborLambda = cfg.gaborLambda;
    header.payloadSize = static_cast<uint64_t>(payload.size());
    header.crc32 = Crc32(payload);
    return header;
}

bool IsValidHeader(const SegAIHeader& header)
{
    return std::equal(kMagic.begin(), kMagic.end(), header.magic) &&
           header.version == kVersion &&
           header.featureCount == SegFeatureExtractor::kNumFeatures &&
           header.numClasses == kNumClasses &&
           header.payloadSize > 0;
}

} // namespace

bool SaveSegai(const std::wstring& path,
               const cv::Ptr<cv::ml::RTrees>& model,
               const FeatureConfig& cfg,
               float threshold,
               uint32_t minArea)
{
    if (!model || model->empty()) {
        return false;
    }

    const std::filesystem::path segaiPath(path);
    const std::filesystem::path payloadPath = MakePayloadTempPath(segaiPath);

    try {
        if (!segaiPath.parent_path().empty()) {
            std::filesystem::create_directories(segaiPath.parent_path());
        }

        model->save(PathToUtf8(payloadPath));

        std::vector<unsigned char> payload;
        if (!ReadAllBytes(payloadPath, payload) || payload.empty()) {
            std::filesystem::remove(payloadPath);
            return false;
        }
        std::filesystem::remove(payloadPath);

        const SegAIHeader header = BuildHeader(cfg, threshold, minArea, payload);

        std::ofstream stream(segaiPath, std::ios::binary | std::ios::trunc);
        if (!stream) {
            return false;
        }

        stream.write(reinterpret_cast<const char*>(&header), sizeof(header));
        stream.write(reinterpret_cast<const char*>(payload.data()), static_cast<std::streamsize>(payload.size()));
        return stream.good();
    } catch (...) {
        std::error_code ignored;
        std::filesystem::remove(payloadPath, ignored);
        return false;
    }
}

bool LoadSegai(const std::wstring& path,
               cv::Ptr<cv::ml::RTrees>& outModel,
               FeatureConfig& outCfg,
               float& outThreshold,
               uint32_t& outMinArea)
{
    const std::filesystem::path segaiPath(path);
    const std::filesystem::path payloadPath = MakePayloadTempPath(segaiPath);

    try {
        std::ifstream stream(segaiPath, std::ios::binary);
        if (!stream) {
            return false;
        }

        SegAIHeader header = {};
        stream.read(reinterpret_cast<char*>(&header), sizeof(header));
        if (!stream || !IsValidHeader(header)) {
            return false;
        }

        std::vector<unsigned char> payload(static_cast<size_t>(header.payloadSize));
        stream.read(reinterpret_cast<char*>(payload.data()), static_cast<std::streamsize>(payload.size()));
        if (!stream || Crc32(payload) != header.crc32) {
            return false;
        }

        if (!WriteAllBytes(payloadPath, payload)) {
            return false;
        }

        cv::Ptr<cv::ml::RTrees> model = cv::ml::RTrees::load(PathToUtf8(payloadPath));
        std::filesystem::remove(payloadPath);
        if (!model || model->empty()) {
            return false;
        }

        FeatureConfig cfg;
        cfg.lbpRadius = static_cast<int>(header.lbpRadius);
        cfg.hsvWindow = static_cast<int>(header.hsvWindow);
        cfg.gaborSize = static_cast<int>(header.gaborSize);
        cfg.gaborSigma = header.gaborSigma;
        cfg.gaborLambda = header.gaborLambda;

        outModel = model;
        outCfg = cfg;
        outThreshold = header.defectThreshold;
        outMinArea = header.minDefectArea;
        return true;
    } catch (...) {
        std::error_code ignored;
        std::filesystem::remove(payloadPath, ignored);
        return false;
    }
}

} // namespace BeeSegAI
