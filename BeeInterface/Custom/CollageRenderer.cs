using BeeGlobal;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BeeInterface
{

    public enum CollageLayout
    {
        AutoWeighted = 0, // theo weight
        One,              // 1 ảnh full
        Two,              // 2 ảnh chia đều (ngang/dọc tùy aspect)
        ThreeRow,         // 1 hàng 3 cột
        FourGrid          // 2x2 chia đều
    }

    public sealed class CollageRenderer : IDisposable
    {
        // Public để BuildCollageBitmap dùng và tránh inconsistent accessibility
        public sealed class ImageItem
        {
            public Bitmap Bmp { get; }
            public FillMode1 Mode { get; }
            public float Weight { get; }
            public bool Owned { get; } // renderer sở hữu và có quyền dispose

            public ImageItem(Bitmap bmp, FillMode1 mode, float weight, bool owned = false)
            {
                Bmp = bmp;
                Mode = mode;
                Weight = Math.Max(0f, weight);
                Owned = owned;
            }
        }

        private readonly Cyotek.Windows.Forms.ImageBox _pb;
        private readonly int _gutter;
        private readonly Color _bg;
        private readonly bool _autoRerenderOnResize;
        private readonly List<ImageItem> _items = new List<ImageItem>();

        private CollageLayout _layoutPreset = CollageLayout.AutoWeighted;
        public CollageLayout LayoutPreset
        {
            get => _layoutPreset;
            set => _layoutPreset = value;
        }

        /// <summary>
        /// Khi Render, có dispose ảnh KẾT QUẢ cũ (PictureBox + bmResult) không.
        /// Ảnh đầu vào trong _items KHÔNG bị dispose ở Render dù cờ này là true.
        /// </summary>
        public bool DisposeOnSwap { get; set; } = true;

        // Lưu index ảnh vừa Modify để highlight
        private int _lastModifiedIndex = -1;

        public CollageRenderer(
            Cyotek.Windows.Forms.ImageBox pictureBox,
            int gutter = 6,
            Color? background = null,
            bool autoRerenderOnResize = false)
        {
            _pb = pictureBox ?? throw new ArgumentNullException(nameof(pictureBox));
            _gutter = Math.Max(0, gutter);
            _bg = background ?? Color.Black;
            _autoRerenderOnResize = autoRerenderOnResize;

          //  if (_autoRerenderOnResize) _pb.Resize += OnPictureBoxResize;
        }

        public int Count() => _items.Count;

        public Size szImage = new Size(0, 0);

        // ==================== Public API: Add/Modify/Remove/Clear ====================

        public void AddImage(Bitmap bmp, FillMode1 mode = FillMode1.Cover, float weight = 1f)
        {
            if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0) return;
            var owned = DeepCopyBitmap(bmp);
            if (owned == null) return;
            _items.Add(new ImageItem(owned, mode, weight, owned: true));
            _lastModifiedIndex = _items.Count - 1;
            Render();
        }

        public void AddImage(Bitmap bmp, FillMode1 mode, float weight, CollageLayout layout)
        {
            _layoutPreset = layout;
            AddImage(bmp, mode, weight);
         
        }

        public void AddImage(string path, FillMode1 mode = FillMode1.Cover, float weight = 1f,
                             bool normalize = true, int longestSideLimit = 2048)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            using (var img = Image.FromFile(path))
            {
                Bitmap bmp = new Bitmap(img);
                if (normalize && longestSideLimit > 0)
                    bmp = NormalizeLongestSide(bmp, longestSideLimit, disposeInput: true);
                if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0) return;
                _items.Add(new ImageItem(bmp, mode, weight, owned: true));
               
            }
        }

        public void ModifyImage(int index, Bitmap bmp, FillMode1 mode = FillMode1.Cover, float weight = 1f)
        {
            if (bmp == null || index < 0 || index >= _items.Count) return;
            if (bmp.Width <= 0 || bmp.Height <= 0) return;

            var newOwned = DeepCopyBitmap(bmp);
            if (newOwned == null) return;

            var old = _items[index];
            if (old?.Owned == true) old.Bmp?.Dispose();

            _items[index] = new ImageItem(newOwned, mode, weight, owned: true);
            _lastModifiedIndex = index;
            Render();
        }

        public void ModifyImage(int index, string path, FillMode1 mode = FillMode1.Cover, float weight = 1f,
                                bool normalize = true, int longestSideLimit = 2048)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            if (index < 0 || index >= _items.Count) return;

            using (var img = Image.FromFile(path))
            {
                Bitmap bmp = new Bitmap(img);
                if (normalize && longestSideLimit > 0)
                    bmp = NormalizeLongestSide(bmp, longestSideLimit, disposeInput: true);
                if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0) return;

                var old = _items[index];
                if (old?.Owned == true) old.Bmp?.Dispose();

                _items[index] = new ImageItem(bmp, mode, weight, owned: true);
            }
            _lastModifiedIndex = index;
            Render();
        }

        public void RemoveImage(int index)
        {
            if (index < 0 || index >= _items.Count) return;
            var old = _items[index];
            if (old?.Owned == true) old.Bmp?.Dispose();
            _items.RemoveAt(index);
            if (_lastModifiedIndex == index) _lastModifiedIndex = -1;
        }

        public void ClearImages()
        {
            foreach (var it in _items)
                if (it?.Owned == true) it.Bmp?.Dispose();
            _items.Clear();
            _lastModifiedIndex = -1;
        }

        // ==================== Render ====================

        public void Render()
        {
            if (_pb.ClientSize.Width <= 0 || _pb.ClientSize.Height <= 0) return;

            bool drawPlaceholders = (_layoutPreset != CollageLayout.AutoWeighted);

            var newBitmap = BuildCollageBitmap(
                _items, _pb.ClientSize, _gutter, _bg, _layoutPreset,
                highlightIndex: _lastModifiedIndex,
                drawPlaceholders: drawPlaceholders,
                placeholderText: "Empty"
            );

            szImage = newBitmap.Size;

            var cloneForSave = newBitmap.Clone(
                new Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
                newBitmap.PixelFormat);

            //var oldResult = BeeCore.Common.bmResult;
            //BeeCore.Common.bmResult = cloneForSave;
            //if (DisposeOnSwap) oldResult?.Dispose();

            var old = _pb.Image;
            _pb.Image = newBitmap;
            if (DisposeOnSwap) old?.Dispose();
        }

        // ==================== Build & Layout ====================

        public static Bitmap BuildCollageBitmap(
         IList<ImageItem> items, Size targetSize, int gutter = 6, Color? bg = null,
         CollageLayout preset = CollageLayout.AutoWeighted,
         int highlightIndex = -1,
         bool drawPlaceholders = false,
         string placeholderText = "Empty",
         Color? placeholderBack = null,
         Color? placeholderBorder = null,
         Color? placeholderTextColor = null)
        {
            int w = Math.Max(1, targetSize.Width);
            int h = Math.Max(1, targetSize.Height);
            var outBmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            outBmp.SetResolution(96, 96);

            using (var g = Graphics.FromImage(outBmp))
            {
                g.Clear(bg ?? Color.Black);
                int itemCount = items?.Count ?? 0;

                // Số ô theo preset
                int presetN = (preset == CollageLayout.One ? 1 :
                              (preset == CollageLayout.Two ? 2 :
                              (preset == CollageLayout.ThreeRow ? 3 : 4)));

                // Quan trọng: với layout cố định, LUÔN tạo đủ số ô preset
                int n = (preset == CollageLayout.AutoWeighted) ? Math.Min(itemCount, 4) : presetN;
                if (n <= 0) return outBmp;

                var dstRect = new Rectangle(0, 0, w, h);
                List<Rectangle> cells =
                    (preset == CollageLayout.AutoWeighted)
                    ? ComputeCellsWeighted(dstRect, items, n, Math.Max(0, gutter))
                    : ComputeCellsPreset(dstRect, n, Math.Max(0, gutter), preset);

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.None;
                g.CompositingQuality = CompositingQuality.AssumeLinear;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                var phBack = placeholderBack ?? Color.White;
                var phBorder = placeholderBorder ?? Color.LightGray;
                var phText = placeholderTextColor ?? Color.Gray;

                // helper: căn giữa một rect kích thước 'sz' vào cell
                Rectangle CenterRect(Rectangle cell, Size sz)
                {
                    int rw = Math.Min(sz.Width, cell.Width);
                    int rh = Math.Min(sz.Height, cell.Height);
                    int rx = cell.X + (cell.Width - rw) / 2;
                    int ry = cell.Y + (cell.Height - rh) / 2;
                    return new Rectangle(rx, ry, Math.Max(1, rw), Math.Max(1, rh));
                }

                Rectangle refDrawnRect = Rectangle.Empty; // vùng vẽ của item[0] (nếu có), dùng scale placeholder cho trường hợp chỉ có 1 ảnh

                for (int i = 0; i < cells.Count; i++)
                {
                    Rectangle drawnRect = Rectangle.Empty;

                    if (i < itemCount && items[i]?.Bmp != null &&
                        items[i].Bmp.Width > 0 && items[i].Bmp.Height > 0)
                    {
                        var it = items[i];
                        drawnRect = (it.Mode == FillMode1.Cover)
                            ? DrawImageCover(g, it.Bmp, cells[i])
                            : DrawImageContain(g, it.Bmp, cells[i]);

                        // Nếu chỉ có 1 ảnh, lưu kích thước đã vẽ để dùng cho placeholder
                        if (itemCount == 1 && i == 0)
                            refDrawnRect = drawnRect;
                    }
                    else
                    {
                        // Ô trống → placeholder, vẫn vẽ như trước
                        if (drawPlaceholders && preset != CollageLayout.AutoWeighted)
                        {
                            Rectangle phRect = (itemCount == 1 && !refDrawnRect.IsEmpty)
                                ? CenterRect(cells[i], refDrawnRect.Size)
                                : cells[i];

                            using (var br = new SolidBrush(phBack)) g.FillRectangle(br, phRect);
                            using (var pen = new Pen(phBorder, 1f)) g.DrawRectangle(pen, phRect);

                            float fs = Math.Max(10f, Math.Min(phRect.Width, phRect.Height) * 0.18f);
                            using (var f = new Font("Segoe UI", fs, FontStyle.Bold, GraphicsUnit.Pixel))
                            using (var brText = new SolidBrush(phText))
                            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                                g.DrawString(placeholderText, f, brText, phRect, sf);

                            drawnRect = phRect; // highlight sẽ ôm sát placeholder
                        }
                        else
                        {
                            drawnRect = cells[i];
                        }
                    }

                    //// ✅ Luôn vẽ VIỀN MỎNG cho MỌI Ô (kể cả đủ ảnh hay không)
                    //using (var penCell = new Pen(Color.FromArgb(180, 180, 180), 1f))
                    //{
                    //    var border = cells[i];
                    //    border.Width = Math.Max(1, border.Width - 1);
                    //    border.Height = Math.Max(1, border.Height - 1);
                    //    g.DrawRectangle(penCell, border);
                    //}

                    // 🔷 Viền HIGHLIGHT ôm sát vùng đã vẽ (ảnh/placeholder) nếu trúng highlightIndex
                    if (i == highlightIndex && !drawnRect.IsEmpty)
                    {
                        using (var pen = new Pen(Color.FromArgb(246, 204, 120), 4f)) // hoặc Color.Blue
                        {
                            var r = drawnRect;
                            r.Width = Math.Max(1, r.Width - 1);
                            r.Height = Math.Max(1, r.Height - 1);
                            g.DrawRectangle(pen, r);
                        }
                    }
                }
            }
            return outBmp;
        }



        // ----- Layout preset (chia đều) -----
        private static List<Rectangle> ComputeCellsPreset(
            Rectangle dst, int n, int gutter, CollageLayout preset)
        {
            var res = new List<Rectangle>(n);

            Rectangle Shrink(Rectangle r, int pad)
                => new Rectangle(r.X + pad, r.Y + pad,
                                 Math.Max(0, r.Width - 2 * pad),
                                 Math.Max(0, r.Height - 2 * pad));

            double aspect = (double)dst.Width / Math.Max(1, dst.Height);
            bool wide = aspect >= 1.0;

            if (preset == CollageLayout.One || n == 1)
            {
                res.Add(Shrink(dst, gutter));
                return res;
            }

            if (preset == CollageLayout.Two || n == 2)
            {
                if (wide)
                {
                    int leftW = (dst.Width - gutter) / 2;
                    var left = new Rectangle(dst.X, dst.Y, leftW, dst.Height);
                    var right = new Rectangle(dst.X + leftW + gutter, dst.Y, dst.Width - leftW - gutter, dst.Height);
                    res.Add(Shrink(left, gutter));
                    res.Add(Shrink(right, gutter));
                }
                else
                {
                    int topH = (dst.Height - gutter) / 2;
                    var top = new Rectangle(dst.X, dst.Y, dst.Width, topH);
                    var bottom = new Rectangle(dst.X, dst.Y + topH + gutter, dst.Width, dst.Height - topH - gutter);
                    res.Add(Shrink(top, gutter));
                    res.Add(Shrink(bottom, gutter));
                }
                return res;
            }

            if (preset == CollageLayout.ThreeRow || n == 3)
            {
                // Chia 3 cột chắc chắn ≥ 1px, không Shrink đúp gây mất cột
                int innerG = Math.Max(0, gutter);
                int wAvail = dst.Width - (2 * innerG);

                int w0 = Math.Max(1, (int)Math.Floor(wAvail / 3.0));
                int w1 = Math.Max(1, (int)Math.Floor(wAvail / 3.0));
                int w2 = Math.Max(1, wAvail - w0 - w1);

                int x0 = dst.X;
                int x1 = x0 + w0 + innerG;
                int x2 = x1 + w1 + innerG;

                var r0 = new Rectangle(x0, dst.Y, w0, dst.Height);
                var r1 = new Rectangle(x1, dst.Y, w1, dst.Height);
                var r2 = new Rectangle(x2, dst.Y, Math.Max(1, dst.Right - x2), dst.Height);

                res.Add(r0);
                res.Add(r1);
                res.Add(r2);
                return res;
            }

            // FourGrid hoặc n >= 4 -> 2x2
            {
                int colLW = (dst.Width - gutter) / 2;
                int rowTopH = (dst.Height - gutter) / 2;

                var r00 = new Rectangle(dst.X, dst.Y, colLW, rowTopH);
                var r01 = new Rectangle(dst.X + colLW + gutter, dst.Y, dst.Width - colLW - gutter, rowTopH);
                var r10 = new Rectangle(dst.X, dst.Y + rowTopH + gutter, colLW, dst.Height - rowTopH - gutter);
                var r11 = new Rectangle(dst.X + colLW + gutter, dst.Y + rowTopH + gutter, dst.Width - colLW - gutter, dst.Height - rowTopH - gutter);

                res.Add(Shrink(r00, gutter));
                res.Add(Shrink(r01, gutter));
                res.Add(Shrink(r10, gutter));
                res.Add(Shrink(r11, gutter));
                return res;
            }
        }

        // ----- Layout theo weight (đã an toàn khi itemCount==0) -----
        private static List<Rectangle> ComputeCellsWeighted(
            Rectangle dst, IList<ImageItem> items, int n, int gutter)
        {
            var res = new List<Rectangle>(Math.Max(0, n));
            int itemCount = items?.Count ?? 0;
            n = Math.Max(0, Math.Min(n, Math.Min(itemCount, 4)));
            if (n == 0) return res;

            double aspect = (double)dst.Width / Math.Max(1, dst.Height);
            bool wide = aspect >= 1.0;

            // Chuẩn hoá weight
            float sum = 0f;
            for (int i = 0; i < n; i++) sum += Math.Max(0f, items[i].Weight);
            if (sum <= 0f) sum = n;

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
            else // n >= 4 -> 2x2
            {
                float w0 = Math.Max(0f, items[0].Weight);
                float w1 = Math.Max(0f, items[1].Weight);
                float w2 = Math.Max(0f, items[2].Weight);
                float w3 = Math.Max(0f, items[3].Weight);
                float total = w0 + w1 + w2 + w3; if (total <= 0f) total = 4f;

                float colLRatio = (w0 + w2) / total;
                int colLW = Math.Max(1, (int)Math.Round((dst.Width - gutter) * colLRatio));
                colLW = Math.Min(colLW, dst.Width - gutter - 1);

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

        // ==================== Vẽ ảnh (trả về vùng đã vẽ) ====================

        private static Rectangle DrawImageCover(Graphics g, Bitmap bmp, Rectangle dst)
        {
            if (bmp == null || dst.Width <= 0 || dst.Height <= 0) return Rectangle.Empty;

            float scaleX = (float)dst.Width / bmp.Width;
            float scaleY = (float)dst.Height / bmp.Height;
            float scale = Math.Max(scaleX, scaleY);

            int cropW = Math.Max(1, (int)Math.Round(dst.Width / scale));
            int cropH = Math.Max(1, (int)Math.Round(dst.Height / scale));
            int cropX = (bmp.Width - cropW) / 2;
            int cropY = (bmp.Height - cropH) / 2;

            cropX = Math.Max(0, Math.Min(cropX, Math.Max(0, bmp.Width - cropW)));
            cropY = Math.Max(0, Math.Min(cropY, Math.Max(0, bmp.Height - cropH)));

            var srcRect = new Rectangle(cropX, cropY, cropW, cropH);
            g.DrawImage(bmp, dst, srcRect, GraphicsUnit.Pixel);
            return dst; // cover: viền trùng cell
        }

        private static Rectangle DrawImageContain(Graphics g, Bitmap bmp, Rectangle dst)
        {
            if (bmp == null || dst.Width <= 0 || dst.Height <= 0) return Rectangle.Empty;

            float sx = (float)dst.Width / bmp.Width;
            float sy = (float)dst.Height / bmp.Height;
            float scale = Math.Min(sx, sy);

            int nw = Math.Max(1, (int)Math.Round(bmp.Width * scale));
            int nh = Math.Max(1, (int)Math.Round(bmp.Height * scale));

            int x = dst.X + (dst.Width - nw) / 2;
            int y = dst.Y + (dst.Height - nh) / 2;

            var drawRect = new Rectangle(x, y, nw, nh);
            g.DrawImage(bmp, drawRect, new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            return drawRect; // contain: viền ôm sát ảnh thực vẽ
        }

        // ==================== Utils ====================

        private static Bitmap DeepCopyBitmap(Bitmap src)
        {
            if (src == null || src.Width <= 0 || src.Height <= 0) return null;
            var dst = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            dst.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            using (var g = Graphics.FromImage(dst))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.None;
                g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
            }
            return dst;
        }

        private static Bitmap NormalizeLongestSide(Bitmap src, int longest, bool disposeInput)
        {
            if (src == null) return null;
            if (longest <= 0) return src;

            int w = src.Width, h = src.Height;
            int maxSide = Math.Max(w, h);
            if (maxSide <= longest) return src;

            float scale = (float)longest / maxSide;
            int nw = Math.Max(1, (int)Math.Round(w * scale));
            int nh = Math.Max(1, (int)Math.Round(h * scale));

            var dst = new Bitmap(nw, nh, PixelFormat.Format24bppRgb);
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

       // private void OnPictureBoxResize(object sender, EventArgs e) => Render();

        public void Dispose()
        {
           // if (_autoRerenderOnResize) _pb.Resize -= OnPictureBoxResize;

            if (DisposeOnSwap) _pb.Image?.Dispose();
            _pb.Image = null;

            foreach (var it in _items)
                if (it?.Owned == true) it.Bmp?.Dispose();
            _items.Clear();
        }
    }
}
