using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace BeeCore
    {
        public static class ResultFilter
        {
        private static bool IsOverlapEnough(
    RectRotate r1,
    RectRotate r2,
    float threshold)
        {
            if (r1 == null || r2 == null) return false;

            var p1 = GetWorldPoly(r1);
            var p2 = GetWorldPoly(r2);

            var inter = PolygonIntersection(p1, p2);
            if (inter == null || inter.Count < 3) return false;

            float interArea = Math.Abs(PolygonArea(inter));
            float minArea = Math.Min(GetArea(r1), GetArea(r2));

            return interArea / minArea >= threshold;
        }
        private static float NormalizeAngle180(float a)
        {
            while (a <= -180f) a += 360f;
            while (a > 180f) a -= 360f;
            return a;
        }

        // Giữ góc mới gần góc tham chiếu nhất (chống lật 180)
        //private static float NormalizeAngleNear(float newAngle, float refAngle)
        //{
        //    newAngle = NormalizeAngle180(newAngle);
        //    refAngle = NormalizeAngle180(refAngle);

        //    float d = newAngle - refAngle;
        //    if (d > 90f) newAngle -= 180f;
        //    else if (d < -90f) newAngle += 180f;

        //    return NormalizeAngle180(newAngle);
        //}
        private static bool IsFinite(float x)
        {
            return !float.IsNaN(x) && !float.IsInfinity(x);
        }
        private static RectRotate MergeRectRotate(
    IEnumerable<RectRotate> rects,
    RectRotate refRect)
        {
            if (refRect == null) return rects.FirstOrDefault(r => r != null);

            float ang = refRect._rectRotation; // 🔒 khóa góc theo refRect (CW)
            double rad = ang * Math.PI / 180.0;
            float c = (float)Math.Cos(rad);
            float s = (float)Math.Sin(rad);

            // trục u (dài) và v (vuông góc)
            // u = ( c, s ), v = (-s, c)
            float minU = float.PositiveInfinity, maxU = float.NegativeInfinity;
            float minV = float.PositiveInfinity, maxV = float.NegativeInfinity;

            foreach (var r in rects)
            {
                if (r == null) continue;
                foreach (var p in GetWorldPoly(r))
                {
                    float u = p.X * c + p.Y * s;
                    float v = -p.X * s + p.Y * c;

                    if (u < minU) minU = u;
                    if (u > maxU) maxU = u;
                    if (v < minV) minV = v;
                    if (v > maxV) maxV = v;
                }
            }

            if (!IsFinite(minU) || !IsFinite(maxU) || !IsFinite(minV) || !IsFinite(maxV))
                return refRect;
            float w = maxU - minU;
            float h = maxV - minV;

            // center trong (u,v)
            float cu = (minU + maxU) * 0.5f;
            float cv = (minV + maxV) * 0.5f;

            // đổi center về (x,y)
            float cx = cu * c - cv * s;
            float cy = cu * s + cv * c;

            return new RectRotate
            {
                _PosCenter = new PointF(cx, cy),
                _rect = new RectangleF(-w * 0.5f, -h * 0.5f, w, h),
                _rectRotation = ang
            };
        }

        //private static RectRotate MergeRectRotate(
        //   IEnumerable<RectRotate> rects,
        //   RectRotate refRect)
        //{
        //    var pts = new List<Point2f>();

        //    foreach (var r in rects)
        //    {
        //        if (r == null) continue;
        //        var poly = GetWorldPoly(r);
        //        foreach (var p in poly)
        //            pts.Add(new Point2f(p.X, p.Y));
        //    }

        //    if (pts.Count < 3)
        //        return refRect;

        //    RotatedRect rr = Cv2.MinAreaRect(pts);

        //    float angle = rr.Angle;          // OpenCV: [-90..0)
        //    float w = rr.Size.Width;
        //    float h = rr.Size.Height;

        //    //// ===== FIX 90° (KHÔNG ĐỔI HƯỚNG) =====
        //    //if (w < h)
        //    //{
        //    //    angle += 90f;
        //    //    (w, h) = (h, w);
        //    //}

        //    //// ===== FIX 180° (KHÓA HƯỚNG THEO RECT GỐC) =====
        //    //if (refRect != null)
        //    //    angle = NormalizeAngleNear(angle, refRect._rectRotation);
        //    //else
        //        angle = NormalizeAngle180(angle);

        //    return new RectRotate
        //    {
        //        _PosCenter = new PointF(rr.Center.X, rr.Center.Y),

        //        // ⚠️ RectRotate của BeeCore: rect local quanh center
        //        _rect = new RectangleF(
        //            -w * 0.5f,
        //            -h * 0.5f,
        //            w,
        //            h),

        //        _rectRotation = angle
        //    };
        //}



        public static List<ResultItem> MergeSameNameOverlap(
          IList<ResultItem> list,
          float overlapThreshold)
        {
            if (list == null || list.Count == 0)
                return new List<ResultItem>();

            if (overlapThreshold > 1f) overlapThreshold /= 100f;

            var result = new List<ResultItem>();
            var groups = list.GroupBy(x => x.Name);

            foreach (var g in groups)
            {
                var items = g.ToList();
                var used = new bool[items.Count];

                for (int i = 0; i < items.Count; i++)
                {
                    if (used[i]) continue;

                    var baseItem = items[i];
                    used[i] = true;

                    // ✅ danh sách rect được gộp vào baseItem
                    var mergedRects = new List<RectRotate>();
                    if (baseItem.rot != null) mergedRects.Add(baseItem.rot);

                    for (int j = i + 1; j < items.Count; j++)
                    {
                        if (used[j]) continue;

                        if (IsOverlapEnough(baseItem.rot, items[j].rot, overlapThreshold))
                        {
                            if (items[j].rot != null) mergedRects.Add(items[j].rot);

                            baseItem.matProcess = MergeMat(baseItem.matProcess, items[j].matProcess);

                            baseItem.Score = Math.Max(baseItem.Score, items[j].Score);
                            baseItem.Area += items[j].Area;
                            baseItem.Percent = Math.Max(baseItem.Percent, items[j].Percent);

                            used[j] = true;
                        }
                    }

                    // ✅ cuối cùng tạo 1 rect bao tất cả (và khóa hướng theo rect ban đầu)
                    if (mergedRects.Count > 0)
                    {

                        RectRotate refRect = baseItem.rot;   // 👈 GIỮ NGUYÊN, KHÔNG ĐỤNG
                        baseItem.rot = MergeRectRotate(mergedRects, refRect);
                    }
                      baseItem.rot = MergeRectRotate(mergedRects, baseItem.rot);

                    result.Add(baseItem);
                }
            }

            return result;
        }


        private static Mat MergeMat(Mat a, Mat b)
        {
            if (a == null) return b?.Clone();
            if (b == null) return a;

            // resize cho cùng size nếu cần
            if (a.Size() != b.Size())
            {
                Mat b2 = new Mat();
                Cv2.Resize(b, b2, a.Size());
                b = b2;
            }

            Mat outMat = new Mat();
            Cv2.BitwiseOr(a, b, outMat);
            return outMat;
        }
        // ====
        // HÀM LỌC CHÍNH
        // ====
        public static List<ResultItem> FilterRectRotate(
                IList<ResultItem> list,
                float threshold)
            {
                if (list == null || list.Count <= 1)
                    return new List<ResultItem>(list);

                // Nếu nhập 80 thì đổi thành 0.8
                if (threshold > 1f) threshold /= 100f;

                // Bỏ item không có rect
                var items = new List<(ResultItem item, float area)>();
                foreach (var it in list)
                {
                    if (it.rot != null)
                    {
                        float area = GetArea(it.rot);
                        if (area > 0)
                            items.Add((it, area));
                    }
                }

                // Sort theo diện tích từ nhỏ → lớn
                items.Sort((a, b) => a.area.CompareTo(b.area));

                bool[] removed = new bool[items.Count];

                for (int i = 0; i < items.Count; i++)
                {
                    if (removed[i]) continue;

                    var small = items[i];
                    var polySmall = GetWorldPoly(small.item.rot);

                    for (int j = i + 1; j < items.Count; j++)
                    {
                        if (removed[j]) continue;

                        var big = items[j];
                        if (big.area < small.area) continue;

                        var polyBig = GetWorldPoly(big.item.rot);
                        var inter = PolygonIntersection(polySmall, polyBig);
                        if (inter == null || inter.Count < 3) continue;

                        float interArea = Math.Abs(PolygonArea(inter));
                        float ratio = interArea / small.area;

                        if (ratio >= threshold)
                        {
                            removed[i] = true; // small bị loại
                            break;
                        }
                    }
                }

                // Trả kết quả
                var outList = new List<ResultItem>();
                for (int i = 0; i < items.Count; i++)
                {
                    if (!removed[i])
                        outList.Add(items[i].item);
                }
                return outList;
            }


            // ====
            // HELPER TÍNH DIỆN TÍCH RECTROTATE
            // ====
            private static float GetArea(RectRotate r)
            {
                return Math.Abs(r._rect.Width * r._rect.Height);
            }


            // ====
            // LẤY 4 GÓC RECT XOAY
            // ====
            //private static List<PointF> GetWorldPoly(RectRotate r)
            //{
            //    float w = r._rect.Width;
            //    float h = r._rect.Height;
            //    float hw = w * 0.5f;
            //    float hh = h * 0.5f;

            //    PointF[] local =
            //    {
            //    new PointF(-hw, -hh),
            //    new PointF(+hw, -hh),
            //    new PointF(+hw, +hh),
            //    new PointF(-hw, +hh)
            //};

            //    var world = new List<PointF>(4);
            //    for (int i = 0; i < 4; i++)
            //    {
            //        PointF pr = Rotate(local[i], r._rectRotation);
            //        world.Add(Add(pr, r._PosCenter));
            //    }
            //    return world;
            //}
        private static List<PointF> GetWorldPoly(RectRotate r)
        {
            float w = r._rect.Width;
            float h = r._rect.Height;
            float hw = w * 0.5f;
            float hh = h * 0.5f;

            PointF[] local =
            {
        new PointF(-hw, -hh),
        new PointF(+hw, -hh),
        new PointF(+hw, +hh),
        new PointF(-hw, +hh)
    };

            var world = new List<PointF>(4);
            for (int i = 0; i < 4; i++)
            {
                PointF pr = Rotate(local[i], r._rectRotation);
                world.Add(new PointF(
                    pr.X + r._PosCenter.X,
                    pr.Y + r._PosCenter.Y));
            }
            return world;
        }

        private static PointF Add(PointF a, PointF b)
            {
                return new PointF(a.X + b.X, a.Y + b.Y);
            }

            private static PointF Rotate(PointF p, float deg)
            {
                double rad = deg * Math.PI / 180.0;
                double c = Math.Cos(rad);
                double s = Math.Sin(rad);

                return new PointF(
                    (float)(p.X * c - p.Y * s),
                    (float)(p.X * s + p.Y * c)
                );
            }


            // ====
            // POLYGON – DIỆN TÍCH SHOELACE
            // ====
            private static float PolygonArea(IList<PointF> poly)
            {
                double sum = 0;
                for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
                {
                    var pi = poly[i];
                    var pj = poly[j];
                    sum += pj.X * pi.Y - pi.X * pj.Y;
                }
                return (float)(sum * 0.5);
            }


            // ====
            // CLIP POLYGON – TÍNH GIAO 2 RECT XOAY
            // ====
            private static float Cross(PointF a, PointF b, PointF p)
            {
                return (b.X - a.X) * (p.Y - a.Y) -
                       (b.Y - a.Y) * (p.X - a.X);
            }

            private static bool Inside(PointF a, PointF b, PointF p)
            {
                return Cross(a, b, p) >= 0;
            }

            private static PointF Intersect(PointF s, PointF e, PointF a, PointF b)
            {
                float dS = Cross(a, b, s);
                float dE = Cross(a, b, e);
                float t = dS / (dS - dE);

                return new PointF(
                    s.X + t * (e.X - s.X),
                    s.Y + t * (e.Y - s.Y)
                );
            }

            private static List<PointF> ClipEdge(List<PointF> poly, PointF a, PointF b)
            {
                var outp = new List<PointF>();
                int n = poly.Count;
                PointF s = poly[n - 1];
                bool sIn = Inside(a, b, s);

                for (int i = 0; i < n; i++)
                {
                    PointF e = poly[i];
                    bool eIn = Inside(a, b, e);

                    if (eIn)
                    {
                        if (!sIn) outp.Add(Intersect(s, e, a, b));
                        outp.Add(e);
                    }
                    else if (sIn)
                    {
                        outp.Add(Intersect(s, e, a, b));
                    }

                    s = e;
                    sIn = eIn;
                }
                return outp;
            }

            private static List<PointF> PolygonIntersection(IList<PointF> p1, IList<PointF> p2)
            {
                var outp = new List<PointF>(p1);
                for (int i = 0; i < p2.Count; i++)
                {
                    PointF a = p2[i];
                    PointF b = p2[(i + 1) % p2.Count];
                    outp = ClipEdge(outp, a, b);
                    if (outp.Count == 0) break;
                }
                return outp;
            }
        }
    }


