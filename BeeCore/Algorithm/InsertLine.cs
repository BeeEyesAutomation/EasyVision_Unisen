using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Algorithm
{
    public static class InsertLine
    {
        public static Line2D TransformLineLocalToGlobal(Line2D ln, RectRotate rr)
        {
            double rad = rr._rectRotation * Math.PI / 180.0;
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

            // rotate direction
            double gvx = ln.Vx * cos - ln.Vy * sin;
            double gvy = ln.Vx * sin + ln.Vy * cos;

            // rotate + translate point
            double gx0 = ln.X1 * cos - ln.Y1 * sin + rr._PosCenter.X;
            double gy0 = ln.X1 * sin + ln.Y1 * cos + rr._PosCenter.Y;

            return new Line2D(gvx, gvy, gx0, gy0);
        }
        //public static RectRotate CreateRectRotate_BotAxis(
        //  Line2D top, Line2D bot, Line2D left, Line2D right)
        //{
        //    // 1) Intersections
        //    PointF TL = Intersect(top, left);
        //    PointF TR = Intersect(top, right);
        //    PointF BR = Intersect(bot, right);
        //    PointF BL = Intersect(bot, left);

        //    // ==================================================
        //    // 2) Center hình học (không phải BR)
        //    // ==================================================
        //    PointF C = new PointF(
        //        (TL.X + TR.X + BR.X + BL.X) / 4f,
        //        (TL.Y + TR.Y + BR.Y + BL.Y) / 4f
        //    );

        //    // ==================================================
        //    // 3) Trục X theo cạnh BOT (BL -> BR)
        //    // ==================================================
        //    double ux = BR.X - BL.X;
        //    double uy = BR.Y - BL.Y;
        //    double ulen = Math.Sqrt(ux * ux + uy * uy);

        //    if (ulen < 1e-9)
        //        throw new Exception("Bot edge too small.");

        //    ux /= ulen;
        //    uy /= ulen;

        //    // ==================================================
        //    // 4) Size
        //    // ==================================================
        //    float w = Distance(BL, BR);   // chiều theo bot
        //    float h = Distance(BR, TR);   // chiều lên trên

        //    if (w <= 0 || h <= 0)
        //        throw new Exception("Invalid rectangle size.");

        //    // ==================================================
        //    // 5) Angle theo cạnh BOT
        //    // ==================================================
        //    float angleDeg = (float)(Math.Atan2(uy, ux) * 180.0 / Math.PI);

        //    // ==================================================
        //    // 6) rectLocal centered chuẩn BeeCore
        //    // ==================================================
        //    RectangleF rectLocal = new RectangleF(
        //        -w / 2f,
        //        -h / 2f,
        //        w,
        //        h
        //    );

        //    return new RectRotate(rectLocal, C, angleDeg, AnchorPoint.None);
        //}
        static void Normalize(ref double x, ref double y)
        {
            double len = Math.Sqrt(x * x + y * y);
            if (len < 1e-12) throw new Exception("Zero direction.");
            x /= len; y /= len;
        }
        public static PointF pInsert = new PointF();
        public static RectRotate CreateRectRotate_FromBotRight(
        Line2D bot,
        Line2D right,
        float width,
        float height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Invalid width/height.");

            // 1) BR
            PointF BR = Intersect(bot, right);
            pInsert = BR;
            // 2) Tạo 1 điểm thứ 2 trên bot để xác định hướng thật
            PointF P2 = new PointF(
                (float)(BR.X + bot.Vx),
                (float)(BR.Y + bot.Vy)
            );

            // 3) u = BR -> P2
            double ux = P2.X - BR.X;
            double uy = P2.Y - BR.Y;
            double ulen = Math.Sqrt(ux * ux + uy * uy);
            if (ulen < 1e-12) throw new Exception("Invalid bot direction.");
            ux /= ulen;
            uy /= ulen;

            // 4) v = perp(u)
            double vx = -uy;
            double vy = ux;

            // ép v luôn hướng lên ảnh
            if (vy > 0)
            {
                vx = -vx;
                vy = -vy;
            }

            // 5) Center
            PointF C = new PointF(
                (float)(BR.X - (width / 2f) * ux + (height / 2f) * vx),
                (float)(BR.Y - (width / 2f) * uy + (height / 2f) * vy)
            );

            float angleDeg = (float)(Math.Atan2(uy, ux) * 180.0 / Math.PI);

            RectangleF rectLocal = new RectangleF(
                -width / 2f,
                -height / 2f,
                width,
                height
            );

            return new RectRotate(rectLocal, C, angleDeg, AnchorPoint.None);
        }
        static double NormalizeAngle(double a)
        {
            while (a > Math.PI) a -= 2 * Math.PI;
            while (a < -Math.PI) a += 2 * Math.PI;

            return a;
        }
        static double MeanAngle(double a, double b)
        {
            double x = Math.Cos(a) + Math.Cos(b);
            double y = Math.Sin(a) + Math.Sin(b);

            return Math.Atan2(y, x);
        }
        public static RectRotate CreateRectRotate_FromRightBot_VisionPro(
    Line2D bot,
    Line2D right,
    float width,
    float height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Invalid width/height.");
            //---------------------------------------------------
            // 1. Intersection BR
            //---------------------------------------------------
            PointF BR = Intersect(bot, right);
            pInsert = BR;
            //---------------------------------------------------
            // 2. BOT direction
            //---------------------------------------------------
            double botAngle = Math.Atan2(bot.Vy, bot.Vx);
            // ép bot hướng sang phải
            if (Math.Cos(botAngle) < 0)
                botAngle += Math.PI;
            botAngle = NormalizeAngle(botAngle);
            //---------------------------------------------------
            // 3. RIGHT direction
            //---------------------------------------------------
            double rightAngle = Math.Atan2(right.Vy, right.Vx);
            // ép right hướng lên
            if (Math.Sin(rightAngle) > 0)
                rightAngle += Math.PI;

            rightAngle = NormalizeAngle(rightAngle);

            //---------------------------------------------------
            // 4. height direction (trung vị)
            //---------------------------------------------------
            double heightAngle = MeanAngle(
                botAngle - Math.PI / 2.0,
                rightAngle
            );

            heightAngle = NormalizeAngle(heightAngle);

            //---------------------------------------------------
            // 5. width direction
            //---------------------------------------------------
            double widthAngle = heightAngle + Math.PI / 2.0;

            widthAngle = NormalizeAngle(widthAngle);

            //---------------------------------------------------
            // 6. vectors
            //---------------------------------------------------
            double ux = Math.Cos(widthAngle);
            double uy = Math.Sin(widthAngle);

            double vx = Math.Cos(heightAngle);
            double vy = Math.Sin(heightAngle);

            Normalize(ref ux, ref uy);
            Normalize(ref vx, ref vy);

            //---------------------------------------------------
            // 7. Center
            //---------------------------------------------------
            PointF C = new PointF(
                (float)(BR.X - width / 2.0 * ux + height / 2.0 * vx),
                (float)(BR.Y - width / 2.0 * uy + height / 2.0 * vy)
            );

            //---------------------------------------------------
            // 8. angle deg
            //---------------------------------------------------
            float angleDeg = (float)(widthAngle * 180.0 / Math.PI);

            //---------------------------------------------------
            // 9. local rect
            //---------------------------------------------------
            RectangleF rectLocal = new RectangleF(
                -width / 2f,
                -height / 2f,
                width,
                height
            );

            return new RectRotate(rectLocal, C, angleDeg, AnchorPoint.None);
        }
        public static RectRotate CreateRectRotate_FromRightBot(
        Line2D bot,
        Line2D right,
        float width,
        float height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Invalid width/height.");

            // 1) BR = giao của bot & right
            PointF BR = Intersect(bot, right);
            pInsert = BR;

            // =====================================================
            // 2) v = hướng của RIGHT (BR -> TR), ép luôn hướng lên ảnh (Y âm)
            // =====================================================
            // Lấy vector right.V
            double vx = right.Vx;
            double vy = right.Vy;

            double vlen = Math.Sqrt(vx * vx + vy * vy);
            if (vlen < 1e-12) throw new Exception("Invalid right direction.");
            vx /= vlen;
            vy /= vlen;

            // ép v hướng lên (Y âm)
            if (vy > 0)
            {
                vx = -vx;
                vy = -vy;
            }

            // =====================================================
            // 3) u = perp(v) sao cho u hướng sang phải (X dương)
            //    Với v hướng lên, chọn u = (-vy, vx) sẽ ra hướng "phải"
            // =====================================================
            double ux = -vy;
            double uy = vx;

            // nếu u bị hướng trái thì đảo u (để BR đúng là bottom-right theo công thức center)
            if (ux < 0)
            {
                ux = -ux;
                uy = -uy;
            }

            // =====================================================
            // 4) Center (GIỐNG HÀM GỐC):
            //    BR -> Center = -w/2*u + h/2*v   (v là hướng lên)
            // =====================================================
            PointF C = new PointF(
                (float)(BR.X - (width / 2f) * ux + (height / 2f) * vx),
                (float)(BR.Y - (width / 2f) * uy + (height / 2f) * vy)
            );

            // 5) angle theo trục width (u) giống hàm gốc
            float angleDeg = (float)(Math.Atan2(uy, ux) * 180.0 / Math.PI);

            RectangleF rectLocal = new RectangleF(
                -width / 2f,
                -height / 2f,
                width,
                height
            );

            return new RectRotate(rectLocal, C, angleDeg, AnchorPoint.None);
        }
        public static RectRotate CreateRectRotate_BotAxis(
    Line2D top, Line2D bot, Line2D left, Line2D right)
        {
            // 1) Intersections
            PointF TL = Intersect(top, left);
            PointF TR = Intersect(top, right);
            PointF BR = Intersect(bot, right);
            PointF BL = Intersect(bot, left);

            // 2) Trục u theo cạnh BOT (BL -> BR)
            double ux = BR.X - BL.X;
            double uy = BR.Y - BL.Y;
            double ulen = Math.Sqrt(ux * ux + uy * uy);
            if (ulen < 1e-9) throw new Exception("Bot edge too small.");
            ux /= ulen; uy /= ulen;

            // 3) v = perp(u). Chọn chiều v sao cho hướng tới TR (bot -> top)
            double vx = -uy;
            double vy = ux;

            double tx = TR.X - BR.X;
            double ty = TR.Y - BR.Y;

            // nếu v đang chỉ xuống dưới (ngược TR), flip
            if (tx * vx + ty * vy < 0)
            {
                vx = -vx;
                vy = -vy;
            }

            // 4) Size
            float w = Distance(BL, BR);
            float h = Distance(BR, TR);
            if (w <= 0 || h <= 0) throw new Exception("Invalid rectangle size.");

            // 5) Center tính từ BR + w/2 + h/2
            // C = BR - (w/2)*u + (h/2)*v
            PointF C = new PointF(
                (float)(BR.X - (w / 2f) * ux + (h / 2f) * vx),
                (float)(BR.Y - (w / 2f) * uy + (h / 2f) * vy)
            );

            // 6) Angle theo cạnh BOT
            float angleDeg = (float)(Math.Atan2(uy, ux) * 180.0 / Math.PI);

            // 7) rectLocal centered chuẩn BeeCore
            RectangleF rectLocal = new RectangleF(-w / 2f, -h / 2f, w, h);

            return new RectRotate(rectLocal, C, angleDeg, AnchorPoint.None);
        }

        public static RectRotate CreateRectRotate_PivotBR(Line2D top, Line2D bot, Line2D left, Line2D right)
        {
            // Intersections
            PointF TL = Intersect(top, left);
            PointF TR = Intersect(top, right);
            PointF BR = Intersect(bot, right);   // <-- PIVOT / điểm xoay
            PointF BL = Intersect(bot, left);

            // Trục u theo top (TL->TR)
            double ux = TR.X - TL.X;
            double uy = TR.Y - TL.Y;
            double ulen = Math.Sqrt(ux * ux + uy * uy);
            if (ulen < 1e-9) throw new Exception("Top edge too small / invalid intersections.");
            ux /= ulen; uy /= ulen;

            // width/height chỉ để lấy kích thước
            float w = Distance(TL, TR);
            float h = Distance(TR, BR); // hoặc Distance(TL, BL)

            if (w <= 0 || h <= 0) throw new Exception("Invalid rectangle size from 4 lines.");

            float angleDeg = (float)(Math.Atan2(uy, ux) * 180.0 / Math.PI);

            // Local rect: gốc tại BR => BR local = (0,0), TL local = (-w,-h)
            RectangleF rectLocal = new RectangleF(-w, -h, w, h);

            // LƯU Ý: tham số thứ 2 (C) là "điểm xoay", không phải center box
            return new RectRotate(rectLocal, BR, angleDeg, AnchorPoint.BottomRight);
        }
        static float Distance(PointF a, PointF b)
        {
            float dx = a.X - b.X, dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        public static RectRotate CreateRectRotate(Line2D top, Line2D bot, Line2D left, Line2D right)
            {
                // 1) Intersections (TL, TR, BR, BL)
                PointF TL = Intersect(top, left);
                PointF TR = Intersect(top, right);
                PointF BR = Intersect(bot, right);
                PointF BL = Intersect(bot, left);

                // 2) Center
                PointF C = new PointF(
                    (TL.X + TR.X + BR.X + BL.X) / 4f,
                    (TL.Y + TR.Y + BR.Y + BL.Y) / 4f
                );

                // 3) X-axis from top edge (TL -> TR)
                double ux = TR.X - TL.X;
                double uy = TR.Y - TL.Y;
                double ulen = Math.Sqrt(ux * ux + uy * uy);
                if (ulen < 1e-9) throw new Exception("Top edge too small / invalid intersections.");

                ux /= ulen; uy /= ulen;                 // unit u
                double vx = -uy, vy = ux;               // unit v (perp)

                // 4) Project 4 corners to get width/height
                double minU = double.PositiveInfinity, maxU = double.NegativeInfinity;
                double minV = double.PositiveInfinity, maxV = double.NegativeInfinity;

                Project(TL, C, ux, uy, vx, vy, ref minU, ref maxU, ref minV, ref maxV);
                Project(TR, C, ux, uy, vx, vy, ref minU, ref maxU, ref minV, ref maxV);
                Project(BR, C, ux, uy, vx, vy, ref minU, ref maxU, ref minV, ref maxV);
                Project(BL, C, ux, uy, vx, vy, ref minU, ref maxU, ref minV, ref maxV);

                float w = (float)(maxU - minU);
                float h = (float)(maxV - minV);
                if (w <= 0 || h <= 0) throw new Exception("Invalid rectangle size from 4 lines.");

                // 5) Angle in degrees (u-axis)
                float angleDeg = (float)(Math.Atan2(uy, ux) * 180.0 / Math.PI);

                // Local rect centered at origin
                RectangleF rectLocal = new RectangleF(-w / 2f, -h / 2f, w, h);

                return new RectRotate(rectLocal, C, angleDeg,AnchorPoint.None);
            }

            private static void Project(
                PointF P, PointF C,
                double ux, double uy, double vx, double vy,
                ref double minU, ref double maxU, ref double minV, ref double maxV)
            {
                double dx = P.X - C.X;
                double dy = P.Y - C.Y;

                double pu = dx * ux + dy * uy;
                double pv = dx * vx + dy * vy;

                if (pu < minU) minU = pu;
                if (pu > maxU) maxU = pu;
                if (pv < minV) minV = pv;
                if (pv > maxV) maxV = pv;
            }

            // Intersection of 2 lines in param form:
            // L1: P = P1 + t*d1, L2: Q = P2 + s*d2
            public static PointF Intersect(Line2D a, Line2D b)
            {
            double ax = a.X1, ay = a.Y1;
                double bx = b.X1, by = b.Y1;
                double avx = a.Vx, avy = a.Vy;
                double bvx = b.Vx, bvy = b.Vy;

                double det = avx * (-bvy) - avy * (-bvx); // det of [d1, d2]
                                                          // Equivalent: det = avx*bvy - avy*bvx
                det = avx * bvy - avy * bvx;

                if (Math.Abs(det) < 1e-12)
                    throw new Exception("Two lines are (nearly) parallel; cannot intersect reliably.");

                // Solve: aP + t*aD = bP + s*bD
                // t = ((bP-aP) x bD) / (aD x bD)
                double dx = bx - ax;
                double dy = by - ay;

                double t = (dx * bvy - dy * bvx) / det;

                return new PointF((float)(ax + t * avx), (float)(ay + t * avy));
            }
                private static bool IsHorizontal(double angDeg)
                {
                    // gần 0° hoặc 180°
                    return Math.Abs(angDeg) <= 45.0 || Math.Abs(Math.Abs(angDeg) - 180.0) <= 45.0;
                }
                public static SideLR GetLineA_Side(Line2D lineA, Point2f P)
                  {
                    // double s = SignedSide(lineA, P);
                    return lineA.X1 < P.X ? SideLR.Left : SideLR.Right;
                }
                public static SideTB GetLineB_Side(Line2D lineB, Point2f P)
                {

                    return lineB.Y1 > P.Y ? SideTB.Below : SideTB.Above;

                }
                private static double AngleDegFromVector(double vx, double vy)
                {
                    double ang = Math.Atan2(vy, vx) * 180.0 / Math.PI;
                    //if (ang > 180) ang -= 360;
                    //if (ang < -180) ang += 360;
                    return ang;
                }
                public static double GetAngleAndSide(Line2D A,Line2D B,PointF P,out SideLR sideLR, out SideTB sideTB )
                {
                    double ang1 = AngleDegFromVector(A.Vx, A.Vy);
                    double ang2 = AngleDegFromVector(B.Vx, B.Vy);

            // ÉP QUY ƯỚC:
            // LineA = ngang, LineB = dọc
                    Line2D lineA = A;
                     Line2D lineB = B;
                    double angA = ang1;
                    double angB = ang2;

                    if (!IsHorizontal(ang1) && IsHorizontal(ang2))
                    {
                        // swap
                        lineA = B;
                        lineB = A;
                        angA = ang2;
                        angB = ang1;
                    }
                sideLR = GetLineA_Side(lineA,new Point2f( P.X,P.Y));
                sideTB = GetLineB_Side(lineB,  new Point2f(P.X, P.Y));
             return angA;
                }
    }
    }

