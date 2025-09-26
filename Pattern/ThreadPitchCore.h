#pragma once
#include <opencv2/opencv.hpp>
#include <vector>

namespace BeeCpp {
    enum class Axis { Vertical = 0, Horizontal = 1 };
    struct PitchResult {
        std::vector<int> crestY;   // toạ độ y (sau xoay) của các đỉnh ren

        // Thống kê pitch (pixel)
        double meanPitchPx = 0.0;
        double stdPitchPx = 0.0;
        double medianPitchPx = 0.0;
        double minPitchPx = 0.0;
        double maxPitchPx = 0.0;

        // Quy đổi mm (nếu có mm_per_px > 0)
        double meanPitchMm = 0.0;
        double medianPitchMm = 0.0;
        double minPitchMm = 0.0;
        double maxPitchMm = 0.0;

        // Đoạn ren (từ crest đầu đến crest cuối)
        int    segmentTopY = 0;
        int    segmentBottomY = 0;
        double segmentHeightPx = 0.0;
        double segmentHeightMm = 0.0;

        // Số ren trong đoạn đo (số khoảng cách giữa các đỉnh)
        int    threadCount = 0;

        // Góc xoay để đưa trục ren thẳng đứng (dương = CCW)
        double angleDeg = 0.0;

        // Ảnh debug BGR (đã xoay) có vẽ dải & các đỉnh
        cv::Mat debugBGR;
    };

    // ===== API giữ ảnh đầu vào global =====
    void        SetInputImage(const cv::Mat& bgr);  // clone & lưu global (1/3 kênh đều ok)
    PitchResult MeasureThreadPitch(double mm_per_px = 0.0,
        int bandHalfWidth = 15,
        int expectedMinPitchPx = 6,
        bool useGabor = true);

    // ====== (public) tiện ích nếu bạn muốn dùng riêng lẻ ======
    cv::Mat rotateKeep(const cv::Mat& src, double angleDeg);
    double  estimateAxisAngleDeg(const cv::Mat& gray);

} // namespace tpitch
