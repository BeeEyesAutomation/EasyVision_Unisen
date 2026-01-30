using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;

namespace BeeCore.Func
{
    public class Cal
    {
        public static PointF WorldToRectLocal(PointF world, RectRotate outer)
        {
            // 1. tịnh tiến về tâm outer
            float dx = world.X - outer._PosCenter.X;
            float dy = world.Y - outer._PosCenter.Y;

            // 2. xoay NGƯỢC theo rotation outer
            double r = -outer._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(r);
            double s = Math.Sin(r);

            return new PointF(
                (float)(dx * c - dy * s),
                (float)(dx * s + dy * c)
            );
        }
        public static RectRotate MapInnerRectToOuterLocal(
        RectRotate inner,
        RectRotate outer)
        {
            // 1. clone để không phá object gốc
            RectRotate r = inner.Clone();

            // 2. center inner -> local outer
            PointF centerLocal = WorldToRectLocal(inner._PosCenter, outer);

            // 3. cập nhật center
            r._PosCenter = centerLocal;

            // 4. rotation tương đối
            r._rectRotation = inner._rectRotation - outer._rectRotation;

            // 5. rect local giữ nguyên (-w/2,-h/2,w,h)
            // r._rect KHÔNG ĐƯỢC ĐỘNG

            return r;
        }
        public static double DistanceLine2D_RectRotate(
             Line2D line,
             BeeGlobal.RectRotate rect,
            
             out PointF closestPointOnRect)
        {
           
            closestPointOnRect = default;

            Point2f[] pts = GetRectWorldPoints(rect);
            double minDist = double.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                Point2f a = pts[i];
                Point2f b = pts[(i + 1) & 3];

                // ===== line cắt cạnh =====
                if (LineIntersectsSegment(line, a, b, out Point2f ip))
                {
                   
                    closestPointOnRect = new PointF(ip.X,ip.Y);
                    return 0.0;
                }

                // ===== không cắt → lấy endpoint gần nhất =====
                double d0 = DistancePointToLine(line, a);
                double d1 = DistancePointToLine(line, b);

                if (d0 < minDist)
                {
                    minDist = d0;
                   
                    closestPointOnRect = new PointF(a.X, a.Y);
                }
                if (d1 < minDist)
                {
                    minDist = d1;
                   
                    closestPointOnRect = new PointF(b.X, b.Y);
                }
            }

            return minDist;
        }

        // ============================================================
        // MATH CORE (OpenCvSharp Line2D)
        // ============================================================

        // Distance từ điểm → Line2D
        static double DistancePointToLine(Line2D l, Point2f p)
        {
            double dx = p.X - l.X1;
            double dy = p.Y - l.Y1;
            return Math.Abs(dx * l.Vy - dy * l.Vx);
        }

        // Line2D (vô hạn) cắt segment hay không
        static bool LineIntersectsSegment(
            Line2D l,
            Point2f a,
            Point2f b,
            out Point2f intersection)
        {
            intersection = default;

            double d1 = (a.X - l.X1) * l.Vy - (a.Y - l.Y1) * l.Vx;
            double d2 = (b.X - l.X1) * l.Vy - (b.Y - l.Y1) * l.Vx;

            if (d1 * d2 > 0)
                return false;

            double t = d1 / (d1 - d2);

            intersection = new Point2f(
                (float)(a.X + t * (b.X - a.X)),
                (float)(a.Y + t * (b.Y - a.Y))
            );
            return true;
        }

        // ============================================================
        // RECTROTATE
        // ============================================================

        static Point2f[] GetRectWorldPoints(BeeGlobal.RectRotate r)
        {
            float w = r._rect.Width;
            float h = r._rect.Height;

            float hw = w * 0.5f;
            float hh = h * 0.5f;

            PointF[] local =
            {
                new PointF(-hw,-hh),
                new PointF(+hw,-hh),
                new PointF(+hw,+hh),
                new PointF(-hw,+hh)
            };

            Point2f[] world = new Point2f[4];

            for (int i = 0; i < 4; i++)
            {
                PointF rot = BeeGlobal.RectRotate.Rotate(local[i], r._rectRotation);
                world[i] = new Point2f(
                    r._PosCenter.X + rot.X,
                    r._PosCenter.Y + rot.Y
                );
            }

            return world;
        }
    

