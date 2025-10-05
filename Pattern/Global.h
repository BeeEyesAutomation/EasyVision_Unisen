#pragma once

#define NOMINMAX
#include <algorithm>
#include <vector>
#include <limits>
#include <cmath>
#include <stdexcept>
#include <opencv2/opencv.hpp>

#include <opencv2/imgproc/imgproc_c.h>
#using <System.Windows.Forms.dll> 
#using <System.Drawing.dll> 
using namespace System;

namespace BeeCpp
{
    public enum class ShapeType : int
    {
        Rectangle = 0,
        Ellipse = 1,
        Hexagon = 2,
        Polygon = 3
    };

    public value struct PointF32
    {
        float X, Y;
        PointF32(float x, float y) : X(x), Y(y) {}
    };

    public value struct RectF32
    {
        float X, Y, Width, Height;
        RectF32(float x, float y, float w, float h) : X(x), Y(y), Width(w), Height(h) {}
    };

    // Mirror tối thiểu của RectRotate bên C#
    public value struct RectRotateCli
    {
        ShapeType     Shape;
        PointF32      PosCenter;        // _PosCenter (world)
        RectF32       RectWH;           // _rect.Width/Height (local, tâm 0)
        double        RectRotationDeg;  // _rectRotation
        bool          IsWhite;          // nền trắng khi apply mask

        // TÁCH BẠCH:
       cli:: array<PointF32>^ PolyLocalPoints;   // chỉ cho Polygon (local, không xoay)
       cli::array<PointF32>^ HexVertexOffsets;  // chỉ cho Hexagon (offset so với hex mặc định theo RectWH)
    };

      class Common 
    {
    public:
        // ==== API gọi từ C# với OpenCvSharp ====
         IntPtr CropRotatedRect( // trả về cv::Mat* mới -> nhớ FreeMat
            IntPtr srcMatCvPtr,
            RectRotateCli rr,
            Nullable<RectRotateCli> rrMask,
            bool returnMaskOnly
        );

         void CropTo(             // ghi thẳng vào dst (cv::Mat*)
            IntPtr srcMatCvPtr,
            RectRotateCli rr,
            Nullable<RectRotateCli> rrMask,
            bool returnMaskOnly,
            IntPtr dstMatCvPtr
        );

         IntPtr SetImgeRaw(       // từ buffer raw → crop → trả về Mat*
            IntPtr data, int w, int h, int stride, int ch,
            RectRotateCli rr, Nullable<RectRotateCli> rrMask,
            bool returnMaskOnly
        );

         void CropRotToMat(       // từ buffer raw → crop → ghi dst
            IntPtr data, int w, int h, int stride, int ch,
            RectRotateCli rr, Nullable<RectRotateCli> rrMask,
            bool returnMaskOnly,
            IntPtr dstMatCvPtr
        );

         void FreeMat(IntPtr p);  // delete cv::Mat*


        // Core crop
         void RunCrop(
            const cv::Mat& src,
            const RectRotateCli% rr,
            const RectRotateCli* rrMask,
            bool returnMaskOnly,
            cv::Mat& out);

        // Helpers lõi
         cv::Mat   RotateMat(const cv::Mat& raw, const cv::RotatedRect& rot);
         cv::Point2f RotatePoint(const cv::Point2f& p, float degree);

         void GetAnchorSizeFor(
            const RectRotateCli% rr,
            cv::Point2f& worldAnchor,
            cv::Size2f& size,
            cv::Point2f& localCenterForShape);

         void GetPolygonBounds(
            const std::vector<cv::Point2f>& pts,
            float& minX, float& minY, float& maxX, float& maxY);

         std::vector<cv::Point> AxisAlignedRectCorners(
            const cv::Point2f& c, int W, int H);

         std::vector<cv::Point> RegularHexFallback(
            const cv::Point2f& centerInMask, float angleInPatch, int W, int H);

         std::vector<cv::Point> PolyFromLocalPoints(
            const std::vector<cv::Point2f>& localPts,
            const cv::Point2f& centerInMask,
            float angleInPatch,
            const cv::Point2f& localCenter);

         void DrawShapeMaskIntoWithSize(
            const RectRotateCli% rr,
            cv::Mat& mask,
            const cv::Point2f& centerInMask,
            float angleInPatch,
            uchar fillValue,
            int W, int H,
            const cv::Point2f& localCenterForShape);

    private:
        // === tiện ích nội bộ ===
        static inline std::vector<cv::Point2f> ToVec(cli::array<PointF32>^ a)
        {
            std::vector<cv::Point2f> v;
            if (a == nullptr) return v;
            v.reserve(static_cast<size_t>(a->LongLength));
            for (int i = 0; i < a->Length; ++i) {
                PointF32 p = a[i];
                v.push_back(cv::Point2f((float)p.X, (float)p.Y));
            }
            return v;
        }

         inline bool isFinite(const cv::Point2f& p)
        {
            return std::isfinite(p.x) && std::isfinite(p.y);
        }

         std::vector<cv::Point2f> BuildHexLocalVertices(
            float w, float h, cli::array<PointF32>^ hexOffsets /*nullable*/);

         int    CvTypeFromChannels(int ch);
         size_t SafeStep(int w, int ch, int stride);
    };
}
