#include "CameraCalibrationCore.h"

#include <algorithm>
#include <cmath>
#include <limits>
#include <numeric>

namespace BeeCpp {
namespace {

constexpr double kPi = 3.14159265358979323846;

bool IsValidSpec(const CalibrationGridSpec& spec)
{
    return spec.rows > 1 && spec.columns > 1 && spec.spacingMm > 0.0;
}

cv::Mat ConvertToGray(const cv::Mat& image)
{
    cv::Mat gray;
    if (image.empty())
        return gray;

    if (image.channels() == 1)
    {
        if (image.type() == CV_8UC1)
            image.copyTo(gray);
        else
            image.convertTo(gray, CV_8UC1);
    }
    else if (image.channels() == 3)
    {
        cv::cvtColor(image, gray, cv::COLOR_BGR2GRAY);
    }
    else if (image.channels() == 4)
    {
        cv::cvtColor(image, gray, cv::COLOR_BGRA2GRAY);
    }
    else
    {
        image.convertTo(gray, CV_8UC1);
    }

    return gray;
}

std::vector<cv::Point3f> BuildObjectPoints(const CalibrationGridSpec& spec)
{
    std::vector<cv::Point3f> points;
    if (!IsValidSpec(spec))
        return points;

    points.reserve(static_cast<size_t>(spec.rows) * static_cast<size_t>(spec.columns));
    for (int row = 0; row < spec.rows; ++row)
    {
        for (int col = 0; col < spec.columns; ++col)
        {
            const double x = (spec.patternType == CalibrationPatternType::AsymmetricCircleGrid)
                ? ((2.0 * static_cast<double>(col) + (row % 2)) * spec.spacingMm)
                : (static_cast<double>(col) * spec.spacingMm);
            const double y = static_cast<double>(row) * spec.spacingMm;
            points.emplace_back(static_cast<float>(x), static_cast<float>(y), 0.0f);
        }
    }
    return points;
}

bool DetectChessboard(
    const cv::Mat& gray,
    const CalibrationGridSpec& spec,
    std::vector<cv::Point2f>& corners)
{
    int flags = cv::CALIB_CB_ADAPTIVE_THRESH | cv::CALIB_CB_NORMALIZE_IMAGE;
    if (spec.useFastCheck)
        flags |= cv::CALIB_CB_FAST_CHECK;

    const cv::Size patternSize(spec.columns, spec.rows);
    const bool found = cv::findChessboardCorners(gray, patternSize, corners, flags);
    if (!found)
        return false;

    cv::cornerSubPix(
        gray,
        corners,
        cv::Size(11, 11),
        cv::Size(-1, -1),
        cv::TermCriteria(
            cv::TermCriteria::EPS | cv::TermCriteria::COUNT,
            30,
            0.01));
    return true;
}

bool DetectCircleGrid(
    const cv::Mat& gray,
    const CalibrationGridSpec& spec,
    std::vector<cv::Point2f>& centers)
{
    const cv::Size patternSize(spec.columns, spec.rows);
    int flags = (spec.patternType == CalibrationPatternType::AsymmetricCircleGrid)
        ? cv::CALIB_CB_ASYMMETRIC_GRID
        : cv::CALIB_CB_SYMMETRIC_GRID;
    return cv::findCirclesGrid(gray, patternSize, centers, flags);
}

cv::Rect2f ScoreGridCoverage(
    const std::vector<cv::Point2f>& imagePoints,
    const cv::Size& imageSize,
    double& qualityScore)
{
    qualityScore = 0.0;
    if (imagePoints.empty() || imageSize.width <= 0 || imageSize.height <= 0)
        return cv::Rect2f();

    float minX = std::numeric_limits<float>::max();
    float minY = std::numeric_limits<float>::max();
    float maxX = std::numeric_limits<float>::lowest();
    float maxY = std::numeric_limits<float>::lowest();
    for (const auto& point : imagePoints)
    {
        minX = std::min(minX, point.x);
        minY = std::min(minY, point.y);
        maxX = std::max(maxX, point.x);
        maxY = std::max(maxY, point.y);
    }

    const cv::Rect2f coverage(minX, minY, std::max(0.0f, maxX - minX), std::max(0.0f, maxY - minY));
    const double imageArea = static_cast<double>(imageSize.width) * static_cast<double>(imageSize.height);
    const double coverageArea = static_cast<double>(coverage.width) * static_cast<double>(coverage.height);
    const double areaScore = imageArea > 0.0 ? std::min(1.0, coverageArea / imageArea) : 0.0;

    const cv::Point2f center(coverage.x + coverage.width * 0.5f, coverage.y + coverage.height * 0.5f);
    const cv::Point2f imageCenter(imageSize.width * 0.5f, imageSize.height * 0.5f);
    const double dx = static_cast<double>(center.x - imageCenter.x);
    const double dy = static_cast<double>(center.y - imageCenter.y);
    const double maxDistance = std::sqrt(
        static_cast<double>(imageSize.width) * static_cast<double>(imageSize.width) +
        static_cast<double>(imageSize.height) * static_cast<double>(imageSize.height)) * 0.5;
    const double centerPenalty = maxDistance > 0.0 ? std::min(1.0, std::sqrt(dx * dx + dy * dy) / maxDistance) : 0.0;

    qualityScore = std::max(0.0, std::min(1.0, areaScore * 0.8 + (1.0 - centerPenalty) * 0.2));
    return coverage;
}

cv::Mat DrawGridDebug(
    const cv::Mat& gray,
    const CalibrationGridSpec& spec,
    const std::vector<cv::Point2f>& points,
    bool found,
    const cv::Rect2f& coverage,
    double qualityScore)
{
    cv::Mat debug;
    if (gray.empty())
        return debug;

    cv::cvtColor(gray, debug, cv::COLOR_GRAY2BGR);
    if (!points.empty())
    {
        cv::drawChessboardCorners(debug, cv::Size(spec.columns, spec.rows), points, found);
        cv::rectangle(debug, coverage, found ? cv::Scalar(0, 255, 0) : cv::Scalar(0, 0, 255), 2);
    }

    const std::string label = found
        ? ("Found score=" + cv::format("%.3f", qualityScore))
        : "Target not found";
    cv::putText(debug, label, cv::Point(12, 28), cv::FONT_HERSHEY_SIMPLEX, 0.7, cv::Scalar(0, 255, 255), 2);
    return debug;
}

std::vector<double> FlattenMat64(const cv::Mat& mat)
{
    std::vector<double> values;
    if (mat.empty())
        return values;

    cv::Mat temp;
    mat.convertTo(temp, CV_64F);
    values.reserve(static_cast<size_t>(temp.rows) * static_cast<size_t>(temp.cols));
    for (int row = 0; row < temp.rows; ++row)
    {
        const double* ptr = temp.ptr<double>(row);
        for (int col = 0; col < temp.cols; ++col)
            values.push_back(ptr[col]);
    }
    return values;
}

cv::Mat VectorToCameraMatrix(const std::vector<double>& values)
{
    if (values.size() < 9)
        return cv::Mat();

    cv::Mat matrix(3, 3, CV_64F);
    for (int index = 0; index < 9; ++index)
        matrix.at<double>(index / 3, index % 3) = values[static_cast<size_t>(index)];
    return matrix;
}

cv::Mat VectorToDistortion(const std::vector<double>& values)
{
    if (values.empty())
        return cv::Mat();

    cv::Mat distortion(static_cast<int>(values.size()), 1, CV_64F);
    for (int index = 0; index < distortion.rows; ++index)
        distortion.at<double>(index, 0) = values[static_cast<size_t>(index)];
    return distortion;
}

void ComputeReprojectionErrors(
    const std::vector<std::vector<cv::Point3f>>& objectPoints,
    const std::vector<std::vector<cv::Point2f>>& imagePoints,
    const std::vector<cv::Mat>& rvecs,
    const std::vector<cv::Mat>& tvecs,
    const cv::Mat& cameraMatrix,
    const cv::Mat& distortionCoefficients,
    std::vector<double>& perFrameErrors,
    std::vector<cv::Point2f>& residualVectors,
    double& meanError)
{
    perFrameErrors.clear();
    residualVectors.clear();
    meanError = 0.0;

    double totalSquaredError = 0.0;
    size_t totalPoints = 0;
    for (size_t index = 0; index < imagePoints.size(); ++index)
    {
        std::vector<cv::Point2f> reprojected;
        cv::projectPoints(
            objectPoints[index],
            rvecs[index],
            tvecs[index],
            cameraMatrix,
            distortionCoefficients,
            reprojected);

        double frameSquaredError = 0.0;
        const size_t pointCount = std::min(imagePoints[index].size(), reprojected.size());
        for (size_t pointIndex = 0; pointIndex < pointCount; ++pointIndex)
        {
            const cv::Point2f residual = imagePoints[index][pointIndex] - reprojected[pointIndex];
            residualVectors.push_back(residual);
            frameSquaredError += static_cast<double>(residual.dot(residual));
        }

        const double frameError = pointCount > 0
            ? std::sqrt(frameSquaredError / static_cast<double>(pointCount))
            : 0.0;
        perFrameErrors.push_back(frameError);
        totalSquaredError += frameSquaredError;
        totalPoints += pointCount;
    }

    meanError = totalPoints > 0
        ? std::sqrt(totalSquaredError / static_cast<double>(totalPoints))
        : 0.0;
}

std::vector<double> BuildRectificationHomography(
    const CalibrationGridSpec& spec,
    const std::vector<cv::Point2f>& imagePoints)
{
    if (imagePoints.size() != static_cast<size_t>(spec.rows * spec.columns))
        return {};

    std::vector<cv::Point2f> planePoints;
    planePoints.reserve(imagePoints.size());
    for (int row = 0; row < spec.rows; ++row)
    {
        for (int col = 0; col < spec.columns; ++col)
        {
            const double x = (spec.patternType == CalibrationPatternType::AsymmetricCircleGrid)
                ? ((2.0 * static_cast<double>(col) + (row % 2)) * spec.spacingMm)
                : (static_cast<double>(col) * spec.spacingMm);
            const double y = static_cast<double>(row) * spec.spacingMm;
            planePoints.emplace_back(static_cast<float>(x), static_cast<float>(y));
        }
    }

    const cv::Mat homography = cv::findHomography(imagePoints, planePoints, 0);
    return FlattenMat64(homography);
}

double NormalizeAngleDeg(double angleDeg)
{
    while (angleDeg > 180.0) angleDeg -= 360.0;
    while (angleDeg < -180.0) angleDeg += 360.0;
    return angleDeg;
}

void EstimateEulerDeg(
    const cv::Mat& rotationMatrix,
    double& tiltXDeg,
    double& tiltYDeg,
    double& rotationDeg)
{
    const double r00 = rotationMatrix.at<double>(0, 0);
    const double r01 = rotationMatrix.at<double>(0, 1);
    const double r10 = rotationMatrix.at<double>(1, 0);
    const double r11 = rotationMatrix.at<double>(1, 1);
    const double r20 = rotationMatrix.at<double>(2, 0);
    const double r21 = rotationMatrix.at<double>(2, 1);
    const double r22 = rotationMatrix.at<double>(2, 2);

    tiltYDeg = std::atan2(-r20, std::sqrt(r21 * r21 + r22 * r22)) * 180.0 / kPi;
    tiltXDeg = std::atan2(r21, r22) * 180.0 / kPi;
    rotationDeg = std::atan2(r10, r00) * 180.0 / kPi;
    rotationDeg = NormalizeAngleDeg(rotationDeg);
}

CalibrationGuidanceAction ClassifyGuidanceAction(
    double tiltXDeg,
    double tiltYDeg,
    double rotationDeg,
    double coverageScore)
{
    const double absRotation = std::abs(rotationDeg);
    const double absTiltX = std::abs(tiltXDeg);
    const double absTiltY = std::abs(tiltYDeg);

    if (absRotation >= 12.0)
        return rotationDeg > 0.0 ? CalibrationGuidanceAction::RotateLeft : CalibrationGuidanceAction::RotateRight;
    if (absTiltX >= 10.0)
        return tiltXDeg > 0.0 ? CalibrationGuidanceAction::TiltDown : CalibrationGuidanceAction::TiltUp;
    if (absTiltY >= 10.0)
        return CalibrationGuidanceAction::NotPerpendicular;
    if (coverageScore < 0.12)
        return CalibrationGuidanceAction::MoveCloser;
    if (coverageScore > 0.70)
        return CalibrationGuidanceAction::MoveFarther;
    return CalibrationGuidanceAction::Ok;
}

std::string GuidanceMessage(CalibrationGuidanceAction action)
{
    switch (action)
    {
    case CalibrationGuidanceAction::Ok: return "OK";
    case CalibrationGuidanceAction::TargetNotFound: return "Target not found";
    case CalibrationGuidanceAction::RotateLeft: return "Rotate left";
    case CalibrationGuidanceAction::RotateRight: return "Rotate right";
    case CalibrationGuidanceAction::TiltUp: return "Tilt up";
    case CalibrationGuidanceAction::TiltDown: return "Tilt down";
    case CalibrationGuidanceAction::MoveCloser: return "Move closer";
    case CalibrationGuidanceAction::MoveFarther: return "Move farther";
    case CalibrationGuidanceAction::NotPerpendicular: return "Camera not perpendicular";
    default: return "Unknown";
    }
}

double ScoreCandidateContour(const std::vector<cv::Point>& contour, const cv::Size& imageSize)
{
    const double area = std::abs(cv::contourArea(contour));
    if (area <= 0.0)
        return 0.0;

    const double perimeter = cv::arcLength(contour, true);
    if (perimeter <= 0.0)
        return 0.0;

    const cv::Rect box = cv::boundingRect(contour);
    const double boxArea = static_cast<double>(box.area());
    const double fill = boxArea > 0.0 ? area / boxArea : 0.0;
    const double circularity = 4.0 * kPi * area / (perimeter * perimeter);
    const double normalizedArea = std::min(
        1.0,
        area / std::max(1.0, static_cast<double>(imageSize.width) * static_cast<double>(imageSize.height) * 0.20));
    return std::max(0.0, circularity * 0.5 + fill * 0.35 + normalizedArea * 0.15);
}

ScaleCalibrationCore DetectKnownScaleSample(const cv::Mat& image, double realSizeMm)
{
    ScaleCalibrationCore result;
    result.realSizeMm = realSizeMm;
    if (image.empty())
    {
        result.message = "Input image is empty.";
        return result;
    }
    if (realSizeMm <= 0.0)
    {
        result.message = "Real sample size must be positive.";
        return result;
    }

    const cv::Mat gray = ConvertToGray(image);
    cv::Mat blurred;
    cv::GaussianBlur(gray, blurred, cv::Size(5, 5), 0.0);

    cv::Mat thresholdLight;
    cv::Mat thresholdDark;
    cv::threshold(blurred, thresholdLight, 0, 255, cv::THRESH_BINARY | cv::THRESH_OTSU);
    cv::threshold(blurred, thresholdDark, 0, 255, cv::THRESH_BINARY_INV | cv::THRESH_OTSU);

    auto buildCandidates = [&](const cv::Mat& mask, std::vector<std::pair<double, cv::Rect>>& candidates, cv::Mat& debugMask)
    {
        debugMask = mask.clone();
        cv::morphologyEx(
            debugMask,
            debugMask,
            cv::MORPH_OPEN,
            cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(5, 5)));
        cv::morphologyEx(
            debugMask,
            debugMask,
            cv::MORPH_CLOSE,
            cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(7, 7)));

