
using BeeCore.Funtion;
using BeeGlobal;
using CvPlus;
using OpenCvSharp;
using OpenCvSharp.Extensions;
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
using static LibUsbDotNet.Main.UsbTransferQueue;
using static OpenCvSharp.ML.DTrees;
using static System.Windows.Forms.MonthCalendar;
using Point = System.Drawing.Point;

namespace BeeCore
{
    [Serializable()]
    public class Yolo
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
        
        public void InitialYolo()
        {
           
        }
        public String pathFullModel = "";
        public  void SetModel()
        {
           
            if (rotArea == null) rotArea = new RectRotate();
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            if (labelItems==null)labelItems = new List<LabelItem>();

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
                    TypeYolo = TypeYolo.YOLO;
                    Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
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
                    Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
                  
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

        public String[] LoadNameModel(String nameTool)
        {

            if (Global.IsIntialPython&&TypeYolo==TypeYolo.YOLO)
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
            else
                return new  string[0];
        }
        public List<LabelItem> labelItems = new List<LabelItem>();
        public List<Labels> listLabelCompare = new List<Labels>();
        public int Index = -1;
        public String PathModel = "",PathLabels="",PathDataSet;
        public TypeYolo TypeYolo = TypeYolo.YOLO;
        public TypeTool TypeTool=TypeTool.Learning;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp, matTemp2, matMask;
        public List<String> Labels = new List<string>();
        private Mode _TypeMode = Mode.Pattern;
        public Compares Compare = Compares.Equal;
        public Compares CompareLine = Compares.More;
        public Compares CompareArea = Compares.More;
        public float LimitArea = 100;
        public int Epoch =100;
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
        //[NonSerialized]
        //public List<float> listScore = new List<float>();
        //[NonSerialized]
        //public List<double> listArea = new List<double>();
        //[NonSerialized]
        //public List<bool> listOK = new List<bool>();
        //[NonSerialized]
        //public List<string> listLabel = new List<string>();
        public List<string> listModels = new List<string>();
        String listMatch;
        public bool IsCheckLine = false;
        public bool IsCheckArea = false;
        public Point p1 = new Point();
        public Point p2 = new Point();
        public int yLine = 100;
       
        public String Content = "";
        public String Matching = "";
        public bool IsEnContent = false;

        //List<RectRotate> boxList = new List<RectRotate>();
        //List<float> scoreList = new List<float>();
        //List<string> labelList = new List<string>();
        [NonSerialized]
        List<ResultItem> resultTemp = new List<ResultItem>();
        public int IndexThread = 0;
        public float CropOffSetX, CropOffSetY=0;
        [NonSerialized]
        private Mat matCropTemp;
        public void DoWork(RectRotate rotCrop)
        {
            if (!Global.IsIntialPython) return;

            using (Py.GIL())
            {
                PyObject result = null;
                PyObject boxes = null;
                PyObject scores = null;
                PyObject labels = null;

                try
                {
                    // === Tính offset (như cũ) ===
                    CropOffSetX =( rotCrop._PosCenter.X - rotCrop._rect.Width/2);
                    CropOffSetY =( rotCrop._PosCenter.Y - rotCrop._rect.Height/2);
                    CropOffSetX = (CropOffSetX > 0) ? 0 :- CropOffSetX;
                    CropOffSetY = (CropOffSetY > 0) ? 0 : -CropOffSetY;

                    // === Crop ROI ===
                    using (Mat matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexThread].matRaw, rotCrop, null))
                    {
                        if (matCrop.Empty()) return;
                      
                        // --- Chuẩn hoá về 8-bit, 3 kênh BGR ---
                        // 1) nếu depth != 8U => scale về 8U
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
                        resultTemp = new List<ResultItem>();
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
                        resultTemp= ResultFilter.FilterRectRotate(resultTemp,0.7f);// = FilterRect.RemoveInnerRectRotates(boxList, 0.6f);
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
                    // Giải phóng PyObject để tránh rò rỉ
                    if (labels != null) labels.Dispose();
                    if (scores != null) scores.Dispose();
                    if (boxes != null) boxes.Dispose();
                    if (result != null) result.Dispose();
                }
            }
        }

