#include "CameraCalibrationCli.h"

#include <cstring>

using namespace BeeCppCli;
using namespace System::Runtime::InteropServices;

namespace {

std::string ToStdString(String^ value)
{
    if (value == nullptr)
        return std::string();

    IntPtr ansi = Marshal::StringToHGlobalAnsi(value);
    try
    {
        const char* text = static_cast<const char*>(ansi.ToPointer());
        return text == nullptr ? std::string() : std::string(text);
    }
    finally
    {
        Marshal::FreeHGlobal(ansi);
    }
}

BeeCpp::CalibrationPatternType ToNativePattern(CalibrationPatternTypeCli pattern)
{
    switch (pattern)
    {
    case CalibrationPatternTypeCli::SymmetricCircleGrid:
        return BeeCpp::CalibrationPatternType::SymmetricCircleGrid;
    case CalibrationPatternTypeCli::AsymmetricCircleGrid:
        return BeeCpp::CalibrationPatternType::AsymmetricCircleGrid;
    default:
        return BeeCpp::CalibrationPatternType::Chessboard;
    }
}

CalibrationGuidanceActionCli ToCliGuidance(BeeCpp::CalibrationGuidanceAction action)
{
    switch (action)
    {
    case BeeCpp::CalibrationGuidanceAction::Ok: return CalibrationGuidanceActionCli::Ok;
    case BeeCpp::CalibrationGuidanceAction::TargetNotFound: return CalibrationGuidanceActionCli::TargetNotFound;
    case BeeCpp::CalibrationGuidanceAction::RotateLeft: return CalibrationGuidanceActionCli::RotateLeft;
    case BeeCpp::CalibrationGuidanceAction::RotateRight: return CalibrationGuidanceActionCli::RotateRight;
    case BeeCpp::CalibrationGuidanceAction::TiltUp: return CalibrationGuidanceActionCli::TiltUp;
    case BeeCpp::CalibrationGuidanceAction::TiltDown: return CalibrationGuidanceActionCli::TiltDown;
    case BeeCpp::CalibrationGuidanceAction::MoveCloser: return CalibrationGuidanceActionCli::MoveCloser;
    case BeeCpp::CalibrationGuidanceAction::MoveFarther: return CalibrationGuidanceActionCli::MoveFarther;
    case BeeCpp::CalibrationGuidanceAction::NotPerpendicular: return CalibrationGuidanceActionCli::NotPerpendicular;
    default: return CalibrationGuidanceActionCli::Unknown;
    }
}

BeeCpp::CalibrationGridSpec ToNativeSpec(CalibrationGridSpecCli^ spec)
{
    BeeCpp::CalibrationGridSpec nativeSpec;
    if (spec == nullptr)
        return nativeSpec;

    nativeSpec.patternType = ToNativePattern(spec->PatternType);
    nativeSpec.rows = spec->Rows;
    nativeSpec.columns = spec->Columns;
    nativeSpec.spacingMm = spec->SpacingMm;
    nativeSpec.useFastCheck = spec->UseFastCheck;
    return nativeSpec;
}

cv::Mat BuildImageView(IntPtr data, int width, int height, int stride, int channels)
{
    if (data == IntPtr::Zero || width <= 0 || height <= 0 || stride <= 0)
        return cv::Mat();

    int type = CV_8UC1;
    if (channels == 3)
        type = CV_8UC3;
    else if (channels == 4)
        type = CV_8UC4;
    else if (channels != 1)
        return cv::Mat();

    const int minStride = width * channels;
    if (stride < minStride)
        return cv::Mat();

    return cv::Mat(height, width, type, data.ToPointer(), static_cast<size_t>(stride));
}

void CopyDebugImage(const cv::Mat& debugImage, IntPtr% debugPtr, int% debugW, int% debugH, int% debugStride, int% debugChannels)
{
    if (debugImage.empty())
    {
        debugPtr = IntPtr::Zero;
        debugW = 0;
        debugH = 0;
        debugStride = 0;
        debugChannels = 0;
        return;
    }

    const int byteCount = static_cast<int>(debugImage.step * debugImage.rows);
    unsigned char* buffer = static_cast<unsigned char*>(::operator new[](byteCount));
    std::memcpy(buffer, debugImage.data, byteCount);
    debugPtr = IntPtr(buffer);
    debugW = debugImage.cols;
    debugH = debugImage.rows;
    debugStride = static_cast<int>(debugImage.step);
    debugChannels = debugImage.channels();
}

array<Point2dCli>^ ToPoint2Array(const std::vector<cv::Point2f>& points)
{
    auto managed = gcnew array<Point2dCli>(static_cast<int>(points.size()));
    for (int index = 0; index < managed->Length; ++index)
    {
        Point2dCli point;
        point.X = points[static_cast<size_t>(index)].x;
        point.Y = points[static_cast<size_t>(index)].y;
        managed[index] = point;
    }
    return managed;
}

array<Point3dCli>^ ToPoint3Array(const std::vector<cv::Point3f>& points)
{
    auto managed = gcnew array<Point3dCli>(static_cast<int>(points.size()));
    for (int index = 0; index < managed->Length; ++index)
    {
        Point3dCli point;
        point.X = points[static_cast<size_t>(index)].x;
        point.Y = points[static_cast<size_t>(index)].y;
        point.Z = points[static_cast<size_t>(index)].z;
        managed[index] = point;
    }
    return managed;
}

array<double>^ ToDoubleArray(const std::vector<double>& values)
{
    auto managed = gcnew array<double>(static_cast<int>(values.size()));
    for (int index = 0; index < managed->Length; ++index)
        managed[index] = values[static_cast<size_t>(index)];
    return managed;
}

std::vector<cv::Point2f> ToNativePoint2Array(array<Point2dCli>^ points)
{
    std::vector<cv::Point2f> nativePoints;
    if (points == nullptr)
        return nativePoints;

    nativePoints.reserve(points->Length);
    for (int index = 0; index < points->Length; ++index)
        nativePoints.emplace_back(static_cast<float>(points[index].X), static_cast<float>(points[index].Y));
    return nativePoints;
}

std::vector<cv::Point3f> ToNativePoint3Array(array<Point3dCli>^ points)
{
    std::vector<cv::Point3f> nativePoints;
    if (points == nullptr)
        return nativePoints;

    nativePoints.reserve(points->Length);
    for (int index = 0; index < points->Length; ++index)
        nativePoints.emplace_back(
            static_cast<float>(points[index].X),
            static_cast<float>(points[index].Y),
            static_cast<float>(points[index].Z));
    return nativePoints;
}

std::vector<double> ToNativeDoubleArray(array<double>^ values)
{
    std::vector<double> nativeValues;
    if (values == nullptr)
        return nativeValues;

    nativeValues.reserve(values->Length);
    for (int index = 0; index < values->Length; ++index)
        nativeValues.push_back(values[index]);
    return nativeValues;
}

BeeCpp::CameraCalibrationProfileCore ToNativeProfile(CameraCalibrationProfileCli^ profile)
{
    BeeCpp::CameraCalibrationProfileCore nativeProfile;
    if (profile == nullptr)
        return nativeProfile;

    nativeProfile.success = profile->Success;
    nativeProfile.imageWidth = profile->ImageWidth;
    nativeProfile.imageHeight = profile->ImageHeight;
    nativeProfile.cameraMatrix = ToNativeDoubleArray(profile->CameraMatrix);
    nativeProfile.distortionCoefficients = ToNativeDoubleArray(profile->DistortionCoefficients);
    nativeProfile.rectificationHomography = ToNativeDoubleArray(profile->RectificationHomography);
    nativeProfile.meanReprojectionError = profile->MeanReprojectionError;
    nativeProfile.perFrameErrors = ToNativeDoubleArray(profile->PerFrameErrors);
    nativeProfile.residualVectors = ToNativePoint2Array(profile->ResidualVectors);
    if (profile->Message != nullptr)
        nativeProfile.message = ToStdString(profile->Message);
    return nativeProfile;
}

CalibrationFrameCli^ ToCliFrame(const BeeCpp::CalibrationFrameCore& frame)
{
    auto managed = gcnew CalibrationFrameCli();
    managed->Found = frame.found;
    managed->ImageWidth = frame.imageWidth;
    managed->ImageHeight = frame.imageHeight;
    managed->ImagePoints = ToPoint2Array(frame.imagePoints);
    managed->ObjectPoints = ToPoint3Array(frame.objectPoints);
    managed->CoverageX = frame.coverageRect.x;
    managed->CoverageY = frame.coverageRect.y;
    managed->CoverageWidth = frame.coverageRect.width;
    managed->CoverageHeight = frame.coverageRect.height;
    managed->QualityScore = frame.qualityScore;
    managed->Message = gcnew String(frame.message.c_str());
    CopyDebugImage(frame.debugBGR, managed->DebugPtr, managed->DebugW, managed->DebugH, managed->DebugStride, managed->DebugChannels);
    return managed;
}

CameraCalibrationProfileCli^ ToCliProfile(const BeeCpp::CameraCalibrationProfileCore& profile)
{
    auto managed = gcnew CameraCalibrationProfileCli();
    managed->Success = profile.success;
    managed->ImageWidth = profile.imageWidth;
    managed->ImageHeight = profile.imageHeight;
    managed->CameraMatrix = ToDoubleArray(profile.cameraMatrix);
    managed->DistortionCoefficients = ToDoubleArray(profile.distortionCoefficients);
    managed->RectificationHomography = ToDoubleArray(profile.rectificationHomography);
    managed->MeanReprojectionError = profile.meanReprojectionError;
    managed->PerFrameErrors = ToDoubleArray(profile.perFrameErrors);
    managed->ResidualVectors = ToPoint2Array(profile.residualVectors);
    managed->Message = gcnew String(profile.message.c_str());
    return managed;
}

ScaleCalibrationCli^ ToCliScale(const BeeCpp::ScaleCalibrationCore& scale)
{
    auto managed = gcnew ScaleCalibrationCli();
    managed->Found = scale.found;
    managed->RealSizeMm = scale.realSizeMm;
    managed->PixelSize = scale.pixelSize;
    managed->MmPerPixel = scale.mmPerPixel;
    managed->Confidence = scale.confidence;
    managed->SampleX = scale.sampleBox.x;
    managed->SampleY = scale.sampleBox.y;
    managed->SampleWidth = scale.sampleBox.width;
    managed->SampleHeight = scale.sampleBox.height;
    managed->Message = gcnew String(scale.message.c_str());
    CopyDebugImage(scale.debugBGR, managed->DebugPtr, managed->DebugW, managed->DebugH, managed->DebugStride, managed->DebugChannels);
    return managed;
}

LiveGuidanceCli^ ToCliGuidanceResult(const BeeCpp::LiveGuidanceCore& guidance)
{
    auto managed = gcnew LiveGuidanceCli();
    managed->Found = guidance.found;
    managed->Action = ToCliGuidance(guidance.action);
    managed->Score = guidance.score;
    managed->TiltXDeg = guidance.tiltXDeg;
    managed->TiltYDeg = guidance.tiltYDeg;
    managed->RotationDeg = guidance.rotationDeg;
    managed->MessageCode = guidance.messageCode;
    managed->Message = gcnew String(guidance.message.c_str());
    CopyDebugImage(guidance.debugBGR, managed->DebugPtr, managed->DebugW, managed->DebugH, managed->DebugStride, managed->DebugChannels);
    return managed;
}

} // namespace