        std::vector<std::vector<cv::Point>> contours;
        cv::findContours(debugMask, contours, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);
        for (const auto& contour : contours)
        {
            const double area = std::abs(cv::contourArea(contour));
            if (area < 100.0)
                continue;

            const cv::Rect box = cv::boundingRect(contour);
            if (box.width < 8 || box.height < 8)
                continue;

            const double boxArea = static_cast<double>(box.area());
            if (boxArea <= 0.0)
                continue;

            const double imageArea = static_cast<double>(gray.cols) * static_cast<double>(gray.rows);
            if (boxArea > imageArea * 0.60)
                continue;

            const double score = ScoreCandidateContour(contour, gray.size());
            if (score >= 0.30)
                candidates.emplace_back(score, box);
        }
    };

    std::vector<std::pair<double, cv::Rect>> candidates;
    cv::Mat maskLight;
    cv::Mat maskDark;
    buildCandidates(thresholdLight, candidates, maskLight);
    buildCandidates(thresholdDark, candidates, maskDark);

    cv::Mat debug;
    cv::cvtColor(gray, debug, cv::COLOR_GRAY2BGR);

    if (candidates.empty())
    {
        result.debugBGR = debug;
        result.message = "Known scale sample not found.";
        return result;
    }

    std::stable_sort(
        candidates.begin(),
        candidates.end(),
        [](const auto& left, const auto& right)
        {
            if (left.first == right.first)
                return left.second.area() > right.second.area();
            return left.first > right.first;
        });

