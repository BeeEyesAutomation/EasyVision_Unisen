using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using BeeCppCli; // PitchCliResult / PeakCli / PeakInfoCli

namespace BeeCore
{
    public sealed class PainterOptions
    {
        // Visibility
        public bool ShowCenterline = true;
        public bool ShowCrestPoints = true;
        public bool ShowRootPoints = true;
        public bool ShowRejected = true;
        public bool ShowPitchCrest = true;
        public bool ShowPitchRoot = true;
        public bool ShowPitchLabels = true;
        public bool ShowHeightsCrest = true;
        public bool ShowHeightsRoot = true;
        public bool ShowHeightLabels = true;
        public bool ShowTitle = true;
        [NonSerialized]
        public bool IsNew = false;
        // Sizes
        public int CrestRadius = 4;
        public int RootRadius = 3;
        public float LineWidthCenter = 2f;
        public float LineWidthPitch = 2f;
        public float LineWidthHeight = 2f;
        public int IndexCCD = 0;

        // Colors
        public Color CenterlineColor = Color.FromArgb(255, 0, 255);
        public Color CrestPointColor = Color.Red;
        public Color RootPointColor = Color.FromArgb(255, 255, 0);
        public Color RejectedColor = Color.DarkGray;

        public Color PitchCrestColor = Color.FromArgb(0, 165, 255);
        public Color PitchRootColor = Color.FromArgb(0, 200, 0);

        public Color HeightCrestColor = Color.FromArgb(200, 0, 200);
        public Color HeightRootColor = Color.FromArgb(0, 200, 200);

        public Color PitchTextColor = Color.Orange;
        public Color HeightTextColor = Color.Blue;
        public Color TitleTextColor = Color.Black;

        // Label layout
        public float LabelOffsetAlongNormal = 10f; // px, lệch khỏi line theo pháp tuyến để dễ đọc
        public StringFormat LabelStringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        // Title format
        public Func<PitchCliResult, string> TitleBuilder; // nếu null dùng title mặc định
    }

    public static class PitchGdiPainter
    {
        public static Graphics Draw(Graphics g, PitchCliResult R, Font font, PainterOptions opt = null)
        {
            if (g == null || R == null) return g;
            if (opt == null) opt = new PainterOptions();
            if (font == null) font = SystemFonts.DefaultFont;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var penCenter = new Pen(opt.CenterlineColor, opt.LineWidthCenter))
            using (var penPitchCrest = new Pen(opt.PitchCrestColor, opt.LineWidthPitch))
            using (var penPitchRoot = new Pen(opt.PitchRootColor, opt.LineWidthPitch))
            using (var penHeightCrest = new Pen(opt.HeightCrestColor, opt.LineWidthHeight))
            using (var penHeightRoot = new Pen(opt.HeightRootColor, opt.LineWidthHeight))
            using (var penReject = new Pen(opt.RejectedColor, 2f))
            using (var brushCrest = new SolidBrush(opt.CrestPointColor))
            using (var brushRoot = new SolidBrush(opt.RootPointColor))
            using (var brushPitchTxt = new SolidBrush(opt.PitchTextColor))
            using (var brushHeightTxt = new SolidBrush(opt.HeightTextColor))
            using (var brushTitle = new SolidBrush(opt.TitleTextColor))
            {
                // 1) Centerline polyline
                if (opt.ShowCenterline)
                {
                    List<Point> cl = BuildCenterlinePolyline(R);
                    if (cl.Count >= 2) g.DrawLines(penCenter, cl.ToArray());
                }

                // 2) Points
                if (opt.ShowCrestPoints) DrawPoints(g, R.CrestInfos, brushCrest, opt.CrestRadius);
                if (opt.ShowRootPoints) DrawPoints(g, R.RootInfos, brushRoot, opt.RootRadius);

                // 3) Rejected crest
                if (opt.ShowRejected && R.RejectedCrests != null)
                    foreach (var p in R.RejectedCrests) DrawX(g, p.X, p.Y, 4, penReject);

                // 4) Pitch segments (+ oriented labels)
                if (opt.ShowPitchCrest) DrawPitchSegmentsOriented(g, R.Crests, R.CrestInfos, penPitchCrest,
                                                                  opt.ShowPitchLabels ? brushPitchTxt : null,
                                                                  font, opt.LabelOffsetAlongNormal, opt.LabelStringFormat);
                if (opt.ShowPitchRoot) DrawPitchSegmentsOriented(g, R.Roots, R.RootInfos, penPitchRoot,
                                                                  opt.ShowPitchLabels ? brushPitchTxt : null,
                                                                  font, opt.LabelOffsetAlongNormal, opt.LabelStringFormat);

                // 5) Heights (+ oriented labels)
                if (opt.ShowHeightsCrest) DrawHeightsOriented(g, R.CrestInfos, penHeightCrest,
                                                              opt.ShowHeightLabels ? brushHeightTxt : null,
                                                              font, opt.LabelOffsetAlongNormal, opt.LabelStringFormat);
                if (opt.ShowHeightsRoot) DrawHeightsOriented(g, R.RootInfos, penHeightRoot,
                                                              opt.ShowHeightLabels ? brushHeightTxt : null,
                                                              font, opt.LabelOffsetAlongNormal, opt.LabelStringFormat);

                // 6) Title
                if (opt.ShowTitle)
                {
                    string title = opt.TitleBuilder != null
                        ? opt.TitleBuilder(R)
                        : BuildDefaultTitle(R);
                    g.DrawString(title, font, brushTitle, new PointF(10, 8));
                }
            }

