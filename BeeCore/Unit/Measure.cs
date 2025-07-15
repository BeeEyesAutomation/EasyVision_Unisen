
using BeeGlobal;
using OpenCvSharp;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
            StatusTool = StatusTool.Initialed;
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

        public TypeTool TypeTool = TypeTool.Measure;
        public StatusTool StatusTool = StatusTool.None;
        public int cycleTime;
        public bool IsOK = false;
        public List<RectRotate> listRot = new List<RectRotate> { new RectRotate(), new RectRotate(), new RectRotate(), new RectRotate() };
        public List<Point> listLine1Point = new List<Point>();
        public List<Point> listLine2Point = new List<Point>();
        public List<Tuple<String, int>> listPointChoose = new List<Tuple<String, int>>();
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public float Score, ScoreRs;
        public void IniTool()
        {
            if(listRot==null)
            {
                listRot = new List<RectRotate> { new RectRotate(), new RectRotate(), new RectRotate(), new RectRotate() };
            }
            if(listPointChoose==null)
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
      
        public void DoWork()
        {
          

        }
    
        public void Complete()
        {
           
        }
    }
}
