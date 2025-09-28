using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Core
{
    [Serializable()]
    public class HSV
    {
        public int H;
        public int S;
        public int V;
        public HSV()
        {

        }
        public HSV(int h, int s, int v)
        { H = h; S = s; V = v; }
    }
}
