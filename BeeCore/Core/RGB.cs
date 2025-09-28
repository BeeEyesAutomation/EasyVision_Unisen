using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Core
{
    [Serializable()]
    public class RGB
    {
        public int R;
        public int G;
        public int B;
        public RGB()
        {

        }
        public RGB(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
