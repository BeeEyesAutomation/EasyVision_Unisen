using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Funtion
{
    public class Init
    {
        public static void CCD(TypeCamera Type=TypeCamera.USB)
        {
            G.TypeCCD = Type;
            G.CCD.typeCCD =(int) G.TypeCCD;
        }
    }
}
