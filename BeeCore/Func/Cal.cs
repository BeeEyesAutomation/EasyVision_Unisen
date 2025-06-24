using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Func
{
    public class Cal
    {
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
