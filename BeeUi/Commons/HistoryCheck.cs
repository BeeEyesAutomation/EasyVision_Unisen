using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeUi.Commons
{
    public class HistoryCheck
    {
        public Bitmap bm;
        public bool IsOK;
        public DateTime date;

        public HistoryCheck(Bitmap bm,bool IsOK,DateTime date )
        {
            this.bm = bm;
            this.IsOK = IsOK;
            this.date = date;
        }
        public HistoryCheck() { 
        }
    }
}