CalibrationFrameCli^ CameraCalibration::DetectGrid(
    IntPtr imageData,
    int width,
    int height,
    int stride,
    int channels,
    CalibrationGridSpecCli^ spec)
{
    const cv::Mat view = BuildImageView(imageData, width, height, stride, channels);
    BeeCpp::CameraCalibrationCore core;
    return ToCliFrame(core.DetectGrid(view, ToNativeSpec(spec)));
}

CameraCalibrationProfileCli^ CameraCalibration::Solve(
    array<CalibrationFrameCli^>^ frames,
    CalibrationGridSpecCli^ spec,
    bool buildRectificationHomography)
{
    std::vector<BeeCpp::CalibrationFrameCore> nativeFrames;
    if (frames != nullptr)
    {
        nativeFrames.reserve(frames->Length);
        for (int index = 0; index < frames->Length; ++index)
        {
            CalibrationFrameCli^ managedFrame = frames[index];
            if (managedFrame == nullptr)
                continue;

            BeeCpp::CalibrationFrameCore nativeFrame;
            nativeFrame.found = managedFrame->Found;
            nativeFrame.imageWidth = managedFrame->ImageWidth;
            nativeFrame.imageHeight = managedFrame->ImageHeight;
            nativeFrame.imagePoints = ToNativePoint2Array(managedFrame->ImagePoints);
            nativeFrame.objectPoints = ToNativePoint3Array(managedFrame->ObjectPoints);
            nativeFrame.coverageRect = cv::Rect2f(
                static_cast<float>(managedFrame->CoverageX),
                static_cast<float>(managedFrame->CoverageY),
                static_cast<float>(managedFrame->CoverageWidth),
                static_cast<float>(managedFrame->CoverageHeight));
            nativeFrame.qualityScore = managedFrame->QualityScore;
            if (managedFrame->Message != nullptr)
                nativeFrame.message = ToStdString(managedFrame->Message);
            nativeFrames.push_back(nativeFrame);
        }
    }

    BeeCpp::CameraCalibrationCore core;
    return ToCliProfile(core.Solve(nativeFrames, ToNativeSpec(spec), buildRectificationHomography));
}

