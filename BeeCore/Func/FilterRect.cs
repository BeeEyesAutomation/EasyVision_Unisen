using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    class FilterRect
    {// Diện tích (local rect luôn là (-w/2,-h/2,w,h))
        private static float GetArea(RectRotate r)
        {
            if (r == null) return 0f;
            return Math.Abs(r._rect.Width * r._rect.Height);
        }

        // Lấy polygon 4 đỉnh hình chữ nhật xoay (tọa độ thế giới)
        private static PointF[] GetRectWorldPolygon(RectRotate r)
        {
            float w = r._rect.Width;
            float h = r._rect.Height;
            float hw = w * 0.5f;
            float hh = h * 0.5f;

            // 4 đỉnh local theo quy ước bạn đang dùng
            PointF[] local =
            {
        new PointF(-hw, -hh),
        new PointF(+hw, -hh),
        new PointF(+hw, +hh),
        new PointF(-hw, +hh)
    };

            PointF[] world = new PointF[4];
            for (int i = 0; i < 4; i++)
            {
                var pRot = Rotate(local[i], r._rectRotation);
                world[i] = Add(r._PosCenter, pRot);
            }
            return world;
        }
        public static PointF Rotate(PointF p, float deg)
        {
            double r = deg * Math.PI / 180.0;
            double c = Math.Cos(r), s = Math.Sin(r);
            return new PointF((float)(p.X * c - p.Y * s), (float)(p.X * s + p.Y * c));
        }

        public static PointF Add(PointF a, PointF b) => new PointF(a.X + b.X, a.Y + b.Y);
        // Diện tích polygon (shoelace)
        private static float PolygonArea(IList<PointF> poly)
        {
            if (poly == null) return 0f;
            int n = poly.Count;
            if (n < 3) return 0f;

            double sum = 0;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                var pi = poly[i];
                var pj = poly[j];
                sum += (double)pj.X * pi.Y - (double)pi.X * pj.Y;
            }
            return (float)(0.5 * sum);
        }

        // Tích có hướng: cross( (b-a), (p-a) )
        private static float Cross(PointF a, PointF b, PointF p)
        {
            float abx = b.X - a.X;
            float aby = b.Y - a.Y;
            float apx = p.X - a.X;
            float apy = p.Y - a.Y;
            return abx * apy - aby * apx;
        }

        private static bool IsInsideEdge(PointF a, PointF b, PointF p)
        {
            // clip polygon đi theo CCW, "trong" là phía bên trái cạnh
            return Cross(a, b, p) >= 0f;
        }

        // Tính giao điểm đoạn SE với đường thẳng biên AB, dùng signed distance
        private static PointF IntersectSegmentWithEdge(PointF s, PointF e, PointF a, PointF b)
        {
            float dS = Cross(a, b, s);
            float dE = Cross(a, b, e);
            float denom = dS - dE;
            if (Math.Abs(denom) < 1e-6f)
                return s; // gần như song song, trả tạm S

            float t = dS / (dS - dE); // dS và dE khác dấu
            return new PointF(
                s.X + t * (e.X - s.X),
                s.Y + t * (e.Y - s.Y));
        }

        // Clip subject polygon theo 1 cạnh [A,B] của clip polygon
        private static List<PointF> ClipPolygonAgainstEdge(
            List<PointF> subject, PointF a, PointF b)
        {
            var output = new List<PointF>();
            int n = subject.Count;
            if (n == 0) return output;

            PointF s = subject[n - 1];
            bool sInside = IsInsideEdge(a, b, s);

            for (int i = 0; i < n; i++)
            {
                PointF e = subject[i];
                bool eInside = IsInsideEdge(a, b, e);

                if (eInside)
                {
                    if (!sInside)
                    {
                        // S ngoài, E trong: thêm giao điểm + E
                        PointF inter = IntersectSegmentWithEdge(s, e, a, b);
                        output.Add(inter);
                    }
                    output.Add(e);
                }
                else if (sInside)
                {
                    // S trong, E ngoài: thêm giao điểm
                    PointF inter = IntersectSegmentWithEdge(s, e, a, b);
                    output.Add(inter);
                }

                s = e;
                sInside = eInside;
            }

            return output;
        }

        // Giao của 2 polygon lồi (rotated rect)
        private static List<PointF> PolygonIntersection(
            IList<PointF> poly1, IList<PointF> poly2)
        {
            if (poly1 == null || poly2 == null ||
                poly1.Count < 3 || poly2.Count < 3)
                return new List<PointF>();

            // copy poly1 làm subject
            var output = new List<PointF>(poly1);

            int nClip = poly2.Count;
            for (int i = 0; i < nClip; i++)
            {
                PointF a = poly2[i];
                PointF b = poly2[(i + 1) % nClip];
                output = ClipPolygonAgainstEdge(output, a, b);
                if (output.Count == 0)
                    break;
            }

            return output;
        }

        public static List<RectRotate> RemoveInnerRectRotates(
    IList<RectRotate> rects,
    float overlapThreshold)
        {
            // overlapThreshold:
            //  - Nếu 0–1: hiểu là tỷ lệ (0.8 = 80%)
            //  - Nếu >1: hiểu là % (80 = 80%)
            if (rects == null || rects.Count == 0)
                return new List<RectRotate>();

            if (overlapThreshold <= 0f) overlapThreshold = 0f;
            if (overlapThreshold > 1f) overlapThreshold /= 100f;

            // Sort theo diện tích tăng dần (nhỏ -> lớn)
            var withArea = rects
                .Select((r, idx) => new
                {
                    Rect = r,
                    Index = idx,
                    Area = GetArea(r)
                })
                .OrderBy(x => x.Area)
                .ToList();

            int n = withArea.Count;
            bool[] removed = new bool[n];

            for (int i = 0; i < n; i++)
            {
                if (removed[i]) continue;

                var small = withArea[i];
                float smallArea = small.Area;
                if (smallArea <= 0f)
                {
                    removed[i] = true;
                    continue;
                }

                var polySmall = GetRectWorldPolygon(small.Rect);

                for (int j = i + 1; j < n; j++)
                {
                    if (removed[j]) continue;

                    var big = withArea[j];
                    if (big.Area <= 0f || big.Area < smallArea)
                        continue;

                    var polyBig = GetRectWorldPolygon(big.Rect);

                    var inter = PolygonIntersection(polySmall, polyBig);
                    if (inter == null || inter.Count < 3)
                        continue;

                    float interArea = Math.Abs(PolygonArea(inter));
                    float ratio = interArea / smallArea; // tỉ lệ diện tích nhỏ bị phủ

                    if (ratio >= overlapThreshold)
                    {
                        // Hình nhỏ bị phủ đủ ngưỡng -> loại
                        removed[i] = true;
                        break;
                    }
                }
            }

            // Build list kết quả
            var result = new List<RectRotate>();
            for (int i = 0; i < n; i++)
            {
                if (!removed[i])
                    result.Add(withArea[i].Rect);
            }
            return result;
        }

    }
}
