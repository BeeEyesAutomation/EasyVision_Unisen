#include "ColorPixel.h"

#include <vector>
#include <cstring>
#include <atomic>
#include <Align.h>

using namespace System;
using namespace BeeCpp;
using namespace cv;


     void ColorPx::RemoveSmallBlobs(cv::Mat& mask, int minArea )
    {
        std::vector<std::vector<cv::Point>> contours;
        findContours(mask, contours, RETR_EXTERNAL, CHAIN_APPROX_SIMPLE);

        for (size_t i = 0; i < contours.size(); ++i)
        {
            double area = contourArea(contours[i]);
            if (area < minArea) {
                drawContours(mask, contours, (int)i, cv::Scalar(0), cv::FILLED);
            }
        }
    }
    //// ---------- Utils ----------
    // inline Mat ImdecodeColor(array<Byte>^ bytes) {
    //    if (!bytes || bytes->Length == 0) return Mat();
    //    pin_ptr<Byte> p = &bytes[0];
    //    std::vector<uchar> v(reinterpret_cast<uchar*>(p), reinterpret_cast<uchar*>(p) + bytes->Length);
    //    return imdecode(v, IMREAD_COLOR); // CV_8UC3
    //}
      bool ColorPx::IsBGR8(const Mat& m) { return m.type() == CV_8UC3; }
      static void FilterBlobsByAspect(
          cv::Mat& mask8u,
          float minAspect
      )
      {
          CV_Assert(mask8u.type() == CV_8UC1);

          std::vector<std::vector<cv::Point>> contours;
          findContours(mask8u, contours, RETR_EXTERNAL, CHAIN_APPROX_SIMPLE);

          cv::Mat filtered = cv::Mat::zeros(mask8u.size(), CV_8UC1);

          for (const auto& c : contours)
          {
              if (c.size() < 10) continue;

              cv::RotatedRect rr = minAreaRect(c);
              float w = rr.size.width;
              float h = rr.size.height;
              if (w <= 1 || h <= 1) continue;

              float aspect = std::min(w, h)/ std::max(w, h) ;

              if (aspect >= minAspect)
              {
                  drawContours(filtered,
                      std::vector<std::vector<cv::Point>>{c},
                      -1, cv::Scalar(255), FILLED);
              }
          }

          mask8u = filtered;
      }
      Mat ColorPx::RemoveByThicknessDT(const Mat& bin255, float minRadius)
      {
          CV_Assert(bin255.type() == CV_8U);

          Mat bin;
          threshold(bin255, bin, 0, 255, THRESH_BINARY);

          Mat labels, stats, centroids;
          int n = connectedComponentsWithStats(
              bin, labels, stats, centroids, 8, CV_32S);

          Mat out = Mat::zeros(bin.size(), CV_8U);

          for (int i = 1; i < n; i++) // skip background
          {
              Mat mask = (labels == i);
              mask.convertTo(mask, CV_8U, 255);

              Mat dist;
              distanceTransform(mask, dist, DIST_L2, 3);

              double maxDist;
              minMaxLoc(dist, nullptr, &maxDist);

              if (maxDist >= minRadius)
              {
                  // blob đủ dày → giữ NGUYÊN
                  out.setTo(255, mask);
              }
              // else: blob mỏng → xóa toàn bộ
          }

          return out;
      }

    // ---------- FAST path: vector hóa + đa luồng nội bộ OpenCV (không filter) ----------
// ---------- FAST path: grayscale + blur để giảm viền ----------
     int ColorPx:: DiffCount_Fast(const Mat& img, const Mat& tpl, int tol, Mat* annotated, int SzClearNoise,float Aspect)
    {
        Rect roi(0, 0, tpl.cols, tpl.rows);
        Mat I = img(roi), T = tpl;

        // 1. Grayscale
        Mat g1, g2;
        cvtColor(I, g1, COLOR_BGR2GRAY);
        cvtColor(T, g2, COLOR_BGR2GRAY);

        // 2. Normalize
        normalize(g1, g1, 0, 255, NORM_MINMAX);
        normalize(g2, g2, 0, 255, NORM_MINMAX);

        // 3. Blur nhẹ để kill sub-pixel
        GaussianBlur(g1, g1, Size(3, 3), 0.8);
        GaussianBlur(g2, g2, Size(3, 3), 0.8);

        // 4. Diff
        Mat diff;
        absdiff(g1, g2, diff);                // 8UC1

        // 5. Threshold
        Mat mask;
        threshold(diff, mask, tol, 255, THRESH_BINARY);  // tol chính là ngưỡng diff gray
        RemoveSmallBlobs(mask,  SzClearNoise);
      
        mask=  RemoveByThicknessDT(mask, Aspect);
        // 6. Đếm
        int diffCount = countNonZero(mask);

        if (annotated) {
            annotated->create(img.size(), img.type());
            annotated->setTo(Scalar(0, 0, 0));                // nền đen
            annotated->operator()(roi).setTo(Scalar(0, 0, 255), mask); // tô đỏ chỗ khác
        }
        return diffCount;
    }


    // ---------- PARALLEL tiled (KHÔNG early-exit) ----------
