﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    public class Global
    {
        //Gui
        public static Font fontTool = new Font("Arial", 22, FontStyle.Regular);
        public static Font fontRS = new Font("Arial", 32, FontStyle.Bold);
        public static StatusDraw _StatusDraw = StatusDraw.None;
        public static event Action<StatusDraw> StatusDrawChanged;
        public static StatusDraw StatusDraw
        {
            get => _StatusDraw;
            set
            {
                if (_StatusDraw != value)
                {
                    _StatusDraw = value;
                    StatusDrawChanged?.Invoke(_StatusDraw); // Gọi event
                }
            }
        }
        public static Trig StatusTrig = Trig.None;
        public static StatusMode StatusMode = StatusMode.None;

        public static StatusProcessing StatusProcessing = StatusProcessing.None;
        public static Step Step = Step.Run;
        public static float Scale = 1, AngleOrigin;
        public static TypeCrop _TypeCrop = TypeCrop.Crop;
        public static event Action<TypeCrop> TypeCropChanged;
        public static TypeCrop TypeCrop
        {
            get => _TypeCrop;
            set
            {
                if (_TypeCrop != value)
                {
                    _TypeCrop = value;
                    TypeCropChanged?.Invoke(_TypeCrop); // Gọi event
                }
            }
        }
        public static TypeCamera TypeCamera;
        public static Model Model = new Model();
        public static ParaCommon ParaCommon = new ParaCommon();
        public static bool IsRun=true;
        public static int IndexChoose = 0;
        public static String Project = "";
        public static RectRotate rotOriginAdj;
        public static Rectangle ClientRectangleMain;
        public static float angle_Adjustment = 0, X_Adjustment = 0, Y_Adjustment = 0;
        public static OpenCvSharp.Point pOrigin = new OpenCvSharp.Point();
        private static int _IndexToolSelected=-1;
        public static Comunication Comunication = new Comunication();
       public static event Action<int> IndexToolChanged;
        public static dynamic OldPropetyTool = null;
       
        public static int IndexToolSelected
        {
            get => _IndexToolSelected;
            set
            {
                if (_IndexToolSelected != value)
                {
                    _IndexToolSelected = value;
                    IndexToolChanged?.Invoke(_IndexToolSelected); // Gọi event
                }
            }
        }
        public static List<ParaCamera> listParaCamera = new List<ParaCamera> { null, null, null, null };
       
      
    }
   
}
