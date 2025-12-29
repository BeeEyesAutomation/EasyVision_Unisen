
#include "BarcodeCli.h"
#include <opencv2/core.hpp>


using namespace cv;

namespace BeeCpp {

    static inline float Deg2Rad(float d) { return d * 3.14159265358979323846f / 180.f; }
    BarcodeCoreCli::BarcodeCoreCli() { _core = new BeeCpp::BarcodeCore(); com = new BeeCpp::CommonPlus(); }
    BarcodeCoreCli::~BarcodeCoreCli() { this->!BarcodeCoreCli(); }
    BarcodeCoreCli::!BarcodeCoreCli() { if (_core) { delete _core; _core = nullptr; } if (com) { delete com; com = nullptr; } }

    PointF32 BarcodeCoreCli::ToPtF32(float x, float y) { PointF32 p; p.X = x; p.Y = y; return p; }

    void BarcodeCoreCli::WorldPolygonToLocalUnrotated(
        const std::vector<cv::Point2f>& worldPts,
        const cv::Point2f& center,
        float angleDeg,
        cli::array<PointF32>^% outLocal)
    {
        int n = (int)worldPts.size();
        outLocal = gcnew cli::array<PointF32>(n);
        float a = Deg2Rad(-angleDeg);
        float c = std::cos(a), s = std::sin(a);
        for (int i = 0; i < n; ++i) {
            float dx = worldPts[i].x - center.x;
            float dy = worldPts[i].y - center.y;
            float lx = dx * c - dy * s;
            float ly = dx * s + dy * c;
            outLocal[i] = ToPtF32(lx, ly);
        }
    }

    // mapping trực tiếp theo thứ tự enum
    static inline BeeCpp::CodeSymbologyCli MapCli(BeeCpp::Symbology s)
    {
        return static_cast<BeeCpp::CodeSymbologyCli>(static_cast<int>(s));
    }

    BeeCpp::FilterParams BarcodeCoreCli::ToNative(FilterParamsCli c)
    {
        BeeCpp::FilterParams f;
        f.MinArea = c.MinArea;
      
        f.CloseKernelWDiv = c.CloseKernelWDiv;
        f.CloseKernelH = c.CloseKernelH;
        f.UseNoRotateMask = c.UseNoRotateMask;
        return f;
    }

    BeeCpp::DetectOptions BarcodeCoreCli::ToNative(DetectOptionsCli c)
    {
        BeeCpp::DetectOptions o;
        o.enablePreprocess = c.EnablePreprocess;
        o.findBoxes = c.FindBoxes;
        o.cropFirst = c.CropFirst;
        o.debugDraw = c.DebugDraw;
        o.debugSave = c.DebugSave;
        if (c.DebugDir != nullptr) {
            IntPtr p = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(c.DebugDir);
            o.debugDir = static_cast<const char*>(p.ToPointer());
            System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
        }
        o.filter = ToNative(c.Filter);
        return o;
    }

    void BarcodeCoreCli::DetectAll(
        IntPtr data, int width, int height, int stride, int channels,
        RectRotateCli rr, Nullable<RectRotateCli> rrMask,
        DetectOptionsCli opts,
        List<RectRotateCli>^% rects,
        List<System::String^>^% payloads,
        List<CodeSymbologyCli>^% types)
    {
        rects = gcnew List<RectRotateCli>();
        payloads = gcnew List<System::String^>();
        types = gcnew List<CodeSymbologyCli>();

        if (data == IntPtr::Zero || width <= 0 || height <= 0) return;

        cv::Mat src;
        try
        {
            Nullable<RectRotateCli> mask =
                rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
                : Nullable<RectRotateCli>();

            com->CropRotToMat(
                data, width, height, stride, channels,
                rr, mask, false,
                System::IntPtr(&src)
            );
        }
        catch (const cv::Exception& ex)
        {
            throw gcnew System::Exception(gcnew System::String(ex.what()));
        }

        std::vector<BeeCpp::CodeResult> results;
        BeeCpp::DetectOptions nativeOpt = ToNative(opts);
        _core->DetectAll(src, results, nativeOpt);

        rects->Capacity = (int)results.size();
        payloads->Capacity = (int)results.size();
        types->Capacity = (int)results.size();

        for (const auto& r : results) {
            RectRotateCli rrOut;
            rrOut.Shape = (r.corners.size() >= 3) ? ShapeType::Polygon : ShapeType::Rectangle;
            rrOut.PosCenter = ToPtF32(r.rrect.center.x, r.rrect.center.y);
            RectF32 wh; wh.Width = (float)r.rrect.size.width; wh.Height = (float)r.rrect.size.height;
            rrOut.RectWH = wh;
            rrOut.RectRotationDeg = r.rrect.angle;
            rrOut.IsWhite = false;
            if (!r.corners.empty()) {
                WorldPolygonToLocalUnrotated(r.corners, r.rrect.center, (float)rrOut.RectRotationDeg, rrOut.PolyLocalPoints);
            }
            else {
                rrOut.PolyLocalPoints = nullptr;
            }
            rrOut.HexVertexOffsets = nullptr;

            rects->Add(rrOut);
            payloads->Add(gcnew System::String(r.text.c_str()));
            types->Add(MapCli(r.symbology));
        }
    }

  

    bool BarcodeCoreCli::ReadMatGray8(IntPtr p, IntPtr dstData, int dstStride, int width, int height)
    {
        if (p == IntPtr::Zero || dstData == IntPtr::Zero) return false;
        cv::Mat* m = reinterpret_cast<cv::Mat*>(p.ToPointer());
        if (!m || m->empty() || m->type() != CV_8UC1) return false;
        if (m->cols != width || m->rows != height) return false;

        unsigned char* dst = reinterpret_cast<unsigned char*>(dstData.ToPointer());
        for (int y = 0; y < height; ++y) {
            const unsigned char* srcRow = m->ptr<unsigned char>(y);
            std::memcpy(dst + (size_t)y * (size_t)dstStride, srcRow, (size_t)width);
        }
        return true;
    }

    void BarcodeCoreCli::FreeMat(IntPtr p)
    {
        if (p == IntPtr::Zero) return;
        cv::Mat* m = reinterpret_cast<cv::Mat*>(p.ToPointer());
        delete m;
    }

} // namespace BeeCpp
