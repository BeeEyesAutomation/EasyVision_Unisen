using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public  class Model
    {
       
       public  CCD CCD = new CCD();
       // public  Modbus PLC = new Modbus();
       
        public  Model( )
        {

        }
    }
}
