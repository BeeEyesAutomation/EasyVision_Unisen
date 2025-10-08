#include "ColorArea.h"

#include <cstring>
#include <cmath>
#include <limits>
using namespace cv;
using namespace std;
using namespace System;
using namespace BeeCpp;

ColorArea::ColorArea() { _ColorPP=new ColorAreaPP(); com = new CommonPlus(); }
ColorArea::~ColorArea() { this->!ColorArea(); }
ColorArea::!ColorArea() { if (_ColorPP) { delete _ColorPP; _ColorPP = nullptr; } }
void ColorArea::SetImgeCrop(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, RectRotateCli rr, Nullable<RectRotateCli> rrMask)
{
    try
    {
        // 1) Wrap dữ liệu managed bằng Mat với stride (step) tùy ý
        int type = (tplChannels == 1) ? CV_8UC1 :
            (tplChannels == 3) ? CV_8UC3 : CV_8UC4;

        Mat wrapped(tplH, tplW, type, tplData.ToPointer(), (size_t)tplStride);

        // 2) Convert sang 8U3 (BGR)
        Mat bgr;
        switch (tplChannels)
        {
        case 1:  cvtColor(wrapped, bgr, COLOR_GRAY2BGR); break; // 1 -> 3
        case 3:  bgr = wrapped; break;                           // đã 3 kênh
        case 4:  cvtColor(wrapped, bgr, COLOR_BGRA2BGR); break;  // 4 -> 3
        }	
        Nullable<RectRotateCli> mask =
            rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
            : Nullable<RectRotateCli>();
        com->CropRotToMat(
            tplData, tplW, tplH, tplStride, tplChannels,
            rr, mask, /*returnMaskOnly*/ false,
            System::IntPtr(&_ColorPP->matCrop)
        );
     //   cv::imwrite("color.png", _ColorPP->matCrop);
    }
    catch (const cv::Exception& ex)
    {
        throw gcnew System::Exception(gcnew System::String(ex.what()));
    }
	//Mat raw(tplH, tplW, CV_8UC3, tplData.ToPointer(), tplStride);
	//_ColorPP->matRaw = com->RotateMat(raw, RotatedRect(cv::Point2f(x, y), cv::Size2f(w, h), angle));

}
void ColorArea::SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels)
{
    try
    {
        // 1) Wrap dữ liệu managed bằng Mat với stride (step) tùy ý
        int type = (tplChannels == 1) ? CV_8UC1 :
            (tplChannels == 3) ? CV_8UC3 : CV_8UC4;

        Mat wrapped(tplH, tplW, type, tplData.ToPointer(), (size_t)tplStride);

        // 2) Convert sang 8U3 (BGR)
        Mat bgr;
        switch (tplChannels)
        {
        case 1:  cvtColor(wrapped, bgr, COLOR_GRAY2BGR); break; // 1 -> 3
        case 3:  bgr = wrapped; break;                           // đã 3 kênh
        case 4:  cvtColor(wrapped, bgr, COLOR_BGRA2BGR); break;  // 4 -> 3
        }
        _ColorPP->matRaw = bgr.clone();
    }
    catch (const cv::Exception& ex)
    {
        throw gcnew System::Exception(gcnew System::String(ex.what()));
    }
}
void ColorArea::SetImgeNoCrop(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels)
{
    try
    {
        // 1) Wrap dữ liệu managed bằng Mat với stride (step) tùy ý
        int type = (tplChannels == 1) ? CV_8UC1 :
            (tplChannels == 3) ? CV_8UC3 : CV_8UC4;

        Mat wrapped(tplH, tplW, type, tplData.ToPointer(), (size_t)tplStride);

        // 2) Convert sang 8U3 (BGR)
        Mat bgr;
        switch (tplChannels)
        {
        case 1:  cvtColor(wrapped, bgr, COLOR_GRAY2BGR); break; // 1 -> 3
        case 3:  bgr = wrapped; break;                           // đã 3 kênh
        case 4:  cvtColor(wrapped, bgr, COLOR_BGRA2BGR); break;  // 4 -> 3
        }
        _ColorPP->matCrop = bgr.clone();
    }
    catch (const cv::Exception& ex)
    {
        throw gcnew System::Exception(gcnew System::String(ex.what()));
    }
}
// Extraction: 0..100  (% mở rộng quanh tâm)
// Ví dụ Extraction=10 -> dH=18 (~10% của 180), dSV=26 (~10% của 255)
void ColorArea::SetTempHSV(cli::array<HSVCli^>^ listHSV, int Extraction)
{
    if (listHSV == nullptr || listHSV->Length == 0) return;

    _ColorPP->TypeColor = TypeColor::HSV;
    _ColorPP->Uppers.clear();
    _ColorPP->Lowers.clear();


    const int dH = ClampI((int)std::lround(Extraction * 1.8), 0, 180);
    const int dSV = ClampI((int)std::lround(Extraction * 2.55), 0, 255);

    for each (HSVCli ^ hsv in listHSV)
    {
        if (hsv == nullptr) continue;

        int H = ClampI(hsv->H, 0, 180);
        int S = ClampI(hsv->S, 0, 255);
        int V = ClampI(hsv->V, 0, 255);

        int S1 = ClampI(S - dSV, 0, 255);
        int S2 = ClampI(S + dSV, 0, 255);
        int V1 = ClampI(V - dSV, 0, 255);
        int V2 = ClampI(V + dSV, 0, 255);

        int H1 = H - dH, H2 = H + dH;

        if (H1 < 0 || H2 > 180)
        {
            int leftLoH = 0;
            int leftUpH = ClampI(H2, 0, 180);
            int rightLoH = ClampI(H1, 0, 180);
            int rightUpH = 180;

            _ColorPP->Lowers.push_back(cv::Scalar(rightLoH, S1, V1));
            _ColorPP->Uppers.push_back(cv::Scalar(rightUpH, S2, V2));

            _ColorPP->Lowers.push_back(cv::Scalar(leftLoH, S1, V1));
            _ColorPP->Uppers.push_back(cv::Scalar(leftUpH, S2, V2));
        }
        else
        {
            int loH = ClampI(H1, 0, 180);
            int upH = ClampI(H2, 0, 180);
            _ColorPP->Lowers.push_back(cv::Scalar(loH, S1, V1));
            _ColorPP->Uppers.push_back(cv::Scalar(upH, S2, V2));
        }
    }

}

