#pragma once
#include <opencv2/opencv.hpp>

using namespace cv;

namespace BeeAlign
{
    static inline cv::Mat ShiftXY(const cv::Mat& img, int dx, int dy)
    {
        cv::Mat M = (cv::Mat_<double>(2, 3) <<
            1, 0, dx,
            0, 1, dy
            );

        cv::Mat out;
        warpAffine(img, out, M, img.size(), cv::INTER_NEAREST, cv::BORDER_CONSTANT);
        return out;
    }
    struct OffsetResult {
        int dx, dy;
        int diffCount;
    };
    inline cv::Mat AlignECC(
        const cv::Mat& raw,
        const cv::Mat& tpl,
        cv::Point2f& outOffset,
        float& outAngleDeg   // thêm tham số này
    )
    {
        cv::Mat g1, g2;
        cv::cvtColor(raw, g1, cv::COLOR_BGR2GRAY);
        cv::cvtColor(tpl, g2, cv::COLOR_BGR2GRAY);

        g1.convertTo(g1, CV_32F);
        g2.convertTo(g2, CV_32F);

        cv::Mat warp = cv::Mat::eye(2, 3, CV_32F);  // affine 2x3

        int maxIter = 100;
        double eps = 1e-6;

        cv::findTransformECC(
            g2, g1,                      // template, input
            warp,
            cv::MOTION_AFFINE,
            cv::TermCriteria(
                cv::TermCriteria::COUNT + cv::TermCriteria::EPS,
                maxIter, eps)
        );

        // --------------------------
        // 1) Tách translation
        // --------------------------
        float tx = warp.at<float>(0, 2);
        float ty = warp.at<float>(1, 2);
        outOffset = cv::Point2f(tx, ty);

        // --------------------------
        // 2) Tách rotation angle
        // --------------------------
        float a = warp.at<float>(0, 0);
        float c = warp.at<float>(1, 0);
        float angleRad = std::atan2(c, a);
        outAngleDeg = angleRad * 180.0f / CV_PI;

        // --------------------------
        // 3) Warp ảnh + nền trắng
        // --------------------------
        cv::Mat aligned;
        cv::warpAffine(
            raw, aligned, warp,
            tpl.size(),
            cv::INTER_LINEAR | cv::WARP_INVERSE_MAP,
            cv::BORDER_CONSTANT,
            cv::Scalar(255, 255, 255)    // bù nền trắng
        );

        return aligned;
    }


    OffsetResult FindBestOffset(
        const cv::Mat& raw,
        const cv::Mat& tpl,
        int tol,
        int searchRange,                // ví dụ 10 pixel
        int (*DiffFunc)(const cv::Mat&, const cv::Mat&, int) // Pixel diff function của bạn
    )
    {
        OffsetResult best = { 0,0,INT_MAX };

        for (int dy = -searchRange; dy <= searchRange; ++dy)
        {
            for (int dx = -searchRange; dx <= searchRange; ++dx)
            {
                cv::Mat shifted = ShiftXY(raw, dx, dy);

                int d = DiffFunc(shifted, tpl, tol);
                if (d < best.diffCount)
                {
                    best.diffCount = d;
                    best.dx = dx;
                    best.dy = dy;
                }
            }
        }

        return best;
    }
}
