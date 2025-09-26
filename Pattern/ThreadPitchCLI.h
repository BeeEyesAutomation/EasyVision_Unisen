#pragma once
#using <System.dll>

#include "ThreadPitchCore.h"
#include <opencv2/opencv.hpp>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace BeeCpp {

    public ref class ThreadPitchResult
    {
    public:
        array<int>^ CrestY;

        // Pitch (px)
        double MeanPitchPx;
        double StdPitchPx;
        double MedianPitchPx;
        double MinPitchPx;
        double MaxPitchPx;

        // Pitch (mm)
        double MeanPitchMm;
        double MedianPitchMm;
        double MinPitchMm;
        double MaxPitchMm;

        // Đoạn ren
        int    SegmentTopY;
        int    SegmentBottomY;
        double SegmentHeightPx;
        double SegmentHeightMm;

        int    ThreadCount;
        double AngleDeg;

        // Debug image (BGR8) trả về dạng byte[], cùng thông số
        array<Byte>^ DebugBgr;  // length = DebugStride * DebugHeight
        int DebugWidth;
        int DebugHeight;
        int DebugStride;
        int DebugChannels; // = 3
    };

    public ref class ThreadPitch sealed
    {
    public:
        // Nạp ảnh input từ OpenCvSharp.Mat (IntPtr + thông số)
         void SetInputMat(IntPtr data, int width, int height, int stride, int channels);

        // Đo trên ảnh global đã nạp
         ThreadPitchResult^ Measure(double mmPerPx,
            int bandHalfWidth,
            int expectedMinPitchPx,
            bool useGabor);

    private:
         ThreadPitchResult^ ConvertResult(const BeeCpp::PitchResult& r);
    };

} // namespace BeeVision
