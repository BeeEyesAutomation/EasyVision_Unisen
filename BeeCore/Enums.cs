using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    public enum TypeImg
    {
        Raw,
        Result,
        Crop,
        Process
    }
    public enum ConditionOK
    {
        TotalOK,
        AnyOK,
        Logic
    }
    public enum LogicOK
    {
        AND,
        OR,

    }
    public enum UsedTool
    {
        NotUsed,
        Used,
        Invertse
    }
    public enum TypeCamera
    {
        USB,
        BaslerGigE,
        TinyIV,

        None
    }
    public  class Enums
    {
    }
}
