#pragma once

#include <opencv2/opencv.hpp>
#include <string>
#include <vector>

namespace BeeCpp {

enum class PinCenterMethod {
    Geometry = 0,
    WeightedCentroidFallback = 1
};

enum class PinArrangeMode {
    X = 0,
    Y = 1,
    RowProjection = 2
};

struct PinPitchOptions {
    int expectedCount = 4;
    PinArrangeMode arrangeMode = PinArrangeMode::X;
    bool useProjectedPitch = true;
    bool useAutoThreshold = true;
    int manualThreshold = 180;
    int morphClose = 3;
    int morphOpen = 0;
    double minAreaPx = 12.0;
    double maxAreaRatio = 0.10;
    double minAspect = 0.45;
    double maxAspect = 2.20;
    double minFillRatio = 0.20;
    bool useOutlineCenter = true;
    int outlineThresholdOffset = 14;
    int outlineClose = 7;
    int outlineDilate = 5;
    int outlinePadding = 8;
    int maxOutlineExpand = 90;
    double mmPerPixel = 1.0;
    // Bug B+C: tách pin khỏi background không thuần đen (halo / phản chiếu / bóng đổ)
    // Top-hat morphology trước threshold để bỏ background biến thiên chậm.
    bool useTopHat = false;
    int topHatKernelPx = 0;          // 0 = auto từ ảnh; cần > kích thước pin lớn nhất
    // Bug C+D: từ chối blob đã merge với halo (halo lan thì solidity thấp).
    double minSolidity = 0.0;        // 0 = không filter; pin vuông thật ~0.85+
    // Bug B: tránh phình halo. Dùng dilate nhỏ (1-3) thay default cũ 5.
    bool reduceDilateForOutline = false;
    // Bug 1+4 (2026-05-08 runtime): mask từ Canny edges thay vì threshold pixel sáng.
    // Threshold blob bị bias về vùng phản chiếu sáng + bóng mờ; edge boundary follow
    // biên thật của pin pad bất kể bright spot ở đâu, và shadow không có gradient mạnh
    // nên Canny tự loại.
    bool useEdgeBoundary = false;
    int edgeCannyLow = 20;
    int edgeCannyHigh = 60;
    // Bug 1+2: center từ midpoint của projection bounds trên 2 trục của minAreaRect,
    // robust với pin xéo và bright spot off-center.
    bool useEdgeGeometryCenter = false;
    // Runtime trial 2 (2026-05-08): pin pad contrast yếu với background -> Canny global
    // chỉ bắt được biên bright reflection bên trong, box detect quá nhỏ. Refinement
    // chạy CLAHE + Sobel magnitude trong patch quanh seed để boost weak edges, tìm
    // contour bao quanh seed -> dùng làm box pad thật.
    bool useGradientRefinement = false;
    int gradientPatchMargin = 60;        // px mở rộng quanh box ban đầu
    int gradientThreshold = 25;          // ngưỡng Sobel magnitude (8U normalize)
    double claheClipLimit = 3.0;
    int claheTileSize = 8;
};

struct PinCenterResult {
    int id = 0;
    cv::Point2f center;
    cv::RotatedRect box;
    double score = 0.0;
    double areaPx = 0.0;
    double fillRatio = 0.0;
    PinCenterMethod method = PinCenterMethod::Geometry;
};

struct PinPitchResultCore {
    bool found = false;
    int status = 0;
    std::string message;
    std::vector<PinCenterResult> pins;
    std::vector<double> adjacentPitchMm;
    double spanP1P4Mm = 0.0;
    cv::Vec4f rowLine = cv::Vec4f(1.f, 0.f, 0.f, 0.f);
    double rowResidualPx = 0.0;
    double scaleMmPerPx = 1.0;
    cv::Mat debugBGR;
};

class PinPitchCore {
public:
    void setImage(const cv::Mat& image);
    void setOptions(const PinPitchOptions& options);
    PinPitchResultCore measure() const;

private:
    cv::Mat _image;
    PinPitchOptions _options;

    static cv::Mat BuildMask(const cv::Mat& gray, const PinPitchOptions& options);
    static cv::Point2f ComputeEdgeGeometryCenter(const std::vector<cv::Point>& contour, const cv::RotatedRect& box);
    static bool RefineByGradient(const cv::Mat& gray, PinCenterResult& cand, const PinPitchOptions& options);
    static std::vector<PinCenterResult> FindCandidates(const cv::Mat& gray, const cv::Mat& mask, const PinPitchOptions& options);
    static PinCenterResult BuildCandidate(const cv::Mat& gray, const std::vector<cv::Point>& contour, const PinPitchOptions& options);
    static void RefineCandidateWithOutline(const cv::Mat& gray, PinCenterResult& candidate, const PinPitchOptions& options);
    static cv::RotatedRect ExpandBox(const cv::RotatedRect& box, double padding);
    static cv::RotatedRect BuildAxisAlignedBox(const std::vector<cv::Point>& contour, double padding);
    static double Percentile8U(const cv::Mat& image, double percentile);
    static cv::Point2f WeightedCentroid(const cv::Mat& gray, const cv::Rect& roi, const cv::Mat& componentMask);
    static void SortAndAssignIds(std::vector<PinCenterResult>& pins, PinArrangeMode arrangeMode);
    static void ComputePitch(PinPitchResultCore& result, bool useProjectedPitch);
    static cv::Mat DrawDebug(const cv::Mat& gray, const cv::Mat& mask, const PinPitchResultCore& result);
};

} // namespace BeeCpp
