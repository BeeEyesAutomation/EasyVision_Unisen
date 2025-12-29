#include "PitchCli.h"
#include <cstring>
#include <cmath>
#include <limits>

using namespace System;
using namespace BeeCppCli;

static array<PeakCli^>^ ToManagedPeaks(const std::vector<BeeCpp::Peak>& v) {
    auto a = gcnew array<PeakCli^>((int)v.size());
    for (int i = 0; i < (int)v.size(); ++i) {
        auto p = gcnew PeakCli();
        p->X = v[i].x; p->Y = v[i].y;
        a[i] = p;
    }
    return a;
}

static double dnan() { return std::numeric_limits<double>::quiet_NaN(); }

PitchCli::PitchCli() : _core(new BeeCpp::PitchCore()) {}
PitchCli::~PitchCli() { delete _core; _core = nullptr; }

void PitchCli::SetImage(IntPtr data, int width, int height, int stride, int channels) {
    cv::Mat tmp(height, width, CV_8UC1, data.ToPointer(), (size_t)stride);
    cv::Mat gray; tmp.copyTo(gray);
    _core->setImage(gray);
}

void PitchCli::SetMargins(int marginX, int marginY) {
    _core->setMargins(marginX, marginY);
}

void PitchCli::SetRejectedPolicy(bool enablePromote, int minNeighborDist) {
    _core->setRejectedPolicy(enablePromote, minNeighborDist);
}

void PitchCli::SetScanAxis(ScanAxisCli axis) {
    _core->setScanAxis(axis == ScanAxisCli::Y ? BeeCpp::ScanAxis::Y : BeeCpp::ScanAxis::X);
}

void PitchCli::SetGaussianSigma(double signalSigma, double centerlineFallbackSigma) {
    _core->setGaussianSigma(signalSigma, centerlineFallbackSigma);
}

void PitchCli::SetScaleMmPerPx(double mmPerPx) {
    _core->setScaleMmPerPx(mmPerPx);
}

