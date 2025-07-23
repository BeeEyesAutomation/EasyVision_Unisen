using BeeCore;
using BeeGlobal;
using BeeUi.Commons;
using BeeUi.Unit;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Tool
{
    [Serializable()]
    public partial class SettingStep4 : UserControl
    {
        public SettingStep4()
        {
            InitializeComponent();
        }
      
       
        private void btnNextStep_Click(object sender, EventArgs e)
        {

            G.Header.btnMode.PerformClick();
            G.EditProg.btnSave.PerformClick();
        }

       
    
        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.Header.btnMode.PerformClick();
           Global.Config.ConditionOK = ConditionOK.TotalOK;
           SaveData.Config(Global.Config);
        }
        public void RefreshLogic()
        {
            int y = 10;
            int index = 0;
            switch (Global.Config.ConditionOK)
            {
                case ConditionOK.TotalOK:

                    btnTotalOK.PerformClick();
                    break;
                case ConditionOK.AnyOK:
                    btnAnyOK.PerformClick();
                    break;
                case ConditionOK.Logic:
                    btnLogin.PerformClick();
                    break;
            }
            //switch (Global.Config.LogicOK)
            //{
            //    case LogicOK.AND:

            //        btnAnd.PerformClick();
            //        break;
            //    case LogicOK.OR:
            //        btnOr.PerformClick();
            //        break;

            //}
            pLogic1.Controls.Clear();
            foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
            {
                ItemLogic itemLogic = new ItemLogic();
                itemLogic.Parent = pLogic1;
                itemLogic.Location = new System.Drawing.Point(10, y);
                itemLogic.name.Text = propetyTool.Name;
                itemLogic.propetyTool = propetyTool;
                index++;
                if (index < 10)
                    itemLogic.lbNumber.Text = "0" + index;
                else
                    itemLogic.lbNumber.Text = "" + index;
                switch (propetyTool.UsedTool)
                {
                    case UsedTool.NotUsed:
                        itemLogic.ckUnused.Checked = true;
                        break;
                    case UsedTool.Used:
                        itemLogic.ckUsed.Checked = true;
                        break;
                    case UsedTool.Invertse:
                        itemLogic.ckInverse.Checked = true;
                        break;
                }

                y += itemLogic.Height + 20;
            }
        }
        private void SettingStep4_Load(object sender, EventArgs e)
        {
            RefreshLogic();
            //numToolOK.Maximum = BeeCore.Common.PropetyTools.Count;
            //  if (Global.ParaCommon.numToolOK > numToolOK.Maximum) Global.ParaCommon.numToolOK =(int) numToolOK.Maximum;
            //  numToolOK.Value = Global.ParaCommon.numToolOK;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           Global.Config.ConditionOK = ConditionOK.Logic;
            //pLogic.Enabled = true; 
            //pAnyTool.Enabled = false;
        }

      
        private void btnTotalOK_Click(object sender, EventArgs e)
        {
           Global.Config.ConditionOK = ConditionOK.TotalOK;
            //pLogic.Enabled = false; pAnyTool.Enabled = false;
        }

        private void btnAnyOK_Click(object sender, EventArgs e)
        {
           Global.Config.ConditionOK = ConditionOK.AnyOK;
            //pLogic.Enabled = false;
            //pAnyTool.Enabled = true;
        }

        private void grLogic_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numToolOK_ValueChanged(object sender, EventArgs e)
        {
         //   Global.ParaCommon.numToolOK =(int) numToolOK.Value;

        }

        private void btnAnd_Click(object sender, EventArgs e)
        {
           Global.Config.LogicOK = LogicOK.AND;
        }

        private void btnOr_Click(object sender, EventArgs e)
        {
           Global.Config.LogicOK = LogicOK.OR;
        }
    }
}
