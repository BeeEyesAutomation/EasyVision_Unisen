using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeGlobal
{
    public class Global
    {
        /// <summary>
        ///
        /// </summary>
        public static bool IsSendRS = false;
        public static bool TotalOK = false;
        //Gui
       
        public static bool IsLoadProgFist = false;
        public static bool IsHideTool = true;
        public static List<iTool> listItool = new List<iTool>();
        public static System.Drawing.Point pShowTool=new Point(10,10);
        public static float ScaleZoom;
        public static Point pScroll;
        public static dynamic ToolSettings;
        public static Font fontTool = new Font("Arial", 22, FontStyle.Regular);
        public static Font fontRS = new Font("Arial", 32, FontStyle.Bold);
        public static StatusDraw _StatusDraw = StatusDraw.None;
        public static event Action<StatusDraw> StatusDrawChanged;
        public static Config Config;
        public static  bool IsLearning=false;
        public static bool IsOCR = false;
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
        public static StatusIO _StatusIO = StatusIO.NotConnect;
        public static event Action<StatusIO> StatusIOChanged;
        public static StatusIO StatusIO
        {
            get => _StatusIO;
            set
            {
                if (_StatusIO != value)
                {
                    _StatusIO = value;
                    StatusIOChanged?.Invoke(_StatusIO); // Gọi event
                }
            }
        }
        public static StatusProcessing _StatusProcessing = StatusProcessing.None;
        public static event Action<StatusProcessing> StatusProcessingChanged;
        public static StatusProcessing StatusProcessing
        {
            get => _StatusProcessing;
            set
            {
                if (_StatusProcessing != value)
                {
                    _StatusProcessing = value;
                    StatusProcessingChanged?.Invoke(_StatusProcessing); // Gọi event
                }
            }
        }public static bool TriggerInternal = false;

        public static bool Initialed = false;
        public static Step Step = Step.Run;
        public static float Scale = 1, AngleOrigin;
        public static  Color ColorOK = Color.FromArgb(0, 172, 73);
        public static Color ColorNG = Color.DarkRed;
        public static Color ColorRead = Color.SkyBlue;
        public static Color ColorProssing = Color.Blue;
        public static Color ColorTrigger = Color.LightBlue;
        public static Color ColorNone = Color.Gray;
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
        public static bool IsEditTool = false;
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
