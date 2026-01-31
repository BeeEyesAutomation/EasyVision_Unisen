#pragma once
#include <opencv2/opencv.hpp>
#include <vector>

struct MonoSegParams
{
    int bgBlurK = 41;
    int openK = 2;
    int closeK = 4;
    int mode = 0;          // 0: bg-gray, 1: gray-bg
    bool useBlackHat = false;
    int blackHatK = 31;
};

// NOTE: sửa solidity -> fillRatio + auto sizeTol + detect paper
struct ChipExtractParams
{
    // lọc thô (chỉ dùng cho candidate trước khi auto-learn)
    int minArea = 80;
    int minW = 4;
    int minH = 20;

    // chip thường cao hơn rộng
    float minAspect = 1.2f;

    // morphology nối blob theo hướng dọc
    int vertKernelW = 3;
    int vertKernelH = 15;
    int openK = 3;

    // NEW: thay solidity bằng fill ratio
    float minFillRatio = 0.12f;     // AOI safe cho chip rỗng

    // NEW: auto-learn size theo median và cho phép lệch ±sizeTol
    float sizeTol = 0.40f;          // ±40%

    // NEW: paper là CC lớn nhất nếu vượt ngưỡng này (tính theo % diện tích ảnh)
    float paperMinAreaFrac = 0.02f; // 2% diện tích ảnh (tùy scene)
};

class MonoSegCore
{
public:
    // ====== STEP 1: Segment low-contrast mono ======
    static int SegmentLowContrastMono(
        const cv::Mat& gray8U,
        cv::Mat& outMask8U,
        const MonoSegParams& p,
        cv::Mat* outScore = nullptr
    );
    // ====== STEP 1: Segment low-contrast mono ======
    static int SegmentLowContrastMonoHardNoise(
        const cv::Mat& gray8U,
        cv::Mat& outMask8U,
        const MonoSegParams& p,
        cv::Mat* outScore = nullptr
    );
    // ====== BACKWARD: Extract chip boxes (axis-aligned) ======
    // (giữ lại để không vỡ chỗ khác, nhưng nội bộ dùng fillRatio + auto size)
    static int ExtractChipBoxesFromMask(
        const cv::Mat& mask8U,                 // CV_8UC1 0/255
        std::vector<cv::Rect>& outBoxes,
        const ChipExtractParams& p
    );

    // ====== NEW: Extract PAPER + CHIP rotated rects ======
    // outIsPaper[i] = 1 nếu là paper, 0 nếu là chip
    static int ExtractPaperAndChipRectRotatesFromMask(
        const cv::Mat& mask8U,                 // CV_8UC1 0/255
        std::vector<cv::RotatedRect>& outRR,
        std::vector<uint8_t>& outIsPaper,
        const ChipExtractParams& p
    );

    // ====== Draw axis-aligned boxes to BGR image ======
    static void DrawBoxesToColorImage(
        const cv::Mat& base8U,                 // CV_8UC1
        const std::vector<cv::Rect>& boxes,
        cv::Mat& outBGR,                       // CV_8UC3
        cv::Scalar boxColor = cv::Scalar(0, 255, 0),
        int thickness = 2
    );

    // ====== NEW: Draw rotated rects to BGR image (paper đỏ, chip xanh) ======
    static void DrawRectRotatesToColorImage(
        const cv::Mat& base8U,                 // CV_8UC1
        const std::vector<cv::RotatedRect>& rects,
        const std::vector<uint8_t>& isPaper,
        cv::Mat& outBGR,                       // CV_8UC3
        int thickness = 2
    );

private:
    static int AutoThresholdHistogramValley(const cv::Mat& score8U);
    static inline int MakeOdd(int k) { return (k % 2 == 0) ? (k + 1) : k; }

    static float MedianInplace(std::vector<float>& v);
    static cv::RotatedRect MinAreaRect_GlobalFromROI(const cv::Mat& roiMask, const cv::Rect& roiBox);
};
