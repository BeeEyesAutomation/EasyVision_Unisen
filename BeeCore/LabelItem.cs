using System;
using System.Collections.Generic;
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
        public bool IsUse { get; set; }
        public bool IsWidth { get; set; }
        public bool IsHeight { get; set; }
        public bool IsCounter { get; set; }
        //public bool IsLine { get; set; }

        public bool IsX { get; set; }//HAU

        public bool IsY { get; set; }//HAU
        public int ValueArea { get; set; }
            public int ValueWidth { get; set; }
            public int ValueHeight { get; set; }
        //public int ValueLine { get; set; }
        public int ValueX { get; set; }//HAU
        public int ValueY { get; set; }//HAU
        public int ValueCounter { get; set; }//HAU
    }
    
}
