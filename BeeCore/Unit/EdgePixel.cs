using BeeGlobal;
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
        public int Index = -1;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public int NumPixelTemp, NumPixelComPare;
        public bool IsOK = false;
       
        public Mode TypeMode;
      
     
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
      
      
       
      
        public EdgePixel()
        {

        }
        int IndexThead;
        public  void LearnPattern(String path,int indexTool)
        {

           
            G.CommonPlus.LoadDst(path);
            pattern.LearnPattern(minArea, indexTool, IndexThead);

        }
        CvPlus.Pattern pattern = new CvPlus.Pattern();

    
        public bool IsBitNot { get => isBitNot; set => isBitNot = value; }
       
        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();
 
    }
}
