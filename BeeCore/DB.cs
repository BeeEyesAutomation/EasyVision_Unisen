using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class DB
    {
        public String StringConnect = "";
        public bool IsCoonect;
        
        public bool Connect()
        {
            return false;
        }
        public DB() { }
    }
}
