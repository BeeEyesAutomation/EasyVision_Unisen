#include "ColorPixel.h"

#include <vector>
#include <cstring>
#include <atomic>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace BeeCpp;
using namespace cv;

namespace {

    // ---------- Utils ----------
    static inline Mat ImdecodeColor(array<Byte>^ bytes) {
        if (!bytes || bytes->Length == 0) return Mat();
        pin_ptr<Byte> p = &bytes[0];
        std::vector<uchar> v(reinterpret_cast<uchar*>(p), reinterpret_cast<uchar*>(p) + bytes->Length);
        return imdecode(v, IMREAD_COLOR); // CV_8UC3
    }
    static inline bool IsBGR8(const Mat& m) { return m.type() == CV_8UC3; }

    // ---------- FAST path: vector hóa + đa luồng nội bộ OpenCV (không filter) ----------
    static int DiffCount_Fast(const Mat& img, const Mat& tpl, int tol, Mat* annotated/*=nullptr*/)
    {
        Rect roi(0, 0, tpl.cols, tpl.rows);
        Mat I = img(roi), T = tpl;

        Mat diff; absdiff(I, T, diff);       // 8UC3
        std::vector<Mat> ch(3); split(diff, ch);

        Mat mx;                               // max(|B|,|G|,|R|)
        max(ch[0], ch[1], mx);
        max(mx, ch[2], mx);

        Mat mask = (mx > tol);                // so sánh thuần (0/255), KHÔNG morphology
        int diffCount = countNonZero(mask);

        if (annotated) {
            annotated->create(img.size(), img.type());
            annotated->setTo(Scalar(0, 0, 0));                         // nền đen
            annotated->operator()(roi).setTo(Scalar(0, 0, 255), mask); // tô đỏ chỗ khác
           // rectangle(*annotated, roi, Scalar(0, 255, 255), 2);
        }
        return diffCount;
    }

    // ---------- PARALLEL tiled (KHÔNG early-exit) ----------
    struct DiffTileBody : public ParallelLoopBody {
        const Mat& I; const Mat& T;
        const int tol;
        std::atomic<int>& acc;
        Mat* ann; bool annotate;

        DiffTileBody(const Mat& I_, const Mat& T_, int tol_,
            std::atomic<int>& acc_, Mat* ann_, bool annotate_)
            : I(I_), T(T_), tol(tol_), acc(acc_), ann(ann_), annotate(annotate_) {
        }

        void operator()(const Range& r) const override {
            const int x0 = 0, w = T.cols;
            for (int y = r.start; y < r.end; ) {
                const int h = std::min(256, r.end - y);           // tile 256 hàng
                Rect tile(x0, y, w, h);

                Mat diff; absdiff(I(tile), T(tile), diff);
                std::vector<Mat> ch(3); split(diff, ch);
                Mat mx; max(ch[0], ch[1], mx); max(mx, ch[2], mx);

                Mat mask = (mx > tol);                            // so sánh thuần
                int cnt = countNonZero(mask);

                acc.fetch_add(cnt, std::memory_order_relaxed);    // KHÔNG dừng sớm
                if (annotate && ann) ann->operator()(tile).setTo(Scalar(0, 0, 255), mask);

                y += h;
            }
        }
    };

    static int DiffCount_ParallelFull(const Mat& img, const Mat& tpl, int tol, Mat* annotated/*=nullptr*/)
    {
        Rect roi(0, 0, tpl.cols, tpl.rows);
        Mat I = img(roi), T = tpl;

        Mat localAnn;
        if (annotated) {
            annotated->create(img.size(), img.type());
            annotated->setTo(Scalar(0, 0, 0));                      // nền đen
            localAnn = annotated->operator()(roi);
        }

        std::atomic<int> acc(0);
        DiffTileBody body(I, T, tol, acc, (annotated ? &localAnn : nullptr), annotated != nullptr);

        parallel_for_(Range(0, roi.height), body, 512);           // quét HẾT
        //if (annotated) rectangle(*annotated, roi, Scalar(0, 255, 255), 2);
        return acc.load(std::memory_order_relaxed);
    }

