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

