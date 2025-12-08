#pragma once
#include <opencv2/opencv.hpp>

using namespace cv;

namespace BeeAlign
{
    enum class ECCSpeed
    {
        Slow = 0,      // chậm nhưng rất chính xác
        Normal = 1,    // mặc định
        Fast = 2       // nhanh, đủ tốt
    };
    struct ECC_Params
    {
        double downScale;          // 0.3–1.0
        int    maxIter;
        double eps;
        int    gaussSize;
        int    pyramid;
        int    motion;
        cv::Scalar border;
    };

    struct ECC_Result
    {
        float dx = 0;
        float dy = 0;
        float angle = 0;
        bool success = false;
        cv::Mat aligned;
    };
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
    inline int FixKernel(int k)
    {
        if (k < 1) k = 1;
        if ((k % 2) == 0) k += 1; // phải lẻ
        return k;
    }
    inline ECC_Params MakeECCParams(ECCSpeed speed)
    {
        ECC_Params P;
        P.border = cv::Scalar(255, 255, 255);

        switch (speed)
        {
        case ECCSpeed::Slow:
            P.downScale = 1.0;                // không giảm
            P.maxIter = 80;
            P.eps = 1e-6;
            P.motion = cv::MOTION_AFFINE;
            P.gaussSize = 7;                  // blur mạnh hơn
            break;

        case ECCSpeed::Fast:
            P.downScale = 0.5;                // giảm 1/2 cho nhanh
            P.maxIter = 25;
            P.eps = 1e-3;
            P.motion = cv::MOTION_EUCLIDEAN; // chỉ xoay + dịch
            P.gaussSize = 3;
            break;

        default: // Normal
            P.downScale = 0.5;
            P.maxIter = 40;
            P.eps = 1e-4;
            P.motion = cv::MOTION_AFFINE;
            P.gaussSize = 5;
            break;
        }

        // đảm bảo gaussSize hợp lệ cho ECC
        P.gaussSize = FixKernel(P.gaussSize);
        return P;
    }

    inline ECC_Result AlignECC_Custom(
        const cv::Mat& raw,
        const cv::Mat& tpl,
        const ECC_Params& P)
    {
        ECC_Result R;
        if (raw.empty() || tpl.empty())
            return R;

        // 1) Gray
        cv::Mat g1, g2;
        cv::cvtColor(raw, g1, cv::COLOR_BGR2GRAY);
        cv::cvtColor(tpl, g2, cv::COLOR_BGR2GRAY);

        // (optional) blur nhẹ ngoài nếu muốn
        // cv::GaussianBlur(g1, g1, cv::Size(3,3), 0.8);
        // cv::GaussianBlur(g2, g2, cv::Size(3,3), 0.8);

        // 2) Downscale để tăng tốc
        double s = P.downScale;
        cv::Mat s1, s2;
        cv::resize(g1, s1, cv::Size(), s, s, cv::INTER_AREA);
        cv::resize(g2, s2, cv::Size(), s, s, cv::INTER_AREA);

        // 3) Kiểm tra kích thước đủ lớn
        if (s1.cols < 8 || s1.rows < 8 || s2.cols < 8 || s2.rows < 8)
        {
            // ảnh quá nhỏ, ECC không ổn định
            R.success = false;
            return R;
        }

        // 4) Warp matrix ECC
        cv::Mat warp = cv::Mat::eye(2, 3, CV_32F);

        try
        {
            cv::TermCriteria criteria(
                cv::TermCriteria::COUNT + cv::TermCriteria::EPS,
                P.maxIter,
                P.eps
            );

            int gaussFiltSize = FixKernel(P.gaussSize);  // dùng làm gaussFiltSize

            // *** LƯU Ý: TRÊN OPENCV 4.5.5, THAM SỐ CUỐI LÀ gaussFiltSize ***
            cv::findTransformECC(
                s2, s1,
                warp,
                P.motion,
                criteria,
                cv::noArray(),
                gaussFiltSize  // KHÔNG PHẢI PYRAMID!
            );

            R.success = true;
        }
        catch (...)
        {
            R.success = false;
            return R;
        }

        // 5) Extract dx,dy,angle
        float a = warp.at<float>(0, 0);
        float b = warp.at<float>(0, 1);
        float c = warp.at<float>(1, 0);
        float d = warp.at<float>(1, 1);
        float tx = warp.at<float>(0, 2);
        float ty = warp.at<float>(1, 2);

        R.angle = std::atan2(c, a) * 180.f / CV_PI;
        R.dx = tx / (float)s;
        R.dy = ty / (float)s;

        // 6) Warp full-res RAW → aligned
        cv::Mat warpFull = (cv::Mat_<float>(2, 3) <<
            a, b, tx / s,
            c, d, ty / s
            );

        cv::warpAffine(
            raw,
            R.aligned,
            warpFull,
            raw.size(),
            cv::INTER_LINEAR | cv::WARP_INVERSE_MAP,
            cv::BORDER_CONSTANT,
            P.border
        );

        return R;
    }


