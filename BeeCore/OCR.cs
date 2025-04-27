using BeeCore.Funtion;
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
        public List<bool> listOK = new List<bool>();
        public List<string> listLabel = new List<string>();
        String listMatch;
        public bool IsCheckArea = false;
        public Point p1 = new Point();
        public Point p2 = new Point();
        public int yLine = 200;
        public String Matching = "";
        public String Content = "";
        public String[] listContent ;
        public String[] listMatching;
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

        public bool Check(RectRotate rot)
        {
            yLine = 710;
            BeeCore.Native.SetImg(BeeCore.Common.matRaw);
            BeeCore.G.CommonPlus.CropRotate((int)rot._PosCenter.X, (int)rot._PosCenter.Y, (int)rot._rect.Width, (int)rot._rect.Height, rot._angle);

             listMatch = G.OCR.Find((float)(Score / 100.0));
            listOK = new List<bool>();
            listLabel = new List<string>();
            rectRotates = new List<RectRotate>();
            listScore = new List<float>();
            cycleTime = (int)G.OCR.Cycle;
            IsOK = false;
            if (listMatch != null)
            {
                sSplit = listMatch.Split('\n');
                float Score = 0;
                int count = 0;
                int numOK = 0, numNG = 0;
                count= sSplit.Length;
                Content = "";
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
                    label = label.Replace("\n", "");
                    Content += label.Trim();
                    int Area = (int)(width * height);
                    double Per = (Math.Min(width, height) * 1.0) / Math.Max(width, height);
                 
                    RectRotate rt = new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None);

                    listOK.Add(false);
                    rectRotates.Add(rt);
                    listLabel.Add(label);
                    Score += score;
                    listScore.Add(score);
                    numOK++;
                    //if (Labels.Contains(label))
                    //{
                    //    if (IsCheckArea )
                    //    {
                    //        if(rt._PosCenter.Y-height/2<yLine)
                    //        {
                    //            listOK.Add(false);
                    //            rectRotates.Add(rt);
                    //            listLabel.Add(label);
                    //            Score += score;
                    //            listScore.Add(score);
                    //            numOK++;
                    //        }
                    //        else
                    //        {
                    //            listOK.Add(true);
                    //            rectRotates.Add(rt);
                    //            listLabel.Add(label);
                    //            Score += score;
                    //            listScore.Add(score);

                    //        }
                    //    }
                    //    else
                    //    {
                    //        rectRotates.Add(rt);
                    //        listLabel.Add(label);
                    //        Score += score;
                    //        listScore.Add(score); numOK++;
                    //    }
                           
                       
                    //}
                  
                   

                    //if (rectF.Contains(new PointF(rt._PosCenter.X, rt._PosCenter.Y))
                    //       )
                    //    rectRotates.Add(rt);
                    //else
                    //{
                    //    String sss = "";
                    //}
                }
                ScoreRs = (int)(Score / (rectRotates.Count() * 1.0));
                listContent= Content.Select(c => c.ToString()).ToArray();
               listMatching= Matching.Select(c => c.ToString()).ToArray();
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
                    listContent = CompareStrings(listMatching, listContent);

                }
                else
                {
                    IsOK = false;
                }
                //switch(Compare)
                //  {
                //      case Compares.Equal:
                //          if (numOK != NumObject)
                //              IsOK = false;
                //          break;
                //      case Compares.Less:
                //          if (numOK >= NumObject)
                //              IsOK = false;
                //          break;
                //      case Compares.More:
                //          if (numOK <= NumObject)
                //              IsOK = false;
                //          break;
                //  }

            }
            G.IsChecked = true;
            return true;
        }
        public static bool SetModel()
        {
           return G.OCR.SetModel();
        }

    }
}
