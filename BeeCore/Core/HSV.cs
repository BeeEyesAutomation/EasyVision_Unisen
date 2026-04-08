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
        public override bool Equals(object obj)
        {
            if (obj is HSV other)
            {
                return Math.Abs(this.H - other.H) < 1 &&
                       Math.Abs(this.S - other.S) < 0.01f &&
                       Math.Abs(this.V - other.V) < 0.01f;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (H, S, V).GetHashCode();
        }
        public HSV(int h, int s, int v)
        { H = h; S = s; V = v; }
    }
}
