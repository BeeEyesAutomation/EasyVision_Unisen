using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]    
    public  class PLCResult
    {
        public String Add = "";
        public ValuePLC ValuePLC = ValuePLC.TotalOK;
        public String NameTool="System";
        public PLCResult(string add, ValuePLC valuePLC, String nameTool)
        {
            Add = add;
            ValuePLC = valuePLC;
            NameTool = nameTool;
        }
    }
}
