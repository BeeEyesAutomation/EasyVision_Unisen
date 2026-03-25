
using BeeCore.Algorithm;
using BeeCore.Core;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using CvPlus;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using static LibUsbDotNet.Main.UsbTransferQueue;
using static OpenCvSharp.ML.DTrees;
using static System.Windows.Forms.MonthCalendar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Point = System.Drawing.Point;

namespace BeeCore
{
    [Serializable()]
    public class Yolo
    {
        public int _Percent = 0;//note
        [field: NonSerialized]
        public event Action<int> PercentChange;
        [NonSerialized]
        public bool IsNew = false;
        public ModeCheck ModeCheck = ModeCheck.Single;
        public List<RectRotate> listRotScan = new List<RectRotate>();
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
        
        public void InitialYolo()
        {
           
        }
        public List<RectRotate> ListRotMask = new List<RectRotate>();
        public List<RectRotate> ListRotCrop = new List<RectRotate>();
        public String pathFullModel = "";
        [NonSerialized]
        private NativeYolo NativeOnnx;
        [NonSerialized]
        private NativeYolo.YoloBox[] OnnxBoxes;
        public int NumThreadCPU = 16;

        public  void SetModel()
        {

            //if (rArea == null)
            //    rArea = rotArea;
            //if (rCrop == null)
            //    rCrop = rotCrop;
            //if (rMask == null)
            //    rMask = rotMask;
            //if (rLimit == null)
            //    rLimit = rotLimit;
            if (rotArea == null) rotArea = new RectRotate();
            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotMask == null) rotMask = new RectRotate();
            if (rotLimit == null) rotLimit = new RectRotate();
            rotCrop.Name = "Area Line";
            rotCrop.TypeCrop = TypeCrop.Crop;


            rotMask.Name = "Area Mask";
            rotMask.TypeCrop = TypeCrop.Mask;

            rotArea.Name = "Area Check";
            rotArea.TypeCrop = TypeCrop.Area;

            rotLimit.Name = "Area Limit";
            rotLimit.TypeCrop = TypeCrop.Limit;
            if (listRotScan == null) listRotScan = new List<RectRotate>();
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            if (labelItems==null)labelItems = new List<LabelItem>();
            if (ListRotMask == null)
                ListRotMask = new List<RectRotate>();
            if (ListRotCrop == null)
                ListRotCrop = new List<RectRotate>();
            try
            {
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.NotInitial;
                if (pathFullModel.Trim().Contains(".pth"))
                {
                    TypeYolo = TypeYolo.RCNN;

                }
                else if (pathFullModel.Trim().Contains(".pt"))
                {
                    TypeYolo = TypeYolo.YOLO;

                }
                else
                {
                    try
                    {
                        NumThreadCPU = 16;
                        String pathModel = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathFullModel);
                        pathModel += "\\best.xml";
                        if (File.Exists(pathModel ))
                        {
                            NativeOnnx = new NativeYolo(pathModel, 0, 0, NumThreadCPU);

                            NativeOnnx.Warmup(10);
                            OnnxBoxes = new NativeYolo.YoloBox[200];
                            TypeYolo = TypeYolo.Onnx;
                        }
                       
                            
                        Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                            
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return;
                }

                if (Global.IsIntialPython)
                using (Py.GIL())
            {

                   if(!File.Exists(pathFullModel))
                        {

                            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                            return;
                        }    
                 
                    G.objYolo.load_model(Common.PropetyTools[IndexThread][Index].Name, pathFullModel, (int)TypeYolo);
               
                            //dynamic mod = Py.Import("Tool.Learning");
                            //dynamic cls = mod.GetAttr("ObjectDetector"); // class
                            //dynamic obj = cls.Invoke();              // khởi tạo instance

                            //if (Common.PropetyTools[IndexThread][Index].Name.Trim() == "")
                            //{
                            //    Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                            //}
                            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                  
                }
                SetListTemp();
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
        //  public int Percent = 0;

        //public void Training(String nameTool,String pathYaml)
        //{
        //    using (Py.GIL())
        //    {

        //        Action<int> onProgress = percent =>
        //        {
        //            Percent = percent;
        //            Console.WriteLine($"Training progress: {percent}%");
        //        };
        //        using (PyObject pyCallback = onProgress.ToPython())
        //        {
        //            var result = G.objYolo.train(nameTool, pathYaml, Epoch, callback: pyCallback);
        //            Console.WriteLine(result.ToString());
        //        }
        //    }


        //}
        String Err = "";
        public void Training(string nameTool, string modelPath, string pathYaml)
        {
            try
            {
                if (Global.IsIntialPython)
                    using (Py.GIL())
                    {
                        Action<int> onProgress = percent =>
                        {
                            Percent = percent;

                            Console.WriteLine($"Training progress: {percent}%");
                        };

                        using (PyObject pyCallback = onProgress.ToPython())
                        {
                            var result = G.objYolo.train(
                                nameTool,
                                modelPath,
                                pathYaml,
                                Epoch,
                                callback: pyCallback
                            );

                            Console.WriteLine(result.ToString());
                        }
                    }
            }
            catch(Exception ex)
            {
                Err=ex.Message;
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "RetTrain", ex.ToString()));
            }
        }
        public Dictionary<int, string> ListNameOnnx = new Dictionary<int, string>();
        public static string[] DictToArray(Dictionary<int, string> dict)
        {
            if (dict == null || dict.Count == 0)
                return Array.Empty<string>();

            int max = dict.Keys.Max();

            string[] arr = new string[max + 1];

            foreach (var kv in dict)
                arr[kv.Key] = kv.Value;

            return arr;
        }

        public String[] LoadNameModel(String nameTool)
        {

            if (Global.IsIntialPython)
            {
                switch (TypeYolo)
                {
                    case TypeYolo.YOLO:
                        using (Py.GIL())
                        {



                            dynamic result = G.objYolo.loadNames(nameTool);

                            // Dùng list() để ép dict_values về list
                            PyObject obj = Py.Import("builtins").GetAttr("list").Invoke(result.InvokeMethod("values"));
                            var labels = new List<string>();
                            int counts = (int)obj.Length();
                            for (int j = 0; j < counts; j++)
                            {

                                labels.Add(obj[j].ToString());  // hoặc item.As<string>() nếu bạn chắc chắn là string
                            }


                            return labels.ToArray();

                        }
                        break;
                    case TypeYolo.Onnx:
                        if (NativeOnnx != null)
                        {

                            ListNameOnnx = NativeOnnx.LoadNames(pathFullModel + "\\metadata.yaml");
                        }
                        return DictToArray(ListNameOnnx);
                        break;
                    default:
                        return new string[0];
                        break;

                }
               
            }
            else
                return new string[0];
        }
        public List<LabelItem> labelItems = new List<LabelItem>();
        public List<Labels> listLabelCompare = new List<Labels>();
        public int Index = -1;
        public String PathModel = "",PathLabels="",PathDataSet;
        public TypeYolo TypeYolo = TypeYolo.YOLO;
        public TypeTool TypeTool=TypeTool.Learning;
        public RectRotate rotArea, rotCrop, rotMask,rotLimit;
        public RectRotate rArea { get; set; }
        public RectRotate rCrop { get; set; }
        public RectRotate rMask { get; set; }
        public RectRotate rLimit { get; set; }

        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotCropAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp, matTemp2, matMask;
        public List<String> Labels = new List<string>();
        private Mode _TypeMode = Mode.Pattern;
        public Compares Compare = Compares.Equal;
        public Compares CompareLine = Compares.More;
        public Compares CompareArea = Compares.More;
        public float LimitArea = 100;
        public int Epoch =100;
        [NonSerialized]
        public String NameChoose = "";
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
        //IsProcess,Convert.ToBoolean((int) TypeMode)
        [NonSerialized]
        public List<RectRotate> rectRotates = new List<RectRotate>();
        [NonSerialized]
        public List<RectRotate> rectTrain = new List<RectRotate>();
        String[] sSplit;
        
        public List<string> listModels = new List<string>();
        public List<string> listModelOnnx = new List<string>();
        String listMatch;
        public bool IsCheckLine = false;
        public bool IsCheckArea = false;
        public Point p1 = new Point();
        public Point p2 = new Point();
        public int yLine = 100;
       
        public String Content = "";
        public String Matching = "";
        public bool IsEnContent = false;

