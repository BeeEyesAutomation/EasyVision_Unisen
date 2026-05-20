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
        public String Name = "";       // User-defined name; tool/global lookup theo Name
        public String ToolKey = "";    // Chi dung khi TypeValuePLC == Tool (IndexProg/IndexTool/Field)
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

        public ParaValue(String Name, TypeValuePLC TypeValuePLC, TypeVar TypeVar, TypeIO TypeIO, String Adddress)
        {
            this.Name = Name;
            this.TypeVar = TypeVar;
            this.TypeValuePLC = TypeValuePLC;
            this.TypeIO = TypeIO;
            this.Adddress = Adddress;
        }


    }

    public static class TypeValuePLCMap
    {
        public static dynamic GetValue(TypeValuePLC key, ParaValue pv = null)
        {
            var p = Global.Comunication?.Protocol;
            switch (key)
            {
                case TypeValuePLC.TotalOK:   return Global.Config.SumOK;
                case TypeValuePLC.TotalNG:   return Global.Config.SumNG;
                case TypeValuePLC.Total:     return Global.Config.SumTime;
                case TypeValuePLC.Qty:       return p?.ValueQty ?? 0;
                case TypeValuePLC.Cycle:     return Global.Cycle;
                case TypeValuePLC.NoProg:    return p?.NoProg ?? 0;
                case TypeValuePLC.CountProg: return p?.ValueCountProg ?? 0;
                case TypeValuePLC.Progress:  return p?.ValueProgress ?? 0;
                case TypeValuePLC.PO:        return p?.ValuePO ?? "";
                case TypeValuePLC.Tool:
                case TypeValuePLC.Custom:    return pv?.Value;
                default:                     return null;
            }
        }

        public static void SetValue(TypeValuePLC key, dynamic v, ParaValue pv = null)
        {
            var p = Global.Comunication?.Protocol;
            switch (key)
            {
                case TypeValuePLC.TotalOK:   Global.Config.SumOK = Convert.ToInt32(v); break;
                case TypeValuePLC.TotalNG:   Global.Config.SumNG = Convert.ToInt32(v); break;
                case TypeValuePLC.Total:     Global.Config.SumTime = Convert.ToInt32(v); break;
                case TypeValuePLC.Qty:       if (p != null) p.ValueQty = Convert.ToInt32(v); break;
                case TypeValuePLC.Cycle:     Global.Cycle = Convert.ToSingle(v); break;
                case TypeValuePLC.NoProg:    if (p != null) p.NoProg = Convert.ToInt32(v); break;
                case TypeValuePLC.CountProg: if (p != null) p.ValueCountProg = Convert.ToInt32(v); break;
                case TypeValuePLC.Progress:  if (p != null) p.ValueProgress = Convert.ToInt32(v); break;
                case TypeValuePLC.PO:        if (p != null) p.ValuePO = Convert.ToString(v); break;
                case TypeValuePLC.Tool:
                case TypeValuePLC.Custom:    if (pv != null) pv.Value = v; break;
            }
        }

        public static TypeVar DefaultTypeVar(TypeValuePLC key)
        {
            switch (key)
            {
                case TypeValuePLC.PO:       return TypeVar.String;
                case TypeValuePLC.Cycle:
                case TypeValuePLC.Progress: return TypeVar.Float;
                default:                    return TypeVar.Int;
            }
        }

        public static bool IsReadOnlySource(TypeValuePLC key)
        {
            // Tool/Custom: gia tri nam tren ParaValue.Value, set tu UI/tool, khong phai tu PLC sync
            return key == TypeValuePLC.None;
        }
    }
}
