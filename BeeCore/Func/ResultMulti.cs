using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{

    [Serializable()]
    public class ResultMulti
    {

        public RectRotate RotOrigin = new RectRotate();
        public RectRotate RotCalib = new RectRotate();
        public Bitmap BTemp = null;
        [NonSerialized]
        public RectRotate RotCheck = new RectRotate();
        [NonSerialized]
        public float deltaX, deltaY, ZeroPixel,Score,ScoreColor;
        [NonSerialized]
        public Mat BCheckColor = null;
        [NonSerialized]
        public Mat BCropColor = null;
        [NonSerialized]
        public bool IsOK = false;
        [NonSerialized]
        public bool IsDot = false;
        public RectRotate rotAdj = null;
        public ResultMulti()
        {

        }
        public ResultMulti(RectRotate rotCalib,  Bitmap bTemp, RectRotate rotOrigin, Bitmap bTempColor,Bitmap bCheckColor, Bitmap bCropColor)
        {
            RotCalib = rotCalib;
            RotOrigin = rotOrigin;
            BTemp = bTemp;
            //BTempColor = bTempColor;
            rotAdj = rotCalib.Clone();
        }
       
    }

}
