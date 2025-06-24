using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeCore.Algorithm
{

    public class FilterItem
    {
        public string Name { get; }
        public ImageFilter Filter { get; }   // tạo UserControl tham số
        public Image Icon { get; }                       // icon hiển thị

        public FilterItem(string name, ImageFilter filter, Image icon)
        {
            Name = name;
            Filter = filter;
            Icon = icon;
        }

        public override string ToString() => Name;        // phòng khi cần hiển thị text
      
    }
    }

