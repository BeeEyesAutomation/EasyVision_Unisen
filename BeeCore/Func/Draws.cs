using BeeCpp;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Point = OpenCvSharp.Point;
using ShapeType = BeeGlobal.ShapeType;
using Size = System.Drawing.Size;

namespace BeeCore
{
    public class Draws
    {
        public static void DrawLineDebug(
       Graphics g,
       Line2DCli r,
       float thresholdPx = 1.5f,
       IEnumerable<PointF> samples = null,   // tập điểm (edge non-zero) để tô inlier/outlier; có thể null
       bool drawInliers = true,
       bool drawOutliers = false,
       Matrix worldToScreen = null,          // null = vẽ theo toạ độ ảnh 1:1
       int lineThickness = 2,
       int bandThickness = 1,
       int pointRadius = 1,
       string infoTextTop = null            // override text; null = auto
   )
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            if (!r.Found)
            {
                 var font = new Font("Segoe UI", 10f, FontStyle.Regular);
                 var br = new SolidBrush(Color.White);
                g.DrawString("No line found", font, br, 10, 10);
                return;
            }

            // Endpoints
            var P1 = new PointF(r.X1, r.Y1);
            var P2 = new PointF(r.X2, r.Y2);

            // Hướng đơn vị v (từ fitLine)
            var v = new PointF(r.Vx, r.Vy);
            // Pháp tuyến đơn vị n = (-vy, vx)
            var n = new PointF(-v.Y, v.X);

            // 1) Band (±threshold): dựng 4 điểm polygon: P1+off, P2+off, P2-off, P1-off
            var off = new PointF(n.X * thresholdPx, n.Y * thresholdPx);
            var p1p = Add(P1, off);
            var p2p = Add(P2, off);
            var p1m = Sub(P1, off);
            var p2m = Sub(P2, off);

            var poly = new[] { p1p, p2p, p2m, p1m };
            Transform(worldToScreen, poly);

            // tô băng mờ
            using (var bandBrush = new SolidBrush(Color.FromArgb(60, 50, 150, 255))) // xanh dương mờ
                g.FillPolygon(bandBrush, poly);

            // viền băng
            using (var bandPen = new Pen(Color.FromArgb(160, 50, 150, 255), bandThickness))
            {
                g.DrawLine(bandPen, poly[0], poly[1]);
                g.DrawLine(bandPen, poly[2], poly[3]);
            }

            // 2) Vẽ line chính + endpoint
            var sP1 = P1; var sP2 = P2;
            Transform(worldToScreen, ref sP1);
            Transform(worldToScreen, ref sP2);

            using (var linePen = new Pen(Color.Gold, lineThickness))
                g.DrawLine(linePen, sP1, sP2);

            using (var endBrush = new SolidBrush(Color.LimeGreen))
            {
                FillCircle(g, endBrush, sP1, Math.Max(2, lineThickness + 1));
                FillCircle(g, endBrush, sP2, Math.Max(2, lineThickness + 1));
            }

            // 3) Inlier/Outlier (nếu có sample)
            if ((drawInliers || drawOutliers) && samples != null)
            {
                // ax + by + c = 0 từ (v, p0) với n = (vy, -vx)
                double a = v.Y;
                double b = -v.X;
                double c = -(a * r.X0 + b * r.Y0);
                double invDen = 1.0 / Math.Sqrt(a * a + b * b);

                 var brIn = new SolidBrush(Color.FromArgb(220, 0, 180, 0));
                 var brOut = new SolidBrush(Color.FromArgb(220, 220, 0, 0));

                int step = 1; // nếu nhiều điểm quá, tăng step
                int i = 0;
                foreach (var pt in samples)
                {
                    if ((i++ % step) != 0) continue;

                    double d = Math.Abs(a * pt.X + b * pt.Y + c) * invDen;
                    bool isIn = d < thresholdPx;

                    if ((isIn && drawInliers) || (!isIn && drawOutliers))
                    {
                        var sp = pt;
                        Transform(worldToScreen, ref sp);
                        FillCircle(g, isIn ? brIn : brOut, sp, pointRadius);
                    }
                }
            }

            // 4) Text info
             var fontInfo = new Font("Segoe UI", 9f, FontStyle.Regular);
             var brText = new SolidBrush(Color.White);
             var outline = new Pen(Color.FromArgb(120, 0, 0, 0), 3f)
            { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };

            string ln = infoTextTop ??
                        $"len={r.LengthPx:F2}px / {r.LengthMm:F3}mm | score={r.Score:F2} | inliers={r.Inliers}";
            string th = $"threshold={thresholdPx:F2}px";

            DrawTextWithOutline(g, ln, fontInfo, new PointF(10, 10), brText, outline);
            DrawTextWithOutline(g, th, fontInfo, new PointF(10, 28), brText, outline);
        }

        // ===== Utils =====
        private static PointF Add(PointF a, PointF b) => new PointF(a.X + b.X, a.Y + b.Y);
        private static PointF Sub(PointF a, PointF b) => new PointF(a.X - b.X, a.Y - b.Y);

        private static void Transform(Matrix m, ref PointF p)
        {
            if (m == null) return;
            var arr = new[] { p };
            m.TransformPoints(arr);
            p = arr[0];
        }
        private static void Transform(Matrix m, PointF[] pts)
        {
            if (m == null) return;
            m.TransformPoints(pts);
        }

        private static void FillCircle(Graphics g, Brush br, PointF center, int radius)
        {
            float d = radius * 2f;
            g.FillEllipse(br, center.X - radius, center.Y - radius, d, d);
        }

        private static void DrawTextWithOutline(Graphics g, string text, Font font, PointF p, Brush fill, Pen outline)
        {
             var gp = new GraphicsPath();
            gp.AddString(text, font.FontFamily, (int)font.Style, g.DpiY * font.SizeInPoints / 72f, p, StringFormat.GenericDefault);
            g.DrawPath(outline, gp);
            g.FillPath(fill, gp);
        }

