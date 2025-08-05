using BeeCore;
using BeeGlobal;

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

namespace BeeInterface
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
           
          
            SaveData.Project(Global.Project);
            Global.EditTool.RefreshGuiEdit(Step.Run);
        }

       
    
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Global.EditTool.RefreshGuiEdit(Step.Step3);
        }
        public void RefreshLogic()
        {
            int y = 10;
            int index = 0;
            //switch (Global.Config.ConditionOK)
            //{
            //    case ConditionOK.TotalOK:

            //        btnTotalOK.PerformClick();
            //        break;
            //    case ConditionOK.AnyOK:
            //        btnAnyOK.PerformClick();
            //        break;
            //    case ConditionOK.Logic:
            //        btnLogin.PerformClick();
            //        break;
            //}
            //switch (Global.Config.LogicOK)
            //{
            //    case LogicOK.AND:

            //        btnAnd.PerformClick();
            //        break;
            //    case LogicOK.OR:
            //        btnOr.PerformClick();
            //        break;

            //}
            pItemsRs.Controls.Clear();
            foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
            {
                ItemRS itemRs = new ItemRS();
                ItemLogic itemLogic = new ItemLogic();
                itemRs.Parent = pItemsRs;
                itemRs.Location = new System.Drawing.Point(5, y);
                itemRs.name.Text = propetyTool.Name;
                itemRs.propetyTool = propetyTool;
                itemRs.Width = 400;
                itemLogic.Parent = pItemsLogis;
                itemLogic.Location = new System.Drawing.Point(5, y);
                itemLogic.name.Text = propetyTool.Name;
                itemLogic.propetyTool = propetyTool;
                itemLogic.Width = 400;
                index++;
                if (index < 10)
                    itemRs.lbNumber.Text = "0" + index;
                else
                    itemRs.lbNumber.Text = "" + index;
                if (index < 10)
                    itemLogic.lbNumber.Text = "0" + index;
                else
                    itemLogic.lbNumber.Text = "" + index;
                switch (propetyTool.UsedTool)
                {
                    case UsedTool.NotUsed:
                        itemRs.ckUnused.Checked = true;
                        break;
                    case UsedTool.Used:
                        itemRs.ckUsed.Checked = true;
                        break;
                    case UsedTool.Invertse:
                        itemRs.ckInverse.Checked = true;
                        break;
                }
                if(propetyTool.IndexLogics==null)
                {
                    propetyTool.IndexLogics = new bool[6];
                    
                }
                itemLogic.ck1.Checked = propetyTool.IndexLogics[0];
                itemLogic.ck2.Checked = propetyTool.IndexLogics[1];
                itemLogic.ck3.Checked = propetyTool.IndexLogics[2];
                itemLogic.ck4.Checked = propetyTool.IndexLogics[3];
                itemLogic.ck5.Checked = propetyTool.IndexLogics[4];
                itemLogic.ck6.Checked = propetyTool.IndexLogics[5];
                y += itemRs.Height + 20;
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
