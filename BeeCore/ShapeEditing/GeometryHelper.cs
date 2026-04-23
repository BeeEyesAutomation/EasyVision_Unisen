using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using BeeGlobal;

namespace BeeCore.ShapeEditing
{
    public static class GeometryHelper
    {
        public static Matrix BuildLocalInverseMatrixFor(RectRotate rr, float zoomPercent, Point scroll, bool useDragCenter, PointF dragCenter, float angleWhenDrag)
        {
            var m = new Matrix();
            m.Translate(scroll.X, scroll.Y);
            m.Scale(zoomPercent / 100f, zoomPercent / 100f);
            m.Translate(useDragCenter ? dragCenter.X : rr._PosCenter.X, useDragCenter ? dragCenter.Y : rr._PosCenter.Y);
            m.Rotate(useDragCenter ? angleWhenDrag : rr._rectRotation);
            m.Invert();
            return m;
        }

        public static PointF TransformPoint(Matrix m, PointF p)
        {
            var pts = new[] { p };
            m.TransformPoints(pts);
            return pts[0];
        }

        public static PointF RotateAround(PointF pt, PointF center, float deg)
        {
            float rad = deg * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);
            float x = pt.X - center.X;
            float y = pt.Y - center.Y;
            return new PointF(center.X + x * cos - y * sin, center.Y + x * sin + y * cos);
        }

        public static PointF RotateVector(PointF v, float deg)
        {
            float rad = deg * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);
            return new PointF(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }

        public static RectangleF BboxOf(IList<PointF> pts)
        {
            if (pts == null || pts.Count == 0)
                return RectangleF.Empty;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
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

        public static int HitTestPolygonVertex(IList<PointF> pts, PointF localPoint, float handleSize)
        {
            if (pts == null)
                return -1;

            for (int i = pts.Count - 1; i >= 0; i--)
            {
                var v = pts[i];
                var h = new RectangleF(v.X - handleSize / 2f, v.Y - handleSize / 2f, handleSize, handleSize);
                if (h.Contains(localPoint))
                    return i;
            }

            return -1;
        }

        public static AnchorPoint HitTestCornerHandle(RectangleF bounds, PointF localPoint, float handleSize)
        {
            float r = handleSize;
            if (new RectangleF(bounds.Left - r / 2f, bounds.Top - r / 2f, r, r).Contains(localPoint))
                return AnchorPoint.TopLeft;
            if (new RectangleF(bounds.Right - r / 2f, bounds.Top - r / 2f, r, r).Contains(localPoint))
                return AnchorPoint.TopRight;
            if (new RectangleF(bounds.Left - r / 2f, bounds.Bottom - r / 2f, r, r).Contains(localPoint))
                return AnchorPoint.BottomLeft;
            if (new RectangleF(bounds.Right - r / 2f, bounds.Bottom - r / 2f, r, r).Contains(localPoint))
                return AnchorPoint.BottomRight;

            return AnchorPoint.None;
        }
    }
}
