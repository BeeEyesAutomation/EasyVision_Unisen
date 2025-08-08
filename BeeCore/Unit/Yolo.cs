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
using BeeCore.Funtion;
using System.Windows.Forms;
using BeeGlobal;
using System.Drawing.Drawing2D;

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
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;

            try
            { 
            using (Py.GIL())
            {

                    if(Common.PropetyTools[IndexThread][Index].Name.Trim()=="")
                    {
                        Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                    }
                    //dynamic mod = Py.Import("Tool.Learning");
                    //dynamic cls = mod.GetAttr("ObjectDetector"); // class
                    //dynamic obj = cls.Invoke();              // khởi tạo instance

                    if (Common.PropetyTools[IndexThread][Index].Name.Trim() == "")
                    {
                        Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                    }
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
       
        public void Training(string nameTool, string modelPath, string pathYaml)
        {
            using (Py.GIL())
            {
                Action<int> onProgress = percent =>
                {
                    Common.PropetyTools[Global.IndexChoose][Index].Percent = percent;
                    
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
       
        public String Content = "";
        public String Matching = "";
        public bool IsEnContent = false;
      
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
                    dynamic result = G.objYolo.predict(npArray, (float)(Common.PropetyTools[IndexThread][Index]. Score / 100.0), Common.PropetyTools[IndexThread][Index].Name);
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
                    Common.PropetyTools[IndexThread][Index].Results = Results.OK;
                    int i = 0;
                    int numOK = 0, numNG = 0;
                    int scoreRS = 0;
                    List<String> _listLabelCompare = new List<String>();
                    if (listLabelCompare == null)
                    {
                        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
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
                    if(IsEnContent)
                    {
                        if(Matching!=Content)
                        {
                            Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                        }
                    }
                    
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
            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;
            switch (Common.PropetyTools[Global.IndexChoose][Index].Results)
            {
                case Results.OK:
                    cl = Global.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ColorNG;
                    break;
            }
            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            if (!Global.IsHideTool)
                Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl, 2);
            int i = 0;
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

                mat.Translate(CropOffSetX, CropOffSetY);
                gc.Transform = mat;

                if (IsCheckArea)
                {
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                    gc.DrawLine(new Pen(Color.Gold, 6), new Point(0, yLine), new Point((int)rotA._rect.Width, yLine));

                    System.Drawing.Point point1 = new System.Drawing.Point((int)(rot._PosCenter.X), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                    System.Drawing.Point point2 = new System.Drawing.Point((int)(rot._PosCenter.X), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                    System.Drawing.Point point3 = new System.Drawing.Point((int)(rot._PosCenter.X - rot._rect.Width / 2), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                    System.Drawing.Point point4 = new System.Drawing.Point((int)(rot._PosCenter.X + rot._rect.Width / 2), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                    System.Drawing.Point point5 = new System.Drawing.Point((int)(rot._PosCenter.X - rot._rect.Width / 2), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                    System.Drawing.Point point6 = new System.Drawing.Point((int)(rot._PosCenter.X + rot._rect.Width / 2), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                    Color clLine = Color.Red;
                    if (listOK[i])
                        clLine = Color.Green;
                    gc.DrawLine(new Pen(clLine, 8), point1, point2);
                    gc.DrawLine(new Pen(clLine, 8), point3, point4);
                    gc.DrawLine(new Pen(clLine, 8), point5, point6);
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);

                    gc.Transform = mat;
                    int index = i + 1;
                    String content = "(" + listLabel[i] + ") \n" + Math.Round(listScore[i], 1) + "%";
                    if (IsCheckArea)
                        content = rot._rect.Height + " px";
                    Font font = new Font("Arial", 30, FontStyle.Bold);
                    SizeF sz1 = gc.MeasureString(content, font);
                    gc.DrawString(content, font, new SolidBrush(clLine), new System.Drawing.Point((int)(rot._rect.X + rot._rect.Width / 2), (int)(rot._rect.Y + rot._rect.Height / 2 - sz1.Height / 2)));
                    i++;
                    //gc.FillEllipse(Brushes.Black, -3, -3, 6, 6);
                    gc.ResetTransform();
                }
                else
                {
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    gc.Transform = mat;
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;

                    int index = i + 1;
                    //String content = "(" + listLabel[i] + ") \n" + Math.Round(listScore[i], 1) + "%";
                    //if (IsCheckArea)
                    //    content = rot._rect.Height + " px";
                    //  Font font = new Font("Arial", 30, FontStyle.Bold);
                    //  SizeF sz2 = gc.MeasureString(content, font);
                    if (IsEnContent)
                        Draws.Box2Label(gc, rot._rect, listLabel[i], "", Global.fontRS, cl, brushText, 30, 3);
                    else
                        Draws.Box2Label(gc, rot._rect, listLabel[i], Math.Round(listScore[i], 1) + "%", Global.fontRS, cl, brushText, 30, 3);

                    //  Draws.Box1Label(gc, rot._rect, Math.Round(listScore[i], 1) + "%", Global.fontRS, brushText, Brushes.Transparent, true);
                    //  gc.DrawString(content, font, new SolidBrush(cl), new System.Drawing.Point((int)(rot._rect.X + rot._rect.Width / 2 - sz2.Width / 2), (int)(rot._rect.Y + rot._rect.Height / 2 - sz2.Height / 2)));
                    i++;
                    //gc.FillEllipse(Brushes.Black, -3, -3, 6, 6);
                    gc.ResetTransform();
                }

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
