#pragma once
#using <System.dll>

using namespace System;

namespace BeeCpp
{
    // Trùng nghĩa với Core
    public enum class MethordEdgeCli {
        CloseEdges = 0,
        StrongEdges = 1,
        Binary = 2,
        InvertBinary = 3
    };

    public value struct EdgePipelineOptionsCli {
        MethordEdgeCli Method;
        double StrongPercentile;
        int    ThresholdBinary;
        int    ClearNoiseSmallArea;
        int    SizeClose;
        int    SizeOpen;
        int    ClearNoiseBigArea;
    };

    // Static utility class
    public ref class FilterCLi  sealed
    {
    public:
        // One-pass pipeline (src/dst là buffer IntPtr)
        static void RunEdgePipeline(
            IntPtr srcData, int width, int height, int type, size_t srcStep,
            EdgePipelineOptionsCli opts,
            IntPtr dstData, int dstType, size_t dstStep);

        // Các hàm lẻ nếu bạn cần dùng riêng
        static void GetStrongEdgesOnly(
            IntPtr srcData, int width, int height, int type, size_t srcStep,
            double percentile,
            IntPtr dstData, int dstType, size_t dstStep);

        static void Edge(
            IntPtr srcData, int width, int height, int type, size_t srcStep,
            IntPtr dstData, int dstType, size_t dstStep);

        static void ThresholdToEdges(
            IntPtr srcData, int width, int height, int type, size_t srcStep,
            int thresh, int thresholdType,
            IntPtr dstData, int dstType, size_t dstStep);

        static void Morphology(
            IntPtr srcData, int width, int height, int type, size_t srcStep,
            int morphType, int kW, int kH, int iterations,
            IntPtr dstData, int dstType, size_t dstStep);

        static void ClearNoise(
            IntPtr edgesData, int width, int height, int type, size_t step,
            int minCompArea,
            IntPtr cleanData, int cleanType, size_t cleanStep);

        static void AutoCannyThresholdFromHistogram(
            IntPtr grayData, int width, int height, int type, size_t step,
            [System::Runtime::InteropServices::Out] int% lower,
            [System::Runtime::InteropServices::Out] int% upper);
    };
} // namespace BeeCppCli
