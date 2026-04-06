using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BeeCore
{
    public static class ResultFilter
    {
        // ==============================
        // MERGE CÁC BOX CÙNG TÊN BỊ CHỒNG / NẰM TRONG NHAU
        // ==============================
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

                    var group = new List<int>();
                    var queue = new Queue<int>();

                    queue.Enqueue(i);
                    used[i] = true;

                    while (queue.Count > 0)
                    {
                        int cur = queue.Dequeue();
                        group.Add(cur);

                        for (int j = 0; j < items.Count; j++)
                        {
                            if (used[j]) continue;

                            if (IsOverlapEnough(items[cur].rot, items[j].rot, overlapThreshold))
                            {
                                used[j] = true;
                                queue.Enqueue(j);
                            }
                        }
                    }

                    // ===== merge toàn bộ group =====
                    var baseItem = items[group[0]];

                    var mergedRects = new List<RectRotate>();
                    foreach (var idx in group)
                    {
                        var it = items[idx];

                        if (it.rot != null)
                            mergedRects.Add(it.rot);

                        if (idx == group[0]) continue;

                        baseItem.matProcess = MergeMat(baseItem.matProcess, it.matProcess);
                        baseItem.Score = Math.Max(baseItem.Score, it.Score);
                        baseItem.Area += it.Area;
                        baseItem.Percent = Math.Max(baseItem.Percent, it.Percent);
                    }

                    if (mergedRects.Count > 0)
                    {
                        baseItem.rot = MergeRectRotate(mergedRects, baseItem.rot);
                    }

                    result.Add(baseItem);
                }
                //for (int i = 0; i < items.Count; i++)
                //{
                //    if (used[i]) continue;

                //    var baseItem = items[i];
                //    used[i] = true;

                //    var mergedRects = new List<RectRotate>();
                //    if (baseItem.rot != null)
                //        mergedRects.Add(baseItem.rot);

                //    for (int j = i + 1; j < items.Count; j++)
                //    {
                //        if (used[j]) continue;

                //        if (IsOverlapEnough(baseItem.rot, items[j].rot, overlapThreshold))
                //        {
                //            if (items[j].rot != null)
                //                mergedRects.Add(items[j].rot);

                //            baseItem.matProcess = MergeMat(baseItem.matProcess, items[j].matProcess);
                //            baseItem.Score = Math.Max(baseItem.Score, items[j].Score);
                //            baseItem.Area += items[j].Area;
                //            baseItem.Percent = Math.Max(baseItem.Percent, items[j].Percent);

                //            used[j] = true;
                //        }
                //    }

                //    if (mergedRects.Count > 0)
                //    {
                //        RectRotate refRect = baseItem.rot;
                //        baseItem.rot = MergeRectRotate(mergedRects, refRect);
                //    }

                //    result.Add(baseItem);
                //}
            }

            return result;
        }

        // ==============================
        // LỌC BOX NHỎ NẰM TRONG BOX LỚN
        // ==============================
        public static List<ResultItem> FilterRectRotate(
            IList<ResultItem> list,
            float threshold)
        {
            if (list == null || list.Count == 0)
                return new List<ResultItem>();

            if (list.Count == 1)
                return new List<ResultItem>(list);

            if (threshold > 1f) threshold /= 100f;

            var items = new List<(ResultItem item, float area)>();
            foreach (var it in list)
            {
                if (it?.rot == null) continue;

                float area = GetArea(it.rot);
                if (area > 0)
                    items.Add((it, area));
            }

            if (items.Count <= 1)
                return items.Select(x => x.item).ToList();

            // nhỏ -> lớn
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

                    // fallback nếu polygon intersection không ra đẹp
                    if (inter == null || inter.Count < 3)
                    {
                        if (IsPolygonInside(polySmall, polyBig))
                        {
                            removed[i] = true;
                            break;
                        }
                        continue;
                    }

                    float interArea = Math.Abs(PolygonArea(inter));
                    float ratio = small.area > 0 ? interArea / small.area : 0f;

                    if (ratio >= threshold)
                    {
                        removed[i] = true;
                        break;
                    }
                }
            }

            var outList = new List<ResultItem>();
            for (int i = 0; i < items.Count; i++)
            {
                if (!removed[i])
                    outList.Add(items[i].item);
            }

            return outList;
        }

        // ==============================
        // CHECK OVERLAP ĐỦ ĐỂ MERGE
        // ==============================
        private static bool IsOverlapEnough(
            RectRotate r1,
            RectRotate r2,
            float threshold)
        {
            if (r1 == null || r2 == null) return false;

            var p1 = GetWorldPoly(r1);
            var p2 = GetWorldPoly(r2);

            var inter = PolygonIntersection(p1, p2);

            if (inter == null || inter.Count < 3)
            {
                if (IsPolygonInside(p1, p2) || IsPolygonInside(p2, p1))
                    return true;

                return false;
            }

            float interArea = Math.Abs(PolygonArea(inter));
            float minArea = Math.Min(GetArea(r1), GetArea(r2));
            if (minArea <= 0) return false;

            float ratio = interArea / minArea;
            return ratio >= threshold;
        }

        // ==============================
        // MERGE RECTROTATE, GIỮ GÓC THEO RECT GỐC
        // ==============================
        private static RectRotate MergeRectRotate(
            IEnumerable<RectRotate> rects,
            RectRotate refRect)
        {
            if (refRect == null)
                return rects.FirstOrDefault(r => r != null);

            float ang = refRect._rectRotation;
            double rad = ang * Math.PI / 180.0;
            float c = (float)Math.Cos(rad);
            float s = (float)Math.Sin(rad);

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
                    if (v < minV) minV = u < minU ? minV : Math.Min(minV, v);
                    if (v > maxV) maxV = Math.Max(maxV, v);
                }
            }

            if (!IsFinite(minU) || !IsFinite(maxU) || !IsFinite(minV) || !IsFinite(maxV))
                return refRect;

            float w = maxU - minU;
            float h = maxV - minV;

            float cu = (minU + maxU) * 0.5f;
            float cv = (minV + maxV) * 0.5f;

            float cx = cu * c - cv * s;
            float cy = cu * s + cv * c;

            return new RectRotate
            {
                _PosCenter = new PointF(cx, cy),
                _rect = new RectangleF(-w * 0.5f, -h * 0.5f, w, h),
                _rectRotation = ang
            };
        }

        // ==============================
        // MERGE 2 MASK
        // ==============================
        private static Mat MergeMat(Mat a, Mat b)
        {
            if (a == null) return b?.Clone();
            if (b == null) return a;

            if (a.Size() != b.Size())
            {
                using (Mat b2 = new Mat())
                {
                    Cv2.Resize(b, b2, a.Size());

                    Mat outResize = new Mat();
                    Cv2.BitwiseOr(a, b2, outResize);
                    return outResize;
                }
            }

            Mat outMat = new Mat();
            Cv2.BitwiseOr(a, b, outMat);
            return outMat;
        }

        // ==============================
        // GEOMETRY
        // ==============================
        private static float GetArea(RectRotate r)
        {
            if (r == null) return 0f;
            return Math.Abs(r._rect.Width * r._rect.Height);
        }

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
            foreach (var p in local)
            {
                PointF pr = Rotate(p, r._rectRotation);
                world.Add(new PointF(
                    pr.X + r._PosCenter.X,
                    pr.Y + r._PosCenter.Y));
            }

            return world;
        }

        private static PointF Rotate(PointF p, float deg)
        {
            double rad = deg * Math.PI / 180.0;
            double c = Math.Cos(rad);
            double s = Math.Sin(rad);

            return new PointF(
                (float)(p.X * c - p.Y * s),
                (float)(p.X * s + p.Y * c));
        }

        private static float PolygonArea(IList<PointF> poly)
        {
            if (poly == null || poly.Count < 3) return 0f;

            double sum = 0;
            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                var pi = poly[i];
                var pj = poly[j];
                sum += pj.X * pi.Y - pi.X * pj.Y;
            }
            return (float)(sum * 0.5);
        }

        // ==============================
        // POLYGON INTERSECTION
        // ==============================
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
            float denom = dS - dE;

            if (Math.Abs(denom) < 1e-6f)
                return e;

            float t = dS / denom;

            return new PointF(
                s.X + t * (e.X - s.X),
                s.Y + t * (e.Y - s.Y));
        }

        private static List<PointF> ClipEdge(List<PointF> poly, PointF a, PointF b)
        {
            var outp = new List<PointF>();
            if (poly == null || poly.Count == 0) return outp;

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
            if (p1 == null || p2 == null || p1.Count < 3 || p2.Count < 3)
                return new List<PointF>();

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

        // ==============================
        // CONTAINMENT
        // ==============================
        private static bool IsPolygonInside(List<PointF> small, List<PointF> big)
        {
            if (small == null || big == null || small.Count == 0 || big.Count < 3)
                return false;

            foreach (var p in small)
            {
                if (!PointInPolygonOrOnEdge(p, big))
                    return false;
            }
            return true;
        }

        private static bool PointInPolygonOrOnEdge(PointF p, List<PointF> poly)
        {
            if (PointOnPolygonEdge(p, poly))
                return true;

            return PointInPolygon(p, poly);
        }

        private static bool PointInPolygon(PointF p, List<PointF> poly)
        {
            bool inside = false;

            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                var pi = poly[i];
                var pj = poly[j];

                bool cross =
                    ((pi.Y > p.Y) != (pj.Y > p.Y)) &&
                    (p.X < (pj.X - pi.X) * (p.Y - pi.Y) / ((pj.Y - pi.Y) + 1e-6f) + pi.X);

                if (cross)
                    inside = !inside;
            }

            return inside;
        }

        private static bool PointOnPolygonEdge(PointF p, List<PointF> poly, float eps = 1.5f)
        {
            for (int i = 0; i < poly.Count; i++)
            {
                var a = poly[i];
                var b = poly[(i + 1) % poly.Count];
                if (DistancePointToSegment(p, a, b) <= eps)
                    return true;
            }
            return false;
        }

        private static float DistancePointToSegment(PointF p, PointF a, PointF b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;

            if (Math.Abs(dx) < 1e-6f && Math.Abs(dy) < 1e-6f)
            {
                float dpx = p.X - a.X;
                float dpy = p.Y - a.Y;
                return (float)Math.Sqrt(dpx * dpx + dpy * dpy);
            }

            float t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0f, Math.Min(1f, t));

            float px = a.X + t * dx;
            float py = a.Y + t * dy;

            float ddx = p.X - px;
            float ddy = p.Y - py;

            return (float)Math.Sqrt(ddx * ddx + ddy * ddy);
        }

        private static bool IsFinite(float x)
        {
            return !float.IsNaN(x) && !float.IsInfinity(x);
        }
    }
}