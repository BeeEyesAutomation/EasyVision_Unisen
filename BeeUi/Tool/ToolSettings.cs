using BeeCore;
using BeeUi.Commons;
using BeeUi.Data;
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

            G.ToolSettings = this;
        }
        public int X = 10, Y = 10;
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
            G.ToolSettings.pAllTool.Controls.Clear();
            Y = 10;
            X = 10;
            int i = 0;
            foreach (Tools c in G.listAlltool[G.indexChoose])
            {
                c.ItemTool.Location = new Point(G.ToolSettings.X, G.ToolSettings.Y);
                G.PropetyTools[G.indexChoose][i].Propety.Index = i;
                G.ToolSettings.Y += c.ItemTool.Height + 10;
                G.ToolSettings.pAllTool.Controls.Add(c.ItemTool);
                G.ToolSettings.ResumeLayout(true);
                i++;
            }
        }

        private void btnDelect_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Xóa","Bạn chắc chứ",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                if (G.indexToolSelected>-1)
                {
                    G.ToolSettings.pAllTool.Controls.RemoveAt(G.indexToolSelected);
                    G.PropetyTools[G.indexChoose].RemoveAt(G.indexToolSelected);
                    G.listAlltool[G.indexChoose].RemoveAt(G.indexToolSelected);
                    G.indexToolSelected = G.listAlltool[G.indexChoose].Count() - 1;
                    RefreshTool();
                }    
            }    
        }

        private void btnEnEdit_Click(object sender, EventArgs e)
        {
            if(btnEnEdit.IsCLick)
            {
            foreach(Commons.Tools tool in G.listAlltool[G.indexChoose])
                {
                    tool.ItemTool.Score.Enabled = true;
                }    
            } 
            else
            {
                foreach (Commons.Tools tool in G.listAlltool[G.indexChoose])
                {
                    tool.ItemTool.Score.Enabled = false;
                }
            }    
        }
     
        private void btnCopy_Click(object sender, EventArgs e)
        {

       
          PropetyTool propety = (PropetyTool)G.PropetyTools[G.indexChoose][G.indexToolSelected].Clone();
       
            propety.Name = propety.TypeTool.ToString() + " " + (int)(G.PropetyTools[G.indexChoose].Count + 1);
            G.PropetyTools[G.indexChoose].Add(propety);
            G.listAlltool[G.indexChoose].Add( DataTool.SetPropety(propety, G.listAlltool[G.indexChoose].Count(), G.indexChoose));
            DataTool.LoadPropety(G.listAlltool[G.indexChoose][G.listAlltool[G.indexChoose].Count()-1].tool);
            RefreshTool();



        }

        private void ToolSettings_Load(object sender, EventArgs e)
        {
            this.pBtn.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

        }

        private void ToolSettings_SizeChanged(object sender, EventArgs e)
        {
            if(G.Header!=null)
             BeeCore.CustomGui.RoundRg(this.pBtn, G.Config.RoundRad);

        }

        private void itemTool1_Load(object sender, EventArgs e)
        {

        }
    }

}
