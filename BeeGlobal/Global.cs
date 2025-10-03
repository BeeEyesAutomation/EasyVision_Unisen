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
        private static PLCStatus _PLCStatus = PLCStatus.NotConnect;
        public static event Action<PLCStatus> PLCStatusChanged;
        public static PLCStatus PLCStatus
        {
            get => _PLCStatus;
            set
            {
                if (_PLCStatus != value)
                {
                    _PLCStatus = value;
                    PLCStatusChanged?.Invoke(_PLCStatus); // Gọi event
                }
            }
        }
        private static CameraStatus _CameraStatus = CameraStatus.NotConnect;
        public static event Action<CameraStatus> CameraStatusChanged;
        public static CameraStatus CameraStatus
        {
            get => _CameraStatus;
            set
            {
                if (_CameraStatus != value)
                {
                    _CameraStatus = value;
                    CameraStatusChanged?.Invoke(_CameraStatus); // Gọi event
                }
            }
        }
        private static Step _Step = Step.Run;
        public static event Action<Step> StepModeChanged;
        public static Step Step
        {
            get => _Step;
            set
            {
                if (_Step != value)
                {
                    _Step = value;
                    StepModeChanged?.Invoke(_Step); // Gọi event
                }
            }
        }
        public static bool IsSetPara = false;
        public static bool IsChange = false;
        public static bool IsAllowReadPLC = true;
        public static TriggerNum TriggerNum = TriggerNum.Trigger0;
        public static dynamic ScanCCD;
        public static bool IsDebug = false;
        public static Bitmap[] bitmaps = new Bitmap[4] { null, null, null, null };
        public static List<HistoryCheck> HistoryChecks = new List<HistoryCheck>();
        public static int WidthOldTools = 0;
         public static dynamic EditTool ;
        public static bool IsSendRS = false;
        public static bool TotalOK = false;
        public static bool TotalOK1 = false;
        public static bool TotalOK2 = false;
        public static bool TotalOK3 = false;
        public static bool TotalOK4 = false;
        public static bool IsByPassResult = false;
        public static int NumSend = 0;
        public static String Ex = "";
       //Gui
       public static bool _IsLive = false;
        public static event Action<bool> LiveChanged;
        public static bool IsLive
        {
            get => _IsLive;
            set
            {
                if (_IsLive != value)
                {
                    _IsLive = value;
                    LiveChanged?.Invoke(_IsLive); // Gọi event
                }
            }
        }
        //private static String _Ex = "";
        //public static event Action<String> ExChanged;
        //public static String Ex
        //{
        //    get => _Ex;
        //    set
        //    {
        //        if (_Ex != value)
        //        {
        //            _Ex = value;
        //            ExChanged?.Invoke(_Ex); // Gọi event
        //        }
        //    }
        //}
        public static LogsDashboard LogsDashboard;
        public static List<ItemNew> itemNews = new List<ItemNew>();
        public static bool IsLoadProgFist = false;
        public static bool IsHideTool = true;
        public static System.Drawing.Point pShowTool=new Point(10,10);
        public static float ScaleZoom;
        public static Point pScroll;
        public static dynamic ToolSettings;
        public static Font fontTool = new Font("Arial", 22, FontStyle.Regular);
        public static Font fontRS = new Font("Arial", 32, FontStyle.Bold);
        public static StatusDraw _StatusDraw = StatusDraw.None;
        public static event Action<StatusDraw> StatusDrawChanged;
        public static Config Config;
        public static float ZoomMinimum = 0;
       public static  bool IsLearning=true;
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
        public static StatusIO _StatusIO = StatusIO.None;
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
        public static Size SizeScreen;
        public static float PerScaleWidth, PerScaleHeight;
        public static bool Initialed = false;
       
        public static float Scale = 1, AngleOrigin;
        public static  Color ColorOK = Color.FromArgb(0, 172, 73);
        public static Color ColorNG = Color.DarkRed;
        public static Color ColorRead = Color.SkyBlue;
        public static Color ColorProssing = Color.Blue;
        public static Color ColorTrigger = Color.LightBlue;
        public static Color ColorNone = Color.Gray;
        public static TypeCrop _TypeCrop = TypeCrop.Crop;
        public static event Action<TypeCrop> TypeCropChanged;
        public static bool IsIntialPython = false;
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
        public static int RadpEdit = 40;
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
