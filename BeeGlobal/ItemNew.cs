using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeGlobal
{
    public class ItemNew
    {
        public Image Icon;
        public Image IconContent;
        public String Content;
        public TypeTool TypeTool;
        public GroupTool GroupTool;
        public dynamic btn;
        public ItemNew(TypeTool typeTool, GroupTool groupTool, Image icon, Image iconContent, string content)
        {
            GroupTool= groupTool;
            Icon = icon;
            IconContent = iconContent;
            Content = content;
            TypeTool = typeTool;
        }
    }
}
