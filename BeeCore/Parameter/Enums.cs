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
    public enum ModeComunication
    {
        Server,
        Client
    }
    public enum MethordComunication
    {
        Params,
        Instance
    }
    public enum StatusComunication
    {
        Disconnect,
        ReConnect,
        Connected,
        Writing,
        Reading
    }
    public enum TypeComunication
    {
        MobusRS485,
        ModbusTCP,
        EthernetIP
    }
    public enum ConditionOK
    {
        TotalOK,
        AnyOK,
        Logic
    }
    public enum ArrangeBox
    {
        X_Left_Rigth,
        X_Right_Left,
        Y_Left_Rigth,
        Y_Right_Left,
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
    public enum TypePara
    {
        ExposureTime,
        Gain,
         Shift,

        None
    }
    public  class Enums
    {
    }
}
