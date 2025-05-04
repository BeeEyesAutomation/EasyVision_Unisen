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



namespace BeeCore
{
    [Serializable()]
    public class Yolo
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

      
        public  void SetModel(String nameTool, String nameModel, TypeYolo TypeYolo)
        {
            G. YoloPlus.LoadModel(nameTool,nameModel, (int)TypeYolo);

        }
        public int Index = -1;
        public String PathModel = "";
        public TypeYolo TypeYolo = TypeYolo.YOLO;
        public TypeTool TypeTool=TypeTool.Yolo;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp, matTemp2, matMask;
        public List<String> Labels = new List<string>();
        private Mode _TypeMode = Mode.Pattern;
        public Compares Compare = Compares.Equal;
       
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
        public List<bool> listOK = new List<bool>();
        public List<string> listLabel = new List<string>();
        String listMatch;
        public bool IsCheckArea = false;
        public Point p1 = new Point();
        public Point p2 = new Point();
        public int yLine = 200;
        public bool Check(String nameTool, RectRotate rot)
        {
            yLine = 710;
            BeeCore.Native.SetImg(BeeCore.Common.matRaw);
            BeeCore.G.CommonPlus.CropRotate((int)rot._PosCenter.X, (int)rot._PosCenter.Y, (int)rot._rect.Width, (int)rot._rect.Height, rot._angle);
            //{
               
            //    G.IsChecked = false; return false;
            //}
            //if (!BeeCore.Common.matRaw.Empty())
            //    BeeCore.Common.matRaw.Release();
        //    BeeCore.Common.matRaw = BeeCore.Native.GetImg().Clone();
             listMatch = G.YoloPlus.Check( nameTool, (float)(Score / 100.0));
            listOK = new List<bool>();
            listLabel = new List<string>();
            rectRotates = new List<RectRotate>();
            listScore = new List<float>();
            cycleTime = (int)G.YoloPlus.Cycle;
            IsOK = true;
            if (listMatch != null)
            {
                sSplit = listMatch.Split('\n');
                float Score = 0;
                int count = 0;
                int numOK = 0, numNG = 0;
                count= sSplit.Length;
                foreach (String s in sSplit)
                {
                    if (s.Trim() == "") break;
                    String[] sSp = s.Split(',');
                    PointF pCenter = new PointF(Convert.ToSingle(sSp[0]), Convert.ToSingle(sSp[1]));

                    float width = Convert.ToSingle(sSp[2]);
                    float height = Convert.ToSingle(sSp[3]);
                    float angle = Convert.ToSingle(sSp[4]);
                    float score = Convert.ToSingle(sSp[5])*100;
                    string label = Convert.ToString(sSp[6]);
                  
                    int Area = (int)(width * height);
                    double Per = (Math.Min(width, height) * 1.0) / Math.Max(width, height);
                 
                    RectRotate rt = new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None);
                   
                    if (Labels.Contains(label))
                    {
                        if (IsCheckArea )
                        {
                            if(rt._PosCenter.Y-height/2<yLine)
                            {
                                listOK.Add(false);
                                rectRotates.Add(rt);
                                listLabel.Add(label);
                                Score += score;
                                listScore.Add(score);
                                numOK++;
                            }
                            else
                            {
                                listOK.Add(true);
                                rectRotates.Add(rt);
                                listLabel.Add(label);
                                Score += score;
                                listScore.Add(score);

                            }
                        }
                        else
                        {
                            rectRotates.Add(rt);
                            listLabel.Add(label);
                            Score += score;
                            listScore.Add(score); numOK++;
                        }
                           
                       
                    }

                }
                ScoreRs = (int)(Score / (rectRotates.Count() * 1.0));
                if (ScoreRs < 0) ScoreRs = 0;
              switch(Compare)
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
              
            }
            G.IsChecked = true;
            return true;
        }

    }
}
