using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Func
{

    public static class Line2DRectRotateXform
    {
        // =========================
        // POINT: RectLocal <-> World
        // =========================

        public static PointF RectLocalToWorld(PointF local, RectRotate rr)
        {
            double rad = rr._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(rad);
            double s = Math.Sin(rad);

            float x = (float)(local.X * c - local.Y * s);
            float y = (float)(local.X * s + local.Y * c);

            return new PointF(x + rr._PosCenter.X, y + rr._PosCenter.Y);
        }

        public static PointF WorldToRectLocal(PointF world, RectRotate rr)
        {
            float dx = world.X - rr._PosCenter.X;
            float dy = world.Y - rr._PosCenter.Y;

            double rad = -rr._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(rad);
            double s = Math.Sin(rad);

            return new PointF(
                (float)(dx * c - dy * s),
                (float)(dx * s + dy * c)
            );
        }

        // =========================
        // LINE2D TRANSFORM (ĐÚNG)
        // =========================

        public static Line2D LineLocal_SrcToLocal_Dst(Line2D lineSrcLocal, RectRotate src, RectRotate dst)
        {
            // 2 điểm trong SRC-local
            var p0_src = new PointF((float)lineSrcLocal.X1, (float)lineSrcLocal.Y1);
            var p1_src = new PointF(
                (float)(lineSrcLocal.X1 + lineSrcLocal.Vx),
                (float)(lineSrcLocal.Y1 + lineSrcLocal.Vy)
            );

            // SRC-local -> WORLD
            var p0_w = RectLocalToWorld(p0_src, src);
            var p1_w = RectLocalToWorld(p1_src, src);

            // WORLD -> DST-local
            var p0_dst = WorldToRectLocal(p0_w, dst);
            var p1_dst = WorldToRectLocal(p1_w, dst);

            // 🚨 PHẢI tạo Line2D đúng
            return LineFromTwoPoints(p0_dst, p1_dst);
        }
        public static PointF CropTopLeftToRectLocal(PointF pCrop, RectRotate rr)
        {
            // chuyển từ (0,0) top-left
            // sang rect-local (0,0) center
            return new PointF(
                pCrop.X - rr._rect.Width * 0.5f,
                pCrop.Y - rr._rect.Height * 0.5f
            );
        }
        public static PointF RectLocalTopLeftToWorld(PointF localTL, RectRotate rr)
        {
            // top-left -> centered
            var lc = new PointF(
                localTL.X - rr._rect.Width * 0.5f,
                localTL.Y - rr._rect.Height * 0.5f
            );

            return Line2DRectRotateXform.RectLocalToWorld(lc, rr);
        }
        public static PointF WorldToRectLocalTopLeft(PointF world, RectRotate rr)
        {
            // đưa world về rect-local CENTERED trước
            var lc = Line2DRectRotateXform.WorldToRectLocal(world, rr); // centered

            // centered -> top-left (0,0)
            return new PointF(
                lc.X + rr._rect.Width * 0.5f,
                lc.Y + rr._rect.Height * 0.5f
            );
        }
        public static Line2D LineWorldToLocal(Line2D lineWorld, RectRotate rr)
        {
            // 2 điểm trên line (WORLD)
            var p0w = new PointF((float)lineWorld.X1, (float)lineWorld.Y1);
            var p1w = new PointF(
                (float)(lineWorld.X1 + lineWorld.Vx),
                (float)(lineWorld.Y1 + lineWorld.Vy)
            );

            // WORLD -> local TOP-LEFT
            var p0 = WorldToRectLocalTopLeft(p0w, rr);
            var p1 = WorldToRectLocalTopLeft(p1w, rr);

            return LineFromTwoPoints(p0, p1);
        }


        public static Line2D LineLocalToWorld(Line2D lineSrcLocal, RectRotate src)
        {
            // 2 điểm trong hệ crop (top-left)
            var p0c = new PointF((float)lineSrcLocal.X1, (float)lineSrcLocal.Y1);
            var p1c = new PointF(
            (float)(lineSrcLocal.X1 + lineSrcLocal.Vx),
                (float)(lineSrcLocal.Y1 + lineSrcLocal.Vy)
            );

            // TOP-LEFT -> CENTERED rect-local
            var p0l = CropTopLeftToRectLocal(p0c, src);
            var p1l = CropTopLeftToRectLocal(p1c, src);

            // rect-local -> world
            var p0w = RectLocalToWorld(p0l, src);
            var p1w = RectLocalToWorld(p1l, src);

            return LineFromTwoPoints(p0w, p1w);
        }

        // =========================
        // HELPER (CỰC QUAN TRỌNG)
        // =========================
        private static Line2D LineFromTwoPoints(PointF p0, PointF p1)
        {
            double dx = p1.X - p0.X;
            double dy = p1.Y - p0.Y;

            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len < 1e-9) len = 1.0;

            double vx = dx / len;
            double vy = dy / len;

            return new Line2D(vx, vy, p0.X, p0.Y);
        }
    }


}
