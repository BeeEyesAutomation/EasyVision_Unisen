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
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using static BeeCore.Algorithm.FilletCornerMeasure;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Pitch
    {
        [NonSerialized]
        public Line2D Line;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsIni = false;
        public int Index = -1;
        [NonSerialized]
        public BeeCppCli.PitchCliResult PitchResult;
        [NonSerialized]
        public Mat matProcess = new Mat();
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<RectRotate> rectRotates = new List<RectRotate>();

        [NonSerialized]
       // public ParallelGapDetector ParallelGapDetector;
        public BeeCppCli.PitchCli PitchMeasure;
        public bool IsEnCrestPitch = true;
        public bool IsEnRootPitch = true;
        public bool IsEnCrestHeight = true;
        public bool IsEnRootHeight = true;
        public bool IsEnCrestCounter = true;
        public bool IsEnRootCounter = true;
        public Values Values = Values.Mean;
        public LineOrientation LineOrientation = LineOrientation.Vertical;
        public float ValueGau=3;
        public int NumCrestCouter;
        public int NumRootCouter;
        public int Magin = 0;
        public Compares Compare = Compares.Equal;
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public int ThresholdBinary;
   
        public bool IsClose = false;
        public bool IsOpen = false;
        public bool IsClearNoiseBig = false;
        public bool IsClearNoiseSmall = false;
        public int SizeClearsmall = 1;
        public int SizeClearBig = 1;
        public int SizeClose = 1;
        public int SizeOpen = 1;

        public float TempPitchCrest = 0;
        public float TempPitchRoot = 0;
        public float TempHeightCrest = 0;
        public float TempHeightRoot = 0;
        public float TempCountCrest = 0;
        public float TempCountRoot = 0;
        public void Default()
        {
            Magin = 0;
            ValueGau = 3;
            Compare = Compares.Equal;
            Values = Values.Mean;
            LineOrientation = LineOrientation.Vertical;
            IsEnCrestPitch = true;
            IsEnCrestHeight= false;
            IsEnRootPitch = false;
            IsEnRootHeight= false;
            ThresholdBinary = 150;
            MethordEdge = MethordEdge.StrongEdges;
            SizeClearsmall = 100;
            SizeClearBig = 1000;
            SizeClose = 3;
            SizeOpen = 3;
         

            IsClearNoiseBig = false;
            IsClearNoiseSmall = false;
            IsClose = false;
            IsOpen = false;
        }
        public Pitch()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }

        float CurPitchCrest;
        float CurPitchRoot;
        float CurHeightCrest;
        float CurHeightRoot;

        public bool IsCalib = false;

       public bool IsNGCrestPitch, IsNGCrestHeight, IsNGRootPitch, IsNGRootHeight,IsNGCountCrest,IsNGCountRoot;
        public void DoWork( RectRotate rectRotate)// lay anh raw xong crop dung bo loc, de loc canh
        {
            try
            {

                using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
                {
                    if (raw.Empty()) return;

                    Mat matCrop = Common.CropRotatedRect(raw, rectRotate, null);
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
                    if (IsClearNoiseSmall)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearsmall);
                    if (IsClose)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                    if (IsOpen)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                    if (IsClearNoiseBig)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearBig);
                    
                    PitchMeasure.SetGaussianSigma(ValueGau, -1.0);
                    PitchMeasure.SetScaleMmPerPx(Scale);
                    if (LineOrientation==LineOrientation.Vertical)
                    {
                        PitchMeasure.SetMargins(0,Magin);
                        PitchMeasure.SetScanAxis(BeeCppCli.ScanAxisCli.Y);
                    }    
                       
                    else
                    {
                        PitchMeasure.SetMargins(Magin, 0);
                        PitchMeasure.SetScanAxis(BeeCppCli.ScanAxisCli.X);
                    }    
                      
                    PitchMeasure.SetRejectedPolicy(true, 100);
                    PitchMeasure.SetImage(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels());
                    PitchResult = PitchMeasure.Measure();
                }
            }

            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Width", ex.Message));

            }


        }
        public void Complete()
        {
           if(!Global.IsRun&& IsCalib)
            {
                TempPitchCrest =(float)( Values == Values.Mean ? PitchResult.PitchMeanMM : (Values == Values.Median ? PitchResult.PitchMedianMM : (Values == Values.Min ? PitchResult.PitchMinMM : PitchResult.PitchMaxMM)));
                TempPitchRoot = (float)(Values == Values.Mean ? PitchResult.PitchRootMeanMM : (Values == Values.Median ? PitchResult.PitchRootMedianMM : (Values == Values.Min ? PitchResult.PitchRootMinMM : PitchResult.PitchRootMaxMM)));
                TempHeightCrest = (float)(Values == Values.Mean ? PitchResult.CrestHMeanMM : (Values == Values.Median ? PitchResult.CrestHMedianMM : (Values == Values.Min ? PitchResult.CrestHMinMM : PitchResult.CrestHMaxMM)));
                TempHeightRoot = (float)(Values == Values.Mean ? PitchResult.RootHMeanMM : (Values == Values.Median ? PitchResult.RootHMedianMM : (Values == Values.Min ? PitchResult.RootHMinMM : PitchResult.RootHMaxMM)));
                TempCountCrest = PitchResult.Crests.Length;
                TempCountRoot = PitchResult.Roots.Length;
            }
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            IsNGCrestPitch = false; IsNGCrestHeight = false; IsNGRootPitch = false; IsNGRootHeight = false; IsNGCountCrest = false; IsNGCountRoot = false; 
            if (IsEnCrestCounter)
            {
                switch (Compare)
                {
                    case Compares.Equal:
                        if(PitchResult.Crests.Length!=NumCrestCouter)
                        {
                            IsNGCountCrest = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        }
                           
                        break;
                    case Compares.Less:
                       
                        if (PitchResult.Crests.Length >= NumCrestCouter)
                        {
                            IsNGCountCrest = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        }
                      
                        break;
                    case Compares.More:
                       
                        if (PitchResult.Crests.Length <= NumCrestCouter)
                        {
                            IsNGCountCrest = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        }
                       
                        break;
                }
            }
            if (IsEnRootCounter)
            {
                switch (Compare)
                {
                    case Compares.Equal:
                        if (PitchResult.Roots.Length != NumRootCouter)
                        {
                            IsNGCountRoot = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        }
                           
                        break;
                    case Compares.Less:
                        if (PitchResult.Roots.Length >= NumRootCouter)
                        {
                            IsNGCountRoot = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        }
                           
                        break;
                    case Compares.More:
                        if (PitchResult.Roots.Length <= NumRootCouter)
                        {
                            IsNGCountRoot = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        }
                       
                        break;
                }
            }
            CurPitchCrest = (float)(Values == Values.Mean ? PitchResult.PitchMeanMM : (Values == Values.Median ? PitchResult.PitchMedianMM : (Values == Values.Min ? PitchResult.PitchMinMM : PitchResult.PitchMaxMM)));
             CurPitchRoot = (float)(Values == Values.Mean ? PitchResult.PitchRootMeanMM : (Values == Values.Median ? PitchResult.PitchRootMedianMM : (Values == Values.Min ? PitchResult.PitchRootMinMM : PitchResult.PitchRootMaxMM)));
             CurHeightCrest = (float)(Values == Values.Mean ? PitchResult.CrestHMeanMM : (Values == Values.Median ? PitchResult.CrestHMedianMM : (Values == Values.Min ? PitchResult.CrestHMinMM : PitchResult.CrestHMaxMM)));
             CurHeightRoot = (float)(Values == Values.Mean ? PitchResult.RootHMeanMM : (Values == Values.Median ? PitchResult.RootHMedianMM : (Values == Values.Min ? PitchResult.RootHMinMM : PitchResult.RootHMaxMM)));
            Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
            float value=0;
            if (IsEnCrestPitch)
            {
                value = (Math.Abs(TempPitchCrest - CurPitchCrest) / TempPitchCrest) * 100;
                if (value > Common.PropetyTools[IndexThread][Index].ScoreResult) Common.PropetyTools[IndexThread][Index].ScoreResult = value;
                if (Common.PropetyTools[IndexThread][Index].ScoreResult > Common.PropetyTools[IndexThread][Index].Score)
                {
                    IsNGCrestPitch = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                }
              
            }
            if (IsEnRootPitch)
            {
                value = (Math.Abs(TempPitchRoot - CurPitchRoot) / TempPitchRoot) * 100;
                if (value > Common.PropetyTools[IndexThread][Index].ScoreResult) Common.PropetyTools[IndexThread][Index].ScoreResult = value;

                if (Common.PropetyTools[IndexThread][Index].ScoreResult > Common.PropetyTools[IndexThread][Index].Score) 
                {
                    IsNGRootPitch = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                }
                
            }
          
            if (IsEnRootHeight)
            {
               value= (Math.Abs(TempHeightRoot - CurHeightRoot) / TempHeightRoot) * 100;
                if (value > Common.PropetyTools[IndexThread][Index].ScoreResult) Common.PropetyTools[IndexThread][Index].ScoreResult = value;

                if (Common.PropetyTools[IndexThread][Index].ScoreResult >Common.PropetyTools[IndexThread][Index].Score) 
                {
                    IsNGRootHeight = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                }
               
            }

            if (IsEnCrestHeight)
            {
                value = (Math.Abs(TempHeightCrest - CurHeightCrest) / TempHeightCrest) * 100;
                if (value > Common.PropetyTools[IndexThread][Index].ScoreResult)
                    Common.PropetyTools[IndexThread][Index].ScoreResult = value;

                if (Common.PropetyTools[IndexThread][Index].ScoreResult > Common.PropetyTools[IndexThread][Index].Score)
                {
                    IsNGCrestHeight = true; Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                }
               
            }
         

        }
        public async Task SendResult()
        {
            if (Common.PropetyTools[IndexThread][Index].IsSendResult)
            {
                if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                   // await Global.ParaCommon.Comunication.Protocol.WriteResultFloat(Common.PropetyTools[IndexThread][Index].AddPLC, WidthResult);
                }
            }
        }
        public Graphics DrawResult(Graphics gc)
        {

            if (rotAreaAdjustment == null && Global.IsRun) return gc;
          
            gc.ResetTransform();       
            RectRotate rotA =rotArea;
            if (Global.IsRun) rotA =rotAreaAdjustment;
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
            switch(Common.PropetyTools[Global.IndexChoose][Index].Results)
            {
                case Results.OK:
                    cl =  Global.Config.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.Config.ColorNG;
                    break;
            }
            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
            {
                String crest = "", root = "";
                try { crest = $"P={CurPitchCrest:0.###} mm"; } catch { }
                try { root = $"P={CurPitchRoot:0.###} mm"; } catch { }

                int nC = PitchResult.Crests?.Length ?? 0;
                int nR = PitchResult.Roots?.Length ?? 0;

                String Content = $"Crest N={nC}  {crest} | Root N={nR}  {root} ";
                Draws.Box2Label(gc, rotA, nameTool, Content, font, cl, brushText,16, Global.Config.ThicknessLine);

            }

            if (!Global.IsRun || Global.Config.IsShowDetail)
            {
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.Config.Opacity / 100.0f);
            }
         
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
            gc.Transform = mat;
            if (PitchResult != null)
            {
                var opt = new BeeCore.PainterOptions
                {   CrestRadius=10,
                    RootRadius=10,
                    ShowCenterline = true,
                    ShowCrestPoints = true,
                    ShowRootPoints = true,
                    ShowHeightsCrest = true,
                    ShowHeightsRoot = true,
                    ShowRejected = true,
                    ShowTitle = false,
                    ShowPitchLabels = true,
                    ShowHeightLabels = true,
                    PitchTextColor = Color.DarkOrange,
                    HeightTextColor = Color.Blue,
                    CenterlineColor = Color.Magenta,
                    CrestPointColor = Color.Red,
                    RootPointColor = Color.Gold
                };

                BeeCore.PitchGdiPainter.Draw(gc, PitchResult, new Font("Segoe UI", 28f, FontStyle.Regular, GraphicsUnit.Pixel), opt);
            }    
         
           // if (!Global.IsRun)
           //     foreach (var l in GapResult.line2Ds)
           //         Draws.DrawInfiniteLine(gc, l, new Pen(Color.Gray, 2));
           // Draws.DrawInfiniteLine(gc,GapResult.LineA, new Pen(cl, 2));
           // Draws.DrawInfiniteLine(gc,GapResult.LineB, new Pen(cl, 2));
           // PointF p1 = new PointF(GapResult.lineMid[0].X,GapResult.lineMid[0].Y);
           // PointF p2 = new PointF(GapResult.lineMid[1].X,GapResult.lineMid[1].Y);
           // Draws.DrawTicks(gc, p1,LineOrientation, pen);
           // Draws.DrawTicks(gc, p2,LineOrientation, pen);
           // gc.DrawLine(new Pen(Color.Blue, 4), p1, p2);          
           //gc.DrawString($"{WidthResult:F2}mm", new Font("Arial", 16), Brushes.Blue, p1.X + 5, (p1.Y + p2.Y) / 2 + 10);
           // gc.ResetTransform();
           // //mat = new Matrix();
           //if (!Global.IsRun)
           //{
           //    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
           //    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
           //    gc.Transform = mat;
           //}
           //gc.DrawString("X:" + listP_Center[0].X + ":" + listP_Center[0].X, new Font("Arial", 24, FontStyle.Bold), new SolidBrush(cl), new PointF(0, 0));

            //gc.DrawEllipse(new Pen(Brushes.Red, 4), new Rectangle(listP_Center[0].X, listP_Center[0].Y, 10, 10));

            return gc;
        }


        public void SetModel()
        {
            rotMask = null;
            PitchMeasure = new BeeCppCli.PitchCli();
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
    
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
          
            Common.PropetyTools[IndexThread][Index].MaxValue = 20;
            Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
