using BeeCore.Funtion;
using BeeCore.Parameter;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static LibUsbDotNet.Main.UsbTransferQueue;
using static OpenCvSharp.ML.DTrees;
using static System.Net.Mime.MediaTypeNames;
using Point = System.Drawing.Point;
using Size = OpenCvSharp.Size;



namespace BeeCore
{
    [Serializable()]
    public class OCR
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

      
      
        public int Index = -1;
        public String PathModel = "";
        public TypeOCR TypeOCR = TypeOCR.CPU;
        public TypeTool TypeTool=TypeTool.Yolo;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp, matTemp2, matMask;
        public List<String> Labels = new List<string>();
    
        public Compares Compare = Compares.Equal;
       
        public string pathRaw;
        public TypeCrop TypeCrop;
        public bool IsOK = false;
        public bool IsAreaWhite = false;
        public int ScoreRs = 0, cycleTime;
        private int _score = 70;
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;

            }
        }
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
        public List<RectRotate> rectRotates = new List<RectRotate>();
        String[] sSplit;
        public List<float> listScore = new List<float>();
        public List<String> listLabelResult= new List<String>();
        public List<bool> listOK = new List<bool>();
        public List<List<string>> listLabel = new List<List<string>>();
        String listMatch;
        public bool IsCheckArea = false;
        public Point p1 = new Point();
        public Point p2 = new Point();
        public int yLine = 200;
        public String Matching = "";
        public String Content = "";
        public String[] listContent ;
        public String[] listMatching;
        public bool IsIni = false;
        public String nameTool = "";
        public StatusTool StatusTool = StatusTool.None;
       int scoreRS = 0;
        String exMess = "";
        public static OpenCvSharp.Point[] ConvertBoxToPoints(PyObject box)
        {
            OpenCvSharp.Point[] points = new OpenCvSharp.Point[4];
            for (int i = 0; i < 4; i++)
            {
                PyObject point = box[i];
                float x = (float)point[0].As<double>();
                float y = (float)point[1].As<double>();
                points[i] = new OpenCvSharp.Point(x, y);
            }
            
            return points;
        }
        public static Mat EnhanceImage(Mat input, double contrastFactor = 4.0, double sharpenFactor = 4.0)
        {
            // Tăng độ tương phản
            Mat contrastImg = new Mat();
            input.ConvertTo(contrastImg, MatType.CV_8UC3, contrastFactor, 0); // beta=0 không thay đổi độ sáng

            // Làm mờ để chuẩn bị sharpen
            Mat blurred = new Mat();
            Cv2.GaussianBlur(contrastImg, blurred, new Size(0, 0), 3);

            // Làm nét bằng unsharp masking
            Mat sharpened = new Mat();
            Cv2.AddWeighted(contrastImg, 1.0 + sharpenFactor, blurred, -sharpenFactor, 0, sharpened);
            Cv2.ImWrite("EnhanceImage.png", sharpened);
            return sharpened;
        }
        public int Enhance = 4;
        public static Mat PreprocessForOCR(Mat input,int clipLimit=3 ,int sigma=3,int blur=3)
        {
            // 1. Chuyển sang grayscale nếu cần
            Mat gray = new Mat();
            if (input.Channels() == 3)
                Cv2.CvtColor(input, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = input.Clone();

            // 2. Tăng tương phản bằng CLAHE
            CLAHE clahe = Cv2.CreateCLAHE(clipLimit, new Size(8, 8));
            Mat contrast = new Mat();
            clahe.Apply(gray, contrast);

            // 3. Làm sắc nét bằng Unsharp Mask
            Mat blurred = new Mat();
            Cv2.GaussianBlur(contrast, blurred, new Size(0, 0), sigma);
            Mat sharp = new Mat();
            Cv2.AddWeighted(contrast, 1.5, blurred, -0.5, 0, sharp);

            // 4. Chuyển sang đen trắng rõ ràng bằng Otsu threshold
            Mat binary = new Mat();
          //  Cv2.Threshold(sharp, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

            // 5. Lọc nhiễu bằng MedianBlur
            Mat clean = new Mat();
            Cv2.MedianBlur(sharp, clean, blur);
            Cv2.ImWrite("CropOCR.png", clean);
            return clean;
        }
        public void DoWork(RectRotate rotCrop)
        {
            using (Py.GIL())
            {
                try
                {
                    var boxList = new List<RectRotate>();
                    var scoreList = new List<float>();
                    var labelList = new List<string>();
                    int numOK = 0, numNG = 0;
                    scoreRS = 0;
                    Content = "";
                    scoreRS = 0;
                    listLabelResult = new List<String>();
                    Mat matCrop = Common.CropRotatedRectSharp(BeeCore.Common.matRaw.Clone(), new RotatedRect(new Point2f(rotCrop._PosCenter.X, rotCrop._PosCenter.Y), new Size2f(rotCrop._rect.Size.Width, rotCrop._rect.Size.Height), rotCrop._angle));
                   // matCrop = EnhanceImage(matCrop,Enhance);
                    if (matCrop.Type() != MatType.CV_8UC3)
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2RGB);
                    if (matCrop.Channels() == 1)
                    {
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2RGB);
                    }
                    //   Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGR2RGB);

                    //   cv::cvtColor(matCrop, matCrop, COLOR_BGR2RGB);
                    //  Cv2.ImWrite("crop.png", matCrop);
                    // Đảm bảo dữ liệu liên tục
                    if (!matCrop.IsContinuous())
                    {
                        matCrop = matCrop.Clone();
                    }

                    // Copy dữ liệu sang byte[]
                    int size = (int)(matCrop.Total() * matCrop.ElemSize());
                    byte[] buffer = new byte[size];
                    Marshal.Copy(matCrop.Data, buffer, 0, size);

                    int height1 = matCrop.Height;
                    int width1 = matCrop.Width;
                    //dynamic np = Py.Import("numpy");
                    ////    G.objYolo = Py.Import("Tool.Learning").ObjectDetector(); // khởi tạo trực tiếp
                    //dynamic mod = Py.Import("Tool.Learning");
                    //dynamic cls = mod.GetAttr("ObjectDetector"); // class

                    //dynamic objYolo = cls.Invoke();              // khởi tạo instance
                    //G.objYolo.load_model(nameTool, nameModel, (int)TypeYolo);
                    //_ocr.attr("find_ocr")(image_array);
                    IsOK = false;
                    listOK = new List<bool>();
                    listLabel = new List<List<string>>();
                    rectRotates = new List<RectRotate>();
                    listScore = new List<float>();

                    var npArray = G.np.array(buffer).reshape(height1, width1, 3);
                    dynamic result = G.objOCR.find_ocr(npArray,Enhance);//, (float)(Score / 100.0), nameTool
                    if (result == null) return;

                    PyObject boxes = result[0];
                    PyObject scores = result[1];
                    PyObject labels = result[2];

                    int counts = (int)boxes.Length(); int i = 0;

                    for (int j = 0; j < counts; j++)
                    {
                        listLabel.Add(new List<string>());
                        // Lấy box: (x1, y1, x2, y2)
                       // PyObject box = boxes[j];
                        //float centerX = (float)box[0][0].AsManagedObject(typeof(float));
                        //float centerY = (float)box[0][1].AsManagedObject(typeof(float));

                        //float width = (float)box[1][0].AsManagedObject(typeof(float));
                        //float height = (float)box[1][1].AsManagedObject(typeof(float));

                        //float angle = (float)box[2].AsManagedObject(typeof(float));
                        //// Tạo RotatedRect trong C#
                        //// Tạo rotated rectangle từ polygon
                        PyObject box = boxes[j];
                      OpenCvSharp . Point[] polygonPoints = ConvertBoxToPoints(box);
                       
                        //float x1 = (float)box[0].As<double>();
                        //float y1 = (float)box[1].As<double>();
                        //float x2 = (float)box[2].As<double>();
                        //float y2 = (float)box[3].As<double>();
                        //Point2f[] polygonPoints = new Point2f[]
                        //{
                        //    new Point2f(10, 10),
                        //    new Point2f(100, 20),
                        //    new Point2f(90, 80),
                        //    new Point2f(20, 90)
                        //};
                        RotatedRect rotatedRect = Cv2.MinAreaRect(polygonPoints);

                        // Lấy thông tin
                        // Point2f center = rotatedRect.Center;
                        //Size2f size = rotatedRect.Size;
                        //   float angle = rotatedRect.Angle;
                        //  if (angle == 180) angle = 0;
                        //RotatedRect rotatedRect = new RotatedRect(new Point2f(centerX, centerY), new Size2f(width, height), angle);

                        //// Lấy 4 đỉnh của hình chữ nhật xoay
                        //Point2f[] vertices = rotatedRect.Points();

                        //// Chuyển sang Point để vẽ
                        //OpenCvSharp.Point[] points = Array.ConvertAll(vertices, pt => new OpenCvSharp.Point((int)pt.X, (int)pt.Y));

                        //// Vẽ các cạnh hình chữ nhật xoay
                        //for (int k = 0; k < 4; k++)
                        //{
                        //    Cv2.Line(matCrop, points[k], points[(k + 1) % 4], Scalar.Red, 2);
                        //}
                        //    Cv2.ImWrite("cropRS.png", matCrop);
                        int width =(int) rotatedRect.Size.Width;
                        int height = (int)rotatedRect.Size.Height;
                        if (width < height)
                            {
                            int h = width, w = height;
                            width = w;
                            height = h;
                            rotatedRect.Angle = rotatedRect.Angle + 90;
                        }
                        if (rotatedRect.Angle > 145) rotatedRect.Angle =-(180- rotatedRect.Angle);
                      
                        RectangleF rect = new RectangleF(-width / 2, -height / 2, width, height);
                        RectRotate rt = new RectRotate(rect, new PointF(rotatedRect.Center.X, rotatedRect.Center.Y), rotatedRect.Angle, AnchorPoint.None,false);

                        //// Gán Rect quay góc 0 (vì YOLO box không có góc)
                        //RotatedRect rect = new RotatedRect(new Point2f(cx, cy), new Size2f(w, h), 0);
                        boxList.Add(rt);

                        // Score
                        float score = (float)scores[j].As<double>();
                        scoreList.Add(score * 100);

                        // Label
                        string label = labels[j].ToString();
                        label = label.Replace("\n", "");
                        Content += label.Trim();
                        listLabelResult.Add(label);
                        listLabel[listLabel.Count()-1].Add(label);
                        listOK.Add(false);
                        rectRotates.Add(rt);

                        scoreRS += (int)score;
                        listScore.Add(score);

                    }
                    List<RotatedBoxInfo> combined = new List<RotatedBoxInfo>();

                    for (int j = 0; j < rectRotates.Count; j++)
                    {
                        combined.Add(new RotatedBoxInfo
                        {
                            Box = rectRotates[j],
                            Label = listLabelResult[j],
                            Score = listScore[j]
                        });
                    }
                    combined = combined.OrderBy(b => b.Box._PosCenter.X).ToList();
                    rectRotates = combined.Select(b => b.Box).ToList();
                    listLabelResult = combined.Select(b => b.Label).ToList();
                    listScore = combined.Select(b => b.Score).ToList();
                    Content += "\n";
                    // result: tuple (boxes, scores, labels) từ Python
                    // e.Result = result;
                }
                catch (PythonException pyEx)
                {
                    exMess=pyEx.Message;
                }
                catch (Exception ex)
                {
                    exMess = ex.Message;
                }
            }

        }
        public void Complete()
        {
            try
            {


                IsOK = false;


                ScoreRs = (int)(scoreRS / (rectRotates.Count() * 1.0));
                //   listContent = Content.Select(c => c.ToString()).ToArray();
                //  listMatching = Matching.Select(c => c.ToString()).ToArray();
                Content = "";
                foreach (String label in listLabelResult)
                    Content += label;
                if (ScoreRs < 0) ScoreRs = 0;
                if (Content != "")
                {
                    if (Matching == "")
                        IsOK = true;
                    else
                        if (Matching == Content)
                    {

                        IsOK = true;
                    }
                  //  listContent = CompareStrings(listMatching, listContent);

                }
                else
                {
                    IsOK = false;
                }
              
                StatusTool = StatusTool.Done;
                // MessageBox.Show($"Predict xong: {boxes.len()} boxes");
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
            }
        }
      
            string[] CompareStrings(string[] original, string[] detected)
        {
            int maxLength = Math.Max(original.Length, detected.Length);
            string[] result = new string[maxLength];

            for (int i = 0; i < maxLength; i++)
            {
                if (i >= detected.Length)  // Nếu thiếu ký tự
                {
                    result[i] = "_";
                }
                else if (i >= original.Length)  // Nếu detect dư ký tự (ít xảy ra)
                {
                    result[i] = "_";
                }
                else if (original[i] != detected[i])  // Nếu sai ký tự
                {
                    result[i] = "_";
                }
                else
                {
                    result[i] = original[i];  // Nếu đúng
                }
            }

            return result;
        }

        public bool Check1(RectRotate rot)
        {
           
            //BeeCore.Native.SetImg(BeeCore.Common.matRaw);
            //BeeCore.G.CommonPlus.CropRotate((int)rot._PosCenter.X, (int)rot._PosCenter.Y, (int)rot._rect.Width, (int)rot._rect.Height, rot._angle);

            // listMatch = G.OCR.Find((float)(Score / 100.0));
            //listOK = new List<bool>();
            //listLabel = new List<List<string>>();
            //rectRotates = new List<RectRotate>();
            //listScore = new List<float>();
            //cycleTime = (int)G.OCR.Cycle;
            //IsOK = false;
            //if (listMatch != null)
            //{
            //    sSplit = listMatch.Split('\n');
            //    float Score = 0;
            //    int count = 0;
            //    int numOK = 0, numNG = 0;
            //    count= sSplit.Length;
            //    Content = "";
            //    foreach (String s in sSplit)
            //    {
            //        if (s.Trim() == "") break;
            //        String[] sSp = s.Split(',');
            //        PointF pCenter = new PointF(Convert.ToSingle(sSp[0]), Convert.ToSingle(sSp[1]));

            //        float width = Convert.ToSingle(sSp[2]);
            //        float height = Convert.ToSingle(sSp[3]);
            //        float angle = Convert.ToSingle(sSp[4]);
            //        float score = Convert.ToSingle(sSp[5])*100;
            //        string label = Convert.ToString(sSp[6]);
            //        label = label.Replace("\n", "");
            //        Content += label.Trim();
            //        int Area = (int)(width * height);
            //        double Per = (Math.Min(width, height) * 1.0) / Math.Max(width, height);
                 
            //        RectRotate rt = new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None,false);

            //        listOK.Add(false);
            //        rectRotates.Add(rt);
            //      //  listLabel.Add(label);
            //        Score += score;
            //        listScore.Add(score);
            //        numOK++;
                   
            //    }
            //    ScoreRs = (int)(Score / (rectRotates.Count() * 1.0));
            //    listContent= Content.Select(c => c.ToString()).ToArray();
            //   listMatching= Matching.Select(c => c.ToString()).ToArray();
            //    if (ScoreRs < 0) ScoreRs = 0;
            //    if (Content != "")
            //    {
            //            if (Matching == "")
            //                IsOK = true;
            //            else
            //                if (Matching == Content)
            //            {
                         
            //                IsOK = true;
            //            }
            //        listContent = CompareStrings(listMatching, listContent);

            //    }
            //    else
            //    {
            //        IsOK = false;
            //    }
            //    //switch(Compare)
            //    //  {
            //    //      case Compares.Equal:
            //    //          if (numOK != NumObject)
            //    //              IsOK = false;
            //    //          break;
            //    //      case Compares.Less:
            //    //          if (numOK >= NumObject)
            //    //              IsOK = false;
            //    //          break;
            //    //      case Compares.More:
            //    //          if (numOK <= NumObject)
            //    //              IsOK = false;
            //    //          break;
            //    //  }

            //}
            //G.IsChecked = true;
            return true;
        }
        public static bool SetModel()
        {
            using (Py.GIL())
            {

                
                      // khởi tạo instance

                G.objOCR.initialize_ocr();
            }
            return true;
        }

    }
}
