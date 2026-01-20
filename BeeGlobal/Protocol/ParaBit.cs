using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class ParaBit
    {
        public TypeIO TypeIO;
        public I_O_Input I_O_Input;
        public I_O_Output I_O_Output ;
        public ValueInput ValueInput;
        public ValueOutput ValueOutput;
        public int Adddress = 0;
        private int _Value = 0;
        private string _ValueString = "";
        [field: NonSerialized]
        public event Action<object, string> StringChanged;
        public string ValueString
        {
            get => _ValueString;
            set
            {
                if (_ValueString != value)
                {
                    _ValueString = value;
                    StringChanged?.Invoke(this, _ValueString); // Gọi event
                }
            }
        }
        [field: NonSerialized]
        public event Action<object , int> ValueChanged;
        public int Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    ValueChanged?.Invoke(this,_Value); // Gọi event
                }
            }
        }
        public ParaBit()
        {
          
        }
        public ParaBit(TypeIO TypeIO ,I_O_Input I_O_Input, int Adddress)
        {
            this.I_O_Input = I_O_Input;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
        public ParaBit(TypeIO TypeIO, ValueInput ValueInput, int Adddress)
        {
            this.ValueInput = ValueInput;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
        public ParaBit(TypeIO TypeIO, ValueOutput ValueOutput, int Adddress)
        {
            this.ValueOutput = ValueOutput;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
        public ParaBit(TypeIO TypeIO, I_O_Output I_O_Output, int Adddress)
        {
            this.I_O_Output = I_O_Output;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
    }
}
