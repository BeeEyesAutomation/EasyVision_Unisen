using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class LabelItem
        { public LabelItem(String Name) { this.Name = Name; }
            public string Name { get; set; }
            public bool IsArea { get; set; }
        public bool IsUse { get; set; }
        public bool IsWidth { get; set; }
            public bool IsHeight { get; set; }

            public int ValueArea { get; set; }
            public int ValueWidth { get; set; }
            public int ValueHeight { get; set; }
        }
    
}