    if (candidates.size() > 1)
    {
        const double topScore = candidates[0].first;
        const double secondScore = candidates[1].first;
        if (secondScore >= topScore * 0.92)
        {
            result.debugBGR = debug;
            result.message = "Scale sample ambiguous.";
            return result;
        }
    }

    result.found = true;
    result.sampleBox = candidates[0].second;
    result.pixelSize = static_cast<double>(std::max(result.sampleBox.width, result.sampleBox.height));
    result.mmPerPixel = realSizeMm / std::max(1.0, result.pixelSize);
    result.confidence = std::min(1.0, std::max(0.0, candidates[0].first));
    result.message = "OK";

    cv::rectangle(debug, result.sampleBox, cv::Scalar(0, 255, 0), 2);
    const std::string label =
        "mm/px=" + cv::format("%.6f", result.mmPerPixel) +
        " conf=" + cv::format("%.3f", result.confidence);
    cv::putText(debug, label, cv::Point(12, 28), cv::FONT_HERSHEY_SIMPLEX, 0.7, cv::Scalar(0, 255, 255), 2);
    result.debugBGR = debug;
    return result;
}

} // namespace

CalibrationFrameCore CameraCalibrationCore::DetectGrid(const cv::Mat& image, const CalibrationGridSpec& spec) const
{
    CalibrationFrameCore result;
    result.imageWidth = image.cols;
    result.imageHeight = image.rows;

    if (image.empty())
    {
        result.message = "Input image is empty.";
        return result;
    }
    if (!IsValidSpec(spec))
    {
        result.message = "Invalid calibration grid spec.";
        result.debugBGR = DrawGridDebug(ConvertToGray(image), spec, {}, false, cv::Rect2f(), 0.0);
        return result;
    }

    const cv::Mat gray = ConvertToGray(image);
    bool found = false;
    if (spec.patternType == CalibrationPatternType::Chessboard)
        found = DetectChessboard(gray, spec, result.imagePoints);
    else
        found = DetectCircleGrid(gray, spec, result.imagePoints);

    result.found = found;
    result.objectPoints = found ? BuildObjectPoints(spec) : std::vector<cv::Point3f>();
    result.coverageRect = ScoreGridCoverage(result.imagePoints, gray.size(), result.qualityScore);
    result.message = found ? "OK" : "Calibration target not found.";
    result.debugBGR = DrawGridDebug(gray, spec, result.imagePoints, found, result.coverageRect, result.qualityScore);
    return result;
}

