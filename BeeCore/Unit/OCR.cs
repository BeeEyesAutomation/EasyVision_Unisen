using BeeCore.Funtion;

using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        public TypeTool TypeTool=TypeTool.Learning;
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
        public int Enhance = 4;
        public int Clahe = 2;
        public int Sigma = 2;
        public int Blur = 1;
        public bool IsEnLimitArea = false;
        public int LimitArea = 100;
        public Compares CompareArea = Compares.More;
      
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
        public  Mat EnhanceImage(Mat input, double contrastFactor = 4.0, double sharpenFactor = 4.0)
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
            if (Common.IsDebug)
                Cv2.ImWrite(nameTool+".png", sharpened);
            return sharpened;
        }
       
        public  Mat PreprocessForOCR(Mat input,int clipLimit=2 ,int sigma=3,int blur=3)
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
            if (blur % 2 == 0) blur += 1;  // Chuyển thành số lẻ nếu chẵn
            if (blur < 3) blur = 3;        // Tối thiểu là 3
            Cv2.MedianBlur(sharp, clean, blur);
            if(Common.IsDebug)
            Cv2.ImWrite(nameTool +".png", clean);
            return clean;
        }
        public  String sAllow = "";
        public int IndexThread = 0;
        public void DoWork(RectRotate rotCrop)
        {
            using (Py.GIL())
            {
               
                try
                {
                    var boxList = new List<RectRotate>();
                
                    var labelList = new List<string>();
                    IsOK = false;
                    listOK = new List<bool>();
                    listLabel = new List<List<string>>();
                    rectRotates = new List<RectRotate>();
                    listScore = new List<float>();
                  
                    Content = "";
                    ScoreRs = 0;
                    listLabelResult = new List<String>();
                    using (Mat raw =BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
                    {
                        if (raw.Empty()) return;
                        Mat matCrop = new Mat();
                        matCrop = Common.CropRotatedRectSharp(raw, new RotatedRect(new Point2f(rotCrop._PosCenter.X, rotCrop._PosCenter.Y), new Size2f(rotCrop._rect.Size.Width, rotCrop._rect.Size.Height), rotCrop._angle));
                        if (Clahe == 0) Clahe = 2;
                        if (Sigma == 0) Sigma = 3;
                        if (Blur == 0) Blur = 3;

                        //   matCrop = PreprocessForOCR(matCrop,Clahe,Sigma,Blur);
                        if (matCrop.Type() != MatType.CV_8UC3)
                            Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2RGB);
                        if (matCrop.Channels() == 1)
                        {
                            Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2RGB);
                        }
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


                        var npArray = G.np.array(buffer).reshape(height1, width1, 3);
                        int limit = LimitArea * 100;
                        //if (!IsEnLimitArea)
                        limit = 0;
                        dynamic result = G.objOCR.find_ocr(npArray, nameTool, limit);//, (float)(Score / 100.0), nameTool

                        if (result == null) return;
                        // File.WriteAllText("ErC.txt", pyEx.Message);
                        PyObject boxes = result[0];
                        PyObject scores = result[1];
                        PyObject labels = result[2];
                        if (boxes == null || scores == null || labels == null)
                            return;
                        if ((int)boxes.Length() == 0 || (int)scores.Length() == 0 || (int)labels.Length() == 0)
                            return;
                        if ((int)boxes.Length() != (int)labels.Length())
                            return;

                        int i = 0;

                        for (int j = 0; j < boxes.Length(); j++)
                        {
                            listLabel.Add(new List<string>());

                            PyObject box = boxes[j];
                            OpenCvSharp.Point[] polygonPoints = ConvertBoxToPoints(box);


                            RotatedRect rotatedRect = Cv2.MinAreaRect(polygonPoints);


                            int width = (int)rotatedRect.Size.Width;
                            int height = (int)rotatedRect.Size.Height;
                            if (width < height)
                            {
                                int h = width, w = height;
                                width = w;
                                height = h;
                                rotatedRect.Angle = rotatedRect.Angle + 90;
                            }
                            if (rotatedRect.Angle > 145) rotatedRect.Angle = -(180 - rotatedRect.Angle);
                            if (IsEnLimitArea)
                            {
                                switch (CompareArea)
                                {
                                    case Compares.Less:
                                        if (width * height > LimitArea)
                                        {
                                            i++;
                                            continue;
                                        }
                                        break;
                                    case Compares.More:
                                        if (width * height < LimitArea)
                                        {
                                            i++;
                                            continue;
                                        }
                                        break;

                                }


                            }

                            RectangleF rect = new RectangleF(-width / 2, -height / 2, width, height);
                            RectRotate rt = new RectRotate(rect, new PointF(rotatedRect.Center.X, rotatedRect.Center.Y), rotatedRect.Angle, AnchorPoint.None, false);
                            boxList.Add(rt);

                            // Score
                            float score = (float)scores[j].As<double>();


                            // Label
                            string label = labels[j].ToString();
                          
                            List<char> allowed = new List<char>(sAllow.ToCharArray());

                            label = new string(label.Where(c => allowed.Contains(c)).ToArray());
              
                            Content += label.Trim();
                            listLabelResult.Add(label);
                            listLabel[listLabel.Count() - 1].Add(label);
                            listOK.Add(false);
                            rectRotates.Add(rt);
                            listScore.Add(score * 100);
                            ScoreRs += (int)(score * 100);


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
                        matCrop.Dispose();
                    }
                    // result: tuple (boxes, scores, labels) từ Python
                    // e.Result = result;
                }
                catch (PythonException pyEx)
                {
                    File.WriteAllText("PythonException.txt", pyEx.Message);
                    exMess=pyEx.Message;
                }
                catch (Exception ex)
                {
                    File.WriteAllText("ErCharp.txt", ex.Message);
                    exMess = ex.Message;
                }
                finally
                {
                  
                }
            }

        }
        public void Complete()
        {
            try
            {


                IsOK = true;


                ScoreRs = (int)(ScoreRs / (rectRotates.Count() * 1.0));
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
                    else
                        IsOK = false;
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

   

        public  bool SetModel()
        {
            using (Py.GIL())
            {
                // khởi tạo instance
                StatusTool = StatusTool.None;
         G.objOCR.initialize_ocr(nameTool);
                StatusTool = StatusTool.Initialed;
            }
            return true;
        }

    }
}
