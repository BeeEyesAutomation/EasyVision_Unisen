#include "ColorMaskWrapper.h"
using namespace BeeCV;

// ===== Native impl =====
int ColorMaskNative::clampi(int v, int lo, int hi) { return v < lo ? lo : (v > hi ? hi : v); }

HSVRangeNative ColorMaskNative::makeRange(unsigned char H, unsigned char S, unsigned char V, int percent)
{
    HSVRangeNative r{};
    int p = clampi(percent, 0, 100);
    int dH = (180 * p) / 100;
    int dSV = (255 * p) / 100;

    int Hc = clampi((int)H, 0, 179);
    int Sc = clampi((int)S, 0, 255);
    int Vc = clampi((int)V, 0, 255);

    int lh = Hc - dH, uh = Hc + dH;
    int ls = clampi(Sc - dSV, 0, 255);
    int us = clampi(Sc + dSV, 0, 255);
    int lv = clampi(Vc - dSV, 0, 255);
    int uv = clampi(Vc + dSV, 0, 255);

    bool wrap = (lh < 0 || uh > 179);
    if (wrap) {
        lh = (lh % 180 + 180) % 180;
        uh = (uh % 180 + 180) % 180;
    }
    else {
        lh = clampi(lh, 0, 179);
        uh = clampi(uh, 0, 179);
    }

    r.lh = (unsigned char)lh; r.uh = (unsigned char)uh;
    r.ls = (unsigned char)ls; r.us = (unsigned char)us;
    r.lv = (unsigned char)lv; r.uv = (unsigned char)uv;
    r.wrapH = wrap;
    return r;
}

void ColorMaskNative::loadBGR(void* scan0, int width, int height, int strideBytes)
{
    w = width; h = height; step = strideBytes;
    cv::Mat hdr(h, w, CV_8UC3, scan0, step);
    bgr = hdr.clone();            // safe
   
    hsvDirty = true;
}

void ColorMaskNative::ensureHSV()
{
    if (!useHSV) return;
    if (hsvDirty || hsv.empty()) {
        cv::cvtColor(bgr, hsv, cv::COLOR_BGR2HSV);
        hsvDirty = false;
    }
}

cv::Vec3b ColorMaskNative::getBgrAt(int x, int y) const
{
    x = clampi(x, 0, w - 1); y = clampi(y, 0, h - 1);
    return bgr.empty() ? cv::Vec3b() : bgr.at<cv::Vec3b>(y, x);
}

cv::Vec3b ColorMaskNative::getHsvAt(int x, int y)
{
    ensureHSV();
    x = clampi(x, 0, w - 1); y = clampi(y, 0, h - 1);
    return hsv.empty() ? cv::Vec3b() : hsv.at<cv::Vec3b>(y, x);
}

void ColorMaskNative::setStyle(bool useHSV_) { useHSV = useHSV_; }
void ColorMaskNative::setSamples(const std::vector<HSVRangeNative>& rs) { ranges = rs; }

int ColorMaskNative::buildMaskAndCountWhite(bool morphOpen)
{
    const cv::Mat& src = useHSV ? (ensureHSV(), hsv) : bgr;
    if (src.empty()) return 0;

    mask.create(src.size(), CV_8U);
    mask.setTo(0);
    tmp.create(src.size(), CV_8U);
    tmp2.create(src.size(), CV_8U);

    for (const auto& r : ranges) {
        if (useHSV && r.wrapH) {
            cv::inRange(src, cv::Scalar(0, r.ls, r.lv),
                cv::Scalar(r.uh, r.us, r.uv), tmp);
            cv::inRange(src, cv::Scalar(r.lh, r.ls, r.lv),
                cv::Scalar(179, r.us, r.uv), tmp2);
            cv::bitwise_or(tmp, tmp2, tmp);
        }
        else {
            cv::inRange(src, cv::Scalar(r.lh, r.ls, r.lv),
                cv::Scalar(r.uh, r.us, r.uv), tmp);
        }
        cv::bitwise_or(mask, tmp, mask);
    }

    if (morphOpen) {
        static const cv::Mat k3 = cv::getStructuringElement(cv::MORPH_RECT, { 3,3 });
        cv::morphologyEx(mask, mask, cv::MORPH_OPEN, k3);
    } cv::imwrite("mask.png", mask);
    return cv::countNonZero(mask);
}

void ColorMaskNative::getMaskBytes(std::vector<unsigned char>& out) const
{
    out.resize((size_t)w * (size_t)h);
    if (mask.empty()) { std::fill(out.begin(), out.end(), 0); return; }

    if (mask.isContinuous()) {
        memcpy(out.data(), mask.data, (size_t)w * (size_t)h);
    }
    else {
        for (int y = 0; y < h; ++y) {
            memcpy(out.data() + (size_t)y * w, mask.ptr(y), (size_t)w);
        }
    }
}

// ===== Managed wrapper =====
ColorMaskWrapper::ColorMaskWrapper() { _p = new ColorMaskNative(); }
ColorMaskWrapper::~ColorMaskWrapper() { this->!ColorMaskWrapper(); }
ColorMaskWrapper::!ColorMaskWrapper() { if (_p) { delete _p; _p = nullptr; } }

void ColorMaskWrapper::LoadBitmap(IntPtr scan0, int width, int height, int strideBytes)
{
    _p->loadBGR(scan0.ToPointer(), width, height, strideBytes);
}
void ColorMaskWrapper::LoadBgrBuffer(IntPtr ptr, int width, int height)
{
    _p->loadBGR(ptr.ToPointer(), width, height, width * 3);
}

Color ColorMaskWrapper::GetBgrAt(int x, int y)
{
    auto v = _p->getBgrAt(x, y); // B,G,R
    return Color::FromArgb(v[2], v[1], v[0]); // R,G,B
}
Color ColorMaskWrapper::GetHsvAt(int x, int y)
{
    auto v = _p->getHsvAt(x, y); // H,S,V
    return Color::FromArgb(v[0], v[1], v[2]); // trả H,S,V vào R,G,B
}

void ColorMaskWrapper::SetStyleColor(bool useHSV) { _p->setStyle(useHSV); }

void ColorMaskWrapper::SetSamples(cli::array<HsvSample>^ samples, int areaPercent)
{
    std::vector<HSVRangeNative> v;
    if (samples && samples->Length) {
        v.reserve(samples->Length);
        for (int i = 0; i < samples->Length; ++i) {
            auto s = samples[i];
            v.push_back(ColorMaskNative::makeRange(s.H, s.S, s.V, areaPercent));
        }
    }
    _p->setSamples(v);
}

int ColorMaskWrapper::BuildMaskAndCountWhite(bool morphOpen)
{
    return _p->buildMaskAndCountWhite(morphOpen);
}

cli::array<System::Byte>^ ColorMaskWrapper::GetMask()
{
    std::vector<unsigned char> buf;
    _p->getMaskBytes(buf);
    auto arr = gcnew cli::array<System::Byte>((int)buf.size());
    if (!buf.empty())
        System::Runtime::InteropServices::Marshal::Copy(
            (IntPtr)buf.data(), arr, 0, (int)buf.size());
    return arr;
}

int ColorMaskWrapper::Width::get() { return _p->w; }
int ColorMaskWrapper::Height::get() { return _p->h; }
