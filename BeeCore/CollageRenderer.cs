using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace BeeCore
{
    public enum FillMode { Cover, Contain }

    public sealed class CollageRenderer : IDisposable
    {
        // Public vì BuildCollageBitmap dùng đến (tránh Inconsistent accessibility)
        public sealed class ImageItem
        {
            public Bitmap Bmp { get; }
            public FillMode Mode { get; }
            public float Weight { get; } // tỉ lệ diện tích mong muốn (>=0)
            public ImageItem(Bitmap bmp, FillMode mode, float weight)
            {
                Bmp = bmp; Mode = mode; Weight = Math.Max(0f, weight);
            }
        }

        private readonly Cyotek.Windows.Forms.ImageBox _pb;
        private readonly int _gutter;
        private readonly Color _bg;
        private readonly bool _autoRerenderOnResize;
        private readonly List<ImageItem> _items = new List<ImageItem>();

        public CollageRenderer(
            Cyotek.Windows.Forms.ImageBox pictureBox,
            int gutter = 6,
            Color? background = null,
            bool autoRerenderOnResize = true)
        {
            _pb = pictureBox ?? throw new ArgumentNullException(nameof(pictureBox));
            _gutter = Math.Max(0, gutter);
            _bg = background ?? Color.Black;
            _autoRerenderOnResize = autoRerenderOnResize;

            if (_autoRerenderOnResize) _pb.Resize += OnPictureBoxResize;
        }

        /// Thêm 1 ảnh từ Bitmap. mode: Cover/Contain, weight: tỉ lệ diện tích (>=0).
        public void AddImage(Bitmap bmp, FillMode mode = FillMode.Cover, float weight = 1f)
        {
            if (bmp != null) _items.Add(new ImageItem(bmp, mode, weight));
        }

        /// Thêm 1 ảnh từ file (không khóa file). Có thể chuẩn hoá size & set weight.
        public void AddImage(string path, FillMode mode = FillMode.Cover, float weight = 1f,
                             bool normalize = true, int longestSideLimit = 2048)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            using (var img = Image.FromFile(path))
            {
                Bitmap bmp = new Bitmap(img);
                if (normalize && longestSideLimit > 0)
                    bmp = NormalizeLongestSide(bmp, longestSideLimit, disposeInput: true);
                _items.Add(new ImageItem(bmp, mode, weight));
            }
        }

        /// Xoá danh sách ảnh (không dispose ảnh nguồn).
        public void ClearImages() => _items.Clear();
        public Size szImage=new Size(0, 0);
        public void Render()
        {
            if (_pb.ClientSize.Width <= 0 || _pb.ClientSize.Height <= 0) return;

            // 1) Build duy nhất 1 bitmap để hiển thị
            var newBitmap = BuildCollageBitmap(_items, _pb.ClientSize, _gutter, _bg);
            szImage = newBitmap.Size;

            // 2) Clone ra bmResult (dành để lưu file sau này)
            //    Dùng Clone(Rect, PixelFormat) để đảm bảo deep copy
            Bitmap cloneForSave = newBitmap.Clone(
                new Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
                newBitmap.PixelFormat);

            // 3) Swap vào BeeCore.Common.bmResult và dispose bản cũ
            var oldResult = BeeCore.Common.bmResult;
            BeeCore.Common.bmResult = cloneForSave;
            oldResult?.Dispose();

            // (Nếu bạn cần thread-safe tuyệt đối cho static field, dùng Interlocked.Exchange)
            // var oldResult = Interlocked.Exchange(ref BeeCore.Common.bmResult, cloneForSave);
            // oldResult?.Dispose();

            // 4) Cập nhật PictureBox và dispose ảnh cũ của PictureBox
            var old = _pb.Image;
            _pb.Image = newBitmap;
            old?.Dispose();
        }
        /// Render ảnh ghép theo kích thước PictureBox hiện tại.
        //public void Render()
        //{
        //    if (_pb.ClientSize.Width <= 0 || _pb.ClientSize.Height <= 0) return;
        //    var newBitmap = BuildCollageBitmap(_items, _pb.ClientSize, _gutter, _bg);
        //    szImage = newBitmap.Size;
        //    BeeCore.Common.bmResult = BuildCollageBitmap(_items, _pb.ClientSize, _gutter, _bg);
        //    var old = _pb.Image; _pb.Image = newBitmap; old?.Dispose();
        //}

        /// API static: dựng và trả về Bitmap ghép (không gán PictureBox).
        public static Bitmap BuildCollageBitmap(
            IList<ImageItem> items, Size targetSize, int gutter = 6, Color? bg = null)
        {
            int w = Math.Max(1, targetSize.Width);
            int h = Math.Max(1, targetSize.Height);
            var outBmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            outBmp.SetResolution(96, 96);

            using (var g = Graphics.FromImage(outBmp))
            {
                g.Clear(bg ?? Color.Black);
                if (items == null || items.Count == 0) return outBmp;

                int n = Math.Min(items.Count, 4);
                var dstRect = new Rectangle(0, 0, w, h);
                var cells = ComputeCellsWeighted(dstRect, items, n, Math.Max(0, gutter));

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.None;
                g.CompositingQuality = CompositingQuality.AssumeLinear;

                for (int i = 0; i < n; i++)
                {
                    var it = items[i];
                    if (it?.Bmp == null) continue;
                    if (it.Mode == FillMode.Cover) DrawImageCover(g, it.Bmp, cells[i]);
                    else DrawImageContain(g, it.Bmp, cells[i]);
                }
            }
            return outBmp;
        }

        // ==================== Layout có WEIGHT ====================

        private static List<Rectangle> ComputeCellsWeighted(
            Rectangle dst, IList<ImageItem> items, int n, int gutter)
        {
            var res = new List<Rectangle>(n);
            double aspect = (double)dst.Width / Math.Max(1, dst.Height);
            bool wide = aspect >= 1.0;

            // Chuẩn hoá weight
            float sum = 0f;
            for (int i = 0; i < n; i++) sum += Math.Max(0f, items[i].Weight);
            if (sum <= 0f) sum = n; // tất cả = 0 -> xem như 1 mỗi ảnh

            Rectangle Shrink(Rectangle r, int pad)
                => new Rectangle(r.X + pad, r.Y + pad,
                                 Math.Max(0, r.Width - 2 * pad),
                                 Math.Max(0, r.Height - 2 * pad));

            if (n == 1)
            {
                res.Add(Shrink(dst, gutter));
            }
            else if (n == 2)
            {
                float w0 = Math.Max(0f, items[0].Weight) / sum;
                float w1 = 1f - w0;

                if (wide)
                {
                    int leftW = Math.Max(1, (int)Math.Round((dst.Width - gutter) * w0));
                    leftW = Math.Min(leftW, dst.Width - gutter - 1);
                    var left = new Rectangle(dst.X, dst.Y, leftW, dst.Height);
                    var right = new Rectangle(dst.X + leftW + gutter, dst.Y, dst.Width - leftW - gutter, dst.Height);
                    res.Add(Shrink(left, gutter));
                    res.Add(Shrink(right, gutter));
                }
                else
                {
                    int topH = Math.Max(1, (int)Math.Round((dst.Height - gutter) * w0));
                    topH = Math.Min(topH, dst.Height - gutter - 1);
                    var top = new Rectangle(dst.X, dst.Y, dst.Width, topH);
                    var bottom = new Rectangle(dst.X, dst.Y + topH + gutter, dst.Width, dst.Height - topH - gutter);
                    res.Add(Shrink(top, gutter));
                    res.Add(Shrink(bottom, gutter));
                }
            }
            else if (n == 3)
            {
                float w0 = Math.Max(0f, items[0].Weight) / sum;
                float w1 = Math.Max(0f, items[1].Weight) / sum;
                float w2 = 1f - w0 - w1;

                if (wide)
                {
                    // Cột trái cho ảnh 0 theo tỉ lệ w0; cột phải chia theo w1:w2 (theo chiều dọc)
                    int leftW = Math.Max(1, (int)Math.Round((dst.Width - gutter) * w0));
                    leftW = Math.Min(leftW, dst.Width - gutter - 1);

                    var left = new Rectangle(dst.X, dst.Y, leftW, dst.Height);
                    int rightW = dst.Width - leftW - gutter;

                    float s = w1 + w2; if (s <= 0f) s = 1f;
                    int topH = Math.Max(1, (int)Math.Round((dst.Height - gutter) * (w1 / s)));
                    topH = Math.Min(topH, dst.Height - gutter - 1);

                    var topR = new Rectangle(dst.X + leftW + gutter, dst.Y, rightW, topH);
                    var botR = new Rectangle(dst.X + leftW + gutter, dst.Y + topH + gutter, rightW, dst.Height - topH - gutter);

                    res.Add(Shrink(left, gutter));   // img0
                    res.Add(Shrink(topR, gutter));   // img1
                    res.Add(Shrink(botR, gutter));   // img2
                }
                else
                {
                    // Hàng trên cho ảnh 0+1 theo tỉ lệ (w0+w1); trong hàng trên chia trái/phải theo w0:w1
                    int topH = Math.Max(1, (int)Math.Round((dst.Height - gutter) * (w0 + w1)));
                    topH = Math.Min(topH, dst.Height - gutter - 1);

                    var top = new Rectangle(dst.X, dst.Y, dst.Width, topH);
                    var bottom = new Rectangle(dst.X, dst.Y + topH + gutter, dst.Width, dst.Height - topH - gutter);

                    float s = w0 + w1; if (s <= 0f) s = 1f;
                    int leftW = Math.Max(1, (int)Math.Round((top.Width - gutter) * (w0 / s)));
                    leftW = Math.Min(leftW, top.Width - gutter - 1);

                    var topL = new Rectangle(top.X, top.Y, leftW, top.Height);
                    var topR = new Rectangle(top.X + leftW + gutter, top.Y, top.Width - leftW - gutter, top.Height);

                    res.Add(Shrink(topL, gutter));   // img0
                    res.Add(Shrink(topR, gutter));   // img1
                    res.Add(Shrink(bottom, gutter)); // img2
                }
            }
            else // n >= 4 -> 2x2, tỉ lệ theo tổng weight hàng/cột
            {
                float w0 = Math.Max(0f, items[0].Weight);
                float w1 = Math.Max(0f, items[1].Weight);
                float w2 = Math.Max(0f, items[2].Weight);
                float w3 = Math.Max(0f, items[3].Weight);
                float total = w0 + w1 + w2 + w3; if (total <= 0f) total = 4f;

                // cột trái = (w0 + w2) / total; cột phải = (w1 + w3) / total
                float colLRatio = (w0 + w2) / total;
                int colLW = Math.Max(1, (int)Math.Round((dst.Width - gutter) * colLRatio));
                colLW = Math.Min(colLW, dst.Width - gutter - 1);

                // hàng trên = (w0 + w1) / total; hàng dưới = (w2 + w3) / total
                float rowTopRatio = (w0 + w1) / total;
                int rowTopH = Math.Max(1, (int)Math.Round((dst.Height - gutter) * rowTopRatio));
                rowTopH = Math.Min(rowTopH, dst.Height - gutter - 1);

                var r00 = new Rectangle(dst.X, dst.Y, colLW, rowTopH);
                var r01 = new Rectangle(dst.X + colLW + gutter, dst.Y, dst.Width - colLW - gutter, rowTopH);
                var r10 = new Rectangle(dst.X, dst.Y + rowTopH + gutter, colLW, dst.Height - rowTopH - gutter);
                var r11 = new Rectangle(dst.X + colLW + gutter, dst.Y + rowTopH + gutter, dst.Width - colLW - gutter, dst.Height - rowTopH - gutter);

                res.Add(Shrink(r00, gutter)); // 0
                res.Add(Shrink(r01, gutter)); // 1
                res.Add(Shrink(r10, gutter)); // 2
                res.Add(Shrink(r11, gutter)); // 3
            }

            return res;
        }

        // ==================== Vẽ ảnh ====================

        private static void DrawImageCover(Graphics g, Bitmap bmp, Rectangle dst)
        {
            if (bmp == null || dst.Width <= 0 || dst.Height <= 0) return;

            float scaleX = (float)dst.Width / bmp.Width;
            float scaleY = (float)dst.Height / bmp.Height;
            float scale = Math.Max(scaleX, scaleY);

            int cropW = (int)Math.Round(dst.Width / scale);
            int cropH = (int)Math.Round(dst.Height / scale);
            int cropX = (bmp.Width - cropW) / 2;
            int cropY = (bmp.Height - cropH) / 2;

            cropX = Math.Max(0, Math.Min(cropX, Math.Max(0, bmp.Width - cropW)));
            cropY = Math.Max(0, Math.Min(cropY, Math.Max(0, bmp.Height - cropH)));

            var srcRect = new Rectangle(cropX, cropY, cropW, cropH);
            g.DrawImage(bmp, dst, srcRect, GraphicsUnit.Pixel);
        }

        private static void DrawImageContain(Graphics g, Bitmap bmp, Rectangle dst)
        {
            if (bmp == null || dst.Width <= 0 || dst.Height <= 0) return;

            float sx = (float)dst.Width / bmp.Width;
            float sy = (float)dst.Height / bmp.Height;
            float scale = Math.Min(sx, sy);

            int nw = Math.Max(1, (int)Math.Round(bmp.Width * scale));
            int nh = Math.Max(1, (int)Math.Round(bmp.Height * scale));

            int x = dst.X + (dst.Width - nw) / 2;
            int y = dst.Y + (dst.Height - nh) / 2;

            g.DrawImage(bmp, new Rectangle(x, y, nw, nh),
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        GraphicsUnit.Pixel);
        }

        // ==================== Utils ====================

        private static Bitmap NormalizeLongestSide(Bitmap src, int longest, bool disposeInput)
        {
            int w = src.Width, h = src.Height;
            int maxSide = Math.Max(w, h);
            if (maxSide <= longest) return src;

            float scale = (float)longest / maxSide;
            int nw = Math.Max(1, (int)Math.Round(w * scale));
            int nh = Math.Max(1, (int)Math.Round(h * scale));

            var dst = new Bitmap(nw, nh, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            dst.SetResolution(96, 96);
            using (var g = Graphics.FromImage(dst))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.None;
                g.DrawImage(src, new Rectangle(0, 0, nw, nh));
            }
            if (disposeInput) src.Dispose();
            return dst;
        }

        private void OnPictureBoxResize(object sender, EventArgs e) => Render();

        public void Dispose()
        {
            if (_autoRerenderOnResize) _pb.Resize -= OnPictureBoxResize;
            _pb.Image?.Dispose();
            _pb.Image = null;
        }
    }
}
