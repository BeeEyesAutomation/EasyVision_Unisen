using BeeCore.Funtion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
  
    public struct G
    {
      
        public static dynamic objYolo,np,objOCR,Classic;
        public static bool IsChecked = false, IniEdge;
        public static bool IsSimulation = false,InitYolo=false,IniOCR;
        public static Common Common = new Common();
       
      
        
        public static CvPlus.MatchingShape MatchingShape = new CvPlus.MatchingShape();
      
      
        public static CvPlus.Common CommonPlus = new CvPlus.Common();
        //public static WindowView WindowView = WindowView.Window;
    }
   public static class GLobal
    {
       
    }
}