void ColorArea::SetTempRGB(cli::array<RGBCli^>^ listRGB, int Extraction)
{
    _ColorPP->TypeColor = TypeColor::RGB;
    _ColorPP->Uppers.clear();
    _ColorPP->Lowers.clear();
    for each (RGBCli ^ RGB in listRGB)
    {
        int R = RGB->R - Extraction ;
        int G = RGB->G - Extraction ;
        int B = RGB->B - Extraction ;
        int R2 = RGB->R + Extraction ;
        int G2 = RGB->G + Extraction ;
        int B2 = RGB->B + Extraction ;
        Scalar lower = Scalar(R,G,B);
        Scalar upper = Scalar(R2, G2, B2);
        _ColorPP->Lowers.push_back(lower);
        _ColorPP->Uppers.push_back(upper);
      
    }
}
HSVCli^ ColorArea::GetHSV(int x,int y)
{
    auto HSV = gcnew HSVCli();
    try{
    
        Mat temp=Mat();
            cvtColor(_ColorPP->matRaw, temp, COLOR_BGR2HSV);
            Mat mat = temp(cv::Rect(x - 1, y - 1, 2, 2));
            int H = 0, S = 0, V = 0;
            for (int k = 0; k < mat.rows; k++)
            {
                for (int j = 0; j < mat.cols; j++)
                {
                    Vec3b color = mat.at<Vec3b>(k, j);
                    H += color[0];
                    S += color[1];
                    V += color[2];
                }
            }
            HSV->H = (int)H / 4;
            HSV->S = (int)S / 4;
            HSV->V = (int)V / 4;
            return HSV;
            }
            catch (...)
            {

            }

}
RGBCli^ ColorArea::GetRGB(int x, int y)
{
    auto RGB = gcnew RGBCli();
    try {

        Mat temp = Mat();
        cvtColor(_ColorPP->matRaw, temp, COLOR_BGR2RGB);
        Mat mat = temp(cv::Rect(x - 1, y - 1, 2, 2));
        int R = 0, G = 0, B= 0;
        for (int k = 0; k < mat.rows; k++)
        {
            for (int j = 0; j < mat.cols; j++)
            {
                Vec3b color = mat.at<Vec3b>(k, j);
                R += color[0];
                G+= color[1];
               B += color[2];
            }
        }
        RGB->R = (int)R / 4;
        RGB->G = (int)G / 4;
        RGB->B = (int)B / 4;
        return RGB;
    }
    catch (...)
    {

    }

}
// C++/CLI


