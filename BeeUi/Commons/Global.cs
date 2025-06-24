using BeeCore;
using BeeUi.Common;
using BeeUi.Commons;
using BeeUi.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BeeUi.Unit;
using Image = System.Drawing.Image;

namespace BeeUi
{
    [Serializable()]
    public enum Trig
    {
       None, Processing,Trigged,Continue,NotTrig,Complete
    }
    public enum StatusProcessing
    {
        None, Adjusting,WaitingDone, Processing, Done
    }
    public enum StatusMode
    {
      None,  SimOne, SimContinuous, Once, Continuous
    }
    public enum Step
    {
        Step1, Step2, Step3, Step4, Run,PLC
    }
    public struct G
    {
        public static Font fontTool = new Font("Arial", 22, FontStyle.Regular);
        public static Font fontRS = new Font("Arial", 32, FontStyle.Bold);
        public static StatusMode StatusMode=StatusMode.None;
        public static StatusProcessing StatusProcessing = StatusProcessing.None;
        public static bool Initial = false,IsShutDown=false;
        public static bool IsReConnectCCD=false,IsPLCNotAlive=false;
        public static int[] valuesPLCInPut=new int[16];
        public static int[] valuesPLCOutPut;
        public static SettingPLC SettingPLC = new SettingPLC();
       public static PLC PLC = new PLC();
        public static Step Step = Step.Run;
        
        public static System.Windows.Forms.ListBox listProgram = new System.Windows.Forms.ListBox ();
       public static float Scale=1,AngleOrigin;
        public static OpenCvSharp.Point pOrigin = new OpenCvSharp.Point ();
      public static System.Drawing.Point pScale=new System.Drawing.Point(0,0);
        // public static BeeDevice.DeviceConnectForm DeviceConnectForm = new BeeDevice.DeviceConnectForm();
        public static String Licence = "";
        public static String Project = "";
        public static ScanCCD ScanCCD=new ScanCCD();
      public static bool IsModeTest = false,IsSendRS=false;
        public static bool IsDone = false,IsIniOCR,IsIniPython;
        public static ucReport ucReport=new ucReport();
        public static bool TotalOK = false;

         public static String NamePort = "";
        public static Main Main;
      public static ForrmAlarm FormActive;
        public static KeyAcitve keys=new KeyAcitve();
        public static bool IsActive;
        public static FormLoad Load;
        public static bool IsCancel;
        public static dynamic PropetyOld;
        public static Tool.AddTool AddTool;
        public static Header Header;
        public static ResultBar ResultBar;
        public static EditProg EditProg;
        public static Trig StatusTrig=Trig.None;
        public static int indexToolSelected;
          public static Config Config;
         public static VideoCapture camera;
       public static bool IsRun = true,IsCCD,IsEdit;
        public static RectRotate rotOriginAdj;
        public static bool IsCheck=false;
        public static StepEdit StepEdit=new StepEdit();
        public static float angle_Adjustment=0, X_Adjustment=0, Y_Adjustment=0;
        public static List<PropetyTool> PropetyTools = new List<PropetyTool>();
        public static List<Tools> listAlltool = new List<Tools>();
        public static List<iTool> listItool = new List<iTool>();
        public static bool IsCalib, isTop;
        public static string _pathSqlMaster;
        public static SqlConnection cnn=new SqlConnection();
        public static ToolSettings ToolSettings=new ToolSettings();
        public static bool IsLoad= false;
     public static TypeCrop TypeCrop = TypeCrop.Crop;
       public static TypeTool TypeTool = TypeTool.OutLine;
       public static Tools tool ;
        public static EditTool EditTool ;
        public static bool IsDrawProcess = true;
        public static bool IsByPassPLC = true;
        public static Color clTrack = (Color)new ColorConverter().ConvertFromString("#444444");
        public static Account account = new Account();
        public static InforBar InforBar = new InforBar();
        public static List<Bitmap>listImgTrainYolo=new List<Bitmap>();
        public static List<String> listLabelTrainYolo = new List<String>();
        public static List<HistoryCheck> listHis= new List<HistoryCheck>();
    }
    internal class Global
    {
    }
}
