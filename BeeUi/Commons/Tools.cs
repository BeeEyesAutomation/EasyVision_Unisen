using BeeCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Commons
{
    [Serializable()]
    public  class Tools
    {
        
        public Tools ()
        {

        }
        public dynamic tool;
        public ItemTool ItemTool;
        public PropetyTool PropetyTool;
        public Tools (ItemTool ItemTool,dynamic tool, PropetyTool PropetyTool)
        {
            this.ItemTool = ItemTool;
            this.tool = tool;
            this.PropetyTool = PropetyTool;
           
        }
        public Tools Clone()
        {

            var clonedJson = JsonConvert.SerializeObject(this);

            return JsonConvert.DeserializeObject<Tools>(clonedJson);
        }
    }
}