// ---------- PARALLEL tiled (grayscale + blur) ----------

    // 
//  PARALLEL TILE BODY
// 
    //struct DiffTileBody : public cv::ParallelLoopBody
    //{
    //    const cv::Mat& G1;    // gray normalized & blurred
    //    const cv::Mat& G2;
    //    const int tol;
    //    std::atomic<int>& acc;
    //    cv::Mat* ann;
    //    bool annotate;

    //    DiffTileBody(const cv::Mat& g1_, const cv::Mat& g2_, int tol_,
    //        std::atomic<int>& acc_, cv::Mat* ann_, bool annotate_)
    //        : G1(g1_), G2(g2_), tol(tol_), acc(acc_), ann(ann_), annotate(annotate_)
    //    {
    //    }

    //    void operator()(const cv::Range& r) const override
    //    {
    //        const int W = G2.cols;

    //        for (int y = r.start; y < r.end; )
    //        {
    //            int h = std::min(256, r.end - y);
    //            cv::Rect tile(0, y, W, h);

    //            cv::Mat diff;
    //            cv::absdiff(G1(tile), G2(tile), diff);

    //            cv::Mat mask;
    //            cv::threshold(diff, mask, tol, 255, cv::THRESH_BINARY);

    //           _img-> RemoveSmallBlobs(mask, 15);

    //            int cnt = cv::countNonZero(mask);
    //            acc.fetch_add(cnt, std::memory_order_relaxed);

    //            if (annotate && ann)
    //                ann->operator()(tile).setTo(cv::Scalar(0, 0, 255), mask);

    //            y += h;
    //        }
    //    }
    //};

    // 
    //  MAIN PARALLEL DIFF
    // 
    //static int DiffCount_ParallelFull(const cv::Mat& img, const cv::Mat& tpl, int tol, cv::Mat* annotated)
    //{
    //    // ====
    //    // 1) CLAMP ROI để đảm bảo img và tpl có cùng kích thước xử lý
    //    // ====
    //    int W = std::min(img.cols, tpl.cols);
    //    int H = std::min(img.rows, tpl.rows);

    //    if (W <= 0 || H <= 0)
    //        return 0;

    //    cv::Rect roi(0, 0, W, H);

    //    cv::Mat I = img(roi);
    //    cv::Mat T = tpl(cv::Rect(0, 0, W, H));

    //    // ====
    //    // 2) Prepare grayscale
    //    // ====
    //    cv::Mat g1, g2;
    //    cv::cvtColor(I, g1, cv::COLOR_BGR2GRAY);
    //    cv::cvtColor(T, g2, cv::COLOR_BGR2GRAY);

    //    cv::normalize(g1, g1, 0, 255, cv::NORM_MINMAX);
    //    cv::normalize(g2, g2, 0, 255, cv::NORM_MINMAX);

    //    cv::GaussianBlur(g1, g1, cv::Size(3, 3), 0.8);
    //    cv::GaussianBlur(g2, g2, cv::Size(3, 3), 0.8);

    //    // ====
    //    // 3) Prepare annotation buffer
    //    // ====
    //    cv::Mat localAnn;
    //    if (annotated)
    //    {
    //        annotated->create(img.size(), img.type());
    //        annotated->setTo(cv::Scalar(0, 0, 0));  // nền đen toàn ảnh
    //        localAnn = annotated->operator()(roi);
    //    }

    //    // ====
    //    // 4) PARALLEL DIFF
    //    // ====
    //    std::atomic<int> acc(0);
    //    DiffTileBody body(g1, g2, tol, acc, (annotated ? &localAnn : nullptr), annotated != nullptr);

    //    // hàng lớn → chia tile theo height
    //    cv::parallel_for_(cv::Range(0, H), body, 512);

    //    // ====
    //    // 5) Return count
    //    // ====
    //    return acc.load(std::memory_order_relaxed);
    //}
    ////struct DiffTileBody : public ParallelLoopBody {
    //    const Mat& G1, & G2;          // grayscale đã chuẩn hoá + blur
    //    const int tol;
    //    std::atomic<int>& acc;
    //    Mat* ann; bool annotate;

    //    DiffTileBody(const Mat& g1_, const Mat& g2_, int tol_,
    //        std::atomic<int>& acc_, Mat* ann_, bool annotate_)
    //        : G1(g1_), G2(g2_), tol(tol_), acc(acc_), ann(ann_), annotate(annotate_) {
    //    }

    //    void operator()(const Range& r) const override {
    //        const int x0 = 0, w = G2.cols;
    //        for (int y = r.start; y < r.end; ) {
    //            const int h = std::min(256, r.end - y);           // tile 256 hàng
    //            Rect tile(x0, y, w, h);

    //            // 4. Diff trên gray
    //            Mat diff;
    //            absdiff(G1(tile), G2(tile), diff);                // 8UC1

    //            // 5. Threshold
    //            Mat mask;
    //            threshold(diff, mask, tol, 255, THRESH_BINARY);
    //            RemoveSmallBlobs(mask, 15);
    //            // Đếm
    //            int cnt = countNonZero(mask);
    //            acc.fetch_add(cnt, std::memory_order_relaxed);

    //            if (annotate && ann)
    //                ann->operator()(tile).setTo(Scalar(0, 0, 255), mask);

    //            y += h;
    //        }
    //    }
    //};

    //static int DiffCount_ParallelFull(const Mat& img, const Mat& tpl, int tol, Mat* annotated/*=nullptr*/)
    //{
    //    Rect roi(0, 0, tpl.cols, tpl.rows);
    //    Mat I = img(roi), T = tpl;

    //    // Chuẩn bị grayscale chung cho cả ROI để tránh cvtColor nhiều lần
    //    Mat g1, g2;
    //    cvtColor(I, g1, COLOR_BGR2GRAY);
    //    cvtColor(T, g2, COLOR_BGR2GRAY);

    //    normalize(g1, g1, 0, 255, NORM_MINMAX);
    //    normalize(g2, g2, 0, 255, NORM_MINMAX);

    //    GaussianBlur(g1, g1, Size(3, 3), 0.8);
    //    GaussianBlur(g2, g2, Size(3, 3), 0.8);

    //    Mat localAnn;
    //    if (annotated) {
    //        annotated->create(img.size(), img.type());
    //        annotated->setTo(Scalar(0, 0, 0));      // nền đen
    //        localAnn = annotated->operator()(roi);
    //    }

    //    std::atomic<int> acc(0);
    //    DiffTileBody body(g1, g2, tol, acc, (annotated ? &localAnn : nullptr), annotated != nullptr);

    //    parallel_for_(Range(0, roi.height), body, 512);

    //    return acc.load(std::memory_order_relaxed);
    //}

    // ---------- Strategy (luôn quét hết) ----------
     int ColorPx:: PixelCheck_MT_FullScan(const Mat& img, const Mat& tpl,
      int tol, Mat* annotated, int SzClearNoise,bool IsMultiCPU, float Aspect)
    {
      /*  CV_Assert(!img.empty() && !tpl.empty());
        CV_Assert(tpl.cols <= img.cols && tpl.rows <= img.rows);
        CV_Assert(IsBGR8(img) && IsBGR8(tpl));*/

       

        // chọn song song toàn phần cho ảnh lớn, còn lại fast path;
        // KHÔNG early-exit ở cả hai nhánh
      //  const bool big = IsMultiCPU; // >= 4MP
        int diffCount = DiffCount_Fast(img, tpl, tol, annotated, SzClearNoise,Aspect); /*big
            ? DiffCount_ParallelFull(img, tpl, tol, annotated)
            :  DiffCount_Fast(img, tpl, tol, annotated,  SzClearNoise);*/

 
        return diffCount;
    }

 // anonymous namespace
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
// === PUBLIC API ===
System::IntPtr ColorPixel::SetImgeSample(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool NoCrop,
    [System::Runtime::InteropServices::Out] int% outW,
    [System::Runtime::InteropServices::Out] int% outH,
    [System::Runtime::InteropServices::Out] int% outStride,
    [System::Runtime::InteropServices::Out] int% outChannels)
{

    if (NoCrop)
        _img->temp = Mat(tplH, tplW, CV_8UC3, tplData.ToPointer(), tplStride).clone();
    else
    {
        Nullable<RectRotateCli> mask =
            rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
            : Nullable<RectRotateCli>();
        com->CropRotToMat(
            tplData, tplW, tplH, tplStride, tplChannels,
            rr, mask, /*returnMaskOnly*/ false,
            System::IntPtr(&_img->temp)
        );
    

    }
 
    const int W = _img->temp.cols, H = _img->temp.rows, C = _img->temp.channels();
    const int S = (int)_img->temp.step;
    const size_t bytes = (size_t)S * H;
    IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal((IntPtr)(long long)bytes);
    if (mem == IntPtr::Zero) return IntPtr::Zero;
    std::memcpy(mem.ToPointer(), _img->temp.data, bytes);
    outW = W; outH = H; outStride = S; outChannels = C;
    
    return mem;
}
void ColorPixel::SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, RectRotateCli rr, Nullable<RectRotateCli> rrMask)
{
  
    Nullable<RectRotateCli> mask =
        rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
        : Nullable<RectRotateCli>();
    com->CropRotToMat(
        tplData, tplW, tplH, tplStride, tplChannels,
        rr, mask, /*returnMaskOnly*/ false,
        System::IntPtr(&_img->raw)
    );
   
}
//static int DiffHelper(const cv::Mat& a, const cv::Mat& b, int tol)
//{
//    return _img->PixelCheck_MT_FullScan(a, b, tol, nullptr,0,false);
//}
//IntPtr ColorPixel::CheckImageFromBytes(
//    array<Byte>^ imageBytes,
//    array<Byte>^ templateBytes,
//    int colorTolerance,
//     float% outPx,
//    int% outW, int% outH, int% outStride, int% outChannels
//   )
//{
//    outW = outH = outStride = outChannels = 0;
//
//    Mat img = ImdecodeColor(imageBytes);
//    Mat tpl = ImdecodeColor(templateBytes);
//    if (img.empty() || tpl.empty()) return IntPtr::Zero;
//    //auto best = BeeAlign::FindBestOffset(
//    //    _img->raw,
//    //    _img->temp,
//    //    colorTolerance,
//    //    50,          // search range
//    //    DiffHelper   // KHÔNG dùng lambda nữa
//    //);
//    Mat annotated;
//    // dịch về vị trí đúng nhất
//   // cv::Mat aligned = BeeAlign::ShiftXY(_img->raw, best.dx, best.dy);
//
//    // cuối cùng mới diff thật
//    outPx = PixelCheck_MT_FullScan(_img->raw, _img->temp, colorTolerance, &annotated,0);
//
//   /* if (!IsBGR8(img)) cvtColor(img, img, COLOR_BGR2BGR);
//    if (!IsBGR8(tpl)) cvtColor(tpl, tpl, COLOR_BGR2BGR);*/
//
//  
//  //  outPx = PixelCheck_MT_FullScan(rawAligned, tpl, colorTolerance, &annotated);
//
//    if (!annotated.isContinuous()) annotated = annotated.clone();
//    const int W = annotated.cols, H = annotated.rows, C = annotated.channels();
//    const int S = (int)annotated.step;
//    const size_t bytes = (size_t)S * H;
//
//    IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal((IntPtr)(long long)bytes);
//    if (mem == IntPtr::Zero) return IntPtr::Zero;
//    std::memcpy(mem.ToPointer(), annotated.data, bytes);
//
//    outW = W; outH = H; outStride = S; outChannels = C;
//   
//    return mem;
//}
// Xóa các vùng nhiễu có diện tích nhỏ hơn minArea (px)
void ColorPixel::SetImgeSampleNoCrop(IntPtr data, int w, int h, int stride, int ch)
{
    cv::Mat m(h, w, CV_8UC3, data.ToPointer(), stride);
    _img->temp = m.clone();   // BẮT BUỘC
    //_img->temp = Mat(h, w, CV_8UC3, data.ToPointer(), stride);

}
void ColorPixel::SetRawNoCrop(IntPtr data, int w, int h, int stride, int ch)
{
    if (data == IntPtr::Zero || w <= 0 || h <= 0 || stride <= 0)
    {
        // reset an toàn
        _img->raw.release();
        return;
    }
    if (!_img->raw.empty())
    {
        _img->raw.release();
    }
    // chọn kiểu Mat đúng kênh
    int cvType;
    switch (ch)
    {
    case 1: cvType = CV_8UC1; break;
    case 3: cvType = CV_8UC3; break;
    case 4: cvType = CV_8UC4; break;
    default: cvType = CV_8UC1; break; // fallback
    }

    // wrap IntPtr thành Mat (không copy dữ liệu)
    cv::Mat wrapped(h, w, cvType, data.ToPointer(), stride);

    // clone để đảm bảo Mat trong C++ sở hữu dữ liệu, tránh phụ thuộc vào vùng nhớ bên C#
    this->_img->raw = wrapped.clone();
    //img->matRaw = Mat (h, w, CV_8UC1, data.ToPointer(), stride);

}
void ColorPixel::SaveRandom(int i)
{
    std::srand((unsigned)std::time(nullptr));

    //std::ostringstream oss;
    //oss << "Temp\\" << "raw_"
    //    
    //    << i
    //    << ".png";

   // cv::imwrite(oss.str(), _img->raw);
    std::ostringstream oss2;
    oss2 << "Temp\\" << "temp_"

        << i
        << ".png";

    cv::imwrite(oss2.str(), _img->temp);
}
IntPtr ColorPixel::CheckImageFromMat(
    bool IsAlign,int ModeAlign,bool IsMultiCPU,
    int colorTolerance, int SzClearNoise, float Aspect,
    float% outPx, float% outOffsetX, float% outOffsetY, float% Offsetangle,
    int% outW, int% outH, int% outStride, int% outChannels)
{
   
    outW = outH = outStride = outChannels = 0;
 
    if (_img->raw.type() != CV_8UC3) {
        return IntPtr::Zero;
    }
    if (_img->temp.type() != CV_8UC3) {
        return IntPtr::Zero;
    }
 
 //   Mat rawAligned = BeeAlign::AutoAlign(_img->raw, _img->temp);
    //auto best = BeeAlign::FindBestOffset(
    //    _img->raw,
    //    _img->temp,
    //    colorTolerance,
    //    50,          // search range
    //    DiffHelper   // KHÔNG dùng lambda nữa
    //);
  // Mat annotated;
   cv::Point2f ofs;
   float angle;
    //// dịch về vị trí đúng nhất
  //  cv::Mat aligned = BeeAlign::ShiftXY(_img->raw, best.dx, best.dy);
   cv::Mat annotated;
   if (IsAlign)
   {
       auto R = BeeAlign::AlignECC(_img->raw, _img->temp.clone(), (BeeAlign::ECCSpeed)ModeAlign);
     //  if (!R.success) return IntPtr::Zero;

       outOffsetX = R.dx;
       outOffsetY = R.dy;
       Offsetangle = R.angle;
       
       if (!R.success)
       {
           outPx = 100000;
           annotated = _img->temp.clone();
           const int W = annotated.cols, H = annotated.rows, C = annotated.channels();
           const int S = (int)annotated.step;
           const size_t bytes = (size_t)S * H;

           IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal((IntPtr)(long long)bytes);
           if (mem == IntPtr::Zero)
               return IntPtr::Zero;
           std::memcpy(mem.ToPointer(), annotated.data, bytes);

           outW = W; outH = H; outStride = S; outChannels = C;
           return mem;
       }
          
       //outPx = PixelCheck_MT_FullScan(R.aligned, _img->temp, colorTolerance, &annotated);

        //Mat aligned = BeeAlign::AlignECC(_img->raw, _img->temp, ofs, angle);
        //// xuất offset cho C#
        //outOffsetX = ofs.x;
        //outOffsetY = ofs.y;
        //Offsetangle = angle;
       outPx = _img->PixelCheck_MT_FullScan(R.aligned, _img->temp.clone(), colorTolerance, &annotated, SzClearNoise, IsMultiCPU,Aspect);
   }
   else
   {
      
      // cv::imwrite("i.png", _img->raw);
      // Mat temp = _img->temp.clone();
       outPx =_img-> PixelCheck_MT_FullScan(_img->raw, _img->temp, colorTolerance, &annotated, SzClearNoise, IsMultiCPU, Aspect);
       outOffsetX =0;
       outOffsetY =0;
       Offsetangle =0;
   }
    if (!annotated.isContinuous()) annotated = annotated.clone();
    const int W = annotated.cols, H = annotated.rows, C = annotated.channels();
    const int S = (int)annotated.step;
    const size_t bytes = (size_t)S * H;

    IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal((IntPtr)(long long)bytes);
    if (mem == IntPtr::Zero) 
        return IntPtr::Zero;
    std::memcpy(mem.ToPointer(), annotated.data, bytes);
   
    outW = W; outH = H; outStride = S; outChannels = C;
    
    return mem;
}

void ColorPixel::FreeBuffer(IntPtr p)
{
    if (p != IntPtr::Zero) System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
}
