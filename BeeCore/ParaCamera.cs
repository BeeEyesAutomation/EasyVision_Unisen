using BeeCore.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class ParaCamera
    {
        public ValuePara Width,Height,OffSetX,OffSetY;
        public ValuePara Exposure,Gain,Briness,Shift;
        public bool IsConnect;
        public String Ex;
        public float Fps;
        public TypeCamera TypeCamera = TypeCamera.USB;
        public String Name = "";

        public ParaCamera()
        {
            if (Exposure == null)
                Exposure = new ValuePara();
            if (Gain == null)
                Gain = new ValuePara();
            if (Shift == null)
                Shift = new ValuePara();

        }
    }
}