    // ---------- Strategy (luôn quét hết) ----------
    static bool PixelCheck_MT_FullScan(const Mat& img, const Mat& tpl,
        int maxDiffPixels, int tol, Mat* annotated/*=nullptr*/)
    {
        CV_Assert(!img.empty() && !tpl.empty());
        CV_Assert(tpl.cols <= img.cols && tpl.rows <= img.rows);
        CV_Assert(IsBGR8(img) && IsBGR8(tpl));

       

        // chọn song song toàn phần cho ảnh lớn, còn lại fast path;
        // KHÔNG early-exit ở cả hai nhánh
        const bool big = (tpl.total() >= 4ull * 1024 * 1024); // >= 4MP
        int diffCount = big
            ? DiffCount_ParallelFull(img, tpl, tol, annotated)
            : DiffCount_Fast(img, tpl, tol, annotated);

 
        return diffCount <= maxDiffPixels;
    }

} // anonymous namespace
//cv::Mat BeeCpp::ColorPx::RotateMat(const cv::Mat& raw, const cv::RotatedRect& rot)
//{
//    Mat matRs, matR = getRotationMatrix2D(rot.center, rot.angle, 1);
//
//    float fTranslationX = (rot.size.width - 1) / 2.0f - rot.center.x;
//    float fTranslationY = (rot.size.height - 1) / 2.0f - rot.center.y;
//    matR.at<double>(0, 2) += fTranslationX;
//    matR.at<double>(1, 2) += fTranslationY;
//    warpAffine(raw, matRs, matR, rot.size, INTER_LINEAR, BORDER_CONSTANT);
//
//    return matRs;
//}
// ================= PUBLIC API =================
System::IntPtr ColorPixel::SetImgeSample(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, float x, float y, float w, float h, float angle, bool NoCrop,
    [System::Runtime::InteropServices::Out] int% outW,
    [System::Runtime::InteropServices::Out] int% outH,
    [System::Runtime::InteropServices::Out] int% outStride,
    [System::Runtime::InteropServices::Out] int% outChannels)
{

    if (NoCrop)
        _img->temp = Mat(tplH, tplW, CV_8UC3, tplData.ToPointer(), tplStride).clone();
    else
    {
        Mat raw(tplH, tplW, CV_8UC3, tplData.ToPointer(), tplStride);
        _img->temp = Common:: RotateMat(raw, RotatedRect(cv::Point2f(x, y), cv::Size2f(w, h), angle));


    }
 
    const int W = _img->temp.cols, H = _img->temp.rows, C = _img->temp.channels();
    const int S = (int)_img->temp.step;
    const size_t bytes = (size_t)S * H;
    IntPtr mem = Marshal::AllocHGlobal((IntPtr)(long long)bytes);
    if (mem == IntPtr::Zero) return IntPtr::Zero;
    std::memcpy(mem.ToPointer(), _img->temp.data, bytes);
    outW = W; outH = H; outStride = S; outChannels = C;
    
    return mem;
}
void ColorPixel::SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, float x, float y, float w, float h, float angle)
{
    Mat raw(tplH, tplW, CV_8UC3, tplData.ToPointer(), tplStride);
    _img->raw = Common::RotateMat(raw, RotatedRect(cv::Point2f(x, y), cv::Size2f(w, h), angle));
   
}

IntPtr ColorPixel::CheckImageFromBytes(
    array<Byte>^ imageBytes,
    array<Byte>^ templateBytes,
    int maxDiffPixels,
    int colorTolerance,
    int% outW, int% outH, int% outStride, int% outChannels,
    bool% pass, double% cycleTimeMs)
{
    outW = outH = outStride = outChannels = 0; pass = false; cycleTimeMs = 0.0;

    Mat img = ImdecodeColor(imageBytes);
    Mat tpl = ImdecodeColor(templateBytes);
    if (img.empty() || tpl.empty()) return IntPtr::Zero;

   /* if (!IsBGR8(img)) cvtColor(img, img, COLOR_BGR2BGR);
    if (!IsBGR8(tpl)) cvtColor(tpl, tpl, COLOR_BGR2BGR);*/

    Mat annotated;
    bool ok = PixelCheck_MT_FullScan(img, tpl, maxDiffPixels, colorTolerance, &annotated);

    if (!annotated.isContinuous()) annotated = annotated.clone();
    const int W = annotated.cols, H = annotated.rows, C = annotated.channels();
    const int S = (int)annotated.step;
    const size_t bytes = (size_t)S * H;

    IntPtr mem = Marshal::AllocHGlobal((IntPtr)(long long)bytes);
    if (mem == IntPtr::Zero) return IntPtr::Zero;
    std::memcpy(mem.ToPointer(), annotated.data, bytes);

    outW = W; outH = H; outStride = S; outChannels = C;
    pass = ok;
    return mem;
}

IntPtr ColorPixel::CheckImageFromMat(
    
    int maxDiffPixels, int colorTolerance,
    bool% pass, double% cycleTimeMs,
    int% outW, int% outH, int% outStride, int% outChannels)
{
    pass = false; cycleTimeMs = 0.0;
    outW = outH = outStride = outChannels = 0;
    TickMeter tm; tm.start();
    if (_img->raw.type() != CV_8UC3) {
        return IntPtr::Zero;
    }
    if (_img->temp.type() != CV_8UC3) {
        return IntPtr::Zero;
    }
 
      
    
    Mat annotated;
    bool ok = PixelCheck_MT_FullScan(_img->raw, _img->temp, maxDiffPixels, colorTolerance, &annotated);

    if (!annotated.isContinuous()) annotated = annotated.clone();
    const int W = annotated.cols, H = annotated.rows, C = annotated.channels();
    const int S = (int)annotated.step;
    const size_t bytes = (size_t)S * H;

    IntPtr mem = Marshal::AllocHGlobal((IntPtr)(long long)bytes);
    if (mem == IntPtr::Zero) 
        return IntPtr::Zero;
    std::memcpy(mem.ToPointer(), annotated.data, bytes);
    tm.stop(); cycleTimeMs = tm.getTimeMilli();
    outW = W; outH = H; outStride = S; outChannels = C;
    pass = ok;
    return mem;
}

void ColorPixel::FreeBuffer(IntPtr p)
{
    if (p != IntPtr::Zero) Marshal::FreeHGlobal(p);
}
