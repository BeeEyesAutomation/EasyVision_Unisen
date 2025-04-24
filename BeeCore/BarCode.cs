using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ZXing;
namespace BeeCore
{
    public class BarCode
    {
        public TypeTool TypeTool;
        public TypeCrop TypeCrop;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp, matMask;
        public bool IsOK = false;
        public double cycleTime;
        // Hàm tạo khung bao quanh mã QR từ tọa độ các điểm
        //static Rectangle GetBoundingBox(ResultPoint[] points)
        //{
        //    float minX = Math.Min(points[0].X, points[1].X);
        //    float minY = Math.Min(points[0].Y, points[1].Y);
        //    float maxX = Math.Max(points[0].X, points[1].X);
        //    float maxY = Math.Max(points[0].Y, points[1].Y);

        //    return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        //}
        public  Mat Read(RectRotate rotateRect)
        {
            //Mat matCrop = Common.CropRotatedRect(BeeCore.Common.matRaw, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), rotateRect._angle));
          
            //    IBarcodeReader reader = new BarcodeReader();
            //Bitmap bmRS= matCrop.ToBitmap();
            //// load a bitmap
            ////  var barcodeBitmap = (Bitmap)Image.FromFile("C:\\sample-barcode-image.png");
            //// detect and decode the barcode inside the bitmap
            //var result = reader.Decode(bmRS);


            //// do something with the result
            //if (result != null)
            //{
            //    var points = result.ResultPoints;

            //    using (Graphics g = Graphics.FromImage(bmRS))
            //    {
            //        // Vẽ khung bao quanh mã QR
            //        if (points.Length > 1)
            //        {
            //            // Vẽ hình chữ nhật xung quanh mã QR
            //            var rect = GetBoundingBox(points);
            //            g.DrawRectangle(Pens.Red, rect);
            //        }

            //        // Hiển thị nội dung mã QR
            //        g.DrawString(result.Text, new Font("Arial", 16), Brushes.Red, new PointF(10, 10));
            //    }
            //    IsOK = true;
            //    return bmRS.ToMat();
             
            //    //result.BarcodeFormat.ToString();
            //  //  return result.Text;
            //}
            //IsOK = false;
            return null;
        }

    }
}
