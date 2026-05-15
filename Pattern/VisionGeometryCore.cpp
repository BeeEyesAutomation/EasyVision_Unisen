#include "VisionGeometryCore.h"
#include <algorithm>
#include <cmath>

namespace BeeCpp {
namespace VisionGeometry {

double DistancePointToLine(const cv::Point2f& p, const cv::Vec4f& line)
{
    const double vx = line[0];
    const double vy = line[1];
    const double x0 = line[2];
    const double y0 = line[3];
    const double norm = std::sqrt(vx * vx + vy * vy);
    if (norm <= 1e-9)
        return 0.0;

    const double dx = p.x - x0;
    const double dy = p.y - y0;
    return std::abs(dx * vy - dy * vx) / norm;
}

cv::Point2f ProjectPointToLine(const cv::Point2f& p, const cv::Vec4f& line)
{
    const double vx = line[0];
    const double vy = line[1];
    const double x0 = line[2];
    const double y0 = line[3];
    const double norm2 = vx * vx + vy * vy;
    if (norm2 <= 1e-9)
        return cv::Point2f((float)x0, (float)y0);

    const double t = ((p.x - x0) * vx + (p.y - y0) * vy) / norm2;
    return cv::Point2f((float)(x0 + t * vx), (float)(y0 + t * vy));
}

std::vector<cv::Point2f> SortAlongAxis(std::vector<cv::Point2f> points, bool horizontal)
{
    std::stable_sort(points.begin(), points.end(),
        [horizontal](const cv::Point2f& a, const cv::Point2f& b)
        {
            if (horizontal)
            {
                if (a.x == b.x) return a.y < b.y;
                return a.x < b.x;
            }

            if (a.y == b.y) return a.x < b.x;
            return a.y < b.y;
        });
    return points;
}

double PointDistance(const cv::Point2f& a, const cv::Point2f& b)
{
    const double dx = (double)b.x - (double)a.x;
    const double dy = (double)b.y - (double)a.y;
    return std::sqrt(dx * dx + dy * dy);
}

double ProjectedDistance(const cv::Point2f& a, const cv::Point2f& b, const cv::Point2f& axis)
{
    const double n = std::sqrt((double)axis.x * axis.x + (double)axis.y * axis.y);
    if (n <= 1e-9)
        return PointDistance(a, b);

    const double ux = axis.x / n;
    const double uy = axis.y / n;
    const double dx = (double)b.x - (double)a.x;
    const double dy = (double)b.y - (double)a.y;
    return std::abs(dx * ux + dy * uy);
}

cv::Vec4f FitLineOrDefault(const std::vector<cv::Point2f>& points)
{
    if (points.size() >= 2)
    {
        cv::Vec4f line;
        cv::fitLine(points, line, cv::DIST_L2, 0, 0.01, 0.01);
        return line;
    }

    if (points.size() == 1)
        return cv::Vec4f(1.f, 0.f, points[0].x, points[0].y);

    return cv::Vec4f(1.f, 0.f, 0.f, 0.f);
}

} // namespace VisionGeometry
} // namespace BeeCpp

