using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class AutoTrig
    {
        [NonSerialized]
        public bool IsNew = false;
        [NonSerialized]
        public Line2D Line;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int IndexCCD = 0;
        public bool IsIni = false;
        public int Index = -1;
        [NonSerialized]
        public int StepCheck = 0;
        [NonSerialized]
        public Line2DCli Line2DLeft = new Line2DCli();
        [NonSerialized]
        public Line2DCli Line2DLeft2 = new Line2DCli();
        [NonSerialized]
        public Line2DCli Line2DRight = new Line2DCli();
        [NonSerialized]
        public RansacLine RansacLine;
  
        [NonSerialized]
        public EdgePipelineOptionsCli EdgePipelineOptionsCli;
        [NonSerialized]
        public Mat matProcess = new Mat();
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<RectRotate> rectRotates = new List<RectRotate>();

     
        public int MaximumLine = 4;
        public GapExtremum GapExtremum = GapExtremum.Middle;
        public LineOrientation LineOrientation = LineOrientation.Vertical;
        public SegmentStatType SegmentStatType = SegmentStatType.Average;
        public int MinInliers = 2;
        public float WidthResult = 0;
        public float WidthTemp = 0;
        public int MinLen = 0, MaxLen = 10000;
        public bool IsCalibs = false;
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public int ThresholdBinary;
        public double RansacThreshold = 2.0; // px
        public int RansacIterations = 200;
        public bool IsClose = false;
        public bool IsOpen = false;
        public bool IsClearNoiseBig = false;
        public bool IsClearNoiseSmall = false;
        public int SizeClearsmall = 1;
        public int SizeClearBig = 1;
        public int SizeClose = 1;
        public int SizeOpen = 1;
        public void Default()
        {
            GapExtremum = GapExtremum.Middle;
            LineOrientation = LineOrientation.Vertical;
            SegmentStatType = SegmentStatType.Average;
            MinLen = 0;
            MaxLen = 10000;
            WidthTemp = 0;
            RansacIterations = 200;
            RansacThreshold = 2;
            ThresholdBinary = 150;
            MethordEdge = MethordEdge.StrongEdges;
            SizeClearsmall = 100;
            SizeClearBig = 1000;
            SizeClose = 3;
            SizeOpen = 3;
            MaximumLine = 10;

            IsClearNoiseBig = false;
            IsClearNoiseSmall = false;
            IsClose = false;
            IsOpen = false;
        }
        public AutoTrig()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
        [NonSerialized]
        private CropPlus CropPlus = new CropPlus();


        public float AspectLen=0.6f;

        public void DoWork(RectRotate rect, RectRotate rotMask)
        {
            LineDirectionMode lineDirectionMode = LineDirectionMode.Vertical;

            if (matProcess != null) { matProcess.Dispose(); matProcess = null; }
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (raw.Empty()) return;
                Mat matCrop = new Mat();
                switch (StepCheck)
                {
                    case 0:
                        matCrop = Cropper.CropRotatedRectUltraFast(raw, rotArea);//Left
                        break;
                    case 1:
                        matCrop = Cropper.CropRotatedRectUltraFast(raw, rotCrop);//Right
                        break;
                    case 2:
                        matCrop = Cropper.CropRotatedRectUltraFast(raw, rotArea);//Left
                        break;
                    
                }
            
               
                if (matProcess == null) matProcess = new Mat();
                if (!matProcess.Empty()) matProcess.Dispose();
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGR2GRAY);
                switch (MethordEdge)
                {
                    case MethordEdge.CloseEdges:
                        matProcess = Filters.Edge(matCrop);
                        break;
                    case MethordEdge.StrongEdges:
                        matProcess = Filters.GetStrongEdgesOnly(matCrop);
                        break;
                    case MethordEdge.Binary:
                        matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.Binary);
                        break;
                    case MethordEdge.InvertBinary:
                        matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.BinaryInv);
                        break;
                }
                if (matProcess.Width == 0)
                    return;
                if (IsClearNoiseSmall)
                    matProcess = Filters.ClearNoise(matProcess, SizeClearsmall);
                if (IsClose)
                    matProcess = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                if (IsOpen)
                    matProcess = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                if (IsClearNoiseBig)
                    matProcess = Filters.ClearNoise(matProcess, SizeClearBig);
                switch(StepCheck)
                {
                    case 0:

                        Line2DLeft = RansacLine.FindBestLine(
                            matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                            iterations: RansacIterations,
                            threshold: (float)RansacThreshold,
                            maxPoints: 120000,
                            seed: Index,
                            mmPerPixel: 1 , AspectLen, (BeeCpp.LineDirectionMode)((int)lineDirectionMode), LineScanMode.RightToLeft, 0, 30
                        );
                        if(Line2DLeft.Found)
                        {
                            StepCheck++;
                        }
                        break;
                    case 1:
                        Line2DRight = RansacLine.FindBestLine(
                           matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                           iterations: RansacIterations,
                           threshold: (float)RansacThreshold,
                           maxPoints: 120000,
                           seed: Index,
                           mmPerPixel: 1 , AspectLen, (BeeCpp.LineDirectionMode)((int)lineDirectionMode), LineScanMode.RightToLeft, 0, 30
                       );
                        if (Line2DRight.Found)
                        {
                            StepCheck++;
                        }
                        break;
                    case 2:
                        Line2DLeft2 = RansacLine.FindBestLine(
                           matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                           iterations: RansacIterations,
                           threshold: (float)RansacThreshold,
                           maxPoints: 120000,
                           seed: Index,
                           mmPerPixel: 1, AspectLen, (BeeCpp.LineDirectionMode)((int)lineDirectionMode), LineScanMode.LeftToRight, 0, 30
                       );
                        if (Line2DLeft2.Found)
                        {
                            StepCheck++;
                        }
                        break;
                    case 3:

                        break;

                }

              
            

            }
           
        
        }
        public void Complete()
        {
            if(StepCheck==3)
            {
               
                Common.TryGetTool(IndexThread, Index).Results = Results.OK;
            }
                
            else
            
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
           
           

        }
        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        public bool IsSendResult;

        public Graphics DrawResult(Graphics gc)
        {

          
          
            gc.ResetTransform();       
            RectRotate rotA =rotArea;
            Color clLeft = Color.LimeGreen;
            Color clRight = Color.LimeGreen;
            switch (StepCheck)
            {
                case 0:
                    clLeft = Global.ParaShow.ColorNone;
                    clRight = Global.ParaShow.ColorNone;
                    break;
                case 1:
                    clLeft = Global.ParaShow.ColorChoose;
                    clRight = Global.ParaShow.ColorNG;
                    break;
                case 2:
                    clLeft = Global.ParaShow.ColorChoose;
                    clRight = Global.ParaShow.ColorOK;
                    break;
                case 3:
                    clLeft = Global.ParaShow.ColorOK;
                    clRight = Global.ParaShow.ColorOK;
                    break;
            }
            Brush brushText = Brushes.White;

            Pen pen = new Pen(Global.ParaShow.ColorInfor, Global.ParaShow.ThicknessLine);
            String nameTool = (int)(Index + 1) + "." + Common.TryGetTool(Global.IndexProgChoose, Index).Name;

            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
           
            if (Global.IsRun) rotA =rotAreaAdjustment;


            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            gc.DrawString("Step:" + StepCheck, font, brushText, new PointF(50, 50));
            if (rotArea!=null)
            {
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotArea._PosCenter.X, rotArea._PosCenter.Y);
                mat.Rotate(rotArea._rectRotation);
                gc.Transform = mat;
                nameTool = (int)(Index + 1) + "." + Common.TryGetTool(Global.IndexProgChoose, Index).Name+"_Left";

                if (Global.ParaShow.IsShowBox)
                    Draws.Box1Label(gc, rotArea, nameTool, font, brushText, clLeft, Global.ParaShow.ThicknessLine);

            }
            if (rotCrop != null)
            {
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotCrop._PosCenter.X, rotCrop._PosCenter.Y);
                mat.Rotate(rotCrop._rectRotation);
                gc.Transform = mat;
                nameTool = (int)(Index + 1) + "." + Common.TryGetTool(Global.IndexProgChoose, Index).Name + "_Right";

                if (Global.ParaShow.IsShowBox)
                    Draws.Box1Label(gc, rotCrop, nameTool, font, brushText, clRight, Global.ParaShow.ThicknessLine);
            }
            if (Line2DLeft.Found)
            {
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotArea._PosCenter.X, rotArea._PosCenter.Y);
                mat.Translate(rotArea._rect.X, rotArea._rect.Y);
                mat.Rotate(rotArea._rectRotation);
                gc.Transform = mat;
                gc.DrawLine(new Pen(new SolidBrush(clLeft), Global.ParaShow.ThicknessLine), Line2DLeft.X1, Line2DLeft.Y1, Line2DLeft.X2, Line2DLeft.Y2);
            }
            if (Line2DLeft2.Found)
            {
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotArea._PosCenter.X, rotArea._PosCenter.Y);
                mat.Translate(rotArea._rect.X, rotArea._rect.Y);
                mat.Rotate(rotArea._rectRotation);
                gc.Transform = mat;
                gc.DrawLine(new Pen(new SolidBrush(clLeft), Global.ParaShow.ThicknessLine), Line2DLeft2.X1, Line2DLeft2.Y1, Line2DLeft2.X2, Line2DLeft2.Y2);
            }
            if (Line2DRight.Found)
            {
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotCrop._PosCenter.X, rotCrop._PosCenter.Y);
                mat.Translate(rotCrop._rect.X, rotCrop._rect.Y);
                mat.Rotate(rotCrop._rectRotation);
                gc.Transform = mat;
                gc.DrawLine(new Pen(new SolidBrush(clRight), Global.ParaShow.ThicknessLine), Line2DRight.X1, Line2DRight.Y1, Line2DRight.X2, Line2DRight.Y2);
            }
        if(StepCheck==3)
            { 
             StepCheck = 0;
            Line2DLeft.Found = false;
            Line2DRight.Found = false;
            Line2DLeft2.Found = false;

            }
            return gc;
        }


        public void SetModel()
        {
            if (rotArea == null) rotArea = new RectRotate();
            if (rotCrop == null) rotCrop = new RectRotate();
         
            if (RansacLine == null) RansacLine = new RansacLine();
           
            CropPlus=new CropPlus();
            Common.TryGetTool(IndexThread, Index).StepValue = 0.1f;
            Common.TryGetTool(IndexThread, Index).MinValue = 0;
          
            Common.TryGetTool(IndexThread, Index).MaxValue = 20;
            Common.TryGetTool(IndexThread, Index). StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