static inline int clampi(int v, int lo, int hi) { return (v < lo) ? lo : (v > hi) ? hi : v; }
void ColorArea::FreeBuffer(System::IntPtr p)
{
    if (p != System::IntPtr::Zero)
       System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
}
System::IntPtr ColorArea::Check(
     int% outW,
     int% outH,
     int% outStride,
     int% outChannels)
{
    outW = outH = outStride = outChannels = 0;

    try
    {
        if (_ColorPP == nullptr) return IntPtr::Zero;
        if (_ColorPP->matCrop.empty()) return IntPtr::Zero;

        const cv::Mat& src = _ColorPP->matCrop;
        cv::Mat bgr, conv, mask, tmp, tmp2;

        // 1) Chuẩn hoá về BGR 8UC3
        if (src.type() == CV_8UC3)
        {
            bgr = src; // share
        }
        else
        {
            // Nếu 1 kênh 8-bit → Gray->BGR; nếu 16U/32F → scale về 8U trước
            if (src.depth() != CV_8U)
            {
                cv::Mat u8;
                cv::convertScaleAbs(src, u8);
                if (u8.channels() == 1) cv::cvtColor(u8, bgr, cv::COLOR_GRAY2BGR);
                else if (u8.channels() == 4) cv::cvtColor(u8, bgr, cv::COLOR_BGRA2BGR);
                else if (u8.channels() == 3) bgr = u8;
                else return IntPtr::Zero;
            }
            else
            {
                if (src.channels() == 1) cv::cvtColor(src, bgr, cv::COLOR_GRAY2BGR);
                else if (src.channels() == 4) cv::cvtColor(src, bgr, cv::COLOR_BGRA2BGR);
                else if (src.channels() == 3) bgr = src;
                else return IntPtr::Zero;
            }
        }

        const cv::Size sz = bgr.size();
        if (sz.width <= 0 || sz.height <= 0) return IntPtr::Zero;

        // 2) Convert màu 1 lần
        if (_ColorPP->TypeColor == TypeColor::RGB)
            cv::cvtColor(bgr, conv, cv::COLOR_BGR2RGB);
        else
            cv::cvtColor(bgr, conv, cv::COLOR_BGR2HSV);

        // 3) Tạo mask 8UC1 và tích luỹ OR theo từng dải
        mask.create(sz, CV_8UC1);
        mask.setTo(0);
        tmp.create(sz, CV_8UC1);
        tmp2.create(sz, CV_8UC1);

        const int nRanges = static_cast<int>(_ColorPP->Lowers.size());
        for (int i = 0; i < nRanges; ++i)
        {
            cv::Scalar lo = _ColorPP->Lowers[i];
            cv::Scalar hi = _ColorPP->Uppers[i];

            if (_ColorPP->TypeColor == TypeColor::HSV)
            {
                // Clamp S,V
                lo[1] = clampi((int)lo[1], 0, 255);
                lo[2] = clampi((int)lo[2], 0, 255);
                hi[1] = clampi((int)hi[1], 0, 255);
                hi[2] = clampi((int)hi[2], 0, 255);

                // Clamp H vào [0..179]
                int Hlo = clampi((int)lo[0], 0, 179);
                int Hhi = clampi((int)hi[0], 0, 179);

                if (Hlo > Hhi)
                {
                    // wrap-around: [0..Hhi] OR [Hlo..179]
                    cv::inRange(conv, cv::Scalar(0, lo[1], lo[2]),
                        cv::Scalar(Hhi, hi[1], hi[2]), tmp);
                    cv::inRange(conv, cv::Scalar(Hlo, lo[1], lo[2]),
                        cv::Scalar(179, hi[1], hi[2]), tmp2);
                    cv::bitwise_or(tmp, tmp2, tmp);
                }
                else
                {
                    cv::inRange(conv, cv::Scalar(Hlo, lo[1], lo[2]),
                        cv::Scalar(Hhi, hi[1], hi[2]), tmp);
                }
            }
            else
            {
                // RGB: clamp 0..255
                for (int k = 0; k < 3; ++k) {
                    lo[k] = clampi((int)lo[k], 0, 255);
                    hi[k] = clampi((int)hi[k], 0, 255);
                }
                cv::inRange(conv, lo, hi, tmp);
            }

            cv::bitwise_or(mask, tmp, mask);
        }

        // 4) (Tuỳ chọn) Morphology cuối (đỡ nhiễu)
        // if (_ColorPP->enableClose && _ColorPP->SizeClose > 1) {
        //     const int k = _ColorPP->SizeClose | 1; // số lẻ
        //     cv::Mat se = cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(k, k));
        //     cv::morphologyEx(mask, mask, cv::MORPH_CLOSE, se, cv::Point(-1,-1), 1);
        // }
        //cv::imwrite("mask.png", mask);
        // 5) Lưu vào matProcess (CV_8UC1)
        _ColorPP->matProcess = mask; // share
        if (!_ColorPP->matProcess.isContinuous())
            _ColorPP->matProcess = _ColorPP->matProcess.clone();

        const int W = _ColorPP->matProcess.cols;
        const int H = _ColorPP->matProcess.rows;
        const int C = _ColorPP->matProcess.channels();    // =1
        const int S = static_cast<int>(_ColorPP->matProcess.step);

        if (W <= 0 || H <= 0 || S <= 0) return IntPtr::Zero;

        const size_t bytes = static_cast<size_t>(S) * static_cast<size_t>(H);

        // 6) Cấp phát HGlobal + copy dữ liệu ra ngoài
        IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal(static_cast<IntPtr>((long long)bytes));
        if (mem == IntPtr::Zero) return IntPtr::Zero;

        std::memcpy(mem.ToPointer(), _ColorPP->matProcess.data, bytes);

        outW = W; outH = H; outStride = S; outChannels = C; // C=1
        return mem;
    }
    catch (...)
    {
        return IntPtr::Zero;
    }
}

