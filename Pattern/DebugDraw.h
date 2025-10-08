// DebugDraw.h
#pragma once
#include <opencv2/opencv.hpp>
#include "RansacLineCore.h"

namespace BeeCpp {

    struct DebugDrawOptions {
        int   lineThickness = 2;      // bề dày nét line chính
        bool  drawInliers = true;
        bool  drawOutliers = false;  // có thể tắt để nhẹ
        int   pointRadius = 1;      // bán kính chấm inlier/outlier
        int   bandThickness = 1;      // bề dày 2 đường song song (±threshold)
        double fontScale = 0.5;
        int   fontThickness = 1;
        cv::Scalar colorLine = cv::Scalar(0, 255, 255); // vàng
        cv::Scalar colorBand = cv::Scalar(255, 0, 0);   // xanh dương
        cv::Scalar colorP1P2 = cv::Scalar(0, 255, 0);   // xanh lá
        cv::Scalar colorInlier = cv::Scalar(0, 200, 0);
        cv::Scalar colorOutlier = cv::Scalar(0, 0, 255);
        cv::Scalar colorText = cv::Scalar(255, 255, 255);
    };

    /// Vẽ debug vào outBgr (3 kênh). baseGrayOrEdge có thể là 8UC1 hoặc 8UC3/4.
    void RenderLineDebug(const cv::Mat& baseGrayOrEdge,
        const std::vector<cv::Point2f>& allSamples,  // tập điểm dùng RANSAC (sau downsample)
        const BeeCpp::LineResult& r,
        float thresholdPx,
        cv::Mat& outBgr,
        const DebugDrawOptions& opt = DebugDrawOptions());

} // namespace BeeCpp
