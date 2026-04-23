
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
        [NonSerialized]
        public int MaxThread = 5;
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
		[NonSerialized]
		private NativeRCNN NativeRCNN;
		[NonSerialized]
        private NativeRCNN.RCNNBox[] RCNNBoxes;
        public int NumThreadCPU = 16;
        public  void SetModel( bool IsAgain=false)
        {
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
            Common.TryGetTool(IndexThread, Index).StepValue = 1;
            Common.TryGetTool(IndexThread, Index).MinValue = 0;
            Common.TryGetTool(IndexThread, Index).MaxValue = 100;
            if (labelItems==null)labelItems = new List<LabelItem>();
            if (ListRotMask == null)
                ListRotMask = new List<RectRotate>();
            if (ListRotCrop == null)
                ListRotCrop = new List<RectRotate>();
            try
            {
                Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.NotInitial;

                //            if (pathFullModel.Trim().Contains(".pth"))
                //            {
                //                TypeYolo = TypeYolo.RCNN;

                //            }
                //            else if (pathFullModel.Trim().Contains(".pt"))
                //            {
                //                TypeYolo = TypeYolo.YOLO;

                //            }
                //            else
                //{
                //	TypeYolo = TypeYolo.Onnx;
                //}
                if (IsIniYolo&& !IsAgain)
                    return;
                switch (TypeYolo)
					{
                    case TypeYolo.YOLO:
						using (Py.GIL())
						{

							if (!File.Exists(pathFullModel))
							{

								Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;
								return;
							}

							G.objYolo.load_model(Common.TryGetTool(IndexThread, Index).Name, pathFullModel, (int)TypeYolo);
							Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;

						}
						break;
					case TypeYolo.Onnx:
						try
						{
							NumThreadCPU = 16;
							String pathModel = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathFullModel);
							pathModel += "\\best.xml";
							if (File.Exists(pathModel))
							{
								NativeOnnx = new NativeYolo(pathModel, 0, 0, NumThreadCPU);

								NativeOnnx.Warmup(10);
								OnnxBoxes = new NativeYolo.YoloBox[200];

							}

							Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;

						}
						catch (Exception ex)
						{
							MessageBox.Show( ex.Message);
						}

						break;
					case TypeYolo.RCNN:
						try
						{
							NumThreadCPU = 16;
							String pathModel = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathFullModel);
							pathModel += "\\best.xml";
							if (File.Exists(pathModel))
							{
								NativeRCNN = new NativeRCNN(pathModel, 1333, 800, 6, NumThreadCPU);

								NativeRCNN.Warmup(10);
								RCNNBoxes = new NativeRCNN.RCNNBox[200];

							}
						Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;

						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message);
						}

						break;
				}



                if (Global.IsIntialPython)

                SetListTemp(); IsIniYolo = true;

			}
                catch (PythonException pyEx)
                {
                       MessageBox.Show("Python Error: " + pyEx.Message);
                }
                catch (Exception ex)
                {
                      MessageBox.Show("Error: " + ex.Message);
                }
            Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;

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

                            // Dùng list() d? ép dict_values v? list
                            PyObject obj = Py.Import("builtins").GetAttr("list").Invoke(result.InvokeMethod("values"));
                            var labels = new List<string>();
                            int counts = (int)obj.Length();
                            for (int j = 0; j < counts; j++)
                            {

                                labels.Add(obj[j].ToString());  // ho?c item.As<string>() n?u b?n ch?c ch?n là string
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
        public LineDirScan LineDirScan { set; get; }
        public static Mat CropRoiView(Mat src, RectRotate rot)
        {
            Rect roi = new Rect(new OpenCvSharp. Point(rot._PosCenter.X - rot._rect.Width / 2, rot._PosCenter.Y - rot._rect.Height / 2), new OpenCvSharp. Size(rot._rect.Width, rot._rect.Height));
            if (src == null || src.Empty()) return new Mat();
            return new Mat(src, roi); // view, dùng xong nh? Dispose
        }
        public static Mat CropRoiViewAuto(Mat src, RectRotate rot)
        {
            if (src == null || src.Empty() || rot == null)
                return new Mat();

            // Tính ROI axis-aligned t? center + width/height
            int x = (int)Math.Round(rot._PosCenter.X - rot._rect.Width / 2.0);
            int y = (int)Math.Round(rot._PosCenter.Y - rot._rect.Height / 2.0);
            int w = (int)Math.Round(rot._rect.Width);
            int h = (int)Math.Round(rot._rect.Height);

            if (w <= 0 || h <= 0)
                return new Mat();

            // Clamp vào trong ?nh
            int x1 = Math.Max(0, x);
            int y1 = Math.Max(0, y);
            int x2 = Math.Min(src.Width, x + w);
            int y2 = Math.Min(src.Height, y + h);

            int cw = x2 - x1;
            int ch = y2 - y1;

            // N?u hoàn toàn n?m ngoài ?nh
            if (cw <= 0 || ch <= 0)
                return new Mat();

            Rect roi = new Rect(x1, y1, cw, ch);
            return new Mat(src, roi); // view, dùng xong nh? Dispose
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
                        // Validate tru?c, nhung KHÔNG return tru?c khi FreeBuffer
                        if (ptr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                        {
                            return 0; // finally phía du?i v?n ch?y d? FreeBuffer n?u c?n
                        }

                        MatType mt = (c == 1) ? MatType.CV_8UC1
                                   : (c == 3) ? MatType.CV_8UC3
                                              : MatType.CV_8UC4;

                        using (var mNative = new Mat(h, w, mt, ptr, s))
                        {
                            matProcess = mNative.Clone(); // bây gi? d? li?u dã thu?c v? OpenCV (managed)
                        }
                    }
                    finally
                    {
                        // GI?I PHÓNG B? NH? DO native C?P PHÁT — luôn luôn!
                        if (ptr != IntPtr.Zero)
                        {
                            ColorAreaPP.FreeBuffer(ptr);
                            ptr = IntPtr.Zero;
                        }
                    }

                    // H?u x? lý:
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

        private static System.Drawing.Rectangle GetAxisRect(RectRotate rot)
        {
            if (rot == null)
                return System.Drawing.Rectangle.Empty;

            int x = (int)Math.Round(rot._PosCenter.X - rot._rect.Width / 2.0f);
            int y = (int)Math.Round(rot._PosCenter.Y - rot._rect.Height / 2.0f);
            int w = (int)Math.Round(rot._rect.Width);
            int h = (int)Math.Round(rot._rect.Height);

            if (w <= 0 || h <= 0)
                return System.Drawing.Rectangle.Empty;

            return new System.Drawing.Rectangle(x, y, w, h);
        }

        private static bool HasAxisOverlap(RectRotate a, RectRotate b)
        {
            System.Drawing.Rectangle rectA = GetAxisRect(a);
            System.Drawing.Rectangle rectB = GetAxisRect(b);

            if (rectA.IsEmpty || rectB.IsEmpty)
                return false;

            return rectA.IntersectsWith(rectB);
        }

        private static List<ResultItem> GetLabelMarkItems(
            ResultItem target,
            IEnumerable<ResultItem> source,
            HashSet<string> labelMarkNames)
        {
            if (target == null || target.rot == null || source == null || labelMarkNames == null || labelMarkNames.Count == 0)
                return new List<ResultItem>();

            return source
                .Where(x =>
                    x != null &&
                    !object.ReferenceEquals(x, target) &&
                    x.rot != null &&
                    !string.IsNullOrEmpty(x.Name) &&
                    labelMarkNames.Contains(x.Name) &&
                    HasAxisOverlap(target.rot, x.rot))
                .ToList();
        }

        private static void SubtractLabelMarkMask(ResultItem target, IEnumerable<ResultItem> labelMarks)
        {
            if (target == null || target.rot == null || target.matProcess == null || target.matProcess.Empty() || labelMarks == null)
                return;

            System.Drawing.Rectangle targetRect = GetAxisRect(target.rot);
            if (targetRect.IsEmpty)
                return;

            foreach (ResultItem mark in labelMarks)
            {
                if (mark == null || mark.rot == null)
                    continue;

                System.Drawing.Rectangle markRect = GetAxisRect(mark.rot);
                if (markRect.IsEmpty)
                    continue;

                System.Drawing.Rectangle inter = System.Drawing.Rectangle.Intersect(targetRect, markRect);
                if (inter.IsEmpty || inter.Width <= 0 || inter.Height <= 0)
                    continue;

                int x = inter.X - targetRect.X;
                int y = inter.Y - targetRect.Y;
                int w = inter.Width;
                int h = inter.Height;

                if (x < 0)
                {
                    w += x;
                    x = 0;
                }
                if (y < 0)
                {
                    h += y;
                    y = 0;
                }

                w = Math.Min(w, target.matProcess.Width - x);
                h = Math.Min(h, target.matProcess.Height - y);
                if (w <= 0 || h <= 0)
                    continue;

                using (Mat roi = new Mat(target.matProcess, new OpenCvSharp.Rect(x, y, w, h)))
                {
                    roi.SetTo(Scalar.Black);
                }
            }
        }

        private int CheckColorExcludeMarks(
            BeeCpp.ColorArea colorArea,
            ref Mat matProcess,
            Mat crop,
            ResultItem target,
            IEnumerable<ResultItem> markSource,
            HashSet<string> labelMarkNames)
        {
            int pixels = CheckColor(colorArea, ref matProcess, crop);

            if (target == null)
                return pixels;

            target.matProcess = matProcess;
            SubtractLabelMarkMask(target, GetLabelMarkItems(target, markSource, labelMarkNames));

            if (target.matProcess == null || target.matProcess.Empty())
                return 0;

            return Cv2.CountNonZero(target.matProcess);
        }

        private static string BuildScanInfo(LabelItem item, double sumArea, double sumColor, float sumWidth, float sumHeight)
        {
            if (item == null)
                return "";

            List<string> lines = new List<string>();
            if (item.IsArea)
                lines.Add("MinArea: " + (int)Math.Round(sumArea / 100.0) + "/" + item.ValueArea);
            if (item.IsMinColor)
                lines.Add("MinColor: " + (int)Math.Round(sumColor) + "/" + item.ValueMinColor);
            if (item.IsHeight)
                lines.Add("MinHeight: " + (int)Math.Round(sumHeight) + "/" + item.ValueHeight);
            if (item.IsWidth)
                lines.Add("MinWidth: " + (int)Math.Round(sumWidth) + "/" + item.ValueWidth);

            return string.Join(Environment.NewLine, lines);
        }

        private static void DrawScanInfo(Graphics graphics, RectRotate rect, Font font, Brush textBrush, Color backColor, int opacity)
        {
            if (graphics == null || rect == null || string.IsNullOrWhiteSpace(rect.Infor))
                return;

            string[] lines = rect.Infor.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
                return;

            int alpha = Math.Max(0, Math.Min(255, opacity));
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(alpha, backColor.R, backColor.G, backColor.B)))
            {
                float y = rect._rect.Bottom + 2;
                foreach (string line in lines)
                {
                    SizeF size = graphics.MeasureString(line, font);
                    RectangleF bg = new RectangleF(rect._rect.Left, y, size.Width + 4, size.Height + 2);
                    graphics.FillRectangle(bgBrush, bg);
                    graphics.DrawString(line, font, textBrush, bg.Left + 2, bg.Top + 1);
                    y += size.Height + 2;
                }
            }
        }

       public HSV HSV = new HSV();
        [NonSerialized]
        public bool IsIniYolo = false;
        public void SetListTemp()
        {

           // using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
            {

                foreach (LabelItem lb in labelItems)
                {
                    if (lb.ListHSV == null)
                        lb.ListHSV = new List<HSV>();

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


                          if(lb.ListInsideBox!=null)
                            {
                                if (lb.ListInsideBox.Count >= 0)
                                {
                                    for (int i = 0; i < lb.ListInsideBox.Count; i++)
                                    {

                                        if (lb.ListHSV == null) continue;
                                        lb.ListColorArea.Add(new BeeCpp.ColorArea());

                                        HSVCli[] arrHSV = new HSVCli[lb.ListHSV.Count];
                                        int h = 0;
                                        foreach (HSV hSV in lb.ListHSV)
                                        {
                                            arrHSV[h] = new HSVCli();
                                            arrHSV[h].H = hSV.H;
                                            arrHSV[h].S = hSV.S;
                                            arrHSV[h].V = hSV.V;
                                            h++;
                                        }
                                        SetTemp(lb.ListColorArea[lb.ListColorArea.Count - 1], arrHSV, lb.ValueExternColor);

                                        //    if (!lb.IsCounter)
                                        //{


                                        //}
                                        //else
                                        //{
                                        //    for (int j = 0; j < lb.ValueCounter; j++)
                                        //    {
                                        //        lb.ListColorArea.Add(new BeeCpp.ColorArea());
                                        //        if (IsIni )
                                        //            lb.ListTempColor.Add(0);
                                        //            HSVCli[] arrHSV = new HSVCli[lb.ListHSV.Count];
                                        //            int h = 0;
                                        //            foreach (HSV hSV in lb.ListHSV)
                                        //            {
                                        //                arrHSV[h] = new HSVCli();
                                        //                arrHSV[h].H = hSV.H;
                                        //                arrHSV[h].S = hSV.S;
                                        //                arrHSV[h].V = hSV.V;
                                        //                h++;
                                        //            }
                                        //            SetTemp(lb.ListColorArea[lb.ListColorArea.Count - 1], arrHSV, lb.ValueExternColor);

                                        //    }
                                        //}

                                    }

                                }
                            }

                          else
                            {
                                lb.ListColorArea.Add(new BeeCpp.ColorArea());

                                HSVCli[] arrHSV = new HSVCli[lb.ListHSV.Count];
                                int h = 0;
                                foreach (HSV hSV in lb.ListHSV)
                                {
                                    arrHSV[h] = new HSVCli();
                                    arrHSV[h].H = hSV.H;
                                    arrHSV[h].S = hSV.S;
                                    arrHSV[h].V = hSV.V;
                                    h++;
                                }
                                SetTemp(lb.ListColorArea[lb.ListColorArea.Count - 1], arrHSV, lb.ValueExternColor);

                                //if (lb.ListColorArea == null || lb.ListTempColor.Count() == 0)
                                //{
                                //    lb.ListColorArea = new List<BeeCpp.ColorArea>();
                                //    IsIni = true; lb.ListTempColor = new List<int>();
                                //}
                            }

                        }
                        }

                }
            }
        }


        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        { try
            {

                if (!Global.IsIntialPython) return;
                if (!Global.IsRun)
                    rotCropAdjustment = rotCrop;
                if (IsLine)
                    if (rotCropAdjustment != null)
                    {
                        Line2D = new Line2DCli();
                        Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotCropAdjustment, null);
                        Mat matProcess = new Mat();

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
                        LenRS = (int)Line2D.LengthPx;

                        if (Math.Abs(LenRS - LenTemp) / (LenTemp * 1.0) > ToleranceLine)
                        {
                            Line2D.Found = false;
                            LineVerital = null;
                        }
                        if (Line2D.Found)
                        {

                            Line2D line = new Line2D(Line2D.Vx, Line2D.Vy, Line2D.X0, Line2D.Y0);

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
                                        matCrop.AssignTo(tmp8u);             // ghi dè d? li?u (OpenCvSharp: AssignTo gi? shape/type m?i)
                                    }
                                }
                                // 2) d?m b?o dúng s? kênh
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
                                        float conf = (float)(Common.TryGetTool(IndexThread, Index).Score / 100.0);
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

                                }
                                ;
                            }
                        }
                        else
                        {
                            using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
                            {

                                float conf = (float)(Common.TryGetTool(IndexThread, Index).Score / 100.0);

                                if (matCrop.Type() == MatType.CV_8UC1)
                                    Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                                if (NativeOnnx == null)
                                    return;
                                if (matCropTemp == null) matCropTemp = new Mat();
                                if (!matCropTemp.Empty()) matCropTemp.Dispose();
                                matCropTemp = matCrop.Clone();

                                int countDetect = NativeOnnx.Detect(
                                matCrop.Data,
                                matCrop.Width,
                                matCrop.Height,
                                (int)matCrop.Step(),
                                conf,
                                0.9f, true,
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
                                    resultTemp[resultTemp.Count - 1].rot = NativeYolo.RCNNBoxToRectRotate(box);
                                    resultTemp[resultTemp.Count - 1].Score = (box.score) * 100f;
                                    resultTemp[resultTemp.Count - 1].IsOK = true;

                                }

                            }
                        }
                        break;
                    case TypeYolo.RCNN:
                        if (IsCropSingle)
                        {
                        }
                        else
                        {
                            try
                            {
                                using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, rotArea, rotMask))
                                {

                                    float conf = (float)(Common.TryGetTool(IndexThread, Index).Score / 100.0);

                                    if (matCrop.Type() == MatType.CV_8UC1)
                                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                                    if (NativeRCNN == null)
                                        return;
                                    int countDetect = NativeRCNN.Detect(
                                    matCrop.Data,
                                    matCrop.Width,
                                    matCrop.Height,
                                    (int)matCrop.Step(),
                                    conf,
                                    0.9f, true,
                                    RCNNBoxes);
                                    foreach (NativeRCNN.RCNNBox box in RCNNBoxes)
                                    {
                                        if (box.score == 0) continue;
                                        string name = "";
                                        if (ListNameOnnx == null)
                                            name = "unknown";
                                        else
                                            name = ListNameOnnx.TryGetValue(box.classId, out var s) ? s : "unknown";
                                        resultTemp.Add(new BeeCore.ResultItem((name)));
                                        resultTemp[resultTemp.Count - 1].rot = NativeRCNN.RCNNBoxToRectRotate(box);
                                        resultTemp[resultTemp.Count - 1].Score = (box.score) * 100f;
                                        resultTemp[resultTemp.Count - 1].IsOK = true;

                                    }

                                }
                            }
                            catch (Exception e)
                            {
                                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Dowork-" + Common.TryGetTool(IndexThread, Index).Name, e.Message));
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
                                // === Tính offset (nhu cu) ===
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

                                        // ===== 1) Convert v? 8U =====
                                        if (matCrop.Type().Depth != MatType.CV_8U)
                                        {
                                            using (var tmp8u = new Mat())
                                            {
                                                Cv2.ConvertScaleAbs(matCrop, tmp8u);
                                                tmp8u.CopyTo(matCrop);
                                            }
                                        }

                                        // ===== 2) d?m b?o BGR =====
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
                                                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "Dowork-" + Common.TryGetTool(IndexThread, Index).Name, "Loi Khoi Tao Yolo"));
                                                return;
                                            }

                                            float conf = (float)(Common.TryGetTool(IndexThread, Index).Score / 100.0);
                                            string toolName = Common.TryGetTool(IndexThread, Index).Name ?? "default";

                                            // ===== 3) Build batch =====
                                            var matTemps = new List<Mat>();
                                            var roiList = new List<RectRotate>();
                                            //  var roiScan = new List<RectRotate>();
                                            var roiScan = new List<RectRotate>();

                                            foreach (LabelItem lb in labelItems)
                                            {
                                                if (lb.ListInsideBox != null)
                                                {
                                                    foreach (RectRotate rot in lb.ListInsideBox)
                                                        roiScan.Add(rot);
                                                }
                                            }
                                            var nameOrder = roiScan
                                            .Select((x, i) => new { x.Name, Index = i })
                                            .GroupBy(x => x.Name ?? "")
                                            .OrderBy(g => g.Min(v => v.Index))
                                            .Select((g, order) => new { Name = g.Key, Order = order })
                                            .ToDictionary(x => x.Name, x => x.Order);

                                            roiScan = roiScan
                                                .OrderBy(r => nameOrder[r.Name ?? ""])
                                                .ToList();
                                            //foreach (LabelItem lb in labelItems)
                                            //{
                                            //    if (lb.ListInsideBox != null)
                                            //    {
                                            //        foreach (RectRotate rot in lb.ListInsideBox)
                                            //            roiScan.Add(rot);
                                            //    }
                                            //}


                                            //roiScan = roiScan
                                            //    .OrderBy(r => nameOrder[r.Name ?? ""])
                                            //    .ToList();
                                            //roiScan = roiScan
                                            //.OrderBy(r => r.Name ?? "")
                                            //.ToList();
                                            foreach (var rot in roiScan)
                                            {
                                                Mat matTemp =Cropper.CropRotatedRect(matCrop, rot,null);

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
                                                int IndexScanBox = 0;
                                                String Old = "";

                                                // ===== 6) Parse k?t qu? =====
                                                for (int i = 0; i < roiList.Count; i++)
                                                {
                                                    var roi = roiList[i];

                                                  if(roi.Name!= Old)
                                                    {
                                                        Old=roi.Name;
                                                        IndexScanBox = 0;
                                                    }
                                                    dynamic dets = results[i];   // list detection c?a riêng hình i
                                                                                 // Console.WriteLine(dets);


                                                    int n = (int)dets.Length();
                                                    if (n > 0)
                                                    {
                                                       var listRect=new List<ResultItem>();
                                                        for (int k = 0; k < n; k++)
                                                        {
                                                            dynamic det = dets[k];

                                                            dynamic box = det["box"];
                                                            float x1 = (float)box[0].As<double>();
                                                            float y1 = (float)box[1].As<double>();
                                                            float x2 = (float)box[2].As<double>();
                                                            float y2 = (float)box[3].As<double>();

                                                            float bw = x2 - x1;
                                                            float bh = y2 - y1;
                                                            float cx = x1 + bw * 0.5f;
                                                            float cy = y1 + bh * 0.5f;

                                                            Point pCenter = new Point(
                                                                (int)cx,
                                                                (int)cy
                                                            );

                                                            var rt = new RectRotate(
                                                                new RectangleF(-bw / 2f, -bh / 2f, bw, bh),
                                                                new PointF(pCenter.X, pCenter.Y),
                                                                0f,
                                                                AnchorPoint.None
                                                            );

                                                            var item = new BeeCore.ResultItem(det["label"].ToString());
                                                            item.rot = rt;
                                                            item.IsOK = true;
                                                            item.Score = (float)det["score"].As<double>() * 100f;
                                                            item.IndexScanBox = IndexScanBox;
                                                            listRect.Add(item);

                                                            //  resultTemp[resultTemp.Count - 1].IndexScanBox = IndexScanBox;
                                                        }
                                                        switch (FilterBox)
                                                        {
                                                            case FilterBox.Merge:
                                                                listRect = ResultFilter.MergeSameNameOverlap(listRect, ThreshOverlap);
                                                                break;
                                                            case FilterBox.Remove:
                                                                listRect = ResultFilter.FilterRectRotate(listRect, ThreshOverlap);
                                                                break;
                                                        }
                                                        foreach(var item in listRect)
                                                        resultTemp.Add(item);
                                                    }
                                                    else
                                                    {
                                                        var item = new BeeCore.ResultItem("NG");
                                                        item.rot = roi;
                                                        item.IsOK = false;
                                                        item.Score = 0;
                                                        item.IndexScanBox = IndexScanBox;

                                                        resultTemp.Add(item);

                                                        //resultTemp[resultTemp.Count - 1].IndexScanBox = IndexScanBox;
                                                    }

                                                    IndexScanBox++;
                                                }
                                                //// ===== 5) CALL BATCH =====
                                                //dynamic results = dyn.predict_batch(pyImages, conf, toolName);

                                                //// ===== 6) Parse k?t qu? =====
                                                //for (int i = 0; i < roiList.Count; i++)
                                                //{
                                                //    var roi = roiList[i];

                                                //    var rs = results[i];
                                                //    boxes = rs["boxes"];
                                                //    scores = rs["scores"];
                                                //    labels = rs["labels"];

                                                //    int n = (int)boxes.Length();

                                                //    if (n > 0)
                                                //    {
                                                //        var b = boxes[0];

                                                //        float x1 = (float)b[0].As<double>();
                                                //        float y1 = (float)b[1].As<double>();
                                                //        float x2 = (float)b[2].As<double>();
                                                //        float y2 = (float)b[3].As<double>();

                                                //        float bw = x2 - x1;
                                                //        float bh = y2 - y1;
                                                //        float cx = x1 + bw * 0.5f;
                                                //        float cy = y1 + bh * 0.5f;

                                                //        Point pCenter = new Point(
                                                //            (int)(cx),
                                                //            (int)(cy)
                                                //        );

                                                //        var rt = new RectRotate(
                                                //            new RectangleF(-bw / 2f, -bh / 2f, bw, bh),
                                                //            new PointF(pCenter.X, pCenter.Y),
                                                //            0f, AnchorPoint.None);

                                                //        var item = new BeeCore.ResultItem(labels[0].ToString());
                                                //        item.rot = rt;
                                                //        item.IsOK = true;
                                                //        item.Score = (float)scores[0].As<double>() * 100f;

                                                //        resultTemp.Add(item);
                                                //    }
                                                //    else
                                                //    {
                                                //        var item = new BeeCore.ResultItem("NG");
                                                //        item.rot = roi;
                                                //        item.IsOK = false;
                                                //        item.Score = 0;

                                                //        resultTemp.Add(item);
                                                //    }
                                                //}
                                            }

                                            // ===== 7) Dispose sau khi Python xong =====
                                            foreach (var m in matTemps)
                                                m?.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            Global.LogsDashboard?.AddLog(
                                                new LogEntry(DateTime.Now, LeveLLog.ERROR, "Dowork-" + Common.TryGetTool(IndexThread, Index).Name, ex.ToString()));
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
                                                matCrop.AssignTo(tmp8u);             // ghi dè d? li?u (OpenCvSharp: AssignTo gi? shape/type m?i)
                                            }
                                        }

                                        // 2) d?m b?o dúng s? kênh
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
                                        // n?u dã 3 kênh BGR thì gi? nguyên

                                        int h = matCrop.Rows;
                                        int w = matCrop.Cols;
                                        int ch = matCrop.Channels(); // 3
                                        int stride = (int)matCrop.Step(); // bytes/row (có th? > w*ch)
                                        IntPtr p = matCrop.Data;

                                        float conf = (float)(Common.TryGetTool(IndexThread, Index).Score / 100.0);
                                        string toolName = Common.TryGetTool(IndexThread, Index).Name ?? "default";

                                        // === G?i YOLO (nh?n: (boxes, scores, labels)) ===
                                        // Ký hi?u: result là tuple-like (3 ph?n)
                                        dynamic dyn = G.objYolo;
                                        if (dyn == null)
                                            return;

                                        result = dyn.predict((long)p, h, w, ch, stride, conf, toolName);

                                        // Ép v? PyObject d? ch? d?ng Dispose
                                        boxes = (PyObject)result[0];
                                        scores = (PyObject)result[1];
                                        labels = (PyObject)result[2];

                                        int n = (int)boxes.Length();

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
                                        }
                                        GC.KeepAlive(matCrop);
                                    }

                                }

                            }
                            catch (PythonException pyEx)
                            {
                                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Dowork-" + Common.TryGetTool(IndexThread, Index).Name, pyEx.ToString()));
                            }
                            catch (Exception ex)
                            {
                                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Dowork-" + Common.TryGetTool(IndexThread, Index).Name, ex.ToString()));
                            }
                            finally
                            {
                                // Gi?i phóng PyObject d? tránh rò r?
                                if (labels != null) labels.Dispose();
                                if (scores != null) scores.Dispose();
                                if (boxes != null) boxes.Dispose();
                                if (result != null) result.Dispose();
                            }
                        }
                        break;
                }
                if(!IsCropSingle)
                switch (FilterBox)
                {
                    case FilterBox.Merge:
                        resultTemp = ResultFilter.MergeSameNameOverlap(resultTemp, ThreshOverlap);
                        break;
                    case FilterBox.Remove:
                        resultTemp = ResultFilter.FilterRectRotate(resultTemp, ThreshOverlap);
                        break;
                }
            }
            catch(Exception ex)
            {

            }
        }


        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        public bool IsSendResult;
        public ArrangeBox ArrangeBox=new ArrangeBox();
        public bool IsArrangeBox = false;
        public bool IsCropSingle = false;
        [NonSerialized]
        public String StringSend = "";
        public bool IsEnSendPos = false;
        public String AddPLCPos = "";
        public String AddPLCCountPoint = "";
        public async Task SendResult()
        {
            if (IsSendResult)
            {
                int i = 0; BitsResult = new bool[16];
                StringSend = "";
                foreach (LabelItem item in labelItems)
                {
                    if (i >= 16)
                        continue;
                    BitsResult[i] = item.IsOK;
                    if (item.IsOK)
                        StringSend += item.Name + ",";
                    i++;
                }
                if (Global.Comunication.Protocol.IsConnected)
                {
                    if (AddPLC != "")
                    {
                        switch (TypeSendPLC)
                        {
                            case TypeSendPLC.Bits:
                                await Global.Comunication.Protocol.WriteResultBits(AddPLC, BitsResult);
                                break;
                            case TypeSendPLC.String:
                                await Global.Comunication.Protocol.WriteResultString(AddPLC, StringSend);
                                break;
                        }
                    }

                }
            }
            if (IsEnSendPos&& valueRobots.Count()>0)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                    int numAdd = Convert2.NumberFromString(AddPLCPos);
                    String AddDefault = AddPLCPos.Replace(numAdd + "", "");
                    String Add = AddDefault + numAdd;
                    await Global.Comunication.Protocol.WriteResultInt(AddPLCCountPoint, valueRobots.Count());
                    foreach (ValueRobot value in valueRobots)
                    {

                        int[] arr = new int[6];
                        arr[0] = value.Val1;
                        arr[1] = value.Val2;
                        arr[2] = value.Val3;
                        arr[3] = value.Val4;
                        arr[4] = value.Val5;
                        arr[5] = value.Val6;
                        await Global.Comunication.Protocol.WriteResultIntArr(Add, arr);

                        numAdd += 12;
                        Add = AddDefault + numAdd;
                    }
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

            return maxX>=valueX ;   // c?t du?ng x = valueX
        }

        static bool IntersectY(RectRotate r, float valueY)
        {
            float w = r._rect.Width, h = r._rect.Height;
            double th = r._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(th), s = Math.Sin(th);

            float extY = (float)(Math.Abs(s) * (w * 0.5) + Math.Abs(c) * (h * 0.5));
            float minY = r._PosCenter.Y - extY;
            float maxY = r._PosCenter.Y + extY;

            return maxY>=valueY ;   // c?t du?ng y = valueY
        }
        static bool IntersectXMax(RectRotate r, float valueX)
        {
            float w = r._rect.Width, h = r._rect.Height;
            double th = r._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(th), s = Math.Sin(th);

            float extX = (float)(Math.Abs(c) * (w * 0.5) + Math.Abs(s) * (h * 0.5));
            float minX = r._PosCenter.X - extX;
            float maxX = r._PosCenter.X + extX;

            return maxX <= valueX;   // c?t du?ng x = valueX
        }

        static bool IntersectYMax(RectRotate r, float valueY)
        {
            float w = r._rect.Width, h = r._rect.Height;
            double th = r._rectRotation * Math.PI / 180.0;
            double c = Math.Cos(th), s = Math.Sin(th);

            float extY = (float)(Math.Abs(s) * (w * 0.5) + Math.Abs(c) * (h * 0.5));
            float minY = r._PosCenter.Y - extY;
            float maxY = r._PosCenter.Y + extY;

            return maxY <= valueY;   // c?t du?ng y = valueY
        }
        public static double CalcMissingPercent_AutoMinMax(ref Mat src, Rect bbox, double brightRatio = 0.4)
        {
            // brightRatio = ph?n tram "vùng sáng nh?t" mu?n l?y (0.2–0.4 thu?ng ok)
            if (brightRatio <= 0) brightRatio = 0.25;
            if (brightRatio >= 1) brightRatio = 0.9;

            // 1. Crop ROI t? YOLO box
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
                // 3. L?y min / max trong ROI
                double minVal, maxVal;
               OpenCvSharp. Point minLoc, maxLoc;
                Cv2.MinMaxLoc(gray, out minVal, out maxVal, out minLoc, out maxLoc);

                // Tru?ng h?p ph?ng màu (không có gì khác bi?t)
                if (Math.Abs(maxVal - minVal) < 1e-6)
                    return 0.0;

                // 4. Tính ngu?ng t d?a trên kho?ng [min, max]
                // Ví d? brightRatio = 0.25 => l?y vùng sáng nh?t 25% g?n max
                // tuong duong: t = min + (max-min)*(1 - brightRatio)
                double t = minVal + (maxVal - minVal) * (1.0 - brightRatio);

                // 5. Threshold vùng sáng (thi?u chì)
                Cv2.Threshold(gray, mask, t, 255, ThresholdTypes.Binary);
                src = mask.Clone();
                // (tu? ch?n) làm s?ch mask m?t chút
                // var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
                // Cv2.MorphologyEx(mask, mask, MorphTypes.Open, kernel);

                // 6. Ð?m pixel tr?ng = vùng thi?u chì
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
        double CalcArea(ResultItem r, LabelItem item, out float percentOut)
        {
            percentOut = 0;

            if (item == null)
                return 0;

            //if (item.Name == "T_CHI")
            //{
            //    Rect rect = new Rect(
            //        (int)r.rot._PosCenter.X + (int)r.rot._rect.X,
            //        (int)r.rot._PosCenter.Y + (int)r.rot._rect.Y,
            //        (int)r.rot._rect.Width,
            //        (int)r.rot._rect.Height);


            //    if (r.matProcess == null)
            //        r.matProcess = new Mat();
            //    if (r.matProcess.Width != 0)
            //        if (!r.matProcess.Empty())
            //            r.matProcess.Dispose();

            //    r.matProcess = matCropTemp.Clone();

            //    percentOut = (float)CalcMissingPercent_AutoMinMax(ref r.matProcess, rect);

            //    return percentOut * r.rot._rect.Width * r.rot._rect.Height / 100;
            //}

            return (int)((r.rot._rect.Width * r.rot._rect.Height)/100.0f);
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
        public static RectRotate IntersectInsideScan(RectRotate obj, RectRotate scan)
        {
            if (obj == null || scan == null)
                return null;

            RectangleF rObj = obj.GetBoundingRect();
            RectangleF rScan = scan.GetBoundingRect();

            // clamp vào scan
            float left = Math.Max(rObj.Left, rScan.Left);
            float top = Math.Max(rObj.Top, rScan.Top);
            float right = Math.Min(rObj.Right, rScan.Right);
            float bottom = Math.Min(rObj.Bottom, rScan.Bottom);

            // th?c s? không còn vùng giao
            if (right <= left || bottom <= top)
                return null;

            float w = right - left;
            float h = bottom - top;

            PointF center = new PointF(
                (left + right) * 0.5f,
                (top + bottom) * 0.5f
            );

            RectangleF localRect = new RectangleF(
                -w * 0.5f,
                -h * 0.5f,
                w,
                h
            );

            return new RectRotate(localRect, center, 0, AnchorPoint.None);
        }
        bool CheckRightRule(RectRotate scan, RectRotate obj, Dir Dir)
        {
            if (scan == null || obj == null) return false;

            PointF pLocal = scan.WorldToLocal(obj._PosCenter);

            float halfW = scan._rect.Width / 2f;

            // clamp tránh l?i âm
            float left = pLocal.X - obj._rect.Width / 2;
            float right = pLocal.X + obj._rect.Width / 2;
            int deltaL = (int)left;
            int deltaR =(int)Math.Abs( scan._rect.Width - right);
            bool IsRS = false;
            switch(Dir)
            {
                case Dir.Left:

                    if (deltaL < deltaR) IsRS= true;
                    else  IsRS =false;
                    break;
                 case Dir.Right:
                    if (deltaL > deltaR) IsRS= true;
                   else IsRS= false;
                    break;
                default:
                    IsRS = true;
                    break;
            }
          return IsRS;

        }
        [NonSerialized]
        private List<ValueRobot> valueRobots = new List<ValueRobot>();

        public async void Complete()
        {
            if (!Global.IsIntialPython)
            {
                Global.LogsDashboard.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", "No Initial"));
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                return;
            }

            try
            {
                ResultItem = new List<ResultItem>();
                numOK = 0;

                if (labelItems == null)
                {
                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                    return;
                }

                var labelMarkNames = new HashSet<string>(
                    labelItems
                        .Where(x => x != null && x.IsUse && x.IsLabelMark && !string.IsNullOrWhiteSpace(x.Name))
                        .Select(x => x.Name),
                    StringComparer.OrdinalIgnoreCase);

                //--------------------------------
                // BUILD RESULT
                //--------------------------------
                if (resultTemp != null)
                {

                    foreach (var rs in resultTemp)
                    {
                        rs.IsOK = false;
                        ResultItem.Add(new ResultItem(rs.Name)
                        {
                            matProcess = rs.matProcess,
                            IsOK = rs.IsOK,
                            rot = rs.rot,
                            IndexScanBox = rs.IndexScanBox,
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
                    //    // world mode: nên check 4 góc c?a object
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

                bool IsMaskedByScan(LabelItem item, ResultItem r)
                {
                    if (item == null || r == null || r.rot == null || item.ListInsideBox == null)
                        return false;

                    return item.ListInsideBox.Any(scan =>
                        scan != null &&
                        scan.Dir == Dir.Mask &&
                        IsInside(scan, r.rot));
                }

                //--------------------------------
                // SCANBOX (FIX CHU?N)
                //--------------------------------

                {


                    foreach (var item in labelItems)
                    {
                        if (item == null)
                            continue;

                        item.IsOK = false;
                        if (!item.IsUse)
                            continue;

                        if (item.IsLabelMark)
                            continue;

                        //--------------------------------
                        // KHÔNG CÓ SCAN BOX
                        //--------------------------------
                        if (item.ListInsideBox == null||item.ListInsideBox.Count==0 && IsCropSingle==false)
                        {
                            var objs = ResultItem
                                .Where(x => x.rot != null &&
                                            x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase) &&
                                            !IsMaskedByScan(item, x))
                                .OrderBy(x => x.rot._PosCenter.X)
                                .ToList();

                            if (objs.Count == 0)
                                continue;



                            //--------------------------------
                            // COUNTER
                            //--------------------------------


                            //--------------------------------
                            // LOOP OBJECT
                            //--------------------------------
                            for (int j = 0; j < objs.Count; j++)
                            {
                                var r = objs[j];
                                float Pecent;
                                double area = CalcArea(r, item, out Pecent);

                                r.Area = (float)area;

                                bool ok = true;

                                if (item.IsWidth)
                                    ok &= r.rot._rect.Width >= item.ValueWidth;

                                if (item.IsHeight)
                                    ok &= r.rot._rect.Height >= item.ValueHeight;

                                if (item.IsX)
                                    ok &= IntersectX(r.rot, item.ValueX);

                                if (item.IsY)
                                    ok &= IntersectY(r.rot, item.ValueY);

                                if (item.IsXMax)
                                    ok &= IntersectXMax(r.rot, item.ValueXMax);

                                if (item.IsYMax)
                                    ok &= IntersectYMax(r.rot, item.ValueYMax);
                                if (item.Name.Trim() == "BI")
                                {
                                    if (item.IsCounter)
                                    {
                                        int numNG = 0;
                                        foreach(ResultItem rs in objs)
                                        {
                                            if (r.Area < item.ValueArea)
                                                numNG++;

                                        }
                                        if(numNG>= item.ValueCounter)
                                        {
                                            objs.ForEach(rs => rs.IsOK = true);

                                            continue;
                                        }
                                        //if (objs.Count(x => x.IsOK) < item.ValueCounter)
                                        //{
                                        //    objs.ForEach(rs => rs.IsOK = false);
                                        //}
                                    }
                                }

                                if (item.IsArea)
                                {

                                    ok &= r.Area >= item.ValueArea;

                                    if(ok==false)
                                    {
                                        r.IsOK = ok;
                                        continue;

                                    }

                                }
                                if (IsLine)
                                {

                                    if (item.IsDistance)
                                    {
                                        if (LineVerital != null)
                                        {
                                            PointF point = new PointF();
                                            r.Distance = (float)Cal.DistanceLine2D_RectRotate(LineVerital, r.rot, out point);
                                            if (r.Distance <= item.ValueDistance)
                                                r.IsOK = true;
                                            else
                                                r.IsOK = false;
                                            r.point = point;

                                        }
                                        if ( !Line2D.Found)
                                            r.IsOK = true;
                                        ok &= r.IsOK;

                                    }
                                }
                                //--------------------------------
                                // AREA
                                //--------------------------------


                                //--------------------------------
                                // COLOR
                                //--------------------------------
                                if (item.IsMinColor)
                                {
                                    using (Mat crop = CropRoiView(matCropTemp, r.rot))
                                    {

                                            r.matProcess = new Mat();
                                        if(item.ListColorArea==null)
                                        {
                                            item.ListColorArea = new List<BeeCpp.ColorArea>();
                                        }
                                        while (item.ListColorArea.Count <= j)
                                        {
                                            item.ListColorArea.Add(new BeeCpp.ColorArea());
                                            HSVCli[] arrHSV = new HSVCli[item.ListHSV.Count];
                                            int h = 0;
                                            foreach (HSV hSV in item.ListHSV)
                                            {
                                                arrHSV[h] = new HSVCli();
                                                arrHSV[h].H = hSV.H;
                                                arrHSV[h].S = hSV.S;
                                                arrHSV[h].V = hSV.V;
                                                h++;
                                            }
                                            SetTemp(item.ListColorArea[item.ListColorArea.Count - 1], arrHSV, item.ValueExternColor);
                                        }
                                        int colorPixels = CheckColorExcludeMarks(item.ListColorArea[j], ref r.matProcess, crop, r, ResultItem, labelMarkNames);
                                        r.ValueColor = (int)(colorPixels / 100.0);

                                        //if (!Global.IsRun)
                                        //    item.ListTempColor[j] = val;

                                      //  int valTemp = item.ListTempColor[j];

                                        if (r.ValueColor > 0)
                                        {
                                           // float percent = Math.Abs(val - valTemp) * 100f / valTemp;
                                            ok &= r.ValueColor >= item.ValueMinColor;
                                        }
                                        else ok = false;
                                    }
                                }

                                r.IsOK = ok;
                            }
                            if (item.IsCounter)
                            {    if ( item.Name.Trim() != "BI")
                                if (objs.Count(x => x.IsOK) < item.ValueCounter)
                                {
                                    objs.ForEach(rs => rs.IsOK = false);
                                }
                            }
                            if (objs.Count(x => x.IsOK) >0)
                                item.IsOK = true;
                            else

                                item.IsOK = false;
                            numOK += objs.Count(x => x.IsOK);

                        }

                        //--------------------------------
                        // CÓ SCAN BOX
                        //--------------------------------
                        else
                        {
                            int k = 0;
                            foreach (var scan in item.ListInsideBox)
                            {
                                if (scan == null)
                                {
                                    k++;
                                    continue;
                                }

                                scan.Infor = "";
                                bool boxOK = true;   // ? reset dúng ch?

                                if (scan.Dir == Dir.Mask)
                                {
                                    scan.IsOK = true;
                                    scan.NumInside = 0;
                                    k++;
                                    continue;
                                }

                                //--------------------------------
                                // GET OBJECT TRONG BOX
                                //--------------------------------
                                List<ResultItem> objs = new List<ResultItem>();
                                bool IsNGBox = false;
                                if (IsCropSingle)
                                {
                                    if (k >= ResultItem.Count)
                                    {
                                        scan.IsOK = false;
                                        scan.NumInside = 0;
                                        k++;
                                        continue;
                                    }
                                    List<ResultItem> objTemp = ResultItem
    .Where(x => x.IndexScanBox == k)
    .ToList();
                                    //                List<ResultItem> objTemp = ResultItem
                                    //.Where(x => x.IndexScanBox == k
                                    //         && x.Name == item.Name)
                                    //.ToList();
                                    objs = objTemp
                                 .Where(x =>
                                     x.rot != null &&
                                     x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase) &&
                                     !IsMaskedByScan(item, x) &&
                                     IsInside(scan, x.rot))
                                 .OrderBy(x => x.rot._PosCenter.X)
                                 .ToList();
                                    if (objs.Count == 0)
                                    {
                                        scan.IsOK = false;
                                        scan.NumInside = 0;
                                        k++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    objs = ResultItem
                                    .Where(x =>
                                        x.rot != null &&
                                        x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase) &&
                                        !IsMaskedByScan(item, x) &&
                                        IsInside(scan, x.rot))
                                    .OrderBy(x => x.rot._PosCenter.X)
                                    .ToList();

                                //    var ObjOK = ResultItem.Where(x =>
                                //    x.rot != null &&
                                //    x.Name.Equals("OK", StringComparison.OrdinalIgnoreCase) &&
                                //    !labelMarkNames.Contains(x.Name) &&
                                //    !IsMaskedByScan(item, x) &&
                                //    IsInside(scan, x.rot))
                                //.OrderBy(x => x.rot._PosCenter.X)
                                //.ToList();
                                //    if (ObjOK.Count() > 0)
                                //    {
                                //        scan.NumInside = 0;
                                //        scan.IsOK = false;
                                //        k++;
                                //        continue;

                                //    }
                                    //    else
                                    //    {
                                    //        IsNGBox = true;
                                    //    }

                                }
                                //--------------------------------
                                // FILTER RIGHT
                                //--------------------------------
                                var objsValid = new List<ResultItem>();

                                foreach (var r in objs)
                                {
                                    bool pass = true;

                                    if (r.rot != null)
                                    {
                                        pass = CheckRightRule(scan, r.rot, scan.Dir);


                                        if (!pass)
                                        {

                                            r.IsOK = false;
                                            continue;
                                        }
                                        else
                                        {
                                            r.IsOK = true;

                                            objsValid.Add(r);
                                        }
                                    }

                                }

                                scan.NumInside = objsValid.Count;
                                foreach (var r in objsValid)
                                {
                                    float percentColor;
                                    r.Area = (float)CalcArea(r, item, out percentColor);
                                }
                                double sumArea = objsValid.Sum(x => x.Area);
                                float sumWidth = objsValid.Sum(x => x.rot != null ? x.rot._rect.Width : 0f);
                                float sumHeight = objsValid.Sum(x => x.rot != null ? x.rot._rect.Height : 0f);

                                //--------------------------------
                                // COUNTER
                                //--------------------------------
                                //if (item.IsCounter)
                                //{
                                //    if (objsValid.Count < item.ValueCounter)
                                //    {
                                //        k++;
                                //        //foreach (var r in objsValid)
                                //        //    r.IsOK = false;
                                //        numOK += objs.Count(x => x.IsOK);
                                //        scan.IsOK = false;
                                //        continue;
                                //    }
                                //}
                                //else
                                //{

                                //    if (objsValid.Count == 0)
                                //    {

                                //        k++;


                                //    }

                                //}

                                //--------------------------------
                                // LOOP OBJECT
                                //--------------------------------
                                for (int j = 0; j < objsValid.Count; j++)
                                {
                                    var r = objsValid[j];
                                    bool ok = true;

                                    if (item.IsWidth)
                                        ok &= sumWidth >= item.ValueWidth;

                                    if (item.IsHeight)
                                        ok &= sumHeight >= item.ValueHeight;

                                    if (item.IsX)
                                        ok &= IntersectX(r.rot, item.ValueX);

                                    if (item.IsY)
                                        ok &= IntersectY(r.rot, item.ValueY);

                                    if (item.IsXMax)
                                        ok &= IntersectXMax(r.rot, item.ValueXMax);

                                    if (item.IsYMax)
                                        ok &= IntersectYMax(r.rot, item.ValueYMax);
                                    if (IsLine)
                                    {

                                        if (item.IsDistance)
                                        {
                                            if (LineVerital != null)
                                            {
                                                PointF point = new PointF();
                                                r.Distance = (float)Cal.DistanceLine2D_RectRotate(LineVerital, r.rot, out point);
                                                if (r.Distance <= item.ValueDistance)
                                                    r.IsOK = true;
                                                else
                                                    r.IsOK = false;
                                                r.point = point;

                                            }
                                            if (!Line2D.Found)
                                                r.IsOK = true;
                                            ok &= r.IsOK;

                                        }
                                    }
                                    //--------------------------------
                                    // AREA
                                    //--------------------------------
                                    if (item.IsArea)
                                    {
                                        ok &= sumArea >= item.ValueArea * 100;
                                    }
                                    if (item.IsMinColor)
                                    {
                                        int colorStride = item.ValueCounter > 0 ? item.ValueCounter : Math.Max(1, objsValid.Count);
                                        int indexColor = k * colorStride + j;

                                        //--------------------------------
                                        // COLOR (fix index dúng)
                                        //--------------------------------

                                        //if(item.ListColorArea == null)
                                        //{
                                        //    item.ListTempColor = new List<int>();
                                        //    item.ListColorArea = null;
                                        //    SetListTemp();
                                        //}
                                        //if (item.ListColorArea != null)
                                        //    if (indexColor>= item.ListColorArea.Count)
                                        //{
                                        //    item.ListTempColor = new List<int>();
                                        //    item.ListColorArea = null;
                                        //    SetListTemp();
                                        //}



                                        using (Mat crop = CropRoiView(matCropTemp, r.rot))
                                        {
                                            r.matProcess = new Mat();
                                            if (item.ListColorArea == null)
                                                item.ListColorArea = new List<BeeCpp.ColorArea>();

                                            while (item.ListColorArea.Count <= indexColor)
                                            {
                                                item.ListColorArea.Add(new BeeCpp.ColorArea());
                                                HSVCli[] arrHSV = new HSVCli[item.ListHSV.Count];
                                                int h = 0;
                                                foreach (HSV hSV in item.ListHSV)
                                                {
                                                    arrHSV[h] = new HSVCli();
                                                    arrHSV[h].H = hSV.H;
                                                    arrHSV[h].S = hSV.S;
                                                    arrHSV[h].V = hSV.V;
                                                    h++;
                                                }
                                                SetTemp(item.ListColorArea[item.ListColorArea.Count - 1], arrHSV, item.ValueExternColor);
                                            }

                                            IEnumerable<ResultItem> markSource = IsCropSingle
                                                ? ResultItem.Where(x => x != null && x.IndexScanBox == k)
                                                : ResultItem.Where(x => x != null && x.rot != null && IsInside(scan, x.rot));

                                            int colorPixels = CheckColorExcludeMarks(item.ListColorArea[indexColor], ref r.matProcess, crop, r, markSource, labelMarkNames);
                                            r.ValueColor = (int)(colorPixels / 100.0);
                                            ok &= r.ValueColor >= item.ValueMinColor;

                                            //if (!Global.IsRun)
                                            //    item.ListTempColor[j] = val;

                                            //  int valTemp = item.ListTempColor[j];

                                            //if (r.ValueColor > 0)
                                            //{
                                            //    // float percent = Math.Abs(val - valTemp) * 100f / valTemp;
                                            //    ok &= r.ValueColor >= item.ValueMinColor;
                                            //}
                                            //else ok = false;
                                            //if (r.matProcess == null)
                                            //    r.matProcess = new Mat();

                                            //int val = CheckColor(item.ListColorArea[indexColor], ref r.matProcess, crop);

                                            //if (!Global.IsRun)
                                            //    item.ListTempColor[indexColor] = val;

                                            //int valTemp = item.ListTempColor[indexColor];

                                            //if (valTemp > 0)
                                            //{
                                            //    float percent = Math.Abs(val - valTemp) * 100f / valTemp;
                                            //    r.PercentColor = percent;

                                            //    if (percent > item.ValueMinColor)
                                            //    {
                                            //        ok = false;
                                            //        scan.NumInside--;
                                            //        boxOK = false;
                                            //    }
                                            //}
                                            //   else ok = false;
                                        }
                                    }

                                    r.IsOK = ok;
                                    if (!ok)
                                        boxOK = false;
                                }
                                if (item.IsMinColor)
                                {
                                    double sumColor = objsValid.Sum(x => x.ValueColor);
                                    boxOK = sumColor >= item.ValueMinColor;
                                    objsValid.ForEach(rs => rs.IsOK = boxOK);

                                }
                                scan.Infor = BuildScanInfo(item, sumArea, objsValid.Sum(x => x.ValueColor), sumWidth, sumHeight);
                                if (item.IsCounter)
                                {
                                    if (objsValid.Count(x => x.IsOK) < item.ValueCounter)
                                    {
                                        boxOK = false;
                                        objsValid.ForEach(rs => rs.IsOK = false);
                                    }
                                }

                                if (IsNGBox)
                                {
                                    numOK += 1;
                                    scan.IsOK = true;
                                    item.IsOK = true;
                                    k++;
                                }

                                else
                                {


                                scan.IsOK = boxOK;
                                k++;

                                numOK += objsValid.Count(x => x.IsOK);
                                if (objsValid.Count(x => x.IsOK) > 0)
                                    item.IsOK = true;
                                else

                                    item.IsOK = false;
                            }
                            }
                        }
                    }
                }
                valueRobots = new List<ValueRobot>();
                foreach (ResultItem rs in ResultItem)
                {
                    if (rs.IsOK)
                    {
                        int X = (int)((rs.rot._PosCenter.X/Global.Config.Scale)*1.0);
                        int Y = (int)((rs.rot._PosCenter.Y /Global.Config.Scale )* 1.0);
                        int A = (int)(rs.rot._rectRotation);

                        valueRobots.Add(new ValueRobot(X,Y,A, Convert.ToInt32(rs.IsOK), 0,0));
                    }
                }

                    //--------------------------------
                    // RESULT
                    //--------------------------------
                    Common.TryGetTool(IndexThread, Index).Results = Results.OK;

                if (Compare == Compares.Equal && numOK != NumObject)
                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;

                if (Compare == Compares.Less && numOK >= NumObject)
                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;

                if (Compare == Compares.More && numOK <= NumObject)
                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;

                if (IsLine && (!Line2D.Found))
                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;

                G.IsChecked = true;
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "Complete-" + Common.TryGetTool(IndexThread, Index).Name, ex.Message));
            }
            Common.TryGetTool(IndexThread, Index).ScoreResult = numOK;
          await  SendResult();
        }
        //public void Complete()
        //{
        //    if (!Global.IsIntialPython)
        //    {
        //        Global.LogsDashboard.AddLog(
        //            new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", "No Initial"));
        //        Common.TryGetTool(IndexThread, Index).Results = Results.NG;
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
        //            Common.TryGetTool(IndexThread, Index).Results = Results.NG;
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
        //        // PASS 3 : ScanBox x? lý chính
        //        //--------------------------------
        //        if (!IsCropSingle && listRotScan != null && listRotScan.Count > 0)
        //        {
        //            for (int scanIndex = 0; scanIndex < listRotScan.Count; scanIndex++)
        //            {
        //                var scan = listRotScan[scanIndex];

        //                // l?y object trong box
        //                var listInBox = ResultItem
        //                    .Where(x => x.rot != null &&
        //                                scan.ContainsPoint(x.rot._PosCenter))
        //                    .ToList();

        //                // ? KHÔNG có object ? FAIL luôn
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
        //        Common.TryGetTool(IndexThread, Index).Results = Results.OK;

        //        switch (Compare)
        //        {
        //            case Compares.Equal:
        //                if (numOK != NumObject)
        //                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;
        //                break;

        //            case Compares.Less:
        //                if (numOK >= NumObject)
        //                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;
        //                break;

        //            case Compares.More:
        //                if (numOK <= NumObject)
        //                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;
        //                break;
        //        }

        //        if (IsLine && !Line2D.Found)
        //            Common.TryGetTool(IndexThread, Index).Results = Results.NG;

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
            Color cl = Color.LimeGreen;
            switch (Common.TryGetTool(Global.IndexProgChoose, Index).Results)
            {
                case Results.OK:
                    cl = Global.ParaShow.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ParaShow.ColorNG;
                    break;
            }
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                Color clScan= Color.White;


                    if (listRotScan != null)
                    foreach (RectRotate rot in listRotScan)
                {
                    String cOK ="("+ rot.NumInside+ ") OK";

                    if (Global.StatusDraw==StatusDraw.Scan)
                        {
                        cOK += "-" + rot.Dir.ToString();

                        if (rot.Name == null)
                            rot.Name = "Area Limit";
                        if (rot._dragAnchor == AnchorPoint.Center && rot.Name.Trim() != "Area Limit")

                            clScan = Global.ParaShow.ColorChoose;
                        else
                        {
                            rot._dragAnchor = AnchorPoint.None;
                            clScan = Color.LightGray;
                        }
                        //if (IsCropSingle)
                        //    {
                        //        if (rot._dragAnchor == AnchorPoint.Center)
                        //            clScan = Global.ParaShow.ColorChoose;
                        //        else
                        //            clScan = Color.LightGray;
                        //    }
                        //    else
                        //{

                        //    if (rot._dragAnchor == AnchorPoint.Center&&rot.Name.Trim()!= "Area Limit")
                        //        clScan = Global.ParaShow.ColorChoose;
                        //    else
                        //    {
                        //        rot._dragAnchor = AnchorPoint.None;
                        //        clScan = Color.LightGray;
                        //    }

                        //    //if (rot.Name != "")
                        //    //    {

                        //    //        clScan = Global.ParaShow.ColorChoose;
                        //    //    }
                        //    //    else
                        //    //        clScan = Global.ParaShow.ColorNone;
                        //    }
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
                                    if (rot.Dir == Dir.Mask)
                                    {
                                        clScan = Global.ParaShow.ColorNone;
                                        cOK = "Mask";
                                    }
                                    else
                                    {


                                        if (rot.IsOK == true)
                                        {
                                            cOK = "(" + rot.NumInside + ") ";
                                            if (IsColorAllObjLabel)
                                                clScan = rot.NumInside == 0 ? Global.ParaShow.ColorNone :cl;
                                            else
                                                clScan = Global.ParaShow.ColorOK;
                                           
                                        }

                                        else
                                        {
                                            cOK = "(" + rot.NumInside + ") ";
                                            if (IsColorAllObjLabel)
                                                clScan = rot.NumInside == 0 ? Global.ParaShow.ColorNone : cl;
                                            else
                                                clScan = Global.ParaShow.ColorNG;
                                        }
                                        if (!IsColorAllObjLabel)
                                            cOK += "-" + rot.Dir.ToString();
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
                    //if (IsColorAllObjLabel)
                    //    cOK = "";

                        Draws.Box2Label(gc, rot, indexArea + "."+ rot.Name, cOK, font, clScan, brushText, Global.ParaShow.Opacity, Global.ParaShow.ThicknessLine,true);
                        if (rot.Dir != Dir.Mask)
                            DrawScanInfo(gc, rot, font, brushText, clScan, Global.ParaShow.Opacity);


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

            if (IsLine)
                if (rotCropAdjustment != null)
                {
                    bool IsHasLine =  Line2D.Found;
                    Color clLine = Global.ParaShow.ColorNG;
                    String sOK = "NG";
                    if (IsHasLine)
                    {
                        clLine = Global.ParaShow.ColorOK;
                        sOK = "OK";

                    }


                            mat = new Matrix();
                    if (!Global.IsRun)
                    {
                        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                    }
                    mat.Translate(rotCropAdjustment._PosCenter.X, rotCropAdjustment._PosCenter.Y);
                    mat.Rotate(rotCropAdjustment._rectRotation);
                    gc.Transform = mat;
                    Draws.Box2Label(gc, rotCropAdjustment, "Line", sOK, font, clLine, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);

                    //gc.DrawRectangle(new Pen(cl, Global.ParaShow.ThicknessLine), new Rectangle((int)rotCropAdjustment._rect.X, (int)rotCropAdjustment._rect.Y, (int)rotCropAdjustment._rect.Width, (int)rotCropAdjustment._rect.Height));
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


            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.TryGetTool(IndexThread, Index).Name;
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
                if (rs.Score == 0)
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
                    if (!item.IsUse)
                    {
                        i++;
                        continue;
                    }

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
                        SolidBrush clLine = new SolidBrush(Global.ParaShow.ColorInfor);
                        if(!rs.IsOK)
                            clLine = new SolidBrush(Global.ParaShow.ColorOK);
                        else
                            clLine = new SolidBrush(Global.ParaShow.ColorNG);
                        Draws.DrawPerpendicularWithDistanceText(
                                gc, pen, rs.point, LineVerital, font,
                                textBrush: new SolidBrush(Global.ParaShow.TextColor),
                                textBackBrush: clLine,
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
                        String content = (int)Math.Round(rs.rot._rect.Height) + " px";
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
                            if(rs.IndexScanBox < item.ListInsideBox.Count())
                            {
                                RectRotate rectRotate = item.ListInsideBox[rs.IndexScanBox];
                                mat.Translate(rectRotate._PosCenter.X, rectRotate._PosCenter.Y);
                                mat.Rotate(rectRotate._rectRotation);
                                mat.Translate(rectRotate._rect.X, rectRotate._rect.Y);
                                gc.Transform = mat;
                            }

                        }
                        mat.Translate(rs.rot._PosCenter.X, rs.rot._PosCenter.Y);
                        mat.Rotate(rs.rot._rectRotation);
                        gc.Transform = mat;
                        if (Global.ParaShow.IsShowPostion)
                            {
                                int min = (int)Math.Min(rs.rot._rect.Width / 4, rs.rot._rect.Height / 4);
                                Draws.Plus(gc, 0, 0, min, cl, Global.ParaShow.ThicknessLine);
                                String sPos = "X,Y,A _ " + (int)Math.Round(rs.rot._PosCenter.X) + "," + (int)Math.Round(rs.rot._PosCenter.Y) + "," + (int)Math.Round(rs.rot._rectRotation);

                                gc.DrawString(sPos, font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 5));

                            }


                        //  gc.Transform = mat;

                        if (!Global.IsRun  || Global.ParaShow.IsShowDetail)
                            if (rs.matProcess != null && !rs.matProcess.Empty())
                            {
                                Draws.DrawMatInRectRotateNotMatrix(gc, rs.matProcess, rs.rot, clShow, Global.ParaShow.Opacity / 100.0f);

                            }
                        font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                        String label =  rs.Name;
                        String valueScore = (int)Math.Round(rs.Score) + "%";
                        if (!Global.ParaShow.IsShowScore) valueScore = "";
                        if (!Global.ParaShow.IsShowLabel) label = "";

                        if(item.IsMinColor)
                        Draws.Box3Label(gc, rs.rot, label, valueScore, (int)Math.Round((double)rs.ValueColor) + "px", font, clShow, brushText, 30,Global.ParaShow.ThicknessLine,false, Global.ParaShow.FontSize, 1,true);//("+Math.Round( ResultItem[i].Percent) + "%)
                        else if (item.IsArea)
                        Draws.Box3Label(gc, rs.rot, label, valueScore, (int)Math.Round(rs.Area) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine, false, Global.ParaShow.FontSize, 1, true);//("+Math.Round( ResultItem[i].Percent) + "%)
                        else
                        Draws.Box3Label(gc, rs.rot, label, valueScore, (int)Math.Round(rs.Area) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine, false, Global.ParaShow.FontSize, 1, false);//("+Math.Round( ResultItem[i].Percent) + "%)
                        gc.ResetTransform();

                    }



                }
                else
                {
                    //if (IsCropSingle)
                    //{
                    //    mat.Translate(listRotScan[i]._PosCenter.X, listRotScan[i]._PosCenter.Y);
                    //    mat.Translate(listRotScan[i]._rect.X, listRotScan[i]._rect.Y);
                    //    mat.Rotate(listRotScan[i]._rectRotation);
                    //    gc.Transform = mat;
                    //}
                    mat.Translate(rs.rot._PosCenter.X, rs.rot._PosCenter.Y);
                    mat.Rotate(rs.rot._rectRotation);
                    gc.Transform = mat;
                    if (Global.ParaShow.IsShowPostion)
                    {
                        int min = (int)Math.Min(rs.rot._rect.Width / 4, rs.rot._rect.Height / 4);
                        Draws.Plus(gc, 0, 0, min, cl, Global.ParaShow.ThicknessLine);
                        String sPos = "X,Y,A _ " + (int)Math.Round(rs.rot._PosCenter.X) + "," + (int)Math.Round(rs.rot._PosCenter.Y) + "," + (int)Math.Round(rs.rot._rectRotation);

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
                    String valueScore = (int)Math.Round(rs.Score) + "%";
                    if (!Global.ParaShow.IsShowScore) valueScore = "";
                    if (!Global.ParaShow.IsShowLabel) label = "";
                    Draws.Box3Label(gc, rs.rot, label, valueScore, (int)Math.Round(rs.Area / 100.0) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine,false, Global.ParaShow.FontSize, 1, false);//("+Math.Round( ResultItem[i].Percent) + "%)
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
