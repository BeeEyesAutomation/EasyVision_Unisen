using BeeCore.Funtion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public  class PropetyTool : ICloneable
    {
        public PropetyTool()
        {

        }
        public String Name = "";
        public dynamic Propety;
        public TypeTool TypeTool;
        public UsedTool UsedTool=UsedTool.NotUsed;
        public PropetyTool(dynamic Propety, TypeTool TypeTool,String Name)
        {
            this.Name = Name;
            this.TypeTool = TypeTool;
            this.Propety = Propety;

        }

        public object Clone()
        {
            PropetyTool propety = new PropetyTool(this.Propety.Clone(), this.TypeTool, this.Name);
            return propety;
        }
    }
}