CameraCalibrationProfileCore CameraCalibrationCore::Solve(
    const std::vector<CalibrationFrameCore>& frames,
    const CalibrationGridSpec& spec,
    bool buildRectificationHomography) const
{
    CameraCalibrationProfileCore result;
    if (!IsValidSpec(spec))
    {
        result.message = "Invalid calibration grid spec.";
        return result;
    }

    std::vector<std::vector<cv::Point3f>> objectPoints;
    std::vector<std::vector<cv::Point2f>> imagePoints;
    cv::Size imageSize;
    for (const auto& frame : frames)
    {
        if (!frame.found || frame.imagePoints.empty() || frame.objectPoints.empty())
            continue;

        if (imageSize.width == 0 || imageSize.height == 0)
            imageSize = cv::Size(frame.imageWidth, frame.imageHeight);
        if (frame.imageWidth != imageSize.width || frame.imageHeight != imageSize.height)
            continue;

        objectPoints.push_back(frame.objectPoints);
        imagePoints.push_back(frame.imagePoints);
    }

    if (imagePoints.size() < 3)
    {
        result.message = "At least 3 accepted frames are required.";
        return result;
    }

    cv::Mat cameraMatrix = cv::Mat::eye(3, 3, CV_64F);
    cv::Mat distortionCoefficients = cv::Mat::zeros(8, 1, CV_64F);
    std::vector<cv::Mat> rvecs;
    std::vector<cv::Mat> tvecs;

    cv::calibrateCamera(
        objectPoints,
        imagePoints,
        imageSize,
        cameraMatrix,
        distortionCoefficients,
        rvecs,
        tvecs,
        0);

    result.success = true;
    result.imageWidth = imageSize.width;
    result.imageHeight = imageSize.height;
    result.cameraMatrix = FlattenMat64(cameraMatrix);
    result.distortionCoefficients = FlattenMat64(distortionCoefficients);
    ComputeReprojectionErrors(
        objectPoints,
        imagePoints,
        rvecs,
        tvecs,
        cameraMatrix,
        distortionCoefficients,
        result.perFrameErrors,
        result.residualVectors,
        result.meanReprojectionError);

    if (buildRectificationHomography && !imagePoints.empty())
        result.rectificationHomography = BuildRectificationHomography(spec, imagePoints.front());

    result.message = "OK";
    return result;
}

