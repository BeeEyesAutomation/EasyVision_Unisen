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
using BeeGlobal;
using BeeInterface;
using System.Windows.Forms;

namespace BeeUi
{
    [Serializable()]
  
    public struct G
    {
        
        public static bool Initial = false,IsShutDown=false;
        public static bool IsReConnectCCD=false,IsPLCNotAlive=false;
        public static int[] valuesPLCInPut=new int[16];
        public static int[] valuesPLCOutPut;
        public static ProtocolPLC SettingPLC;

        public static StatusProcessing StatusProcessing = StatusProcessing.None;

         public static System.Windows.Forms.ListBox listProgram ;
       public static float Scale=1;
      
      public static System.Drawing.Point pScale=new System.Drawing.Point(0,0);
        // public static BeeDevice.DeviceConnectForm DeviceConnectForm = new BeeDevice.DeviceConnectForm();
        public static String Licence = "";
       
      
      public static bool IsModeTest = false;
        public static bool IsDone = false,IsIniOCR,IsIniPython;
        public static ucReport ucReport=new ucReport();
        

         public static String NamePort = "";
        public static Main Main;
      public static ForrmAlarm FormActive;
        public static KeyAcitve keys=new KeyAcitve();
        public static bool IsActive;
        public static FormLoad Load;
        public static bool IsCancel;
       
        public static Tool.AddTool AddTool;
        public static Header Header;
        public static StatusDashboard StatusDashboard;
        public static EditProg EditProg;
      
       
         
        public static VideoCapture camera;
        public static bool IsCCD;
        public static RectRotate rotOriginAdj;
      
       
        public static float angle_Adjustment=0, X_Adjustment=0, Y_Adjustment=0;



       
        public static bool IsCalib, isTop;
        public static string _pathSqlMaster;
        public static SqlConnection cnn=new SqlConnection();
        

        // public static TypeTool TypeTool = TypeTool.OutLine;
        public static Tools tool ;
        
        public static bool IsDrawProcess = true;

        public static Color clTrack = (Color)new ColorConverter().ConvertFromString("#444444");
        public static Account account = new Account();
        public static InforBar InforBar = new InforBar();
        public static List<Bitmap>listImgTrainYolo=new List<Bitmap>();
        public static List<String> listLabelTrainYolo = new List<String>();
 
    }
    internal class Globals
    {
    }
}
