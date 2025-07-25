﻿using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static LibUsbDotNet.Main.UsbTransferQueue;
using Point = System.Drawing.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Circle
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
       
        [NonSerialized]
        public Mat matProcess=new Mat();
        public int MaxCircles = 2;
        public int Iterations = 100;
        public float Threshold = 1f;
        public int MinInliers=100;
        public bool IsIni = false;
        public int Index = -1;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp,matMask;
        public List<OpenCvSharp.Point> Postion=new List<OpenCvSharp.Point>();
        public TypeCrop TypeCrop;
        public Compares Compare = Compares.Equal;
        public List< System.Drawing.Point > listP_Center=new List<System.Drawing.Point>();
        public Circle()
        {


        }
        public void SetModel()
        {
            rotMask = null;
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 20;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
       

        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();
        public Exception exEr;
        public bool IsCalibs = false;
        public CircleScanDirection CircleScanDirection = CircleScanDirection.InsideOut;
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public void DoWork( RectRotate rectRotate)
        {
            try
            {
                RadiusResult = 0;
                rectRotates = new List<RectRotate>();            
                listP_Center = new List<Point>();
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.Processing;
                if (IsCalibs)
                {
                    MinInliers = 2;
                    MinRadius = 1;
                    MaxRadius = 10000;
                }
                    using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
                {
                    if (raw.Empty()) return;
                    Mat matCrop = Common.CropRotatedRect(raw, rectRotate, rotMask);
                    if(matProcess==null) matProcess = new Mat();
                    if (!matProcess.Empty()) matProcess.Dispose();
                    switch (MethordEdge)
                    {
                        case MethordEdge.CloseEdges:
                            matProcess = Filters.Edge(matCrop);
                            break;
                        case MethordEdge.StrongEdges:
                            matProcess = Filters.GetStrongEdgesOnly(matCrop);
                            break;
                        case MethordEdge.Binary:
                            matProcess = Filters.Threshold(matCrop);
                            break;
                    }

                    var circles = RansacCircleFitter.DetectCircles(
                   matProcess,
                   maxCircles: 3,
                  iterations: Iterations,
                  threshold: Threshold,
                  minInliers: MinInliers,
                   direction: CircleScanDirection,(int)(MinRadius*Scale),(int)(MaxRadius*Scale)

               );
                    foreach (var (center, radius, Inliers) in circles)
                    {
                        PointF pCenter = new PointF(center.X, center.Y);// new PointF(Convert.ToSingle(c.Center.X), Convert.ToSingle(c.Center.Y));
                        float angle = 0;
                        float width = Convert.ToSingle(radius * 2);
                        float height = Convert.ToSingle(radius * 2);
                        //float Score = Convert.ToSingle(100);
                        rectRotates.Add(new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None, false));
                        
                        listP_Center.Add(new System.Drawing.Point((int)rotAreaAdjustment._PosCenter.X - (int)rotAreaAdjustment._rect.Width / 2 + (int)pCenter.X, (int)rotAreaAdjustment._PosCenter.Y - (int)rotAreaAdjustment._rect.Height / 2 + (int)pCenter.Y));
                        RadiusResult = (float)((radius) / Scale);
                        RadiusResult = (float)Math.Round(RadiusResult, 2);
                        if (IsCalibs)
                        {
                            MinInliers = (int)((Inliers * (100-Common.PropetyTools[IndexThread][Index].Score)) / 100.0);
                            double Delta =(  Common.PropetyTools[IndexThread][Index].Score) /100.0;
                            MinRadius = (float)(RadiusResult * (1-Delta));
                            MaxRadius = (float)(RadiusResult * (1 + Delta));
                           // IsCalibs = false;
                        }    
                        
                        // Cv2.Circle(src, (OpenCvSharp.Point)center, (int)radius, Scalar.Red, 2);
                    }
                }
            }
            catch(Exception ex)
            {
                exEr = ex;
            }

        }
      
        public Graphics DrawResult(Graphics gc)
        {
            if (rotAreaAdjustment == null && Global.IsRun) return gc;
          
                gc.ResetTransform();
            RectRotate rotA = rotArea;
            if (Global.IsRun) rotA = rotAreaAdjustment;
            
            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;
            switch (Common.PropetyTools[Global.IndexChoose][Index].Results)
            {
                case Results.OK:
                    cl = Global.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ColorNG;
                    break;
            }
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Index].Name;
            if (!Global.IsHideTool)
                Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl, 1);
           
            if (!Global.IsRun)
            {
                if (!matProcess.Empty())
                {
                    gc.ResetTransform();
                    mat = new Matrix();
                    if (!Global.IsRun)
                    {
                        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                    }
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
                    gc.Transform = mat;
                    Bitmap myBitmap = matProcess.ToBitmap();
                    myBitmap.MakeTransparent(Color.Black);
                    myBitmap = General.ChangeToColor(myBitmap, Color.Red, 0.7f);
                    gc.DrawImage(myBitmap, rotA._rect);
                }
            }
            if (rectRotates.Count > 0)
            {
                int i = 1;
                foreach (RectRotate rot in rectRotates)
                {
                    gc.ResetTransform();
                    mat = new Matrix();
                    if (!Global.IsRun)
                    {
                        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                    }
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
                    mat.Translate(rotA._rect.X, rotA._rect.Y);
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                    Draws.Plus(gc, 0, 0, (int)rot._rect.Width / 6, cl, 2);
                    gc.DrawEllipse(new Pen(cl, 2), rot._rect);
                    gc.DrawString("D:" + RadiusResult, new Font("Arial", 24, FontStyle.Bold), new SolidBrush(cl), new PointF(0, 0));
                    gc.ResetTransform();
                    i++;
                }
            }
            return gc;
        }
        public float RadiusTemp = 0;
        public float RadiusResult= 0;
        public void Complete()
        {
            if(rectRotates.Count>0)
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            else
            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
           
          
            if (!Global.IsRun)
            {
                RadiusTemp = RadiusResult;
            }
          
            Common.PropetyTools[IndexThread][Index].ScoreResult = (int)((Math.Abs(RadiusResult - RadiusTemp) / (RadiusTemp * 1.0)) * 100);
            if (Common.PropetyTools[IndexThread][Index].ScoreResult < 0) Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
            if (rectRotates.Count==0)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }
            else if (Common.PropetyTools[IndexThread][Index].ScoreResult <= Common.PropetyTools[IndexThread][Index].Score)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            }
            else
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }
            //switch (Compare)
            //{
            //    case Compares.Equal:
            //        if (rectRotates.Count() != LimitCounter)
            //            IsOK = false;
            //        break;
            //    case Compares.Less:
            //        if (rectRotates.Count() >= LimitCounter)
            //            IsOK = false;
            //        break;
            //    case Compares.More:
            //        if (rectRotates.Count() <= LimitCounter)
            //            IsOK = false;
            //        break;
            //}


        }
      
        public float MinRadius = 0;
        public float MaxRadius = 0;
       
        public float Scale = 1;
        public int IndexThread = 0;
     
    }
}
