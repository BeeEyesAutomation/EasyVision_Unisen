using BeeGlobal;
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
            // ================================
            // HÀM LỌC CHÍNH
            // ================================
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


            // ================================
            // HELPER TÍNH DIỆN TÍCH RECTROTATE
            // ================================
            private static float GetArea(RectRotate r)
            {
                return Math.Abs(r._rect.Width * r._rect.Height);
            }


            // ================================
            // LẤY 4 GÓC RECT XOAY
            // ================================
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
                    world.Add(Add(pr, r._PosCenter));
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


            // ================================
            // POLYGON – DIỆN TÍCH SHOELACE
            // ================================
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


            // ================================
            // CLIP POLYGON – TÍNH GIAO 2 RECT XOAY
            // ================================
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


