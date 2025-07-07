using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class Labels
    {
        public Labels()
        {

        }
        public   String label;
        public bool IsEn;

        public Labels( String label,bool IsEn)
        {  this.label = label;
            this.IsEn = IsEn; 
        }
    }
}
