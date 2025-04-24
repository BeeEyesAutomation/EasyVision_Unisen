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
        public static void Rectangle(Graphics gc, TypeCrop TypeCrop, RectRotate RectDraw,Image ImageRotate, int WidthPoint,Point posAutoScroll,float zoom, int Thiness=2)
        {
            RectangleF _rect = new RectangleF(); ;
            PointF _rectPos = new PointF(); ;
            Single _rectRotation = 0;
        
            _rect = RectDraw._rect;
            _rectPos = RectDraw._PosCenter;
            _rectRotation = RectDraw._rectRotation;
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
                        penRect = new Pen(Color.DeepSkyBlue, 2);
                    break;
                case TypeCrop.Crop:
                       penRect = new Pen(Color.Orange, 2);
                    break;
                case TypeCrop.Mask:
                       penRect = new Pen(Color.FromArgb(100, 111, 211, 213), 2);
                    break;
            }
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
