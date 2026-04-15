#include "BarcodeCli.h"
#include <opencv2/core.hpp>
#include <cmath>
using namespace cv;
namespace BeeCpp {

    static inline float Deg2Rad(float d) { return d * 3.14159265358979323846f / 180.f; }

    BarcodeCoreCli::BarcodeCoreCli() { _core = new BeeCpp::BarcodeCore(); com = new BeeCpp::CommonPlus(); }
    BarcodeCoreCli::~BarcodeCoreCli() { this->!BarcodeCoreCli(); }
    BarcodeCoreCli::!BarcodeCoreCli() { if (_core) { delete _core; _core = nullptr; } }

    PointF32 BarcodeCoreCli::ToPtF32(float x, float y) {
        PointF32 p; p.X = x; p.Y = y; return p;
    }

    // Convert các điểm polygon từ world → local (tâm 0, không xoay):
    // 1) dịch về tâm (pt - center)
    // 2) "un-rotate" theo -angleDeg để local không xoay
    void BarcodeCoreCli::WorldPolygonToLocalUnrotated(
        const std::vector<cv::Point2f>& worldPts,
        const cv::Point2f& center,
        float angleDeg,
        cli::array<PointF32>^% outLocal)
    {
        int n = (int)worldPts.size();
        outLocal = gcnew cli::array<PointF32>(n);

        // OpenCV RotatedRect.angle quy ước [ -90 , 0 ), chiều xoay (CW) lịch sử.
        // Ta giữ nguyên angleDeg cho RectRotationDeg, còn local thì "un-rotate": rot(-angleDeg)
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

    static BeeCpp::CodeSymbologyCli MapCli(BeeCpp::Symbology s)
    {
        return static_cast<BeeCpp::CodeSymbologyCli>(static_cast<int>(s));
    }

    void BarcodeCoreCli::DetectAll(
        IntPtr data, int width, int height, int stride, int channels, RectRotateCli rr, Nullable<RectRotateCli> rrMask,
        List<RectRotateCli>^% rects,
        List<System::String^>^% payloads,
        List<CodeSymbologyCli>^% types,bool FindBox)
    {
        rects = gcnew List<RectRotateCli>();
        payloads = gcnew List<System::String^>();
        types = gcnew List<CodeSymbologyCli>();
        cv::Mat src = Mat();
        if (data == IntPtr::Zero || width <= 0 || height <= 0) return;
        try
        {
            // 1) Wrap dữ liệu managed bằng Mat với stride (step) tùy ý
            int type = (channels == 1) ? CV_8UC1 :
                (channels == 3) ? CV_8UC3 : CV_8UC4;

            Mat wrapped(height, width, type, data.ToPointer(), (size_t)stride);

            // 2) Convert sang 8U3 (BGR)
            Mat bgr;
            switch (channels)
            {
            case 1:  cvtColor(wrapped, bgr, COLOR_GRAY2BGR); break; // 1 -> 3
            case 3:  bgr = wrapped; break;                           // đã 3 kênh
            case 4:  cvtColor(wrapped, bgr, COLOR_BGRA2BGR); break;  // 4 -> 3
            }
            Nullable<RectRotateCli> mask =
                rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
                : Nullable<RectRotateCli>();
            com->CropRotToMat(
                data, width, height, stride, channels,
                rr, mask, false,
                System::IntPtr(&src)
            );
            //   cv::imwrite("color.png", _ColorPP->matCrop);
        }
        catch (const cv::Exception& ex)
        {
            throw gcnew System::Exception(gcnew System::String(ex.what()));
        }

        std::vector<BeeCpp::CodeResult> results;
        BeeCpp::DetectOptions opt;
        opt.enablePreprocess = true;
        opt.cropFirst = true;   // pipeline đã theo rule mới
        opt.debugSave = true;
        opt.debugDir = "debug_bar";

        _core->DetectAll(src, results, opt, FindBox);

        rects->Capacity = (int)results.size();
        payloads->Capacity = (int)results.size();
        types->Capacity = (int)results.size();

        for (const auto& r : results) {
            RectRotateCli rr;
            // Shape: nếu ZXing trả polygon (thường 4 đỉnh), coi là Polygon
            rr.Shape = (r.corners.size() >= 3) ? ShapeType::Polygon : ShapeType::Rectangle;

            rr.PosCenter = ToPtF32(r.rrect.center.x, r.rrect.center.y);
            RectF32 wh;
            wh.Width = static_cast<float>(r.rrect.size.width);
            wh.Height = static_cast<float>(r.rrect.size.height);
            rr.RectWH = wh;

            rr.RectRotationDeg = r.rrect.angle;   // giữ đúng quy ước OpenCV ([-90,0))
            rr.IsWhite = false;                   // mặc định

            // PolyLocalPoints (nếu có polygon)
            if (!r.corners.empty()) {
                WorldPolygonToLocalUnrotated(r.corners, r.rrect.center, (float)rr.RectRotationDeg, rr.PolyLocalPoints);
            }
            else {
                rr.PolyLocalPoints = nullptr;
            }

            // Hexagon: không áp dụng ở đây
            rr.HexVertexOffsets = nullptr;

            rects->Add(rr);
            payloads->Add(gcnew System::String(r.text.c_str()));
            types->Add(MapCli(r.symbology));
        }
    }

 

} // namespace BeeCpp
