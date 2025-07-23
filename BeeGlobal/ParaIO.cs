using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class ParaIO
    {
        public TypeIO TypeIO;
        public I_O_Input I_O_Input;
        public I_O_Output I_O_Output ;
        public int Adddress = 0;
        public int Value = 0;
        public ParaIO()
        {
          
        }
        public ParaIO(TypeIO TypeIO ,I_O_Input I_O_Input, int Adddress)
        {
            this.I_O_Input = I_O_Input;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
        public ParaIO(TypeIO TypeIO, I_O_Output I_O_Output, int Adddress)
        {
            this.I_O_Output = I_O_Output;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
    }
}
