#pragma once
#include <opencv2/opencv.hpp>
#include <string>
#include <vector>
#include <array>

namespace BeeCpp {

    // ===== Loại mã =====
    enum class Symbology {
        Unknown = 0,
        EAN8, EAN13, UPC_A, UPC_E,
        Code39, Code93, Code128, ITF,
        QR, DataMatrix, PDF417, Aztec,
        MaxiCode, Codabar, MicroQR, MicroPDF417, RSS14, RSSExpanded
    };

    // ===== Kết quả =====
    struct CodeResult {
        std::string                 text;       // payload
        Symbology                   symbology;  // loại mã
        std::vector<cv::Point2f>    corners;    // TL,TR,BR,BL (nếu có)
        cv::RotatedRect             rrect;      // hộp ở toạ độ gốc
        float                       score = 0;  // xếp hạng (ví dụ: diện tích × AR)
    };

    // ===== Tham số lọc (Filter) cho 1D =====
    struct FilterParams {
        // Mặc định đúng như bạn đang set
        int   MinArea = 700;

        int   CloseKernelWDiv = 15;  // width / 60
        int   CloseKernelH = 3;      // 3 px
        bool  UseNoRotateMask = true;
    };

    // ===== Tuỳ chọn detect =====
    struct DetectOptions {
        bool        enablePreprocess = false;     // CLAHE + unsharp nhẹ
        bool        findBoxes = true;             // THAY cho tham số FindBox rời
        bool        cropFirst = true;             // detect ROI → crop/rectify → ZXing
        bool        debugDraw = true;            // vẽ overlay cuối (mặc định tắt)
        bool        debugSave = true;            // lưu chuỗi ảnh debug (mặc định tắt)
        std::string debugDir;                     // thư mục lưu debug
        FilterParams filter;                      // tham số lọc 1D
    };

    // ===== Debugger lưu ảnh theo bước =====
    struct Debugger {
        bool        save = true;
        bool        draw = true;
        std::string dir;
        mutable int step = 0;

        void init(bool enableSave, bool enableDraw, const std::string& outDir)
        {
            save = enableSave; draw = enableDraw; dir = outDir; step = 0;
        }
        inline std::string nextName(const std::string& tag) const {
            char f[256]; std::snprintf(f, sizeof(f), "%04d_%s.png", step++, tag.c_str());
            return dir.empty() ? std::string(f) : (dir + "/" + f);
        }
    };

    // ===== Vẽ / Save tiện ích =====
    class DebugViz {
    public:
        static void Save(const Debugger& D, const cv::Mat& img, const std::string& tag);
        static void SaveMask(const Debugger& D, const cv::Mat& mask, const std::string& tag);
        static void DrawRRects(cv::Mat& bgr, const std::vector<cv::RotatedRect>& rrs,
            const cv::Scalar& col = { 0,255,0 }, int thick = 2);
        static void PutText(cv::Mat& bgr, const std::string& s, cv::Point org,
            double scale = 0.6, const cv::Scalar& col = { 0,0,255 }, int thick = 2);
    };

    // ====== Bộ phát hiện/đọc mã vạch ======
    class BarcodeCore {
    public:
        BarcodeCore() = default;

        // API chính
        void DetectAll(const cv::Mat& src,
            std::vector<CodeResult>& out,
            const DetectOptions& opt) const;



    private:
        // --- mapping & geometry ---
        static Symbology MapFormat(int zxingFormat);
        static void OrderQuadTLTRBRBL(std::vector<cv::Point2f>& quad);

        // --- crop 1 box (xoay theo box, không xoay ảnh gốc) ---
        static cv::RotatedRect NormalizeRR(cv::RotatedRect rr);
        static cv::RotatedRect NormalizeRR(cv::RotatedRect rr, Symbology sym);
        static cv::RotatedRect RectFromCorners(const std::vector<cv::Point2f>& pts);
        static cv::Mat  CropRotatedBox(const cv::Mat& gray, cv::RotatedRect rr, float pad = 0.06f);


        // ====== 1D detection per-box (không xoay toàn cục) ======
        struct RotBox { cv::RotatedRect rr; float angle; float score; };

        static cv::Mat  MakeMask(const cv::Mat& gray8, const FilterParams& fp); // mask 1D theo structure tensor
        static std::vector<RotBox> DetectBox(const cv::Mat& gray8, const FilterParams& fp);

        // Trim biên ngang của patch trước ZXing (tùy chọn)
        static cv::Mat TrimHorizBorders(const cv::Mat& patch);

        // PCA angle “nguyên tác”
        static float PCAAngleDeg(const std::vector<cv::Point>& pts);
        static float PCAAngleDeg(const std::vector<cv::Point2f>& pts);
    };

} // namespace BeeCpp