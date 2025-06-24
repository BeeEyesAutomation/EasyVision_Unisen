using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public  class LocalTool
    {

        public ColorArea ColorArea;
        public TypeCrop TypeCrop;
        public TypeTool TypeTool;
        public RectRotate rotArea, rotCrop;
        public OutLine OutLine;
        public bool IsFullSize = true;
        public bool IsClear = false;
       
        public Mat matTemp = new Mat();
        public Mat matClear = new Mat(); Mat matMask = new Mat();
        public LocalTool()
        {

        }
    }
}