bool CameraCalibration::UndistortPreview(
    IntPtr inputData,
    int width,
    int height,
    int stride,
    int channels,
    CameraCalibrationProfileCli^ profile,
    IntPtr outputData,
    int outputStride,
    int outputChannels)
{
    const cv::Mat inputView = BuildImageView(inputData, width, height, stride, channels);
    cv::Mat outputView = BuildImageView(outputData, width, height, outputStride, outputChannels);
    if (inputView.empty() || outputView.empty())
        return false;

    BeeCpp::CameraCalibrationCore core;
    cv::Mat corrected;
    if (!core.UndistortPreview(inputView, ToNativeProfile(profile), corrected))
        return false;
    if (corrected.empty() || corrected.size() != outputView.size() || corrected.type() != outputView.type())
        return false;

    corrected.copyTo(outputView);
    return true;
}

ScaleCalibrationCli^ CameraCalibration::DetectScaleSample(
    IntPtr imageData,
    int width,
    int height,
    int stride,
    int channels,
    double realSizeMm)
{
    const cv::Mat view = BuildImageView(imageData, width, height, stride, channels);
    BeeCpp::CameraCalibrationCore core;
    return ToCliScale(core.DetectScaleSample(view, realSizeMm));
}

LiveGuidanceCli^ CameraCalibration::AnalyzeLiveGuidance(
    IntPtr imageData,
    int width,
    int height,
    int stride,
    int channels,
    CalibrationGridSpecCli^ spec,
    CameraCalibrationProfileCli^ profile)
{
    const cv::Mat view = BuildImageView(imageData, width, height, stride, channels);
    BeeCpp::CameraCalibrationCore core;
    const BeeCpp::CameraCalibrationProfileCore nativeProfile = ToNativeProfile(profile);
    const BeeCpp::CameraCalibrationProfileCore* profilePtr = profile == nullptr ? nullptr : &nativeProfile;
    return ToCliGuidanceResult(core.AnalyzeLiveGuidance(view, ToNativeSpec(spec), profilePtr));
}

void CameraCalibration::FreeBuffer(IntPtr pointer)
{
    if (pointer != IntPtr::Zero)
        ::operator delete[](pointer.ToPointer());
}
