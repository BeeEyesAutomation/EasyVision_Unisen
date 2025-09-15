
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class ParaCamera
    {
        public ValuePara Width,Height,OffSetX,OffSetY;
        public ValuePara Exposure,Gain,Briness,Shift;
        public bool IsConnect;
        public float CenterX;
        public float CenterY;
        public String Ex;
        public float Fps;
        public TypeCamera TypeCamera = TypeCamera.USB;
        public String Name = "";
        public int Focus, Zoom;
        public int ColorCode = -1;
        public ParaCamera()
        {
            if (Exposure == null)
                Exposure = new ValuePara();
            if (Gain == null)
                Gain = new ValuePara();
            if (Shift == null)
                Shift = new ValuePara();
            if (Width == null)
                Width = new ValuePara();
            if (Height == null)
                Height = new ValuePara();
            if (OffSetX == null)
                OffSetX = new ValuePara();
            if (OffSetY == null)
                OffSetY = new ValuePara();
        }
    }
}
