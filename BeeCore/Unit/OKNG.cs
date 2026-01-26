using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using CvPlus;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
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
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class OKNG
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        [NonSerialized]
        public bool IsNew = false;
        public bool IsIni = false;
        public int Index = -1;
        public int IndexCCD = 0;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp,matMask;
        public List<Point> Postion=new List<Point>();
      
        public List<double> listScore = new List<double>();
     
        int _NumObject = 0;
        public int NumObject
        {
            get
            {
                return _NumObject;
            }
            set
            {
                _NumObject = value;
               
            }
        }
         bool isHighSpeed=false;
        public bool IsHighSpeed {
            get
            {
                return isHighSpeed;
            }
            set
            {
                isHighSpeed = value;
              
            }
        }

        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public int cycleTime = 0;
        public RectangleF rectArea;
        public Compares Compare = Compares.Equal;
        public int LimitCounter = 0;
       
       
       
     
      
       
        private int numOK;
        
        public List< System.Drawing.Point > listP_Center=new List<System.Drawing.Point>();
    
    
        public List<Bitmap> bmOK=new List<Bitmap>();
        public List<Bitmap> bmNG = new List<Bitmap>();
        public OKNG()
        {
          
            oKNGHandle = new OKNGHandle();

            //// ===== Detect =====
            //Mat scene = Cv2.ImRead("OKNG//check1.png");
            //if (OKNGAPI.DetectFromMat(OKNGapi, scene, out int label, out float score,
            //                       out int modelId, out int x, out int y, out int w, out int hgt))
            //{
            //    Console.WriteLine($"Detected: {(label > 0 ? "OK" : "NG")}, score={score:F3}, modelId={modelId}");
            //    Console.WriteLine($"Location: ({x},{y}), size=({w}x{hgt})");
            //}
            //Cv2.PutText(scene, label.ToString(), new OpenCvSharp.Point(x, y), HersheyFonts.HersheySimplex, 2, Scalar.Red);
            //Cv2.Rectangle(scene, new Rect(x, y, w, hgt), Scalar.Red, 3, LineTypes.Link4);
            //Cv2.ImShow("rs", scene);
            //OKNGAPI.OKNG_Destroy(OKNGapi);
        }


      
     
       
        public int numTempOK;
    
        public int NumOK { get => numOK; set => numOK = value; }
        public int _MinArea;
        public int MinArea { get => _MinArea; set {

                _MinArea = value;
                OKNGAPI.OKNG_SetEdgeSpeckleMinArea(oKNGHandle.Handle, _MinArea);   // xoá blob nhiễu < 50 px (tuỳ ảnh, 30..120)
            }
        }
        public bool _MultiScaleCanny;
        public bool MultiScaleCanny
        {
            get => _MultiScaleCanny; set
            {

                _MultiScaleCanny = value;
                OKNGAPI.OKNG_EnableMultiScaleCanny(oKNGHandle.Handle, Convert.ToInt32( _MultiScaleCanny));   // xoá blob nhiễu < 50 px (tuỳ ảnh, 30..120)
            }
        }
        [NonSerialized]
        private OKNGHandle oKNGHandle = new OKNGHandle();
        public void SetSample()
        {// cấu hình nâng cao:

            //OKNGapi = OKNGAPI.OKNG_Create();
            //// vẫn set threshold như trước:
            //float Score = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);
            //OKNGAPI.OKNG_SetMatchThreshold(OKNGapi, Score);
           
            oKNGHandle.SetMatchMode(1);
            oKNGHandle.SetIntensityMultiScale(false, 0.8f, 1.25f, 0.05f); // scale 0.8 → 1.25 step 0.05
            oKNGHandle.SetIntensityRotation(false, 10f, 2f);              // xoay ±10° step 2°
            oKNGHandle.SetThreshold(0.75f); // chỉ áp dụng cho detect NG→OK (không áp dụng Nearest)

            // vẫn set threshold như trước:
            float Score = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);
            OKNGAPI.OKNG_SetMatchThreshold(oKNGHandle.Handle, 0.8f);
            OKNGAPI.OKNG_RemoveAllByLabel(oKNGHandle.Handle, -1);
            OKNGAPI.OKNG_RemoveAllByLabel(oKNGHandle.Handle, +1);
            foreach (Bitmap bm in bmOK)
            {
                Mat m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bm);
                int id = OKNGAPI.LearnFromMat(oKNGHandle.Handle, m, +1); // label +1 = OK
                Console.WriteLine($"Learned OK ID={id}");
            }
            foreach (Bitmap bm in bmNG)
            {
                Mat m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bm);
                int id = OKNGAPI.LearnFromMat(oKNGHandle.Handle, m, -1); // label -1 = NG
                Console.WriteLine($"Learned OK ID={id}");
            }
            SaveModel();
        }
        public void AddOK()
        {
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {

                if (raw.Empty()) return;
               
                Mat matCrop = Cropper.CropRotatedRect(raw, rotCrop,null);
                Cv2.ImWrite("CropOK.png", matCrop);
                bmOK.Add(matCrop.ToBitmap());
             }
         
            
        }
        public void AddNG()
        {
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {

                if (raw.Empty()) return;

                Mat matCrop = Cropper.CropRotatedRect(raw, rotCrop, null);
                bmNG.Add(matCrop.ToBitmap());
            }


        }

        public void RemoteOK()
        {
            if(bmOK.Count == 0) return;
            bmOK.RemoveAt(bmOK.Count - 1);
          
        }
        public void RemoveNG()
        {
            if (bmNG.Count == 0) return;
            bmNG.RemoveAt(bmNG.Count - 1);

        }
        public void RemoveAll()
        {
            OKNGAPI.OKNG_RemoveAllByLabel(oKNGHandle.Handle, -1);
            OKNGAPI.OKNG_RemoveAllByLabel(oKNGHandle.Handle, +1);
        }
        public void RemoveAllOK()
        {
          
            OKNGAPI.OKNG_RemoveAllByLabel(oKNGHandle.Handle, +1);
            bmOK = new List<Bitmap>();
        }
        public void RemoveAllNG()
        {
            OKNGAPI.OKNG_RemoveAllByLabel(oKNGHandle.Handle, -1);
            bmNG = new List<Bitmap>();

        }
        public void SaveModel()
        {
            if (!Directory.Exists("Program\\" + Global.Project + "\\" + Common.PropetyTools[IndexThread][Index].Name))
                Directory.CreateDirectory("Program\\" + Global.Project + "\\" + Common.PropetyTools[IndexThread][Index].Name);
            OKNGAPI.OKNG_SaveModels(oKNGHandle.Handle, "Program\\" + Global.Project + "\\" + Common.PropetyTools[IndexThread][Index].Name+"\\data.yaml");
        }
        [NonSerialized]
        private bool IsLoadModel = false;
        public bool _EnResize = true;
        public bool EnResize
        {
            get => _EnResize; set
            {
                _EnResize = value;

                oKNGHandle.SetWorkingResize(EnResize, (float)(ScaleResize / 100.0));
            }
        }
        public float _ScaleResize;
        public float ScaleResize { get => _ScaleResize; set
            {
                _ScaleResize = value;
                
                oKNGHandle.SetWorkingResize(EnResize, (float)( _ScaleResize/100.0));
            }
        }
        public bool _Multi = true;
        public bool Multi
        {
            get => _Multi; set
            {
                _Multi = value;
                OKNGAPI.OKNG_SetOMPThreadCount(oKNGHandle.Handle, numCPU);
                OKNGAPI.OKNG_SetUseOMP(oKNGHandle.Handle, Convert.ToInt32(Multi));
            }
        }
        public int _numCPU = 1;
        public int numCPU
        {
            get => _numCPU; set
            {
                if (oKNGHandle == null) oKNGHandle = new OKNGHandle();
                _numCPU = value;
                OKNGAPI.OKNG_SetOMPThreadCount(oKNGHandle.Handle, numCPU);
                OKNGAPI.OKNG_SetUseOMP(oKNGHandle.Handle, Convert.ToInt32(Multi));
            }
        }
     
        public void LoadModel()
        {
            if (OKNGAPI.OKNG_LoadModels(oKNGHandle.Handle, "Program\\" + Global.Project + "\\" + Common.PropetyTools[IndexThread][Index].Name + "\\data.yaml") == 0)
                IsLoadModel = false;
            else IsLoadModel = true;
        }
        
        public void SetModel()
        {if (oKNGHandle == null)
                oKNGHandle = new OKNGHandle();

            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotArea == null) rotArea = new RectRotate();
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
			Common.PropetyTools[IndexThread][Index].MinValue = 0;

            Common.PropetyTools[IndexThread][Index].MaxValue = 100;


            oKNGHandle.SetWorkingResize(EnResize, (float)(_ScaleResize / 100.0));
            oKNGHandle.SetMatchMode(1);
            oKNGHandle.SetIntensityMultiScale(false, 0.8f, 1.25f, 0.05f); // scale 0.8 → 1.25 step 0.05
            oKNGHandle.SetIntensityRotation(false, 10f, 2f);              // xoay ±10° step 2°
            oKNGHandle.SetThreshold(0.75f); // chỉ áp dụng cho detect NG→OK (không áp dụng Nearest)
                                            // ===== OpenMP =====
            OKNGAPI.OKNG_SetOMPDynamic(oKNGHandle.Handle, 0); // tắt dynamic để cố định số luồng
            OKNGAPI.OKNG_SetOMPThreadCount(oKNGHandle.Handle, numCPU);
            OKNGAPI.OKNG_SetUseOMP(oKNGHandle.Handle,Convert.ToInt32( Multi));
           
            //OKNGAPI.OKNG_EnableMultiScaleCanny(oKNGHandle.Handle, Convert.ToInt32(_MultiScaleCanny));    // bật multi-scale
            if (bmNG == null) bmNG = new List<Bitmap>();
            if (bmOK == null) bmOK = new List<Bitmap>();
            LoadModel();
            //// ===== Học nhiều mẫu NG =====
            //foreach (Bitmap bm in bmOK)
            //{
            //    Mat m =OpenCvSharp.Extensions.BitmapConverter.ToMat( bm);
            //    int id = OKNGAPI.LearnFromMat(OKNGapi, m, +1); // label +1 = OK

            //}
            //foreach (Bitmap bm in bmNG)
            //{
            //    Mat m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bm);
            //    int id = OKNGAPI.LearnFromMat(OKNGapi, m, -1); // label -1 = NG

            //}


            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public List<RectRotate> rectRotates = new List<RectRotate>();
        public List<String> listLabel = new List<String>();
        public bool IsLimitCouter = true;
        public float ScoreOK, ScoreNG;
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
           
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {

                if (raw.Empty()) return;

                Mat matCrop = Cropper.CropRotatedRect(raw, rotArea, null);
                rectRotates = new List<RectRotate>();
                listScore = new List<double>();
                listP_Center = new List<System.Drawing.Point>(); 
                listLabel = new List<string>();
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
           //     Cv2.ImWrite("Crop.png", matCrop);
                // Nearest 2: best OK vs best NG (bỏ ngưỡng)
                if (OKNGAPI.BestPerLabelFromMat(oKNGHandle.Handle, matCrop,
                       out int okId, out float okScore, out int ngId, out float ngScore))
                {
                    ScoreOK =(float)Math.Round( okScore*100);
                    ScoreNG = (float)Math.Round(ngScore*100);
                    if (okScore>=ngScore)
                    {
                        Common.PropetyTools[IndexThread][Index].Results = Results.OK;
                        Common.PropetyTools[IndexThread][Index].ScoreResult =  (float)Math.Round(okScore * 100);
                        rectRotates.Add(rotArea);
                        listLabel.Add("OK");
                       
                    }
                    else
                    {

                        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        Common.PropetyTools[IndexThread][Index].ScoreResult = 100 - (float)Math.Round(ngScore * 100);
                        rectRotates.Add(rotArea);
                        listLabel.Add("NG");
                    }

                    //    Console.WriteLine($"[BestPerLabel] OK: id={okId} s={okScore:0.000} | NG: id={ngId} s={ngScore:0.000}");
                   // Console.WriteLine(okScore >= ngScore ? "=> GẦN OK HƠN" : "=> GẦN NG HƠN");
                }
                //if (OKNGAPI.DetectFromMat(oKNGHandle.Handle, matCrop, out int label, out float score,
                //                       out int modelId, out int x, out int y, out int w, out int hgt))
                //{
                //    PointF pCenter = new PointF(x+w/2, y+hgt/2);
                //    rectRotates.Add(new RectRotate(new RectangleF(-w / 2, -hgt / 2, w, hgt), pCenter, 0, AnchorPoint.None));
                //    listScore.Add(score);

                //    if (label > 0)
                //    {
                //        listLabel.Add("OK");
                //        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                //    }
                //    else listLabel.Add("NG");

                //    Console.WriteLine($"Detected: {(label > 0 ? "OK" : "NG")}, score={score:F3}, modelId={modelId}");
                //    Console.WriteLine($"Location: ({x},{y}), size=({w}x{hgt})");
                //}
                //Cv2.PutText(scene, label.ToString(), new OpenCvSharp.Point(x, y), HersheyFonts.HersheySimplex, 2, Scalar.Red);
                //Cv2.Rectangle(scene, new Rect(x, y, w, hgt), Scalar.Red, 3, LineTypes.Link4);
                //Cv2.ImShow("rs", scene);
                //OKNGAPI.OKNG_Destroy(OKNGapi);
            }


        }
        public void Debug()
        {
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {

                if (raw.Empty()) return;

                Mat scene = Cropper.CropRotatedRect(raw, rotArea, null);
                // Thử các cấu hình số luồng
                foreach (int th in new[] { 1, 2, 4, 8 })
                {
                    OKNGAPI.OKNG_SetOMPThreadCount(oKNGHandle.Handle, th);
                    double total = 0; int runs = 5;

                    for (int i = 0; i < runs; i++)
                    {
                        OKNGAPI.ProfileDetectFromMat(oKNGHandle.Handle, scene,
                            out var label, out var score, out var modelId,
                            out var x, out var y, out var w, out var hh,
                            out var ms, out var threadsUsed,
                            out var triedNG, out var passedNG, out var triedOK, out var passedOK);

                        total += ms;
                        Console.WriteLine($"[threads={th}] run={i + 1}: {ms:0.2} ms (used={threadsUsed}) | "
                            + $"NG tried={triedNG}/pass={passedNG} | OK tried={triedOK}/pass={passedOK} | "
                            + $"Result={(label > 0 ? "OK" : "NG")} score={score:0.000} id={modelId}");
                    }
                    Console.WriteLine($"=> threads={th}  avg={total / runs:0.2} ms\n");
                }

                // Dùng Nearest:
                if (OKNGAPI.ClosestAnyFromMat(oKNGHandle.Handle, scene,
                    out int nLabel, out float nScore, out int nId, out int nx, out int ny, out int nw, out int nh))
                {
                    Console.WriteLine($"[Nearest Any] {(nLabel > 0 ? "OK" : "NG")}  id={nId}  score={nScore:0.000}");
                }

                // So sánh BestPerLabel:
                if (OKNGAPI.BestPerLabelFromMat(oKNGHandle.Handle, scene,
                    out int bestOKId, out float bestOKScore, out int bestNGId, out float bestNGScore))
                {
                    Console.WriteLine($"[BestPerLabel] OK(id={bestOKId})={bestOKScore:0.000}  |  NG(id={bestNGId})={bestNGScore:0.000}");
                }
            }
        }
        public void Complete()
        {
            //Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            //switch (Compare)
            //{
            //    case Compares.Equal:
            //        if (rectRotates.Count() != LimitCounter)
            //            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            //        break;
            //    case Compares.Less:
            //        if (rectRotates.Count() >= LimitCounter)
            //            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            //        break;
            //    case Compares.More:
            //        if (rectRotates.Count() <= LimitCounter)
            //            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            //        break;
            //}
           
           
         
        }
        public Graphics DrawResult(Graphics gc)
        {

            if (rotAreaAdjustment == null && Global.IsRun) return gc;
            if (Global.IsRun)
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

            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.NG)
            {
                cl = Color.Red;
                //if (BeeCore.Common.PropetyTools[IndexThread][Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.LimeGreen;


            }
            else
            {
                cl = Color.LimeGreen;
                //if (BeeCore.Common.PropetyTools[IndexThread][Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.Red;
            }
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl,  Global.ParaShow.ThicknessLine);

            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.OK)
            {
                ScoreOK += 10;
                ScoreNG -= 10;
                Draws.Box2Label(gc, rotA._rect, ScoreOK + "% OK ", ScoreNG + "% NG", font, cl, brushText, 16, Global.ParaShow.ThicknessLine);

            }
            else
            {
                ScoreOK -= 10;
                ScoreNG += 10;
                Draws.Box2Label(gc, rotA._rect, ScoreNG + "% NG ", ScoreOK + "% OK", font, cl, brushText, 16, Global.ParaShow.ThicknessLine);
            }    
               
            gc.ResetTransform();
            //if (listScore == null) return gc;
            //if (rectRotates.Count > 0)
            //{
            //    int i = 0;
            //    //foreach (RectRotate rot in rectRotates)
            //    //{
            //    //    mat = new Matrix();
            //    //    if (!Global.IsRun)
            //    //    {
            //    //        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
            //    //        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            //    //    }
            //    //    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            //    //    mat.Rotate(rotA._rectRotation);
            //    //    mat.Translate(rotA._rect.X, rotA._rect.Y);
            //    //    gc.Transform = mat;
            //    //    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
            //    //    mat.Rotate(rot._rectRotation);
            //    //    gc.Transform = mat;
            //    //    //mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
            //    //    //mat.Rotate(rot._rectRotation);
            //    //    //gc.Transform = mat;
            //    // //   Draws.Plus(gc, 0, 0, (int)rot._rect.Width / 2, cl, 2);
            //    // //   Draws.Box2Label(gc, rot._rect, listLabel[i], Math.Round(listScore[i], 1) + "%", Global.fontRS, cl, brushText, 16, 2);

            //    //    gc.ResetTransform();
            //    //    i++;
            //    //}
            //}



            return gc;
        }

        public int IndexThread;
       

    }
}
