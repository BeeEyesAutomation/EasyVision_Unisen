using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using BeeGlobal;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Các helper hình học thuần — KHÔNG phụ thuộc WinForms control nào,
    /// KHÔNG đọc Global.* hay imgView. Tất cả input đều inject qua tham số
    /// để 2 UserControl mới (UC1/UC2) và View.cs cũ đều dùng chung một bản.
    ///
    /// Những method phụ thuộc imgView (ControlToImage/ScreenToImage/GetImageViewPort)
    /// KHÔNG nằm ở đây — chúng thuộc về ImageCanvasControl (UC1).
    /// </summary>
    public static class GeometryHelper
    {
        // ---------------------------------------------------------------------
        // Rotation primitives
        // ---------------------------------------------------------------------

        /// <summary>Quay điểm <paramref name="pt"/> quanh <paramref name="center"/> theo góc độ.</summary>
        public static PointF RotateAround(PointF pt, PointF center, float deg)
        {
            float rad = deg * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad), sin = (float)Math.Sin(rad);
            float x = pt.X - center.X, y = pt.Y - center.Y;
            return new PointF(center.X + x * cos - y * sin, center.Y + x * sin + y * cos);
        }

        /// <summary>Quay vector (delta, không cộng tâm) theo góc độ.</summary>
        public static PointF RotateVector(PointF v, float deg)
        {
            float rad = deg * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad), sin = (float)Math.Sin(rad);
            return new PointF(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }

        /// <summary>Wrap quanh <see cref="RectRotate.Rotate"/> cho đồng bộ API.</summary>
        public static PointF RotatePoint(float angleDeg, PointF p)
        {
            return RectRotate.Rotate(p, angleDeg);
        }

        // ---------------------------------------------------------------------
        // Matrix transform
        // ---------------------------------------------------------------------

        /// <summary>Áp Matrix lên 1 điểm, trả về điểm mới (không đụng input).</summary>
        public static PointF TransformPoint(Matrix m, PointF p)
        {
            var pts = new[] { p };
            m.TransformPoints(pts);
            return pts[0];
        }

        /// <summary>
        /// Xây dựng Matrix nghịch đảo để đưa một điểm từ screen/client space về
        /// local space của <paramref name="rr"/> (tâm tại _PosCenter, xoay _rectRotation).
        /// Caller chịu trách nhiệm dispose Matrix bằng using.
        /// </summary>
        /// <param name="rr">RectRotate đích.</param>
        /// <param name="zoomPercent">Zoom của canvas (ví dụ 100 = 100%).</param>
        /// <param name="scroll">AutoScrollPosition của canvas (thường âm theo convention WinForms).</param>
        /// <param name="useDragCenter">Nếu true dùng dragCenter/angleWhenDrag thay cho giá trị hiện tại của rr.</param>
        public static Matrix BuildLocalInverseMatrixFor(RectRotate rr, float zoomPercent, Point scroll,
                                                       bool useDragCenter, PointF dragCenter, float angleWhenDrag)
        {
            var m = new Matrix();
            m.Translate(scroll.X, scroll.Y);
            float s = zoomPercent / 100f;
            m.Scale(s, s);
            if (useDragCenter) m.Translate(dragCenter.X, dragCenter.Y);
            else m.Translate(rr._PosCenter.X, rr._PosCenter.Y);
            if (useDragCenter) m.Rotate(angleWhenDrag);
            else m.Rotate(rr._rectRotation);
            m.Invert();
            return m;
        }

        // ---------------------------------------------------------------------
        // Polygon / bounds helpers
        // ---------------------------------------------------------------------

        /// <summary>
        /// Tính bounding box từ list điểm. Nếu điểm đầu trùng điểm cuối (polygon đã close)
        /// thì bỏ qua điểm cuối để không ảnh hưởng width/height.
        /// Width/Height tối thiểu = 1 để tránh rect zero gây chia 0 ở caller.
        /// </summary>
        public static RectangleF BboxOf(IList<PointF> pts)
        {
            if (pts == null || pts.Count == 0) return RectangleF.Empty;
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            int n = pts.Count;
            int m = (n >= 2 && pts[0].Equals(pts[n - 1])) ? n - 1 : n;
            for (int i = 0; i < m; i++)
            {
                var p = pts[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            return new RectangleF(minX, minY, Math.Max(1f, maxX - minX), Math.Max(1f, maxY - minY));
        }

        /// <summary>
        /// Bounding box local của polygon trong RectRotate. Nếu rr không có polygon
        /// thì fallback trả về rr._rect. Khác <see cref="BboxOf"/> ở chỗ width/height
        /// có thể là 0 (không ép minimum).
        /// </summary>
        public static RectangleF GetPolygonBoundsLocal(RectRotate rr)
        {
            var pts = rr == null ? null : rr.PolyLocalPoints;
            if (pts == null || pts.Count == 0)
                return rr != null ? rr._rect : RectangleF.Empty;

            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            for (int i = 0; i < pts.Count; i++)
            {
                var p = pts[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            return new RectangleF(minX, minY, Math.Max(0, maxX - minX), Math.Max(0, maxY - minY));
        }

        /// <summary>True nếu mọi điểm trong <paramref name="pts"/> nằm trong <paramref name="r"/>.</summary>
        public static bool BoundsContainAll(RectangleF r, IList<PointF> pts)
        {
            if (pts == null || pts.Count == 0) return true;
            for (int i = 0; i < pts.Count; i++)
                if (!r.Contains(pts[i])) return false;
            return true;
        }

        /// <summary>True nếu cả 6 đỉnh hexagon nằm trong _rect hiện tại của rr.</summary>
        public static bool HexBoundsContainAll(RectRotate rr)
        {
            if (rr == null) return true;
            var r = rr._rect;
            var verts = rr.GetHexagonVerticesLocal();
            for (int i = 0; i < verts.Length; i++)
                if (!r.Contains(verts[i])) return false;
            return true;
        }
    }
}
