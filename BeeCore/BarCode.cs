using OpenCvSharp;
using System;
using System.Drawing;
//using ZXing;
namespace BeeCore
{
    public class BarCode
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
<<<<<<< HEAD
        public int Index;
=======
>>>>>>> edca0d23a94938580687a3e9499f230b6b57c2ae
        public TypeTool TypeTool;
        public TypeCrop TypeCrop;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp, matMask;
        public bool IsOK = false;
        public String MathQRCODE ="";
        public int Score = 0;
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
        public Polygon[] rectQRCode;
<<<<<<< HEAD
        public String Content = "";
=======
>>>>>>> edca0d23a94938580687a3e9499f230b6b57c2ae
        public  Mat Read(RectRotate rotateRect)
        {
            if(HEROJE.BarcodeStr!="")
            {
<<<<<<< HEAD
                Content = HEROJE.BarcodeStr;
=======
>>>>>>> edca0d23a94938580687a3e9499f230b6b57c2ae
                if (HEROJE.BarCodeRegion.Length>0);
                {
                    rectQRCode = HEROJE.BarCodeRegion;
                    if (MathQRCODE == "")
                        IsOK = true;
                }
                //for (int i = 0; i < BarCodeRegion.Length; i++)
                //{
                //    Point[] array4 = BarCodeRegion[i].ToPointArray();
                //    for (int j = 0; j < array4.Length; j++)
                //    {
                //        array4[j].X += num7;
                //        array4[j].Y += num9;
                //    }
                //    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //    graphics.DrawLines(BarcodePen, array4);
                //}
                //HEROJE.BarCodeRegion.
            }
            else
            {
                IsOK = false;
            }
              
                return null;
        }

    }
}
