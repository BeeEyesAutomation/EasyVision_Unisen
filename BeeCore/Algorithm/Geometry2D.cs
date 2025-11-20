using System;
using OpenCvSharp;

public static class Geometry2D
{
    /// <summary>
    /// Tìm chân đường vuông góc từ điểm p xuống đường thẳng (a,b).
    /// Có thể clamp vào đoạn thẳng [a,b] nếu muốn.
    /// </summary>
    /// <param name="p">Điểm nguồn</param>
    /// <param name="a">Đầu đoạn / 1 điểm trên line</param>
    /// <param name="b">Đầu đoạn / điểm còn lại</param>
    /// <param name="foot">Chân đường vuông góc</param>
    /// <param name="t">Tham số dọc theo (a->b): foot = a + t*(b-a). Nếu không clamp: t∈(-∞,+∞); clamp: t∈[0,1]</param>
    /// <param name="clampToSegment">true: ép về đoạn [a,b]; false: line vô hạn</param>
    /// <param name="epsilon">Ngưỡng coi (a,b) là trùng nhau</param>
    /// <returns>true nếu tính được; false nếu (a,b) gần như trùng nhau</returns>
    public static bool TryPerpendicularFoot(
        Point2d p, Point2d a, Point2d b,
        out Point2d foot, out double t,
        bool clampToSegment = false, double epsilon = 1e-12)
    {
        double vx = b.X - a.X;
        double vy = b.Y - a.Y;
        double vv = vx * vx + vy * vy;

        if (vv < epsilon)
        {
            // line suy biến: trả về chính a
            foot = a;
            t = 0.0;
            return false;
        }

        double px = p.X - a.X;
        double py = p.Y - a.Y;

        // t = (AP·AB)/|AB|^2
        t = (px * vx + py * vy) / vv;

        if (clampToSegment)
        {
            if (t < 0.0) t = 0.0;
            else if (t > 1.0) t = 1.0;
        }

        foot = new Point2d(a.X + t * vx, a.Y + t * vy);
        return true;
    }

    /// <summary>
    /// Overload tiện cho LineSegmentPoint (OpenCvSharp)
    /// </summary>
    public static bool TryPerpendicularFoot(
        Point2d p, LineSegmentPoint line,
        out Point2d foot, out double t,
        bool clampToSegment = false, double epsilon = 1e-12)
    {
        var a = new Point2d(line.P1.X, line.P1.Y);
        var b = new Point2d(line.P2.X, line.P2.Y);
        return TryPerpendicularFoot(p, a, b, out foot, out t, clampToSegment, epsilon);
    }

    /// <summary>
    /// Khoảng cách vuông góc từ p tới line (hoặc segment nếu clamp=true).
    /// </summary>
    public static double PerpendicularDistance(
        Point2d p, Point2d a, Point2d b, bool clampToSegment = false)
    {
        Point2d foot;
        double t;
        TryPerpendicularFoot(p, a, b, out foot, out t, clampToSegment);
        double dx = p.X - foot.X;
        double dy = p.Y - foot.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
