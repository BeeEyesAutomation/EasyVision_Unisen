using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    public partial class Cameras : UserControl
    {
        public Cameras()
        {
            InitializeComponent();
        }
      
        private  void btnHide_Click(object sender, EventArgs e)
        {
            Global.WidthOldTools = Global.EditTool.pEdit.Width;
            Global.EditTool.pEdit.Width = 0;
            Global.EditTool.hideBar.btnShowToolBar.IsCLick = true;
        
        }
        int percent = 0;
   
  

 
        ScanCCD scanCCD = new ScanCCD();
        private void btnCamera1_Click(object sender, EventArgs e)
        {
            
            //Global.IndexChoose = 0;
            //if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
            //{
            //    btnCamera1.IsCLick = false;
            //    return;
            //}
            Global.EditTool.pEditTool.Show("Tool");
        
            ShowTool.ShowAllChart(Global.ToolSettings.pAllTool);

        }

     

        private void btnHistory_Click(object sender, EventArgs e)
        { if(Global.EditTool. DashboardImages==null)
            {
                Global.EditTool.DashboardImages = new DashboardImages();

                //  DashboardImages.
            }
            
            Global.EditTool.pEditTool.Show("Images");
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if (Global.LogsDashboard == null)
            {
                Global.LogsDashboard = new LogsDashboard();
                Global.LogsDashboard.MaxLogCount = 5000;
                Global.LogsDashboard.ProgressiveBatchSize = 200;
                Global.LogsDashboard.ProgressiveIntervalMs = 10;
                Global.LogsDashboard.IngestBatchSize = 100;
                Global.LogsDashboard.IngestIntervalMs = 16; // ~60Hz
            }
            Global.EditTool.pEditTool.Show("Logs");
        }

        private void btnSettingPLC_Click(object sender, EventArgs e)
        {
               Global.EditTool.pEditTool.Show("PLC");
          
        }
    }
}
