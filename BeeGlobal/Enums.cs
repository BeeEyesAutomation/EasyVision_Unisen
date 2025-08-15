using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    public enum ShaftMeasureType
    {
        /// <summary>Khoảng cách giữa hai đường tâm</summary>
        CenterLine,
        /// <summary>Khe nhỏ nhất giữa hai biên</summary>
        MinEdge,
        /// <summary>Khoảng cách mép lớn nhất (max span)</summary>
        MaxEdge
    }
    public enum LineOrientation
    {
        Any, Horizontal, Vertical
    }
    public enum GapExtremum
    {
        Nearest,
        Farthest,
        Outermost,
        Middle
    }
    public enum TypeIO
    {
        Input,
        Output
    }
    public enum I_O_Input
    {
        None,
        Trigger,
        Mode,
        Live,
        ByPass,
        Reset,
        Shuttdown,
        Prog1,
        Prog2,
        Prog3,
        Prog4
    }
    public enum I_O_Output
    {
        None,
        Result,
        Ready,
        Busy,
        Error,
        Logic1,
        Logic2,
        Logic3,
        Logic4,
        Light1,
        Light2,
        Light3,

    }
    public enum SegmentStatType { Shortest, Longest, Average }
    public enum StatusDraw
    {
        Edit,
        Check,
        Color,None
    }
    public enum StatusIO
    {
        NotConnect,
        Reading,
        None,
        Writing, ErrRead,ErrWrite
    }
    public enum MethordEdge
    {
        StrongEdges,
        CloseEdges,
        Binary,None
    }
    public enum TypeMeasure
    {
        Angle,Distance
    }
    public enum DirectMeasure
    {
       X,Y,XY
    }
    public enum MethordMeasure
    {
       Min,Max,Medium
    }
    public enum Corner
    {
        Both,
        Left,
        Right, None, Bottom, Top
    }
    public enum AnchorPoint
    {
        TopLeft, TopRight, BottomLeft, BottomRight, Rotation, Center, None
    }
    public enum Result
    {
        Success,
        Failure,
        NotFound,
        NoDevice,
        LostConnect,
        More,
        Less,
        Null
    }
    public enum IO_Processing
    {
      None,Trigger, Result,Reset,Error,NoneErr,Close,ChangeMode,Light,ChangeProg,ByPass
    }
    public enum AreaCrop
    {
        Rect, Circle
    }
    public enum TypeTool
    {
        Position_Adjustment,
        Pattern,
        MatchingShape,
        OutLine,
        Edge_Pixels,
        Color_Area,
        Width,
        Diameter,
        Edge,
        Pitch,
        OCR, BarCode
        , Learning,
        Positions,
        Measure,
        Circle,OKNG
    }
    public enum TypeOCR
    {
        CPU,
        GPU
    }
    public enum TypeYolo
    {
        FAST,
        YOLO,
        RCNN
    }
    public enum TypeCtr
    {
        Menu,
        Bar,
        BG,
        Text
    }
    public enum GroupTool
    {
        Basic_Tool, Extra_Tool_1, Extra_Tool_2, None
    }
    public enum WindowView
    {
        Window,
        Process
    }
    public enum TypeCrop
    {
        Crop, Area, Mask, None
    }
    public enum Mode
    {
        Pattern, OutLine, Edge
    }
    public enum Compares
    {
        Equal, Less, More
    }
    public enum Function
    {
        LoadImage, LearnPattern
    }
    public enum StatusTool
    {
        NotInitial,  WaitCheck, Edit, Processing, Done
    }
   
    public enum Results
    {
        None, OK, NG,ERR, REVERSE,
    }
    public enum Trig
    {
        None, Processing, Trigged, Continue, NotTrig, Complete
    }
    public enum StatusProcessing
    {
        None,Trigger,Read,Checking, Adjusting, WaitingDone,SendResult, Done
    }
    public enum StatusMode
    {
        None, SimOne, SimContinuous, Once, Continuous
    }
    public enum Step
    {
        Step1, Step2, Step3, Step4, Run, PLC
    }
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
    public class Enums
    {
    }
 
}
