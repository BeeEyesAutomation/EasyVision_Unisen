using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using BeeInterface.General;

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

namespace BeeInterface
{
    public partial class ToolSettings : UserControl
    {
        
        public ToolSettings()
        {
            InitializeComponent();
            //_layout = new LayoutPersistence(this, key: "ToolLayout");
          
        
            Global.ToolSettings = this;
        }
      public  InforGroup[] Labels = new InforGroup[4] { new InforGroup(), new InforGroup(), new InforGroup(), new InforGroup() };
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTool AddTool = new AddTool();
            AddTool.ShowDialog();
            AddTool.Close();
        }
      

        private void btnDelect_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Xóa","B?n ch?c ch?",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                if (Global.IndexToolSelected>-1)
                {
                    Global.ToolSettings.pAllTool.Controls.RemoveAt(Global.IndexToolSelected);
                    BeeCore.Common.EnsureToolList(Global.IndexProgChoose).RemoveAt(Global.IndexToolSelected);
                   // G.listAlltool[Global.IndexProgChoose].RemoveAt(Global.IndexToolSelected);
                    Global.IndexToolSelected = BeeCore.Common.EnsureToolList(Global.IndexProgChoose).Count() - 1;
                    int index = 0;
                    foreach (PropetyTool propetyTool in BeeCore.Common.EnsureToolList(Global.IndexProgChoose))
                    {
                       
                        propetyTool.Propety2.Index = index;
                        propetyTool.ItemTool.IndexTool = index;
                        if(propetyTool.ItemTool2!=null)
                            propetyTool.ItemTool2.IndexTool = index;
                        if (propetyTool.ItemTool3 != null)
                            propetyTool.ItemTool3.IndexTool = index;
                        if (propetyTool.ItemTool4!= null)
                            propetyTool.ItemTool4.IndexTool = index;
                        index++;
                       

                    }
                   
                  BeeInterface. Load.ArrangeLogic();
                    ShowTool.ShowChart(Global.ToolSettings.pAllTool, BeeCore.Common.EnsureToolList(Global.IndexProgChoose));
                }    
            }    
        }

        private void btnEnEdit_Click(object sender, EventArgs e)
        {
            if (btnEnEdit.IsCLick)
            {
                foreach (List<PropetyTool> List in BeeCore.Common.PropetyTools)
                {
                    if (List != null)
                        foreach (PropetyTool propetyTool in List)
                        {
                            if (propetyTool.ItemTool == null) continue;
                            propetyTool.ItemTool.IsEdit = true;
                        }
                    if (!Global.Config.IsMultiProg)
                        break;
                }
            }
            else
            {
                foreach (List<PropetyTool> List in BeeCore.Common.PropetyTools)
                {
                    if (List != null)
                        foreach (PropetyTool propetyTool in List)
                        {
                            if (propetyTool.ItemTool == null) continue;
                            propetyTool.ItemTool.IsEdit = false;
                        }
                    if (!Global.Config.IsMultiProg)
                        break;
                }
            }
        }
     
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (Global.IndexToolSelected == -1) return;
       
          PropetyTool propety = (PropetyTool)BeeCore.Common.TryGetTool(Global.IndexProgChoose, Global.IndexToolSelected).Clone();
       
            propety.Name = propety.TypeTool.ToString() + " " + (int)(BeeCore.Common.EnsureToolList(Global.IndexProgChoose).Count + 1);
            BeeCore.Common.EnsureToolList(Global.IndexProgChoose).Add(propety);
           
            int Index = BeeCore.Common.EnsureToolList(Global.IndexProgChoose).Count - 1;
         
            PropetyTool propetyTools = BeeCore.Common.TryGetTool(Global.IndexProgChoose, Index);
            propetyTools.ItemTool = DataTool.CreateItemTool(propety, Index, Global.IndexProgChoose);
            propetyTools.Control = DataTool.CreateControls(propety, Index, Global.IndexProgChoose);

           
            DataTool.LoadPropety(propetyTools.Control);
            propetyTools.UsedTool = UsedTool.Used;
          
            propetyTools.Propety2.SetModel(true);
            BeeInterface.Load.ArrangeLogic();
            ShowTool.ShowChart(Global.ToolSettings.pAllTool, BeeCore.Common.EnsureToolList(Global.IndexProgChoose));


        }

        private void ToolSettings_Load(object sender, EventArgs e)
        {
            CustomGui.RoundRg(pBtn, 20, Corner.Both);
          
            //this.pBtn.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
         //   _layout.EnableAuto(); // t? load sau Shown, t? save khi Closing
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
                BeeCore.Common.TryGetTool(Global.IndexProgChoose, Global.IndexToolSelected).ItemTool.VisbleEditname();
            }
            else
            {
                if (Global.IndexToolSelected == -1)
                {
                    btnRename.IsCLick = false;
                    return;
                }
                BeeCore.Common.TryGetTool(Global.IndexProgChoose, Global.IndexToolSelected).ItemTool.EditName();
               
            }
         
        }
    }

}
