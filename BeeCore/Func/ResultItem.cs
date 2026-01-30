using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
   
    public class ResultItem
    {
        public ResultItem(String Name) { this.Name = Name; }
        public string Name { get; set; }
        public RectRotate rot { get; set; }
        public float Score { get; set; }
        public float Percent { get; set; }
        public float Area { get; set; }
        public float Distance { get; set; }
        public PointF point { get; set; }
        public bool IsOK { get; set; }
        public Mat matProcess = null;
      
    }

}
