﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    public enum StatusDraw
    {
        Edit,
        Check,
        Color,None
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
      None,Trigger, Result,Reset,Error,NoneErr,Close,ChangeMode,Light,ChangeProg
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
        Circle
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
        None, Edit, Processing, Done, Initialed
    }
    public enum Results
    {
        None, OK, NG
    }
    public enum Trig
    {
        None, Processing, Trigged, Continue, NotTrig, Complete
    }
    public enum StatusProcessing
    {
        None, Adjusting, WaitingDone, Processing, Done
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
