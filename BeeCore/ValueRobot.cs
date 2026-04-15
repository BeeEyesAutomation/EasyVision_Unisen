using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class ValueRobot
    {
        public int Val1 { set; get; }
        public int Val2 { set; get; }
        public int Val3 { set; get; }
        public int Val4 { set; get; }

        public int Val5 { set; get; }
        public int Val6 { set; get; }
        public ValueRobot(int val1, int val2, int val3, int val4, int val5, int val6)
        {
            Val1 = val1;
            Val2 = val2;
            Val3 = val3;
            Val4 = val4;
            Val5 = val5;
            Val6 = val6;
        }
    }
}
