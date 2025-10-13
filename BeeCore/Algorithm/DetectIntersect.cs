using BeeCore.Algorithm;
using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Flags]
    public enum DrawFlags
    {
        None = 0,
        RansacRejected = 1 << 0,  // line bị RANSAC loại
        RansacAccepted = 1 << 1,  // line RANSAC chấp nhận (thô)
        Inliers = 1 << 2,  // vẽ các inlier/contiguous points
        ContinuityRejected = 1 << 3,  // line bị loại ở bước tính liên tục
        ContinuityAccepted = 1 << 4,  // line qua kiểm tra tính liên tục
        Runs = 1 << 5,  // các đoạn run P0-P1
        PairRejectedAngle = 1 << 6,  // cặp bị loại do góc
        PairRejectedParallel = 1 << 7,  // cặp bị loại do song song
        PairRejectedOther = 1 << 8,  // cặp bị loại lý do khác
        BestLines = 1 << 9,  // 2 line tốt nhất
        BestCorner = 1 << 10, // điểm giao tốt nhất
        Labels = 1 << 11, // chữ/nhãn (vd: góc)

        // Gói gọn cho tiện
        PairRejectedAll = PairRejectedAngle | PairRejectedParallel | PairRejectedOther,
        BestAll = BestLines | BestCorner | Labels,
        All = RansacRejected | RansacAccepted | Inliers | ContinuityRejected |
                                 ContinuityAccepted | Runs | PairRejectedAll | BestAll
    }
    public sealed class DrawStyle
    {
        public Color LineResult { get; set; } = Color.Lime;       // best lines
        public Color LineChoose { get; set; } = Color.Gold;       // best lines
        public Color LineNone { get; set; } = Color.DodgerBlue;  // others
        public Color Inlier { get; set; } = Color.Orange;      // inlier points
        public int Thickness { get; set; } = 2;                 // line thickness
        public int InlierSize { get; set; } = 1;                 // inlier size (px)
        public DashStyle LineDash { get; set; } = DashStyle.Solid; // Solid/Dash/Dot...

        public static DrawStyle Default => new DrawStyle();
    }
    public struct OrthCornerOptions
    {
        public int MaxCandidateLines;
        public int RansacIterations;
        public double RansacThreshold;
        public int MinInliersPerLine;
        public int FixMean;
        // Cắt đoạn liên tục: splitThr = ContinuityGapFactor * meanFiltered
        public double ContinuityGapFactor;
        // Two-stage mean (no-trim). Nếu NearQuantile <= 0 → dùng median thay thế
        public double NearQuantile;    // vd 0.60  (để tắt đặt = 0)
        public double NearMultiplier;  // vd 2.5
        public bool AutoMean ;
        public double AngleTargetDeg;
        public double AngleToleranceDeg;

        public static OrthCornerOptions Default => new OrthCornerOptions
        {
            MaxCandidateLines = 200,
            RansacIterations = 1200,
            RansacThreshold = 1.5,
            MinInliersPerLine = 20,
            ContinuityGapFactor = 1.2,
            AutoMean=true,
            FixMean=3,
            NearQuantile = 0.60,  // bật two-stage mean
            NearMultiplier = 2.5,

            AngleTargetDeg = 90.0,
            AngleToleranceDeg = 20.0
           
        };
    }

    public sealed class DetectIntersect
    {
        // Vẽ theo cờ, giới hạn bởi dest
        public void RenderDebugToGraphics(Graphics g, RectangleF dest, LineEdge dbg, DrawFlags flags, DrawStyle style)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));
            if (dbg == null) throw new ArgumentNullException(nameof(dbg));
            if (style == null) style = DrawStyle.Default;

            var oldSmoothing = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Pens/Brush theo style
            using (var penNone = new Pen(style.LineNone, Math.Max(1, style.Thickness)) { DashStyle = style.LineDash })
            using (var penChoose = new Pen(style.LineChoose, Math.Max(1, style.Thickness + 1)) { DashStyle = DashStyle.Solid })
            using (var penResult = new Pen(style.LineResult, Math.Max(1, style.Thickness + 1)) { DashStyle = DashStyle.Solid })
            using (var brushInlier = new SolidBrush(style.Inlier))
            using (var brushLabel = new SolidBrush(style.LineChoose))
            using (var font = new Font("Segoe UI", 10f))
            {
                // 1) RANSAC rejected
                if (Has(flags, DrawFlags.RansacRejected))
                    foreach (var rr in dbg.RansacRejectedRaw)
                        DrawLineClipped(g, rr.ln, dest, penNone);

                // 2) RANSAC accepted + inliers
                if (Has(flags, DrawFlags.Inliers))
                    foreach (var ar in dbg.RansacAcceptedRaw)
                    {
                        if (ar.inl != null)
                            DrawPointsClipped(g, ar.inl, dest, brushInlier, Math.Max(2, style.InlierSize));
                    }
                if (Has(flags, DrawFlags.RansacAccepted))
                    foreach (var ar in dbg.RansacAcceptedRaw)
                    {
                        DrawLineClipped(g, ar.ln, dest, penNone);
                        //if (Has(flags, DrawFlags.Inliers) && ar.inl != null)
                        //    DrawPointsClipped(g, ar.inl, dest, brushInlier, Math.Max(2, style.InlierSize));
                    }

                // 3) Continuity rejected
                if (Has(flags, DrawFlags.ContinuityRejected))
                    foreach (var cr in dbg.ContinuityRejected)
                        DrawLineClipped(g, cr.ln, dest, penNone);

                // 4) Continuity accepted + inliers
                if (Has(flags, DrawFlags.ContinuityAccepted))
                    foreach (var ca in dbg.ContinuityAccepted)
                    {
                        DrawLineClipped(g, ca.ln, dest, penNone);
                        if (Has(flags, DrawFlags.Inliers) && ca.contiguous != null)
                            DrawPointsClipped(g, ca.contiguous, dest, brushInlier, Math.Max(2, style.InlierSize));
                    }

                // 5) Runs (clip segment)
                if (Has(flags, DrawFlags.Runs))
                    foreach (var r in dbg.RunsKept)
                    {
                        var p0 = new PointF(r.P0.X, r.P0.Y);
                        var p1 = new PointF(r.P1.X, r.P1.Y);
                        PointF q0, q1;
                        if (ClipSegmentToRect(p0, p1, dest, out q0, out q1))
                            g.DrawLine(penChoose, q0, q1);
                    }

                // 6) PairRejected* — vẽ cùng màu LineNone (không chọn)
                if (HasAny(flags, DrawFlags.PairRejectedAll))
                    foreach (var pr in dbg.PairRejected)
                    {
                        // Lọc theo cờ chi tiết
                        bool draw = (pr.reason.StartsWith("angle") && Has(flags, DrawFlags.PairRejectedAngle))
                                  || (pr.reason.StartsWith("parallel") && Has(flags, DrawFlags.PairRejectedParallel))
                                  || (!pr.reason.StartsWith("angle") && !pr.reason.StartsWith("parallel") && Has(flags, DrawFlags.PairRejectedOther));
                        if (!draw) continue;

                        DrawLineClipped(g, pr.A.Line, dest, penNone);
                        DrawLineClipped(g, pr.B.Line, dest, penNone);

                        if (pr.hasP && Has(flags, DrawFlags.Inliers) && dest.Contains(pr.P.X, pr.P.Y))
                            FillCircle(g, brushInlier, new PointF(pr.P.X, pr.P.Y), Math.Max(2, style.InlierSize + 1));
                    }

                // 7) Best — 2 đường chọn dùng LineChoose, corner + label theo LineChoose
                if (dbg.Best.Found)
                {
                    if (Has(flags, DrawFlags.BestLines))
                    {
                        DrawLineClipped(g, dbg.Best.L1, dest, penResult);
                        DrawLineClipped(g, dbg.Best.L2, dest, penResult);
                    }
                    if (Has(flags, DrawFlags.BestCorner) && dest.Contains(dbg.Best.Corner.X, dbg.Best.Corner.Y))
                        FillCircle(g, brushInlier, new PointF(dbg.Best.Corner.X, dbg.Best.Corner.Y), Math.Max(4, style.InlierSize + 2));

                    if (Has(flags, DrawFlags.Labels))
                    {
                        var pc = new PointF(dbg.Best.Corner.X, dbg.Best.Corner.Y);
                        if (dest.Contains(pc))
                        {
                            var txt = $"{dbg.Best.AngleDeg:0.1}°";
                            g.DrawString(txt, font, brushLabel, new PointF(pc.X + 8, pc.Y - 8));
                        }
                    }
                }
            }

            g.SmoothingMode = oldSmoothing;
        }
        // ====== Helpers: vẽ line vô hạn nhưng cắt theo clip ======
        // Trong class DetectIntersect
        private static bool Has(DrawFlags flags, DrawFlags bit)
        {
            return (flags & bit) == bit;
        }

        private static bool HasAny(DrawFlags flags, DrawFlags bits)
        {
            return (flags & bits) != 0;
        }
        // Clip segment p0-p1 vào rect (Liang–Barsky)
        private static bool ClipSegmentToRect(PointF p0, PointF p1, RectangleF rc, out PointF q0, out PointF q1)
        {
            float dx = p1.X - p0.X, dy = p1.Y - p0.Y;
            float t0 = 0f, t1 = 1f;

            if (!ClipTest(-dx, p0.X - rc.Left, ref t0, ref t1)) { q0 = q1 = default(PointF); return false; }
            if (!ClipTest(dx, rc.Right - p0.X, ref t0, ref t1)) { q0 = q1 = default(PointF); return false; }
            if (!ClipTest(-dy, p0.Y - rc.Top, ref t0, ref t1)) { q0 = q1 = default(PointF); return false; }
            if (!ClipTest(dy, rc.Bottom - p0.Y, ref t0, ref t1)) { q0 = q1 = default(PointF); return false; }

            q0 = new PointF(p0.X + t0 * dx, p0.Y + t0 * dy);
            q1 = new PointF(p0.X + t1 * dx, p0.Y + t1 * dy);
            return true;

            bool ClipTest(float p, float q, ref float tE, ref float tL)
            {
                if (Math.Abs(p) < 1e-6f)
                    return q >= 0; // song song và nằm trong biên
                float r = q / p;
                if (p < 0) { if (r > tL) return false; if (r > tE) tE = r; }
                else { if (r < tE) return false; if (r < tL) tL = r; }
                return true;
            }
        }

        // Vẽ inlier nếu nằm trong dest
        private static void DrawPointsClipped(Graphics g, IEnumerable<OpenCvSharp.Point2f> pts,
                                       RectangleF rc, Brush br, float size)
        {
            float r = size * 0.5f;
            var buf = new List<RectangleF>(1024);
            foreach (var p in pts)
            {
                if (p.X < rc.Left || p.X > rc.Right || p.Y < rc.Top || p.Y > rc.Bottom) continue;
                buf.Add(new RectangleF(p.X - r, p.Y - r, size, size));
                if (buf.Count == 1024)
                {
                    g.FillRectangles(br, buf.ToArray());
                    buf.Clear();
                }
            }
            if (buf.Count != 0) g.FillRectangles(br, buf.ToArray());
        }
        private static void DrawLineClipped(Graphics g, Line2D ln, RectangleF clip, Pen pen)
        {
            PointF a, b;
            if (!TryIntersectLineWithRect(ln, clip, out a, out b))
                return; // line không cắt vùng vẽ
            g.DrawLine(pen, a, b);
        }
        private static bool TryIntersectLineWithRect(Line2D ln, RectangleF rc, out PointF A, out PointF B)
        {
            const float EPS = 1e-6f;
            A = default(PointF);
            B = default(PointF);

            if (ln == null) return false;
            float vx =(float) ln.Vx, vy = (float)ln.Vy, x0 = (float)ln.X1, y0 = (float)ln.Y1;
            if (Math.Abs(vx) < EPS && Math.Abs(vy) < EPS) return false;

            // Tìm trực tiếp tMin/tMax của 2 giao điểm nằm TRONG biên
            // Không cần mảng/Sort — chỉ theo dõi 2 đầu mút
            float tMin = float.PositiveInfinity, tMax = float.NegativeInfinity;
            int hit = 0;

            // x = L
            if (Math.Abs(vx) >= EPS)
            {
                float t = (rc.Left - x0) / vx;
                float y = y0 + t * vy;
                if (y >= rc.Top - 1 && y <= rc.Bottom + 1) { if (t < tMin) tMin = t; if (t > tMax) tMax = t; hit++; }
                // x = R
                t = (rc.Right - x0) / vx;
                y = y0 + t * vy;
                if (y >= rc.Top - 1 && y <= rc.Bottom + 1) { if (t < tMin) tMin = t; if (t > tMax) tMax = t; hit++; }
            }
            // y = T
            if (Math.Abs(vy) >= EPS)
            {
                float t = (rc.Top - y0) / vy;
                float x = x0 + t * vx;
                if (x >= rc.Left - 1 && x <= rc.Right + 1) { if (t < tMin) tMin = t; if (t > tMax) tMax = t; hit++; }
                // y = B
                t = (rc.Bottom - y0) / vy;
                x = x0 + t * vx;
                if (x >= rc.Left - 1 && x <= rc.Right + 1) { if (t < tMin) tMin = t; if (t > tMax) tMax = t; hit++; }
            }

            if (hit < 2 || float.IsInfinity(tMin) || float.IsInfinity(tMax)) return false;

            A = new PointF(x0 + tMin * vx, y0 + tMin * vy);
            B = new PointF(x0 + tMax * vx, y0 + tMax * vy);
            return true;
        }

        //private static bool TryIntersectLineWithRect(Line2D ln, RectangleF rc, out PointF A, out PointF B)
        //{
        //    // BẮT BUỘC: khởi tạo out trước mọi return
        //    A = default(PointF);
        //    B = default(PointF);


        //    if (ln == null)
        //        return false;
        //    // Nếu vector hướng quá nhỏ, coi như không vẽ
        //    if (Math.Abs(ln.Vx) < 1e-6f && Math.Abs(ln.Vy) < 1e-6f)
        //        {
        //            A = B = default(PointF);
        //            return false;
        //        }

        //        var tList = new List<float>(4);

        //        // Các biên: x = L/R, y = T/B
        //        if (Math.Abs(ln.Vx) > 1e-6f)
        //        {
        //            double tL = ((rc.Left - ln.X1) / ln.Vx);
        //            double yL = (ln.Y1 + tL * ln.Vy);
        //            if (yL >= rc.Top - 1 && yL <= rc.Bottom + 1) tList.Add((float)tL);

        //            double tR = ((rc.Right - ln.X1) / ln.Vx);
        //            double yR = (ln.Y1 + tR * ln.Vy);
        //            if (yR >= rc.Top - 1 && yR <= rc.Bottom + 1) tList.Add((float)tR);
        //        }
        //        if (Math.Abs(ln.Vy) > 1e-6f)
        //        {
        //            double tT = (rc.Top - ln.Y1) / ln.Vy;
        //            double xT = ln.X1 + tT * ln.Vx;
        //            if (xT >= rc.Left - 1 && xT <= rc.Right + 1) tList.Add((float)tT);

        //            double tB = (rc.Bottom - ln.Y1) / ln.Vy;
        //            double xB = ln.X1 + tB * ln.Vx;
        //            if (xB >= rc.Left - 1 && xB <= rc.Right + 1) tList.Add((float)tB);
        //        }

        //        if (tList.Count < 2)
        //        {
        //            A = B = default(PointF);
        //            return false;
        //        }

        //        tList.Sort();
        //        float t0 = tList[0];
        //        float t1 = tList[tList.Count - 1];

        //        // Hai điểm xa nhất trong clip
        //        A = new PointF((float)(ln.X1 + t0 * ln.Vx), (float)(ln.Y1 + t0 * ln.Vy));
        //        B = new PointF((float)(ln.X1 + t1 * ln.Vx), (float)(ln.Y1 + t1 * ln.Vy));

        //        return true;
           
        //}

        // Lấy tham số line (phù hợp với cách bạn tạo Line2D(vx, vy, x, y))
        private static void GetLineParams(Line2D ln, out float x0, out float y0, out float vx, out float vy)
        {
            // Dùng dynamic để tương thích các phiên bản OpenCvSharp Line2D khác nhau
            // (tránh lỗi nếu field/property có tên khác nhau giữa bản lib)
            dynamic d = ln;
            // Ưu tiên property nếu có; fallback sang field bằng try-catch
            try { vx = (float)d.Vx; } catch { vx = (float)d.VX; }
            try { vy = (float)d.Vy; } catch { vy = (float)d.VY; }
            try { x0 = (float)d.X; } catch { x0 = (float)d.Px; }
            try { y0 = (float)d.Y; } catch { y0 = (float)d.Py; }
        }

        // Vẽ các điểm inlier nhỏ (hình vuông/điểm tròn đều được)
        private static void DrawPoints(Graphics g, IEnumerable<OpenCvSharp.Point2f> pts, Brush br, float size)
        {
            float r = size * 0.5f;
            foreach (var p in pts)
            {
                g.FillRectangle(br, p.X - r, p.Y - r, size, size);
            }
        }
        private static void RemoveSet(List<Point2f> cloud, List<Point2f> rm)
        {
            var kill = new HashSet<long>(rm.Count);
            for (int i = 0; i < rm.Count; i++)
            {
                int xi = (int)Math.Round(rm[i].X);
                int yi = (int)Math.Round(rm[i].Y);
                long key = ((long)yi << 32) | (uint)xi;
                kill.Add(key);
            }
            cloud.RemoveAll(p =>
            {
                int xi = (int)Math.Round(p.X);
                int yi = (int)Math.Round(p.Y);
                long key = ((long)yi << 32) | (uint)xi;
                return kill.Contains(key);
            });
        }

        //private static void RemoveSet(List<Point2f> cloud, List<Point2f> rm)
        //{
        //    var kill = new HashSet<long>();
        //    for (int i = 0; i < rm.Count; i++)
        //    {
        //        int xi = (int)Math.Round(rm[i].X);
        //        int yi = (int)Math.Round(rm[i].Y);
        //        long key = ((long)yi << 32) | (uint)xi;
        //        kill.Add(key);
        //    }
        //    var kept = new List<Point2f>(cloud.Count);
        //    for (int i = 0; i < cloud.Count; i++)
        //    {
        //        int xi = (int)Math.Round(cloud[i].X);
        //        int yi = (int)Math.Round(cloud[i].Y);
        //        long key = ((long)yi << 32) | (uint)xi;
        //        if (!kill.Contains(key)) kept.Add(cloud[i]);
        //    }
        //    cloud.Clear();
        //    cloud.AddRange(kept);
        //}
        private static void FillCircle(Graphics g, Brush br, PointF c, float r)
        {
            g.FillEllipse(br, c.X - r, c.Y - r, 2 * r, 2 * r);
        }
    
        // Helper tạo Line2D đúng thứ tự ctor (vx,vy,x,y)
      
        public LineEdge LineEdge = new LineEdge();
        Stopwatch stopwatch = new Stopwatch();
        // ===== PUBLIC =====
        public CornerResult FindBestCorner_RansacRuns(Mat gray, Mat edges, OrthCornerOptions? opt = null)
        {
            if (gray == null || gray.Empty()) throw new ArgumentException("gray empty");
            var o = opt ?? OrthCornerOptions.Default;

            if (edges == null || edges.Empty())
            {
                edges = new Mat();
                Cv2.Canny(gray, edges, 50, 150, 3, true);
            }
            LineEdge = new LineEdge();
            List<Point2f> cloud = gray.ExtractEdgePoints( edges);
            if (cloud.Count < 2) return new CornerResult { Found = false };
            var remain = new List<Point2f>(cloud);
            var pool = new List<(Line2D ln, List<Point2f> inl)>();
            for (int r = 0; r < o.MaxCandidateLines; r++)
            {
                List<Point2f> contiguous;
                Line2D ln = Ransac.FitLine(remain, o, out contiguous, LineEdge);

                if (contiguous == null || contiguous.Count < o.MinInliersPerLine)
                    break;
          
                pool.Add((ln, contiguous));
                RemoveSet(remain, contiguous);
                if (remain.Count < 8) break;
            }
            if (pool.Count < 2) return new CornerResult { Found = false};
            var runs = BuildRunsFromRansac(pool);
            LineEdge.RunsKept.AddRange(runs);
            if (runs.Count < 2) return new CornerResult { Found = false };

            var best = SelectBestPairFromRuns_WithDebug(runs, gray.Size(), o.AngleTargetDeg, o.AngleToleranceDeg, LineEdge);


           
            return best;
        }

        
        private static List<Seg> BuildRunsFromRansac(List<(Line2D ln, List<Point2f> inl)> pool)
        {
            var runs = new List<Seg>(pool.Count);
            for (int i = 0; i < pool.Count; i++)
            {
                var r = LongestRunOnLine(pool[i].ln, pool[i].inl);
                if (r.Count>= 3 && r.Len >= 2.0)
                {
                    runs.Add(r);
                   
                }
            }
            runs.Sort((a, b) => b.Len.CompareTo(a.Len));
            return runs;
        }
        private static Seg LongestRunOnLine(Line2D ln, List<Point2f> inliers)
        {
            var sr = new Seg { Line = ln, RunPts = new List<Point2f>(), Count = 0, Len = 0, T0 = 0, T1 = 0 };
            if (inliers == null || inliers.Count < 2) return sr;

            int n = inliers.Count;
            var tArr = new double[n];
            var pArr = new Point2f[n];

            double ax = ln.X1, ay = ln.Y1, vx = ln.Vx, vy = ln.Vy;
            for (int i = 0; i < n; i++)
            {
                var pt = inliers[i];
                double t = (pt.X - ax) * vx + (pt.Y - ay) * vy;
                tArr[i] = t;
                pArr[i] = pt;
            }

            // sort theo tArr, sắp xếp đồng thời pArr
            Array.Sort(tArr, pArr);

            // ước lượng median Δt không cần tạo dts list: chỉ scan và “lấy phần tử giữa”
            // (vì đã sort theo tArr, Δt[i] = tArr[i]-tArr[i-1] cũng đã gần-sort theo vị trí)
            double medDt;
            if (n <= 2) medDt = n == 2 ? Math.Max(1.0, tArr[1] - tArr[0]) : 1.0;
            else
            {
                // Lấy Δt ở giữa (xấp xỉ median) — nhanh, đủ tốt để tách gap
                int mid = n / 2;
                medDt = tArr[mid] - tArr[mid - 1];
                if (medDt <= 0) medDt = 1.0;
            }
            double gap = Math.Max(1.0, 3.0 * medDt);

            int bestS = 0, bestE = 0, curS = 0;
            for (int i = 1; i < n; i++)
            {
                double dt = tArr[i] - tArr[i - 1];
                if (dt > gap)
                {
                    if ((tArr[i - 1] - tArr[curS]) > (tArr[bestE] - tArr[bestS]))
                    { bestS = curS; bestE = i - 1; }
                    curS = i;
                }
            }
            if ((tArr[n - 1] - tArr[curS]) > (tArr[bestE] - tArr[bestS]))
            { bestS = curS; bestE = n - 1; }

            int m = bestE - bestS + 1;
            var run = new List<Point2f>(m);
            for (int i = bestS; i <= bestE; i++) run.Add(pArr[i]);

            double t0 = tArr[bestS], t1 = tArr[bestE];
            sr.RunPts = run; sr.Count = m; sr.Len = Math.Max(0.0, t1 - t0); sr.T0 = t0; sr.T1 = t1;
            sr.P0 = new Point2f((float)(ax + t0 * vx), (float)(ay + t0 * vy));
            sr.P1 = new Point2f((float)(ax + t1 * vx), (float)(ay + t1 * vy));
            return sr;
        }

        //private static Seg LongestRunOnLine(Line2D ln, List<Point2f> inliers)
        //{
        //    var sr = new Seg { Line = ln, RunPts = new List<Point2f>(), Count = 0, Len = 0, T0 = 0, T1 = 0 };
        //    if (inliers == null || inliers.Count < 2) return sr;

        //    var arr = new List<Tuple<double, Point2f>>(inliers.Count);
        //    for (int i = 0; i < inliers.Count; i++)
        //    {
        //        double t = (inliers[i].X - ln.X1) * ln.Vx + (inliers[i].Y - ln.Y1) * ln.Vy;
        //        arr.Add(Tuple.Create(t, inliers[i]));
        //    }
        //    arr.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        //    var dts = new List<double>(Math.Max(1, arr.Count - 1));
        //    for (int i = 1; i < arr.Count; i++) dts.Add(arr[i].Item1 - arr[i - 1].Item1);
        //    dts.Sort();
        //    double medDt = dts.Count > 0 ? dts[dts.Count / 2] : 1.0;
        //    double gap = Math.Max(1.0, 3.0 * medDt);

        //    int bestS = 0, bestE = 0, curS = 0;
        //    for (int i = 1; i < arr.Count; i++)
        //    {
        //        double dt = arr[i].Item1 - arr[i - 1].Item1;
        //        if (dt > gap)
        //        {
        //            if ((arr[i - 1].Item1 - arr[curS].Item1) > (arr[bestE].Item1 - arr[bestS].Item1))
        //            { bestS = curS; bestE = i - 1; }
        //            curS = i;
        //        }
        //    }
        //    if ((arr[arr.Count - 1].Item1 - arr[curS].Item1) > (arr[bestE].Item1 - arr[bestS].Item1))
        //    { bestS = curS; bestE = arr.Count - 1; }

        //    var run = new List<Point2f>(bestE - bestS + 1);
        //    for (int i = bestS; i <= bestE; i++) run.Add(arr[i].Item2);

        //    double t0 = arr[bestS].Item1, t1 = arr[bestE].Item1;
        //    double len = Math.Max(0.0, t1 - t0);

        //    sr.RunPts = run; sr.Count= run.Count; sr.Len = len; sr.T0 = t0; sr.T1 = t1;
        //    sr.P0 = new Point2f((float)(ln.X1 + t0 * ln.Vx), (float)(ln.Y1 + t0 * ln.Vy));
        //    sr.P1 = new Point2f((float)(ln.X1 + t1 * ln.Vx), (float)(ln.Y1 + t1 * ln.Vy));
        //    return sr;
        //}
        private static CornerResult SelectBestPairFromRuns_WithDebug(
    List<Seg> runs, Size imgSize,
    double targetDeg, double tolDeg, LineEdge dbg)
        {
            var best = new CornerResult { Found = false };
            if (runs.Count < 2) { dbg.Best = best; return best; }

            var imgRect = new Rect(0, 0, imgSize.Width, imgSize.Height);

            // các biến chấm điểm theo “ưu tiên inliers”
            int bestMinCnt = -1;                // ƯU TIÊN 1: lớn nhất
            int bestSumCnt = -1;                // ƯU TIÊN 2: tổng lớn nhất
            double bestMinLen = double.NegativeInfinity; // ƯU TIÊN 3: độ dài min lớn nhất
            double bestDev = double.PositiveInfinity; // ƯU TIÊN 4: lệch 90° nhỏ nhất
            double bestScore = double.NegativeInfinity; // chỉ để log/hiển thị

            for (int i = 0; i < runs.Count; i++)
            {
                for (int j = i + 1; j < runs.Count; j++)
                {
                    var A = runs[i]; var B = runs[j];

                    // góc giữa 2 hướng
                    double dot = A.Line.Vx * B.Line.Vx + A.Line.Vy * B.Line.Vy;
                    if (dot > 1.0) dot = 1.0; else if (dot < -1.0) dot = -1.0;
                    double ang = Math.Acos(dot) * 180.0 / Math.PI;
                    if (ang > 90.0) ang = 180.0 - ang;

                    double dev = Math.Abs(ang - targetDeg);
                    if (dev > tolDeg)
                    {
                        dbg?.PairRejected.Add((A, B, $"angle_dev>{tolDeg}", ang, new Point2f(), false));
                        continue;
                    }

                    // giao điểm phải nằm trong ảnh
                    Point2f P;
                    if (!Intersect(A.Line, B.Line, out P))
                    {
                        dbg?.PairRejected.Add((A, B, "parallel_or_no_intersect", ang, new Point2f(), false));
                        continue;
                    }
                    if (!imgRect.Contains((Point)P))
                    {
                        dbg?.PairRejected.Add((A, B, "intersection_outside_image", ang, P, true));
                        continue;
                    }

                    // các tiêu chí ưu tiên
                    int minCnt = Math.Min(A.Count, B.Count);
                    int sumCnt = A.Count+ B.Count;
                    double minLen = Math.Min(A.Len, B.Len);

                    // điểm tổng hợp chỉ để theo dõi/log — KHÔNG dùng làm ưu tiên
                    double score = minCnt * 1.0
                                 + 0.01 * sumCnt
                                 + 0.001 * minLen
                                 - 0.1 * (dev);   // nhỏ hơn tốt hơn

                   

                    // So sánh theo thứ tự ưu tiên:
                    bool better = false;
                    if (minCnt > bestMinCnt) better = true;
                    else if (minCnt == bestMinCnt)
                    {
                        if (sumCnt > bestSumCnt) better = true;
                        else if (sumCnt == bestSumCnt)
                        {
                            if (minLen > bestMinLen) better = true;
                            else if (Math.Abs(minLen - bestMinLen) <= 1e-9 && dev < bestDev)
                                better = true;
                        }
                    }

                    if (better)
                    {
                        bestMinCnt = minCnt;
                        bestSumCnt = sumCnt;
                        bestMinLen = minLen;
                        bestDev = dev;
                        bestScore = score;

                        best = new CornerResult
                        {
                            Found = true,
                            L1 = A.Line,
                            L2 = B.Line,
                            Corner = P,
                            AngleDeg = ang,
                            Inliers1 = A.Count,
                            Inliers2 = B.Count,
                            Score = score
                        };
                    }
                }
            }

            dbg.Best = best;
            return best;
        }
        // ===== DEBUG RENDER =====
        private static Mat RenderAllDebug(Mat gray, LineEdge dbg)
        {
            Mat canvas = gray.CvtColor(ColorConversionCodes.GRAY2BGR);

            foreach (var rr in dbg.RansacRejectedRaw)
                if (IsValid(rr.ln)) DrawInfiniteLine(canvas, rr.ln, new Scalar(200, 180, 80), 1);

            foreach (var ar in dbg.RansacAcceptedRaw)
                DrawInfiniteLine(canvas, ar.ln, new Scalar(180, 220, 255), 1);

            foreach (var cr in dbg.ContinuityRejected)
                DrawInfiniteLine(canvas, cr.ln, new Scalar(128, 128, 128), 2);

            foreach (var ca in dbg.ContinuityAccepted)
            {
                DrawInfiniteLine(canvas, ca.ln, new Scalar(170, 255, 170), 2);
                foreach (var p in ca.contiguous) canvas.Circle((Point)p, 1, new Scalar(0, 165, 255), -1);
            }

            foreach (var r in dbg.RunsKept)
                Cv2.Line(canvas, (Point)r.P0, (Point)r.P1, new Scalar(0, 140, 255), 2, LineTypes.AntiAlias);

            foreach (var pr in dbg.PairRejected)
            {
                Scalar col = pr.reason.StartsWith("angle") ? new Scalar(255, 255, 0)
                             : pr.reason.StartsWith("parallel") ? new Scalar(0, 255, 255)
                             : new Scalar(0, 0, 255);
                DrawInfiniteLine(canvas, pr.A.Line, col, 1);
                DrawInfiniteLine(canvas, pr.B.Line, col, 1);
                if (pr.hasP) Cv2.Circle(canvas, (Point)pr.P, 3, col, -1);
            }

            if (dbg.Best.Found)
            {
                DrawInfiniteLine(canvas, dbg.Best.L1, new Scalar(0, 255, 0), 2);
                DrawInfiniteLine(canvas, dbg.Best.L2, new Scalar(0, 0, 255), 2);
                Cv2.Circle(canvas, (Point)dbg.Best.Corner, 6, new Scalar(255, 0, 255), -1);
                Cv2.PutText(canvas, $"{dbg.Best.AngleDeg:0.1}°", (Point)(dbg.Best.Corner + new Point2f(8, -8)),
                            HersheyFonts.HersheySimplex, 0.6, new Scalar(255, 0, 255), 1);
            }
            return canvas;
        }

        private static bool IsValid(Line2D ln) => !(Math.Abs(ln.Vx) < 1e-12 && Math.Abs(ln.Vy) < 1e-12);

        // ===== Helpers =====
        private static bool Intersect(Line2D a, Line2D b, out Point2f P)
        {
            double ax = a.X1, ay = a.Y1, avx = a.Vx, avy = a.Vy;
            double bx = b.X1, by = b.Y1, bvx = b.Vx, bvy = b.Vy;

            double A11 = avx, A12 = -bvx;
            double A21 = avy, A22 = -bvy;
            double B1 = bx - ax, B2 = by - ay;

            double det = A11 * A22 - A12 * A21;
            if (Math.Abs(det) < 1e-9) { P = new Point2f(); return false; }

            double t = (B1 * A22 - A12 * B2) / det;
            P = new Point2f((float)(ax + t * avx), (float)(ay + t * avy));
            return true;
        }

        private static void DrawInfiniteLine(Mat img, Line2D ln, Scalar col, int thickness)
        {
            double t1 = -20000, t2 = 20000;
            var p1 = new Point2f((float)(ln.X1 + t1 * ln.Vx), (float)(ln.Y1 + t1 * ln.Vy));
            var p2 = new Point2f((float)(ln.X1 + t2 * ln.Vx), (float)(ln.Y1 + t2 * ln.Vy));
            Cv2.Line(img, (Point)p1, (Point)p2, col, thickness, LineTypes.AntiAlias);
        }

     
        

    
    }
}
