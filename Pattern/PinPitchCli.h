#pragma once

#include "PinPitchCore.h"
#include <opencv2/opencv.hpp>

using namespace System;

namespace BeeCppCli {

public enum class PinCenterMethodCli {
    Geometry = 0,
    WeightedCentroidFallback = 1
};

public enum class PinArrangeModeCli {
    X = 0,
    Y = 1,
    RowProjection = 2
};

public ref class PinCenterCli sealed {
public:
    int Id;
    double X;
    double Y;
    double Score;
    double AreaPx;
    double WidthPx;
    double HeightPx;
    double FillRatio;
    double AngleDeg;
    PinCenterMethodCli Method;
};

public ref class PinPitchCliResult sealed {
public:
    bool Found;
    int Status;
    System::String^ Message;
    double ScaleMmPerPx;
    array<PinCenterCli^>^ Pins;
    array<double>^ AdjacentPitchMm;
    double SpanP1P4Mm;
    double RowVx;
    double RowVy;
    double RowX0;
    double RowY0;
    double RowResidualPx;
    IntPtr DebugPtr;
    int DebugW;
    int DebugH;
    int DebugStride;
    int DebugChannels;
};

public ref class PinPitchCli sealed {
public:
    PinPitchCli();
    ~PinPitchCli();

    void SetImage(IntPtr data, int width, int height, int stride, int channels);
    void SetOptions(
        int expectedCount,
        PinArrangeModeCli arrangeMode,
        bool useProjectedPitch,
        bool useAutoThreshold,
        int manualThreshold,
        int morphClose,
        int morphOpen,
        double minAreaPx,
        double maxAreaRatio,
        double minAspect,
        double maxAspect,
        double minFillRatio,
        bool useOutlineCenter,
        int outlineThresholdOffset,
        int outlineClose,
        int outlineDilate,
        int outlinePadding,
        int maxOutlineExpand,
        double mmPerPixel,
        bool useTopHat,
        int topHatKernelPx,
        double minSolidity,
        bool reduceDilateForOutline,
        bool useEdgeBoundary,
        int edgeCannyLow,
        int edgeCannyHigh,
        bool useEdgeGeometryCenter,
        bool useGradientRefinement,
        int gradientPatchMargin,
        int gradientThreshold,
        double claheClipLimit,
        int claheTileSize);

    PinPitchCliResult^ Measure();
    static void FreeBuffer(IntPtr p);

private:
    BeeCpp::PinPitchCore* _core;
};

} // namespace BeeCppCli