PitchCliResult^ PitchCli::Measure() {
    auto R = _core->measure();

    auto M = gcnew PitchCliResult();
    M->Status = R.status;
    M->Message = gcnew System::String(R.message.c_str());

    // scale
    M->ScaleMmPerPx = R.scale_mm_per_px;

    // ===== Summary MM only =====
    M->PitchMeanMM = R.pitch_mean_mm;
    M->PitchMedianMM = R.pitch_median_mm;
    M->PitchMinMM = R.pitch_min_mm;
    M->PitchMaxMM = R.pitch_max_mm;
    M->PitchStdMM = R.pitch_std_mm;

    M->PitchRootMeanMM = R.pitch_root_mean_mm;
    M->PitchRootMedianMM = R.pitch_root_median_mm;
    M->PitchRootMinMM = R.pitch_root_min_mm;
    M->PitchRootMaxMM = R.pitch_root_max_mm;
    M->PitchRootStdMM = R.pitch_root_std_mm;

    M->CrestHMeanMM = R.crest_h_mean_mm;
    M->CrestHMedianMM = R.crest_h_med_mm;
    M->CrestHMinMM = R.crest_h_min_mm;
    M->CrestHMaxMM = R.crest_h_max_mm;
    M->CrestHStdMM = R.crest_h_std_mm;

    M->RootHMeanMM = R.root_h_mean_mm;
    M->RootHMedianMM = R.root_h_med_mm;
    M->RootHMinMM = R.root_h_min_mm;
    M->RootHMaxMM = R.root_h_max_mm;
    M->RootHStdMM = R.root_h_std_mm;

    M->UsedMinPeakDist = R.usedMinPeakDist;
    M->UsedMinProm = R.usedMinProm;

    // (tuỳ chọn) peaks pixel để vẽ
    M->Crests = ToManagedPeaks(R.crests);
    M->Roots = ToManagedPeaks(R.roots);
    M->RejectedCrests = ToManagedPeaks(R.rejected_crests);

    // ===== Per-peak info (mm) =====
    {
        int n = (int)R.crests.size();
        M->CrestInfos = gcnew array<PeakInfoCli^>(n);
        for (int i = 0; i < n; ++i) {
            auto info = gcnew PeakInfoCli();
            info->X = R.crests[i].x;
            info->Y = R.crests[i].y;

            int    clx = (i < (int)R.crest_cl_x.size() ? R.crest_cl_x[i] : info->X);
            double cly = (i < (int)R.crest_cl_y.size() ? R.crest_cl_y[i] : (double)info->Y);
            info->CLX = clx;
            info->CLY = (int)std::lround(cly);

            // height (mm)
            double hpx = (i < (int)R.crest_heights.size() ? R.crest_heights[i] : 0.0);
            info->HeightMM = hpx * R.scale_mm_per_px;

            // pitch prev/next (mm) — chọn trục chi phối (ΔX hoặc ΔY lớn hơn)
            double dXprev = (i > 0) ? double(R.crests[i].x - R.crests[i - 1].x) : std::numeric_limits<double>::quiet_NaN();
            double dYprev = (i > 0) ? double(R.crests[i].y - R.crests[i - 1].y) : std::numeric_limits<double>::quiet_NaN();
            double dXnext = (i < n - 1) ? double(R.crests[i + 1].x - R.crests[i].x) : std::numeric_limits<double>::quiet_NaN();
            double dYnext = (i < n - 1) ? double(R.crests[i + 1].y - R.crests[i].y) : std::numeric_limits<double>::quiet_NaN();
            double prevPx = ChooseDominantDelta(dXprev, dYprev);
            double nextPx = ChooseDominantDelta(dXnext, dYnext);
            info->PitchPrevMM = std::isfinite(prevPx) ? prevPx * R.scale_mm_per_px : std::numeric_limits<double>::quiet_NaN();
            info->PitchNextMM = std::isfinite(nextPx) ? nextPx * R.scale_mm_per_px : std::numeric_limits<double>::quiet_NaN();
            M->CrestInfos[i] = info;
        }
    }
    {
        int n = (int)R.roots.size();
        M->RootInfos = gcnew array<PeakInfoCli^>(n);
        for (int i = 0; i < n; ++i) {
            auto info = gcnew PeakInfoCli();
            info->X = R.roots[i].x;
            info->Y = R.roots[i].y;

            int    clx = (i < (int)R.root_cl_x.size() ? R.root_cl_x[i] : info->X);
            double cly = (i < (int)R.root_cl_y.size() ? R.root_cl_y[i] : (double)info->Y);
            info->CLX = clx;
            info->CLY = (int)std::lround(cly);

            // height (mm)
            double hpx = (i < (int)R.root_heights.size() ? R.root_heights[i] : 0.0);
            info->HeightMM = hpx * R.scale_mm_per_px;

            // pitch prev/next (mm)
            double dXprev = (i > 0) ? double(R.roots[i].x - R.roots[i - 1].x) : std::numeric_limits<double>::quiet_NaN();
            double dYprev = (i > 0) ? double(R.roots[i].y - R.roots[i - 1].y) : std::numeric_limits<double>::quiet_NaN();
            double dXnext = (i < n - 1) ? double(R.roots[i + 1].x - R.roots[i].x) : std::numeric_limits<double>::quiet_NaN();
            double dYnext = (i < n - 1) ? double(R.roots[i + 1].y - R.roots[i].y) : std::numeric_limits<double>::quiet_NaN();

            double prevPx = ChooseDominantDelta(dXprev, dYprev);
            double nextPx = ChooseDominantDelta(dXnext, dYnext);

            info->PitchPrevMM = std::isfinite(prevPx) ? prevPx * R.scale_mm_per_px : std::numeric_limits<double>::quiet_NaN();
            info->PitchNextMM = std::isfinite(nextPx) ? nextPx * R.scale_mm_per_px : std::numeric_limits<double>::quiet_NaN();

            M->RootInfos[i] = info;
        }
    }

    // ===== Debug image buffer (BGR8) =====
    if (!R.debugBGR.empty()) {
        const int W = R.debugBGR.cols, H = R.debugBGR.rows, C = R.debugBGR.channels();
        const int S = (int)R.debugBGR.step;
        size_t bytes = (size_t)S * H;
        unsigned char* buf = (unsigned char*)::operator new[](bytes);
        std::memcpy(buf, R.debugBGR.data, bytes);
        M->DebugPtr = IntPtr((void*)buf);
        M->DebugW = W; M->DebugH = H; M->DebugStride = S; M->DebugChannels = C;
    }
    else {
        M->DebugPtr = IntPtr::Zero;
        M->DebugW = M->DebugH = M->DebugStride = M->DebugChannels = 0;
    }

    return M;
}


void PitchCli::FreeBuffer(IntPtr p) {
    if (p != IntPtr::Zero) {
        void* ptr = p.ToPointer();
        ::operator delete[](ptr);
    }
}
