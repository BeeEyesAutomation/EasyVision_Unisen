#pragma once
#include "PitchCore.h"
#using <System.Drawing.dll>

using namespace System;

namespace BeeCppCli {
    inline double ChooseDominantDelta(double a, double b) {
        if (!std::isfinite(a)) return b;
        if (!std::isfinite(b)) return a;
        return (std::fabs(a) >= std::fabs(b)) ? a : b;
    }
    public enum class ScanAxisCli { X = 0, Y = 1 };

    // Peak toạ độ pixel (để vẽ)
    public ref class PeakCli sealed {
    public:
        int X;
        int Y;
    };

    // Thông tin theo MỖI peak, trả về height/pitch theo mm (tọa độ vẽ vẫn px)
    public ref class PeakInfoCli sealed {
    public:
        // toạ độ pixel của peak
        int X;
        int Y;

        // toạ độ pixel của trung trục tại peak
        int CLX;
        int CLY;

        // khoảng cách đến trung trục (mm)
        double HeightMM;

        // pitch (mm) với peak cùng loại
        double PitchPrevMM; // mm tới peak trước (NaN nếu không có)
        double PitchNextMM; // mm tới peak sau   (NaN nếu không có)
    };

    public ref class PitchCliResult sealed {
    public:
        int Status;
        System:: String^ Message;

        // đơn vị
        double ScaleMmPerPx;

        // ===== Summary (MM only) =====
        // Crest pitch
        double PitchMedianMM;
        double PitchMeanMM;
        double PitchMinMM;
        double PitchMaxMM;
        double PitchStdMM;

        // Root pitch
        double PitchRootMedianMM;
        double PitchRootMeanMM;
        double PitchRootMinMM;
        double PitchRootMaxMM;
        double PitchRootStdMM;

        // Heights
        double CrestHMedianMM;
        double CrestHMeanMM;
        double CrestHMinMM;
        double CrestHMaxMM;
        double CrestHStdMM;

        double RootHMedianMM;
        double RootHMeanMM;
        double RootHMinMM;
        double RootHMaxMM;
        double RootHStdMM;

        int UsedMinPeakDist;
        double UsedMinProm;

        // (tuỳ chọn) danh sách peak pixel để vẽ
        array<PeakCli^>^ Crests;
        array<PeakCli^>^ Roots;

        // gộp per-peak (mm)
        array<PeakInfoCli^>^ CrestInfos;
        array<PeakInfoCli^>^ RootInfos;

        // (debug)
        array<PeakCli^>^ RejectedCrests;

        // Ảnh debug BGR8 (malloc). Gọi FreeBuffer để giải phóng.
        IntPtr DebugPtr;
        int DebugW, DebugH, DebugStride, DebugChannels;
    };

    public ref class PitchCli sealed {
    public:
        PitchCli();
        ~PitchCli();

        // Ảnh input: CV_8UC1
        void SetImage(IntPtr data, int width, int height, int stride, int channels);

        // Margin X/Y
        void SetMargins(int marginX, int marginY);

        // Promote rejected crest theo khoảng cách
        void SetRejectedPolicy(bool enablePromote, int minNeighborDist);

        // Chọn trục quét
        void SetScanAxis(ScanAxisCli axis);

        // Đặt sigma Gaussian
        void SetGaussianSigma(double signalSigma, double centerlineFallbackSigma);

        // Đặt scale mm/px
        void SetScaleMmPerPx(double mmPerPx);

        PitchCliResult^ Measure();

        static void FreeBuffer(IntPtr p);

    private:
        BeeCpp::PitchCore* _core;
    };

}
