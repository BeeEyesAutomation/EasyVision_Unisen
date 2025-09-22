
using BeeCore.Func;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Point = System.Drawing.Point;

namespace BeeCore
{
    [Serializable()]
    public class Measure
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public void SetModel()
        {
            rotMask = null;
            rotCrop = null;
            rotArea = null;
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 45;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
            if (listRot == null)
            {
                listRot = new List<RectRotate> { new RectRotate(), new RectRotate(), new RectRotate(), new RectRotate() };
            }
            if (listPointChoose == null)
            {
                listPointChoose = new List<Tuple<String, int>>();
                listPointChoose.Add(new Tuple<String, int>(null, -1));
                listPointChoose.Add(new Tuple<String, int>(null, -1));
                listPointChoose.Add(new Tuple<String, int>(null, -1));
                listPointChoose.Add(new Tuple<String, int>(null, -1));
            }
            if (listPointChoose.Count() == 0)
            {
                listPointChoose = new List<Tuple<String, int>>();
                listPointChoose.Add(new Tuple<String, int>(null, -1));
                listPointChoose.Add(new Tuple<String, int>(null, -1));
                listPointChoose.Add(new Tuple<String, int>(null, -1));
                listPointChoose.Add(new Tuple<String, int>(null, -1));
            }
            listLine1Point = new List<Point>();
            listLine2Point = new List<Point>();
            listLine1Point.Add(new Point(0, 0));
            listLine1Point.Add(new Point(0, 0));
            listLine2Point.Add(new Point(0, 0));
            listLine2Point.Add(new Point(0, 0));
        }
        public float Scale = 1;
        public TypeMeasure TypeMeasure = TypeMeasure.Angle;
        public DirectMeasure DirectMeasure = DirectMeasure.X;
        public MethordMeasure MethordMeasure = MethordMeasure.Min;
      
        public Measure() { }
        public bool IsCheckArea = false;
        public String nameTool = "";
        public int Index = 0;
        public double AngleDetect = 0;
        public int IndexThread = 0;

        
       