    //inline ECC_Result AlignECC_Custom(
    //    const cv::Mat& raw,
    //    const cv::Mat& tpl,
    //    const ECC_Params& P)
    //{
    //    ECC_Result R;
    //    if (raw.empty() || tpl.empty()) return R;

    //    cv::Mat g1, g2;
    //    cv::cvtColor(raw, g1, cv::COLOR_BGR2GRAY);
    //    cv::cvtColor(tpl, g2, cv::COLOR_BGR2GRAY);

    //    // Blur nhẹ
    //    if (P.gaussSize > 1)
    //    {
    //        int k = (P.gaussSize % 2 == 0 ? P.gaussSize + 1 : P.gaussSize);
    //        cv::GaussianBlur(g1, g1, cv::Size(k, k), 1.2);
    //        cv::GaussianBlur(g2, g2, cv::Size(k, k), 1.2);
    //    }

    //    // Downscale
    //    double s = P.downScale;
    //    cv::Mat s1, s2;
    //    cv::resize(g1, s1, cv::Size(), s, s, cv::INTER_AREA);
    //    cv::resize(g2, s2, cv::Size(), s, s, cv::INTER_AREA);

    //    cv::Mat warp = cv::Mat::eye(2, 3, CV_32F);

    //    try
    //    {
    //        cv::TermCriteria crit(
    //            cv::TermCriteria::COUNT + cv::TermCriteria::EPS,
    //            P.maxIter, P.eps);

    //        cv::findTransformECC(
    //            s2, s1,
    //            warp,
    //            P.motion,
    //            crit,
    //            cv::noArray(),
    //            P.pyramid
    //        );
    //        R.success = true;
    //    }
    //    catch (...)
    //    {
    //        R.success = false;
    //        return R;
    //    }

    //    float a = warp.at<float>(0, 0);
    //    float c = warp.at<float>(1, 0);
    //    float tx = warp.at<float>(0, 2);
    //    float ty = warp.at<float>(1, 2);

    //    R.dx = tx / (float)s;
    //    R.dy = ty / (float)s;
    //    R.angle = std::atan2(c, a) * 180.0f / CV_PI;

    //    cv::Mat warpFull = warp.clone();
    //    warpFull.at<float>(0, 2) = tx / (float)s;
    //    warpFull.at<float>(1, 2) = ty / (float)s;

    //    cv::warpAffine(
    //        raw, R.aligned,
    //        warpFull,
    //        tpl.size(),
    //        cv::INTER_LINEAR | cv::WARP_INVERSE_MAP,
    //        cv::BORDER_CONSTANT,
    //        P.border
    //    );

    //    return R;
    //}
    inline ECC_Result AlignECC(
        const cv::Mat& raw,
        const cv::Mat& tpl,
        ECCSpeed speed = ECCSpeed::Normal)
    {
        ECC_Params P = MakeECCParams(speed);
        return AlignECC_Custom(raw, tpl, P);
    }
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
