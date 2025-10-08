#pragma once
#include <opencv2/core.hpp>
#include <string>
#include <vector>

namespace BeeCpp {

    // Loại mã
    enum class CodeSymbology {
        Unknown = 0,
        QR_CODE,
        EAN_13, EAN_8, UPC_A, UPC_E,
        CODE_128, CODE_39, ITF, CODABAR,
        PDF417, AZTEC, DATA_MATRIX
    };

    // Kết quả
    struct CodeResult {
        std::string              text;      // payload UTF-8
        CodeSymbology            symbology; // loại mã
        std::vector<cv::Point2f> corners;   // polygon (TL,TR,BR,BL) đã chuẩn hoá
        cv::RotatedRect          rrect;     // minAreaRect từ corners (đã normalize)
        double                   score = 1.0;
    };

    class BarcodeCore
    {
    public:
        // src: CV_8UC1 hoặc CV_8UC3 (BGR). out sẽ clear trước khi ghi.
        // enablePreprocess=true: CLAHE + unsharp nhẹ cho ảnh mờ/nhạt.
        void DetectAll(const cv::Mat& src, std::vector<CodeResult>& out, bool enablePreprocess = false) const;

    private:
        // util
        static CodeSymbology   MapFormat(int zxingFmt);
        static cv::RotatedRect RectFromCorners(const std::vector<cv::Point2f>& pts);
        static cv::RotatedRect NormalizeRR(cv::RotatedRect rr, CodeSymbology sym);
        static void            OrderQuadTLTRBRBL(std::vector<cv::Point2f>& q);
        static std::vector<cv::Point2f> SynthesizeQuadFromLine(
            const cv::Point2f& p0, const cv::Point2f& p1, float heightRatio = 0.15f, float quiet = 0.08f);

        static float           GuessHeightRatioByText(const std::string& txt);
        static float           Estimate1DHeightPx(const cv::Mat& gray,
            const cv::Point2f& c,
            const cv::Point2f& u,
            const cv::Point2f& n,
            float w);
    };

} // namespace BeeCpp
