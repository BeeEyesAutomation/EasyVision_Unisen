using System;
using System.Collections.Generic;
using System.Drawing;

public static class PolyOffset
{
    /// <summary>
    /// Offset theo % trục X/Y quanh tâm (0,0).
    /// pctXPercent = 50 nghĩa là nở 50% theo chiều rộng; pctYPercent = 50 nghĩa là nở 50% theo chiều cao.
    /// </summary>
    /// <summary>
    /// Offset RADIAL tuyệt đối (pixel): tăng bán kính mỗi điểm thêm 'offset' px.
    /// offset > 0: nở ra, offset < 0: thu vào.
    /// </summary>
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

        if (closed) outPts.Add(outPts[0]);
        return outPts;
    }

    public static List<PointF> OffsetAxisPercent(IList<PointF> polyLocal, float pctXPercent, float pctYPercent)
    {
        const float EPS = 1e-6f;
        if (polyLocal == null || polyLocal.Count < 3) return new List<PointF>();

        bool closed = IsClosed(polyLocal);
        int N = closed ? polyLocal.Count - 1 : polyLocal.Count;

        float kx = 1f + (pctXPercent / 100f);
        float ky = 1f + (pctYPercent / 100f);

        var outPts = new List<PointF>(polyLocal.Count);
        for (int i = 0; i < N; i++)
        {
            var p = polyLocal[i];
            if (Math.Abs(p.X) < EPS && Math.Abs(p.Y) < EPS)
                outPts.Add(new PointF((float)(EPS * kx), 0));
            else
                outPts.Add(new PointF(p.X * kx, p.Y * ky));
        }

        if (closed) outPts.Add(outPts[0]);
        return outPts;
    }

    /// <summary>
    /// Offset theo % từng cạnh: trái/phải và trên/dưới (đơn vị phần trăm).
    /// - Điểm X<0 dùng pctLeftPercent, X>=0 dùng pctRightPercent.
    /// - Điểm Y<0 dùng pctTopPercent,  Y>=0 dùng pctBottomPercent.
    /// Ví dụ: pctLeftPercent=50f nghĩa là nở 50% về phía trái.
    /// </summary>
    public static List<PointF> OffsetSidesPercent(
        IList<PointF> polyLocal,
        float pctLeftPercent, float pctRightPercent,
        float pctTopPercent, float pctBottomPercent)
    {
        const float EPS = 1e-6f;
        if (polyLocal == null || polyLocal.Count < 3) return new List<PointF>();

        bool closed = IsClosed(polyLocal);
        int N = closed ? polyLocal.Count - 1 : polyLocal.Count;

        var outPts = new List<PointF>(polyLocal.Count);
        for (int i = 0; i < N; i++)
        {
            var p = polyLocal[i];

            float kx = 1f + ((p.X >= 0 ? pctRightPercent : pctLeftPercent) / 100f);
            float ky = 1f + ((p.Y >= 0 ? pctBottomPercent : pctTopPercent) / 100f);

            if (Math.Abs(p.X) < EPS && Math.Abs(p.Y) < EPS)
                outPts.Add(new PointF((float)(EPS * kx), 0));
            else
                outPts.Add(new PointF(p.X * kx, p.Y * ky));
        }

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