        public static void DrawTicks(Graphics gc,System.Drawing. PointF p, LineOrientation ori,Pen pen)
        {
            int tickLen = 10;
            if (ori == LineOrientation.Any || ori == LineOrientation.Vertical)
            {
                // Vertical ticks
                gc.DrawLine( pen, new System.Drawing.PointF(p.X, p.Y - tickLen), new System.Drawing.PointF(p.X, p.Y + tickLen));
            }
            else
            {
                // Horizontal ticks
                gc.DrawLine(pen, new System.Drawing.PointF(p.X - tickLen, p.Y), new System.Drawing.PointF(p.X + tickLen, p.Y));
            }
        }
        public static void DrawTicks(Mat img, Point p, LineOrientation ori)
        {
            int tickLen = 10;
            if (ori == LineOrientation.Any || ori == LineOrientation.Vertical)
            {
                // Vertical ticks
                Cv2.Line(img, new Point(p.X, p.Y - tickLen), new Point(p.X, p.Y + tickLen), new Scalar(255, 128, 0), 2);
            }
            else
            {
                // Horizontal ticks
                Cv2.Line(img, new Point(p.X - tickLen, p.Y), new Point(p.X + tickLen, p.Y), new Scalar(255, 128, 0), 2);
            }
        }
        public static void DrawPerpendicularLine(Mat img, Line2D Line, Scalar color, int thickness)
        {

            DrawInfiniteLine(img, Line, color, thickness);
        }
        public static void Plus(Graphics gc, int centerX, int centerY, int lineLength, Color color, int thiness)
        {
            Pen pen = new Pen(color, thiness);
            gc.DrawLine(pen, centerX - lineLength / 2, centerY, centerX + lineLength / 2, centerY);
            gc.DrawLine(pen, centerX, centerY - lineLength / 2, centerX, centerY + lineLength / 2);
        }
        private static void DrawLineClipped(Graphics g, Line2D ln, RectangleF clip, Pen pen)
        {
          
        }
        private static bool TryDrawLineWithRect(Line2D ln, RectangleF rc, out PointF A, out PointF B)
        {
            const float EPS = 1e-6f;
            A = default(PointF);
            B = default(PointF);

            if (ln == null) return false;
            float vx = (float)ln.Vx, vy = (float)ln.Vy, x0 = (float)ln.X1, y0 = (float)ln.Y1;
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

        public static void DrawInfiniteLine(Graphics g, Line2D ln, Pen pen, RectangleF rectClient)
        {
            PointF a, b;
            if (!TryDrawLineWithRect(ln, rectClient, out a, out b))
                return; // line không cắt vùng vẽ
            g.DrawLine(pen, a, b);
            g.DrawLine(pen, a, b);
            //float vx =(float) ln.Vx;
            //float vy = (float)ln.Vy;
            //float x0 = (float)ln.X1;
            //float y0 = (float)ln.Y1;
            //System.Drawing.PointF pt1 = new System.Drawing.PointF(x0 + vx * 1000, y0 + vy * 1000);
            //System.Drawing.PointF pt2 = new System.Drawing.PointF(x0 - vx * 1000, y0 - vy * 1000);
            //g.DrawLine(pen, pt1, pt2);
        }
        public static void DrawInfiniteLine(Mat img, Line2D ln, Scalar col, int thickness)
        {
            double vx = ln.Vx;
            double vy = ln.Vy;
            double x0 = ln.X1;
            double y0 = ln.Y1;
            Point pt1 = new Point(x0 + vx * 1000, y0 + vy * 1000);
            Point pt2 = new Point(x0 - vx * 1000, y0 - vy * 1000);
            Cv2.Line(img, pt1, pt2, col, thickness);
        }
        public static void Box1Label(Graphics graphics, RectangleF baseRect, string text, Font font, Brush textBrush, Color backgroundBrush,int thiness=4, bool alignRight = false)
        {
           
            graphics.DrawRectangle(new Pen(backgroundBrush, thiness), new Rectangle((int)baseRect.X, (int)baseRect.Y, (int)baseRect.Width, (int)baseRect.Height));

            // Đo kích thước vùng text
            SizeF textSize = graphics.MeasureString(text, font);

            // Tính vị trí của rectangle chứa text nằm phía trên baseRect
            int padding =1; // padding giữa text và viền rectangle

            // Tính width/height vùng nền có padding
            int labelWidth = (int)textSize.Width + padding * 2;
            int labelHeight = (int)textSize.Height + padding * 2;

            // Tính toạ độ top-left của labelRect
            int labelX = alignRight
                ? (int)baseRect.Right - labelWidth   // Bám góc phải
                : (int)baseRect.Left;                // Bám góc trái

            int labelY = (int)baseRect.Top - labelHeight; // Nằm phía trên rectangle

            // Nếu vượt ra trên ảnh, bạn có thể kiểm tra và điều chỉnh labelY >= 0 nếu muốn

            Rectangle labelRect = new Rectangle(labelX, labelY, labelWidth, labelHeight);

            // Vẽ nền rectangle
            graphics.FillRectangle(new SolidBrush( backgroundBrush), labelRect);

            // Vẽ text (trong rectangle, có padding)
            graphics.DrawString(text, font, textBrush, labelX + padding, labelY + padding);
        }
        public static void Box1Label(Graphics graphics, RectRotate rot, string text, Font font, Brush textBrush, Color backgroundBrush, int thiness = 4, bool alignRight = false)
        {
            DrawRectRotate(graphics, rot, new Pen(backgroundBrush, thiness));
            
            // Đo kích thước vùng text
            SizeF textSize = graphics.MeasureString(text, font);

            // Tính vị trí của rectangle chứa text nằm phía trên baseRect
            int padding = 1; // padding giữa text và viền rectangle

            // Tính width/height vùng nền có padding
            int labelWidth = (int)textSize.Width + padding * 2;
            int labelHeight = (int)textSize.Height + padding * 2;

            // Tính toạ độ top-left của labelRect
            int labelX = alignRight
                ? (int)rot._rect.Right - labelWidth   // Bám góc phải
                : (int)rot._rect.Left;                // Bám góc trái

            int labelY = (int)rot._rect.Top - labelHeight; // Nằm phía trên rectangle

            // Nếu vượt ra trên ảnh, bạn có thể kiểm tra và điều chỉnh labelY >= 0 nếu muốn

            Rectangle labelRect = new Rectangle(labelX, labelY, labelWidth, labelHeight);

            // Vẽ nền rectangle
            graphics.FillRectangle(new SolidBrush(backgroundBrush), labelRect);

            // Vẽ text (trong rectangle, có padding)
            graphics.DrawString(text, font, textBrush, labelX + padding, labelY + padding);
        }

        public static void Box2Label(Graphics graphics, RectangleF baseRect, string leftText, string rightText, Font baseFont, Color baseBackColor, Brush textBrush, int opacity = 128,int thiness=4, int minFontSize = 10, int padding = 1,bool ShowArea=false)
        {
            graphics.DrawRectangle(new Pen(baseBackColor, thiness), new Rectangle((int)baseRect.X, (int)baseRect.Y, (int)baseRect.Width, (int)baseRect.Height));

            float fontSize = baseFont.Size;
       
            Font currentFont;
            SizeF leftSize, rightSize;
            int totalTextWidth;

            // Tìm font size phù hợp
            do
            {
                currentFont = new Font(baseFont.FontFamily, fontSize, baseFont.Style);
                leftSize = graphics.MeasureString(leftText, currentFont);
                rightSize = graphics.MeasureString(rightText, currentFont);
                totalTextWidth = (int)(leftSize.Width + rightSize.Width + 3 * padding);
                fontSize--;
            }
            while (totalTextWidth > baseRect.Width && fontSize >= minFontSize);

            // Tính kích thước label
            int labelHeight = (int)Math.Max(leftSize.Height, rightSize.Height) + 2 * padding;
            int labelY = (int)baseRect.Top - labelHeight;

            // LEFT LABEL
            int leftWidth = (int)leftSize.Width + 2 * padding;
            Rectangle leftRect = new Rectangle((int)baseRect.Left, labelY, leftWidth, labelHeight);
            if(ShowArea)
            {
                double Area =Math.Round( baseRect.Width * baseRect.Height/100);
                graphics.DrawString(Area+"px", currentFont,new SolidBrush( Color.Gray), baseRect.X, (int)baseRect.Y + (int)baseRect.Height - labelHeight - 5);
            }    
           
            using (SolidBrush leftBgBrush = new SolidBrush(baseBackColor))
            {
                graphics.FillRectangle(leftBgBrush, leftRect);
                graphics.DrawString(leftText, currentFont, textBrush, leftRect.Left + padding, leftRect.Top + padding);
            }

            // RIGHT LABEL background: kéo từ sau left đến hết box, nhưng text phải căn phải
            int rightStartX = leftRect.Right;
            int rightEndX = (int)baseRect.Right;
            int rightWidth = rightEndX - rightStartX;

            Rectangle rightRect = new Rectangle(rightStartX, labelY, rightWidth, labelHeight);

            Color transparentColor = Color.FromArgb(opacity, baseBackColor.R, baseBackColor.G, baseBackColor.B);
            using (SolidBrush rightBgBrush = new SolidBrush(transparentColor))
            {
                graphics.FillRectangle(rightBgBrush, rightRect);

                // Vị trí chữ: căn phải bên trong rightRect
                float textX = rightRect.Right - rightSize.Width - padding;
                float textY = rightRect.Top + padding;

                graphics.DrawString(rightText, currentFont, textBrush, textX, textY);
            }
        }
        public static void Box3Label(Graphics graphics, RectangleF baseRect, string leftText, string rightText, string Area , Font baseFont, Color baseBackColor, Brush textBrush, int opacity = 128, int thiness = 4, int minFontSize = 10, int padding = 1, bool ShowArea = false)
        {
            graphics.DrawRectangle(new Pen(baseBackColor, thiness), new Rectangle((int)baseRect.X, (int)baseRect.Y, (int)baseRect.Width, (int)baseRect.Height));

            float fontSize = baseFont.Size;

            Font currentFont;
            SizeF leftSize, rightSize;
            int totalTextWidth;

            // Tìm font size phù hợp
            do
            {
                currentFont = new Font(baseFont.FontFamily, fontSize, baseFont.Style);
                leftSize = graphics.MeasureString(leftText, currentFont);
                rightSize = graphics.MeasureString(rightText, currentFont);
                totalTextWidth = (int)(leftSize.Width + rightSize.Width + 3 * padding);
                fontSize--;
            }
            while (totalTextWidth > baseRect.Width && fontSize >= minFontSize);

            // Tính kích thước label
            int labelHeight = (int)Math.Max(leftSize.Height, rightSize.Height) + 2 * padding;
            int labelY = (int)baseRect.Top - labelHeight;

            // LEFT LABEL
            int leftWidth = (int)leftSize.Width + 2 * padding;
            Rectangle leftRect = new Rectangle((int)baseRect.Left, labelY, leftWidth, labelHeight);
            if (ShowArea)
            {
               // double Area2 = Math.Round(baseRect.Width * baseRect.Height / 100);

                graphics.DrawString(Area, currentFont, new SolidBrush(Color.Gray), baseRect.X, (int)baseRect.Y + (int)baseRect.Height - labelHeight - 5);
            }

            using (SolidBrush leftBgBrush = new SolidBrush(baseBackColor))
            {
                graphics.FillRectangle(leftBgBrush, leftRect);
                graphics.DrawString(leftText, currentFont, textBrush, leftRect.Left + padding, leftRect.Top + padding);
            }

            // RIGHT LABEL background: kéo từ sau left đến hết box, nhưng text phải căn phải
            int rightStartX = leftRect.Right;
            int rightEndX = (int)baseRect.Right;
            int rightWidth = rightEndX - rightStartX;

            Rectangle rightRect = new Rectangle(rightStartX, labelY, rightWidth, labelHeight);

            Color transparentColor = Color.FromArgb(opacity, baseBackColor.R, baseBackColor.G, baseBackColor.B);
            using (SolidBrush rightBgBrush = new SolidBrush(transparentColor))
            {
                graphics.FillRectangle(rightBgBrush, rightRect);

                // Vị trí chữ: căn phải bên trong rightRect
                float textX = rightRect.Right - rightSize.Width - padding;
                float textY = rightRect.Top + padding;

                graphics.DrawString(rightText, currentFont, textBrush, textX, textY);
            }
        }

        public static void Box2Label(Graphics graphics, RectRotate baseRect, string leftText, string rightText, Font baseFont, Color baseBackColor, Brush textBrush, int opacity = 128, int thiness = 4, int minFontSize = 10, int padding = 1, bool ShowArea = false)
        {
            DrawRectRotate(graphics, baseRect, new Pen(baseBackColor, thiness));
            float fontSize = baseFont.Size;

            Font currentFont;
            SizeF leftSize, rightSize;
            int totalTextWidth;

            // Tìm font size phù hợp
            do
            {
                currentFont = new Font(baseFont.FontFamily, fontSize, baseFont.Style);
                leftSize = graphics.MeasureString(leftText, currentFont);
                rightSize = graphics.MeasureString(rightText, currentFont);
                totalTextWidth = (int)(leftSize.Width + rightSize.Width + 3 * padding);
                fontSize--;
            }
            while (totalTextWidth > baseRect._rect.Width && fontSize >= minFontSize);

            // Tính kích thước label
            int labelHeight = (int)Math.Max(leftSize.Height, rightSize.Height) + 2 * padding;
            int labelY = (int)baseRect._rect.Top - labelHeight;

            // LEFT LABEL
            int leftWidth = (int)leftSize.Width + 2 * padding;
            Rectangle leftRect = new Rectangle((int)baseRect._rect.Left, labelY, leftWidth, labelHeight);
            if (ShowArea)
            {
                double Area = Math.Round(baseRect._rect.Width * baseRect._rect.Height / 100);
                graphics.DrawString(Area + "px", currentFont, new SolidBrush(Color.Gray), baseRect._rect.X, (int)baseRect._rect.Y + (int)baseRect._rect.Height - labelHeight - 5);
            }

            using (SolidBrush leftBgBrush = new SolidBrush(baseBackColor))
            {
                graphics.FillRectangle(leftBgBrush, leftRect);
                graphics.DrawString(leftText, currentFont, textBrush, leftRect.Left + padding, leftRect.Top + padding);
            }

            // RIGHT LABEL background: kéo từ sau left đến hết box, nhưng text phải căn phải
            int rightStartX = leftRect.Right;
            int rightEndX = (int)baseRect._rect.Right;
            int rightWidth = rightEndX - rightStartX;

            Rectangle rightRect = new Rectangle(rightStartX, labelY, rightWidth, labelHeight);

            Color transparentColor = Color.FromArgb(opacity, baseBackColor.R, baseBackColor.G, baseBackColor.B);
            using (SolidBrush rightBgBrush = new SolidBrush(transparentColor))
            {
                graphics.FillRectangle(rightBgBrush, rightRect);

                // Vị trí chữ: căn phải bên trong rightRect
                float textX = rightRect.Right - rightSize.Width - padding;
                float textY = rightRect.Top + padding;

                graphics.DrawString(rightText, currentFont, textBrush, textX, textY);
            }
        }

    public    static void DrawRectRotate(
    Graphics g, RectRotate rr,
    float zoomPercent, System.Drawing.Point scroll,
    Pen outlinePen,
    bool drawHandles = false,
    bool hasMouse = false,System.Drawing. Point mouseScreen = default)
    {
        if (rr == null) return;

        var oldSmoothing = g.SmoothingMode;
        var oldTrans = g.Transform.Clone();
        g.SmoothingMode = SmoothingMode.AntiAlias;
            float rH = Global.Config.RadEdit;
            if (!Global.IsRun&& drawHandles)
                if (rr._dragAnchor == AnchorPoint.Center)
            {
                
                outlinePen = new Pen(Brushes.Blue, outlinePen.Width*2);
            }
            using (var m = new Matrix())
        {
                if(!Global.IsRun)
                {
                    m.Translate(scroll.X, scroll.Y);
                    float s = zoomPercent / 100f;
                    m.Scale(s, s);
                    m.Translate(rr._PosCenter.X, rr._PosCenter.Y);
                    m.Rotate(rr._rectRotation);
                    g.Transform = m;
                }    
           

            using (var path = new GraphicsPath())
            {
                switch (rr.Shape)
                {
                    case ShapeType.Rectangle: path.AddRectangle(rr._rect); break;
                    case ShapeType.Ellipse: path.AddEllipse(rr._rect); break;
                    case ShapeType.Hexagon: path.AddPolygon(rr.GetHexagonVerticesLocal()); break;
                    case ShapeType.Polygon:
                        var poly = rr.GetPolygonVerticesLocal();
                        if (poly != null && poly.Length >= 2)
                        {
                            if (rr.IsPolygonClosed) path.AddPolygon(poly);
                            else path.AddLines(poly);
                        }
                        break;
                }
                if (outlinePen != null) g.DrawPath(outlinePen, path);
            }

            PointF pLocal = PointF.Empty;
            if (hasMouse)
            {
                using (var inv = new Matrix())
                {
                    inv.Translate(scroll.X, scroll.Y);
                    float sInv = zoomPercent / 100f;
                    inv.Scale(sInv, sInv);
                    inv.Translate(rr._PosCenter.X, rr._PosCenter.Y);
                    inv.Rotate(rr._rectRotation);
                    inv.Invert();
                    var arr = new[] { new PointF(mouseScreen.X, mouseScreen.Y) };
                    inv.TransformPoints(arr);
                    pLocal = arr[0];
                }
            }

            if (rr.Shape == ShapeType.Polygon && !rr.IsPolygonClosed &&
                rr.PolyLocalPoints != null && rr.PolyLocalPoints.Count > 0 && hasMouse)
            {
                var last = rr.PolyLocalPoints[rr.PolyLocalPoints.Count - 1];
                using (var dashPen = new Pen(Color.Orange, 1f) { DashStyle = DashStyle.Dash })
                    g.DrawLine(dashPen, last, pLocal);

                float rM = Math.Max(2f, Global.Config.RadEdit * 0.35f);
                var hoverDot = new RectangleF(pLocal.X - rM, pLocal.Y - rM, 2f * rM, 2f * rM);
                g.FillEllipse(Brushes.Orange, hoverDot);
                g.DrawEllipse(Pens.Black, hoverDot);
            }

            if (drawHandles)
            {
                AnchorPoint hoverAnchor = AnchorPoint.None;
                int hoverVertexIndex = -1;

             
                RectangleF rect = rr._rect;
                float centerXLocal = rect.X + rect.Width / 2f; // <-- luôn căn giữa bbox

                if (hasMouse)
                {
                    if (rr.Shape == ShapeType.Hexagon)
                    {
                        var verts = rr.GetHexagonVerticesLocal();
                        for (int i = 0; i < 6; i++)
                        {
                            var h = new RectangleF(verts[i].X - rH / 2f, verts[i].Y - rH / 2f, rH, rH);
                            if (h.Contains(pLocal)) { hoverAnchor = (AnchorPoint)((int)AnchorPoint.V0 + i); break; }
                        }
                    }
                    else if (rr.Shape == ShapeType.Polygon)
                    {
                        var verts = rr.GetPolygonVerticesLocal();
                        for (int i = 0; i < verts.Length; i++)
                        {
                            var h = new RectangleF(verts[i].X - rH / 2f, verts[i].Y - rH / 2f, rH, rH);
                            if (h.Contains(pLocal)) { hoverAnchor = AnchorPoint.Vertex; hoverVertexIndex = i; break; }
                        }
                    }
                    else
                    {
                        var tl = new RectangleF(rect.Left - rH / 2f, rect.Top - rH / 2f, rH, rH);
                        var tr = new RectangleF(rect.Right - rH / 2f, rect.Top - rH / 2f, rH, rH);
                        var bl = new RectangleF(rect.Left - rH / 2f, rect.Bottom - rH / 2f, rH, rH);
                        var br = new RectangleF(rect.Right - rH / 2f, rect.Bottom - rH / 2f, rH, rH);
                        var rot = new RectangleF(centerXLocal - rH / 2f, rect.Top - rH,rH, rH);

                        if (tl.Contains(pLocal)) hoverAnchor = AnchorPoint.TopLeft;
                        else if (tr.Contains(pLocal)) hoverAnchor = AnchorPoint.TopRight;
                        else if (bl.Contains(pLocal)) hoverAnchor = AnchorPoint.BottomLeft;
                        else if (br.Contains(pLocal)) hoverAnchor = AnchorPoint.BottomRight;
                        else if (rot.Contains(pLocal)) hoverAnchor = AnchorPoint.Rotation;
                    }
                }
                    
                      
                    using (var brSel = new SolidBrush(Color.Blue))
                using (var brHover = new SolidBrush(Color.Orange))
                using (var brNone = new SolidBrush(Color.OrangeRed))
                    {
                       
                        Brush Pick(bool sel, bool hov) => sel ? (Brush)brSel : (hov ? (Brush)brHover : (Brush)brNone);

                    if (rr.Shape == ShapeType.Hexagon)
                        {
                           
                        var verts = rr.GetHexagonVerticesLocal();
                        for (int i = 0; i < 6; i++)
                        {
                            var rc = new RectangleF(verts[i].X - rH / 2f, verts[i].Y - rH / 2f, rH, rH);
                            var ap = (AnchorPoint)((int)AnchorPoint.V0 + i);
                            g.FillRectangle(Pick(rr._dragAnchor == ap, hoverAnchor == ap), rc);
                            g.DrawRectangle(Pens.Black, rc.X, rc.Y, rc.Width, rc.Height);
                        }

                        var tl = new RectangleF(rect.Left - rH / 2f, rect.Top - rH / 2f, rH, rH);
                        var tr = new RectangleF(rect.Right - rH / 2f, rect.Top - rH / 2f, rH, rH);
                        var bl = new RectangleF(rect.Left - rH / 2f, rect.Bottom - rH / 2f, rH, rH);
                        var br = new RectangleF(rect.Right - rH / 2f, rect.Bottom - rH / 2f, rH, rH);
                        var rot = new RectangleF(centerXLocal - rH / 2f, rect.Top -  rH,  rH,  rH);
                         
                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.TopLeft, hoverAnchor == AnchorPoint.TopLeft), tl);
                                g.DrawRectangle(Pens.Black, tl.X, tl.Y, tl.Width, tl.Height);

                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.TopRight, hoverAnchor == AnchorPoint.TopRight), tr);
                                g.DrawRectangle(Pens.Black, tr.X, tr.Y, tr.Width, tr.Height);

                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.BottomLeft, hoverAnchor == AnchorPoint.BottomLeft), bl);
                                g.DrawRectangle(Pens.Black, bl.X, bl.Y, bl.Width, bl.Height);

                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.BottomRight, hoverAnchor == AnchorPoint.BottomRight), br);
                                g.DrawRectangle(Pens.Black, br.X, br.Y, br.Width, br.Height);

                                g.FillEllipse(Pick(rr._dragAnchor == AnchorPoint.Rotation, hoverAnchor == AnchorPoint.Rotation), rot);
                                g.DrawEllipse(Pens.Black, rot);
                            
                    }
                    else if (rr.Shape == ShapeType.Polygon)
                        {
                          
                                var verts = rr.GetPolygonVerticesLocal();
                                for (int i = 0; i < verts.Length; i++)
                                {
                                    var h = new RectangleF(verts[i].X - rH / 2f, verts[i].Y - rH / 2f, rH, rH);
                                    bool sel = (rr._dragAnchor == AnchorPoint.Vertex && rr.ActiveVertexIndex == i);
                                    bool hov = (hoverAnchor == AnchorPoint.Vertex && hoverVertexIndex == i);
                                    g.FillRectangle(Pick(sel, hov), h);
                                    g.DrawRectangle(Pens.Black, h.X, h.Y, h.Width, h.Height);
                                }
                            
                    }
                    else
                        {
                          
                                      var tl = new RectangleF(rect.Left - rH / 2f, rect.Top - rH / 2f, rH, rH);
                                var tr = new RectangleF(rect.Right - rH / 2f, rect.Top - rH / 2f, rH, rH);
                                var bl = new RectangleF(rect.Left - rH / 2f, rect.Bottom - rH / 2f, rH, rH);
                                var br = new RectangleF(rect.Right - rH / 2f, rect.Bottom - rH / 2f, rH, rH);
                                var rot = new RectangleF(centerXLocal - rH / 2f, rect.Top - rH, rH, rH);

                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.TopLeft, hoverAnchor == AnchorPoint.TopLeft), tl);
                                g.DrawRectangle(Pens.Black, tl.X, tl.Y, tl.Width, tl.Height);

                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.TopRight, hoverAnchor == AnchorPoint.TopRight), tr);
                                g.DrawRectangle(Pens.Black, tr.X, tr.Y, tr.Width, tr.Height);

                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.BottomLeft, hoverAnchor == AnchorPoint.BottomLeft), bl);
                                g.DrawRectangle(Pens.Black, bl.X, bl.Y, bl.Width, bl.Height);

                                g.FillRectangle(Pick(rr._dragAnchor == AnchorPoint.BottomRight, hoverAnchor == AnchorPoint.BottomRight), br);
                                g.DrawRectangle(Pens.Black, br.X, br.Y, br.Width, br.Height);

                                g.FillEllipse(Pick(rr._dragAnchor == AnchorPoint.Rotation, hoverAnchor == AnchorPoint.Rotation), rot);
                                g.DrawEllipse(Pens.Black, rot);
                            
                    }
                }
            }
        }

        g.Transform = oldTrans;
        g.SmoothingMode = oldSmoothing;
        oldTrans.Dispose();
    }
        static void DrawRectRotate(Graphics g, RectRotate rr,
                          
                           Color edge, float penWidth = 2f,
                           bool fill = false, float fillAlpha = 64)
        {
           
             
                //mat.Translate(rr._PosCenter.X, rr._PosCenter.Y);
                //mat.Rotate(rr._rectRotation);

                //var old = g.Transform;
                //g.Transform = mat;

                // Vẽ polygon local
                var pts = rr.PolyLocalPoints;
                if (pts != null && pts.Count >= 2)
                {
                    using (var pen = new Pen(edge, penWidth )) // giữ độ dày tương đối khi zoom
                    {
                        if (fill)
                        {
                            using (var br = new SolidBrush(Color.FromArgb((int)fillAlpha, edge)))
                            {
                                g.FillPolygon(br, pts.ToArray());
                            }
                        }
                        g.DrawPolygon(pen, pts.ToArray());
                    }
                }

                // Vẽ bounding rect local nếu muốn
                // g.DrawRectangle(Pens.Yellow, rr._rect.X, rr._rect.Y, rr._rect.Width, rr._rect.Height);

               // g.Transform = old;
            
        }

        public static void DrawRectRotate(
Graphics g, RectRotate rr,
Pen outlinePen)
        {
            if (rr == null) return;

            var oldSmoothing = g.SmoothingMode;
            var oldTrans = g.Transform.Clone();
            g.SmoothingMode = SmoothingMode.AntiAlias;

                using (var path = new GraphicsPath())
                {
                    switch (rr.Shape)
                    {
                        case ShapeType.Rectangle: path.AddRectangle(rr._rect); break;
                        case ShapeType.Ellipse: path.AddEllipse(rr._rect); break;
                        case ShapeType.Hexagon: path.AddPolygon(rr.GetHexagonVerticesLocal()); break;
                        case ShapeType.Polygon:
                            var poly = rr.GetPolygonVerticesLocal();
                            if (poly != null && poly.Length >= 2)
                            {
                                if (rr.IsPolygonClosed) path.AddPolygon(poly);
                                else path.AddLines(poly);
                            }
                            break;
                    }
                    if (outlinePen != null) g.DrawPath(outlinePen, path);
                }

              

      
        }
        static GraphicsPath BuildLocalPath(RectRotate rr)
        {
            var p = new GraphicsPath();
            switch (rr.Shape)
            {
                case ShapeType.Rectangle:
                    p.AddRectangle(rr._rect);
                    break;
                case ShapeType.Ellipse:
                    p.AddEllipse(rr._rect);
                    break;
                case ShapeType.Hexagon:
                    p.AddPolygon(rr.GetHexagonVerticesLocal());
                    break;
                case ShapeType.Polygon:
                    var poly = rr.GetPolygonVerticesLocal();
                    if (poly != null && poly.Length >= 2)
                    {
                        if (rr.IsPolygonClosed) p.AddPolygon(poly);
                        else p.AddLines(poly); // chưa đóng thì chỉ hiển thị outline
                    }
                    break;
            }
            return p;
        }

      public  static void DrawMatInRectRotate(
            Graphics g,
            Mat matProcess,
            RectRotate rr,
            float zoomPercent,
            System.Drawing.Point scroll,
            Color tint,
            float alpha = 0.5f,
            bool blackAsTransparent = true
        )
        {
            if (matProcess == null || matProcess.Empty() || rr == null) return;

            var oldSmoothing = g.SmoothingMode;
            var oldTransform = g.Transform;
            var oldClip = g.Clip; // có thể null

            g.SmoothingMode = SmoothingMode.HighQuality;

            using (var world = new Matrix())
            {
                if (!Global.IsRun)
                {
                    world.Translate(scroll.X, scroll.Y);
                    float s = Math.Max(0.0001f, zoomPercent / 100f);
                    world.Scale(s, s);
                }
                world.Translate(rr._PosCenter.X, rr._PosCenter.Y);
                world.Rotate(rr._rectRotation);
                g.Transform = world;

                using (var path = BuildLocalPath(rr))
                {
                    // Nếu polygon chưa đóng, không fill ảnh (tránh vẽ sai), chỉ return
                    if (rr.Shape == ShapeType.Polygon && !rr.IsPolygonClosed&&!Global.IsRun)
                    {
                        g.Transform = oldTransform;
                        g.SmoothingMode = oldSmoothing;
                        oldClip?.Dispose();
                        return;
                    }

                    // Bbox THỰC theo hình
                    var bounds = path.GetBounds();
                    // Trường hợp degenerate (rất mỏng): fallback sang _rect
                    if (bounds.Width < 1f || bounds.Height < 1f)
                        bounds = rr._rect;

                    using (var bmp = matProcess.ToBitmap())
                    using (var ia = new ImageAttributes())
                    {
                        float tr = tint.R / 255f;
                        float tg = tint.G / 255f;
                        float tb = tint.B / 255f;

                        ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                          {
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, alpha, 0},
                 new float[] {tr, tg, tb, 0, 1}  // kudos to OP!
                          });
                        ia.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        if (blackAsTransparent)
                            ia.SetColorKey(Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), ColorAdjustType.Bitmap);
                        else
                            ia.SetColorKey(Color.FromArgb(255,255,255), Color.FromArgb(255, 255, 255), ColorAdjustType.Bitmap);
                        // Clip đúng hình để ảnh không tràn ra ngoài
                        g.SetClip(path, CombineMode.Intersect);

                        // Vẽ khít vào bbox của path (nếu muốn giữ tỉ lệ ảnh, có thể fit theo chiều ngắn)
                        g.DrawImage(
                            bmp,
                            Rectangle.Round(bounds),
                            0, 0, bmp.Width, bmp.Height,
                            GraphicsUnit.Pixel,
                            ia
                        );

                        // Bỏ clip
                        g.ResetClip();
                    }
                }
            }

            g.Transform = oldTransform;
            g.SmoothingMode = oldSmoothing;
            oldClip?.Dispose();
        }
        public static void DrawMatInRectRotateNotMatrix(
      Graphics g,
      Mat matProcess,
      RectRotate rr,
      Color tint,
      float alpha = 0.5f,
      bool blackAsTransparent = true
  )
        {
            if (matProcess == null || matProcess.Empty() || rr == null) return;

            var oldSmoothing = g.SmoothingMode;
            var oldTransform = g.Transform;
            var oldClip = g.Clip; // có thể null

            g.SmoothingMode = SmoothingMode.HighQuality;

         

                using (var path = BuildLocalPath(rr))
                {
                    // Nếu polygon chưa đóng, không fill ảnh (tránh vẽ sai), chỉ return
                    if (rr.Shape == ShapeType.Polygon && !rr.IsPolygonClosed && !Global.IsRun)
                    {
                        g.Transform = oldTransform;
                        g.SmoothingMode = oldSmoothing;
                        oldClip?.Dispose();
                        return;
                    }

                    // Bbox THỰC theo hình
                    var bounds = path.GetBounds();
                    // Trường hợp degenerate (rất mỏng): fallback sang _rect
                    if (bounds.Width < 1f || bounds.Height < 1f)
                        bounds = rr._rect;

                    using (var bmp = matProcess.ToBitmap())
                    using (var ia = new ImageAttributes())
                    {
                        float tr = tint.R / 255f;
                        float tg = tint.G / 255f;
                        float tb = tint.B / 255f;

                        ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                          {
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, alpha, 0},
                 new float[] {tr, tg, tb, 0, 1}  // kudos to OP!
                          });
                        ia.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        if (blackAsTransparent)
                            ia.SetColorKey(Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), ColorAdjustType.Bitmap);
                        else
                            ia.SetColorKey(Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), ColorAdjustType.Bitmap);
                        // Clip đúng hình để ảnh không tràn ra ngoài
                        g.SetClip(path, CombineMode.Intersect);

                        // Vẽ khít vào bbox của path (nếu muốn giữ tỉ lệ ảnh, có thể fit theo chiều ngắn)
                        g.DrawImage(
                            bmp,
                            Rectangle.Round(bounds),
                            0, 0, bmp.Width, bmp.Height,
                            GraphicsUnit.Pixel,
                            ia
                        );

                        // Bỏ clip
                        g.ResetClip();
                    }
                }
           
        }

        static GraphicsPath BuildPathLocal(RectRotate rr)
        {
            var path = new GraphicsPath();
            switch (rr.Shape)
            {
                case ShapeType.Rectangle:
                    path.AddRectangle(rr._rect);
                    break;

                case ShapeType.Ellipse:
                    path.AddEllipse(rr._rect);
                    break;

                case ShapeType.Hexagon:
                    {
                        // 6 đỉnh trong local-rect-space (đã gồm offsets)
                        PointF[] pts = rr.GetHexagonVerticesLocal();
                        path.AddPolygon(pts);
                    }
                    break;
            }
            return path;
        }

        // Vẽ fill + stroke cho RectRotate (dựa trên zoom/scroll của ImageBox)
        static void FillRectRotate(Graphics g, RectRotate rr,
                                       float zoomPercent, Point scroll,
                                       Brush fillBrush)
        {
            if (rr == null) return;

            // Lưu state
            var oldSmoothing = g.SmoothingMode;
            var oldTransform = g.Transform;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Transform forward: screen <- local
            using (var m = new Matrix())
            {
                m.Translate(scroll.X, scroll.Y);                 // scroll của ImageBox
                float s = zoomPercent / 100f;                    // zoom %
                m.Scale(s, s);
                m.Translate(rr._PosCenter.X, rr._PosCenter.Y);   // tâm
                m.Rotate(rr._rectRotation);                      // góc
                g.Transform = m;

                using (var path = BuildPathLocal(rr))
                {
                    if (fillBrush != null) g.FillPath(fillBrush, path);
                   
                }
            }

            // Khôi phục
            g.Transform = oldTransform;
            g.SmoothingMode = oldSmoothing;
        }
        public static void RectEdit(Graphics gc, TypeCrop TypeCrop, RectRotate RectDraw,Image ImageRotate, int WidthPoint, System.Drawing.Point posAutoScroll,float zoom, System.Drawing.Point pMouse, int Thiness = 2)
        {
            if (RectDraw == null) return;
            //RectangleF _rect = new RectangleF(); ;
            //PointF _rectPos = new PointF(); ;
            //Single _rectRotation = 0;
        
            //_rect = RectDraw._rect;
            //_rectPos = RectDraw._PosCenter;
            //_rectRotation = RectDraw._rectRotation;
            if (float.IsNaN(RectDraw._rectRotation)) 
                return;
            //var rectTopLeft = new RectangleF(_rect.Left - WidthPoint / 2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
            //var rectTopRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
            //var rectBottomLeft = new RectangleF(_rect.Left - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
            //var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
            //var rectRotate = new RectangleF(-WidthPoint / 2, _rect.Top + -WidthPoint * 2, WidthPoint * 2, WidthPoint * 2);
            //var rectCenter = new RectangleF(-WidthPoint / 2, -WidthPoint / 2, WidthPoint, WidthPoint);
            Pen penRect = new Pen(Color.Orange, Thiness);
            //var backNG = new SolidBrush(Color.FromArgb(0, 0, 0, 255));
            //var backChoose = new SolidBrush(Color.FromArgb(60, 255, 205, 35));
            //var cornerNone = new SolidBrush(Color.OrangeRed);
            //var cornerChoose = new SolidBrush(Color.Blue);
            //var _clX = new Pen(Color.LightGray, 1);
            //var _clY = new Pen(Color.Gray, 1);
            //AnchorPoint AnchorPoint = RectDraw._dragAnchor;
            //Matrix mat = new Matrix();
            //mat.Translate(posAutoScroll.X, posAutoScroll.Y);
            //mat.Scale((float)(zoom / 100.0), (float)(zoom / 100.0));

            //mat.Translate(_rectPos.X, _rectPos.Y);
            //mat.Rotate(_rectRotation);
            //gc.Transform = mat;
            switch (TypeCrop)
            {
                case TypeCrop.Area:
                    penRect = new Pen(Color.DeepSkyBlue, Thiness);
                    break;
                case TypeCrop.Crop:
                    penRect = new Pen(Color.Goldenrod, Thiness);
                    break;
                case TypeCrop.Mask:
                    penRect = new Pen(Color.DarkRed, Thiness);
                    break;
            }
            DrawRectRotate(gc, RectDraw,zoom,new  System.Drawing.Point( posAutoScroll.X,posAutoScroll.Y), penRect,true,true, pMouse);
            //switch (RectDraw.Shape)
            //{
            //    case ShapeType.Ellipse:
            //        gc.DrawEllipse(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                   
            //        break;
            //    case ShapeType.Rectangle:
            //        gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

            //        break;
            //    case ShapeType.Hexagon:
            //        // nếu bạn vẽ ở world-space, chuyển local->world trước
            //        // Ở đây ví dụ vẽ local sau đó để Matrix chung xử lý
            //        PointF[] vertsLocal = RectDraw.GetHexagonVerticesLocal();
            //        float rad = Global.Config.RadEdit;
            //        for (int i = 0; i < 6; i++)
            //        {
            //            RectangleF handle = new RectangleF(vertsLocal[i].X - rad / 2f, vertsLocal[i].Y - rad / 2f, rad, rad);
            //            // fill/draw handle theo brush/pen của bạn
            //        }
            //        break;

            //}

           
            //  switch (AnchorPoint)
            //{
            //    case AnchorPoint.None:
                  
            //        gc.FillEllipse(cornerNone, rectTopLeft);
            //        gc.FillEllipse(cornerNone, rectTopRight);
            //        gc.FillEllipse(cornerNone, rectBottomLeft);
            //        gc.FillEllipse(cornerNone, rectBottomRight);
            //        //gc.FillRectangle(cornerNone, rectCenter);
            //        break;

            //    case AnchorPoint.TopLeft:
            //        rectTopLeft.Width +=WidthPoint; rectTopLeft.Height += WidthPoint;
            //        rectTopLeft.X -= WidthPoint / 2; rectTopLeft.Y -= WidthPoint / 2;
                 
            //        gc.FillRectangle(cornerChoose, rectTopLeft);
            //        gc.FillEllipse(cornerNone, rectTopRight);
            //        gc.FillEllipse(cornerNone, rectBottomLeft);
            //        gc.FillEllipse(cornerNone, rectBottomRight);
            //        gc.FillEllipse(cornerNone, rectRotate);
            //        break;
            //    case AnchorPoint.TopRight:
            //        //  gc.FillRectangle(backNone, _rect);
            //        rectTopRight.Width += WidthPoint; rectTopRight.Height += WidthPoint;
            //        rectTopRight.X -= WidthPoint / 2; rectTopRight.Y -= WidthPoint / 2;
            //        gc.FillEllipse(cornerNone, rectTopLeft);
            //        gc.FillRectangle(cornerChoose, rectTopRight);
            //        gc.FillEllipse(cornerNone, rectBottomLeft);
            //        gc.FillEllipse(cornerNone, rectBottomRight);
            //        gc.FillEllipse(cornerNone, rectRotate);
            //        break;
            //    case AnchorPoint.BottomLeft:
            //        //  gc.FillRectangle(backNone, _rect);
            //        rectBottomLeft.Width += WidthPoint; rectBottomLeft.Height += WidthPoint;
            //        rectBottomLeft.X -= WidthPoint / 2; rectBottomLeft.Y -= WidthPoint / 2;
            //        gc.FillEllipse(cornerNone, rectTopLeft);
            //        gc.FillEllipse(cornerNone, rectTopRight);
            //        gc.FillRectangle(cornerChoose, rectBottomLeft);
            //        gc.FillEllipse(cornerNone, rectBottomRight);
            //        gc.FillEllipse(cornerNone, rectRotate);
            //        break;
            //    case AnchorPoint.BottomRight:
            //        //gc.FillRectangle(backNone, _rect);
            //        rectBottomRight.Width += WidthPoint; rectBottomRight.Height += WidthPoint;
            //        rectBottomRight.X -= WidthPoint / 2; rectBottomRight.Y -= WidthPoint / 2;
            //        gc.FillEllipse(cornerNone, rectTopLeft);
            //        gc.FillEllipse(cornerNone, rectTopRight);
            //        gc.FillEllipse(cornerNone, rectBottomLeft);
            //        gc.FillRectangle(cornerChoose, rectBottomRight);
            //        gc.FillEllipse(cornerNone, rectRotate);
            //        break;
            //    case AnchorPoint.Center:
            //        // gc.FillRectangle(backChoose, _rect);
             
            //        gc.FillEllipse(cornerNone, rectTopLeft);
            //        gc.FillEllipse(cornerNone, rectTopRight);
            //        gc.FillEllipse(cornerNone, rectBottomLeft);
            //        gc.FillEllipse(cornerNone, rectBottomRight);
            //        gc.FillEllipse(cornerNone, rectRotate);
            //        break;
            //    case AnchorPoint.Rotation:
            //        // gc.FillRectangle(backNone, _rect);
            //         gc.FillEllipse(cornerNone, rectTopLeft);
            //        gc.FillEllipse(cornerNone, rectTopRight);
            //        gc.FillEllipse(cornerNone, rectBottomLeft);
            //        gc.FillEllipse(cornerNone, rectBottomRight);
            //        gc.DrawImage(ImageRotate, rectRotate);
            //        break;


            //}
          
            gc.ResetTransform();
        }
        public static void FillRect(Graphics gc, TypeCrop TypeCrop, RectRotate RectDraw, System.Drawing. Point posAutoScroll, float zoom, int Opacity =10)
        {
            if (RectDraw == null) return;
            RectangleF _rect = new RectangleF(); ;
           

            _rect = RectDraw._rect;
             Brush backcolor = new SolidBrush(Color.FromArgb(0, 0, 0, 255));
            Matrix mat = new Matrix();
            mat.Translate(posAutoScroll.X, posAutoScroll.Y);
            mat.Scale((float)(zoom / 100.0), (float)(zoom / 100.0));
            mat.Translate(RectDraw._PosCenter.X, RectDraw._PosCenter.Y);
            mat.Rotate(RectDraw._rectRotation);
            gc.Transform = mat;
            switch (TypeCrop)
            {
                case TypeCrop.Area:
                    backcolor = new SolidBrush(Color.FromArgb(Opacity, 0, 191, 255));
                    break;
                case TypeCrop.Crop:
                    backcolor = new SolidBrush(Color.FromArgb(Opacity, 255, 165, 0));
                    break;
                case TypeCrop.Mask:
                    backcolor = new SolidBrush(Color.FromArgb(Opacity, 91, 91, 91));
                    break;
            }
            switch(RectDraw.Shape)
            {
                case ShapeType.Ellipse:
                    gc.FillEllipse(backcolor, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

                    break;
                case ShapeType.Rectangle:
                    gc.FillRectangle(backcolor, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

                    break;
                case ShapeType.Hexagon:
                    FillRectRotate(gc, RectDraw, zoom, new Point(posAutoScroll.X, posAutoScroll.Y), backcolor);
                    break;
               
            }
          
              
            gc.ResetTransform();
        }
        public static void DrawInfiniteLine(Graphics g, PointF p1, PointF p2, Rectangle bounds, Pen pen)
        {
            if (p1 == p2) return; // Không thể xác định được nếu 2 điểm trùng nhau

            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            // Tránh chia cho 0 nếu là đường thẳng đứng
            if (dx == 0)
            {
                // Vẽ đường thẳng đứng đi qua p1.X
                g.DrawLine(pen, new PointF(p1.X, bounds.Top), new PointF(p1.X, bounds.Bottom));
                return;
            }

            float slope = dy / dx;
            float intercept = p1.Y - slope * p1.X;

            // Tính giao điểm với 2 cạnh trái - phải của bounds
            float xLeft = bounds.Left;
            float yLeft = slope * xLeft + intercept;

            float xRight = bounds.Right;
            float yRight = slope * xRight + intercept;

            // Cắt với phần hiển thị nếu cần
            PointF start = new PointF(xLeft, yLeft);
            PointF end = new PointF(xRight, yRight);

            g.DrawLine(pen, start, end);
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
     (
         int nLeftRect,     // x-coordinate of upper-left corner
         int nTopRect,      // y-coordinate of upper-left corner
         int nRightRect,    // x-coordinate of lower-right corner
         int nBottomRect,   // y-coordinate of lower-right corner
         int nWidthEllipse, // height of ellipse
         int nHeightEllipse // width of ellipse
     );

    }
}
