using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static OpenCvSharp.ML.DTrees;
using Point = System.Drawing.Point;
using Python.Runtime;
using static LibUsbDotNet.Main.UsbTransferQueue;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.UI.WebControls;
using BeeCore.Parameter;
using BeeCore.Funtion;
using System.Windows.Forms;

namespace BeeCore
{
    [Serializable()]
    public class Yolo
    {
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
            try
            { 
            using (Py.GIL())
            {


                dynamic mod = Py.Import("Tool.Learning");
                dynamic cls = mod.GetAttr("ObjectDetector"); // class
                dynamic obj = cls.Invoke();              // khởi tạo instance

                G.objYolo.load_model(nameTool, pathFullModel, (int)TypeYolo.YOLO);
                StatusTool = StatusTool.Initialed;
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
            // G.YoloPlus.LoadModel(nameTool, nameModel, (int)TypeYolo);
        }
        public int Percent = 0;
        
        public void Training(String nameTool,String pathYaml)
        {
            using (Py.GIL())
            {
              
                Action<int> onProgress = percent =>
                {
                    Percent = percent;
                    Console.WriteLine($"Training progress: {percent}%");
                };
                using (PyObject pyCallback = onProgress.ToPython())
                {
                    var result = G.objYolo.train(nameTool, pathYaml, Epoch, callback: pyCallback);
                    Console.WriteLine(result.ToString());
                }
            }

          
        }
      
        public String[] LoadNameModel(String nameTool)
        {
            using (Py.GIL())
            {



                dynamic result = G.objYolo.loadNames(nameTool );

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
        }
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
        public int Epoch = 100;
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
        public bool IsOK = false;
        public bool IsAreaWhite = false;
        public int ScoreRs = 0, cycleTime;
        private float _score = 70;
        public bool IsIni = false;
        public float Score
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
        public List<bool> listOK = new List<bool>();
        public List<string> listLabel = new List<string>();
        public List<string> listModels = new List<string>();
        String listMatch;
        public bool IsCheckArea = false;
        public Point p1 = new Point();
        public Point p2 = new Point();
        public int yLine = 100;
        public String nameTool = "";
        public String Content = "";
        public String Matching = "";
        public bool IsEnContent = false;
        public StatusTool StatusTool =StatusTool.None;
        List<RectRotate> boxList = new List<RectRotate>();
        List<float> scoreList = new List<float>();
        List<string> labelList = new List<string>();
        public int IndexThread = 0;
        public float CropOffSetX, CropOffSetY=0;
        public void DoWork(RectRotate rotCrop)
        {
            using (Py.GIL())
            {
                try
                {
                    CropOffSetX = rotCrop._PosCenter.X + rotCrop._rect.X;
                    CropOffSetY = rotCrop._PosCenter.Y + rotCrop._rect.Y;
                    if (CropOffSetX > 0) CropOffSetX = 0; else CropOffSetX = -CropOffSetX;
                    if (CropOffSetY > 0) CropOffSetY = 0; else CropOffSetY = -CropOffSetY;
                    if (yLine==0)
                    yLine = 300;
                    boxList = new List<RectRotate>();
                     scoreList = new List<float>();
                     labelList = new List<string>();
                    Mat matCrop = Common.CropRotatedRectSharp(BeeCore.Common.listCamera[IndexThread].matRaw, new RotatedRect(new Point2f(rotCrop._PosCenter.X, rotCrop._PosCenter.Y), new Size2f(rotCrop._rect.Size.Width, rotCrop._rect.Size.Height), rotCrop._angle));
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
                    int channels = matCrop.Channels();
                    //dynamic np = Py.Import("numpy");
                    ////    G.objYolo = Py.Import("Tool.Learning").ObjectDetector(); // khởi tạo trực tiếp
                    //dynamic mod = Py.Import("Tool.Learning");
                    //dynamic cls = mod.GetAttr("ObjectDetector"); // class
                    //dynamic objYolo = cls.Invoke();              // khởi tạo instance
                    //G.objYolo.load_model(nameTool, nameModel, (int)TypeYolo);
                    var npArray = G.np.array(buffer).reshape(height1, width1, 3);
                    dynamic result = G.objYolo.predict(npArray, (float)(Score / 100.0), nameTool);
                    PyObject boxes = result[0];
                    PyObject scores = result[1];
                    PyObject labels = result[2];

                    int counts = (int)boxes.Length();
                    for (int j = 0; j < counts; j++)
                    {
                        // Lấy box: (x1, y1, x2, y2)
                        PyObject box = boxes[j];
                        float x1 = (float)box[0].As<double>();
                        float y1 = (float)box[1].As<double>();
                        float x2 = (float)box[2].As<double>();
                        float y2 = (float)box[3].As<double>();

                        // Tính width, height và center
                        float w = x2 - x1;
                        float h = y2 - y1;
                        float cx = x1 + w / 2;
                        float cy = y1 + h / 2;
                        RectRotate rt = new RectRotate(new RectangleF(-w / 2, -h / 2, w, h), new PointF(cx, cy), 0, AnchorPoint.None,false);

                        //// Gán Rect quay góc 0 (vì YOLO box không có góc)
                        //RotatedRect rect = new RotatedRect(new Point2f(cx, cy), new Size2f(w, h), 0);
                        boxList.Add(rt);

                        // Score
                        float score = (float)scores[j].As<double>();
                        scoreList.Add(score * 100);

                        // Label
                        string label = labels[j].ToString();
                        labelList.Add(label);
                    }


                    // result: tuple (boxes, scores, labels) từ Python
                    // e.Result = result;
                }
                catch (PythonException pyEx)
                {
                       MessageBox.Show("Python Error: " + pyEx.Message);
                }
                catch (Exception ex)
                {
                      MessageBox.Show("Error: " + ex.Message);
                }
            }

        }
        public ArrangeBox ArrangeBox=new ArrangeBox();
        public bool IsArrangeBox = false;
        public void Complete()
        {
            try
            {


                try
                {
                    listOK = new List<bool>();
                    listLabel = new List<string>();
                    rectRotates = new List<RectRotate>();
                    listScore = new List<float>();
                   // cycleTime = (int)G.YoloPlus.Cycle;
                    IsOK = true;
                    int i = 0;
                    int numOK = 0, numNG = 0;
                    int scoreRS = 0;
                    List<String> _listLabelCompare = new List<String>();
                    if (listLabelCompare == null)
                    {
                        IsOK = false;
                        return;
                    }
                    Content = "";
                    foreach (Labels label in listLabelCompare)
                    {
                        if (label == null) continue;
                        if(!label.IsEn) continue;
                        _listLabelCompare.Add(label.label);
                    }
                    foreach (String label in labelList)
                    {
                        if (_listLabelCompare.Contains(label))
                        {
                            if (IsCheckArea)
                            {
                               switch(CompareLine)
                                {
                                    case Compares.More:
                                        if (boxList[i]._PosCenter.Y - boxList[i]._rect.Size.Height / 2 < yLine)
                                        {
                                            listOK.Add(false);
                                            rectRotates.Add(boxList[i]);
                                            listLabel.Add(label);
                                            scoreRS += (int)scoreList[i];
                                            listScore.Add(scoreList[i]);
                                            numOK++;
                                        }
                                        else
                                        {
                                            listOK.Add(true);
                                            rectRotates.Add(boxList[i]);
                                            listLabel.Add(label);
                                            scoreRS += (int)scoreList[i];
                                            listScore.Add(scoreList[i]);

                                        }
                                        break;
                                    case Compares.Less:
                                        if (boxList[i]._PosCenter.Y - boxList[i]._rect.Size.Height / 2 >= yLine)
                                        {
                                            listOK.Add(false);
                                            rectRotates.Add(boxList[i]);
                                            listLabel.Add(label);
                                            scoreRS += (int)scoreList[i];
                                            listScore.Add(scoreList[i]);
                                            numOK++;
                                        }
                                        else
                                        {
                                            listOK.Add(true);
                                            rectRotates.Add(boxList[i]);
                                            listLabel.Add(label);
                                            scoreRS += (int)scoreList[i];
                                            listScore.Add(scoreList[i]);

                                        }
                                        break;
                                }
                                   
                                
                            }
                            else
                            {
                                Content += label;
                                rectRotates.Add(boxList[i]);
                                listLabel.Add(label);
                                scoreRS += (int)scoreList[i];
                                listScore.Add(scoreList[i]); numOK++;
                            }


                        }
                        i++;
                    }
                    if (IsArrangeBox)
                    {
                        List<RotatedBoxInfo> combined = new List<RotatedBoxInfo>();

                        for (int j = 0; j < rectRotates.Count; j++)
                        {
                            combined.Add(new RotatedBoxInfo
                            {
                                Box = rectRotates[j],
                                Label = listLabel[j],
                                Score = listScore[j]
                            });
                        }
                        switch(ArrangeBox)
                        {
                            case ArrangeBox.X_Left_Rigth:
                                // Sort theo X tăng dần (trái → phải)
                                combined = combined.OrderBy(b => b.Box._PosCenter.X).ToList();
                                break;
                            case ArrangeBox.X_Right_Left:
                                // Sort theo X giảm dần (phải → trái)
                                 combined = combined.OrderByDescending(b => b.Box._PosCenter.X).ToList();

                                break;
                            case ArrangeBox.Y_Left_Rigth:
                                // Sort theo Y tăng dần (trên → dưới)
                                 combined = combined.OrderBy(b => b.Box._PosCenter.Y).ToList();
                                break;
                            case ArrangeBox.Y_Right_Left:
                                combined = combined.OrderByDescending(b => b.Box._PosCenter.Y).ToList();
                                break;
                        }
                        rectRotates = combined.Select(b => b.Box).ToList();
                        listLabel = combined.Select(b => b.Label).ToList();
                        listScore = combined.Select(b => b.Score).ToList();
                        Content = "";
                        foreach (string s in listLabel)
                            Content += s;
                    }
                    ScoreRs = (int)(scoreRS / (rectRotates.Count() * 1.0));
                    if (ScoreRs < 0) ScoreRs = 0;
                    IsOK = true;
                    switch (Compare)
                    {
                        case Compares.Equal:
                            if (numOK != NumObject)
                                IsOK = false;
                            break;
                        case Compares.Less:
                            if (numOK >= NumObject)
                                IsOK = false;
                            break;
                        case Compares.More:
                            if (numOK <= NumObject)
                                IsOK = false;
                            break;
                    }
                    if(IsEnContent)
                    {
                        if(Matching!=Content)
                        {
                            IsOK = false;
                        }
                    }
                    StatusTool = StatusTool.Done;
                    G.IsChecked = true;
                    // MessageBox.Show($"Predict xong: {boxes.len()} boxes");
                }
                catch (Exception ex)
                {
                    // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
            }
        }
   
        public bool Check()
        {
              
               
            //{

            //    G.IsChecked = false; return false;
            //}
            //if (!BeeCore.Common.matRaw.Empty())
            //    BeeCore.Common.matRaw.Release();
            //    BeeCore.Common.matRaw = BeeCore.Native.GetImg().Clone();
        
            //worker.RunWorkerAsync();
        
      

           
                // result là tuple (boxes, scores, labels)
              
            
            //    BeeCore.Native.SetImg(BeeCore.Common.matRaw);
            //  BeeCore.Common.CropRotate(rot);
            //   BeeCore.G.CommonPlus.CropRotate((int)rot._PosCenter.X, (int)rot._PosCenter.Y, (int)rot._rect.Width, (int)rot._rect.Height, rot._angle);


            //  listMatch = G.YoloPlus.Check( nameTool, (float)(Score / 100.0));
            //if (listMatch != null)
            //{
            //    sSplit = listMatch.Split('\n');
            //    float Score = 0;
            //    int count = 0;
            //    int numOK = 0, numNG = 0;
            //    count= sSplit.Length;
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
                  
            //        int Area = (int)(width * height);
            //        double Per = (Math.Min(width, height) * 1.0) / Math.Max(width, height);
                 
            //        RectRotate rt = new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None);
                   
            //        if (Labels.Contains(label))
            //        {
            //            if (IsCheckArea )
            //            {if (height < 300)
            //                {
            //                    if (rt._PosCenter.Y - height / 2 < yLine)
            //                    {
            //                        listOK.Add(false);
            //                        rectRotates.Add(rt);
            //                        listLabel.Add(label);
            //                        Score += score;
            //                        listScore.Add(score);
            //                        numOK++;
            //                    }
            //                    else
            //                    {
            //                        listOK.Add(true);
            //                        rectRotates.Add(rt);
            //                        listLabel.Add(label);
            //                        Score += score;
            //                        listScore.Add(score);

            //                    }
            //                }
            //            }
            //            else
            //            {
            //                rectRotates.Add(rt);
            //                listLabel.Add(label);
            //                Score += score;
            //                listScore.Add(score); numOK++;
            //            }
                           
                       
            //        }

            //    }
            //    ScoreRs = (int)(Score / (rectRotates.Count() * 1.0));
            //    if (ScoreRs < 0) ScoreRs = 0;
            //  switch(Compare)
            //    {
            //        case Compares.Equal:
            //            if (numOK != NumObject)
            //                IsOK = false;
            //            break;
            //        case Compares.Less:
            //            if (numOK >= NumObject)
            //                IsOK = false;
            //            break;
            //        case Compares.More: 
            //            if (numOK <= NumObject)
            //                IsOK = false;
            //            break;
            //    }
              
            //}
          
            return true;
        }

    }
}
