#pragma once
#include <opencv2/opencv.hpp>
#include "RansacLineCore.h"

using namespace System;

namespace BeeCpp {

    public value struct Line2DCli {
        bool  Found;
        // đoạn thẳng
        float X1, Y1, X2, Y2;
        // tham số đường (cv::fitLine)
        float Vx, Vy, X0, Y0;
        // thống kê
        int   Inliers;
        float Score;
        // độ dài
        float LengthPx;
        float LengthMm;
    };

    public enum class LineDirectionMode
    {
        Any = 0,        // như hiện tại
        Horizontal,     // gần 0° / 180°
        Vertical,       // gần 90°
        AngleRange      // dải góc tùy chọn
    };


        public ref struct DebugDrawOptionsCli {
            int   LineThickness;
            bool  DrawInliers;
            bool  DrawOutliers;
            int   PointRadius;
            int   BandThickness;
            double FontScale;
            int   FontThickness;
            // màu giữ nguyên default ở core; nếu cần, ta có thể thêm map màu sau
        };
        public enum class LineScanMode
        {
            None = 0,
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        };
        public ref class RansacLine {
       
        public:
            // === API cũ vẫn giữ ===
            static Line2DCli FindBestLine(
                IntPtr edge,
                int width,
                int height,
                int step,
                int iterations,
                float threshold,
                int maxPoints,
                int seed,
                float mmPerPixel,
                float AspectLen,
                LineDirectionMode dirMode,
                LineScanMode scanMode,          // <<< NEW
                float angleCenterDeg,
                float angleToleranceDeg
            );

            // === API mới: debug & save ===
            static Line2DCli FindBestLineAndDebug(
                IntPtr edgeData, int width, int height, int stride,
                int iterations, float threshold, int maxPoints, int seed, float mmPerPixel,
                // ảnh nền để vẽ (có thể cùng buffer với edge; chấp nhận 8UC1/8UC3/8UC4)
                IntPtr dbgBaseData, int dbgBaseType, size_t dbgBaseStep,
                // buffer đích để nhận BGR (CV_8UC3)
                IntPtr dbgOutData, int dbgOutType, size_t dbgOutStep,
                DebugDrawOptionsCli^ opts,
                System:: String^ savePath   // null/empty nếu không muốn lưu
            );
        };

    } // namespace BeeCppCli


 // namespace BeeCppCli
