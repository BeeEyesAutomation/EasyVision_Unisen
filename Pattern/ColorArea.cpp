#include "ColorArea.h"

#include <cstring>
#include <cmath>
#include <limits>
using namespace cv;
using namespace std;
using namespace System;
using namespace BeeCpp;

using namespace System::Runtime::InteropServices;
ColorArea::ColorArea() : _ColorPP(new ColorAreaPP()) {}
ColorArea::~ColorArea() { this->!ColorArea(); }
ColorArea::!ColorArea() { if (_ColorPP) { delete _ColorPP; _ColorPP = nullptr; } }
void ColorArea::SetImgeCrop(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, float x, float y, float w, float h, float angle)
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

        // 3) Xoay/cắt theo RotatedRect (giả định Common::RotateMat trả Mat mới)
        RotatedRect rr(Point2f(x, y), Size2f(w, h), angle);
        Mat rotatedRoi = Common::RotateMat(bgr, rr);

        // 4) Clone để sở hữu bộ nhớ độc lập khỏi buffer .NET
        _ColorPP->matCrop = rotatedRoi.clone();   // đảm bảo CV_8UC3
        // Nếu Common::RotateMat có thể thay đổi type, bạn có thể ép lại:
        // if (_ColorPP->matRaw.type() != CV_8UC3)
        //     cvtColor(_ColorPP->matRaw, _ColorPP->matRaw, COLOR_BGRA2BGR);
    }
    catch (const cv::Exception& ex)
    {
        throw gcnew System::Exception(gcnew System::String(ex.what()));
    }
	Mat raw(tplH, tplW, CV_8UC3, tplData.ToPointer(), tplStride);
	_ColorPP->matRaw = Common::RotateMat(raw, RotatedRect(cv::Point2f(x, y), cv::Size2f(w, h), angle));

}void ColorArea::SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels)
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
System::IntPtr ColorArea::Check(
    [System::Runtime::InteropServices::Out] int% outW,
    [System::Runtime::InteropServices::Out] int% outH,
    [System::Runtime::InteropServices::Out] int% outStride,
    [System::Runtime::InteropServices::Out] int% outChannels)
{
    cv::Mat _conv, _tmp, _tmp2;
    Mat  matMask = Mat();
    Size sz = _ColorPP->matCrop.size();
    matMask.create(sz, CV_8UC1);
    matMask.setTo(0);

    // Chuyển màu 1 lần, KHÔNG dùng .clone()
    cv::cvtColor(_ColorPP->matCrop, _conv,
        (_ColorPP->TypeColor ==TypeColor::RGB) ? cv::COLOR_BGR2RGB : cv::COLOR_BGR2HSV);

    // Tái sử dụng buffer tránh cấp phát vòng lặp
    _tmp.create(sz, CV_8UC1);
    _tmp2.create(sz, CV_8UC1);

    // Với nhiều dải màu, OR tích luỹ trực tiếp vào matMask
    for(int i=0;i<_ColorPP->Lowers.size();i++)
    {
        Scalar Lower = _ColorPP->Lowers[i];
        Scalar Upper = _ColorPP->Uppers[i];
        if (_ColorPP->TypeColor == TypeColor::HSV) {
            // HSV: xử lý wrap-around Hue (khi lower.h > upper.h)
            if (Lower[0] > Upper[0]) {
                // [0..upper.h]  OR  [lower.h..179]
                cv::inRange(_conv,
                    cv::Scalar(0, Lower[1], Lower[2]),
                    cv::Scalar(Upper[0], Upper[1], Upper[2]),
                    _tmp);
                cv::inRange(_conv,
                    cv::Scalar(Lower[0], Lower[1], Lower[2]),
                    cv::Scalar(179, Upper[1], Upper[2]),
                    _tmp2);
                cv::bitwise_or(_tmp, _tmp2, _tmp);
            }
            else {
                cv::inRange(_conv, Lower, Upper, _tmp);
            }
        }
        else {
            // RGB
            cv::inRange(_conv, Lower, Upper, _tmp);
        }

        // OR tích luỹ in-place, không tạo matGroup/clone
        cv::bitwise_or(matMask, _tmp, matMask);
    }
    // Morphology 1 lần cuối (nhanh hơn nhiều so với mỗi dải màu làm 1 lần)
  //  static const cv::Mat k3 = cv::getStructuringElement(cv::MORPH_ELLIPSE, { SizeClose ,SizeClose });
    // Tương đương erode+ dilate nhẹ để khử nhiễu
   // cv::morphologyEx(matMask, matMask, cv::MORPH_CLOSE, k3, { -1,-1 }, 1);
   // NoneZeroResult = countNonZero(matMask);
    if (matMask.type() == CV_8UC3)
        cvtColor(matMask, matMask, COLOR_BGR2GRAY);
    Mat mask = Mat(matMask.rows, matMask.cols, CV_8UC1, Scalar(255, 255, 255));
    bitwise_and(mask, matMask, _ColorPP->matProcess);
    if (!_ColorPP->matProcess.isContinuous()) _ColorPP->matProcess = _ColorPP->matProcess.clone();
    const int W = _ColorPP->matProcess.cols, H = _ColorPP->matProcess.rows, C = _ColorPP->matProcess.channels();
    const int S = (int)_ColorPP->matProcess.step;
    const size_t bytes = (size_t)S * H;
    IntPtr mem = Marshal::AllocHGlobal((IntPtr)(long long)bytes);
    if (mem == IntPtr::Zero) return IntPtr::Zero;
    std::memcpy(mem.ToPointer(), _ColorPP->matProcess.data, bytes);
    outW = W; outH = H; outStride = S; outChannels = C;
    return mem;
}