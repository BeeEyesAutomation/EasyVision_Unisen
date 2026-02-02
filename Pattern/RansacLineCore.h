#pragma once
#include <opencv2/opencv.hpp>
#include <vector>

namespace BeeCpp {

    struct LineResult {
        bool          found = false;
        cv::Vec4f     line;        // (vx, vy, x0, y0) từ cv::fitLine
        cv::Point2f   p1;          // endpoint 1
        cv::Point2f   p2;          // endpoint 2
        int           inliers = 0;
        float         score = 0.f;     // inliers / tổng sample
        float         length_px = 0.f;   // độ dài theo pixel
        float         length_mm = 0.f;   // độ dài theo mm (= length_px * mmPerPixel)
    };
    enum LineDirNative
    {
        Any = 0,        // như hiện tại
        Horizontal,     // gần 0° / 180°
        Vertical,       // gần 90°
        AngleRange      // dải góc tùy chọn
    };
    enum class LineScanPriority
    {
        None = 0,

        LeftToRight,
        RightToLeft,

        TopToBottom,
        BottomToTop
    };
    class RansacLineCore {
    private: static bool CheckDirection(
        double dx, double dy,
        LineDirNative mode,
        float angleCenterDeg,
        float tolDeg
    );
    public:
        // Overload mới: có tham số mmPerPixel
        static LineResult FindBestLine(
            const cv::Mat& edges8u1,     // ảnh edges CV_8UC1
            int iterations = 2000,       // số vòng RANSAC
            float threshold = 1.5f,      // ngưỡng khoảng cách điểm -> line (px)
            int maxPoints = 10000,       // giới hạn sample điểm
            unsigned seed = 987654321u,  // seed RNG
            float mmPerPixel = 1.0f  ,    // scale mm/pixel
            float AspectLen=0.6f,
            LineDirNative dirMode = LineDirNative::Any,
            float angleCenterDeg = 0.0f,   // dùng cho AngleRange
            float angleToleranceDeg = 10.0f,
            LineScanPriority scanMode = LineScanPriority::None // <<< NEW
        );

        // Overload cũ (tương thích ngược)
        static inline LineResult FindBestLine(
            const cv::Mat& edges8u1,
            int iterations,
            float threshold,
            int maxPoints,
            unsigned seed
        ) {
            return FindBestLine(edges8u1, iterations, threshold, maxPoints, seed, 1.0f);
        }

    private:
        static inline uint32_t NextIdx(uint32_t& s, int n) {
            s ^= s << 13; s ^= s >> 17; s ^= s << 5;  // xorshift32
            return (uint32_t)(s % (uint32_t)n);
        }

        static std::vector<std::pair<int, int>> PrecomputePairs(int n, int iterations, uint32_t seed);
    };

} // namespace BeeCpp

