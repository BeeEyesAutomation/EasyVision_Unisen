#pragma once

#include <opencv2/opencv.hpp>
#include <string>
#include <vector>

namespace BeeCpp {

enum class CalibrationPatternType {
    Chessboard = 0,
    SymmetricCircleGrid = 1,
    AsymmetricCircleGrid = 2
};

enum class CalibrationGuidanceAction {
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

struct CalibrationGridSpec {
    CalibrationPatternType patternType = CalibrationPatternType::Chessboard;
    int rows = 0;
    int columns = 0;
    double spacingMm = 1.0;
    bool useFastCheck = true;
};

struct CalibrationFrameCore {
    bool found = false;
    int imageWidth = 0;
    int imageHeight = 0;
    std::vector<cv::Point2f> imagePoints;
    std::vector<cv::Point3f> objectPoints;
    cv::Rect2f coverageRect;
    double qualityScore = 0.0;
    std::string message;
    cv::Mat debugBGR;
};

struct CameraCalibrationProfileCore {
    bool success = false;
    int imageWidth = 0;
    int imageHeight = 0;
    std::vector<double> cameraMatrix;
    std::vector<double> distortionCoefficients;
    std::vector<double> rectificationHomography;
    double meanReprojectionError = 0.0;
    std::vector<double> perFrameErrors;
    std::vector<cv::Point2f> residualVectors;
    std::string message;
};

struct ScaleCalibrationCore {
    bool found = false;
    double realSizeMm = 0.0;
    double pixelSize = 0.0;
    double mmPerPixel = 0.0;
    double confidence = 0.0;
    cv::Rect sampleBox;
    std::string message;
    cv::Mat debugBGR;
};

struct LiveGuidanceCore {
    bool found = false;
    CalibrationGuidanceAction action = CalibrationGuidanceAction::Unknown;
    double score = 0.0;
    double tiltXDeg = 0.0;
    double tiltYDeg = 0.0;
    double rotationDeg = 0.0;
    int messageCode = 0;
    std::string message;
    cv::Mat debugBGR;
};

class CameraCalibrationCore {
public:
    CalibrationFrameCore DetectGrid(const cv::Mat& image, const CalibrationGridSpec& spec) const;
    CameraCalibrationProfileCore Solve(
        const std::vector<CalibrationFrameCore>& frames,
        const CalibrationGridSpec& spec,
        bool buildRectificationHomography = true) const;
    bool UndistortPreview(
        const cv::Mat& source,
        const CameraCalibrationProfileCore& profile,
        cv::Mat& destination) const;
    ScaleCalibrationCore DetectScaleSample(const cv::Mat& image, double realSizeMm) const;
    LiveGuidanceCore AnalyzeLiveGuidance(
        const cv::Mat& image,
        const CalibrationGridSpec& spec,
        const CameraCalibrationProfileCore* profile = nullptr) const;
};

} // namespace BeeCpp
