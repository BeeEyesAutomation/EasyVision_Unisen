using BeeCore;
using BeeGlobal;
using BeeInterface;
using BeeUi.Commons;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using UserControl = System.Windows.Forms.UserControl;

namespace BeeUi.Tool
{
    public partial class ToolSettings : UserControl
    {
        
        public ToolSettings()
        {
            InitializeComponent();
            //_layout = new LayoutPersistence(this, key: "ToolLayout");
          
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            Global.ToolSettings = this;
        }
      
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (G.AddTool == null)
                G.AddTool = new AddTool();
            else
            {
                G.AddTool.Dispose();
                G.AddTool = new AddTool();
            }    
              
            G.AddTool.Show();
        }
        public void RefreshTool()
        {
            Global.ToolSettings.pAllTool.Controls.Clear();
            Global.pShowTool. Y = 10;
            Global.pShowTool.X = 10;
            int i = 0;
            foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
            {
                PropetyTool.ItemTool.Location = new Point( Global.pShowTool.X,  Global.pShowTool.Y);
                PropetyTool.Propety.Index = i;
                 Global.pShowTool.Y += PropetyTool.ItemTool.Height + 10;
                Global.ToolSettings.pAllTool.Controls.Add(PropetyTool.ItemTool);
                Global.ToolSettings.ResumeLayout(true);
                i++;
            }
        }

        private void btnDelect_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Xóa","Bạn chắc chứ",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                if (Global.IndexToolSelected>-1)
                {
                    Global.ToolSettings.pAllTool.Controls.RemoveAt(Global.IndexToolSelected);
                    BeeCore.Common.PropetyTools[Global.IndexChoose].RemoveAt(Global.IndexToolSelected);
                   // G.listAlltool[Global.IndexChoose].RemoveAt(Global.IndexToolSelected);
                    Global.IndexToolSelected = BeeCore.Common.PropetyTools[Global.IndexChoose].Count() - 1;
                    RefreshTool();
                }    
            }    
        }

        private void btnEnEdit_Click(object sender, EventArgs e)
        {
            if (btnEnEdit.IsCLick)
            {
                foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
                {
                    propetyTool.ItemTool.IsEdit = true;
                }
            }
            else
            {
                foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
                {
                    propetyTool.ItemTool.IsEdit = false;
                }
            }
        }
     
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (Global.IndexToolSelected == -1) return;
       
          PropetyTool propety = (PropetyTool)BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Clone();
       
            propety.Name = propety.TypeTool.ToString() + " " + (int)(BeeCore.Common.PropetyTools[Global.IndexChoose].Count + 1);
            BeeCore.Common.PropetyTools[Global.IndexChoose].Add(propety);
            int Index = BeeCore.Common.PropetyTools[Global.IndexChoose].Count - 1;
            PropetyTool propetyTools = BeeCore.Common.PropetyTools[Global.IndexChoose][Index];
            propetyTools.ItemTool = DataTool.CreateItemTool(propety, Index, Global.IndexChoose,Global.pShowTool);
            propetyTools.Control = DataTool.CreateControls(propety, Index, Global.IndexChoose, Global.pShowTool);
            //Tools tool = DataTool.CreateControl(propety, G.listAlltool[Global.IndexChoose].Count(), Global.IndexChoose, new Point( Global.pShowTool.X,  Global.pShowTool.Y));
            //G.listAlltool[Global.IndexChoose].Add(tool);
            propetyTools.ItemTool.Width = Global.ToolSettings.Width - 10;
            Global.pShowTool.Y += propetyTools.ItemTool.Height + 10;
            DataTool.LoadPropety(propetyTools.Control);
            RefreshTool();



        }

        private void ToolSettings_Load(object sender, EventArgs e)
        {
          
            this.pBtn.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
         //   _layout.EnableAuto(); // tự load sau Shown, tự save khi Closing
        }

        private void ToolSettings_SizeChanged(object sender, EventArgs e)
        {
           // if(G.Header!=null)
            // BeeCore.CustomGui.RoundRg(this.pBtn,Global.Config.RoundRad);

        }

        private void itemTool1_Load(object sender, EventArgs e)
        {

        }

        private void pBtn_SizeChanged(object sender, EventArgs e)
        {
            //BeeCore.CustomGui.RoundRg(this.pBtn, Global.Config.RoundRad);
        }
    }

}
