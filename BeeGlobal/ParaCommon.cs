
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = System.Drawing.Size;

namespace BeeGlobal
{
  
    [Serializable()]
    public class ParaCommon
    {

        public bool IsExternal = false;
        public String CardChoosed = "";
        public Size SizeCCD;
        public  int _Exposure = 0, _Gain=1, _TypeResolution=1, _TypeLight=1;
        public bool IsOnLight = false, IsEqualization, IsRevese, IsMirror, IsHance;
        public int TypeLight 
         {
            get => _TypeLight; set
            {
                _TypeLight = value;
                if (_TypeLight > 0)
                {
             // BeeCore.Camera.Light(_TypeLight, IsOnLight);

                   
                       
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
              //BeeCore.Camera.SetReSolution(_TypeResolution);

                   
                       
                }
            }
        }
        public Comunication Comunication = new Comunication();
        public List<String> NameCCDs = new List<string>();
        public Bitmap matRegister;
        public Bitmap matSample;
    }
}
