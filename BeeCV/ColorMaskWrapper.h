#pragma once
#include"G.h"
#include <vector>

using namespace System;
using namespace System::Drawing;

#pragma managed(push, off)

// ===== Native part (không-managed) =====
struct HSVRangeNative {
    unsigned char lh, ls, lv, uh, us, uv;
    bool wrapH;
};

struct ColorMaskNative {
    cv::Mat bgr, hsv, mask, tmp, tmp2;
    std::vector<HSVRangeNative> ranges;
    bool useHSV = true;
    int  w = 0, h = 0, step = 0;
    bool hsvDirty = true;

    static int clampi(int v, int lo, int hi);
    static HSVRangeNative makeRange(unsigned char H, unsigned char S, unsigned char V, int percent);

    void loadBGR(void* scan0, int width, int height, int strideBytes);
    void ensureHSV();
    cv::Vec3b getBgrAt(int x, int y) const;
    cv::Vec3b getHsvAt(int x, int y);
    void setStyle(bool useHSV_);
    void setSamples(const std::vector<HSVRangeNative>& rs);
    int  buildMaskAndCountWhite(bool morphOpen);
    void getMaskBytes(std::vector<unsigned char>& out) const;
};

#pragma managed(pop)

namespace BeeCV {

    public value struct HsvSample { System::Byte H; System::Byte S; System::Byte V; };

    public ref class ColorMaskWrapper
    {
    public:
        ColorMaskWrapper();
        ~ColorMaskWrapper();
        !ColorMaskWrapper();

        void LoadBitmap(IntPtr scan0, int width, int height, int strideBytes);
        void LoadBgrBuffer(IntPtr ptr, int width, int height); // stride = width*3

        Color GetBgrAt(int x, int y);
        Color GetHsvAt(int x, int y);

        void SetStyleColor(bool useHSV);              // true=HSV, false=RGB (chỉ HSV dùng ranges dưới)
        void SetSamples(cli::array<HsvSample>^ samples, int areaPercent);

        int  BuildMaskAndCountWhite(bool morphOpen);
        cli::array<System::Byte>^ GetMask();

        property int Width { int get(); }
        property int Height { int get(); }

    private:
        ColorMaskNative* _p; // native pimpl
    };
}