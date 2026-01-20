using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class ParaValue
    {
        public TypeIO TypeIO;
        public TypeVar TypeVar;
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
        public event Action<object, int> ValueChanged;
        public int Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    ValueChanged?.Invoke(this, _Value); // Gọi event
                }
            }
        }
        public ParaValue()
        {

        }
     
        public ParaValue(TypeIO TypeIO, ValueInput ValueInput, TypeVar TypeVar, int Adddress)
        {
            this.TypeVar = TypeVar;
            this.ValueInput = ValueInput;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
        public ParaValue(TypeIO TypeIO, ValueOutput ValueOutput, TypeVar TypeVar, int Adddress)
        {   this.TypeVar = TypeVar;
            this.ValueOutput = ValueOutput;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
       
    }
}
