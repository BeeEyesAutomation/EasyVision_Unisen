// barcode_pca_autoclass.hpp
#pragma once
#include <opencv2/opencv.hpp>
#include <vector>
#include <string>

enum class BarcodeKind { Unknown = 0, Linear1D, QR, DataMatrix, PDF417 };

struct BarcodeDetectParams {
    int   blurKsize = 0;
    int   sobelKsize = -1;
    float gradPercentile = 0.0f;
    int   morphCloseW = 0;
    int   morphCloseH = 0;
    double minArea = 800.0;
    double minAspect = 2.0;
    double minAnisotropy = 2.0;
    double padScale = 1.12;
    int   maxCandidates = 8;
    bool  returnAll = true;
};

struct DebugDrawOptions {
    std::string outDir = "debug_out";
    bool saveStages = true;   // gray/grad/thresh/morph
    bool saveCrops = true;   // crop từng mã
    bool saveOverlay = true;   // overlay_all + overlay từng ứng viên
    int  thickness = 2;      // nét vẽ
    double visScale = 1.0;    // scale khi lưu overlay
};

struct BarcodeRegion {
    cv::RotatedRect box;
    cv::Mat cropped;
    BarcodeKind kind = BarcodeKind::Unknown;
    double confidence = 0.0;
    double score = 0.0;
};

std::vector<BarcodeRegion>
detectAndClassifyBarcodes(const cv::Mat& bgrOrGray,
    const BarcodeDetectParams& P = {});

// Overload có debug
std::vector<BarcodeRegion>
detectAndClassifyBarcodes(const cv::Mat& bgrOrGray,
    const BarcodeDetectParams& P,
    const DebugDrawOptions& D);
