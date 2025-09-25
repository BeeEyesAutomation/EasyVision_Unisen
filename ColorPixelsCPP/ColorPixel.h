#pragma once
#include <opencv2/opencv.hpp>

using namespace System;

namespace ColorPixels
{
    class Img {
    public:
        cv::Mat temp; cv::Mat raw;
       
        Img() {}
        Img(const cv::Mat& m) : temp(m.clone()) {}
    };
    public ref class ColorPixel sealed
    {
    private:Img* _img = new Img();   // native pointer
    public:
        // So khớp ảnh (BGR8) từ bytes nén (PNG/JPG), trả RAW BGR (AllocHGlobal).
        System::IntPtr CheckImageFromBytes(
            array<System::Byte>^ imageBytes,
            array<System::Byte>^ templateBytes,
            int maxDiffPixels,
            int colorTolerance,
            [System::Runtime::InteropServices::Out] int% outW,
            [System::Runtime::InteropServices::Out] int% outH,
            [System::Runtime::InteropServices::Out] int% outStride,
            [System::Runtime::InteropServices::Out] int% outChannels,
            [System::Runtime::InteropServices::Out] bool% pass,
            [System::Runtime::InteropServices::Out] double% cycleTimeMs);
        //
     //   System::IntPtr imgData, int imgW, int imgH, int imgStride, int imgChannels,
        // So khớp trực tiếp từ Mat (OpenCvSharp) — data pointer + w/h/stride/channels.
        System::IntPtr CheckImageFromMat(
            
            int maxDiffPixels, int colorTolerance,
            [System::Runtime::InteropServices::Out] bool% pass,
            [System::Runtime::InteropServices::Out] double% cycleTimeMs,
            [System::Runtime::InteropServices::Out] int% outW,
            [System::Runtime::InteropServices::Out] int% outH,
            [System::Runtime::InteropServices::Out] int% outStride,
            [System::Runtime::InteropServices::Out] int% outChannels);
        void  SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, float x, float y, float w, float h, float angle);
        void  SetImgeSample(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels);
        // Giải phóng buffer RAW trả về.
        static void FreeBuffer(System::IntPtr p);
    };
}
