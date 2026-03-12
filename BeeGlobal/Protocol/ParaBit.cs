using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BeeGlobal
{
    [Serializable()]
    public class ParaBit
    {
        public I_O_Output OldOutPut;
        public I_O_Input OldInPut;
        public bool IsBlink=false;
        public TypeIO TypeIO;
        public I_O_Input _I_O_Input;
        public I_O_Output _I_O_Output ;
        public ValueInput ValueInput;
        public ValueOutput ValueOutput;
        [field: NonSerialized]
        public event Action< I_O_Output> OutputChanged;
        public I_O_Output I_O_Output
        {
            get => _I_O_Output;
            set
            {
                if (_I_O_Output != value)
                {
                    _I_O_Output = value;

                    OutputChanged?.Invoke( _I_O_Output); // Gọi event
                }
            }
        }
        [field: NonSerialized]
        public event Action<I_O_Input> InputChanged;
        public I_O_Input I_O_Input
        {
            get => _I_O_Input;
            set
            {
                if (_I_O_Input != value)
                {
                    _I_O_Input = value;

                    InputChanged?.Invoke(_I_O_Input); // Gọi event
                }
            }
        }
        public int Adddress = 0;
        [field: NonSerialized]
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
        [field: NonSerialized]
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
