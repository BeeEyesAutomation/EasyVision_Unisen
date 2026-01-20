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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
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

        [NonSerialized]
        public bool IsNew = false;
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
        public ModeCheck ModeCheck = ModeCheck.Single;
        public int IndexChoose = 0;
        public int _OffSetArea = 30;
        public bool Is1D;
        public void UpdateOffSet()
        {
            if (rotTemp != null)
            {
                int w = OffSetArea, h = OffSetArea;
                if (Is1D)
                    h = h * 4;

                List<PointF> points = PolyOffset.OffsetAxisPercent(rotTemp.PolyLocalPoints, w, h);
                RectRotate rectRotate = new RectRotate(rotTemp._rect, rotTemp._PosCenter, rotTemp._rectRotation, rotTemp._dragAnchor);
                rectRotate.Shape = ShapeType.Polygon;

                rectRotate.PolyLocalPoints = points;
                rectRotate.IsPolygonClosed = true;
                rotArea = rectRotate.Clone();
                rotArea.UpdateFromPolygon(false);
            }
        }
        public int OffSetArea
        {
            get
            {
                return _OffSetArea;
            }
            set
            {
                _OffSetArea = value;
               
            }
        }
        
        public void SetTemp( RectRotate rot)
        {
            int w = OffSetArea,h = OffSetArea;
                if (IndexChoose< listTypeBarcode.Count())
                {
                    Is1D = listTypeBarcode[IndexChoose].Is1D();
                }    
            if (Is1D)
                h = h * 4;
            rotTemp = rot.Clone();
            List<PointF>  points = PolyOffset.OffsetAxisPercent(rot.PolyLocalPoints,w,h);
            RectRotate rectRotate2 = new RectRotate(rot._rect, rot._PosCenter, rot._rectRotation, rot._dragAnchor);
            rectRotate2.Shape = ShapeType.Polygon;
            rectRotate2.AutoExpandBounds = true;
            rectRotate2.AutoOrientPolygon = true;
            rectRotate2.PolyLocalPoints = points;
            rectRotate2.IsPolygonClosed = true;
            rotArea = rectRotate2.Clone();
            rotArea._dragAnchor=AnchorPoint.None;
            rotArea.UpdateFromPolygon(false);
            ListTempBarcode = new List<string>();
            ListTempBarcode.Add(rot.Name);
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                matTemp = Cropper.CropRotatedRect(raw,rot, null);
              
                bmRaw = matTemp.ToBitmap();
               
                
            }
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.Done;
        }
        public void AutoTemp(List< RectRotate> rots)
        {
            if (ModeCheck == ModeCheck.Single)
            {
                ListTempBarcode = new List<string>();
                ListTempBarcode.Add(rots[0].Name);
            }
            else
            {
                using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
                {
                    ListTempBarcode = new List<string>();
                    List<Bitmap> bitmaps = new List<Bitmap>();
                    int i = 0;
                    List<RectRotate> listNoneArrange = new List<RectRotate>();
                    foreach (RectRotate rot in rots)
                    {
                        
                        PointF pCenter = rot._PosCenter;
                        Matrix mat = new Matrix();
                        System.Drawing.Point pZero = new System.Drawing.Point(0, 0);
                        PointF[] pMatrix = { pZero };
                        mat.Translate(rotAreaAdjustment._PosCenter.X, rotAreaAdjustment._PosCenter.Y);
                        mat.Rotate(rotAreaAdjustment._rectRotation);
                        mat.Translate(rotAreaAdjustment._rect.X, rotAreaAdjustment._rect.Y);
                        mat.Translate(pCenter.X, pCenter.Y);
                        mat.Rotate(rot._rectRotation);
                        mat.TransformPoints(pMatrix);
                        int x = (int)pMatrix[0].X;
                        int y = (int)pMatrix[0].Y; 

                        PointF p = new System.Drawing.PointF(x, y);
                        RectRotate rect = new RectRotate(rot._rect, p, rotAreaAdjustment._rectRotation + rot._rectRotation, AnchorPoint.None);
                        rect.Shape = ShapeType.Polygon;
                        rect.PolyLocalPoints = rot.PolyLocalPoints;
                        rect.IsPolygonClosed = true;
                        rect.AutoExpandBounds = true;
                        rect.AutoOrientPolygon = true;
                        rect.UpdateFromPolygon(false);
                        rect.Name = rot.Name;
                        rect.TypeValue = rot.TypeValue;
                        listNoneArrange.Add(rect.Clone());
                        i++;

                    }
                    List<RectRotate> combined = new List<RectRotate>();

                    for (int j = 0; j < listNoneArrange.Count; j++)
                    {
                        combined.Add(listNoneArrange[j]);
                    }
                    combined = combined.OrderBy(b => b._PosCenter.X).ToList();
                    listNoneArrange = combined.Select(b => b).ToList();
                    foreach (RectRotate rot in listNoneArrange)
                    {
                        if (rot._dragAnchor == AnchorPoint.Center)
                        {
                            ListTempBarcode.Add(rot.Name.ToString());
                           
                            bitmaps.Add(Cropper.CropRotatedRect(raw, rot, null).ToBitmap());
                        }
                    }
                    if (bitmaps.Count > 0)
                    {
                        var merged = ImageUtils73.StitchVerticalWithCaptions(
                            bitmaps: bitmaps,
                            captions: ListTempBarcode,
                            space: 5,
                            captionImageGap: 6,
                            font: new Font("Segoe UI", 11f, FontStyle.Regular),
                            textColor: Color.Black,
                            background: Color.White,   // hoặc null để trong suốt
                            align: HAlign.Left,        // CĂN TRÁI
                            paddingLeft: 8,
                            paddingRight: 8
                        );
                        bmRaw = merged;
                    }
                }
            }

        }
        public List<String> ListTempBarcode = new List<string>();
        public void SetMulTemp()
        {
            List<Bitmap> bitmaps = new List<Bitmap>();
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                rotArea = rotCrop.Clone();
                rotArea._dragAnchor = AnchorPoint.None;
                ListTempBarcode = new List<string>();

                foreach (RectRotate rot in listRotScan)
                {
                    if (rot._dragAnchor == AnchorPoint.Center)
                    {
                        ListTempBarcode.Add(rot.Name.ToString());
                        bitmaps.Add(Cropper.CropRotatedRect(raw, rot, null).ToBitmap());
                    }
                }

                if(bitmaps.Count>0)
                {
                    var merged = ImageUtils73.StitchVerticalWithCaptions(
                        bitmaps: bitmaps,
                        captions: ListTempBarcode,
                        space: 5,
                        captionImageGap: 6,
                        font: new Font("Segoe UI", 11f, FontStyle.Regular),
                        textColor: Color.Black,
                        background: Color.White,   // hoặc null để trong suốt
                        align: HAlign.Left,        // CĂN TRÁI
                        paddingLeft: 8,
                        paddingRight: 8
                    );
                   
                    // Nếu muốn căn trái:
                    //  Bitmap mergedLeft = ImageUtils.StitchVertical(bitmaps, 5, null, false);

                    bmRaw = merged;
                }    
              
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
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
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
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
        public int IndexCCD = 0;
        public void Scan()
        {

            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            // 5) Gom kết quả
            rectRotates = new List<RectRotate>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
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
                    var rrCli = Converts.ToCli(rotCrop); // như ở reply trước
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                     var cli = new BarcodeCoreCli();
                    var opts = DetectOptionsCli.Defaults();
                    opts.FindBoxes = true;                      // hoặc false nếu muốn ZXing trực tiếp trên ROI
                                                                //opts.Filters.MinArea = 500;                 // tuỳ môi trường
                                                                //opts.Filters.MinAspectRatio = 2;
                                                                //  var opts = DetectOptionsCli.Defaults();
                                                                // opts.FindBoxes = true;
                    BarcodeCoreCli.DetectAll(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels(), rrCli, rrMaskCli,opts, ref listRectBarcode, ref listContentBarcode, ref listTypeBarcode);


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
                            rect.PolyLocalPoints = poly;
                            rect.IsPolygonClosed = true;
                            rect.AutoExpandBounds = true;
                            rect.UpdateFromPolygon(false);
                            rectRotates.Add(rect);
                            list_AngleCenter.Add(rotArea._rectRotation + angle);
                            listP_Center.Add(new System.Drawing.Point(
                                (int)(rotCrop._PosCenter.X - rotCrop._rect.Width / 2f + pCenter.X),
                                (int)(rotCrop._PosCenter.Y - rotCrop._rect.Height / 2f + pCenter.Y)));
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
            listRotScan = new List<RectRotate>();
            int i = 0;
            foreach (RectRotate rot in rectRotates)
            {
                rot.Name = listContentBarcode[i];
                rot.TypeValue = (int)listTypeBarcode[i];
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
                int x = (int)pMatrix[0].X;
                int y = (int)pMatrix[0].Y; ;
              
                PointF p = new System.Drawing.PointF(x, y);
                RectRotate rect = new RectRotate(rot._rect, p, rotCrop._rectRotation + rot._rectRotation, AnchorPoint.None);
                rect.Shape = ShapeType.Polygon;
                rect.PolyLocalPoints = rot.PolyLocalPoints;
                rect.IsPolygonClosed = true;
                rect.AutoExpandBounds = true;
                rect.AutoOrientPolygon = true;
                rect.UpdateFromPolygon(false);
                rect.Name = rot.Name;
                rect.TypeValue = rot.TypeValue;
                listRotScan.Add(rect.Clone());
                i++;

            }
            List<RectRotate> combined = new List<RectRotate>();

            for (int j = 0; j < listRotScan.Count; j++)
            {
                combined.Add(listRotScan[j]);
            }
            combined = combined.OrderBy(b => b._PosCenter.X).ToList();
            listRotScan = combined.Select(b => b).ToList();
            combined = new List<RectRotate>();

            for (int j = 0; j < rectRotates.Count; j++)
            {
                combined.Add(rectRotates[j]);
            }
            combined = combined.OrderBy(b => b._PosCenter.X).ToList();
            rectRotates = combined.Select(b => b).ToList();
            if (ModeCheck==ModeCheck.Single&& listRotScan.Count()>0)
            {
                if(IndexChoose>= listRotScan.Count()) IndexChoose = 0;

                listRotScan[IndexChoose]._dragAnchor = AnchorPoint.Center;
                SetTemp(listRotScan[IndexChoose]);
            }
         
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.Done;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Scan;
        }
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            // 5) Gom kết quả
            rectRotates = new List<RectRotate>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
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
                    var rrCli = Converts.ToCli(rotArea); // như ở reply trước
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
                    var cli = new BarcodeCoreCli();
                    var opts = DetectOptionsCli.Defaults();
                    opts.FindBoxes = false;// ModeCheck== ModeCheck.Single? false:true;
                    BarcodeCoreCli.DetectAll(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels(), rrCli, rrMaskCli,opts, ref listRectBarcode,ref listContentBarcode,ref listTypeBarcode);


                    // 3) Nạp ảnh vào Pattern (con trỏ phải còn sống đến sau khi Match)
                    GC.KeepAlive(matProcess);

                



                    float scoreSum = 0f;

                    if (listRectBarcode != null)
                    {
                        int i = 0;
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
                            rect.Name = listContentBarcode[i];
                            rect.TypeValue =(int) listTypeBarcode[i];
                            rectRotates.Add(rect);
                            list_AngleCenter.Add(rotArea._rectRotation + angle);
                            listP_Center.Add(new System.Drawing.Point(
                                (int)(rotArea._PosCenter.X - rotArea._rect.Width / 2f + pCenter.X),
                                (int)(rotArea._PosCenter.Y - rotArea._rect.Height / 2f + pCenter.Y)));
                            i++; 
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
            if (IsScan)
            {
                IsScan = false;
                
            }
            if(!Global.IsRun&&ModeCheck==ModeCheck.Single&&rectRotates.Count>0)
            {
                ListTempBarcode = new List<string>();
                ListTempBarcode.Add(rectRotates[0].Name);
            }
            if(rectRotates.Count()==0)
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            else
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            //foreach (String s in ListTempBarcode)
            //{
            //    int index = rectRotates.FindIndex(a => a.Name.Contains(s));
            //   if(index < 0)
            //        Common.PropetyTools[IndexThread][Index].Results = Results.NG;

            //}
              
                if (Global.IsAutoTemp)
                {
                if (rectRotates.Count() > 0)
                    AutoTemp(rectRotates);
                DoWork(rotAreaAdjustment,rotMaskAdjustment);
                //if (rectRotates.Count() > 0)
                //{

                //    ListTempBarcode = new List<string>();
                //    ListTempBarcode.Add(rectRotates[0].Name);
                //}
                if (rectRotates.Count() == 0)
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                else
                    Common.PropetyTools[IndexThread][Index].Results = Results.OK;
                //foreach (String s in ListTempBarcode)
                //{
                //    int index = rectRotates.FindIndex(a => a.Name.Contains(s));
                //    if (index < 0)
                //        Common.PropetyTools[IndexThread][Index].Results = Results.NG;

                //}

            }

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
               if( Global.Comunication.Protocol.IsConnected)
                {
                //  await  Global.Comunication.Protocol.WriteResultBits(AddPLC, BitsResult);
                }
            }
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
                    cl =  Global.ParaShow.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ParaShow.ColorNG;
                    break;
            }
          
                Pen pen = new Pen(cl, Global.ParaShow.ThicknessLine);
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (ModeCheck == ModeCheck.Single&&!IsScan)
                Draws.DrawRectRotate(gc, rotA,new Pen(cl, Global.ParaShow.ThicknessLine));
            else  if (Global.ParaShow.IsShowBox || IsScan)
                Draws.Box1Label(gc, rotA, nameTool, font, new SolidBrush(Global.ParaShow.TextColor), cl, Global.ParaShow.ThicknessLine);

            int i = 0;
           

            gc.ResetTransform();
          
                i = 0;
                foreach (RectRotate rot in rectRotates)
                {
                if (IsScan)
                {
                    if (listRotScan[i]._dragAnchor == AnchorPoint.Center)
                    {
                     
                      
                        cl = Global.ParaShow.ColorChoose;
                    }    
                        
                    else
                        cl = Global.ParaShow.ColorNone;
                }
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

                Draws.Box1Label(gc, rot,((CodeSymbologyCli)( rot.TypeValue)).ToString()+": "+ rot.Name, font, brushText, cl,  Global.ParaShow.ThicknessLine);
                gc.ResetTransform();
                i++;
                 }
          
              
            
            
               

         
            return gc;
        }

    }
}
