using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
//using ZXing;
namespace BeeCore
{
    [Serializable()]
    public class BarCode
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int Index;

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
       // public Polygon[] rectQRCode;

        public String Content = "";
        public int ScoreRs = 0;
        public void DoWork()
        {
            StatusTool = StatusTool.Processing;
            IsOK = false;
            Content = HEROJE.BarcodeStr;
            if (Content != "")
            {


                if (HEROJE.BarCodeRegion == null)
                {
                    IsOK = false;
                    return;
                }
                if (HEROJE.BarCodeRegion.Length > 0) ;
                {
                    // rectQRCode = HEROJE.BarCodeRegion;
                    if (MathQRCODE == "")
                        IsOK = true;
                    else
                        if (MathQRCODE == Content)
                    {
                        IsOK = true;
                    }
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

            

        }
        public void Complete()
        {
            StatusTool = StatusTool.Done;

        }
        //public void Ini()
        //{
        //    var boxList = new List<RectRotate>();
        //    var scoreList = new List<float>();
        //    var labelList = new List<string>();
        //    int numOK = 0, numNG = 0;
        //    int scoreRS = 0;
        //    worker = new BackgroundWorker();
        //    worker.DoWork += (sender, e) =>
        //    {
        //    };
        //    worker.RunWorkerCompleted += (sender, e) =>
        //    {
        //        StatusTool = StatusTool.Done;
        //    };
        //}
    
        public String nameTool = "";
        public StatusTool StatusTool = StatusTool.None;
        public  Mat Read(RectRotate rotateRect)
        {
            IsOK = false; 
            Content = HEROJE.BarcodeStr;
            if (Content != "")
            {


                if (HEROJE.BarCodeRegion == null) return null;
                if (HEROJE.BarCodeRegion.Length>0);
                {
                   // rectQRCode = HEROJE.BarCodeRegion;
                    if (MathQRCODE == "")
                        IsOK = true;
                    else
                        if (MathQRCODE == Content)
                    {
                        IsOK = true;
                    }
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