        [NonSerialized]
        List<ResultItem> resultTemp = new List<ResultItem>();
        public int IndexThread = 0;
        public int IndexCCD = 0;
        public float CropOffSetX, CropOffSetY=0;
        [NonSerialized]
        private Mat matCropTemp;
        public FilterBox FilterBox = FilterBox.Merge;
        public float ThreshOverlap = 0.1f;
        [NonSerialized]
        Line2DCli Line2D;
        [NonSerialized]
        Line2D LineVerital;
        public bool IsLine = false;
        public int LenTemp = 0;
        public int LenRS= 0;
        public float ThresholdLine = 0.5f;
        public float ToleranceLine = 0.1f;
        public static Mat CropRoiView(Mat src, RectRotate rot)
        {
            Rect roi = new Rect(new OpenCvSharp. Point(rot._PosCenter.X - rot._rect.Width / 2, rot._PosCenter.Y - rot._rect.Height / 2), new OpenCvSharp. Size(rot._rect.Width, rot._rect.Height));
            if (src == null || src.Empty()) return new Mat();
            return new Mat(src, roi); // view, dùng xong nhớ Dispose
        }
       // [NonSerialized]
        //BeeCpp. ColorArea ColorAreaPP = new BeeCpp.ColorArea();
        public void SetTemp(BeeCpp.ColorArea ColorAreaPP, HSVCli[] arrHSV,int Extraction)
        {
            ColorAreaPP.SetTempHSV(arrHSV, Extraction);
        }
        public int SizeClearBig = 50;
        public int SizeClose = 5;
        public int CheckColor(BeeCpp.ColorArea ColorAreaPP, ref Mat matProcess,Mat Crop)
        {
            int pxRs = 0;
            using (Mat src = Crop)
            {
                if (src.Empty()) return -1;
                Mat bgr = null;
                try
                {
                    if (src.Type() == MatType.CV_8UC1)
                    {
                        bgr = new Mat();
                        Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
                    }
                    else
                    {
                        bgr = src; // reuse
                    }  
                    SizeClearBig = 50;
         SizeClose = 5;
                   
                    ColorAreaPP.SetImgeNoCrop(
                        bgr.Data, bgr.Width, bgr.Height, (int)bgr.Step(), bgr.Channels());

                    GC.KeepAlive(bgr);

                    int w, h, s, c;
                    IntPtr ptr = ColorAreaPP.Check(out w, out h, out s, out c);
                    try
                    {
                        // Validate trước, nhưng KHÔNG return trước khi FreeBuffer
                        if (ptr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                        {
                            return 0; // finally phía dưới vẫn chạy để FreeBuffer nếu cần
                        }

                        MatType mt = (c == 1) ? MatType.CV_8UC1
                                   : (c == 3) ? MatType.CV_8UC3
                                              : MatType.CV_8UC4;

                        using (var mNative = new Mat(h, w, mt, ptr, s))
                        {
                            matProcess = mNative.Clone(); // bây giờ dữ liệu đã thuộc về OpenCV (managed)
                        }
                    }
                    finally
                    {
                        // GIẢI PHÓNG BỘ NHỚ DO native CẤP PHÁT — luôn luôn!
                        if (ptr != IntPtr.Zero)
                        {
                            ColorAreaPP.FreeBuffer(ptr);
                            ptr = IntPtr.Zero;
                        }
                    }

                    // Hậu xử lý:
                   // if (IsClearNoiseSmall)
                    //{
                    //    Mat t = Filters.ClearNoise(matProcess, SizeClearsmall);
                    //    if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                    //}
                  ////  if (IsClose)
                  //  {
                  //      Mat t = Filters.Morphology(matProcess, MorphTypes.Close, new OpenCvSharp.Size(SizeClose, SizeClose));
                  //      if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                  //  }
                  //  //if (IsOpen)
                  //  //{
                  //  //    Mat t = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                  //  //    if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                  //  //}
                  ////  if (IsClearNoiseBig)
                  //  {
                  //      Mat t = Filters.ClearNoise(matProcess, SizeClearBig);
                  //      if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                  //  }

                    if (matProcess.Channels() != 1)
                    {
                        using (var gray = new Mat())
                        {
                            if (matProcess.Channels() == 3)
                                Cv2.CvtColor(matProcess, gray, ColorConversionCodes.BGR2GRAY);
                            else
                                Cv2.CvtColor(matProcess, gray, ColorConversionCodes.BGRA2GRAY);

                            matProcess.Dispose();
                            matProcess = gray.Clone();
                        }
                    }
                    Random rnd = new Random();
                    //string fileName = $"Temp\\ img_{DateTime.Now:yyyyMMdd_HHmmss}_{rnd.Next(1000, 9999)}.png";

                    //Cv2.ImWrite(fileName, matProcess);
                    pxRs = Cv2.CountNonZero(matProcess);
                    return pxRs;
                }
                finally
                {
                    if (bgr != null && !object.ReferenceEquals(bgr, src))
                        bgr.Dispose();
                }
            }
        }
        
       public HSV HSV = new HSV();
        public void SetListTemp()
        {
           
           // using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
            {

                foreach (LabelItem lb in labelItems)
                {

                
                        if (lb.IsUse)
                        {
                            if (lb.IsMinColor)
                            {
                                if (lb.ListColorArea != null)
                                
                                    lb.ListColorArea.Clear();
                            if (lb.ListColorArea != null)
                                if (lb.ListColorArea.Count() !=0)
                                    lb.ListColorArea.Clear();
                                lb.ListColorArea = new List<BeeCpp.ColorArea>();
                            bool IsIni = false;
                            if(lb.ListColorArea == null|| lb.ListTempColor.Count()==0)
                            {
                                IsIni = true; lb.ListTempColor = new List<int>();
                            }
                          
                            for (int i = 0; i < lb.ListIndexBox.Count; i++)
                            {
                               
                                if (lb.HSV == null) continue;
                                if (!lb.IsCounter)
                                {
                                    lb.ListColorArea.Add(new BeeCpp.ColorArea());
                                    if(IsIni)
                                    lb.ListTempColor.Add(0);
                                    HSVCli[] arrHSV = new HSVCli[1];
                                    arrHSV[0] = new HSVCli();
                                    arrHSV[0].H = lb.HSV.H;
                                    arrHSV[0].S = lb.HSV.S;
                                    arrHSV[0].V = lb.HSV.V;
                                    SetTemp(lb.ListColorArea[lb.ListColorArea.Count - 1], arrHSV, lb.ValueExternColor);

                                }
                                else
                                {
                                    for (int j = 0; j < lb.ValueCounter; j++)
                                    {
                                        lb.ListColorArea.Add(new BeeCpp.ColorArea());
                                        if (IsIni )
                                            lb.ListTempColor.Add(0);
                                        HSVCli[] arrHSV = new HSVCli[1];
                                        arrHSV[0] = new HSVCli();
                                        arrHSV[0].H = lb.HSV.H;
                                        arrHSV[0].S = lb.HSV.S;
                                        arrHSV[0].V = lb.HSV.V;
                                        SetTemp(lb.ListColorArea[lb.ListColorArea.Count - 1], arrHSV, lb.ValueExternColor);

                                    }
                                }

                            }

                            }
                        }
                    
                }
            }    
        }
     
      
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            
            if (!Global.IsIntialPython) return;
            if (!Global.IsRun) 
                rotCropAdjustment = rotCrop;
            if(IsLine)
            if (rotCropAdjustment != null)
            {
                Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotCropAdjustment,null);
                Mat  matProcess = new Mat();
              
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGR2GRAY);
                matProcess = Filters.GetStrongEdgesOnly(matCrop);
            
                LineDirectionMode lineDirectionMode = LineDirectionMode.Vertical;
                    Line2D = new Line2DCli();
                 Line2D = RansacLine.FindBestLine(
                    matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                    iterations: 2000,
                    threshold: ThresholdLine,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, 0.6f, (BeeCpp.LineDirectionMode)((int)lineDirectionMode), LineScanMode.RightToLeft, 0, 10


                );
                  LenRS =(int) Line2D.LengthPx;

