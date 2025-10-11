
#pragma once
#include "Global.h"
#include "BarcodeCore.h"

using namespace System;
using namespace System::Collections::Generic;

namespace BeeCpp {

    // Map gần 1-1 với Symbology (giữ nguyên thứ tự để cast trực tiếp)
    public enum class CodeSymbologyCli
    {
        Unknown = 0,
        EAN8, EAN13, UPC_A, UPC_E,
        Code39, Code93, Code128, ITF,
        QR, DataMatrix, PDF417, Aztec,
        MaxiCode, Codabar, MicroQR, MicroPDF417, RSS14, RSSExpanded
    };

    // Tham số filter cho 1D (mặc định đúng như bạn đang set)
    public value struct FilterParamsCli {
        int   MinArea;
    
        int   CloseKernelWDiv;
        int   CloseKernelH;
        bool  UseNoRotateMask;
        static FilterParamsCli Defaults() {
            FilterParamsCli f;
            f.MinArea = 700;  f.CloseKernelWDiv = 15; f.CloseKernelH = 3; f.UseNoRotateMask = true;
            return f;
        }
    };

    // Tuỳ chọn detect
    public value struct DetectOptionsCli {
        bool        EnablePreprocess;
        bool        FindBoxes;      // thay cho tham số FindBox rời
        bool        CropFirst;
        bool        DebugDraw;
        bool        DebugSave;
        System::String^ DebugDir;
        FilterParamsCli Filter;
        static DetectOptionsCli Defaults() {
            DetectOptionsCli o;
            o.EnablePreprocess = false;
            o.FindBoxes = true;
            o.CropFirst = true;
            o.DebugDraw = false;
            o.DebugSave = false;
            o.DebugDir = gcnew System::String("");
            o.Filter = FilterParamsCli::Defaults();
            return o;
        }
    };

    public ref class BarcodeCoreCli
    {
    public:
        BarcodeCoreCli();
        ~BarcodeCoreCli();
        !BarcodeCoreCli();

        // Detect nhiều mã trong ROI rr (có thể có rrMask). Output qua % (tracking reference)
        void DetectAll(
            IntPtr data, int width, int height, int stride, int channels,
            RectRotateCli rr, Nullable<RectRotateCli> rrMask,
            DetectOptionsCli opts,
            List<RectRotateCli>^% rects,
            List<System::String^>^% payloads,
            List<CodeSymbologyCli>^% types
        );

        
        // Copy dữ liệu CV_8U (grayscale) từ cv::Mat* về buffer managed (stride tuỳ ý).
        // Trả false nếu type không phải CV_8UC1 hoặc kích thước không khớp.
        bool ReadMatGray8(IntPtr matPtr, IntPtr dstData, int dstStride, int width, int height);

        // Giải phóng cv::Mat* đã trả về từ GetMask1D
        void FreeMat(IntPtr matPtr);

    private:
        BeeCpp::BarcodeCore* _core;
        CommonPlus* com;
        static PointF32 ToPtF32(float x, float y);
        static void WorldPolygonToLocalUnrotated(
            const std::vector<cv::Point2f>& worldPts,
            const cv::Point2f& center,
            float angleDeg,
            cli::array<PointF32>^% outLocal
        );

        static BeeCpp::FilterParams ToNative(FilterParamsCli c);
        static BeeCpp::DetectOptions ToNative(DetectOptionsCli c);
    };

} // namespace BeeCpp
