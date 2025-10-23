
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
        public int NumRetryCamera = 0;
        public int NumRetryPLC = 0;
        public bool IsResetReady = false;
        public bool IsForceByPassRS = false;
        public bool IsOnlyTrigger = false;
        public bool IsMultiCamera = false;
        public  bool IsONNG = false;
        public int NumTrig = 1;
        public bool IsAutoReload = false;
        public List<int> indexLogic1 = new List<int>();
        public List<int> indexLogic2 = new List<int>();
        public List<int> indexLogic3 = new List<int>();
        public List<int> indexLogic4 = new List<int>();
        public List<int> indexLogic5 = new List<int>();
        public List<int> indexLogic6 = new List<int>();
        private bool _IsExternal = false;
        [field: NonSerialized]
        public event Action<bool> ExternalChange;
        public bool IsExternal
        {
            get => _IsExternal;
            set
            {
                if (_IsExternal != value)
                {
                    _IsExternal = value;
                    ExternalChange?.Invoke(_IsExternal); // Gọi event
                }
            }
        }
        public String CardChoosed = "";
        public Size SizeCCD;
        public  int _Exposure = 0, _Gain=1, _TypeResolution=1, _TypeLight=1;
        public bool IsOnLight = false, IsEqualization, IsRevese, IsMirror, IsHance;
        public bool IsSaveLog = false;
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