bool CameraCalibrationCore::UndistortPreview(
    const cv::Mat& source,
    const CameraCalibrationProfileCore& profile,
    cv::Mat& destination) const
{
    destination.release();
    if (source.empty() || profile.cameraMatrix.size() < 9 || profile.distortionCoefficients.empty())
        return false;

    const cv::Mat cameraMatrix = VectorToCameraMatrix(profile.cameraMatrix);
    const cv::Mat distortion = VectorToDistortion(profile.distortionCoefficients);
    if (cameraMatrix.empty() || distortion.empty())
        return false;

    cv::Mat map1;
    cv::Mat map2;
    const cv::Size targetSize(
        profile.imageWidth > 0 ? profile.imageWidth : source.cols,
        profile.imageHeight > 0 ? profile.imageHeight : source.rows);
    cv::initUndistortRectifyMap(
        cameraMatrix,
        distortion,
        cv::Mat(),
        cameraMatrix,
        targetSize,
        CV_32FC1,
        map1,
        map2);
    cv::remap(source, destination, map1, map2, cv::INTER_LINEAR, cv::BORDER_CONSTANT);
    return !destination.empty();
}

ScaleCalibrationCore CameraCalibrationCore::DetectScaleSample(const cv::Mat& image, double realSizeMm) const
{
    return DetectKnownScaleSample(image, realSizeMm);
}

