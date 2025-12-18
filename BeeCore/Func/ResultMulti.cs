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
        public Bitmap BTemp = null, BTempColor=null;
        [NonSerialized]
        public RectRotate RotCheck = new RectRotate();
        [NonSerialized]
        public float deltaX, deltaY, ZeroPixel,Score,ScoreColor;
        [NonSerialized]
        public Bitmap BCheckColor = null;
        [NonSerialized]
        public Bitmap BCropColor = null;

        public ResultMulti(RectRotate rotCalib,  Bitmap bTemp, RectRotate rotOrigin, Bitmap bTempColor,Bitmap bCheckColor, Bitmap bCropColor)
        {
            RotCalib = rotCalib;
            RotOrigin = rotOrigin;
            BTemp = bTemp;
            BTempColor = bTempColor;
        }
       
    }

}
