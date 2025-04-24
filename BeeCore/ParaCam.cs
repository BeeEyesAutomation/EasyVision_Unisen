using BeeCore.Funtion;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = System.Drawing.Size;

namespace BeeCore
{
  
    [Serializable()]
    public class ParaCam
    {
        public  int Exposure
        {
            get => _Exposure; set
            {
                _Exposure = value;
                if (BeeCore.G.ParaCam._Exposure > 0)
                    BeeCore.Camera.SetExpo(_Exposure);
            }
        }
        public Size SizeCCD;
        public  int _Exposure = 0, _TypeResolution=1, _TypeLight=1;
        public bool IsOnLight = false;
        public int TypeLight 
         {
            get => _TypeLight; set
            {
                _TypeLight = value;
                if (_TypeLight > 0)
                {
              BeeCore.Camera.Light(_TypeLight, IsOnLight);

                   
                       
                }
}
        }
        public int numToolOK = 0;
        public int TypeResolution
        {
            get => _TypeResolution; set
            {
                _TypeResolution = value;
                if (_TypeResolution >0)
                {
              BeeCore.Camera.SetReSolution(_TypeResolution);

                   
                       
                }
            }
        }
        public Bitmap matRegister;
        public Bitmap matSample;
    }
}
