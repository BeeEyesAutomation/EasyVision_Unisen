using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BeeCore.Func
{



    public enum HAlign { Left, Center, Right }

    public static class ImageUtils73
    {
        public static Bitmap StitchVerticalWithCaptions(
            IList<Bitmap> bitmaps,
            IList<string> captions = null,
            int space = 5,
            int captionImageGap = 4,
            Font font = null,
            Color? textColor = null,
            Color? background = null,
            HAlign align = HAlign.Left,
            int paddingLeft = 0,
            int paddingRight = 0)
        {
            if (bitmaps == null) throw new ArgumentNullException(nameof(bitmaps));

            var imgs = new List<Bitmap>();
            foreach (var b in bitmaps)
                if (b != null && b.Width > 0 && b.Height > 0) imgs.Add(b);
            if (imgs.Count == 0) throw new ArgumentException("Danh sách bitmap rỗng hoặc không hợp lệ.");

            var useFont = font ?? new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point);
            var brush = new SolidBrush(textColor ?? Color.Black);

            // content width = max width ảnh
            int contentW = 0;
            long sumH = 0;
            foreach (var bm in imgs) { if (bm.Width > contentW) contentW = bm.Width; sumH += bm.Height; }

            // đo chiều cao caption cho từng block
            var capHeights = new int[imgs.Count];
            int totalCaptionH = 0;
            using (var tmp = new Bitmap(1, 1))
            using (var gtmp = Graphics.FromImage(tmp))
            {
                gtmp.PageUnit = GraphicsUnit.Pixel;
                var fmt = new StringFormat(StringFormatFlags.LineLimit);
                fmt.Alignment = ToStringAlignment(align);
                fmt.Trimming = StringTrimming.None;

                for (int i = 0; i < imgs.Count; i++)
                {
                    string cap = (captions != null && i < captions.Count && captions[i] != null) ? captions[i] : string.Empty;
                    if (cap.Length == 0) { capHeights[i] = 0; continue; }

                    SizeF sz = gtmp.MeasureString(cap, useFont, new SizeF(contentW, 10000), fmt);
                    int h = (int)Math.Ceiling(sz.Height) + 2;
                    capHeights[i] = h;
                    totalCaptionH += h;
                }
            }

            int totalW = Math.Max(1, contentW + Math.Max(0, paddingLeft) + Math.Max(0, paddingRight));
            int blockSpaces = Math.Max(0, imgs.Count - 1) * Math.Max(0, space);
            int captionGaps = CountNonZero(capHeights) * Math.Max(0, captionImageGap);
            int totalH = checked((int)(sumH + totalCaptionH + blockSpaces + captionGaps));

            float dpiX = imgs[0].HorizontalResolution > 0 ? imgs[0].HorizontalResolution : 96f;
            float dpiY = imgs[0].VerticalResolution > 0 ? imgs[0].VerticalResolution : 96f;

            var canvas = new Bitmap(totalW, totalH, PixelFormat.Format32bppArgb);
            canvas.SetResolution(dpiX, dpiY);

            using (var g = Graphics.FromImage(canvas))
            {
                g.Clear(background ?? Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PageUnit = GraphicsUnit.Pixel;

                var fmt = new StringFormat(StringFormatFlags.LineLimit);
                fmt.Alignment = ToStringAlignment(align);
                fmt.Trimming = StringTrimming.None;

                int y = 0;
                for (int i = 0; i < imgs.Count; i++)
                {
                    var bmp = imgs[i];

                    // caption (nếu có)
                    int ch = capHeights[i];
                    if (ch > 0)
                    {
                        string cap = (captions != null && i < captions.Count && captions[i] != null) ? captions[i] : string.Empty;
                        var rect = new RectangleF(paddingLeft, y, contentW, ch);
                        g.DrawString(cap, useFont, brush, rect, fmt);
                        y += ch + Math.Max(0, captionImageGap);
                    }

                    // vị trí X theo align
                    int x;
                    if (align == HAlign.Left)
                        x = paddingLeft;
                    else if (align == HAlign.Right)
                        x = totalW - paddingRight - bmp.Width;
                    else
                        x = paddingLeft + (contentW - bmp.Width) / 2;

                    g.DrawImageUnscaled(bmp, x, y);
                    y += bmp.Height;

                    if (i < imgs.Count - 1) y += Math.Max(0, space);
                }
            }

            brush.Dispose();
            if (font == null) useFont.Dispose();

            return canvas;
        }

        private static int CountNonZero(int[] arr)
        {
            int c = 0; for (int i = 0; i < arr.Length; i++) if (arr[i] > 0) c++; return c;
        }

        private static StringAlignment ToStringAlignment(HAlign align)
        {
            switch (align)
            {
                case HAlign.Left: return StringAlignment.Near;
                case HAlign.Right: return StringAlignment.Far;
                default: return StringAlignment.Center;
            }
        }
    }


}
