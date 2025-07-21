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
        public static System.Drawing.Size GetSizeText(string text ,Font f)
        {
         
            using (Font font =f)
            {
                // Measurement bằng TextRenderer (thường khớp với việc vẽ bằng TextRenderer.DrawText)
               return TextRenderer.MeasureText(text, font);

            
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
