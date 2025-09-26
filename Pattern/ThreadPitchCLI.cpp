#using <System.dll>
#include "ThreadPitchCLI.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace BeeCpp {

    ThreadPitchResult^ ThreadPitch::ConvertResult(const BeeCpp::PitchResult& r)
    {
        ThreadPitchResult^ out = gcnew ThreadPitchResult();

        // Crest
        out->CrestY = gcnew array<int>((int)r.crestY.size());
        for (size_t i = 0; i < r.crestY.size(); ++i) out->CrestY[(int)i] = r.crestY[i];

        // Pitch px/mm
        out->MeanPitchPx = r.meanPitchPx;
        out->StdPitchPx = r.stdPitchPx;
        out->MedianPitchPx = r.medianPitchPx;
        out->MinPitchPx = r.minPitchPx;
        out->MaxPitchPx = r.maxPitchPx;

        out->MeanPitchMm = r.meanPitchMm;
        out->MedianPitchMm = r.medianPitchMm;
        out->MinPitchMm = r.minPitchMm;
        out->MaxPitchMm = r.maxPitchMm;

        // Segment
        out->SegmentTopY = r.segmentTopY;
        out->SegmentBottomY = r.segmentBottomY;
        out->SegmentHeightPx = r.segmentHeightPx;
        out->SegmentHeightMm = r.segmentHeightMm;

        out->ThreadCount = r.threadCount;
        out->AngleDeg = r.angleDeg;

        // Debug image -> byte[]
        const cv::Mat& dbg = r.debugBGR; // CV_8UC3
        if (!dbg.empty()) {
            out->DebugWidth = dbg.cols;
            out->DebugHeight = dbg.rows;
            out->DebugStride = (int)dbg.step;
            out->DebugChannels = dbg.channels();

            int bytes = out->DebugStride * out->DebugHeight;
            out->DebugBgr = gcnew array<Byte>(bytes);
            // Copy raw
            Marshal::Copy(IntPtr((void*)dbg.data), out->DebugBgr, 0, bytes);
        }
        else {
            out->DebugWidth = out->DebugHeight = out->DebugStride = out->DebugChannels = 0;
            out->DebugBgr = gcnew array<Byte>(0);
        }

        return out;
    }

    void ThreadPitch::SetInputMat(IntPtr data, int width, int height, int stride, int channels)
    {
        if (data == IntPtr::Zero) throw gcnew ArgumentNullException("data");
        if (!(channels == 1 || channels == 3)) throw gcnew ArgumentException("channels must be 1 or 3");

        int type = (channels == 1) ? CV_8UC1 : CV_8UC3;
        cv::Mat src(height, width, type, data.ToPointer(), stride);
        cv::Mat bgr;
        if (channels == 1) cv::cvtColor(src, bgr, cv::COLOR_GRAY2BGR);
        else bgr = src.clone();

        BeeCpp::SetInputImage(bgr);
    }

    ThreadPitchResult^ ThreadPitch::Measure(double mmPerPx,
        int bandHalfWidth,
        int expectedMinPitchPx,
        bool useGabor)
    {
        BeeCpp::PitchResult r = BeeCpp::MeasureThreadPitch(mmPerPx, bandHalfWidth, expectedMinPitchPx, useGabor);
        return ConvertResult(r);
    }

} // namespace BeeVision
