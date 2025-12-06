#pragma once
#include <opencv2/opencv.hpp>
#include "Global.h"
using namespace System;

namespace BeeCpp
{
    class ColorPx {
    
    public:  cv::Mat temp; cv::Mat raw;
        
        ColorPx() {}
        ColorPx(const cv::Mat& m) : temp(m.clone()) {}
    };
    public ref class ColorPixel sealed
    {
    private:CommonPlus* com = new CommonPlus();
    private:ColorPx* _img = new ColorPx();   // native pointer
    public:
        // So khớp ảnh (BGR8) từ bytes nén (PNG/JPG), trả RAW BGR (AllocHGlobal).
        System::IntPtr CheckImageFromBytes(
            array<System::Byte>^ imageBytes,
            array<System::Byte>^ templateBytes,
            int colorTolerance,
            [System::Runtime::InteropServices::Out] float% PxOut,
            [System::Runtime::InteropServices::Out] int% outW,
            [System::Runtime::InteropServices::Out] int% outH,
            [System::Runtime::InteropServices::Out] int% outStride,
            [System::Runtime::InteropServices::Out] int% outChannels
           );
        //
     //   System::IntPtr imgData, int imgW, int imgH, int imgStride, int imgChannels,
        // So khớp trực tiếp từ Mat (OpenCvSharp) — data pointer + w/h/stride/channels.
        System::IntPtr CheckImageFromMat(int colorTolerance,
            [System::Runtime::InteropServices::Out] float% PxOut,
            float% outOffsetX, float% outOffsetY, float% Offsetangle,
            [System::Runtime::InteropServices::Out] int% outW,
            [System::Runtime::InteropServices::Out] int% outH,
            [System::Runtime::InteropServices::Out] int% outStride,
            [System::Runtime::InteropServices::Out] int% outChannels);
        void  SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels , RectRotateCli rr, Nullable<RectRotateCli> rrMask);
        System::IntPtr SetImgeSample(IntPtr data, int w, int h, int stride, int ch
            , RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool NoCrop,
            [System::Runtime::InteropServices::Out] int% outW,
            [System::Runtime::InteropServices::Out] int% outH,
            [System::Runtime::InteropServices::Out] int% outStride,
            [System::Runtime::InteropServices::Out] int% outChannels);
        // Giải phóng buffer RAW trả về.
        static void FreeBuffer(System::IntPtr p);
    };
}
