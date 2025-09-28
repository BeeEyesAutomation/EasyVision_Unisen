#pragma once
#include <opencv2/opencv.hpp>
#include <vector>
#include <cstdint>
#include "Global.h"
namespace BeeCpp {

    enum class LineOrientation { Any = 0, Horizontal = 1, Vertical = 2 };
    enum class GapExtremum { Nearest = 0, Farthest = 1, Outermost = 2, Middle = 3 };
    enum class SegmentStatType { Shortest = 0, Average = 1, Longest = 2 };

    struct Line2DCore {
        // OpenCV fitLine format: (vx, vy, x0, y0)
        float vx{}, vy{}, x0{}, y0{};
    };

    struct GapResultCore {
        std::vector<Line2DCore> lines;
        Line2DCore lineA{}, lineB{};
        cv::Point lineMid[2]{};

        // Pixel
        double gapMinPx{ 0 }, gapMedPx{ 0 }, gapMaxPx{ 0 };
        // Millimeter
        double gapMinMm{ 0 }, gapMedMm{ 0 }, gapMaxMm{ 0 };

        int inlierRemain{ 0 };
    };

    class WidthCore {
    public:
        WidthCore();

        void SetMmPerPixel(double mpp);
        void SetRansac(int iterations, double threshold);

        // data: pointer tới buffer 8-bit; stride theo byte
        void SetImageRaw(const unsigned char* data, int width, int height, int stride, int channels);
        void SetImageCrop(const unsigned char* data, int width, int height, int stride, int channels,
            float cx, float cy, float w, float h, float angleDeg);

        GapResultCore MeasureParallelGap(int numLines,
            GapExtremum extremum,
            LineOrientation orientation = LineOrientation::Any,
            SegmentStatType segStat = SegmentStatType::Average,
            int minInlierRemain = 2);

    private:
        double mmPerPixel_ = 1.0;
        int    ransacIter_ = 1000;
        double ransacThr_ = 2.0;

        cv::Mat raw_;   // GRAY
        cv::Mat edges_; // Canny result

        static Line2DCore FitLineCv(const std::vector<cv::Point2f>& inliers);
        static double DistanceBetweenLines(const Line2DCore& l1, const Line2DCore& l2);

        void buildEdges(); // build edges_ from raw_
        std::vector<cv::Point2f> extractEdgePoints() const;

        // RANSAC song song, ổn định
        static std::vector<std::pair<int, int>> precomputePairs(int n, int iterations, uint32_t seed);
        Line2DCore ransacFitLineParallel(const std::vector<cv::Point2f>& pts,
            std::vector<cv::Point2f>& outInliers) const;

        static double solveX(const Line2DCore& ln, double y);
        static double solveY(const Line2DCore& ln, double x);

        static double median8U(const cv::Mat& gray); // median helper 0..255
    };

} // namespace BeeCpp