LiveGuidanceCore CameraCalibrationCore::AnalyzeLiveGuidance(
    const cv::Mat& image,
    const CalibrationGridSpec& spec,
    const CameraCalibrationProfileCore* profile) const
{
    LiveGuidanceCore result;
    const CalibrationFrameCore frame = DetectGrid(image, spec);
    result.found = frame.found;
    result.debugBGR = frame.debugBGR.clone();

    if (!frame.found)
    {
        result.action = CalibrationGuidanceAction::TargetNotFound;
        result.score = 0.0;
        result.messageCode = 1;
        result.message = GuidanceMessage(result.action);
        return result;
    }

    result.score = frame.qualityScore;
    result.rotationDeg = 0.0;
    result.tiltXDeg = 0.0;
    result.tiltYDeg = 0.0;

    if (profile != nullptr && profile->cameraMatrix.size() >= 9 && !profile->distortionCoefficients.empty())
    {
        const cv::Mat cameraMatrix = VectorToCameraMatrix(profile->cameraMatrix);
        const cv::Mat distortion = VectorToDistortion(profile->distortionCoefficients);
        const std::vector<cv::Point3f> objectPoints = frame.objectPoints.empty()
            ? BuildObjectPoints(spec)
            : frame.objectPoints;
        if (!cameraMatrix.empty() && !distortion.empty() && objectPoints.size() == frame.imagePoints.size())
        {
            cv::Mat rvec;
            cv::Mat tvec;
            if (cv::solvePnP(objectPoints, frame.imagePoints, cameraMatrix, distortion, rvec, tvec))
            {
                cv::Mat rotationMatrix;
                cv::Rodrigues(rvec, rotationMatrix);
                EstimateEulerDeg(rotationMatrix, result.tiltXDeg, result.tiltYDeg, result.rotationDeg);
            }
        }
    }
    else if (frame.imagePoints.size() >= 2)
    {
        const cv::Point2f delta = frame.imagePoints[static_cast<size_t>(spec.columns - 1)] - frame.imagePoints.front();
        result.rotationDeg = std::atan2(delta.y, delta.x) * 180.0 / kPi;
        result.rotationDeg = NormalizeAngleDeg(result.rotationDeg);
    }

    result.action = ClassifyGuidanceAction(
        result.tiltXDeg,
        result.tiltYDeg,
        result.rotationDeg,
        frame.qualityScore);
    result.messageCode = static_cast<int>(result.action);
    result.message = GuidanceMessage(result.action);

    if (!result.debugBGR.empty())
    {
        const std::string label =
            result.message +
            " rx=" + cv::format("%.1f", result.tiltXDeg) +
            " ry=" + cv::format("%.1f", result.tiltYDeg) +
            " rot=" + cv::format("%.1f", result.rotationDeg);
        cv::putText(result.debugBGR, label, cv::Point(12, 54), cv::FONT_HERSHEY_SIMPLEX, 0.6, cv::Scalar(255, 200, 0), 2);
    }

    return result;
}

} // namespace BeeCpp
