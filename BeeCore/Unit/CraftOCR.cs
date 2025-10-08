﻿using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using CvPlus;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using static LibUsbDotNet.Main.UsbTransferQueue;
using static OpenCvSharp.ML.DTrees;
using static System.Windows.Forms.MonthCalendar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Point = System.Drawing.Point;
using ShapeType = BeeGlobal.ShapeType;

namespace BeeCore
{
    [Serializable()]
    public class CraftOCR
    {
        public int _Percent = 0;//note
        [field: NonSerialized]
        public event Action<int> PercentChange;

        public int Percent
        {
            get => _Percent;
            set
            {
                if (_Percent != value)
                {
                    _Percent = value;
                    PercentChange?.Invoke(_Percent);
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        
     
        public String pathFullModel = "";
        [NonSerialized]
        public Mat matTemp;
        [NonSerialized]
        public Pattern Pattern;
             public Mat LearnPattern(Mat raw, bool IsNoCrop)
        {

            using (Mat img = raw.Clone())
            {
                if (img.Channels() == 3)
                    Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
                else if (img.Channels() == 4)
                    Cv2.CvtColor(img, img, ColorConversionCodes.BGRA2GRAY);
                Mat mat = new Mat();
                if (!IsNoCrop)
                    mat = Cropper.CropRotatedRect(img, rotCrop, rotMask);
                else
                    mat = img;
                Pattern.SetImgeSampleNoCrop(mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels());
                Pattern.LearnPattern();
                return mat;

        
            }





        }
        public int OffSetSample = 10;
        public int _OffSetArea = 30;
        public int OffSetArea
        {
            get
            {
                return _OffSetArea;
            }
            set
            {
                _OffSetArea = value;
                if (rotTemp != null)
                {


                    List<PointF> points = PolyOffset.OffsetRadial(rotTemp.PolyLocalPoints, _OffSetArea);
                    RectRotate rectRotate = new RectRotate(rotTemp._rect, rotTemp._PosCenter, rotTemp._rectRotation, rotTemp._dragAnchor);
                    rectRotate.Shape = ShapeType.Polygon;
                    rectRotate.PolyLocalPoints = points;
                    rectRotate.IsPolygonClosed = true;
                    rotArea = rectRotate.Clone();
                    rotArea.UpdateFromPolygon(false);
                }
            }
        }
        public void SetTemp( RectRotate rot)
        {
          
            List<PointF> points = PolyOffset.OffsetRadial(rot.PolyLocalPoints, OffSetSample);
            RectRotate rectRotate = new RectRotate(rot._rect, rot._PosCenter, rot._rectRotation, rot._dragAnchor);
            rectRotate.Shape = ShapeType.Polygon;
            rectRotate.PolyLocalPoints = points;
            rectRotate.IsPolygonClosed = true;
           rotTemp = rectRotate.Clone();
            rotTemp.UpdateFromPolygon(false);
            points = PolyOffset.OffsetRadial(rot.PolyLocalPoints, OffSetArea);
            RectRotate rectRotate2 = new RectRotate(rot._rect, rot._PosCenter, rot._rectRotation, rot._dragAnchor);
            rectRotate2.Shape = ShapeType.Polygon;
            rectRotate2.PolyLocalPoints = points;
            rectRotate2.IsPolygonClosed = true;
            rotArea = rectRotate2.Clone();
            rotArea.UpdateFromPolygon(false);
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                matTemp = Cropper.CropRotatedRect(raw,rotTemp, null);
                LearnPattern(matTemp, true);
                bmRaw = matTemp.ToBitmap();
               
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.Done;
            }

        }

        public  void SetModel()
        {    OffSetSample = 1;
          
            if (Pattern == null)
            {
                Pattern = new BeeCpp.Pattern();

            }
            if (bmRaw != null)
            {
                matTemp = bmRaw.ToMat();
                LearnPattern(matTemp, true);
            }
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            if (labelItems==null)labelItems = new List<LabelItem>();

            try
            {
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.NotInitial;
             
                if (Global.IsIntialPython)
              

                using (Py.GIL())
                {
                        //// Thêm Tool vào sys.path
                        //dynamic sys = Py.Import("sys");
                        //sys.path.insert(0, "Tool");

                        int longSize = 1280;
                   
                        var pyNone = PyObject.FromManagedObject(null);
                        // Gọi load_model()
                        G.objCraftOCR.load_model_positional(true, 1280, null, null, true, 0.7, 0.4, 0.4);


                    }
           
            }
                catch (PythonException pyEx)
                {
                       MessageBox.Show("Python Error: " + pyEx.Message);
                }
                catch (Exception ex)
                {
                      MessageBox.Show("Error: " + ex.Message);
                }
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;

            // G.YoloPlus.LoadModel(nameTool, nameModel, (int)TypeYolo);
        }
      
        public List<LabelItem> labelItems = new List<LabelItem>();
        public List<Labels> listLabelCompare = new List<Labels>();
        public int Index = -1;
        public String PathModel = "",PathLabels="",PathDataSet;
        public TypeYolo TypeYolo = TypeYolo.YOLO;
        public TypeTool TypeTool=TypeTool.Learning;
        public RectRotate rotArea, rotCrop,rotTemp, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();

        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public List<RectRotate> listRotScan = new List<RectRotate>();
        public Bitmap bmRaw;
        public List<String> Labels = new List<string>();
        private Mode _TypeMode = Mode.Pattern;
        public Compares Compare = Compares.Equal;
        public Compares CompareLine = Compares.More;
        public Compares CompareArea = Compares.More;
        public float LimitArea = 100;
        public int Epoch =100;
        public bool IsScanOCR = false;
        public Mode TypeMode
        {
            get
            {
                return _TypeMode;
            }
            set
            {
                _TypeMode = value;

            }
        }
        public string pathRaw;
        public TypeCrop TypeCrop;
     
        public bool IsAreaWhite = false;
       
        public bool IsIni = false;
      
      
        //IsProcess,Convert.ToBoolean((int) TypeMode)
        [NonSerialized]
        public List<RectRotate> rectRotates = new List<RectRotate>();
   
        String[] sSplit;
       
    
     
      
   
      
        List<RectRotate> boxList = new List<RectRotate>();
        List<float> scoreList = new List<float>();
        List<string> labelList = new List<string>();
        public int IndexThread = 0;
        public float CropOffSetX, CropOffSetY=0;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<float> list_AngleCenter = new List<float>();
        public List<double> listScore = new List<double>();
        public bool IsLimitCouter = true;
        public void ScanOCR()
        {

            using (Py.GIL())
            {
                PyObject result = null;


                try
                {


                    // === Crop ROI ===
                    using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexThread].matRaw, rotCrop, null))
                    {
                        if (matCrop.Empty()) return;

                        if (matCrop.Type().Depth != MatType.CV_8U)
                        {
                            using (var tmp8u = new Mat())
                            {
                                Cv2.ConvertScaleAbs(matCrop, tmp8u); // 16U/32F -> 8U
                                matCrop.AssignTo(tmp8u);             // ghi đè dữ liệu (OpenCvSharp: AssignTo giữ shape/type mới)
                            }
                        }

                        // 2) đảm bảo đúng số kênh
                        if (matCrop.Channels() == 1)
                        {
                            Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                        }
                        else if (matCrop.Channels() == 4)
                        {
                            Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGRA2BGR);
                        }

                        int h = matCrop.Rows, w = matCrop.Cols, c = matCrop.Channels();
                        int stride = (int)matCrop.Step();
                        long addr = matCrop.Data.ToInt64();

                        using (Py.GIL())
                        {
                            result = G.objCraftOCR.predict_from_pointer(addr, h, w, c, stride);
                            rectRotates = Converts.PyToRectRotates(result["payloads"]);
                        }

                        // đảm bảo matCrop còn sống trong suốt thời gian Python dùng p
                        GC.KeepAlive(matCrop);
                    }
                }
                catch (PythonException pyEx)
                {
                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", pyEx.ToString()));
                }
                catch (Exception ex)
                {
                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.ToString()));
                }
                finally
                {

                    if (result != null) result.Dispose();
                }
            }


            listRotScan = new List<RectRotate>(); ;
            foreach (RectRotate rot in rectRotates)
            {
                PointF pCenter = rot._PosCenter;
                Matrix mat = new Matrix();
                System.Drawing.Point pZero = new System.Drawing.Point(0, 0);
                PointF[] pMatrix = { pZero };
                mat.Translate(rotCrop._PosCenter.X, rotCrop._PosCenter.Y);
                mat.Rotate(rotCrop._rectRotation);
                mat.Translate(rotCrop._rect.X, rotCrop._rect.Y);
                mat.Translate(pCenter.X, pCenter.Y);
                mat.Rotate(rot._rectRotation);
                mat.TransformPoints(pMatrix);
                int x = (int)pMatrix[0].X;// (int)rotArea._PosCenter.X -(int) rotArea ._rect.Width/2 + (int)rot._PosCenter.X;
                int y = (int)pMatrix[0].Y; ;// (int)rotArea._PosCenter.Y - (int)rotArea._rect.Height / 2 + (int)rot._PosCenter.Y;
              
                PointF p = new System.Drawing.PointF(x, y);
                RectRotate rect = new RectRotate(rot._rect, p, rotCrop._rectRotation + rot._rectRotation, AnchorPoint.None);
                rect.Shape = ShapeType.Polygon;
                rect.PolyLocalPoints = rot.PolyLocalPoints;
                listRotScan.Add(rect);

            }
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.Done;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Check;
        }
        public void DoWork(RectRotate rectRotate)
        {
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            // 5) Gom kết quả
            rectRotates = new List<RectRotate>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty()) return;


                Mat matProcess = null;
                byte[] rentedBuffer = null;

                try
                {
                    if (raw.Type() == MatType.CV_8UC3)
                    {
                        matProcess = new Mat();
                        Cv2.CvtColor(raw, matProcess, ColorConversionCodes.BGR2GRAY);
                    }
                    else
                    {
                        matProcess = raw; // reuse backing store
                    }
                    var rrCli = Converts.ToCli(rectRotate); // như ở reply trước
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                    Pattern.SetImgeRaw(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels(), rrCli, rrMaskCli);


                    // 3) Nạp ảnh vào Pattern (con trỏ phải còn sống đến sau khi Match)
                    GC.KeepAlive(matProcess);

                    var listRS = Pattern.Match(
                         false,                 // m_bStopLayer1
                         0,                           // m_dToleranceAngle (bỏ, vì bạn dùng range dưới)
                         -10,                  // m_dTolerance1
                         10,                   // m_dTolerance2
                         Common.PropetyTools[IndexThread][Index].Score / 100.0, // m_dScore
                         true,                      // m_ckSIMD
                         false,                // m_ckBitwiseNot
                         true,                  // m_bSubPixel
                         1,                   // m_iMaxPos
                         0,                     // m_dMaxOverlap
                         false,              // useMultiThread  <-- MỚI
                         -1                   // numThreads      <-- MỚI
                     );



                    float scoreSum = 0f;

                    if (listRS != null)
                    {
                        foreach (Rotaterectangle rot in listRS)
                        {
                            float w = (float)rot.Width;
                            float h = (float)rot.Height;
                            var pCenter = new System.Drawing.PointF((float)rot.Cx, (float)rot.Cy);
                            float angle = (float)rot.AngleDeg;
                            float score = (float)rot.Score;
                            scoreSum += score;

                            rectRotates.Add(new RectRotate(
                                new System.Drawing.RectangleF(-w / 2f, -h / 2f, w, h),
                                pCenter, angle, AnchorPoint.None));

                            listScore.Add(Math.Round(score, 1));
                            list_AngleCenter.Add(rotArea._rectRotation + angle);
                            listP_Center.Add(new System.Drawing.Point(
                                (int)(rectRotate._PosCenter.X - rectRotate._rect.Width / 2f + pCenter.X),
                                (int)(rectRotate._PosCenter.Y - rectRotate._rect.Height / 2f + pCenter.Y)));
                        }
                    }

                    if (scoreSum != 0 && rectRotates.Count > 0)
                    {
                        Common.PropetyTools[Global.IndexChoose][Index].ScoreResult =
                            (int)Math.Round(scoreSum / rectRotates.Count, 1);
                    }
                }
                finally
                {
                    if (rentedBuffer != null)
                        System.Buffers.ArrayPool<byte>.Shared.Return(rentedBuffer);

                    if (matProcess != null)
                        matProcess.Dispose();


                }
            }
        }
        public int LimitCounter = 0;
        public void Complete()
        {
            
            if (rectRotates.Count() ==0)
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            else
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
         
           

        }

        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        public bool IsSendResult;
        public ArrangeBox ArrangeBox=new ArrangeBox();
        public bool IsArrangeBox = false;
        public async Task SendResult()
        {
            if (IsSendResult)
            {
               if( Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                  await  Global.ParaCommon.Comunication.Protocol.WriteResultBits(AddPLC, BitsResult);
                }
            }
        }
        bool[] BitsResult=new bool[16];
        static bool IntersectX(RectRotate r, float valueX)
        {
            float w = r._rect.Width, h = r._rect.Height;
            double th = r._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(th), s = Math.Sin(th);

            float extX = (float)(Math.Abs(c) * (w * 0.5) + Math.Abs(s) * (h * 0.5));
            float minX = r._PosCenter.X - extX;
            float maxX = r._PosCenter.X + extX;

            return maxX>=valueX ;   // cắt đường x = valueX
            return maxX>=valueX ;   // cắt đường x = valueX
        }

        static bool IntersectY(RectRotate r, float valueY)
        {
            float w = r._rect.Width, h = r._rect.Height;
            double th = r._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(th), s = Math.Sin(th);

            float extY = (float)(Math.Abs(s) * (w * 0.5) + Math.Abs(c) * (h * 0.5));
            float minY = r._PosCenter.Y - extY;
            float maxY = r._PosCenter.Y + extY;

            return maxY>=valueY ;   // cắt đường y = valueY
        }
    


        public Graphics DrawResult(Graphics gc)
        {
            if (rotAreaAdjustment == null && Global.IsRun) return gc;
            gc.ResetTransform();
            RectRotate rotA = rotArea;
            if (Global.IsRun) rotA = rotAreaAdjustment;
            if (IsScanOCR)
            {
               
                rotA = rotCrop;
                
            }
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
                    cl =  Global.Config.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.Config.ColorNG;
                    break;
            }
          
                Pen pen = new Pen(cl, Global.Config.ThicknessLine);
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox&&!IsScanOCR)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl,  Global.Config.ThicknessLine);
            int i = 0;
            if (IsScanOCR)
            {
              
                cl = Global.Config.ColorNone;
            }

            gc.ResetTransform();
            if(IsScanOCR)
            {
                foreach (RectRotate rot in rectRotates)
                {

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

                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                    // mat.Translate(rot._rect.X, rot._rect.Y);
                    gc.Transform = mat;
                    if(rot.PolyLocalPoints.Count>0)
                    gc.DrawPolygon(pen, rot.PolyLocalPoints.ToArray());

                    gc.ResetTransform();

                }
                IsScanOCR = false;
            }
            else
            {
                i = 0;
                foreach (RectRotate rot in rectRotates)
                {

                    
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

                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                     mat.Translate(rot._rect.X, rot._rect.Y);
                    gc.Transform = mat;
                    Rectangle rect = new Rectangle(0, 0, (int)rot._rect.Width, (int)rot._rect.Height);
                    gc.DrawRectangle(pen, rect);
                  
                    gc.ResetTransform();
                    i++;
                }
            }    
               

         
            return gc;
        }

    }
}
