using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class EdgePixel
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public TypeTool TypeTool;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public int NumPixelTemp, NumPixelComPare;
        public bool IsOK = false;
        private int score;
        public int ScoreRs = 0;
        public Mode TypeMode;
        //{
        //    get
        //    {
        //        return (Mode)(Convert.ToInt32( pattern.IsOutLine));
        //    }
        //    set
        //    {
               
        //        pattern.IsOutLine =Convert.ToBoolean( (int)value);
        //    }
        //}
     
        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public int cycleTime = 0;
        public RectangleF rectArea;
        private bool isBitNot = false;
        public int threshMin;
        //{
        //    //get
        //    //{
        //    //  //  return pattern.threshMin;
        //    //}
        //    //set
        //    //{
        //    //  //  pattern.threshMin = value;
        //    //}
        //}
        public int threshMax;
        //{
        //    //get
        //    //{
        //    //  //  return pattern.threshMax;
        //    //}
        //    //set
        //    //{
        //    //   // pattern.threshMax = value;
        //    //}
        //}
      
        public int minArea
        {
            get
            {
                return pattern.m_iMinReduceArea;
            }
            set
            {
                pattern.m_iMinReduceArea = value;
            }
        }
      
        public bool IsProcess
        {
            get
            {
                return pattern.IsProcess;
            }
            set
            {
                pattern.IsProcess = value;
            }
        }
       
      
        public EdgePixel()
        {

        }
 
        public  void LearnPattern(String path,int indexTool)
        {

           
            G.CommonPlus.LoadDst(path);
            pattern.LearnPattern(minArea, indexTool);

        }
        CvPlus.Pattern pattern = new CvPlus.Pattern();

    
        public bool IsBitNot { get => isBitNot; set => isBitNot = value; }
        public int Score { get => score; set => score = value; }

        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();
 
    }
}
