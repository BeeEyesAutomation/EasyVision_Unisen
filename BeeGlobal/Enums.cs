using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    public enum ModeCheck
    {
       Single,Multi
    }
    public enum ColorGp
    {
        HSV,RGB,BGR
    }
    public enum Users
    {
        User, Leader, Admin
    }
    //Corner
    public enum Values
    {
        Mean, Median, Min,Max
    }
    public enum MethodSample
    {
        Pattern,
        Corner,
        Edge,
    }
    public enum LinePairStrategy
    {
        //BothMaxInliers,        // (cũ) 2 line inliers cao & gần 90°
        StrongPlusOrth,        // line mạnh nhất + line còn lại gần 90° nhất (từ candidates)
        StrongPlusContourOrth  // line mạnh nhất + line còn lại fit từ contour và ÉP 90° chính xác
    }
    public enum FillMode1 { Cover, Contain }
    public enum PLCStatus
    {
        NotConnect,
        Reconnect,
        ErrorConnect,
        Ready,
       
    }
    public enum CameraStatus
    {
        NotConnect,
        Reconnect,
        ErrorConnect,
        Ready,

    }
    public enum TypeSendPLC
    {
        Bit,
        Bits,
        String,
        Float,
        Int
    }
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
    public enum TriggerNum
     {
        Trigger0,
        Trigger1,
        Trigger2,
        Trigger3,
        Trigger4
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
        Trigger2,
        Trigger3,
        Trigger4,
        Mode,
        Live,
        ByPass,
        Reset,
        Shuttdown,
        Prog1,
        Prog2,
        Prog3,
        Prog4,
        Light1,
        Light2,
        Light3,
        Alive
    }
    public enum I_O_Output
    {
        None,
        Result,
        Result2,
        Result3,
        Result4,
        Ready,
        Ready2,
        Ready3,
        Ready4,
        Reset,
        Busy,
        Busy2,
        Busy3,
        Busy4,
        Error,
        Logic1,
        Logic2,
        Logic3,
        Logic4,
        Light1,
        Light2,
        Light3,
        Alive,
        Logic5,
        Logic6,

    }
    public enum SegmentStatType { Shortest, Longest, Average }
    public enum StatusDraw
    {
        Edit,
        Check,
        Color,None,Choose,Scan
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
        Binary,InvertBinary,None
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
    public enum ShapeType
    {
        Rectangle,
        Ellipse,Hexagon,Polygon
    }

    public enum AnchorPoint
    {
        None,
        TopLeft, TopRight, BottomLeft, BottomRight,
        Rotation, Center,
        V0, V1, V2, V3, V4, V5 , Vertex// 6 đỉnh lục giác
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
      None,Trigger, Trigger2, Trigger3, Trigger4, Result,Reset,Error,NoneErr,Close,ChangeMode,Light,ChangeProg,ByPass,Busy
    }
    public enum AreaCrop
    {
        Rect, Circle
    }
    public enum TypeTool
    {
        Position_Adjustment=0,
        Pattern=1,
        OKNG = 16,
        Color_Area = 5,
        MatchingShape = 2,
        Crop = 17,
        Width=6,
        Circle = 15,
        Measure = 14,
        Learning = 12,
        OCR =10,
        BarCode=11, 
        Corner=18,
        VisualMatch=19,
        Pitch = 20,//note
        EdgePixel= 21, 
        Edge=22,
        CraftOCR = 23,
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
        None,Trigger,Read,Checking, Adjusting, WaitingDone,SendResult, Done,Drawing
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
        
        MVS,
        TinyIV,
        Pylon,
       
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
