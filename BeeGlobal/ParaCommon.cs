
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
        public List<int> indexLogic1 = new List<int>();
        public List<int> indexLogic2 = new List<int>();
        public List<int> indexLogic3 = new List<int>();
        public List<int> indexLogic4 = new List<int>();
        public List<int> indexLogic5 = new List<int>();
        public List<int> indexLogic6 = new List<int>();
         public  int _TypeLight=1;
        // _Exposure = 0, _Gain=1, _TypeResolution=1,
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
        //        public int numToolOK = 0;
        //        public int TypeResolution
        //        {
        //            get => _TypeResolution; set
        //            {
        //                _TypeResolution = value;
        //                if (_TypeResolution >0)
        //                {
        //              //BeeCore.Camera.SetReSolution(_TypeResolution);



        //                }
        //            }
        //        }
      
       // public List<String> NameCCDs = new List<string>();
        public Bitmap matRegister;
        public Bitmap matRegister2;
        public Bitmap matRegister3;
        public Bitmap matRegister4;
        //  public Bitmap matSample;
    }
}
