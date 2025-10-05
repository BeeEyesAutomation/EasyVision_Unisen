#pragma once
#pragma once
#include <opencv2/opencv.hpp>
#include "Global.h"
using namespace cv;
using namespace std;
using namespace System;
namespace BeeCpp {


    public ref class HSVCli sealed {
    public:
        int H;
        int S;
        int V;
    };
    public ref class RGBCli sealed {
    public:
        int R;
        int G;
        int B;
    };
    public enum class TypeColor { HSV = 0, RGB = 1, BGR };

     class ColorAreaPP {

    public:  cv::Mat matProcess; cv::Mat matRaw; cv::Mat matCrop;
          TypeColor TypeColor;
          vector< Scalar> Lowers;
          vector< Scalar> Uppers;
          ColorAreaPP() {}
          ColorAreaPP(const cv::Mat& m) : matProcess(m.clone()) {}
    };
    public  ref class ColorArea
    {
    private:
        static int ClampI(int v, int lo, int hi)
        {
            return v < lo ? lo : (v > hi ? hi : v);
        }
    private:
        ColorAreaPP* _ColorPP;
        Common* com;
    public:
        void FreeBuffer(System::IntPtr p);
        void SetImgeRaw(IntPtr data, int width, int height, int stride, int channels);
        void SetImgeCrop(IntPtr data, int width, int height, int stride, int channels, RectRotateCli rr, Nullable<RectRotateCli> rrMask);
        void SetImgeNoCrop(IntPtr data, int width, int height, int stride, int channels);
        HSVCli^ GetHSV(int x, int y);
        RGBCli^ GetRGB(int x, int y);
        void SetTempHSV(cli::array<HSVCli^>^ listHSV, int Extraction);
        void SetTempRGB(cli::array<RGBCli^>^ listHSV, int Extraction);
        System::IntPtr Check(
            [System::Runtime::InteropServices::Out] int% outW,
            [System::Runtime::InteropServices::Out] int% outH,
            [System::Runtime::InteropServices::Out] int% outStride,
            [System::Runtime::InteropServices::Out] int% outChannels);
       
    public:
        ColorArea();
        ~ColorArea();
        !ColorArea();
    };
}
