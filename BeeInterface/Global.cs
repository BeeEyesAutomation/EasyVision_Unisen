
using BeeUi.Tool;
using BeeUi.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeInterface
{
    public struct G
    {
        public static ProtocolPLC SettingPLC;
        public static Header Header;
        public static EditProg EditProg;
        public static bool Initial = false, IsShutDown = false;
        public static bool IsReConnectCCD = false, IsPLCNotAlive = false;
        public static System.Windows.Forms.ListBox listProgram;

        public static StatusDashboard StatusDashboard;
   
        public static ucReport ucReport = new ucReport();

        // public 
    }
    class Globals
    {

    }
}
