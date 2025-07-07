using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeCore
{
    public class Draws
    {
        public static void Plus(Graphics gc, int centerX, int centerY, int lineLength, Color color, int thiness)
        {
            Pen pen = new Pen(color, thiness);
            gc.DrawLine(pen, centerX - lineLength / 2, centerY, centerX + lineLength / 2, centerY);
            gc.DrawLine(pen, centerX, centerY - lineLength / 2, centerX, centerY + lineLength / 2);
        }
        public static void Box1Label(Graphics graphics, RectangleF baseRect, string text, Font font, Brush textBrush, Color backgroundBrush,int thiness=4, bool alignRight = false)
        {
            graphics.DrawRectangle(new Pen(backgroundBrush, thiness), new Rectangle((int)baseRect.X, (int)baseRect.Y, (int)baseRect.Width, (int)baseRect.Height));

            // Đo kích thước vùng text
            SizeF textSize = graphics.MeasureString(text, font);

            // Tính vị trí của rectangle chứa text nằm phía trên baseRect
            int padding =1; // padding giữa text và viền rectangle

            // Tính width/height vùng nền có padding
            int labelWidth = (int)textSize.Width + padding * 2;
            int labelHeight = (int)textSize.Height + padding * 2;

            // Tính toạ độ top-left của labelRect
            int labelX = alignRight
                ? (int)baseRect.Right - labelWidth   // Bám góc phải
                : (int)baseRect.Left;                // Bám góc trái

            int labelY = (int)baseRect.Top - labelHeight; // Nằm phía trên rectangle

            // Nếu vượt ra trên ảnh, bạn có thể kiểm tra và điều chỉnh labelY >= 0 nếu muốn

            Rectangle labelRect = new Rectangle(labelX, labelY, labelWidth, labelHeight);

            // Vẽ nền rectangle
            graphics.FillRectangle(new SolidBrush( backgroundBrush), labelRect);

            // Vẽ text (trong rectangle, có padding)
            graphics.DrawString(text, font, textBrush, labelX + padding, labelY + padding);
        }
        public static void Box2Label(Graphics graphics, RectangleF baseRect, string leftText, string rightText, Font baseFont, Color baseBackColor, Brush textBrush, int opacity = 128,int thiness=4, int minFontSize = 10, int padding = 1)
        {
            graphics.DrawRectangle(new Pen(baseBackColor, thiness), new Rectangle((int)baseRect.X, (int)baseRect.Y, (int)baseRect.Width, (int)baseRect.Height));

            float fontSize = baseFont.Size;
       
            Font currentFont;
            SizeF leftSize, rightSize;
            int totalTextWidth;

            // Tìm font size phù hợp
            do
            {
                currentFont = new Font(baseFont.FontFamily, fontSize, baseFont.Style);
                leftSize = graphics.MeasureString(leftText, currentFont);
                rightSize = graphics.MeasureString(rightText, currentFont);
                totalTextWidth = (int)(leftSize.Width + rightSize.Width + 3 * padding);
                fontSize--;
            }
            while (totalTextWidth > baseRect.Width && fontSize >= minFontSize);

            // Tính kích thước label
            int labelHeight = (int)Math.Max(leftSize.Height, rightSize.Height) + 2 * padding;
            int labelY = (int)baseRect.Top - labelHeight;

            // LEFT LABEL
            int leftWidth = (int)leftSize.Width + 2 * padding;
            Rectangle leftRect = new Rectangle((int)baseRect.Left, labelY, leftWidth, labelHeight);

            using (SolidBrush leftBgBrush = new SolidBrush(baseBackColor))
            {
                graphics.FillRectangle(leftBgBrush, leftRect);
                graphics.DrawString(leftText, currentFont, textBrush, leftRect.Left + padding, leftRect.Top + padding);
            }

            // RIGHT LABEL background: kéo từ sau left đến hết box, nhưng text phải căn phải
            int rightStartX = leftRect.Right;
            int rightEndX = (int)baseRect.Right;
            int rightWidth = rightEndX - rightStartX;

            Rectangle rightRect = new Rectangle(rightStartX, labelY, rightWidth, labelHeight);

            Color transparentColor = Color.FromArgb(opacity, baseBackColor.R, baseBackColor.G, baseBackColor.B);
            using (SolidBrush rightBgBrush = new SolidBrush(transparentColor))
            {
                graphics.FillRectangle(rightBgBrush, rightRect);

                // Vị trí chữ: căn phải bên trong rightRect
                float textX = rightRect.Right - rightSize.Width - padding;
                float textY = rightRect.Top + padding;

                graphics.DrawString(rightText, currentFont, textBrush, textX, textY);
            }
        }

       
        public static void RectEdit(Graphics gc, TypeCrop TypeCrop, RectRotate RectDraw,Image ImageRotate, int WidthPoint,Point posAutoScroll,float zoom, int Thiness=2)
        {
            if (RectDraw == null) return;
            RectangleF _rect = new RectangleF(); ;
            PointF _rectPos = new PointF(); ;
            Single _rectRotation = 0;
        
            _rect = RectDraw._rect;
            _rectPos = RectDraw._PosCenter;
            _rectRotation = RectDraw._rectRotation;
            if (float.IsNaN( _rectRotation)) 
                return;
            var rectTopLeft = new RectangleF(_rect.Left - WidthPoint / 2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
            var rectTopRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
            var rectBottomLeft = new RectangleF(_rect.Left - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
            var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
            var rectRotate = new RectangleF(-WidthPoint / 2, _rect.Top + -WidthPoint * 3, WidthPoint * 2, WidthPoint * 2);
            var rectCenter = new RectangleF(-WidthPoint / 2, -WidthPoint / 2, WidthPoint, WidthPoint);
            Pen penRect = new Pen(Color.Orange, Thiness);
            var backNG = new SolidBrush(Color.FromArgb(0, 0, 0, 255));
            var backChoose = new SolidBrush(Color.FromArgb(60, 255, 205, 35));
            var cornerNone = new SolidBrush(Color.OrangeRed);
            var cornerChoose = new SolidBrush(Color.Blue);
            var _clX = new Pen(Color.LightGray, 1);
            var _clY = new Pen(Color.Gray, 1);
            AnchorPoint AnchorPoint = RectDraw._dragAnchor;
            Matrix mat = new Matrix();
            mat.Translate(posAutoScroll.X, posAutoScroll.Y);
            mat.Scale((float)(zoom / 100.0), (float)(zoom / 100.0));

            mat.Translate(_rectPos.X, _rectPos.Y);
            mat.Rotate(_rectRotation);
            gc.Transform = mat;
            switch (TypeCrop)
            {
                case TypeCrop.Area:
                        penRect = new Pen(Color.DeepSkyBlue, Thiness);
                    break;
                case TypeCrop.Crop:
                       penRect = new Pen(Color.Goldenrod, Thiness);
                    break;
                case TypeCrop.Mask:
                       penRect = new Pen(Color.DarkRed, Thiness);
                    break;
            }
            if (RectDraw.IsElip)
                gc.DrawEllipse(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
            else
              gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
            switch (AnchorPoint)
            {
                case AnchorPoint.None:
                  
                    gc.FillEllipse(cornerNone, rectTopLeft);
                    gc.FillEllipse(cornerNone, rectTopRight);
                    gc.FillEllipse(cornerNone, rectBottomLeft);
                    gc.FillEllipse(cornerNone, rectBottomRight);
                    //gc.FillRectangle(cornerNone, rectCenter);
                    break;

                case AnchorPoint.TopLeft:
                    rectTopLeft.Width +=WidthPoint; rectTopLeft.Height += WidthPoint;
                    rectTopLeft.X -= WidthPoint / 2; rectTopLeft.Y -= WidthPoint / 2;
                 
                    gc.FillRectangle(cornerChoose, rectTopLeft);
                    gc.FillEllipse(cornerNone, rectTopRight);
                    gc.FillEllipse(cornerNone, rectBottomLeft);
                    gc.FillEllipse(cornerNone, rectBottomRight);
                    gc.FillEllipse(cornerNone, rectRotate);
                    break;
                case AnchorPoint.TopRight:
                    //  gc.FillRectangle(backNone, _rect);
                    rectTopRight.Width += WidthPoint; rectTopRight.Height += WidthPoint;
                    rectTopRight.X -= WidthPoint / 2; rectTopRight.Y -= WidthPoint / 2;
                    gc.FillEllipse(cornerNone, rectTopLeft);
                    gc.FillRectangle(cornerChoose, rectTopRight);
                    gc.FillEllipse(cornerNone, rectBottomLeft);
                    gc.FillEllipse(cornerNone, rectBottomRight);
                    gc.FillEllipse(cornerNone, rectRotate);
                    break;
                case AnchorPoint.BottomLeft:
                    //  gc.FillRectangle(backNone, _rect);
                    rectBottomLeft.Width += WidthPoint; rectBottomLeft.Height += WidthPoint;
                    rectBottomLeft.X -= WidthPoint / 2; rectBottomLeft.Y -= WidthPoint / 2;
                    gc.FillEllipse(cornerNone, rectTopLeft);
                    gc.FillEllipse(cornerNone, rectTopRight);
                    gc.FillRectangle(cornerChoose, rectBottomLeft);
                    gc.FillEllipse(cornerNone, rectBottomRight);
                    gc.FillEllipse(cornerNone, rectRotate);
                    break;
                case AnchorPoint.BottomRight:
                    //gc.FillRectangle(backNone, _rect);
                    rectBottomRight.Width += WidthPoint; rectBottomRight.Height += WidthPoint;
                    rectBottomRight.X -= WidthPoint / 2; rectBottomRight.Y -= WidthPoint / 2;
                    gc.FillEllipse(cornerNone, rectTopLeft);
                    gc.FillEllipse(cornerNone, rectTopRight);
                    gc.FillEllipse(cornerNone, rectBottomLeft);
                    gc.FillRectangle(cornerChoose, rectBottomRight);
                    gc.FillEllipse(cornerNone, rectRotate);
                    break;
                case AnchorPoint.Center:
                    // gc.FillRectangle(backChoose, _rect);
             
                    gc.FillEllipse(cornerNone, rectTopLeft);
                    gc.FillEllipse(cornerNone, rectTopRight);
                    gc.FillEllipse(cornerNone, rectBottomLeft);
                    gc.FillEllipse(cornerNone, rectBottomRight);
                    gc.FillEllipse(cornerNone, rectRotate);
                    break;
                case AnchorPoint.Rotation:
                    // gc.FillRectangle(backNone, _rect);
                     gc.FillEllipse(cornerNone, rectTopLeft);
                    gc.FillEllipse(cornerNone, rectTopRight);
                    gc.FillEllipse(cornerNone, rectBottomLeft);
                    gc.FillEllipse(cornerNone, rectBottomRight);
                    gc.DrawImage(ImageRotate, rectRotate);
                    break;


            }
          
            gc.ResetTransform();
        }
        public static void FillRect(Graphics gc, TypeCrop TypeCrop, RectRotate RectDraw,  Point posAutoScroll, float zoom, int Opacity =10)
        {
            if (RectDraw == null) return;
            RectangleF _rect = new RectangleF(); ;
           

            _rect = RectDraw._rect;
             Brush backcolor = new SolidBrush(Color.FromArgb(0, 0, 0, 255));
            Matrix mat = new Matrix();
            mat.Translate(posAutoScroll.X, posAutoScroll.Y);
            mat.Scale((float)(zoom / 100.0), (float)(zoom / 100.0));
            mat.Translate(RectDraw._PosCenter.X, RectDraw._PosCenter.Y);
            mat.Rotate(RectDraw._rectRotation);
            gc.Transform = mat;
            switch (TypeCrop)
            {
                case TypeCrop.Area:
                    backcolor = new SolidBrush(Color.FromArgb(Opacity, 0, 191, 255));
                    break;
                case TypeCrop.Crop:
                    backcolor = new SolidBrush(Color.FromArgb(Opacity, 255, 165, 0));
                    break;
                case TypeCrop.Mask:
                    backcolor = new SolidBrush(Color.FromArgb(Opacity, 91, 91, 91));
                    break;
            }
            if (RectDraw.IsElip)
                gc.FillEllipse(backcolor, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
            else
                gc.FillRectangle(backcolor, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

            gc.ResetTransform();
        }
        public static void DrawInfiniteLine(Graphics g, PointF p1, PointF p2, Rectangle bounds, Pen pen)
        {
            if (p1 == p2) return; // Không thể xác định được nếu 2 điểm trùng nhau

            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            // Tránh chia cho 0 nếu là đường thẳng đứng
            if (dx == 0)
            {
                // Vẽ đường thẳng đứng đi qua p1.X
                g.DrawLine(pen, new PointF(p1.X, bounds.Top), new PointF(p1.X, bounds.Bottom));
                return;
            }

            float slope = dy / dx;
            float intercept = p1.Y - slope * p1.X;

            // Tính giao điểm với 2 cạnh trái - phải của bounds
            float xLeft = bounds.Left;
            float yLeft = slope * xLeft + intercept;

            float xRight = bounds.Right;
            float yRight = slope * xRight + intercept;

            // Cắt với phần hiển thị nếu cần
            PointF start = new PointF(xLeft, yLeft);
            PointF end = new PointF(xRight, yRight);

            g.DrawLine(pen, start, end);
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
     (
         int nLeftRect,     // x-coordinate of upper-left corner
         int nTopRect,      // y-coordinate of upper-left corner
         int nRightRect,    // x-coordinate of lower-right corner
         int nBottomRect,   // y-coordinate of lower-right corner
         int nWidthEllipse, // height of ellipse
         int nHeightEllipse // width of ellipse
     );

    }
}
