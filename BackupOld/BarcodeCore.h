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
        float                       score = 0;  // xếp hạng (diện tích × AR)
    };

    // ===== Tuỳ chọn detect =====
    struct DetectOptions {
        bool        enablePreprocess = false;   // CLAHE + unsharp nhẹ
        bool        cropFirst = true;    // detect ROI → crop/rectify → ZXing
        bool        debugDraw = false;   // vẽ overlay cuối
        bool        debugSave = false;   // lưu chuỗi ảnh debug
        std::string debugDir;                   // thư mục lưu debug
    };

    // ===== Debugger lưu ảnh theo bước =====
    struct Debugger {
        bool        save = false;
        bool        draw = false;
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
            const DetectOptions& opt, bool FindBox) const;



    private:
        // --- mapping & geometry ---
        static Symbology MapFormat(int zxingFormat);
        static void OrderQuadTLTRBRBL(std::vector<cv::Point2f>& quad);

        // --- crop 1 box (xoay theo box, không xoay ảnh gốc) ---
        static cv::RotatedRect NormalizeRR(cv::RotatedRect rr);
        static cv::RotatedRect NormalizeRR(cv::RotatedRect rr, Symbology sym);
        static cv::RotatedRect RectFromCorners(const std::vector<cv::Point2f>& pts);
        static cv::Mat  CropRotatedBox(const cv::Mat& gray, cv::RotatedRect rr, float pad = 0.06f);

        // --- preprocess fallback (không bắt buộc) ---
        static cv::Mat  Preprocess1D_Auto(const cv::Mat& gray);

        // ====== 1D detection per-box (không xoay toàn cục) ======
        struct OneDBox { cv::RotatedRect rr; float score; };

        static cv::Mat  MakeMask1D_NoRotate(const cv::Mat& gray8);      // mask 1D dựa structure tensor
        static std::vector<OneDBox> Detect1D_PerBox_NoGlobalRotate(const cv::Mat& gray8);

        // trim biên ngang của patch trước ZXing (tùy chọn)
        static cv::Mat TrimHorizBorders(const cv::Mat& patch);
    };

} // namespace BeeCpp