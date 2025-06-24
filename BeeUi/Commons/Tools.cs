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
        public Tools (ItemTool ItemTool,dynamic tool)
        {
            this.ItemTool = ItemTool;
            this.tool = tool;
           
        }
        public Tools Clone()
        {

            var clonedJson = JsonConvert.SerializeObject(this);

            return JsonConvert.DeserializeObject<Tools>(clonedJson);
        }
    }
}
