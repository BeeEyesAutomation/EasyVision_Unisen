using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Func
{

    public static class PolyOffset
    {
        /// Offset radial quanh tâm (0,0) trong hệ local của RectRotate.
        /// offset > 0: nở ra, offset < 0: thu vào.
        public static List<PointF> OffsetRadial(IList<PointF> polyLocal, float offset)
        {
            const float EPS = 1e-6f;
            if (polyLocal == null || polyLocal.Count < 3) return new List<PointF>();

            bool closed = IsClosed(polyLocal);
            int N = closed ? polyLocal.Count - 1 : polyLocal.Count;

            var outPts = new List<PointF>(polyLocal.Count);

            for (int i = 0; i < N; i++)
            {
                var p = polyLocal[i];
                float r = (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
                if (r < EPS)
                {
                    // điểm sát tâm: đẩy theo trục X để tránh NaN
                    outPts.Add(new PointF(p.X + offset, p.Y));
                }
                else
                {
                    float k = 1f + offset / r;   // tăng bán kính thêm 'offset'
                    outPts.Add(new PointF(p.X * k, p.Y * k));
                }
            }

            // đóng kín nếu đầu vào đóng
            if (closed) outPts.Add(outPts[0]);
            return outPts;
        }

        private static bool IsClosed(IList<PointF> pts)
            => pts.Count >= 3 && Distance(pts[0], pts[pts.Count - 1]) < 1e-3f;

        private static float Distance(PointF a, PointF b)
        {
            float dx = a.X - b.X, dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }

}