//System::IntPtr ColorArea::Check(
//    [System::Runtime::InteropServices::Out] int% outW,
//    [System::Runtime::InteropServices::Out] int% outH,
//    [System::Runtime::InteropServices::Out] int% outStride,
//    [System::Runtime::InteropServices::Out] int% outChannels)
//{
//    cv::Mat _conv, _tmp, _tmp2;
//    Mat  matMask = Mat();
//    Size sz = _ColorPP->matCrop.size();
//    matMask.create(sz, CV_8UC1);
//    matMask.setTo(0);
//
//    // Chuyển màu 1 lần, KHÔNG dùng .clone()
//    cv::cvtColor(_ColorPP->matCrop, _conv,
//        (_ColorPP->TypeColor ==TypeColor::RGB) ? cv::COLOR_BGR2RGB : cv::COLOR_BGR2HSV);
//
//    // Tái sử dụng buffer tránh cấp phát vòng lặp
//    _tmp.create(sz, CV_8UC1);
//    _tmp2.create(sz, CV_8UC1);
//
//    // Với nhiều dải màu, OR tích luỹ trực tiếp vào matMask
//    for(int i=0;i<_ColorPP->Lowers.size();i++)
//    {
//        Scalar Lower = _ColorPP->Lowers[i];
//        Scalar Upper = _ColorPP->Uppers[i];
//        if (_ColorPP->TypeColor == TypeColor::HSV) {
//            // HSV: xử lý wrap-around Hue (khi lower.h > upper.h)
//            if (Lower[0] > Upper[0]) {
//                // [0..upper.h]  OR  [lower.h..179]
//                cv::inRange(_conv,
//                    cv::Scalar(0, Lower[1], Lower[2]),
//                    cv::Scalar(Upper[0], Upper[1], Upper[2]),
//                    _tmp);
//                cv::inRange(_conv,
//                    cv::Scalar(Lower[0], Lower[1], Lower[2]),
//                    cv::Scalar(179, Upper[1], Upper[2]),
//                    _tmp2);
//                cv::bitwise_or(_tmp, _tmp2, _tmp);
//            }
//            else {
//                cv::inRange(_conv, Lower, Upper, _tmp);
//            }
//        }
//        else {
//            // RGB
//            cv::inRange(_conv, Lower, Upper, _tmp);
//        }
//
//        // OR tích luỹ in-place, không tạo matGroup/clone
//        cv::bitwise_or(matMask, _tmp, matMask);
//    }
//    // Morphology 1 lần cuối (nhanh hơn nhiều so với mỗi dải màu làm 1 lần)
//  //  static const cv::Mat k3 = cv::getStructuringElement(cv::MORPH_ELLIPSE, { SizeClose ,SizeClose });
//    // Tương đương erode+ dilate nhẹ để khử nhiễu
//   // cv::morphologyEx(matMask, matMask, cv::MORPH_CLOSE, k3, { -1,-1 }, 1);
//   // NoneZeroResult = countNonZero(matMask);
//    if (matMask.type() == CV_8UC3)
//        cvtColor(matMask, matMask, COLOR_BGR2GRAY);
//    Mat mask = Mat(matMask.rows, matMask.cols, CV_8UC1, Scalar(255, 255, 255));
//    bitwise_and(mask, matMask, _ColorPP->matProcess);
//    if (!_ColorPP->matProcess.isContinuous()) _ColorPP->matProcess = _ColorPP->matProcess.clone();
//    const int W = _ColorPP->matProcess.cols, H = _ColorPP->matProcess.rows, C = _ColorPP->matProcess.channels();
//    const int S = (int)_ColorPP->matProcess.step;
//    const size_t bytes = (size_t)S * H;
//    IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal((IntPtr)(long long)bytes);
//    if (mem == IntPtr::Zero) return IntPtr::Zero;
//    std::memcpy(mem.ToPointer(), _ColorPP->matProcess.data, bytes);
//    outW = W; outH = H; outStride = S; outChannels = C;
//    return mem;
//}