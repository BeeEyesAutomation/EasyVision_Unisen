using BeeCore.Funtion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    public enum Corner
    {
        Both,
        Left,
        Right, None, Bottom,Top
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
        OCR,BarCode
        ,Yolo,
        Positions, Measure,Circle
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
        Basic_Tool,Extra_Tool_1,Extra_Tool_2,None
    }    
    public enum WindowView { 
        Window,
        Process
    }
    public enum TypeCrop
    {
        Crop, Area,Mask,None
    }
    public enum Mode
    {
        Pattern, OutLine,Edge
    }
    public enum Compares
    {
        Equal,  Less,More
    }
    public enum Function
    {
        LoadImage, LearnPattern
    }
    public enum StatusTool
    {
        None, Edit, Processing, Done,Initialed
    }
    public enum Results
    {
        None, OK, NG
    }
    public struct G
    {
      
        public static dynamic objYolo,np,objOCR,Classic;
        public static bool IsChecked = false, IniEdge;
   
        public static Model Model=new Model();
        public static bool IsSimulation = false,InitYolo=false,IniOCR;
        public static Common Common = new Common();
        public static ParaCommon ParaCam = new ParaCommon();
        public static CvPlus.ColorArea colorArea = new CvPlus.ColorArea();
        
        public static CvPlus.MatchingShape MatchingShape = new CvPlus.MatchingShape();
        public static CvPlus.Pattern pattern = new CvPlus.Pattern();
      
        public static bool IsCheck = false;
        public static CvPlus.Common CommonPlus = new CvPlus.Common();
        public static WindowView WindowView = WindowView.Window;
    }
   public static class GLobal
    {
       
    }
}
