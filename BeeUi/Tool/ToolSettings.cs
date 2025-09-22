using BeeCore;
using BeeCore.Funtion;
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
          
        
            Global.ToolSettings = this;
        }
      public  Label[] Labels = new Label[4] { new Label(), new Label(), new Label(), new Label() };
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
                    Shows.ShowChart(Global.ToolSettings.pAllTool, BeeCore.Common.PropetyTools[Global.IndexChoose]);
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
           // propety.Propety.Index = Index;
            PropetyTool propetyTools = BeeCore.Common.PropetyTools[Global.IndexChoose][Index];
            propetyTools.ItemTool = DataTool.CreateItemTool(propety, Index, Global.IndexChoose,Global.pShowTool);
            propetyTools.Control = DataTool.CreateControls(propety, Index, Global.IndexChoose, Global.pShowTool);

           
            DataTool.LoadPropety(propetyTools.Control);
            propetyTools.UsedTool = UsedTool.Used;
            Shows.ShowChart(Global.ToolSettings.pAllTool, BeeCore.Common.PropetyTools[Global.IndexChoose]);


        }

        private void ToolSettings_Load(object sender, EventArgs e)
        {
            CustomGui.RoundRg(pBtn, 20, Corner.Both);
          
            //this.pBtn.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
         //   _layout.EnableAuto(); // tự load sau Shown, tự save khi Closing
        }
     
        private void ToolSettings_SizeChanged(object sender, EventArgs e)
        {
           
        }

        private void itemTool1_Load(object sender, EventArgs e)
        {

        }

        private void pBtn_SizeChanged(object sender, EventArgs e)
        {
            CustomGui.RoundRg(pBtn, 20, Corner.Both);
            //BeeCore.CustomGui.RoundRg(this.pBtn, Global.Config.RoundRad);
        }

        private void btnRename_Click(object sender, EventArgs e)
        {   if(!btnRename.IsCLick)
            {
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].ItemTool.VisbleEditname();
            }
            else
            {
                if (Global.IndexToolSelected == -1)
                {
                    btnRename.IsCLick = false;
                    return;
                }
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].ItemTool.EditName();
               
            }
         
        }
    }

}
