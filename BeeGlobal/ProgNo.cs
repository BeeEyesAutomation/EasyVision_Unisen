using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable]
    public class ProgNo
    {
        public int No;
        public String Name;
        public ProgNo(int No ,String Name) {
            this.No = No;
            this.Name = Name;
        } 
    }
}
