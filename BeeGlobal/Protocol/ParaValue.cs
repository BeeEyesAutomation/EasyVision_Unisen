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
        public TypeValuePLC TypeValuePLC;
        public TypeVar TypeVar;
        public TypeIO TypeIO;
        public bool IsBlink = false;
        public String Adddress = "";
        private dynamic _Value = 0;
        //private string _ValueString = "";
        //[field: NonSerialized]
        //public event Action<object, string> StringChanged;
        //public string ValueString
        //{
        //    get => _ValueString;
        //    set
        //    {
        //        if (_ValueString != value)
        //        {
        //            _ValueString = value;
        //            StringChanged?.Invoke(this, _ValueString); // Gọi event
        //        }
        //    }
        //}
        [field: NonSerialized]
        public event Action<object, dynamic> ValueChanged;
        public dynamic Value
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
     
        public ParaValue(TypeValuePLC TypeValuePLC,TypeVar TypeVar, TypeIO TypeIO, String Adddress)
        {
            this.TypeVar = TypeVar;
            this.TypeValuePLC = TypeValuePLC;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }
   
       
    }
}