                    if (Math.Abs( LenRS - LenTemp)/ (LenTemp*1.0) >ToleranceLine)
                    {
                        Line2D.Found = false;
                        LineVerital = null;
                    }
                    if (Line2D.Found)
                {
                            
                    Line2D line = new Line2D( Line2D.Vx, Line2D.Vy, Line2D.X0, Line2D.Y0);
                      
                    Line2D lineWorld =
                    Line2DRectRotateXform.LineLocalToWorld(
                        line,
                        rotCropAdjustment
                    );
                    Line2D lineInRotArea =
                Line2DRectRotateXform.LineWorldToLocal(
                    lineWorld,
                    rotArea
                );
                    LineVerital = lineInRotArea;// Line2DRectRotateXform.LineLocal_SrcToLocal_Dst(LineVerital, rotCropAdjustment, rotArea);
                }
            }
            resultTemp = new List<ResultItem>();
            switch (TypeYolo)
            {
                case TypeYolo.Onnx:
                    if (IsCropSingle)
                    {
                        using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
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
                            for (int i = 0; i < ListRotMask.Count; i++)
                            {
                            
                            using (Mat matTemp = Cropper.CropRotatedRect(matCrop, ListRotMask[i], null))
                            {
                                float conf = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);
                                int countDetect = NativeOnnx.Detect(
                                  matTemp.Data,
                                  matTemp.Width,
                                  matTemp.Height,
                                  (int)matTemp.Step(),
                                  conf,
                                  0.9f, true,
                                  OnnxBoxes);
                                countDetect = 0;
                                foreach (NativeYolo.YoloBox box in OnnxBoxes)
                                {
                                    if (box.score == 0) continue;
                                    countDetect++;

                                }
                                if (countDetect > 0)
                                {
                                    string name = "";
                                    if (ListNameOnnx == null)
                                        name = "unknown";
                                    else
                                        name = ListNameOnnx.TryGetValue(OnnxBoxes[0].classId, out var s) ? s : "unknown";

                                    resultTemp.Add(new BeeCore.ResultItem((name)));
                                    resultTemp[resultTemp.Count - 1].rot = ListRotMask[i];
                                    resultTemp[resultTemp.Count - 1].Score = (OnnxBoxes[0].score) * 100f;
                                    resultTemp[resultTemp.Count - 1].IsOK = true;

                                }
                                else
                                {

                                    resultTemp.Add(new BeeCore.ResultItem("NG"));
                                    resultTemp[resultTemp.Count - 1].rot = ListRotMask[i];
                                    resultTemp[resultTemp.Count - 1].IsOK = false;
                                    resultTemp[resultTemp.Count - 1].Score = 0;

                                }

                            }

                        };
                        }
                    }
                    else
                    {
                        using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
                        {
                          
                            float conf = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);

                            if (matCrop.Type() == MatType.CV_8UC1)
                                Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                            int countDetect = NativeOnnx.Detect(
                            matCrop.Data,
                            matCrop.Width,
                            matCrop.Height,
                            (int)matCrop.Step(),
                            conf,
                            0.9f, false,
                            OnnxBoxes);
                            foreach (NativeYolo.YoloBox box in OnnxBoxes)
                            {
                                if (box.score == 0) continue;
                                string name = "";
                                if (ListNameOnnx == null)
                                    name = "unknown";
                                else
                                    name = ListNameOnnx.TryGetValue(box.classId, out var s) ? s : "unknown";
                                resultTemp.Add(new BeeCore.ResultItem((name)));
                                resultTemp[resultTemp.Count - 1].rot = NativeYolo.YoloBoxToRectRotate(box);
                                resultTemp[resultTemp.Count - 1].Score = (box.score) * 100f;
                                resultTemp[resultTemp.Count - 1].IsOK = true;

                            }

                        }
                    }
                    break;
                case TypeYolo.YOLO:
                    using (Py.GIL())
                    {
                        PyObject result = null;
                        PyObject boxes = null;
                        PyObject scores = null;
                        PyObject labels = null;

                        try
                        {
                            // === Tính offset (như cũ) ===
                            CropOffSetX = (rotArea._PosCenter.X - rotArea._rect.Width / 2);
                            CropOffSetY = (rotArea._PosCenter.Y - rotArea._rect.Height / 2);
                            CropOffSetX = (CropOffSetX > 0) ? 0 : -CropOffSetX;
                            CropOffSetY = (CropOffSetY > 0) ? 0 : -CropOffSetY;
                        
                            if (IsCropSingle && labelItems.Count > 0)
                            {
                                using (Mat matCrop = Cropper.CropRotatedRect(
                                    BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
                                {
                                    if (matCrop.Empty()) return;

                                    // ===== 1) Convert về 8U =====
                                    if (matCrop.Type().Depth != MatType.CV_8U)
                                    {
                                        using (var tmp8u = new Mat())
                                        {
                                            Cv2.ConvertScaleAbs(matCrop, tmp8u);
                                            tmp8u.CopyTo(matCrop);
                                        }
                                    }

                                    // ===== 2) đảm bảo BGR =====
                                    if (matCrop.Channels() == 1)
                                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                                    else if (matCrop.Channels() == 4)
                                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGRA2BGR);

                                    try
                                    {
                                        dynamic dyn = G.objYolo;
                                        if (dyn == null)
                                        {
                                            Global.LogsDashboard?.AddLog(
                                                new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", "Loi Khoi Tao Yolo"));
                                            return;
                                        }

                                        float conf = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);
                                        string toolName = Common.PropetyTools[IndexThread][Index].Name ?? "default";

                                        // ===== 3) Build batch =====
                                        var matTemps = new List<Mat>();
                                        var roiList = new List<RectRotate>();

                                        foreach (var rot in ListRotMask)
                                        {
                                            Mat matTemp = CropRoiView(matCrop, rot);

                                            if (matTemp.Empty())
                                            {
                                                matTemps.Add(null);
                                                roiList.Add(rot);
                                                continue;
                                            }

                                            matTemps.Add(matTemp);
                                            roiList.Add(rot);
                                        }

                                        // ===== 4) Convert sang PyList =====
                                        using (Py.GIL())
                                        {
                                            PyList pyImages = new PyList();

                                            foreach (var m in matTemps)
                                            {
                                                if (m == null)
                                                {
                                                    pyImages.Append(new PyTuple(new PyObject[0]));
                                                    continue;
                                                }

                                                int h = m.Rows;
                                                int w = m.Cols;
                                                int ch = m.Channels();
                                                int stride = (int)m.Step();
                                                long addr = (long)m.Data;

                                                var tuple = new PyTuple(new PyObject[]
                                                {
                                        new PyInt(addr),
                                        new PyInt(h),
                                        new PyInt(w),
                                        new PyInt(ch),
                                        new PyInt(stride)
                                                });

                                                pyImages.Append(tuple);
                                            }

                                            // ===== 5) CALL BATCH =====
                                            dynamic results = dyn.predict_batch(pyImages, conf, toolName);

                                            // ===== 6) Parse kết quả =====
                                            for (int i = 0; i < roiList.Count; i++)
                                            {
                                                var roi = roiList[i];

                                                var rs = results[i];
                                                 boxes = rs["boxes"];
                                                 scores = rs["scores"];
                                                 labels = rs["labels"];

                                                int n = (int)boxes.Length();

                                                if (n > 0)
                                                {
                                                    var b = boxes[0];

                                                    float x1 = (float)b[0].As<double>();
                                                    float y1 = (float)b[1].As<double>();
                                                    float x2 = (float)b[2].As<double>();
                                                    float y2 = (float)b[3].As<double>();

                                                    float bw = x2 - x1;
                                                    float bh = y2 - y1;
                                                    float cx = x1 + bw * 0.5f;
                                                    float cy = y1 + bh * 0.5f;

                                                    Point pCenter = new Point(
                                                        (int)(cx),
                                                        (int)(cy)
                                                    );

                                                    var rt = new RectRotate(
                                                        new RectangleF(-bw / 2f, -bh / 2f, bw, bh),
                                                        new PointF(pCenter.X, pCenter.Y),
                                                        0f, AnchorPoint.None);

                                                    var item = new BeeCore.ResultItem(labels[0].ToString());
                                                    item.rot = rt;
                                                    item.IsOK = true;
                                                    item.Score = (float)scores[0].As<double>() * 100f;

                                                    resultTemp.Add(item);
                                                }
                                                else
                                                {
                                                    var item = new BeeCore.ResultItem("NG");
                                                    item.rot = roi;
                                                    item.IsOK = false;
                                                    item.Score = 0;

                                                    resultTemp.Add(item);
                                                }
                                            }
                                        }

                                        // ===== 7) Dispose sau khi Python xong =====
                                        foreach (var m in matTemps)
                                            m?.Dispose();
                                    }
                                    catch (Exception ex)
                                    {
                                        Global.LogsDashboard?.AddLog(
                                            new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.ToString()));
                                    }
                                }
                            }
                            else
                            {
                               
                                // === Crop ROI ===
                                using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
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
                                   
                                    if (matCropTemp == null) matCropTemp = new Mat();
                                    if (!matCropTemp.Empty()) matCropTemp.Dispose();
                                    matCropTemp = matCrop.Clone();
                                    // nếu đã 3 kênh BGR thì giữ nguyên

                                    int h = matCrop.Rows;
                                    int w = matCrop.Cols;
                                    int ch = matCrop.Channels(); // 3
                                    int stride = (int)matCrop.Step(); // bytes/row (có thể > w*ch)
                                    IntPtr p = matCrop.Data;

                                    float conf = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);
                                    string toolName = Common.PropetyTools[IndexThread][Index].Name ?? "default";

                                    // === Gọi YOLO (nhận: (boxes, scores, labels)) ===
                                    // Ký hiệu: result là tuple-like (3 phần)
                                    dynamic dyn = G.objYolo;
                                    if (dyn == null)
                                        throw new InvalidOperationException("objYolo chưa được khởi tạo.");

                                    result = dyn.predict((long)p, h, w, ch, stride, conf, toolName);

                                    // Ép về PyObject để chủ động Dispose
                                    boxes = (PyObject)result[0];
                                    scores = (PyObject)result[1];
                                    labels = (PyObject)result[2];

                                    int n = (int)boxes.Length();

                                    // === Chuẩn bị danh sách output ===
                                    //  if (resultTemp == null) resultTemp = new List<ResultItem>(n);

                                    //else { boxList.Clear(); if (boxList.Capacity < n) boxList.Capacity = n; }

                                    //if (scoreList == null) scoreList = new List<float>(n);
                                    //else { scoreList.Clear(); if (scoreList.Capacity < n) scoreList.Capacity = n; }

                                    //if (labelList == null) labelList = new List<string>(n);
                                    //else { labelList.Clear(); if (labelList.Capacity < n) labelList.Capacity = n; }

                                    // === Đọc kết quả ===
                                    for (int j = 0; j < n; j++)
                                    {
                                        var b = boxes[j];   // PyObject
                                        float x1 = (float)b[0].As<double>();
                                        float y1 = (float)b[1].As<double>();
                                        float x2 = (float)b[2].As<double>();
                                        float y2 = (float)b[3].As<double>();

                                        float bw = x2 - x1;
                                        float bh = y2 - y1;
                                        float cx = x1 + bw * 0.5f;
                                        float cy = y1 + bh * 0.5f;

                                        var rt = new RectRotate(
                                            new System.Drawing.RectangleF(-bw / 2f, -bh / 2f, bw, bh),
                                            new System.Drawing.PointF(cx, cy),
                                            0f, AnchorPoint.None);
                                        resultTemp.Add(new BeeCore.ResultItem(((PyObject)labels[j]).ToString()));
                                        resultTemp[resultTemp.Count - 1].rot = rt;
                                        resultTemp[resultTemp.Count - 1].Score = (float)((PyObject)scores[j]).As<double>() * 100f;
                                        //boxList.Add(rt);
                                        //scoreList.Add((float)((PyObject)scores[j]).As<double>() * 100f);
                                        //labelList.Add(((PyObject)labels[j]).ToString());
                                    }
                                    switch (FilterBox)
                                    {
                                        case FilterBox.Merge:
                                            resultTemp = ResultFilter.MergeSameNameOverlap(resultTemp, ThreshOverlap);
                                            break;
                                        case FilterBox.Remove:
                                            resultTemp = ResultFilter.FilterRectRotate(resultTemp, ThreshOverlap);
                                            break;
                                    }
                                    //foreach (var r in resultTemp)
                                    //{

                                    //    int index = labelItems.FindIndex(x =>
                                    //    string.Equals(x.Name, r.Name, StringComparison.OrdinalIgnoreCase));
                                    //    if (index >= 0)
                                    //    {
                                    //        LabelItem lb = labelItems[index];
                                    //        Parallel.For(0, lb.ListIndexBox.Count, new ParallelOptions { MaxDegreeOfParallelism = Math.Min(300, Environment.ProcessorCount) },
                                    //        j =>
                                    //        //for (int j = 0; j < lb.ListIndexBox.Count; j++)
                                    //        {
                                    //            HSVCli[] arrHSV = new HSVCli[1];
                                    //            arrHSV[0] = new HSVCli();
                                    //            arrHSV[0].H = lb.HSV.H;
                                    //            arrHSV[0].S = lb.HSV.S;
                                    //            arrHSV[0].V = lb.HSV.V;
                                    //          //  SetTemp(lb.ListColorArea[j], arrHSV, lb.ValueExternColor);
                                    //            using (Mat matCrop2 = CropRoiView(matCrop, r.rot))
                                    //            {
                                    //                r.matProcess = new Mat();
                                    //                r.ValueColor = CheckColor(lb.ListColorArea[j], ref r.matProcess, matCrop2);
                                    //                if (!Global.IsRun)
                                    //                    lb.ListTempColor[j] = r.ValueColor;
                                    //            }
                                    //        });
                                    //    }
                                    //}
                                    GC.KeepAlive(matCrop);
                                }
                               
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
                            // Giải phóng PyObject để tránh rò rỉ
                            if (labels != null) labels.Dispose();
                            if (scores != null) scores.Dispose();
                            if (boxes != null) boxes.Dispose();
                            if (result != null) result.Dispose();
                        }
                    }
                    break;
            }    
           
        }

    
        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        public bool IsSendResult;
        public ArrangeBox ArrangeBox=new ArrangeBox();
        public bool IsArrangeBox = false;
        public bool IsCropSingle = false;
        public async Task SendResult()
        {
            if (IsSendResult)
            {
               if( Global.Comunication.Protocol.IsConnected)
                {
                  await  Global.Comunication.Protocol.WriteResultBits(AddPLC, BitsResult);
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
        static bool IntersectXMax(RectRotate r, float valueX)
        {
            float w = r._rect.Width, h = r._rect.Height;
            double th = r._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(th), s = Math.Sin(th);

            float extX = (float)(Math.Abs(c) * (w * 0.5) + Math.Abs(s) * (h * 0.5));
            float minX = r._PosCenter.X - extX;
            float maxX = r._PosCenter.X + extX;

            return maxX <= valueX;   // cắt đường x = valueX
        }

        static bool IntersectYMax(RectRotate r, float valueY)
        {
            float w = r._rect.Width, h = r._rect.Height;
            double th = r._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(th), s = Math.Sin(th);

            float extY = (float)(Math.Abs(s) * (w * 0.5) + Math.Abs(c) * (h * 0.5));
            float minY = r._PosCenter.Y - extY;
            float maxY = r._PosCenter.Y + extY;

            return maxY <= valueY;   // cắt đường y = valueY
        }
        public static double CalcMissingPercent_AutoMinMax(ref Mat src, Rect bbox, double brightRatio = 0.25)
        {
            // brightRatio = phần trăm "vùng sáng nhất" muốn lấy (0.2–0.4 thường ok)
            if (brightRatio <= 0) brightRatio = 0.25;
            if (brightRatio >= 1) brightRatio = 0.9;

            // 1. Crop ROI từ YOLO box
            using (var roi = new Mat(src, bbox))
            using (var gray = new Mat())
            using (var mask = new Mat())
            {
                // 2. BGR -> Gray
                if (roi.Channels() == 3 || roi.Channels() == 4)
                    Cv2.CvtColor(roi, gray, ColorConversionCodes.BGR2GRAY);
                else
                    roi.CopyTo(gray);
               // Cv2.ImWrite("cropyolo.png", gray);
                // 3. Lấy min / max trong ROI
                double minVal, maxVal;
               OpenCvSharp. Point minLoc, maxLoc;
                Cv2.MinMaxLoc(gray, out minVal, out maxVal, out minLoc, out maxLoc);

                // Trường hợp phẳng màu (không có gì khác biệt)
                if (Math.Abs(maxVal - minVal) < 1e-6)
                    return 0.0;

                // 4. Tính ngưỡng t dựa trên khoảng [min, max]
                // Ví dụ brightRatio = 0.25 => lấy vùng sáng nhất 25% gần max
                // tương đương: t = min + (max-min)*(1 - brightRatio)
                double t = minVal + (maxVal - minVal) * (1.0 - brightRatio);

                // 5. Threshold vùng sáng (thiếu chì)
                Cv2.Threshold(gray, mask, t, 255, ThresholdTypes.Binary);
                src = mask.Clone();
                // (tuỳ chọn) làm sạch mask một chút
                // var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
                // Cv2.MorphologyEx(mask, mask, MorphTypes.Open, kernel);

                // 6. Đếm pixel trắng = vùng thiếu chì
                int missingPixels = Cv2.CountNonZero(mask);
                int totalPixels = bbox.Width * bbox.Height;

                if (totalPixels <= 0)
                    return 0.0;

                double percent = missingPixels * 100.0 / totalPixels;
                return percent;
            }
        }
        double percent = 0;
        [NonSerialized]
        public List< ResultItem> ResultItem=new List<ResultItem>() ;
        int numOK = 0, numNG = 0;
        double CalcArea(ResultItem r, LabelItem item, out double percentOut)
        {
            percentOut = 0;

            if (item.Name == "T_CHI")
            {
                Rect rect = new Rect(
                    (int)r.rot._PosCenter.X + (int)r.rot._rect.X,
                    (int)r.rot._PosCenter.Y + (int)r.rot._rect.Y,
                    (int)r.rot._rect.Width,
                    (int)r.rot._rect.Height);

                if (r.matProcess == null)
                    r.matProcess = new Mat();

                if (!r.matProcess.Empty())
                    r.matProcess.Dispose();

                r.matProcess = matCropTemp.Clone();

                percentOut = CalcMissingPercent_AutoMinMax(ref r.matProcess, rect);

                return percentOut * r.rot._rect.Width * r.rot._rect.Height / 100;
            }

            return r.rot._rect.Width * r.rot._rect.Height;
        }
        int FindScanBox(ResultItem rs ,String Name)
        {
            for (int k = 0; k < listRotScan.Count; k++)
            {
                if(listRotScan[k].Name != Name) continue ;
                if (listRotScan[k].ContainsPoint(rs.rot._PosCenter))
                  
                    return k;
            }

            return -1;
        }
        double CalcScanArea(int scanIndex, LabelItem item)
        {
            RectRotate scan = listRotScan[scanIndex];

            double area = 0;

            foreach (var r in resultTemp)
            {
                if (r.Name != item.Name)
                    continue;

                if (scan.ContainsPoint(r.rot._PosCenter))
                    area += r.Area;
            }

            return area;
        }
        public void Complete()
        {
            if (!Global.IsIntialPython)
            {
                Global.LogsDashboard.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", "No Initial"));
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                return;
            }

            try
            {
                ResultItem = new List<ResultItem>();
                numOK = 0;

                if (labelItems == null)
                {
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                    return;
                }

                //--------------------------------
                // MAP LABEL
                //--------------------------------
                var labelMap = labelItems
                    .GroupBy(x => x.Name.ToLower())
                    .ToDictionary(g => g.Key, g => g.First());

                //--------------------------------
                // BUILD RESULT
                //--------------------------------
                if (resultTemp != null)
                {
                    foreach (var rs in resultTemp)
                    {
                        ResultItem.Add(new ResultItem(rs.Name)
                        {
                            matProcess = rs.matProcess,
                            IsOK = rs.IsOK,
                            rot = rs.rot,
                            Score = rs.Score
                        });
                    }
                }

                //--------------------------------
                // HELPER
                //--------------------------------
                bool IsBool = false;
                String lbName = "";
                bool IsInside(RectRotate scan, RectRotate r)
                {
                    if (scan == null || r == null) return false;
                    //if (IsCropSingle)
                    //{
                    //    RectangleF scanRect = new RectangleF(0, 0, scan._rect.Width, scan._rect.Height);

                    //    RectangleF objRect = new RectangleF(
                    //        r._PosCenter.X - r._rect.Width / 2f,
                    //        r._PosCenter.Y - r._rect.Height / 2f,
                    //        r._rect.Width,
                    //        r._rect.Height
                    //    );

                    //    return objRect.Left >= scanRect.Left &&
                    //           objRect.Top >= scanRect.Top &&
                    //           objRect.Right <= scanRect.Right &&
                    //           objRect.Bottom <= scanRect.Bottom;
                    //}
                    //else
                    //{
                    //    // world mode: nên check 4 góc của object
                    //    PointF p1 = new PointF(r._PosCenter.X - r._rect.Width / 2f, r._PosCenter.Y - r._rect.Height / 2f);
                    //    PointF p2 = new PointF(r._PosCenter.X + r._rect.Width / 2f, r._PosCenter.Y - r._rect.Height / 2f);
                    //    PointF p3 = new PointF(r._PosCenter.X + r._rect.Width / 2f, r._PosCenter.Y + r._rect.Height / 2f);
                    //    PointF p4 = new PointF(r._PosCenter.X - r._rect.Width / 2f, r._PosCenter.Y + r._rect.Height / 2f);

                    //    return scan.ContainsPoint(p1) &&
                    //           scan.ContainsPoint(p2) &&
                    //           scan.ContainsPoint(p3) &&
                    //           scan.ContainsPoint(p4);
                    //}

                    if (IsCropSingle)
                    {
                        RectangleF rect = new RectangleF(0, 0, scan._rect.Width, scan._rect.Height);
               
                       
                        return rect.Contains(r._PosCenter);
                    }
                    else
                        return scan.ContainsPoint(r._PosCenter);
                }
                
                //--------------------------------
                // SCANBOX (FIX CHUẨN)
                //--------------------------------
                if (listRotScan != null && listRotScan.Count > 0)
                {
                    for (int scanIndex = 0; scanIndex < listRotScan.Count; scanIndex++)
                    {
                        var scan = listRotScan[scanIndex];

    

                                bool boxOK = true;
                                bool hasRule = false;

                                foreach (var item in labelItems)
                                {
                                    if (item == null || !item.IsUse)
                                        continue;

                                    if (item.ListIndexBox == null || !item.ListIndexBox.Contains(scanIndex))
                                        continue;

                                    hasRule = true;

                                    //--------------------------------
                                    // GET OBJECTS IN BOX
                                    //--------------------------------
                                    List<ResultItem> objs;

                                    if (IsCropSingle)
                                    {
                                        objs = new List<ResultItem>();

                                        if (scanIndex < ResultItem.Count)
                                        {
                                            var r = ResultItem[scanIndex];

                                    if (r.rot != null &&
                                        r.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                                    {
                                        objs.Add(r);
                                    }
                                    else
                                        r.IsOK = false;
                                        }
                                    }
                                    else
                                    {
                                        objs = ResultItem
                                            .Where(x =>
                                                x.rot != null &&
                                                x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase) &&
                                                scan.ContainsPoint(x.rot._PosCenter))
                                            .OrderBy(x => x.rot._PosCenter.X)
                                            .ToList();
                                    }
                            scan.NumInside = objs.Count;
                            //--------------------------------
                            // NO OBJECT
                            //--------------------------------
                            if (objs.Count == 0)
                                    {
                                        boxOK = false;
                                        continue;
                                    }
                           
                            //--------------------------------
                            // COUNTER
                            //--------------------------------
                            if (item.IsCounter)
                                    {
                                        if (objs.Count < item.ValueCounter)
                                        {
                                            foreach (var rr in objs)
                                                rr.IsOK = false;
                                 
                                            boxOK = false;
                                            continue;
                                        }
                                    }

                                    //--------------------------------
                                    // LOOP OBJECT
                                    //--------------------------------
                                    for (int j = 0; j < objs.Count; j++)
                                    {
                                        var r = objs[j];
                                        bool ok = true;

                                        //--------------------------------
                                        // SIZE
                                        //--------------------------------
                                        if (item.IsWidth)
                                            ok &= r.rot._rect.Width >= item.ValueWidth;

                                        if (item.IsHeight)
                                            ok &= r.rot._rect.Height >= item.ValueHeight;

                                        //--------------------------------
                                        // POSITION
                                        //--------------------------------
                                        if (item.IsX)
                                            ok &= IntersectX(r.rot, item.ValueX);

                                        if (item.IsY)
                                            ok &= IntersectY(r.rot, item.ValueY);

                                        if (item.IsXMax)
                                            ok &= IntersectXMax(r.rot, item.ValueXMax);

                                        if (item.IsYMax)
                                            ok &= IntersectYMax(r.rot, item.ValueYMax);

                                        //--------------------------------
                                        // AREA (SCANBOX LEVEL)
                                        //--------------------------------
                                        if (item.IsArea)
                                        {
                                            double sumArea = objs.Sum(x => x.Area);
                                            ok &= sumArea >= item.ValueArea * 100;
                                        }

                                        //--------------------------------
                                        // COLOR
                                        //--------------------------------
                                        if (item.IsMinColor)
                                        {
                                            if (j >= item.ListColorArea.Count || j >= item.ListTempColor.Count)
                                            {
                                                ok = false;
                                            }
                                            else
                                            {
                                                using (Mat matCrop2 = CropRoiView(matCropTemp, r.rot))
                                                {
                                                    if (r.matProcess == null)
                                                        r.matProcess = new Mat();

                                                    int val = CheckColor(item.ListColorArea[scanIndex * item.ValueCounter + j], ref r.matProcess, matCrop2);

                                                    if (!Global.IsRun)
                                                        item.ListTempColor[scanIndex*item.ValueCounter+ j] = val;

                                                    int valTemp = item.ListTempColor[scanIndex * item.ValueCounter + j];

                                                  r.PercentColor = (float)((Math.Abs(val - valTemp) / (valTemp*1.0)) * 100.0);

                                                    ok &= r.PercentColor <= item.ValueMinColor;
                                                }
                                            }
                                        }

                                        r.IsOK = ok;

                                        if (!ok)
                                            boxOK = false;
                                        
                                    }
                                }

                                //--------------------------------
                                // NO RULE → OK
                                //--------------------------------
                                if (!hasRule)
                                {
                                    scan.IsOK = true;
                                    numOK++;
                                    continue;
                                }

                                scan.IsOK = boxOK;

                                if (boxOK)
                                    numOK++;
                            
                        }
                }
                else
                {
                    foreach (var item in labelItems)
                    {
                        if (item == null || !item.IsUse)
                            continue;

                        //--------------------------------
                        // GET OBJECTS THEO LABEL
                        //--------------------------------
                        var objs = ResultItem
                            .Where(x => x.rot != null &&
                                        x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                            .OrderBy(x => x.rot._PosCenter.X)
                            .ToList();

                        //--------------------------------
                        // KHÔNG CÓ OBJECT
                        //--------------------------------
                        if (objs.Count == 0)
                            continue;

                        //--------------------------------
                        // COUNTER
                        //--------------------------------
                        if (item.IsCounter)
                        {
                            if (objs.Count < item.ValueCounter)
                            {
                                foreach (var r in objs)
                                    r.IsOK = false;

                                continue;
                            }
                        }

                        //--------------------------------
                        // LOOP OBJECT
                        //--------------------------------
                        for (int j = 0; j < objs.Count; j++)
                        {
                            var r = objs[j];
                            bool ok = true;

                            //--------------------------------
                            // SIZE
                            //--------------------------------
                            if (item.IsWidth)
                                ok &= r.rot._rect.Width >= item.ValueWidth;

                            if (item.IsHeight)
                                ok &= r.rot._rect.Height >= item.ValueHeight;

                            //--------------------------------
                            // POSITION
                            //--------------------------------
                            if (item.IsX)
                                ok &= IntersectX(r.rot, item.ValueX);

                            if (item.IsY)
                                ok &= IntersectY(r.rot, item.ValueY);

                            if (item.IsXMax)
                                ok &= IntersectXMax(r.rot, item.ValueXMax);

                            if (item.IsYMax)
                                ok &= IntersectYMax(r.rot, item.ValueYMax);

                            //--------------------------------
                            // AREA (GLOBAL)
                            //--------------------------------
                            if (item.IsArea)
                            {
                                double sumArea = objs.Sum(x => x.Area);
                                ok &= sumArea >= item.ValueArea * 100;
                            }

                            //--------------------------------
                            // COLOR (GLOBAL INDEX)
                            //--------------------------------
                            if (item.IsMinColor)
                            {
                                if (j >= item.ListColorArea.Count || j >= item.ListTempColor.Count)
                                {
                                    ok = false;
                                }
                                else
                                {
                                    using (Mat matCrop2 = CropRoiView(matCropTemp, r.rot))
                                    {
                                        if (r.matProcess == null)
                                            r.matProcess = new Mat();

                                        int val = CheckColor(item.ListColorArea[j], ref r.matProcess, matCrop2);

                                        if (!Global.IsRun)
                                            item.ListTempColor[j] = val;

                                        int valTemp = item.ListTempColor[j];

                                        if (valTemp > 0)
                                        {
                                            float percent = (float)(Math.Abs(val - valTemp) / valTemp * 100.0);
                                            ok &= percent <= item.ValueMinColor;
                                        }
                                        else
                                        {
                                            ok = false;
                                        }
                                    }
                                }
                            }

                            r.IsOK = ok;
                        }

                        //--------------------------------
                        // COUNT OK
                        //--------------------------------
                        numOK += objs.Count(x => x.IsOK);
                    }
                }    
                    //--------------------------------
                    // RESULT
                    //--------------------------------
                    Common.PropetyTools[IndexThread][Index].Results = Results.OK;

                if (Compare == Compares.Equal && numOK != NumObject)
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;

                if (Compare == Compares.Less && numOK >= NumObject)
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;

                if (Compare == Compares.More && numOK <= NumObject)
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;

                if (IsLine && !Line2D.Found)
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;

                G.IsChecked = true;
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.Message));
            }
        }
        //public void Complete()
        //{
        //    if (!Global.IsIntialPython)
        //    {
        //        Global.LogsDashboard.AddLog(
        //            new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", "No Initial"));
        //        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //        return;
        //    }

        //    try
        //    {
        //        ResultItem = new List<ResultItem>();
        //        rectRotates = new List<RectRotate>();

        //        numOK = 0;
        //        numNG = 0;

        //        if (labelItems == null)
        //        {
        //            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //            return;
        //        }

        //        //--------------------------------
        //        // PASS 0 : map label
        //        //--------------------------------
        //        var labelMap = labelItems
        //            .GroupBy(x => x.Name.ToLower())
        //            .ToDictionary(g => g.Key, g => g.First());

        //        //--------------------------------
        //        // PASS 1 : build ResultItem
        //        //--------------------------------
        //        if (resultTemp != null)
        //            foreach (var rs in resultTemp)
        //            {
        //                ResultItem.Add(new ResultItem(rs.Name)
        //                {
        //                    matProcess = rs.matProcess,
        //                    IsOK = rs.IsOK,
        //                    rot = rs.rot,
        //                    Score = rs.Score
        //                });
        //            }

        //        //--------------------------------
        //        // PASS 2 : Area
        //        //--------------------------------
        //        foreach (var r in ResultItem)
        //        {
        //            if (!labelMap.ContainsKey(r.Name.ToLower()))
        //                continue;

        //            var item = labelMap[r.Name.ToLower()];

        //            if (!IsCropSingle)
        //            {
        //                double percentLocal;
        //                double area = CalcArea(r, item, out percentLocal);
        //                r.Area = (float)area;
        //                r.Percent = (float)percentLocal;
        //            }
        //        }

        //        //--------------------------------
        //        // PASS 3 : ScanBox xử lý chính
        //        //--------------------------------
        //        if (!IsCropSingle && listRotScan != null && listRotScan.Count > 0)
        //        {
        //            for (int scanIndex = 0; scanIndex < listRotScan.Count; scanIndex++)
        //            {
        //                var scan = listRotScan[scanIndex];

        //                // lấy object trong box
        //                var listInBox = ResultItem
        //                    .Where(x => x.rot != null &&
        //                                scan.ContainsPoint(x.rot._PosCenter))
        //                    .ToList();

        //                // ❌ KHÔNG có object → FAIL luôn
        //                if (listInBox.Count == 0)
        //                {
        //                    scan.IsOK = false;
        //                    continue;
        //                }

        //                bool boxOK = true;

        //                //--------------------------------
        //                // GROUP theo label
        //                //--------------------------------
        //                var groupByLabel = listInBox
        //                    .GroupBy(x => x.Name);

        //                foreach (var group in groupByLabel)
        //                {
        //                    string name = group.Key;

        //                    if (!labelMap.ContainsKey(name.ToLower()))
        //                        continue;

        //                    var item = labelMap[name.ToLower()];
        //                    var objs = group.OrderBy(x => x.rot._PosCenter.X).ToList();

        //                    //--------------------------------
        //                    // COUNTER
        //                    //--------------------------------
        //                    if (item.IsCounter)
        //                    {
        //                        int need = item.ValueCounter;

        //                        if (objs.Count < need)
        //                        {
        //                            boxOK = false;
        //                            continue;
        //                        }
        //                    }

        //                    //--------------------------------
        //                    // COLOR
        //                    //--------------------------------
        //                    if (item.IsMinColor)
        //                    {
        //                        for (int j = 0; j < objs.Count; j++)
        //                        {
        //                            var rr = objs[j];

        //                            if (j >= item.ListColorArea.Count)
        //                            {
        //                                rr.IsOK = false;
        //                                boxOK = false;
        //                                continue;
        //                            }

        //                            using (Mat matCrop2 = CropRoiView(matCropTemp, rr.rot))
        //                            {
        //                                if (rr.matProcess == null)
        //                                    rr.matProcess = new Mat();

        //                                int val = CheckColor(item.ListColorArea[j], ref rr.matProcess, matCrop2);
        //                                rr.ValueColor = val;

        //                                if (!Global.IsRun)
        //                                    item.ListTempColor[j] = val;

        //                                int valTemp = item.ListTempColor[j];

        //                                if (valTemp > 0)
        //                                {
        //                                    rr.PercentColor =
        //                                        (float)(Math.Abs(val - valTemp) / (valTemp * 1.0) * 100.0);
        //                                }

        //                                bool colorOK = rr.PercentColor <= item.ValueMinColor;

        //                                if (!colorOK)
        //                                {
        //                                    rr.IsOK = false;
        //                                    boxOK = false;
        //                                }
        //                                else
        //                                {
        //                                    rr.IsOK = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                //--------------------------------
        //                // SET BOX RESULT
        //                //--------------------------------
        //                scan.IsOK = boxOK;

        //                if (boxOK)
        //                    numOK++;
        //            }
        //        }

        //        //--------------------------------
        //        // RESULT FINAL
        //        //--------------------------------
        //        Common.PropetyTools[IndexThread][Index].Results = Results.OK;

        //        switch (Compare)
        //        {
        //            case Compares.Equal:
        //                if (numOK != NumObject)
        //                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                break;

        //            case Compares.Less:
        //                if (numOK >= NumObject)
        //                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                break;

        //            case Compares.More:
        //                if (numOK <= NumObject)
        //                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                break;
        //        }

        //        if (IsLine && !Line2D.Found)
        //            Common.PropetyTools[IndexThread][Index].Results = Results.NG;

        //        G.IsChecked = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.LogsDashboard.AddLog(
        //            new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.Message));
        //    }
        //}

        public bool IsColorAllObjLabel = false;
        [NonSerialized]
        public bool IsScan = false;
        public int IndexProgChoose = 0;
        public Graphics DrawResult(Graphics gc)
        {
            Brush brushText = new SolidBrush(Global.ParaShow.TextColor);
            if (rotAreaAdjustment == null && Global.IsRun) return gc;
            gc.ResetTransform();
            RectRotate rotA = rotArea;
            if (Global.IsRun) rotA = rotAreaAdjustment;
            var mat = new Matrix();
            int i = 0;

                Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                Color clScan= Color.White;
               
           
                    if (listRotScan != null)
                    foreach (RectRotate rot in listRotScan)
                {
                    String cOK ="("+ rot.NumInside+ ") OK";
                    if (Global.StatusDraw==StatusDraw.Scan)
                        {
                        cOK = "";
                            if (IsCropSingle)
                            {
                                if (rot._dragAnchor == AnchorPoint.Center)
                                    clScan = Global.ParaShow.ColorChoose;
                                else
                                    clScan = Color.LightGray;
                            }
                            else
                        {

                            if (rot._dragAnchor == AnchorPoint.Center&&rot.Name.Trim()!= "Area Limit")
                                clScan = Global.ParaShow.ColorChoose;
                            else
                            {
                                rot._dragAnchor = AnchorPoint.None;
                                clScan = Color.LightGray;
                            }    
                                
                            //if (rot.Name != "")
                            //    {

                            //        clScan = Global.ParaShow.ColorChoose;
                            //    }
                            //    else
                            //        clScan = Global.ParaShow.ColorNone;
                            }
                        }
                        else
                        {
                        //if (Global.IsRun)
                        {
                           
                            //if (!IsCropSingle)
                            {
                                int index = labelItems.FindIndex(item => string.Equals(item.Name, rot.Name, StringComparison.OrdinalIgnoreCase));

                                if (index > -1)
                                {
                                    LabelItem item = labelItems[index];
                                    if (rot.IsOK == true)
                                        clScan = Global.ParaShow.ColorOK;
                                    else
                                    {
                                        cOK = "(" + rot.NumInside + ") NG";
                                        clScan = Global.ParaShow.ColorNG;
                                    }
                                }
                                else
                                {
                                    if (rot.Name != "")
                                        clScan = Global.ParaShow.ColorChoose;
                                    else
                                    {
                                        clScan = Global.ParaShow.ColorNone;
                                        //  continue;
                                    }
                                }
                            }
                            //else
                            //{
                            //    if (rot.Name != "")
                            //        clScan = Global.ParaShow.ColorChoose;
                            //    else
                            //    {
                            //        clScan = Global.ParaShow.ColorNone;
                            //        //  continue;
                            //    }
                            //}
                        }
                    
                        //else
                        //{
                        ////    if (rot.Name != "")
                        ////        clScan = Global.ParaShow.ColorChoose;
                        ////    else
                        ////    {
                        ////        clScan = Global.ParaShow.ColorNone;
                        ////        //  continue;
                        ////    }
                        //}
                           
                         

                        }    
                           
                        Pen penScan = new Pen(clScan, Global.ParaShow.ThicknessLine);
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
                    int indexArea = i + 1;
                   
                        Draws.Box2Label(gc, rot, indexArea + "."+ rot.Name, cOK, font, clScan, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);


                    ////  Draws.Box1Label(gc, rotA, rot.Name, "Count: " + numOK, font, cl, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);

                    //      Draws.DrawRectRotate(gc, rot, penScan);
                    i++;
                        }
                        gc.ResetTransform();
            if (Global.StatusDraw == StatusDraw.Scan)
                return gc;
                //if (Line2D.Found)
                //{

                //    if (!Global.IsRun)
                //    {
                //        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                //        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                //    }
                //    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                //    mat.Rotate(rotA._rectRotation);
                //    mat.Translate(rotA._rect.X, rotA._rect.Y);
                //    gc.Transform = mat;


                //    //PointF p1 = new PointF(LineVerital.X1, LiLineVeritalne2D.Y1);
                //    //PointF p2 = new PointF(LineVerital.X2, LineVerital.Y2);
                //  //  Draws.DrawInfiniteLine(gc, new Pen(Global.ParaShow.ColorChoose, Global.ParaShow.ThicknessLine), LineVerital);

                //    //   gc.DrawLine(new Pen(new SolidBrush(Global.ParaShow.ColorInfor), Global.ParaShow.ThicknessLine), p1, p2);
                //    gc.ResetTransform();
                //}
                mat = new Matrix();
            if (rotMaskAdjustment != null)
            {
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotMaskAdjustment._PosCenter.X, rotMaskAdjustment._PosCenter.Y);
                mat.Rotate(rotMaskAdjustment._rectRotation);
                gc.Transform = mat;
                int alpha = Global.ParaShow.Opacity * 255 / 100;

                Color color = Color.FromArgb(
                    alpha,
                    Global.ParaShow.ColorNone.R,
                    Global.ParaShow.ColorNone.G,
                    Global.ParaShow.ColorNone.B
                );
                gc.FillRectangle(new SolidBrush(color), new Rectangle((int)rotMaskAdjustment._rect.X, (int)rotMaskAdjustment._rect.Y, (int)rotMaskAdjustment._rect.Width, (int)rotMaskAdjustment._rect.Height));
                gc.ResetTransform();

            }
            mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;

          
            Color cl = Color.LimeGreen;
            switch (Common.PropetyTools[Global.IndexProgChoose][Index].Results)
            {
                case Results.OK:
                    cl =  Global.ParaShow.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ParaShow.ColorNG;
                    break;
            }
            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            //Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box2Label(gc, rotA, nameTool,"Count: "+ numOK, font, cl, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);
            if (IsLine)
                if (Line2D.Found)
            {
                mat.Translate(rotA._rect.X, rotA._rect.Y);
                gc.Transform = mat;
                Draws.DrawInfiniteLine(gc, new Pen(Global.ParaShow.ColorChoose, Global.ParaShow.ThicknessLine), LineVerital);
                gc.ResetTransform();
                    Line2D = new Line2DCli();

                }

            //if (Line2D.Found)
            //{

            //    //if (!Global.IsRun)
            //    //{
            //    //    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
            //    //    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            //    //}
            //    //mat.Translate(rotCropAdjustment._PosCenter.X, rotCropAdjustment._PosCenter.Y);
            //    //mat.Rotate(rotCropAdjustment._rectRotation);
            //    //mat.Translate(rotCropAdjustment._rect.X, rotCropAdjustment._rect.Y);
            //    //gc.Transform = mat;

            //  //  mat.Translate(rotA._rect.X, rotA._rect.Y);

            // //   gc.Transform = mat;
            //    Draws.DrawInfiniteLine(gc, new Pen(Global.ParaShow.ColorChoose, Global.ParaShow.ThicknessLine), LineVerital);
            //    //   gc.DrawLine(new Pen(new SolidBrush(Global.ParaShow.ColorInfor), Global.ParaShow.ThicknessLine), p1, p2);
            //    // gc.ResetTransform();
            //}


            //  Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl,  Global.ParaShow.ThicknessLine);
            //int i = 0;
            if (!Global.IsRun)
                foreach (LabelItem item in labelItems)
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

                    mat.Translate(CropOffSetX, CropOffSetY);
                    gc.Transform = mat;
                    if (item.IsY)
                    {
                        Point p1 = new Point(0, item.ValueY);
                        Point p2 = new Point(50, item.ValueY);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.SkyBlue, Global.ParaShow.ThicknessLine));
                    }
                    if (item.IsX)
                    {
                        Point p1 = new Point(item.ValueX, 0);
                        Point p2 = new Point(item.ValueX, 50);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.SkyBlue, Global.ParaShow.ThicknessLine));
                    }
                    if (item.IsYMax)
                    {
                        Point p1 = new Point(0, item.ValueYMax);
                        Point p2 = new Point(50, item.ValueYMax);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, Global.ParaShow.ThicknessLine));
                    }
                    if (item.IsXMax)
                    {
                        Point p1 = new Point(item.ValueXMax, 0);
                        Point p2 = new Point(item.ValueXMax, 50);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, Global.ParaShow.ThicknessLine));
                    }
                    gc.ResetTransform();  
            }
            if (ResultItem == null)
                return gc;
            i = 0;
            foreach (ResultItem rs in ResultItem)
            {
                if (rs.rot == null)
                {
                    i++;
                    continue;
                }
                Color clShow = Global.ParaShow.ColorNone;

                if(IsCropSingle)
                {
                    if (rs.IsOK == true)
                        clShow = Global.ParaShow.ColorOK;
                    else
                        clShow = Global.ParaShow.ColorNG;
                }    
                else
                {
                    if (rs.IsOK == true)
                        if(IsColorAllObjLabel)
                        clShow = cl;
                    else
                        clShow = Global.ParaShow.ColorOK;

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
                if (CropOffSetX < 0 || CropOffSetY < 0)
                {
               
                    mat.Translate(CropOffSetX, CropOffSetY);
                    gc.Transform = mat;
                }
                
                      

                int index = labelItems.FindIndex(item => string.Equals(item.Name, rs.Name, StringComparison.OrdinalIgnoreCase));
               
                if (index > -1)
                {
                    LabelItem item = labelItems[index];

                    if (item.IsY)
                    {
                        Point p1 = new Point(0, item.ValueY);
                        Point p2 = new Point(50, item.ValueY);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.SkyBlue, Global.ParaShow.ThicknessLine));
                    }
                    if (item.IsX)
                    {
                        Point p1 = new Point(item.ValueX, 0);
                        Point p2 = new Point(item.ValueX, 50);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.SkyBlue, Global.ParaShow.ThicknessLine));
                    }
                    if (item.IsYMax)
                    {
                        Point p1 = new Point(0, item.ValueYMax);
                        Point p2 = new Point(50, item.ValueYMax);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, Global.ParaShow.ThicknessLine));
                    }
                    if (item.IsXMax)
                    {
                        Point p1 = new Point(item.ValueXMax, 0);
                        Point p2 = new Point(item.ValueXMax, 50);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, Global.ParaShow.ThicknessLine));
                    }
                    if (item.IsDistance&& LineVerital != null)
                    {
                        Draws.Plus(gc, (int)rs.point.X, (int)rs.point.Y, 10, Color.Red, 2);
                        Draws.DrawPerpendicularWithDistanceText(
                            gc, pen, rs.point, LineVerital, font,
                            textBrush:new SolidBrush( Global.ParaShow.TextColor),
                            textBackBrush: new SolidBrush(Global.ParaShow.ColorInfor),
                            decimals: 1,
                            textOffsetPx: 8f
                        );
                       
                    }
                    if (item.IsHeight || item.IsWidth)
                    {
                        //mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                        //gc.Transform = mat;
                        mat.Rotate(rs.rot._rectRotation);
                        gc.Transform = mat;
                        //mat.Rotate(rot._rectRotation);
                        //gc.Transform = mat;
                        System.Drawing.Point point1 = new System.Drawing.Point((int)(rs.rot._PosCenter.X), (int)(rs.rot._PosCenter.Y - rs.rot._rect.Height / 2));
                        System.Drawing.Point point2 = new System.Drawing.Point((int)(rs.rot._PosCenter.X), (int)(rs.rot._PosCenter.Y + rs.rot._rect.Height / 2));
                        System.Drawing.Point point3 = new System.Drawing.Point((int)(rs.rot._PosCenter.X - rs.rot._rect.Width / 2), (int)(rs.rot._PosCenter.Y - rs.rot._rect.Height / 2));
                        System.Drawing.Point point4 = new System.Drawing.Point((int)(rs.rot._PosCenter.X + rs.rot._rect.Width / 2), (int)(rs.rot._PosCenter.Y - rs.rot._rect.Height / 2));
                        System.Drawing.Point point5 = new System.Drawing.Point((int)(rs.rot._PosCenter.X - rs.rot._rect.Width / 2), (int)(rs.rot._PosCenter.Y + rs.rot._rect.Height / 2));
                        System.Drawing.Point point6 = new System.Drawing.Point((int)(rs.rot._PosCenter.X + rs.rot._rect.Width / 2), (int)(rs.rot._PosCenter.Y + rs.rot._rect.Height / 2));
                        gc.DrawLine(new Pen(clShow, 8), point1, point2);
                        gc.DrawLine(new Pen(clShow, 8), point3, point4);
                        gc.DrawLine(new Pen(clShow, 8), point5, point6);
                        mat.Translate(rs.rot._PosCenter.X, rs.rot._PosCenter.Y);
                        gc.Transform = mat;
                        String content = rs.rot._rect.Height + " px";
                         font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                        SizeF sz1 = gc.MeasureString(content, font);
                         gc.DrawString(content, font, new SolidBrush(Global.ParaShow.ColorInfor) , new System.Drawing.Point((int)(rs.rot._rect.X + rs.rot._rect.Width / 2), (int)(rs.rot._rect.Y + rs.rot._rect.Height / 2 - sz1.Height / 2)));
                        String label =rs.Name;
                        sz1 = gc.MeasureString(label, font);
                        gc.FillRectangle(new SolidBrush(clShow), new RectangleF((int)(rs.rot._rect.X), (int)(rs.rot._rect.Y - sz1.Height-2), sz1.Width, sz1.Height +4));
                        gc.DrawString(label, font, new SolidBrush(Color.White), new System.Drawing.Point((int)(rs.rot._rect.X ), (int)(rs.rot._rect.Y - sz1.Height-2)));
                        //String valueScore = Math.Round(ResultItem[i].Score, 1) + "%";
                        //if (!Global.ParaShow.IsShowScore) valueScore = "";
                        //if (!Global.ParaShow.IsShowLabel) label = "";
                        //Draws.Box3Label(gc, rs.rot._rect, label, valueScore, (int)(ResultItem[i].Area / 100) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine, Global.ParaShow.FontSize, 20, Global.ParaShow.IsShowDetail);//("+Math.Round( ResultItem[i].Percent) + "%)
                        gc.ResetTransform();
                    }
                   
                    else
                    {
                      
                        //  mat = new Matrix();
                        //mat = new Matrix();
                        if(IsCropSingle)
                        {
                        
                           
                            mat.Translate(listRotScan[i]._PosCenter.X, listRotScan[i]._PosCenter.Y);
                            mat.Translate(listRotScan[i]._rect.X, listRotScan[i]._rect.Y);
                            mat.Rotate(listRotScan[i]._rectRotation);
                            gc.Transform = mat;
                        }    
                        mat.Translate(rs.rot._PosCenter.X, rs.rot._PosCenter.Y);
                        mat.Rotate(rs.rot._rectRotation);
                        gc.Transform = mat;
                        if (Global.ParaShow.IsShowPostion)
                            {
                                int min = (int)Math.Min(rs.rot._rect.Width / 4, rs.rot._rect.Height / 4);
                                Draws.Plus(gc, 0, 0, min, cl, Global.ParaShow.ThicknessLine);
                                String sPos = "X,Y,A _ " + rs.rot._PosCenter.X + "," + rs.rot._PosCenter.Y + "," + Math.Round(rs.rot._rectRotation, 1);
                               
                                gc.DrawString(sPos, font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 5));

                            }
                          
                        
                        //  gc.Transform = mat;
                  
                        if (!Global.IsRun  || Global.ParaShow.IsShowDetail)
                            if (rs.matProcess != null && !rs.matProcess.Empty())
                            {
                                Draws.DrawMatInRectRotateNotMatrix(gc, rs.matProcess, rs.rot, clShow, Global.ParaShow.Opacity / 100.0f);

                            }
                        font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                        String label = rs.Name;
                        String valueScore = Math.Round(rs.Score, 1) + "%";
                        if (!Global.ParaShow.IsShowScore) valueScore = "";
                        if (!Global.ParaShow.IsShowLabel) label = "";
                        if(item.IsMinColor)
                        Draws.Box3Label(gc, rs.rot._rect, label, valueScore,Math.Round( rs.PercentColor) + "%", font, clShow, brushText, 30,Global.ParaShow.ThicknessLine, Global.ParaShow.FontSize, 1,true);//("+Math.Round( ResultItem[i].Percent) + "%)
                      else if (item.IsArea)
                            Draws.Box3Label(gc, rs.rot._rect, label, valueScore, (int)(rs.Area / 100) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine, Global.ParaShow.FontSize, 1, true);//("+Math.Round( ResultItem[i].Percent) + "%)
                        else
                            Draws.Box3Label(gc, rs.rot._rect, label, valueScore, (int)(rs.Area / 100) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine, Global.ParaShow.FontSize, 1, false);//("+Math.Round( ResultItem[i].Percent) + "%)

                        //if (rs.matProcess != null)
                        //    if (!rs.matProcess.IsDisposed)
                        //        if (!rs.matProcess.Empty())
                        //        {
                        //            Bitmap myBitmap = rs.matProcess.ToBitmap();
                        //            myBitmap.MakeTransparent(Color.Black);
                        //            myBitmap = General.ChangeToColor(myBitmap, Color.Red, 0.3f);
                        //            gc.DrawImage(myBitmap, rotA._rect);
                        //        }
                        gc.ResetTransform();

                    }



                }
                else
                {
                    if (IsCropSingle)
                    {
                        mat.Translate(listRotScan[i]._PosCenter.X, listRotScan[i]._PosCenter.Y);
                        mat.Translate(listRotScan[i]._rect.X, listRotScan[i]._rect.Y);
                        mat.Rotate(listRotScan[i]._rectRotation);
                        gc.Transform = mat;
                    }
                    mat.Translate(rs.rot._PosCenter.X, rs.rot._PosCenter.Y);
                    mat.Rotate(rs.rot._rectRotation);
                    gc.Transform = mat;
                    if (Global.ParaShow.IsShowPostion)
                    {
                        int min = (int)Math.Min(rs.rot._rect.Width / 4, rs.rot._rect.Height / 4);
                        Draws.Plus(gc, 0, 0, min, cl, Global.ParaShow.ThicknessLine);
                        String sPos = "X,Y,A _ " + rs.rot._PosCenter.X + "," + rs.rot._PosCenter.Y + "," + Math.Round(rs.rot._rectRotation, 1);

                        gc.DrawString(sPos, font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 5));

                    }


                    //  gc.Transform = mat;

                    if (!Global.IsRun || Global.ParaShow.IsShowDetail)
                        if (rs.matProcess != null && !rs.matProcess.Empty())
                        {
                            Draws.DrawMatInRectRotateNotMatrix(gc, rs.matProcess, rs.rot, clShow, Global.ParaShow.Opacity / 100.0f);

                        }
                    font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                    String label = rs.Name;
                    String valueScore = Math.Round(rs.Score, 1) + "%";
                    if (!Global.ParaShow.IsShowScore) valueScore = "";
                    if (!Global.ParaShow.IsShowLabel) label = "";
                    Draws.Box3Label(gc, rs.rot._rect, label, valueScore, (int)(rs.Area / 100) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine, Global.ParaShow.FontSize, 1, false);//("+Math.Round( ResultItem[i].Percent) + "%)
                    gc.ResetTransform();


                }
                i++;


            }
            //if (rectRotates != null)
            //{
            //    gc.ResetTransform();
            //    var mat2 = new Matrix();
            //    if (!Global.IsRun)
            //    {
            //        mat2.Translate(Global.pScroll.X, Global.pScroll.Y);
            //        mat2.Global.ScaleZoom(Global.ScaleZoom, Global.ScaleZoom);
            //    }
            //    mat2.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            //    mat2.Rotate(rotA._rectRotation);
            //    gc.Transform = mat2;
            //    gc.DrawString("Count: " + rectRotates.Count() + "", new Font("Arial", 16, FontStyle.Bold), Brushes.White, new System.Drawing.Point((int)rotA._rect.X + 20, (int)rotA._rect.Y + 20));

            //}
            //gc.ResetTransform();
            //mat = new Matrix();
            //if (!Global.IsRun)
            //{
            //    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
            //    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            //}
            //mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            //mat.Rotate(rotA._rectRotation);
            //gc.Transform = mat;
            //String sContent = (int)(Index + 1) + "." + nameTool;
            //Draws.Box1Label(gc, rotA._rect, sContent, Global.fontTool, brushText, cl);
            //  Draws.Box1Label(gc, rotA._rect, sContent,Global.fontTool, Brushes.Black, Brushes.White);

            return gc;
        }

    }
}
