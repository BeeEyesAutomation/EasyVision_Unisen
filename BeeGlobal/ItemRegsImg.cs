using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable]
    public class ItemRegsImg
    {
        public String Name;
        public Bitmap Image;
        public ItemRegsImg(String Name,Bitmap Imgage)
        {
            this.Name = Name;
            this.Image = Imgage;
        }
    }
}
