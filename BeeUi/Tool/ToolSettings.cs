﻿using BeeCore;
using BeeGlobal;
using BeeInterface;
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
            foreach (Tools c in G.listAlltool[Global.IndexChoose])
            {
                c.ItemTool.Location = new Point(G.ToolSettings.X, G.ToolSettings.Y);
                BeeCore.Common.PropetyTools[Global.IndexChoose][i].Propety.Index = i;
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
                if (Global.IndexToolSelected>-1)
                {
                    G.ToolSettings.pAllTool.Controls.RemoveAt(Global.IndexToolSelected);
                    BeeCore.Common.PropetyTools[Global.IndexChoose].RemoveAt(Global.IndexToolSelected);
                    G.listAlltool[Global.IndexChoose].RemoveAt(Global.IndexToolSelected);
                    Global.IndexToolSelected = G.listAlltool[Global.IndexChoose].Count() - 1;
                    RefreshTool();
                }    
            }    
        }

        private void btnEnEdit_Click(object sender, EventArgs e)
        {
            if(btnEnEdit.IsCLick)
            {
            foreach(Tools tool in G.listAlltool[Global.IndexChoose])
                {
                    tool.ItemTool.Score.Enabled = true;
                }    
            } 
            else
            {
                foreach (Tools tool in G.listAlltool[Global.IndexChoose])
                {
                    tool.ItemTool.Score.Enabled = false;
                }
            }    
        }
     
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (Global.IndexToolSelected == -1) return;
       
          PropetyTool propety = (PropetyTool)BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Clone();
       
            propety.Name = propety.TypeTool.ToString() + " " + (int)(BeeCore.Common.PropetyTools[Global.IndexChoose].Count + 1);
            BeeCore.Common.PropetyTools[Global.IndexChoose].Add(propety);
            Tools tool = DataTool.CreateControl(propety, G.listAlltool[Global.IndexChoose].Count(), Global.IndexChoose, new Point(G.ToolSettings.X, G.ToolSettings.Y));
            G.listAlltool[Global.IndexChoose].Add(tool);
            G.ToolSettings.Y += tool.ItemTool.Height + 10;
            DataTool.LoadPropety(G.listAlltool[Global.IndexChoose][G.listAlltool[Global.IndexChoose].Count()-1].tool);
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
