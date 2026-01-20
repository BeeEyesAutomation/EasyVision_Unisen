using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using SD = System.Drawing; // alias: dùng SD.PointF nếu cần đọc dữ liệu từ RectRotate

namespace BeeCore
{
    public static class Cropper
    {
        public sealed class PatchCropContext : IDisposable
        {
            public Mat Patch;                  // patch sau bước 1 (bạn có thể chỉnh sửa/edge process)
            public SD.PointF WorldAnchor;      // tâm neo trong world
            public Size2f RectSize;            // kích thước (đã hoán đổi nếu cần) dùng để cut patch
            public Point2f PatchCenter;        // tâm patch (W/2,H/2)
            public float AngleUsed;            // góc đã dùng để warp (đã normalize -45 logic)
            public SD.PointF LocalCenterForShape; // (0,0) non-polygon; (cx,cy) polygon
            public int PatchW, PatchH;

            // Tuỳ chọn: lưu Ma trận warp nếu bạn cần debug (không cần cho masking lần 2)
            public Mat M;

            public void Dispose()
            {
                if (M != null) { M.Dispose(); M = null; }
                if (Patch != null) { Patch.Dispose(); Patch = null; }
            }
        }
        // (1) Cắt outer rotated bounding patch — trả về Mat patch
        public static Mat CropOuterPatch(Mat source, RectRotate rot, out PatchCropContext ctx)
        {
            ctx = null;
            if (source == null || source.Empty()) return new Mat();

            // 1) Neo & size theo shape
            Size2f rectSize;
            SD.PointF localCenterForShape;
            GetAnchorSizeFor(rot, out SD.PointF worldAnchor, out rectSize, out localCenterForShape);

            // 2) Chuẩn hoá góc & có thể hoán đổi W/H
            double angleUsed = rot._rectRotation;
            if (angleUsed < -45.0)
            {
                angleUsed += 90.0;
                float tmp = rectSize.Width; rectSize.Width = rectSize.Height; rectSize.Height = tmp;
            }

            var anchor = new Point2f(worldAnchor.X, worldAnchor.Y);

            Mat M = null, warped = null, patch = null;
            try
            {
                // 3) Warp quanh anchor
                M = Cv2.GetRotationMatrix2D(anchor, angleUsed, 1.0);
                warped = new Mat();
                Cv2.WarpAffine(
                    src: source, dst: warped, m: M, dsize: source.Size(),
                    flags: InterpolationFlags.Cubic,
                    borderMode: BorderTypes.Constant,
                    borderValue: new Scalar(0, 0, 0)
                );

                // 4) Lấy patch theo size đã chuẩn hoá
                patch = new Mat();
                Cv2.GetRectSubPix(
                    image: warped,
                    patchSize: new OpenCvSharp.Size(rectSize.Width, rectSize.Height),
                    center: anchor,
                    patch: patch
                );
                if (patch.Empty())
                {
                    patch.Dispose();
                    return new Mat();
                }

                // Tạo ctx để dùng cho bước 2 (mask)
                ctx = new PatchCropContext
                {
                    Patch = null, // chủ đích: quyền sở hữu patch trả về cho caller
                    WorldAnchor = worldAnchor,
                    RectSize = rectSize,
                    PatchW = patch.Cols,
                    PatchH = patch.Rows,
                    PatchCenter = new Point2f(patch.Cols * 0.5f, patch.Rows * 0.5f),
                    AngleUsed = (float)angleUsed,
                    LocalCenterForShape = localCenterForShape,
                    M = M // giữ nếu bạn cần debug; nếu không, có thể bỏ để tiết kiệm RAM
                };

                // có thể giải phóng warped
                warped.Dispose(); warped = null;

                // LƯU Ý sở hữu:
                // - Trả về 'patch' cho caller. ctx không giữ patch để tránh dispose trùng.
                M = null; // đã chuyển cho ctx; tránh dispose 2 lần
                return patch;
            }
            catch
            {
                if (patch != null) patch.Dispose();
                if (warped != null) warped.Dispose();
                if (M != null) M.Dispose();
                ctx = null;
                return new Mat();
            }
        }
        public static Mat ApplyShapeMaskAndCompose(
                    Mat patchInput,
                    PatchCropContext ctx,
                    RectRotate rot,
                    RectRotate rotMask,          // có thể null
                    bool returnMaskOnly = false,
                    bool whiteBackground = false // nếu bạn muốn ép nền trắng/đen thay vì dùng rot.IsWhite
                )
        {
            if (patchInput == null || patchInput.Empty() || ctx == null) return new Mat();

            // Góc DƯ trong hệ patch
            float angleInPatchCrop = (float)(rot._rectRotation - ctx.AngleUsed);
            if (angleInPatchCrop > 180f) angleInPatchCrop -= 360f;
            if (angleInPatchCrop < -180f) angleInPatchCrop += 360f;

            Mat cropMask = null, mask2 = null, finalMask = null, bgMat = null, result = null;
            try
            {
                int patchH = patchInput.Rows;
                int patchW = patchInput.Cols;

                cropMask = new Mat(patchH, patchW, MatType.CV_8UC1, new Scalar(0));

                // VẼ MASK chính xác theo shape của rot, đặt giữa patch bằng cách dùng localCenterForShape
                DrawShapeMaskIntoWithSize(
                    rot,
                    cropMask,
                    ctx.PatchCenter,
                    angleInPatchCrop,
                    255,
                    (int)Math.Round(ctx.RectSize.Width),
                    (int)Math.Round(ctx.RectSize.Height),
                    ctx.LocalCenterForShape
                );

                // Nếu có mask loại trừ
                if (rotMask != null)
                {
                    mask2 = new Mat(patchH, patchW, MatType.CV_8UC1, new Scalar(255));

                    // Tính neo/size riêng của mask
                    Size2f maskSize;
                    SD.PointF maskLocalCenter;
                    GetAnchorSizeFor(rotMask, out SD.PointF worldAnchorMask, out maskSize, out maskLocalCenter);

                    // Delta neo giữa worldAnchorMask và ctx.WorldAnchor → chuyển về hệ patch
                    Point2f deltaWorld = new Point2f(
                        (float)(worldAnchorMask.X - ctx.WorldAnchor.X),
                        (float)(worldAnchorMask.Y - ctx.WorldAnchor.Y)
                    );
                    Point2f deltaInPatch = RotatePoint(deltaWorld, -ctx.AngleUsed);
                    Point2f maskCenterInPatch = new Point2f(
                        ctx.PatchCenter.X + deltaInPatch.X,
                        ctx.PatchCenter.Y + deltaInPatch.Y
                    );

                    float maskAngleInPatch = (float)(rotMask._rectRotation - ctx.AngleUsed);

                    // Vẽ vùng cần loại trừ (0) vào mask2
                    DrawShapeMaskIntoWithSize(
                        rotMask,
                        mask2,
                        maskCenterInPatch,
                        maskAngleInPatch,
                        0,
                        (int)Math.Round(maskSize.Width),
                        (int)Math.Round(maskSize.Height),
                        maskLocalCenter
                    );

                    finalMask = new Mat();
                    Cv2.BitwiseAnd(cropMask, mask2, finalMask);
                }
                else
                {
                    finalMask = cropMask.Clone();
                }

                if (returnMaskOnly)
                {
                    return finalMask.Clone();
                }
                bool isWhite = false;
                if (rotMask!=null)
                // Nền (trắng/đen) — giữ tương thích tham số IsWhite cũ
                 isWhite = whiteBackground ? true : rotMask.IsWhite;
                Scalar bg = isWhite
                    ? new Scalar(255, 255, 255, 255)
                    : new Scalar(0, 0, 0, 0);

                bgMat = new Mat(patchInput.Size(), patchInput.Type(), bg);
                result = bgMat.Clone();
                patchInput.CopyTo(result, finalMask);
                return result;
            }
            catch
            {
                return new Mat();
            }
            finally
            {
                if (cropMask != null) cropMask.Dispose();
                if (mask2 != null) mask2.Dispose();
                if (finalMask != null && result == null) finalMask.Dispose();
                if (bgMat != null) bgMat.Dispose();
            }
        }
        public static Mat CropRotatedRect(
            Mat source, RectRotate rot, RectRotate rotMask,
            bool returnMaskOnly = false)
        {
            if (source == null || source.Empty()) return new Mat();

            // ==== 1) Xác định tâm neo & kích thước theo shape ====
            // Với Polygon: tâm neo = PosCenter + Rotate(bboxLocalCenter, rectRotation)
            //              size = kích thước bboxLocal của polygon
            // Các shape khác: tâm neo = PosCenter, size = _rect.(W,H)
            Size2f rectSize;
            SD.PointF localCenterForShape; // (0,0) cho non-Polygon; (cx,cy) cho Polygon
            GetAnchorSizeFor(rot, out SD.PointF worldAnchor, out rectSize, out localCenterForShape);

            // ==== 2) Chuẩn hoá góc/size crop (như nguyên tác) ====
            double angleUsed = rot._rectRotation; // xoay ảnh quanh worldAnchor
            if (angleUsed < -45.0)
            {
                angleUsed += 90.0;
                float tmp = rectSize.Width; rectSize.Width = rectSize.Height; rectSize.Height = tmp;
            }

            // Góc DƯ trong hệ patch
            float angleInPatchCrop = (float)(rot._rectRotation - angleUsed);
            if (angleInPatchCrop > 180f) angleInPatchCrop -= 360f;
            if (angleInPatchCrop < -180f) angleInPatchCrop += 360f;

            Point2f anchor = new Point2f(worldAnchor.X, worldAnchor.Y);

            Mat M = null, warped = null, patch = null, cropMask = null, mask2 = null, finalMask = null, result = null, bgMat = null;

            try
            {
                // 3) Warp ảnh quanh anchor (giữ nguyên như cũ)
                M = Cv2.GetRotationMatrix2D(anchor, angleUsed, 1.0);
                warped = new Mat();
                Cv2.WarpAffine(source, warped, M, source.Size(), InterpolationFlags.Cubic, BorderTypes.Constant, new Scalar(0, 0, 0));

                // 4) Cắt patch theo size của shape (Polygon dùng size bboxLocal)
                patch = new Mat();
                Cv2.GetRectSubPix(warped, new OpenCvSharp.Size(rectSize.Width, rectSize.Height), anchor, patch);
                if (patch.Empty()) return new Mat();

                int patchH = patch.Rows;
                int patchW = patch.Cols;
                Point2f patchCenter = new Point2f(patchW * 0.5f, patchH * 0.5f);

                // 5) Mask crop 8UC1
                cropMask = new Mat(patchH, patchW, MatType.CV_8UC1, new Scalar(0));

                // >>> VẼ MASK với góc DƯ & kích thước khớp patch
                //     Với Polygon: sẽ tự trừ localCenterForShape bên trong để mask nằm giữa patch
                DrawShapeMaskIntoWithSize(rot, cropMask, patchCenter, angleInPatchCrop, 255,
                                          (int)Math.Round(rectSize.Width), (int)Math.Round(rectSize.Height),
                                          localCenterForShape);
                Scalar bg= new Scalar(0, 0, 0, 0);
                // 6) Mask loại trừ (nếu có)
                if (rotMask != null)
                {
                    mask2 = new Mat(patchH, patchW, MatType.CV_8UC1, new Scalar(255));

                    // ---- Tính anchor riêng cho mask (Polygon thì dùng bbox local center) ----
                    Size2f maskSize;
                    SD.PointF maskLocalCenter;
                    GetAnchorSizeFor(rotMask, out SD.PointF worldAnchorMask, out maskSize, out maskLocalCenter);

                    // Delta giữa 2 tâm neo (trong world), đưa vào hệ patch
                    Point2f deltaWorld = new Point2f(
                        (float)(worldAnchorMask.X - worldAnchor.X),
                        (float)(worldAnchorMask.Y - worldAnchor.Y)
                    );
                    Point2f deltaInPatch = RotatePoint(deltaWorld, (float)(-angleUsed));
                    Point2f maskCenterInPatch = new Point2f(patchCenter.X + deltaInPatch.X,
                                                            patchCenter.Y + deltaInPatch.Y);
                    float maskAngleInPatch = (float)(rotMask._rectRotation - angleUsed);

                    // Vẽ mask phụ đúng kích thước & tâm neo của chính nó
                    DrawShapeMaskIntoWithSize(rotMask, mask2, maskCenterInPatch, maskAngleInPatch, 0,
                                              (int)Math.Round(maskSize.Width),
                                              (int)Math.Round(maskSize.Height),
                                              maskLocalCenter);

                    finalMask = new Mat();
                    Cv2.BitwiseAnd(cropMask, mask2, finalMask);
                    bg = rotMask.IsWhite ? new Scalar(255, 255, 255, 255) : new Scalar(0, 0, 0, 0);
                }
                else
                {
                    finalMask = cropMask.Clone();
                }

                if (returnMaskOnly)
                    return finalMask.Clone();

                // 7) Áp mask lên nền
             
                bgMat = new Mat(patch.Size(), patch.Type(), bg);
                result = bgMat.Clone();
                patch.CopyTo(result, finalMask);
                return result;
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Crop", ex.Message));
                return new Mat();
            }
            finally
            {
                if (M != null) M.Dispose();
                if (warped != null) warped.Dispose();
                if (patch != null) patch.Dispose();
                if (cropMask != null) cropMask.Dispose();
                if (mask2 != null) mask2.Dispose();
                if (finalMask != null && result == null) finalMask.Dispose();
                if (bgMat != null) bgMat.Dispose();
            }
        }
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        public static Mat CropRect(Mat source, RectRotate rot)
        {

            MatType TypeMat = source.Type();
            Mat matResult = new Mat();

            Point2f pCenter = new Point2f(rot._PosCenter.X, rot._PosCenter.Y);
            Size2f rect_size = new Size2f(rot._rect.Size.Width, rot._rect.Size.Height);
            RotatedRect rot2 = new RotatedRect(pCenter, rect_size, rot._rectRotation);
            double angle = rot._rectRotation;
            if (angle < -45)
            {
                angle += 90.0;

                Swap(ref rect_size.Width, ref rect_size.Height);
            }
            InputArray M = Cv2.GetRotationMatrix2D(rot2.Center, angle, 1.0);
            Mat mCrop = new Mat();
            Mat crop1 = new Mat();
            try
            {

                Cv2.WarpAffine(source, crop1, M, source.Size(), InterpolationFlags.Cubic);

                Cv2.GetRectSubPix(crop1, new OpenCvSharp.Size(rect_size.Width, rect_size.Height), rot2.Center, mCrop);

            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message);
            }
            return mCrop;
        }
        public static Mat CropRect(Mat source, RectRotate rot, RectRotate rotMask)
        {
            MatType TypeMat = source.Type();
            Mat matResult = new Mat();

            Point2f pCenter = new Point2f(rot._PosCenter.X, rot._PosCenter.Y);
            Size2f rect_size = new Size2f(rot._rect.Size.Width, rot._rect.Size.Height);
            RotatedRect rot2 = new RotatedRect(pCenter, rect_size, rot._rectRotation);
            double angle = rot._rectRotation;
            if (angle < -45)
            {
                angle += 90.0;

                Swap(ref rect_size.Width, ref rect_size.Height);
            }



            InputArray M = Cv2.GetRotationMatrix2D(rot2.Center, angle, 1.0);

            Mat crop1 = new Mat();
            try
            {
                Mat mCrop = new Mat();
                Cv2.WarpAffine(source, crop1, M, source.Size(), InterpolationFlags.Cubic);

                Cv2.GetRectSubPix(crop1, new OpenCvSharp.Size(rect_size.Width, rect_size.Height), rot2.Center, mCrop);
                //if (TypeMat == MatType.CV_8UC3)
                //{
                //    Cv2.CvtColor(mCrop, mCrop, ColorConversionCodes.BGR2GRAY);
                //}

                if (rot.Shape == ShapeType.Ellipse)
                {
                    Mat matMask = new Mat((int)rot._rect.Height, (int)rot._rect.Width, TypeMat, new Scalar(0));
                    int deltaX = (int)rot._rect.Width / 2;
                    int deltaY = (int)rot._rect.Height / 2;
                    RotatedRect rectElip = new RotatedRect(new Point2f(deltaX, deltaY), new Size2f(rot._rect.Width, rot._rect.Height), rot._rectRotation);
                    Cv2.Ellipse(matMask, rectElip, new Scalar(255), -1);
                    Cv2.BitwiseAnd(mCrop, matMask, matResult);
                }
                else
                    matResult = mCrop;
                //  return matResult;
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message);
            }
            if (rotMask != null)
            {
                Mat matMask = new Mat((int)rot._rect.Height, (int)rot._rect.Width, TypeMat, new Scalar(255));
                int deltaX = (int)rot._rect.Width / 2 - (int)(rot._PosCenter.X - rotMask._PosCenter.X);
                int deltaY = (int)rot._rect.Height / 2 - (int)(rot._PosCenter.Y - rotMask._PosCenter.Y);
                RotatedRect retMask = new RotatedRect(new Point2f(deltaX, deltaY), new Size2f(rotMask._rect.Width, rotMask._rect.Height), rotMask._rectRotation);

                if (rotMask.Shape == ShapeType.Ellipse)
                {
                    Cv2.Ellipse(matMask, retMask, new Scalar(0), -1);
                }
                else
                {
                    // Lấy ra các điểm góc sau khi xoay
                    Point2f[] vertices = new Point2f[4];
                    vertices = retMask.Points();

                    // Chuyển Point2f sang Point để vẽ
                    Point[] pts = Array.ConvertAll(vertices, v => new Point((int)v.X, (int)v.Y));

                    // Tô hình chữ nhật xoay (dùng fillPoly)
                    Cv2.FillPoly(matMask, new[] { pts }, new Scalar(0)); // Đỏ

                }
                //if (matResult.Type() == MatType.CV_8UC3)
                //{
                //    Cv2.CvtColor(matResult, matResult, ColorConversionCodes.BGR2GRAY);

                //}
                Mat matAnd = new Mat();
                Cv2.BitwiseAnd(matResult, matMask, matAnd);

                return matAnd;

            }
            return matResult;
        }

        // = Helpers cho neo/size theo shape =
        private static void GetAnchorSizeFor(RectRotate rr,
            out SD.PointF worldAnchor, out Size2f size, out SD.PointF localCenterForShape)
        {
            if (rr.Shape == ShapeType.Polygon && rr.PolyLocalPoints != null && rr.PolyLocalPoints.Count >= 3)
            {
                GetPolygonBounds(rr.PolyLocalPoints, out float minX, out float minY, out float maxX, out float maxY);
                float w = Math.Max(1f, maxX - minX);
                float h = Math.Max(1f, maxY - minY);
                float cx = (minX + maxX) * 0.5f;
                float cy = (minY + maxY) * 0.5f;

                // Tâm neo world = PosCenter + Rotate((cx,cy), rectRotation)
                SD.PointF delta = RectRotate.Rotate(new SD.PointF(cx, cy), rr._rectRotation);
                worldAnchor = new SD.PointF(rr._PosCenter.X + delta.X, rr._PosCenter.Y + delta.Y);

                size = new Size2f(w, h);
                localCenterForShape = new SD.PointF(cx, cy); // để trừ ra khi vẽ polygon vào patch
            }
            else
            {
                // Non-Polygon: dùng _rect và _PosCenter như cũ
                worldAnchor = rr._PosCenter;
                size = new Size2f(rr._rect.Width, rr._rect.Height);
                localCenterForShape = new SD.PointF(0, 0);
            }
        }

        private static void GetPolygonBounds(IList<SD.PointF> pts, out float minX, out float minY, out float maxX, out float maxY)
        {
            minX = float.MaxValue; minY = float.MaxValue;
            maxX = float.MinValue; maxY = float.MinValue;
            int n = pts.Count;
            int max = n;
            if (n >= 2 && pts[0].Equals(pts[n - 1])) max = n - 1; // nếu đã đóng, bỏ điểm cuối

            for (int i = 0; i < max; i++)
            {
                var p = pts[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            if (max == 0) { minX = minY = 0; maxX = maxY = 1; }
        }

        // = Vẽ mask vào patch (có hỗ trợ tâm local riêng cho polygon) =
        private static void DrawShapeMaskIntoWithSize(
            RectRotate rr, Mat mask, Point2f centerInMask, float angleInPatch, byte fillValue,
            int W, int H, SD.PointF localCenterForShape)
        {
            if (W <= 0 || H <= 0) return;

            switch (rr.Shape)
            {
                case ShapeType.Rectangle:
                    if (Math.Abs(angleInPatch) < 1e-6f)
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Cv2.Rectangle(mask, new Rect(tlx, tly, W, H), new Scalar(fillValue), -1);
                    }
                    else
                    {
                        var ptsRect = GetAxisAlignedRectCorners(centerInMask, W, H);
                        for (int i = 0; i < ptsRect.Length; i++)
                            ptsRect[i] = RotateAround(ptsRect[i], centerInMask, angleInPatch);
                        Cv2.FillPoly(mask, new[] { ptsRect }, new Scalar(fillValue));
                    }
                    break;

                case ShapeType.Ellipse:
                    {
                        var rrBox = new RotatedRect(centerInMask, new Size2f(W, H), angleInPatch);
                        Cv2.Ellipse(mask, rrBox, new Scalar(fillValue), -1);
                        break;
                    }

                case ShapeType.Hexagon:
                    {
                        // Hex đã nằm quanh gốc (0,0) theo _rect, không cần trừ localCenter
                        var mi = rr.GetType().GetMethod("GetHexagonVerticesLocal");
                        SD.PointF[] pf = mi != null ? (SD.PointF[])mi.Invoke(rr, null) : null;
                        if (pf != null && pf.Length >= 3)
                        {
                            var list = new List<OpenCvSharp.Point>(pf.Length);
                            for (int i = 0; i < pf.Length; i++)
                            {
                                var p = new Point2f(pf[i].X, pf[i].Y);
                                if (Math.Abs(angleInPatch) > 1e-6f)
                                    p = RotatePoint(p, angleInPatch);
                                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                                list.Add(new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y)));
                            }
                            Cv2.FillPoly(mask, new[] { list.ToArray() }, new Scalar(fillValue));
                        }
                        break;
                    }

                case ShapeType.Polygon:
                    {
                        var listF = rr.PolyLocalPoints;
                        if (listF != null && listF.Count >= 3)
                        {
                            // Trừ tâm local của polygon để polygon nằm giữa patch
                            float cx = localCenterForShape.X;
                            float cy = localCenterForShape.Y;

                            int n = listF.Count;
                            // Nếu polygon đã đóng, có thể giữ nguyên – FillPoly chấp nhận lặp điểm đầu/cuối
                            var arr = new OpenCvSharp.Point[n];
                            for (int i = 0; i < n; i++)
                            {
                                var p = new Point2f(listF[i].X - cx, listF[i].Y - cy);
                                if (Math.Abs(angleInPatch) > 1e-6f)
                                    p = RotatePoint(p, angleInPatch);
                                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                                arr[i] = new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y));
                            }
                            Cv2.FillPoly(mask, new[] { arr }, new Scalar(fillValue));
                        }
                        break;
                    }

                default:
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Cv2.Rectangle(mask, new Rect(tlx, tly, W, H), new Scalar(fillValue), -1);
                        break;
                    }
            }
        }

        // ===== Helpers gốc =====

        private static Point2f RotatePoint(Point2f p, float degree)
        {
            double rad = degree * Math.PI / 180.0;
            double c = Math.Cos(rad), s = Math.Sin(rad);
            return new Point2f(
                (float)(p.X * c - p.Y * s),
                (float)(p.X * s + p.Y * c)
            );
        }

        private static OpenCvSharp.Point RotateAround(OpenCvSharp.Point p, Point2f center, float degree)
        {
            Point2f pf = new Point2f(p.X - center.X, p.Y - center.Y);
            pf = RotatePoint(pf, degree);
            return new OpenCvSharp.Point(
                (int)Math.Round(pf.X + center.X),
                (int)Math.Round(pf.Y + center.Y)
            );
        }

        private static void DrawShapeMaskInto(
            RectRotate rr, Mat mask, Point2f centerInMask, float angleInPatch, byte fillValue)
        {
            int W = (int)Math.Round(rr._rect.Width);
            int H = (int)Math.Round(rr._rect.Height);
            if (W <= 0 || H <= 0) return;

            switch (rr.Shape)
            {
                case ShapeType.Rectangle:
                    if (Math.Abs(angleInPatch) < 1e-6f)
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Rect r = new Rect(tlx, tly, W, H);
                        Cv2.Rectangle(mask, r, new Scalar(fillValue), thickness: -1);
                    }
                    else
                    {
                        OpenCvSharp.Point[] ptsRect = GetAxisAlignedRectCorners(centerInMask, W, H);
                        for (int i = 0; i < ptsRect.Length; i++)
                            ptsRect[i] = RotateAround(ptsRect[i], centerInMask, angleInPatch);
                        Cv2.FillPoly(mask, new[] { ptsRect }, new Scalar(fillValue));
                    }
                    break;

                case ShapeType.Ellipse:
                    {
                        var rrBox = new RotatedRect(centerInMask, new Size2f(W, H), angleInPatch);
                        Cv2.Ellipse(mask, rrBox, new Scalar(fillValue), -1);
                        break;
                    }

                case ShapeType.Hexagon:
                    {
                        OpenCvSharp.Point[] poly = TryGetPolygonFromHex(rr, centerInMask, angleInPatch, W, H);
                        if (poly != null && poly.Length >= 3)
                            Cv2.FillPoly(mask, new[] { poly }, new Scalar(fillValue));
                        break;
                    }

                case ShapeType.Polygon:
                    {
                        IList<SD.PointF> list = rr.PolyLocalPoints;
                        if (list != null && list.Count >= 3)
                        {
                            OpenCvSharp.Point[] arr = new OpenCvSharp.Point[list.Count];
                            for (int i = 0; i < list.Count; i++)
                            {
                                Point2f p = new Point2f(list[i].X, list[i].Y);
                                if (Math.Abs(angleInPatch) > 1e-6f)
                                    p = RotatePoint(p, angleInPatch);
                                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                                arr[i] = new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y));
                            }
                            Cv2.FillPoly(mask, new[] { arr }, new Scalar(fillValue));
                        }
                        break;
                    }

                default:
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Rect r = new Rect(tlx, tly, W, H);
                        Cv2.Rectangle(mask, r, new Scalar(fillValue), thickness: -1);
                        break;
                    }
            }
        }

        private static OpenCvSharp.Point[] GetAxisAlignedRectCorners(Point2f center, int W, int H)
        {
            float hx = W * 0.5f, hy = H * 0.5f;
            return new[]
            {
                new OpenCvSharp.Point((int)Math.Round(center.X - hx), (int)Math.Round(center.Y - hy)),
                new OpenCvSharp.Point((int)Math.Round(center.X + hx), (int)Math.Round(center.Y - hy)),
                new OpenCvSharp.Point((int)Math.Round(center.X + hx), (int)Math.Round(center.Y + hy)),
                new OpenCvSharp.Point((int)Math.Round(center.X - hx), (int)Math.Round(center.Y + hy)),
            };
        }

        private static OpenCvSharp.Point[] TryGetPolygonFromHex(RectRotate rr, Point2f centerInMask, float angleInPatch, int W, int H)
        {
            try
            {
                var mi = rr.GetType().GetMethod("GetHexagonVerticesLocal");
                if (mi != null)
                {
                    object ptsObj = mi.Invoke(rr, null);
                    var pf = ptsObj as SD.PointF[];
                    if (pf != null && pf.Length >= 3)
                    {
                        var list = new List<OpenCvSharp.Point>(pf.Length);
                        for (int i = 0; i < pf.Length; i++)
                        {
                            Point2f p = new Point2f(pf[i].X, pf[i].Y);
                            if (Math.Abs(angleInPatch) > 1e-6f)
                                p = RotatePoint(p, angleInPatch);
                            p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                            list.Add(new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y)));
                        }
                        return list.ToArray();
                    }
                }
            }
            catch { }

            // hex đều nội tiếp vào W×H (fallback)
            var list2 = new List<OpenCvSharp.Point>(6);
            for (int i = 0; i < 6; i++)
            {
                double theta = (Math.PI / 3.0) * i; // 0,60,120,...
                Point2f p = new Point2f(
                    (float)((W * 0.5) * Math.Cos(theta)),
                    (float)((H * 0.5) * Math.Sin(theta))
                );
                if (Math.Abs(angleInPatch) > 1e-6f)
                    p = RotatePoint(p, angleInPatch);
                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                list2.Add(new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y)));
            }
            return list2.ToArray();
        }
    }
}
