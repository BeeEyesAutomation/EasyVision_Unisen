#pragma once
#include "WidthCore.h"

using namespace System;

namespace BeeCpp {

    public enum class LineOrientationCli { Any = 0, Horizontal = 1, Vertical = 2 };
    public enum class GapExtremumCli { Nearest = 0, Farthest = 1, Outermost = 2, Middle = 3 };
    public enum class SegmentStatTypeCli { Shortest = 0, Average = 1, Longest = 2 };

    public value struct Line2DCli {
        float Vx, Vy, X0, Y0;
    };

    public value struct GapResultCli {
        array<Line2DCli>^ Lines;
        Line2DCli LineA;
        Line2DCli LineB;
        cli::array<System::Drawing::Point>^ LineMid; // len=2

        // Pixel
        double GapMinPx, GapMediumPx, GapMaxPx;
        // Millimeter
        double GapMinMm, GapMediumMm, GapMaxMm;

        int InlierRemain;
    };

    public ref class WidthCli
    {
    public:
        WidthCli();
        ~WidthCli();
        !WidthCli();

        void SetMmPerPixel(double mmPerPixel);
        void SetRansac(int iterations, double threshold);

        // === API bạn yêu cầu ===
        void SetImgeRaw(IntPtr data, int width, int height, int stride, int channels);
        void SetImgeCrop(IntPtr data, int width, int height, int stride, int channels,
            float x, float y, float w, float h, float angleDeg);

        GapResultCli MeasureParallelGap(int numLines,
            GapExtremumCli extremum,
            LineOrientationCli orientation,
            SegmentStatTypeCli segStat,
            int minInlierRemain);

    private:
        BeeCpp::WidthCore* core_;
        static Line2DCli ToCli(const BeeCpp::Line2DCore& s);
    };

} // namespace BeeCppCli
