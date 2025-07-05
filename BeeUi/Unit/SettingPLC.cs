using BeeCore;
using BeeUi.Common;
using BeeUi.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    public partial class SettingPLC : UserControl
    {
        public SettingPLC()
        {
            InitializeComponent();
        }
        public PLC PLC = new PLC();
        public void RefreshValuePLC()
        {if (IsPress) return;
            if (!this.Visible) return;
            if (G.PLC.valueInput.Count() < 16) return;
            foreach (Control c1 in G.SettingPLC.LayIntput.Controls)
            {
                foreach (Control c in c1.Controls)
                { 
                    if ((c is RJButton))
                    {
                        RJButton btn = c as RJButton;
                        int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

                        btn.IsCLick = Convert.ToBoolean(G.PLC.valueInput[numAdd]);
                    }
               }
            }
            if (G.PLC.valueOutput == null) return;
            if (G.PLC.valueOutput.Count() < 16) return;
            foreach (Control c1 in G.SettingPLC.LayOutput.Controls)
            {
                foreach (Control c in c1.Controls)
                {
                    if ((c is RJButton))
                    {
                        RJButton btn = c as RJButton;
                        int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

                        btn.IsCLick = Convert.ToBoolean(G.PLC.valueOutput[numAdd]);
                    }
                }
            }
            //In1.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[0]);
            //In2.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[1]);
            //In3.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[2]);
            //In4.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[3]);
            //In5.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[4]);
            //In6.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[5]);
            //In7.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[6]);
            //In8.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[7]);
            //In9.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[8]);
            //In10.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[9]);
            //In11.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[10]);
            //In12.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[11]);
            //In13.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[12]);
            //In14.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[13]);
            //In15.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[14]);
            //In16.IsCLick = Convert.ToBoolean(G.valuesPLCInPut[15]);

        }
        private void tmShow_Tick(object sender, EventArgs e)
        {
             }

        bool IsPress = false;
        bool IsShow = false;
        private async void BtnWriteInPLC(RJButton btn)
        {
            IsPress = true;
            btn.Font = new Font("Arial", 14, FontStyle.Bold);
        X: G.Header.tmReadPLC.Enabled = false;
            if (G.Header.workPLC.IsBusy)
            {
                await Task.Delay(5);
                goto X;
            }
int numAdd =Convert.ToInt32( btn.Name.Substring(2).Trim())-1;

            await Task.Run(() => G.PLC.WriteInPut(numAdd, btn.IsCLick));
            
            G.Header.tmReadPLC.Enabled = true;
            IsPress = false;
            btn.Font = new Font("Arial", 12, FontStyle.Regular);
        }
        private async void BtnWriteOutPLC(RJButton btn)
        {
            IsPress = true;
            btn.Font = new Font("Arial", 14, FontStyle.Bold);
        X: G.Header.tmReadPLC.Enabled = false;
            if (G.Header.workPLC.IsBusy)
            {
                await Task.Delay(5);
                goto X;
            }
            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;
            G.PLC.SetOutPut(numAdd, btn.IsCLick);
            await Task.Run(() => G.PLC.WriteOutPut());
           
            G.Header.tmReadPLC.Enabled = true;
            IsPress = false;
            btn.Font = new Font("Arial", 12, FontStyle.Regular);
        }

        private void In_Click(object sender, EventArgs e)
        {
            BtnWriteInPLC((RJButton)sender);
        }

        private void Ou_Click(object sender, EventArgs e)
        {
            BtnWriteOutPLC((RJButton)sender);
        }

        private void cbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
        
          BeeCore.Common.Comunication.Com= cbSerialPort.Text;
        }

        private void SettingPLC_Load(object sender, EventArgs e)
        {
            G.SettingPLC.cbSerialPort.DataSource = SerialPort.GetPortNames();

            cbSerialPort.Text = G.Config.IDPort;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
           G.PLC.Connect(G.Config.IDPort);
            if (G.PLC.IsConnected)
            {
                if (File.Exists("Default.config"))
                    File.Delete("Default.config");
                Access.SaveConfig("Default.config", G.Config);
            }
        }

        private void SettingPLC_VisibleChanged(object sender, EventArgs e)
        {
           
                this.LayIntput.Enabled = G.PLC.IsConnected;
           this.LayOutput.Enabled = G.PLC.IsConnected;

        }

        private void btnModeEhternetIP_Click(object sender, EventArgs e)
        {
            BeeCore.Common.Comunication.TypeComunication = TypeComunication.EthernetIP;
        }

        private void btnModeMobusTCP_Click(object sender, EventArgs e)
        {
            BeeCore.Common.Comunication.TypeComunication = TypeComunication.ModbusTCP;
        }

        private void btnModeRS485_Click(object sender, EventArgs e)
        {
            BeeCore.Common.Comunication.TypeComunication = TypeComunication.MobusRS485;
        }

        private void comBaurate_SelectedIndexChanged(object sender, EventArgs e)
        {
            BeeCore.Common.Comunication.Baurate =Convert.ToInt32( comBaurate.Text);
        }

        private void numIDSlave_ValueChanged(object sender, EventArgs e)
        {
            BeeCore.Common.Comunication.SlaveID =(byte) numIDSlave.Value;
        }
    }
}