        //public void DoWork(RectRotate rotCrop)
        //{
        //    if (Global.IsIntialPython)
        //        using (Py.GIL())
        //        {
        //            try
        //            {
        //                // --- offset như cũ ---
        //                CropOffSetX = rotCrop._PosCenter.X + rotCrop._rect.X;
        //                CropOffSetY = rotCrop._PosCenter.Y + rotCrop._rect.Y;
        //                CropOffSetX = (CropOffSetX > 0) ? 0 : -CropOffSetX;
        //                CropOffSetY = (CropOffSetY > 0) ? 0 : -CropOffSetY;

        //                // --- crop ---
        //                using (var matCrop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexThread].matRaw, rotCrop, null))
        //                {
        //                    // Đưa về CV_8U 1/3 kênh (tránh double-convert)
        //                    if (matCrop.Type().Depth != MatType.CV_8U)
        //                        Cv2.ConvertScaleAbs(matCrop, matCrop);               // 16-bit -> 8-bit nếu có
        //                    if (matCrop.Channels() == 1)
        //                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR); // YOLO thích 3 kênh

        //                    int h = matCrop.Rows, w = matCrop.Cols, ch = matCrop.Channels();
        //                    long stride = matCrop.Step(); // có thể != w*ch, đã hỗ trợ ở Python
        //                    IntPtr p = matCrop.Data;

        //                    float conf = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);
        //                    string toolName = Common.PropetyTools[IndexThread][Index].Name;

        //                    dynamic result = G.objYolo.predict((long)p, h, w, ch, (int)stride, conf, toolName);

        //                    PyObject boxes = result[0], scores = result[1], labels = result[2];
        //                    int n = (int)boxes.Length();

        //                    if (boxList == null) boxList = new List<RectRotate>(n);
        //                    else
        //                    {
        //                        boxList.Clear();
        //                        if (boxList.Capacity < n) boxList.Capacity = n; // (tùy chọn) tránh realloc
        //                    }
        //                    if (scoreList == null) scoreList = new List<float>(n);
        //                    else
        //                    {
        //                        scoreList.Clear();
        //                        if (scoreList.Capacity < n) scoreList.Capacity = n;
        //                    }

        //                    if (labelList == null) labelList = new List<string>(n);
        //                    else
        //                    {
        //                        labelList.Clear();
        //                        if (labelList.Capacity < n) labelList.Capacity = n;
        //                    }
        //                    boxList.Clear(); scoreList.Clear(); labelList.Clear();

        //                    for (int j = 0; j < n; j++)
        //                    {
        //                        var box = boxes[j];
        //                        float x1 = (float)box[0].As<double>();
        //                        float y1 = (float)box[1].As<double>();
        //                        float x2 = (float)box[2].As<double>();
        //                        float y2 = (float)box[3].As<double>();

        //                        float w2 = x2 - x1, h2 = y2 - y1;
        //                        float cx = x1 + w2 * 0.5f, cy = y1 + h2 * 0.5f;

        //                        var rt = new RectRotate(new RectangleF(-w2 / 2, -h2 / 2, w2, h2), new PointF(cx, cy), 0, AnchorPoint.None);
        //                        boxList.Add(rt);

        //                        scoreList.Add((float)scores[j].As<double>() * 100f);
        //                        labelList.Add(labels[j].ToString());
        //                    }
        //                }

        //            }
        //            catch (PythonException pyEx)
        //            {
        //                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", pyEx.ToString()));
        //            }
        //            catch (Exception ex)
        //            {
        //                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.ToString()));
        //            }
        //        }