            return g;
        }

        // ===== Helpers =====

        private static List<Point> BuildCenterlinePolyline(PitchCliResult R)
        {
            var pts = new List<Point>();
            if (R.CrestInfos != null)
                for (int i = 0; i < R.CrestInfos.Length; i++)
                    pts.Add(new Point(R.CrestInfos[i].CLX, R.CrestInfos[i].CLY));
            if (R.RootInfos != null)
                for (int i = 0; i < R.RootInfos.Length; i++)
                    pts.Add(new Point(R.RootInfos[i].CLX, R.RootInfos[i].CLY));

            pts.Sort((a, b) => a.X != b.X ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

            var outPts = new List<Point>(pts.Count);
            Point? last = null;
            foreach (var p in pts)
            {
                if (last.HasValue && last.Value.X == p.X && last.Value.Y == p.Y) continue;
                outPts.Add(p);
                last = p;
            }
            return outPts;
        }

        private static void DrawPoints(Graphics g, PeakInfoCli[] infos, Brush brush, int radius)
        {
            if (infos == null || brush == null) return;
            int d = radius * 2;
            for (int i = 0; i < infos.Length; i++)
            {
                var pi = infos[i];
                g.FillEllipse(brush, pi.X - radius, pi.Y - radius, d, d);
            }
        }

        private static void DrawX(Graphics g, int x, int y, int r, Pen pen)
        {
            g.DrawLine(pen, x - r, y - r, x + r, y + r);
            g.DrawLine(pen, x - r, y + r, x + r, y - r);
        }

        // ----- PITCH (đoạn giữa peak i -> i+1) + label xoay theo đoạn -----
        // Dùng PitchNextMM nếu có; nếu null/NaN -> hiển thị khoảng cách theo X (px) như cũ.
        private static void DrawPitchSegmentsOriented(
            Graphics g,
            PeakCli[] peaks,
            PeakInfoCli[] infos,
            Pen penLine,
            Brush brushLabel,
            Font font,
            float normalOffset,
            StringFormat sf)
        {
            if (peaks == null || peaks.Length < 2 || penLine == null) return;

            for (int i = 0; i < peaks.Length - 1; i++)
            {
                var a = peaks[i];
                var b = peaks[i + 1];
                g.DrawLine(penLine, a.X, a.Y, b.X, b.Y);

                if (brushLabel == null || font == null || infos == null || i >= infos.Length) continue;

                // --- giá trị: ưu tiên mm từ PeakInfoCli.PitchNextMM ---
                string text;
                double val = double.NaN;
                try { val = infos[i].PitchNextMM; } catch { val = double.NaN; }
                if (double.IsNaN(val))
                {
                    // fall-back px (khi chưa cập nhật CLI): dùng chênh lệch X
                    val = b.X - a.X;
                    text = $"{val:0.0}px";
                }
                else
                {
                    text = $"{val:0.###} ";
                }

                // --- label theo hướng đoạn ---
                DrawTextAlongSegment(g, font, brushLabel, text, a.X, a.Y, b.X, b.Y, normalOffset, sf);
            }
        }

        // ----- HEIGHT (peak -> centerline) + label xoay theo đoạn -----
        private static void DrawHeightsOriented(
            Graphics g,
            PeakInfoCli[] infos,
            Pen penLine,
            Brush brushLabel,
            Font font,
            float normalOffset,
            StringFormat sf)
        {
            if (infos == null || penLine == null) return;

            for (int i = 0; i < infos.Length; i++)
            {
                var pi = infos[i];
                g.DrawLine(penLine, pi.X, pi.Y, pi.CLX, pi.CLY);

                if (brushLabel == null || font == null) continue;

                // giá trị: ưu tiên HeightMM; fall-back Height (px) nếu project cũ
                double h = double.NaN;
                try { h = pi.HeightMM; } catch { h = double.NaN; }
                string text;
                if (double.IsNaN(h))
                {
                    try { h = pi.HeightMM; } catch { h = double.NaN; }
                    text = double.IsNaN(h) ? "" : $"{h:0.0}px";
                }
                else
                {
                    text = $"{h:0.###} ";
                }
                if (string.IsNullOrEmpty(text)) continue;

                DrawTextAlongSegment(g, font, brushLabel, text, pi.X, pi.Y, pi.CLX, pi.CLY, normalOffset, sf);
            }
        }

        // ----- Core: vẽ text theo hướng line, lệch ra 1 khoảng theo pháp tuyến -----
        private static void DrawTextAlongSegment(Graphics g, Font font, Brush brush, string text,
                                                 int x1, int y1, int x2, int y2,
                                                 float normalOffset, StringFormat sf)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            if (dx == 0 && dy == 0) return;

            // góc theo rad/deg
            double angRad = Math.Atan2(dy, dx);
            float angDeg = (float)(angRad * 180.0 / Math.PI);

            // midpoint
            float mx = (x1 + x2) * 0.5f;
            float my = (y1 + y2) * 0.5f;

            // pháp tuyến (dx,dy) -> (-dy, dx)
            float nx = -dy;
            float ny = dx;
            float nlen = (float)Math.Sqrt(nx * nx + ny * ny);
            if (nlen > 1e-3f) { nx /= nlen; ny /= nlen; } else { nx = 0; ny = -1; }

            // điểm vẽ label (lệch khỏi line)
            float lx = mx + nx * normalOffset;
            float ly = my + ny * normalOffset;

            // xoay hệ trục rồi vẽ text tại (0,0)
            var state = g.Save();
            g.TranslateTransform(lx, ly);
            g.RotateTransform(angDeg);
            g.DrawString(text, font, brush, 0f, 0f, sf);
            g.Restore(state);
        }

        private static string BuildDefaultTitle(PitchCliResult R)
        {
            // ưu tiên số theo mm nếu có, else rỗng
            string crest = "", root = "", height = "";
            try { crest = $"P(mm med/mean)={R.PitchMedianMM:0.###}/{R.PitchMeanMM:0.###} sd={R.PitchStdMM:0.###}"; } catch { }
            try { root = $"rP(mm med/mean)={R.PitchRootMedianMM:0.###}/{R.PitchRootMeanMM:0.###} sd={R.PitchRootStdMM:0.###}"; } catch { }
            try { height = $"hC(mm med/mean)={R.CrestHMedianMM:0.###}/{R.CrestHMeanMM:0.###}  hR(mm med/mean)={R.RootHMedianMM:0.###}/{R.RootHMeanMM:0.###}"; } catch { }

            int nC = R.Crests?.Length ?? 0;
            int nR = R.Roots?.Length ?? 0;

            return $"Crest N={nC}  {crest} | Root N={nR}  {root} | {height}";
        }
    }
}
