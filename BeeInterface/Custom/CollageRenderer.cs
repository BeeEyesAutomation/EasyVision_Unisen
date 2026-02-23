using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BeeInterface
{
    public enum CollageLayout
    {
        AutoWeighted = 0,
        One,
        Two,
        ThreeRow,
        FourGrid
    }

    public sealed class CollageRenderer : IDisposable
    {
        // ================= IMAGE ITEM =================
        public sealed class ImageItem
        {
            public Bitmap Bmp;
            public FillMode1 Mode;
            public float Weight;
            public bool Owned;

            public ImageItem(Bitmap bmp, FillMode1 mode, float weight, bool owned)
            {
                Bmp = bmp;
                Mode = mode;
                Weight = Math.Max(0f, weight);
                Owned = owned;
            }
        }

        // ================= FIELDS =================
        private Cyotek.Windows.Forms.ImageBox _pb;
        private int _gutter;
        private Color _bg;
        private List<ImageItem> _items = new List<ImageItem>();

        private int _lastModifiedIndex = -1;

        public bool DisposeOnSwap = true;

        public CollageLayout LayoutPreset = CollageLayout.AutoWeighted;

        // 🔥 NEW
        public LayoutOrientation Orientation = LayoutOrientation.Auto;

        public Size szImage = Size.Empty;

        // ================= CTOR =================
        public CollageRenderer(
            Cyotek.Windows.Forms.ImageBox pictureBox,
            int gutter,
            Color? background)
        {
            _pb = pictureBox;
            _gutter = Math.Max(0, gutter);
            _bg = background.HasValue ? background.Value : Color.Black;
        }

        // ⚠️ overload để KHỚP View.cs cũ
        public CollageRenderer(
            Cyotek.Windows.Forms.ImageBox pictureBox,
            int gutter,
            Color? background,
            bool autoRenderOnResize)
            : this(pictureBox, gutter, background)
        {
            // giữ param cho tương thích – không dùng
        }

        // ================= API =================
        public int Count()
        {
            return _items.Count;
        }

        public void AddImage(Bitmap bmp, FillMode1 mode, float weight=1.0f)
        {
            if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0)
                return;

            Bitmap owned = DeepCopyBitmap(bmp);
            if (owned == null)
                return;

            _items.Add(new ImageItem(owned, mode, weight, true));
            _lastModifiedIndex = _items.Count - 1;
            Render();
        }

        public void ModifyImage(int index, Bitmap bmp, FillMode1 mode, float weight = 1.0f)
        {
            if (index < 0 || index >= _items.Count)
                return;
            if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0)
                return;

            Bitmap owned = DeepCopyBitmap(bmp);
            if (owned == null)
                return;

            if (_items[index].Owned && _items[index].Bmp != null)
                _items[index].Bmp.Dispose();

            _items[index] = new ImageItem(owned, mode, weight, true);
            _lastModifiedIndex = index;
            Render();
        }

        public void ClearImages()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Owned && _items[i].Bmp != null)
                    _items[i].Bmp.Dispose();
            }

            _items.Clear();
            _lastModifiedIndex = -1;
            Render();
        }

        // ================= RENDER =================
        public void Render()
        {
           
            if (_pb == null)
                return;

            int w = _pb.ClientSize.Width;
            int h = _pb.ClientSize.Height;
            if (w <= 0 || h <= 0)
                return;

            Bitmap bmp;

            if (Global.Config.DisplayResolution == DisplayResolution.Full)
            {
                bmp = BuildCollageBitmapAuto(
                    _items,
                    _gutter,
                    _bg,
                    LayoutPreset,
                    Orientation,
                    _lastModifiedIndex,
                    LayoutPreset != CollageLayout.AutoWeighted,
                    "Empty",
                    1
                );
            }
            else
            {
                bmp = BuildCollageBitmap(
                    _items,
                    new Size(w, h),
                    _gutter,
                    _bg,
                    LayoutPreset,
                    Orientation,
                    _lastModifiedIndex,
                    LayoutPreset != CollageLayout.AutoWeighted,
                    "Empty"
                );
            }

            szImage = bmp.Size;

            Image old = _pb.Image;
            _pb.Image = bmp;
            if (DisposeOnSwap && old != null)
                old.Dispose();
        }

        // ================= BUILD =================
        public static Bitmap BuildCollageBitmap(
            IList<ImageItem> items,
            Size targetSize,
            int gutter,
            Color bg,
            CollageLayout preset,
            LayoutOrientation orientation,
            int highlightIndex,
            bool drawPlaceholders,
            string placeholderText)
        {
            int w = Math.Max(1, targetSize.Width);
            int h = Math.Max(1, targetSize.Height);

            Bitmap outBmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            outBmp.SetResolution(96, 96);

            using (Graphics g = Graphics.FromImage(outBmp))
            {
                g.Clear(bg);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                int count = items != null ? items.Count : 0;
                int n;

                if (preset == CollageLayout.AutoWeighted)
                    n = Math.Min(count, 4);
                else if (preset == CollageLayout.One)
                    n = 1;
                else if (preset == CollageLayout.Two)
                    n = 2;
                else if (preset == CollageLayout.ThreeRow)
                    n = 3;
                else
                    n = 4;

                if (n <= 0)
                    return outBmp;

                Rectangle dst = new Rectangle(0, 0, w, h);

                List<Rectangle> cells =
                    preset == CollageLayout.AutoWeighted
                    ? ComputeCellsWeighted(dst, n, gutter, orientation)
                    : ComputeCellsPreset(dst, n, gutter, preset, orientation);

                for (int i = 0; i < cells.Count; i++)
                {
                    Rectangle drawn = Rectangle.Empty;

                    if (i < count && items[i] != null && items[i].Bmp != null)
                    {
                        if (items[i].Mode == FillMode1.Cover)
                            drawn = DrawImageCover(g, items[i].Bmp, cells[i]);
                        else
                            drawn = DrawImageContain(g, items[i].Bmp, cells[i]);
                    }
                    else if (drawPlaceholders)
                    {
                        using (SolidBrush br = new SolidBrush(Color.White))
                        using (Pen pen = new Pen(Color.LightGray))
                        {
                            g.FillRectangle(br, cells[i]);
                            g.DrawRectangle(pen, cells[i]);
                        }
                        drawn = cells[i];
                    }

                    if (i == highlightIndex && !drawn.IsEmpty)
                    {
                        using (Pen pen = new Pen(Color.FromArgb(246, 204, 120), 4f))
                        {
                            g.DrawRectangle(pen, drawn);
                        }
                    }
                }
            }
            return outBmp;
        }

        // ================= AUTO SIZE =================
        public static Bitmap BuildCollageBitmapAuto(
            IList<ImageItem> items,
            int gutter,
            Color bg,
            CollageLayout preset,
            LayoutOrientation orientation,
            int highlightIndex,
            bool drawPlaceholders,
            string placeholderText,
            int downscale)
        {
            int maxW = 1, maxH = 1;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].Bmp != null)
                {
                    maxW = Math.Max(maxW, items[i].Bmp.Width);
                    maxH = Math.Max(maxH, items[i].Bmp.Height);
                }
            }

            int w = maxW;
            int h = maxH;

            if (preset == CollageLayout.Two)
            {
                bool wide = ResolveWide(w, h, orientation);
                if (wide) w = maxW * 2 + gutter;
                else h = maxH * 2 + gutter;
            }
            else if (preset == CollageLayout.ThreeRow)
            {
                w = maxW * 3 + gutter * 2;
            }
            else if (preset == CollageLayout.FourGrid || preset == CollageLayout.AutoWeighted)
            {
                w = maxW * 2 + gutter;
                h = maxH * 2 + gutter;
            }

            if (downscale > 1)
            {
                w /= downscale;
                h /= downscale;
            }

            return BuildCollageBitmap(
                items,
                new Size(Math.Max(1, w), Math.Max(1, h)),
                gutter,
                bg,
                preset,
                orientation,
                highlightIndex,
                drawPlaceholders,
                placeholderText
            );
        }

        // ================= LAYOUT =================
        private static bool ResolveWide(int w, int h, LayoutOrientation o)
        {
            if (o == LayoutOrientation.ForceHorizontal)
                return true;
            if (o == LayoutOrientation.ForceVertical)
                return false;
            return w >= h;
        }

        private static List<Rectangle> ComputeCellsPreset(
            Rectangle dst,
            int n,
            int gutter,
            CollageLayout preset,
            LayoutOrientation orientation)
        {
            List<Rectangle> res = new List<Rectangle>(n);
            bool wide = ResolveWide(dst.Width, dst.Height, orientation);

            if (preset == CollageLayout.One || n == 1)
            {
                res.Add(dst);
                return res;
            }

            if (preset == CollageLayout.Two)
            {
                if (wide)
                {
                    int w = (dst.Width - gutter) / 2;
                    res.Add(new Rectangle(dst.X, dst.Y, w, dst.Height));
                    res.Add(new Rectangle(dst.X + w + gutter, dst.Y, dst.Width - w - gutter, dst.Height));
                }
                else
                {
                    int h = (dst.Height - gutter) / 2;
                    res.Add(new Rectangle(dst.X, dst.Y, dst.Width, h));
                    res.Add(new Rectangle(dst.X, dst.Y + h + gutter, dst.Width, dst.Height - h - gutter));
                }
                return res;
            }

            if (preset == CollageLayout.ThreeRow)
            {
                int w = (dst.Width - gutter * 2) / 3;
                for (int i = 0; i < 3; i++)
                    res.Add(new Rectangle(dst.X + i * (w + gutter), dst.Y, w, dst.Height));
                return res;
            }

            int cw = (dst.Width - gutter) / 2;
            int ch = (dst.Height - gutter) / 2;

            res.Add(new Rectangle(dst.X, dst.Y, cw, ch));
            res.Add(new Rectangle(dst.X + cw + gutter, dst.Y, dst.Width - cw - gutter, ch));
            res.Add(new Rectangle(dst.X, dst.Y + ch + gutter, cw, dst.Height - ch - gutter));
            res.Add(new Rectangle(dst.X + cw + gutter, dst.Y + ch + gutter,
                                  dst.Width - cw - gutter, dst.Height - ch - gutter));
            return res;
        }

        private static List<Rectangle> ComputeCellsWeighted(
            Rectangle dst,
            int n,
            int gutter,
            LayoutOrientation orientation)
        {
            // giữ đơn giản: reuse preset Two theo orientation
            return ComputeCellsPreset(dst, n, gutter, CollageLayout.Two, orientation);
        }

        // ================= DRAW =================
        private static Rectangle DrawImageCover(Graphics g, Bitmap bmp, Rectangle dst)
        {
            float sx = (float)dst.Width / bmp.Width;
            float sy = (float)dst.Height / bmp.Height;
            float scale = Math.Max(sx, sy);

            int cw = (int)(dst.Width / scale);
            int ch = (int)(dst.Height / scale);

            int cx = (bmp.Width - cw) / 2;
            int cy = (bmp.Height - ch) / 2;

            g.DrawImage(bmp, dst, new Rectangle(cx, cy, cw, ch), GraphicsUnit.Pixel);
            return dst;
        }

        private static Rectangle DrawImageContain(Graphics g, Bitmap bmp, Rectangle dst)
        {
            float sx = (float)dst.Width / bmp.Width;
            float sy = (float)dst.Height / bmp.Height;
            float scale = Math.Min(sx, sy);

            int w = (int)(bmp.Width * scale);
            int h = (int)(bmp.Height * scale);

            Rectangle r = new Rectangle(
                dst.X + (dst.Width - w) / 2,
                dst.Y + (dst.Height - h) / 2,
                w, h);

            g.DrawImage(bmp, r);
            return r;
        }

        // ================= UTILS =================
        private static Bitmap DeepCopyBitmap(Bitmap src)
        {
            Bitmap bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(src, 0, 0);
            }
            return bmp;
        }

        public void Dispose()
        {
            if (DisposeOnSwap && _pb.Image != null)
                _pb.Image.Dispose();

            _pb.Image = null;

            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Owned && _items[i].Bmp != null)
                    _items[i].Bmp.Dispose();
            }
            _items.Clear();
        }
    }
}
