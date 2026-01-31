#include "MonoSegCli.h"
#include "MonoSegCore.h"
#include <opencv2/opencv.hpp>

using namespace cv;
using namespace System;

namespace BeeCpp
{
    static MonoSegParams ToNative(MonoSegCliParams p)
    {
        MonoSegParams n;
        n.bgBlurK = p.BgBlurK > 0 ? p.BgBlurK : 41;
        n.openK = p.OpenK > 0 ? p.OpenK : 2;
        n.closeK = p.CloseK > 0 ? p.CloseK : 4;
        n.mode = p.Mode;
        n.useBlackHat = p.UseBlackHat;
        n.blackHatK = p.BlackHatK > 0 ? p.BlackHatK : 31;
        return n;
    }

    static ChipExtractParams ToNative(ChipExtractCliParams p)
    {
        ChipExtractParams n;

        n.minArea = p.MinArea > 0 ? p.MinArea : 80;
        n.minW = p.MinW > 0 ? p.MinW : 4;
        n.minH = p.MinH > 0 ? p.MinH : 20;
        n.minAspect = p.MinAspect > 0 ? p.MinAspect : 1.2f;

        n.vertKernelW = p.VertKW > 0 ? p.VertKW : 3;
        n.vertKernelH = p.VertKH > 0 ? p.VertKH : 15;
        n.openK = p.OpenK > 0 ? p.OpenK : 3;

        n.minFillRatio = (p.MinFillRatio > 0.f) ? p.MinFillRatio : 0.12f;
        n.sizeTol = (p.SizeTol > 0.f) ? p.SizeTol : 0.40f;
        n.paperMinAreaFrac = (p.PaperMinAreaFrac > 0.f) ? p.PaperMinAreaFrac : 0.02f;

        return n;
    }

    int MonoSegCli::SegmentMonoLowContrast(
        IntPtr grayPtr, int w, int h, int step,
        IntPtr% outMaskPtr, int% outMaskStep,
        MonoSegCliParams p, bool IsHardNoise
    )
    {
        Mat gray(h, w, CV_8UC1, grayPtr.ToPointer(), (size_t)step);
        Mat mask;
        int area = 0;
        if (!IsHardNoise)
            int area = MonoSegCore::SegmentLowContrastMono(gray, mask, ToNative(p));
        else
            int area = MonoSegCore::SegmentLowContrastMonoHardNoise(gray, mask, ToNative(p));
        // output mask: step = w (CV_8UC1 packed)
        outMaskStep = w;
        size_t bytes = (size_t)w * h;
        if (outMaskPtr == IntPtr::Zero)
            outMaskPtr = IntPtr(std::malloc(bytes));

        for (int y = 0; y < h; y++)
            memcpy((uchar*)outMaskPtr.ToPointer() + (size_t)y * w,
                mask.ptr(y), w);

        return area;
    }

    cli::array<RectRotateCli>^ MonoSegCli::ExtractPaperAndChipRects(
        IntPtr maskPtr, int w, int h, int step,
        ChipExtractCliParams p
    )
    {
        if (maskPtr == IntPtr::Zero) throw gcnew ArgumentNullException("maskPtr");

        Mat mask(h, w, CV_8UC1, maskPtr.ToPointer(), (size_t)step);

        std::vector<cv::RotatedRect> rr;
        std::vector<uint8_t> isPaper;
        MonoSegCore::ExtractPaperAndChipRectRotatesFromMask(mask, rr, isPaper, ToNative(p));

        auto arr = gcnew cli::array<RectRotateCli>((int)rr.size());
        for (int i = 0; i < (int)rr.size(); i++)
        {
            RectRotateCli r;
            r.Shape = ShapeType::Rectangle;
            r.PosCenter = PointF32((float)rr[i].center.x, (float)rr[i].center.y);
            RectF32 wh(
                0.0f, 0.0f,                               // X,Y local = 0
                (float)rr[i].size.width,
                (float)rr[i].size.height
            );
            // cli.RectWH = wh;
            r.RectWH = wh;// RectF32((float)rr[i].size.width, (float)rr[i].size.height);
            r.RectRotationDeg = rr[i].angle;

            bool paper = (i < (int)isPaper.size()) ? (isPaper[i] != 0) : false;
            r.IsWhite = !paper; // chip=true, paper=false

            r.PolyLocalPoints = nullptr;
            r.HexVertexOffsets = nullptr;

            arr[i] = r;
        }

        return arr;
    }

    static void DrawOneRotatedRect(Mat& img, const RotatedRect& rr, const Scalar& col, int thickness)
    {
        Point2f pts[4];
        rr.points(pts);
        for (int k = 0; k < 4; k++)
            line(img, pts[k], pts[(k + 1) & 3], col, thickness);
    }

    void MonoSegCli::DrawRectRotateToColorImage(
        IntPtr basePtr, int w, int h, int baseStep,
        cli::array<RectRotateCli>^ rects,
        IntPtr% outColorPtr,
        int% outColorStep
    )
    {
        if (basePtr == IntPtr::Zero) throw gcnew ArgumentNullException("basePtr");
        if (rects == nullptr) throw gcnew ArgumentNullException("rects");
        if (baseStep < w) throw gcnew ArgumentException("Invalid baseStep");

        Mat base(h, w, CV_8UC1, basePtr.ToPointer(), (size_t)baseStep);

        Mat outBGR;
        cvtColor(base, outBGR, COLOR_GRAY2BGR);

        for (int i = 0; i < rects->Length; i++)
        {
            RectRotateCli r = rects[i];

            RotatedRect rr(
                Point2f(r.PosCenter.X, r.PosCenter.Y),
                Size2f(r.RectWH.Width, r.RectWH.Height),
                (float)r.RectRotationDeg
            );

            // paper đỏ, chip xanh
            Scalar col = r.IsWhite ? Scalar(0, 255, 0) : Scalar(0, 0, 255);
            DrawOneRotatedRect(outBGR, rr, col, 2);
        }

        // allocate + copy BGR
        outColorStep = w * 3;
        size_t bytes = (size_t)outColorStep * h;
        if (outColorPtr == IntPtr::Zero)
            outColorPtr = IntPtr(std::malloc(bytes));

        for (int y = 0; y < h; y++)
        {
            memcpy(
                (uchar*)outColorPtr.ToPointer() + (size_t)y * outColorStep,
                outBGR.ptr(y),
                outColorStep
            );
        }
    }

    void MonoSegCli::FreeBuffer(IntPtr ptr)
    {
        if (ptr != IntPtr::Zero) std::free(ptr.ToPointer());
    }
}