        // ========================= RECT =========================

      
        public static System.Drawing.Size GetSizeText(string text, Font font)
        {
            using (Bitmap bmp = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                return System.Drawing.Size.Ceiling(g.MeasureString(text, font));
            }
        }
        public static void GetLineParams(Line2D ln, out double a, out double b, out double c)
        {
            double vx = ln.Vx;
            double vy = ln.Vy;
            a = vy;
            b = -vx;
            c = -(a * ln.X1 + b * ln.Y1);
        }
        public static Line2D FindPerpendicularLine(Line2D baseLine, Point throughPoint)
        {
            // Vector chỉ phương của baseLine
            double vx = baseLine.Vx;
            double vy = baseLine.Vy;
            // Vector pháp tuyến (perpendicular)
            double px = -vy;
            double py = vx;
            // Chuẩn hóa
            double norm = Math.Sqrt(px * px + py * py);
            px /= norm; py /= norm;
            Point[] points = new Point[2];
            // Tạo hai điểm rất xa trên đường thẳng vuông góc
            points[0] = new Point((int)Math.Round(throughPoint.X + px * 1000),
                                (int)Math.Round(throughPoint.Y + py * 1000));
            points[1] = new Point((int)Math.Round(throughPoint.X - px * 1000),
                                (int)Math.Round(throughPoint.Y - py * 1000));
            // 4. Fit center lines (Line2D) nếu cần
            return Cv2.FitLine(points.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);

        }
    
        public static double DistanceBetweenLines(Line2D l1, Line2D l2)
        {
            // Local helper without static keyword
            void Params(Line2D ln, out double a, out double b, out double c)
            {
                double vx = ln.Vx;
                double vy = ln.Vy;
                double x0 = ln.X1;
                double y0 = ln.Y1;
                a = vy; b = -vx; c = -(a * x0 + b * y0);
            }

            Params(l1, out double a1, out double b1, out double c1);
            Params(l2, out double a2, out double b2, out double c2);
            return Math.Abs(c2 - c1) / Math.Sqrt(a1 * a1 + b1 * b1);
        }

     
        public static float Finddistasnce( PointF A, PointF B)
        {
            float deX = A.X - B.X;
            float deY = A.Y - B.Y;

            return (float)( Math.Sqrt(deX*deX+deY*deY));

        }
        public static bool FindIntersection(
  PointF A, PointF B,
    PointF C, PointF D,
    out PointF intersection)
        {
            intersection = new PointF();

            float dx1 = B.X - A.X;
            float dy1 = B.Y - A.Y;
            float dx2 = D.X - C.X;
            float dy2 = D.Y - C.Y;

            float determinant = dx1 * dy2 - dy1 * dx2;

            // Nếu determinant = 0 → hai đường song song (không giao nhau hoặc trùng)
            if (Math.Abs(determinant) < 1e-10)
                return false;

            float dx = C.X - A.X;
            float dy = C.Y - A.Y;

            float t = (dx * dy2 - dy * dx2) / determinant;

            intersection = new PointF(
                A.X + t * dx1,
                A.Y + t * dy1
            );

            return true;
        
        }
        public static float AngleDeg_FromB_ToA(PointF A, PointF B)
        {
            double dx = A.X - B.X;
            double dy = A.Y - B.Y;

            double rad = Math.Atan2(dy, dx);          // [-pi, +pi]
            double deg = rad * 180.0 / Math.PI;       // [-180, +180]

            if (deg < 0) deg += 360.0;                // [0, 360)
            return (float)deg;
        }
        public static float AngleDeg_FromB_ToA_MathYUp(PointF A, PointF B)
        {
            double dx = A.X - B.X;
            double dy = -(A.Y - B.Y);                 // đảo trục Y
            double deg = Math.Atan2(dy, dx) * 180.0 / Math.PI;
            if (deg < 0) deg += 360.0;
            return (float)deg;
        }
        public static double GetAngleBetweenSegments(PointF A, PointF B, PointF C, PointF D)
        {
            // Vector AB
            float v1x = B.X - A.X;
            float v1y = B.Y - A.Y;

            // Vector CD
            float v2x = D.X - C.X;
            float v2y = D.Y - C.Y;

            // Dot product
            double dot = v1x * v2x + v1y * v2y;

            // Độ dài hai vector
            double mag1 = Math.Sqrt(v1x * v1x + v1y * v1y);
            double mag2 = Math.Sqrt(v2x * v2x + v2y * v2y);

            if (mag1 == 0 || mag2 == 0)
                return 0; // Tránh chia cho 0

            // Cosine của góc
            double cosTheta = dot / (mag1 * mag2);

            // Clamp để tránh lỗi do sai số (cos có thể hơi >1 hoặc <-1)
            cosTheta = Math.Max(-1.0, Math.Min(1.0, cosTheta));

            // Góc theo radian
            double angleRad = Math.Acos(cosTheta);

            // Đổi sang độ
            double angleDeg = angleRad * 180.0 / Math.PI;

            // Luôn trả về góc nhỏ nhất (0–180 độ)
            if (angleDeg > 90) angleDeg = 180 - angleDeg;
            return angleDeg;
        }
    }
}
