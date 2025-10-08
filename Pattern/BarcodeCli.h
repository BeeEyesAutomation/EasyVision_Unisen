#include "Global.h"
#include "BarcodeCore.h" // core ZXing của bạn

using namespace System;
using namespace System::Collections::Generic;

namespace BeeCpp {
    public enum class CodeSymbologyCli
    {
        Unknown = 0,
        QR_CODE,
        EAN_13, EAN_8, UPC_A, UPC_E,
        CODE_128, CODE_39, ITF, CODABAR,
        PDF417, AZTEC, DATA_MATRIX
    };
    public ref class BarcodeCoreCli
    {
    public:
        BarcodeCoreCli();
        ~BarcodeCoreCli();
        !BarcodeCoreCli();

        // KHÔNG dùng [Out]; dùng % (tracking reference) như bạn yêu cầu
        void DetectAll(
            IntPtr data, int width, int height, int stride, int channels, RectRotateCli rr, Nullable<RectRotateCli> rrMask,
            List<RectRotateCli>^% rects,
            List<System:: String^>^% payloads,
            List<CodeSymbologyCli>^% types
        );

        // Nếu muốn lấy cả polygon local
        void DetectAllWithCorners(
            IntPtr data, int width, int height, int stride, int channels, RectRotateCli rr, Nullable<RectRotateCli> rrMask,
            List<RectRotateCli>^% rects,
            List<System:: String^>^% payloads,
            List<CodeSymbologyCli>^% types
        );

    private:
        BeeCpp::BarcodeCore* _core;
        CommonPlus* com;
        // tiện ích: xoay điểm (deg), và convert polygon world -> local (un-rotate)
        static PointF32 ToPtF32(float x, float y);
        static void WorldPolygonToLocalUnrotated(
            const std::vector<cv::Point2f>& worldPts,
            const cv::Point2f& center,
            float angleDeg, // OpenCV angle
            cli::array<PointF32>^% outLocal // alloc & fill
        );
    };

} // namespace BeeCpp
