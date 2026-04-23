using BeeCore.Core;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class LabelItem
        {

        public LabelItem(String Name) { this.Name = Name; }
        public string Name { get; set; }
        public bool IsArea { get; set; }
        public bool IsRight{ get; set; }
        public bool IsUse { get; set; }
        public bool IsWidth { get; set; }
        public bool IsHeight { get; set; }
        public bool IsCounter { get; set; }
        public bool IsOK = false;
        public bool IsLabelMark { get; set; }
        //public bool IsLine { get; set; }

        public bool IsX { get; set; }//HAU
        public bool IsXMax { get; set; }//HAU
        public bool IsY { get; set; }//HAU
        public bool IsYMax { get; set; }//HAU
        public bool IsDistance { get; set; }//HAU
        public int ValueArea { get; set; }
            public int ValueWidth { get; set; }
            public int ValueHeight { get; set; }
        //public int ValueLine { get; set; }
        public int ValueX { get; set; }//HAU
        public int ValueDistance { get; set; }//HAU
        public int ValueY { get; set; }//HAU
        public int ValueXMax { get; set; }//HAU
        public int ValueYMax { get; set; }//HAU
        public int ValueCounter { get; set; }//HAU
                                             // ===== LIST BOX AREA =====
        public bool IsMinColor { get; set; }
        public int ValueMinColor { get; set; }
        public int ValueExternColor { get; set; } // nằm trong MinColor
      //  public Color SampleColor { get; set; } = Color.Empty;
        public bool IsChoosingColor { get; set; }
        // NEW: list màu hiển thị trên nút Color
        public List<Color> ListColor { get; set; } = new List<Color>();
        /// <summary>
        /// Các index box được chọn
        /// </summary>
        public List<int> ListTempColor { get; set; } = new List<int>();
        public List<RectRotate> ListInsideBox { get; set; } = new List<RectRotate>();
        [field: NonSerialized]
        public List<int> ListIndexBoxBackup { get; set; } = new List<int>();
        [field: NonSerialized]
        public List<BeeCpp.ColorArea> ListColorArea  { get; set; }
        public List<HSV> ListHSV { get; set; }
    }

}
