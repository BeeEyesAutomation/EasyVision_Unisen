using BeeCore.Algorithm;
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
    public class Barcode
    {
      
     

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        
     
        public String pathFullModel = "";
        [NonSerialized]
        public Mat matTemp;
        [NonSerialized]
        public BeeCpp.BarcodeCoreCli BarcodeCoreCli = new BarcodeCoreCli();
        [NonSerialized]
        public List<RectRotateCli> listRectBarcode;
        [NonSerialized]
        public List<String> listContentBarcode;
        [NonSerialized]
        public List<CodeSymbologyCli> listTypeBarcode;

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

          
            List<PointF>  points = PolyOffset.OffsetRadial(rot.PolyLocalPoints, OffSetArea);
            RectRotate rectRotate2 = new RectRotate(rot._rect, rot._PosCenter, rot._rectRotation, rot._dragAnchor);
            rectRotate2.Shape = ShapeType.Polygon;
            rectRotate2.AutoExpandBounds = true;
            rectRotate2.AutoOrientPolygon = true;
            rectRotate2.PolyLocalPoints = points;
            rectRotate2.IsPolygonClosed = true;
            rotArea = rectRotate2.Clone();
            rotArea._dragAnchor=AnchorPoint.None;
            rotArea.UpdateFromPolygon(false);
            rotTemp=rotArea.Clone();
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                matTemp = Cropper.CropRotatedRect(raw,rot, null);
              
                bmRaw = matTemp.ToBitmap();
               
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.Done;
            }

        }

        public  void SetModel()
        {
            if (rotCrop == null)
                rotCrop = new RectRotate();
            if (rotArea == null)
                rotArea = new RectRotate();
         
            BarcodeCoreCli = new BarcodeCoreCli();
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
          

            
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;

            // G.YoloPlus.LoadModel(nameTool, nameModel, (int)TypeYolo);
        }

        public int Index = -1;
      
        public RectRotate rotArea, rotCrop,rotTemp, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();

        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public List<RectRotate> listRotScan = new List<RectRotate>();
        public Bitmap bmRaw;

       
        public Compares Compare = Compares.Equal;
        public Compares CompareLine = Compares.More;
        public Compares CompareArea = Compares.More;
        public float LimitArea = 100;

        public bool IsScan = false;

        public TypeCrop TypeCrop;


        [NonSerialized]
        public List<RectRotate> rectRotates = new List<RectRotate>();
   
        public int IndexThread = 0;
        public float CropOffSetX, CropOffSetY=0;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<float> list_AngleCenter = new List<float>();
        public List<double> listScore = new List<double>();

        public void Scan()
        {

            DoWork(rotCrop);
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
                rect.IsPolygonClosed = true;
                rect.AutoExpandBounds = true;
                rect.AutoOrientPolygon = true;
                rect.UpdateFromPolygon(false);

                listRotScan.Add(rect.Clone());

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

                    BarcodeCoreCli.DetectAllWithCorners(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels(), rrCli, rrMaskCli, ref listRectBarcode,ref listContentBarcode,ref listTypeBarcode);


                    // 3) Nạp ảnh vào Pattern (con trỏ phải còn sống đến sau khi Match)
                    GC.KeepAlive(matProcess);

                



                    float scoreSum = 0f;

                    if (listRectBarcode != null)
                    {
                        foreach (RectRotateCli rot in listRectBarcode)
                        {
                            float w = (float)rot.RectWH.Width;
                            float h = (float)rot.RectWH.Height;
                            var pCenter = new System.Drawing.PointF((float)rot.PosCenter.X, (float)rot.PosCenter.Y);
                            float angle = (float)rot.RectRotationDeg;

                            RectRotate rect = new RectRotate(
                                new System.Drawing.RectangleF(-w / 2f, -h / 2f, w, h),
                                pCenter, angle, AnchorPoint.None);
                            rect.Shape = ShapeType.Polygon;
                            List<PointF> poly = rot.PolyLocalPoints?
                            .Select(p => new PointF(p.X, p.Y))
                            .ToList();
                            rect.IsPolygonClosed = true;
                            rect.PolyLocalPoints = poly;
                            rectRotates.Add(rect);
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
            if (IsScan)
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
            if (Global.Config.IsShowBox&&!IsScan)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.Config.ThicknessLine);
            int i = 0;
            if (IsScan)
            {
                IsScan = false;
                cl = Global.Config.ColorNone;
            }

            gc.ResetTransform();
          
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
                   // mat.Translate(rot._rect.X, rot._rect.Y);
                    gc.Transform = mat;
                    //if (rot.PolyLocalPoints.Count > 0)
                    //    gc.DrawPolygon(pen, rot.PolyLocalPoints.ToArray());

                Draws.Box1Label(gc, rot, listTypeBarcode[i].ToString()+": "+ listContentBarcode[i], font, brushText, cl,  Global.Config.ThicknessLine);
                gc.ResetTransform();
                i++;
                 }
          
              
            
            
               

         
            return gc;
        }

    }
}
