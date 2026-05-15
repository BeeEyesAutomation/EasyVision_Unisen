#pragma once

#include <opencv2/opencv.hpp>
#include <vector>

namespace BeeCpp {
namespace VisionGeometry {

double DistancePointToLine(const cv::Point2f& p, const cv::Vec4f& line);
cv::Point2f ProjectPointToLine(const cv::Point2f& p, const cv::Vec4f& line);
std::vector<cv::Point2f> SortAlongAxis(std::vector<cv::Point2f> points, bool horizontal);
double PointDistance(const cv::Point2f& a, const cv::Point2f& b);
double ProjectedDistance(const cv::Point2f& a, const cv::Point2f& b, const cv::Point2f& axis);
cv::Vec4f FitLineOrDefault(const std::vector<cv::Point2f>& points);

} // namespace VisionGeometry
} // namespace BeeCpp

