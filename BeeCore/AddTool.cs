using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public enum TypeTools
    {
        Width,OutLine,OutLine1
    }
   public class AddTool
    {
      public  TypeTools typeTools;
        public List<String> listContent = new List<string>();
        public List<Image> listImage = new List<Image>();
        public AddTool(TypeTools typeTools, List<String> listContent, List<Image> listImage)
        {
            this.typeTools = typeTools;
            this.listContent = listContent;
            this.listImage = listImage;
        }
    }
}
