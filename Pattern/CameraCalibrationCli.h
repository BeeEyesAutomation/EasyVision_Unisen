#pragma once

#include "CameraCalibrationCore.h"

using namespace System;

namespace BeeCppCli {

public enum class CalibrationPatternTypeCli {
    Chessboard = 0,
    SymmetricCircleGrid = 1,
    AsymmetricCircleGrid = 2
};

public enum class CalibrationGuidanceActionCli {
    Unknown = 0,
    Ok = 1,
    TargetNotFound = 2,
    RotateLeft = 3,
    RotateRight = 4,
    TiltUp = 5,
    TiltDown = 6,
    MoveCloser = 7,
    MoveFarther = 8,
    NotPerpendicular = 9
};

public value struct Point2dCli {
    double X;
    double Y;
};

public value struct Point3dCli {
    double X;
    double Y;
    double Z;
};

public ref class CalibrationGridSpecCli sealed {
public:
    CalibrationPatternTypeCli PatternType;
    int Rows;
    int Columns;
    double SpacingMm;
    bool UseFastCheck;
};

public ref class CalibrationFrameCli sealed {
public:
    bool Found;
    int ImageWidth;
    int ImageHeight;
    array<Point2dCli>^ ImagePoints;
    array<Point3dCli>^ ObjectPoints;
    double CoverageX;
    double CoverageY;
    double CoverageWidth;
    double CoverageHeight;
    double QualityScore;
    String^ Message;
    IntPtr DebugPtr;
    int DebugW;
    int DebugH;
    int DebugStride;
    int DebugChannels;
};

public ref class CameraCalibrationProfileCli sealed {
public:
    bool Success;
    int ImageWidth;
    int ImageHeight;
    array<double>^ CameraMatrix;
    array<double>^ DistortionCoefficients;
    array<double>^ RectificationHomography;
    double MeanReprojectionError;
    array<double>^ PerFrameErrors;
    array<Point2dCli>^ ResidualVectors;
    String^ Message;
};

public ref class ScaleCalibrationCli sealed {
public:
    bool Found;
    double RealSizeMm;
    double PixelSize;
    double MmPerPixel;
    double Confidence;
    int SampleX;
    int SampleY;
    int SampleWidth;
    int SampleHeight;
    String^ Message;
    IntPtr DebugPtr;
    int DebugW;
    int DebugH;
    int DebugStride;
    int DebugChannels;
};

public ref class LiveGuidanceCli sealed {
public:
    bool Found;
    CalibrationGuidanceActionCli Action;
    double Score;
    double TiltXDeg;
    double TiltYDeg;
    double RotationDeg;
    int MessageCode;
    String^ Message;
    IntPtr DebugPtr;
    int DebugW;
    int DebugH;
    int DebugStride;
    int DebugChannels;
};

public ref class CameraCalibration sealed {
public:
    static CalibrationFrameCli^ DetectGrid(
        IntPtr imageData,
        int width,
        int height,
        int stride,
        int channels,
        CalibrationGridSpecCli^ spec);
    static CameraCalibrationProfileCli^ Solve(
        array<CalibrationFrameCli^>^ frames,
        CalibrationGridSpecCli^ spec,
        bool buildRectificationHomography);
    static bool UndistortPreview(
        IntPtr inputData,
        int width,
        int height,
        int stride,
        int channels,
        CameraCalibrationProfileCli^ profile,
        IntPtr outputData,
        int outputStride,
        int outputChannels);
    static ScaleCalibrationCli^ DetectScaleSample(
        IntPtr imageData,
        int width,
        int height,
        int stride,
        int channels,
        double realSizeMm);
    static LiveGuidanceCli^ AnalyzeLiveGuidance(
        IntPtr imageData,
        int width,
        int height,
        int stride,
        int channels,
        CalibrationGridSpecCli^ spec,
        CameraCalibrationProfileCli^ profile);
    static void FreeBuffer(IntPtr pointer);
};

} // namespace BeeCppCli
