using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class dataMat
    {
        public IntPtr data;
        public int Rows;
        public int Cols;
        public int  Type;
        public dataMat(int Rows, int Cols, int Type, IntPtr data)
        {
            this.data = data;
            this.Rows = Rows;
            this.Cols = Cols;
            this.Type = Type;
        }
    }
}