        //}
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
        public void Complete()
        {
            if (Global.IsIntialPython)
            {
                try
                {


                    try
                    {
                        ResultItem = new List<ResultItem>();
                        //listArea = new List<double>();
                        //listOK = new List<bool>();
                        //listLabel = new List<string>();
                        rectRotates = new List<RectRotate>();
                       // listScore = new List<float>();
                        // cycleTime = (int)G.YoloPlus.Cycle;
                        Common.PropetyTools[IndexThread][Index].Results = Results.OK;
                        int i = 0;
                        numOK = 0; numNG = 0;
                        int scoreRS = 0;
                        List<String> _listLabelCompare = new List<String>();
                        if (labelItems == null)
                        {
                            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                            return;
                        }
                        Content = "";
                        //foreach (Labels label in listLabelCompare)
                        //{
                        //    if (label == null) continue;
                        //    if (!label.IsEn) continue;
                        //    _listLabelCompare.Add(label.label);
                        //}
                        foreach (ResultItem rs in resultTemp)
                        {
                            ResultItem.Add(new ResultItem(rs.Name));
                            int index = labelItems.FindIndex(item => string.Equals(item.Name, rs.Name, StringComparison.OrdinalIgnoreCase));
                            if (index > -1)
                            {
                                LabelItem item = labelItems[index];
                                if (!item.IsUse)
                                { i++; continue; }
                                bool IsOK = false;
                                if (item.IsHeight)
                                    if (rs.rot._rect.Height >= item.ValueHeight)
                                        IsOK = true;
                                if (item.IsWidth)
                                    if (rs.rot._rect.Width >= item.ValueWidth)
                                        IsOK = true;
                                if (item.IsX)
                                    if (IntersectX(rs.rot, item.ValueX))// if (rs.rot._PosCenter.X + rs.rot._rect.Width / 2 >= item.ValueX)

                                        IsOK = true;
                                if (item.IsY)
                                    if (IntersectY(rs.rot, item.ValueY)) // [i]._PosCenter.Y + rs.rot._rect.Height / 2 >= item.ValueY)
                                        IsOK = true;
                               
                                    double Area = 0;
                                if (item.IsArea)
                                {
                                    if(item.Name=="T_CHI")
                                    {
                                      
                                        Rect rect=  new Rect((int)rs.rot._PosCenter.X+(int)rs.rot._rect.X, (int)rs.rot._PosCenter.Y + (int)rs.rot._rect.Y, (int)rs.rot._rect.Width, (int)rs.rot._rect.Height);
                                        if (ResultItem[i].matProcess == null) ResultItem[i].matProcess = new Mat();
                                        if (!ResultItem[i].matProcess.Empty()) ResultItem[i].matProcess.Dispose();
                                        ResultItem[i].matProcess = matCropTemp.Clone();
                                        percent =  CalcMissingPercent_AutoMinMax(ref ResultItem[i].matProcess, rect);
                                        Area = percent * rs.rot._rect.Size.Width * rs.rot._rect.Size.Height / 100;
                                        if (Area >= item.ValueArea * 100)
                                            IsOK = true;
                                      
                                    }
                                    else
                                    {
                                        if (item.Name == "B_CHI")
                                        {
                                            Area = rs.rot._rect.Size.Width * rs.rot._rect.Size.Height;
                                            if (Area >= item.ValueArea * 100)
                                            {
                                                IsOK = true;
                                            }    
                                            else
                                            {
                                                if (item.IsCounter)
                                                {
                                                    int count = resultTemp.Count(it => it.Name == rs.Name); ;// labelList.Count(l => l == label);
                                                    if (count >= item.ValueCounter)
                                                        IsOK = true;
                                                    else
                                                        IsOK = false;
                                                }

                                            }    
                                        }
                                        else
                                        {
                                            Area = rs.rot._rect.Size.Width * rs.rot._rect.Size.Height;
                                            if (Area >= item.ValueArea * 100)
                                                IsOK = true;
                                        }    
                                          
                                      
                                    }

                                }
                                
                                if (item.Name != "B_CHI")
                                    if (item.IsCounter)
                                {
                                    int count = resultTemp.Count(it => it.Name == rs.Name); ;//  labelList.Count(l => l == label);
                                        if (count >= item.ValueCounter)
                                        IsOK = true;
                                    else
                                        IsOK = false;
                                }
                                if (!item.IsHeight && !item.IsWidth && !item.IsArea && !item.IsX && !item.IsY)
                                    IsOK = true;
                                ResultItem[i].IsOK = IsOK;
                                ResultItem[i].rot = rs.rot;
                                ResultItem[i].Score = rs.Score;
                                ResultItem[i].Area =(float) Area;
                                ResultItem[i].Percent = (float)percent;
                                rectRotates.Add(rs.rot);
                                if (IsOK)
                                {
                                    //listOK.Add(true);
                                    //rectRotates.Add(rs.rot);
                                    //listLabel.Add(label);
                                    //scoreRS += (int)scoreList[i];
                                    //listScore.Add(scoreList[i]);
                                    numOK++;
                                }
                                //else
                                //{
                                //    listOK.Add(false);
                                //    rectRotates.Add(rs.rot);
                                //    listLabel.Add(label);
                                //    scoreRS += (int)scoreList[i];
                                //    listScore.Add(scoreList[i]);

                                //}
                                //if (IsCheckLine)
                                //{
                                //    switch (CompareLine)
                                //    {
                                //        case Compares.More:

                                //            break;
                                //        case Compares.Less:
                                //            if (rs.rot._rect.Height <= yLine)
                                //            {
                                //                listOK.Add(true);
                                //                rectRotates.Add(rs.rot);
                                //                listLabel.Add(label);
                                //                scoreRS += (int)scoreList[i];
                                //                listScore.Add(scoreList[i]);
                                //                numOK++;
                                //            }
                                //            else
                                //            {
                                //                listOK.Add(false);
                                //                rectRotates.Add(rs.rot);
                                //                listLabel.Add(label);
                                //                scoreRS += (int)scoreList[i];
                                //                listScore.Add(scoreList[i]);

                                //            }
                                //            break;
                                //    }


                                //}
                                //else if (IsCheckArea)
                                //{
                                //    switch (CompareArea)
                                //    {
                                //        case Compares.More:
                                //            if (rs.rot._rect.Size.Width * rs.rot._rect.Size.Height >= LimitArea*100)
                                //            {
                                //                listOK.Add(true);
                                //                rectRotates.Add(rs.rot);
                                //                listLabel.Add(label);
                                //                scoreRS += (int)scoreList[i];
                                //                listScore.Add(scoreList[i]);
                                //                numOK++;
                                //            }
                                //            else
                                //            {
                                //                listOK.Add(false);
                                //                rectRotates.Add(rs.rot);
                                //                listLabel.Add(label);
                                //                scoreRS += (int)scoreList[i];
                                //                listScore.Add(scoreList[i]);

                                //            }
                                //            break;
                                //        case Compares.Less:
                                //            if (rs.rot._rect.Size.Width * rs.rot._rect.Size.Height <= LimitArea*100)
                                //            {
                                //                listOK.Add(true);
                                //                rectRotates.Add(rs.rot);
                                //                listLabel.Add(label);
                                //                scoreRS += (int)scoreList[i];
                                //                listScore.Add(scoreList[i]);
                                //                numOK++;
                                //            }
                                //            else
                                //            {
                                //                listOK.Add(false);
                                //                rectRotates.Add(rs.rot);
                                //                listLabel.Add(label);
                                //                scoreRS += (int)scoreList[i];
                                //                listScore.Add(scoreList[i]);

                                //            }
                                //            break;
                                //    }


                                //}
                                //else
                                //{
                                //    listOK.Add(true);
                                //    Content += label;
                                //    rectRotates.Add(rs.rot);
                                //    listLabel.Add(label);
                                //    scoreRS += (int)scoreList[i];
                                //    listScore.Add(scoreList[i]); numOK++;
                                //}

                            }
                            i++;
                        }
                        //if (IsArrangeBox)
                        //{
                        //    List<RotatedBoxInfo> combined = new List<RotatedBoxInfo>();

                        //    for (int j = 0; j < rectRotates.Count; j++)
                        //    {
                        //        combined.Add(new RotatedBoxInfo
                        //        {
                        //            Box = rectRotates[j],
                        //            Label = ResultItem[j].Name,
                        //            Score = ResultItem[j].Score
                        //        });
                        //    }
                        //    switch (ArrangeBox)
                        //    {
                        //        case ArrangeBox.X_Left_Rigth:
                        //            // Sort theo X tăng dần (trái → phải)
                        //            combined = combined.OrderBy(b => b.Box._PosCenter.X).ToList();
                        //            break;
                        //        case ArrangeBox.X_Right_Left:
                        //            // Sort theo X giảm dần (phải → trái)
                        //            combined = combined.OrderByDescending(b => b.Box._PosCenter.X).ToList();

                        //            break;
                        //        case ArrangeBox.Y_Left_Rigth:
                        //            // Sort theo Y tăng dần (trên → dưới)
                        //            combined = combined.OrderBy(b => b.Box._PosCenter.Y).ToList();
                        //            break;
                        //        case ArrangeBox.Y_Right_Left:
                        //            combined = combined.OrderByDescending(b => b.Box._PosCenter.Y).ToList();
                        //            break;
                        //    }
                        //    rectRotates = combined.Select(b => b.Box).ToList();
                        //    listLabel = combined.Select(b => b.Label).ToList();
                        //    listScore = combined.Select(b => b.Score).ToList();
                        //    Content = "";
                        //    foreach (string s in listLabel)
                        //        Content += s;
                        //}
                        Common.PropetyTools[IndexThread][Index].ScoreResult = (int)(scoreRS / (rectRotates.Count() * 1.0));
                        if (Common.PropetyTools[IndexThread][Index].ScoreResult < 0) Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
                        Common.PropetyTools[IndexThread][Index].Results = Results.OK;
                        switch (Compare)
                        {
                            case Compares.Equal:
                                if (numOK != NumObject)
                                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                                break;
                            case Compares.Less:
                                if (numOK >= NumObject)
                                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                                break;
                            case Compares.More:
                                if (numOK <= NumObject)
                                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                                break;
                        }
                        if (IsEnContent)
                        {
                            if (Matching != Content)
                            {
                                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                            }
                        }

                        G.IsChecked = true;
                        // MessageBox.Show($"Predict xong: {boxes.len()} boxes");
                    }
                    catch (Exception ex)
                    {

                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.Message));
                        // Global.Ex = "Complete_Learning" + ex.Message;
                        // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {

                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.Message));
                    //  Global.Ex = "Complete_Learning" + ex.Message;
                    // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
                }
            }
            else
            {

                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", "No Initial"));
                //  Global.Ex = "No Initial PY";
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }

        }
        //public void Complete()
        //{
        //    if (Global.IsIntialPython)
        //    {
        //        try
        //        {


        //            try
        //            {
        //                listOK = new List<bool>();
        //                listLabel = new List<string>();
        //                rectRotates = new List<RectRotate>();
        //                listScore = new List<float>();
        //                // cycleTime = (int)G.YoloPlus.Cycle;
        //                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
        //                int i = 0;
        //                int numOK = 0, numNG = 0;
        //                int scoreRS = 0;
        //                List<String> _listLabelCompare = new List<String>();
        //                if (labelItems == null)
        //                {
        //                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                    return;
        //                }
        //                Content = "";
        //                //foreach (Labels label in listLabelCompare)
        //                //{
        //                //    if (label == null) continue;
        //                //    if (!label.IsEn) continue;
        //                //    _listLabelCompare.Add(label.label);
        //                //}
        //                foreach (String label in labelList)
        //                {
        //                    String labelConvert = label;
        //                    if(TypeYolo==TypeYolo.RCNN)
        //                    {
        //                        int indexLabel = Convert.ToInt32(label);

        //                        labelConvert = labelItems[indexLabel-1].Name;
        //                    }    
        //                    int index = labelItems.FindIndex(item =>string.Equals(item.Name, labelConvert, StringComparison.OrdinalIgnoreCase));
        //                    if (index>-1)
        //                    {
        //                        LabelItem item = labelItems[index];
        //                        if (!item.IsUse)
        //                        { i++; continue; }
        //                        bool IsOK = false;
        //                        if (item.IsHeight)
        //                            if (rs.rot._rect.Height >= item.ValueHeight)
        //                                IsOK = true;
        //                        if (item.IsWidth)
        //                            if (rs.rot._rect.Width >= item.ValueWidth)
        //                                IsOK = true;
        //                        if (item.IsArea)
        //                            if (rs.rot._rect.Size.Width * rs.rot._rect.Size.Height >= item.ValueArea * 100)
        //                                IsOK = true;
        //                        if(!item.IsHeight&&!item.IsWidth&&!item.IsArea)
        //                            IsOK = true;
        //                        if (IsOK)
        //                        {
        //                            listOK.Add(true);
        //                            rectRotates.Add(rs.rot);
        //                            listLabel.Add(labelConvert);
        //                            scoreRS += (int)scoreList[i];
        //                            listScore.Add(scoreList[i]);
        //                            numOK++;
        //                        }
        //                        else
        //                        {
        //                            listOK.Add(false);
        //                            rectRotates.Add(rs.rot);
        //                            listLabel.Add(labelConvert);
        //                            scoreRS += (int)scoreList[i];
        //                            listScore.Add(scoreList[i]);

        //                        }
                           
        //                    }
        //                    i++;
        //                }
        //                int k = 0; BitsResult = new bool[16];
        //                foreach (bool Iss in listOK)
        //                {
        //                    BitsResult[k] = Iss;
        //                        k++;
        //                }
        //                if (IsArrangeBox)
        //                {
        //                    List<RotatedBoxInfo> combined = new List<RotatedBoxInfo>();

        //                    for (int j = 0; j < rectRotates.Count; j++)
        //                    {
        //                        combined.Add(new RotatedBoxInfo
        //                        {
        //                            Box = rectRotates[j],
        //                            Label = listLabel[j],
        //                            Score = listScore[j]
        //                        });
        //                    }
        //                    switch (ArrangeBox)
        //                    {
        //                        case ArrangeBox.X_Left_Rigth:
        //                            // Sort theo X tăng dần (trái → phải)
        //                            combined = combined.OrderBy(b => b.Box._PosCenter.X).ToList();
        //                            break;
        //                        case ArrangeBox.X_Right_Left:
        //                            // Sort theo X giảm dần (phải → trái)
        //                            combined = combined.OrderByDescending(b => b.Box._PosCenter.X).ToList();

        //                            break;
        //                        case ArrangeBox.Y_Left_Rigth:
        //                            // Sort theo Y tăng dần (trên → dưới)
        //                            combined = combined.OrderBy(b => b.Box._PosCenter.Y).ToList();
        //                            break;
        //                        case ArrangeBox.Y_Right_Left:
        //                            combined = combined.OrderByDescending(b => b.Box._PosCenter.Y).ToList();
        //                            break;
        //                    }
        //                    rectRotates = combined.Select(b => b.Box).ToList();
        //                    listLabel = combined.Select(b => b.Label).ToList();
        //                    listScore = combined.Select(b => b.Score).ToList();
        //                    Content = "";
        //                    foreach (string s in listLabel)
        //                        Content += s;
        //                }
        //                Common.PropetyTools[IndexThread][Index].ScoreResult = (int)(scoreRS / (rectRotates.Count() * 1.0));
        //                if (Common.PropetyTools[IndexThread][Index].ScoreResult < 0) Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
        //                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
        //                switch (Compare)
        //                {
        //                    case Compares.Equal:
        //                        if (numOK != NumObject)
        //                            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                        break;
        //                    case Compares.Less:
        //                        if (numOK >= NumObject)
        //                            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                        break;
        //                    case Compares.More:
        //                        if (numOK <= NumObject)
        //                            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                        break;
        //                }
        //                if (IsEnContent)
        //                {
        //                    if (Matching != Content)
        //                    {
        //                        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //                    }
        //                }

        //                G.IsChecked = true;
        //                // MessageBox.Show($"Predict xong: {boxes.len()} boxes");
        //            }
        //            catch (Exception ex)
        //            {
        //                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.ToString()));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.ToString()));
        //            // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning","No Initial PY"));
           
        //        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
        //    }    
               
        //}

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
            Brush brushText = new SolidBrush(Global.Config.TextColor);
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
            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
                Draws.Box2Label(gc, rotA, nameTool,"Count: "+ numOK, font, cl, brushText, Global.Config.FontSize, Global.Config.ThicknessLine);

          //  Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl,  Global.Config.ThicknessLine);
            int i = 0;
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
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, 5));
                    }
                    if (item.IsX)
                    {
                        Point p1 = new Point(item.ValueX, 0);
                        Point p2 = new Point(item.ValueX, 50);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, 5));
                    }
                    gc.ResetTransform();  
            }
            foreach (RectRotate rot in rectRotates)
            {
                Color clShow = Global.Config.ColorNone;
                if (ResultItem[i].IsOK == true)
                    clShow = cl;
              
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
                
                      

                int index = labelItems.FindIndex(item => string.Equals(item.Name, ResultItem[i].Name, StringComparison.OrdinalIgnoreCase));
               
                if (index > -1)
                {
                    LabelItem item = labelItems[index];

                    if (item.IsY)
                    {
                        Point p1 = new Point(0, item.ValueY);
                        Point p2 = new Point(50, item.ValueY);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, 5));
                    }
                    if (item.IsX)
                    {
                        Point p1 = new Point(item.ValueX, 0);
                        Point p2 = new Point(item.ValueX, 50);
                        Draws.DrawInfiniteLine(gc, p1, p2, new Rectangle(0, 0, (int)rotA._rect.Width, (int)rotA._rect.Height), new Pen(Color.Blue, 5));
                    }
                    if (item.IsHeight || item.IsWidth)
                    {
                        //mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                        //gc.Transform = mat;
                        mat.Rotate(rot._rectRotation);
                        gc.Transform = mat;
                        //mat.Rotate(rot._rectRotation);
                        //gc.Transform = mat;
                        System.Drawing.Point point1 = new System.Drawing.Point((int)(rot._PosCenter.X), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                        System.Drawing.Point point2 = new System.Drawing.Point((int)(rot._PosCenter.X), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                        System.Drawing.Point point3 = new System.Drawing.Point((int)(rot._PosCenter.X - rot._rect.Width / 2), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                        System.Drawing.Point point4 = new System.Drawing.Point((int)(rot._PosCenter.X + rot._rect.Width / 2), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                        System.Drawing.Point point5 = new System.Drawing.Point((int)(rot._PosCenter.X - rot._rect.Width / 2), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                        System.Drawing.Point point6 = new System.Drawing.Point((int)(rot._PosCenter.X + rot._rect.Width / 2), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                        gc.DrawLine(new Pen(clShow, 8), point1, point2);
                        gc.DrawLine(new Pen(clShow, 8), point3, point4);
                        gc.DrawLine(new Pen(clShow, 8), point5, point6);
                        mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                        gc.Transform = mat;
                        String content = rot._rect.Height + " px";
                         font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
                        SizeF sz1 = gc.MeasureString(content, font);
                        gc.DrawString(content, font, new SolidBrush(clShow), new System.Drawing.Point((int)(rot._rect.X + rot._rect.Width / 2), (int)(rot._rect.Y + rot._rect.Height / 2 - sz1.Height / 2)));

                        gc.ResetTransform();
                    }
                    else
                    {
                      
                        //  mat = new Matrix();
                        //mat = new Matrix();
                        mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                      //  gc.Transform = mat;
                        mat.Rotate(rot._rectRotation);
                        gc.Transform = mat;
                        if (!Global.IsRun  || Global.Config.IsShowDetail)
                            if (ResultItem[i].matProcess != null && !ResultItem[i].matProcess.Empty())
                            {
                                Draws.DrawMatInRectRotateNotMatrix(gc, ResultItem[i].matProcess, rot, clShow, Global.Config.Opacity / 100.0f);

                            }
                        font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
                        String label = ResultItem[i].Name;
                        String valueScore = Math.Round(ResultItem[i].Score, 1) + "%";
                        if (!Global.Config.IsShowScore) valueScore = "";
                        if (!Global.Config.IsShowLabel) label = "";
                        Draws.Box3Label(gc, rot._rect, label, valueScore, (int)(ResultItem[i].Area/100) + "px", font, clShow, brushText, 30,Global.Config.ThicknessLine, Global.Config.FontSize, 1, Global.Config.IsShowDetail);//("+Math.Round( ResultItem[i].Percent) + "%)
                        gc.ResetTransform();

                    }



                }
                i++;

                //else
                //{


                //    int index = i + 1;
                //    Color clShow = Color.Red;
                //    if (!listOK[i])
                //        clShow = Color.LightGray;
                //    else
                //        clShow = cl;
                //    //String content = "(" + listLabel[i] + ") \n" + Math.Round(listScore[i], 1) + "%";
                //    //if (IsCheckArea)
                //    //    content = rot._rect.Height + " px";
                //    //  Font font = new Font("Arial", 30, FontStyle.Bold);
                //    //  SizeF sz2 = gc.MeasureString(content, font);

                //    //  Draws.Box1Label(gc, rot._rect, Math.Round(listScore[i], 1) + "%", Global.fontRS, brushText, Brushes.Transparent, true);
                //    i++;
                //    //gc.FillEllipse(Brushes.Black, -3, -3, 6, 6);
                //    gc.ResetTransform();
                //}

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