        public List<RectRotate> listRot = new List<RectRotate> { new RectRotate(), new RectRotate(), new RectRotate(), new RectRotate() };
        public List<Point> listLine1Point = new List<Point>();
        public List<Point> listLine2Point = new List<Point>();
        public List<Tuple<String, int>> listPointChoose = new List<Tuple<String, int>>();
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
    
     
      [NonSerialized]
      private bool IsDone1=false,  IsDone2 = false,  IsDone3 = false,  IsDone4 = false;
        public void DoWork(RectRotate rectRotate)
        {
        X: Common.PropetyTools[Global.IndexChoose][Index].Results = Results.OK;
          
          PropetyTool PropetyTool1 = BeeCore.Common.PropetyTools[IndexThread][BeeCore.Common.PropetyTools[IndexThread].FindIndex(a => a.Name == listPointChoose[0].Item1)];
            PropetyTool PropetyTool2 = BeeCore.Common.PropetyTools[IndexThread][BeeCore.Common.PropetyTools[IndexThread].FindIndex(a => a.Name == listPointChoose[1].Item1)];
            PropetyTool PropetyTool3 = null, PropetyTool4 = null;
            if (TypeMeasure == TypeMeasure.Angle)
            {
                PropetyTool3 = BeeCore.Common.PropetyTools[IndexThread][BeeCore.Common.PropetyTools[IndexThread].FindIndex(a => a.Name == listPointChoose[2].Item1)];
                PropetyTool4 = BeeCore.Common.PropetyTools[IndexThread][BeeCore.Common.PropetyTools[IndexThread].FindIndex(a => a.Name == listPointChoose[3].Item1)];


            }
            if (PropetyTool1 != null)
                if (!IsDone1)
                if (PropetyTool1.StatusTool == StatusTool.Done || !Global.IsRun)
                {
                IsDone1 = true;
                if (PropetyTool1.Results == Results.OK)
                {
                    int index = listPointChoose[0].Item2;
                    if (index < PropetyTool1.Propety.listP_Center.Count)
                    {
                        listLine1Point[0] = PropetyTool1.Propety.listP_Center[index];
                        listRot[0] = PropetyTool1.Propety.rectRotates[index];
                    }

                    else
                        Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                }
                else
                {
                    Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                }


            }
            if (PropetyTool2 != null)
                if (!IsDone2)
                if (PropetyTool2.StatusTool == StatusTool.Done || !Global.IsRun)
                 {
                IsDone2 = true;
                if (PropetyTool2.Results==Results.OK)
                {
                    int index = listPointChoose[1].Item2;
                    if (index < PropetyTool2.Propety.listP_Center.Count)
                    {
                        listLine1Point[1] = PropetyTool2.Propety.listP_Center[index];
                        listRot[1] = PropetyTool2.Propety.rectRotates[index];
                    }

                    else
                        Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                }
                else
                {
                    Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG; 
                }
            }
            if (PropetyTool3 != null)
                if (!IsDone3)
                    if (PropetyTool3.StatusTool == StatusTool.Done || !Global.IsRun)
                {
                    IsDone3 = true;
                    if (PropetyTool3.Results == Results.OK)
                    {
                        int index = listPointChoose[2].Item2;
                        if (index < PropetyTool3.Propety.listP_Center.Count)
                        {
                            listLine2Point[0] = PropetyTool3.Propety.listP_Center[index];
                            listRot[2] = PropetyTool3.Propety.rectRotates[index];
                        }

                        else
                            Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                    }
                    else
                    {
                        Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                    }

                }
            if (PropetyTool4 != null)
                if(!IsDone4)
                if (PropetyTool4.StatusTool == StatusTool.Done ||!Global.IsRun )
                {
                    try
                    {
                        IsDone4 = true;
                        if (PropetyTool4.Results == Results.OK)
                        {
                            int index = listPointChoose[3].Item2;
                            if (index < PropetyTool4.Propety.listP_Center.Count)
                            {
                                listRot[3] = PropetyTool4.Propety.rectRotates[index];
                                listLine2Point[1] = PropetyTool4.Propety.listP_Center[index];
                            }

                            else
                                Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                        }
                        else
                        {
                            Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                        }
                    }
                    catch (Exception ex)
                    {
                        String s = ex.Message;
                        //MessageBox.Show(s);
                    }

                }
            if (!IsDone1 || !IsDone2 || !IsDone3 || !IsDone4)
                goto X;
        }
        [NonSerialized]
        private   PointF pCenter1, pCenter2, pCenter3, pCenter4, pIntersection;
        public async Task SendResult()
        {
            if (Common.PropetyTools[IndexThread][Index].IsSendResult)
            {
                if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                    if (TypeMeasure == TypeMeasure.Angle)
                    {
                        await Global.ParaCommon.Comunication.Protocol.WriteResultFloat(Common.PropetyTools[IndexThread][Index].AddPLC, (float)AngleDetect);
                    }
                }
            }
        }
        public void Complete()
        {
           
            switch (TypeMeasure)
            {
                case TypeMeasure.Angle:
                   
                        IsDone1 = false;
                        IsDone2 = false;
                        IsDone3 = false;
                        IsDone4 = false;
                    Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
                        if ( Common.PropetyTools[Global.IndexChoose][Index].Results == Results.OK)
                        {
                            pCenter1 = listLine1Point[0];
                            pCenter2 = listLine1Point[1];
                            pCenter3 = listLine2Point[0];
                            pCenter4 = listLine2Point[1];
                            Cal.FindIntersection(pCenter1, pCenter2, pCenter3, pCenter4, out pIntersection);
                            AngleDetect = Cal.GetAngleBetweenSegments(pCenter1, pCenter2, pCenter3, pCenter4);
                            // AngleDetect = Cal.Finddistasnce(listLine1Point[0], listLine1Point[1]) / Scale;
                            AngleDetect = Math.Round(AngleDetect, 1);
                            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = (float)AngleDetect;
                            if (Common.PropetyTools[Global.IndexChoose][Index].ScoreResult <= Common.PropetyTools[Global.IndexChoose][Index].Score)
                                Common.PropetyTools[Global.IndexChoose][Index].Results = Results.OK;
                            else Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                        }
                       
                 
                    break;
                //case TypeMeasure.Distance:
                //    if (IsDone1 && IsDone2 || !IsOK)
                //    {
                //        IsDone1 = false;
                //        IsDone2 = false;
                //        switch (DirectMeasure)
                //        {
                //            case DirectMeasure.XY:
                //                pCenter1 = listLine1Point[0];
                //                pCenter2 = listLine1Point[1];
                //                AngleDetect = Cal.Finddistasnce(pCenter1, pCenter2) / Scale;
                //                break;
                //            case DirectMeasure.X:
                //                float width = Math.Abs(listRot[0]._rect.Width);
                //                float width2 = Math.Abs(listRot[1]._rect.Width);
                //                float Ymin1 = Math.Min(listLine1Point[0].Y - listRot[0]._rect.Height / 2, listLine1Point[1].Y - listRot[1]._rect.Height / 2);
                //                float Ymax1 = Math.Max(listLine1Point[0].Y + listRot[0]._rect.Height / 2, listLine1Point[1].Y + -listRot[1]._rect.Height / 2);
                //                float Xmin1 = Math.Min(listLine1Point[0].X, listLine1Point[1].X);
                //                float Xmax1 = Math.Max(listLine1Point[0].X, listLine1Point[1].X);
                //                pCenter1 = new PointF(Xmin1, Ymin1);
                //                pCenter2 = new PointF(Xmin1, Ymax1);
                //                pCenter3 = new PointF(Xmax1, Ymin1);
                //                pCenter4 = new PointF(Xmax1, Ymax1);
                //                AngleDetect = Cal.Finddistasnce(pCenter1, pCenter3) / Scale;
                //                break;
                //            case DirectMeasure.Y:
                //                float height = Math.Abs(listRot[0]._rect.Height);
                //                float height2 = Math.Abs(listRot[1]._rect.Height);
                //                float Xmin = Math.Min(listLine1Point[0].X, listLine1Point[1].X);
                //                float Xmax = Math.Max(listLine1Point[0].X, listLine1Point[1].X);
                //                pCenter1 = new PointF(Xmin, listLine1Point[0].Y - height / 2);
                //                pCenter2 = new PointF(Xmax, listLine1Point[0].Y - height / 2);
                //                pCenter3 = new PointF(Xmin, listLine1Point[1].Y - height2 / 2);
                //                pCenter4 = new PointF(Xmax, listLine1Point[1].Y - height2 / 2);
                //                AngleDetect = Cal.Finddistasnce(pCenter1, pCenter3) / Scale;
                //                break;
                //        }
                //        AngleDetect = Math.Round(AngleDetect, 2);
                //        Common.PropetyTools[Global.IndexChoose][Index].ScoreRs = (float)AngleDetect;
                //        if (ScoreRs <= Common.PropetyTools[Global.IndexChoose][Index].Score)
                //            IsOK = true;
                //        else IsOK = false;

                //        StatusTool = StatusTool.Done;
                //    }
                //    else
                //        StatusTool = StatusTool.Processing;
                //    break;

            }
        }
        [NonSerialized]
        public Mat matProcess = new Mat();

        public LineOrientation LineOrientation = LineOrientation.Vertical;
        [NonSerialized]
        public GapResult GapResult = new GapResult();
        public Graphics DrawResult(Graphics gc)
        {
          
         

            gc.ResetTransform();
          
          

            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
          
            gc.Transform = mat;
            // === Vẽ 2 line dựa vào listLine1Point và listLine2Point ===
            if (listLine1Point.Count >= 2 && listLine2Point.Count >= 2)
            {
                Point p1 = listLine1Point[0];
                Point p2 = listLine1Point[1];
                Point p3 = listLine2Point[0];
                Point p4 = listLine2Point[1];

                // highlight các điểm
                int r = 5;
                using (SolidBrush redBrush = new SolidBrush(Color.Red))
                using (SolidBrush blueBrush = new SolidBrush(Color.Blue))
                using (Pen yellowPen = new Pen(Color.Yellow, 1))
                {
                    gc.FillEllipse(redBrush, p1.X - r, p1.Y - r, r * 2, r * 2);
                    gc.DrawEllipse(yellowPen, p1.X - r, p1.Y - r, r * 2, r * 2);

                    gc.FillEllipse(redBrush, p2.X - r, p2.Y - r, r * 2, r * 2);
                    gc.DrawEllipse(yellowPen, p2.X - r, p2.Y - r, r * 2, r * 2);

                    gc.FillEllipse(blueBrush, p3.X - r, p3.Y - r, r * 2, r * 2);
                    gc.DrawEllipse(yellowPen, p3.X - r, p3.Y - r, r * 2, r * 2);

                    gc.FillEllipse(blueBrush, p4.X - r, p4.Y - r, r * 2, r * 2);
                    gc.DrawEllipse(yellowPen, p4.X - r, p4.Y - r, r * 2, r * 2);
                }

                // vẽ 2 line
                gc.DrawLine(new Pen(Color.Red, 2), p1, p2);
                gc.DrawLine(new Pen(Color.Blue, 2), p3, p4);

                //// === Tính góc giữa 2 line ===
                //double v1x = p2.X - p1.X;
                //double v1y = p2.Y - p1.Y;
                //double v2x = p4.X - p3.X;
                //double v2y = p4.Y - p3.Y;
                float cx = (p1.X + p2.X + p3.X + p4.X) / 4f;
                float cy = (p1.Y + p2.Y + p3.Y + p4.Y) / 4f;
                string txt = $"{AngleDetect:F2}°";
                using (Font font = new Font("Arial", 16, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.Yellow))
                {
                    gc.DrawString(txt, font, brush, cx + 5, cy + 5);
                }
                //double dot = v1x * v2x + v1y * v2y;
                //double mag1 = Math.Sqrt(v1x * v1x + v1y * v1y);
                //double mag2 = Math.Sqrt(v2x * v2x + v2y * v2y);

                //if (mag1 > 0 && mag2 > 0)
                //{
                //    double cosTheta = dot / (mag1 * mag2);
                //    cosTheta = Math.Max(-1.0, Math.Min(1.0, cosTheta));
                //    double angleRad = Math.Acos(cosTheta);
                //    double angleDeg = angleRad * 180.0 / Math.PI;

                //    this.AngleDetect = angleDeg;

                   
                  
                //}
            }

            return gc;
        }

    }
}
