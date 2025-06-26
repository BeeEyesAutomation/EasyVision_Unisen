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
        public int Width,Height,OffSetX,OffSetY;
        public float Exposure,Gain,Briness;
        public bool IsConnect;
        public String Ex;
        public float Fps;
        public TypeCamera TypeCamera = TypeCamera.USB;
        public String Name = "";

        public ParaCamera()
        {

        }
    }
}
