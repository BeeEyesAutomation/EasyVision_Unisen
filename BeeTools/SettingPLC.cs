using BeeCore;
using BeeGlobal;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BeeInterface
{
    public partial class SettingPLC : UserControl
    {
        public SettingPLC()
        {
            InitializeComponent();
            //cbIn1.DataSource = listIn[0];
            //cbIn2.DataSource = listIn[1];
            //cbIn3.DataSource = listIn[2];
            //cbIn4.DataSource = listIn[3];
            //cbIn5.DataSource = listIn[4];
            //cbIn6.DataSource = listIn[5];
            //cbIn7.DataSource = listIn[6];
            //cbIn8.DataSource = listIn[7];
        }
        List<List<string>> listIn = new List<List<string>>{
            new List<string>(), new List<string>(), new List<string>(), new List<string>(),
             new List<string>(), new List<string>(), new List<string>(), new List<string>() };
        List<List<string>> listOut = new List<List<string>>{
            new List<string>(), new List<string>(), new List<string>(), new List<string>(),
             new List<string>(), new List<string>(), new List<string>(), new List<string>() };
        
        // 3. Đổ tất cả enum chưa chọn vào ComboBox
        private void RefreshComboBoxIn(int index ,String text)
        {

            listIn[index] = new List<string>();
            List<String> list = new List<string>();
            foreach (I_O_Input io in Enum.GetValues(typeof(I_O_Input)))
            {
              //  if (Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a=>a.I_O_Input==io)==-1|| text.Contains(io.ToString()))
                    listIn[index].Add(io.ToString());
            }

        }
        private void RefreshComboBoxOut(int index, String text)
        {

            listOut[index] = new List<string>();
            List<String> list = new List<string>();
            foreach (I_O_Output io in Enum.GetValues(typeof(I_O_Output)))
            {
             //   if (Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == io) == -1 || text.Contains(io.ToString()))
                    listOut[index].Add(io.ToString());
            }

        }

        public void ChangeDatasource(int ix,String i_O_Input)
        {
            
            for(int i=0;i<7;i++)
            {
                if (ix == i) continue;

                switch (i)
                {
                    case 0:
                        if (i_O_Input.ToString() == cbIn1.Text)
                        {
                            cbIn1.SelectedIndex = 0;
                            return;
                        }

                        break;
                    case 1:
                        if (i_O_Input.ToString() == cbIn2.Text)
                        {
                            cbIn2.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 2:
                        if (i_O_Input.ToString() == cbIn3.Text)
                        {
                            cbIn3.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 3:
                        if (i_O_Input.ToString() == cbIn4.Text)
                        {
                            cbIn4.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 4:
                        if (i_O_Input.ToString() == cbIn5.Text)
                        {
                            cbIn5.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 5:
                        if (i_O_Input.ToString() == cbIn6.Text)
                        {
                            cbIn6.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 6:
                        if (i_O_Input.ToString() == cbIn7.Text)
                        {
                            cbIn7.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 7:
                        if (i_O_Input.ToString() == cbIn8.Text)
                        {
                            cbIn8.SelectedIndex = 0;
                            return;
                        }
                        break;

                }    
            
            }    
           
          
        }
        private void RefreshComboBoxOut(ComboBox cb)
        {

            cb.Items.Clear();
            foreach (I_O_Output io in Enum.GetValues(typeof(I_O_Output)))
            {
                if (Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == io) == -1)
                    cb.Items.Add(io);
            }
            // (tuỳ chọn) nếu không có item nào thì disable
            cb.Enabled = cb.Items.Count > 0;
        }
        public void ChangeDatasourceOut(int ix, String i_O_Output)
        {

            for (int i = 0; i < 7; i++)
            {
                if (ix == i) continue;

                switch (i)
                {
                    case 0:
                        if (i_O_Output.ToString() == cbIn1.Text)
                        {
                            cbO0.SelectedIndex = 0;
                            return;
                        }

                        break;
                    case 1:
                        if (i_O_Output.ToString() == cbIn2.Text)
                        {
                            cbO1.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 2:
                        if (i_O_Output.ToString() == cbIn3.Text)
                        {
                            cbO2.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 3:
                        if (i_O_Output.ToString() == cbIn4.Text)
                        {
                            cbO3.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 4:
                        if (i_O_Output.ToString() == cbIn5.Text)
                        {
                            cbO4.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 5:
                        if (i_O_Output.ToString() == cbIn6.Text)
                        {
                            cbO5.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 6:
                        if (i_O_Output.ToString() == cbIn7.Text)
                        {
                            cbO6.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case 7:
                        if (i_O_Output.ToString() == cbIn8.Text)
                        {
                            cbO7.SelectedIndex = 0;
                            return;
                        }
                        break;

                }

            }


        }
        public void RefreshValuePLC()
        {if (IsPress) return;
            if (!this.Visible) return;
            if (Global.ParaCommon.Comunication.IO.valueInput.Length < 16) return;
            foreach (Control c1 in LayIntput.Controls)
            {
                foreach (Control c in c1.Controls)
                { 
                    if ((c is RJButton))
                    {
                        RJButton btn = c as RJButton;
                        int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

                        btn.IsCLick = Convert.ToBoolean(Global.ParaCommon.Comunication.IO.valueInput[numAdd]);
                    }
               }
            }
            if (Global.ParaCommon.Comunication.IO.valueOutput == null) return;
            if (Global.ParaCommon.Comunication.IO.valueOutput.Length < 16) return;
            foreach (Control c1 in LayOutput.Controls)
            {
                foreach (Control c in c1.Controls)
                {
                    if ((c is RJButton))
                    {
                        RJButton btn = c as RJButton;
                        int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

                        btn.IsCLick = Convert.ToBoolean(Global.ParaCommon.Comunication.IO.valueOutput[numAdd]);
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
        int index = 0;
        private void tmShow_Tick(object sender, EventArgs e)
        {
           // index++;
           // if(Global.ParaCommon.Comunication.IO.valueInput!=null)
           //Global.ParaCommon.Comunication.IO.valueInput.ReplaceAll( new int[16] { index, index, index, index, index, index, index, index, index, index, index, index, index, index, index, index });

        }

        bool IsPress = false;
        bool IsShow = false;
        private async void BtnWriteInPLC(RJButton btn)
        {
            IsPress = true;
            btn.Font = new Font("Arial", 14, FontStyle.Bold);
        //X: G.Header.tmReadPLC.Enabled = false;
        //    if (G.Header.workPLC.IsBusy)
        //    {
        //        await Task.Delay(5);
        //        goto X;
        //    }
int numAdd =Convert.ToInt32( btn.Name.Substring(2).Trim())-1;

            await Task.Run(() => Global.ParaCommon.Comunication.IO.WriteInPut(numAdd, btn.IsCLick));
            
          //  G.Header.tmReadPLC.Enabled = true;
            IsPress = false;
            btn.Font = new Font("Arial", 12, FontStyle.Regular);
        }
        private async void BtnWriteOutPLC(RJButton btn)
        {
            IsPress = true;
            btn.Font = new Font("Arial", 14, FontStyle.Bold);
        //X: G.Header.tmReadPLC.Enabled = false;
        //    if (G.Header.workPLC.IsBusy)
        //    {
        //        await Task.Delay(5);
        //        goto X;
        //    }
            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;
            Global.ParaCommon.Comunication.IO.SetOutPut(numAdd, btn.IsCLick);
            await Task.Run(() => Global.ParaCommon.Comunication.IO.WriteOutPut());
           
          //  G.Header.tmReadPLC.Enabled = true;
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
           Global.Config.IDPort = cbSerialPort.Text;
            BeeCore.Common.Comunication.IO.Port= cbSerialPort.Text;
        }
      
        private void SettingPLC_Load(object sender, EventArgs e)
        {// 1) Khởi tạo và bind:
         
            comIO.DataSource = SerialPort.GetPortNames();
            btnBypass.IsCLick = Global.ParaCommon.Comunication.IO.IsBypass;
            Parallel.For(0, 8, i =>
            {
                RefreshComboBoxIn(i,"");
            });
            Parallel.For(0, 8, i =>
            {
                RefreshComboBoxOut(i, "");
            });
            cbIn1.DataSource = listIn[0];
            cbIn2.DataSource = listIn[1];
            cbIn3.DataSource = listIn[2];
            cbIn4.DataSource = listIn[3];
            cbIn5.DataSource = listIn[4];
            cbIn6.DataSource = listIn[5];
            cbIn7.DataSource = listIn[6];
            cbIn8.DataSource = listIn[7];

            cbO0.DataSource = listOut[0];
            cbO1.DataSource = listOut[1];
            cbO2.DataSource = listOut[2];
            cbO3.DataSource = listOut[3];
            cbO4.DataSource = listOut[4];
            cbO5.DataSource = listOut[5];
            cbO6.DataSource = listOut[6];
            cbO7.DataSource = listOut[7];
            var paraIOs = Global.ParaCommon.Comunication.IO.paraIOs;

            cbO0.Text = paraIOs.Find(x => x.Adddress == 0&&x.TypeIO==TypeIO.Output)? .I_O_Output .ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO1.Text = paraIOs.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO2.Text = paraIOs.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO3.Text = paraIOs.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO4.Text = paraIOs.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO5.Text = paraIOs.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO6.Text = paraIOs.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO7.Text = paraIOs.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            
            cbIn1.Text = paraIOs.Find(x => x.Adddress == 0 && x.TypeIO == TypeIO.Input).I_O_Input.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbIn2.Text = paraIOs.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Input)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn3.Text = paraIOs.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Input)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn4.Text = paraIOs.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Input)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn5.Text = paraIOs.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Input)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn6.Text = paraIOs.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Input)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn7.Text = paraIOs.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Input)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn8.Text = paraIOs.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Input) ? .I_O_Input.ToString();      // giữ nguyên text cũ nếu không tìm thấy

            cbBaurate.Text = Global.ParaCommon.Comunication.IO.Baurate + "";
            slaveID.Value = Global.ParaCommon.Comunication.IO.SlaveID;
            cbSerialPort.Text = Global.ParaCommon.Comunication.IO.Port;
            if (!Global.ParaCommon.Comunication.IO.IsBypass)
            {
                if (Global.ParaCommon.Comunication.IO.IsConnected)
                {
                    btnConectIO.Text = "Connected";
                    btnConectIO.IsCLick = true;

                    Global.ParaCommon.Comunication.IO.IsBypass = false;
                    Global.ParaCommon.Comunication.IO.StartRead();
                }
                else
                {
                    btnConectIO.Text = "Fail Connect";
                    Global.ParaCommon.Comunication.IO.IsBypass = true;
                    MessageBox.Show("Fail Connect to Module I/O");

                }
            }
            if (Global.ParaCommon.Comunication.IO.valueInput == null)
                Global.ParaCommon.Comunication.IO.valueInput = new IntArrayWithEvent(16);
            //if (Global.ParaCommon.Comunication.IO.valueOutput == null)
            //    Global.ParaCommon.Comunication.IO.valueOutput = new IntArrayWithEvent(16);
            //// 2) Subscribe sự kiện
            Global.ParaCommon.Comunication.IO.valueInput.BulkChanged += ValueInput_BulkChanged;
            //Global.ParaCommon.Comunication.IO.valueOutput.BulkChanged += ValueOutput_BulkChanged;
          listLabelsIn = new List<Label> { DI0, DI1, DI2, DI3, DI4, DI5, DI6, DI7 };
          //  listLabelsOut = new List<Label> { DO0, DO1, DO2, DO3, DO4, DO5, DO6, DO7 };
            btnBypass.IsCLick = Global.ParaCommon.Comunication.IO.IsBypass;
        }

        private void ValueOutput_BulkChanged(object sender, BulkChangedEventArgs e)
        {
            for (int k = 0; k < e.Count; k++)
            {
                int idx = e.StartIndex + k;
                if (idx < listLabelsOut.Count)
                    listLabelsOut[idx].Text = e.NewValues[k].ToString();
                else
                    break;
            }
        }

        List<Label> listLabelsIn = new List<Label>();
        List<Label> listLabelsOut = new List<Label>();
        private void ValueInput_BulkChanged(object sender, BulkChangedEventArgs e)
        {

           
                // 1) If we're not on the UI thread, re‑invoke the entire handler
                if (this.InvokeRequired)
                {
                    // You can use Invoke or BeginInvoke; BeginInvoke won’t block the background thread.
                    this.BeginInvoke(new Action<object, BulkChangedEventArgs>(ValueInput_BulkChanged), sender, e);
                    return;
                }

                // 2) Now we're on the UI thread—safe to touch any control
                for (int k = 0; k < e.Count; k++)
                {
                    int idx = e.StartIndex + k;
                    if (idx >= listLabelsIn.Count)
                        break;

                    listLabelsIn[idx].Text = e.NewValues[k].ToString();
                }
            
        }

      
        private void btnConnect_Click(object sender, EventArgs e)
        {
           Global.ParaCommon.Comunication.IO.Connect();
        
        }

        private void SettingPLC_VisibleChanged(object sender, EventArgs e)
        {
           
                this.LayIntput.Enabled = Global.ParaCommon.Comunication.IO.IsConnected;
           this.LayOutput.Enabled = Global.ParaCommon.Comunication.IO.IsConnected;

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
        List< String> OldIn=new List<string> { "", "", "", "", "", "", "", "" };

        private void cbIn8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn8.SelectedIndex == -1) return;
            String name = cbIn8.Text;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(7, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[7], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(7, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[7] = name;
            }
            ChangeDatasource(7, name);

            //RefreshComboBoxIn(cbIn1);
            //RefreshComboBoxIn(cbIn2);
            //RefreshComboBoxIn(cbIn4);
            //RefreshComboBoxIn(cbIn5);
            //RefreshComboBoxIn(cbIn6);
            //RefreshComboBoxIn(cbIn7);
            //RefreshComboBoxIn(cbIn3);
        }

        private void cbIn7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn7.SelectedIndex == -1) return;
            String name = cbIn7.Text;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(6, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[6], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(6, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[6] = name;
            }
            ChangeDatasource(6,name);
            //RefreshComboBoxIn(cbIn1);
            //RefreshComboBoxIn(cbIn2);
            //RefreshComboBoxIn(cbIn4);
            //RefreshComboBoxIn(cbIn5);
            //RefreshComboBoxIn(cbIn6);
            //RefreshComboBoxIn(cbIn3);
            //RefreshComboBoxIn(cbIn8);

        }

        private void cbIn6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn6.SelectedIndex == -1) return;
            String name = cbIn6.Text;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(5, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[5], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(5, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[5] = name;
            }
            ChangeDatasource(5, name);
            //RefreshComboBoxIn(cbIn1);
            //RefreshComboBoxIn(cbIn2);
            //RefreshComboBoxIn(cbIn4);
            //RefreshComboBoxIn(cbIn3);
            //RefreshComboBoxIn(cbIn5);
            //RefreshComboBoxIn(cbIn7);
            //RefreshComboBoxIn(cbIn8);

        }

        private void cbIn5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn5.SelectedIndex == -1) return;
            String name = cbIn5.Text;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(4, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[4], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(4, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[4] = name;
            }
            ChangeDatasource(4, name);

            //RefreshComboBoxIn(cbIn1);
            //RefreshComboBoxIn(cbIn2);
            //RefreshComboBoxIn(cbIn4);
            //RefreshComboBoxIn(cbIn3);
            //RefreshComboBoxIn(cbIn6);
            //RefreshComboBoxIn(cbIn7);
            //RefreshComboBoxIn(cbIn8);

        }

        private void cbIn4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn4.SelectedIndex == -1) return;
            String name = cbIn4.Text;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(3, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[3], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(3, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[3] = name;
            }
            ChangeDatasource(3, name);
            //RefreshComboBoxIn(cbIn1);
            //RefreshComboBoxIn(cbIn2);
            //RefreshComboBoxIn(cbIn3);
            //RefreshComboBoxIn(cbIn5);
            //RefreshComboBoxIn(cbIn6);
            //RefreshComboBoxIn(cbIn7);
            //RefreshComboBoxIn(cbIn8);

        }

        private void cbIn3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn3.SelectedIndex == -1) return;
            String name = cbIn3.Text;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(2, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[2], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(2, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[2] = name;
            }
            ChangeDatasource(2, name);
            //RefreshComboBoxIn(cbIn1);
            //RefreshComboBoxIn(cbIn2);
            //RefreshComboBoxIn(cbIn4);
            //RefreshComboBoxIn(cbIn5);
            //RefreshComboBoxIn(cbIn6);
            //RefreshComboBoxIn(cbIn7);
            //RefreshComboBoxIn(cbIn8);
        }

        private void cbIn2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn2.SelectedIndex == -1) return;
            String name = cbIn2.Text;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(1, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[1], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(1, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[1] = name;
            }
            ChangeDatasource(1, name);
            //Parallel.For(0, 7, i =>
            //{
            //    RefreshComboBoxIn(cbIn1);
            //    RefreshComboBoxIn(cbIn3);
            //    RefreshComboBoxIn(cbIn4);
            //    RefreshComboBoxIn(cbIn5);
            //    RefreshComboBoxIn(cbIn6);
            //    RefreshComboBoxIn(cbIn7);
            //    RefreshComboBoxIn(cbIn8);

            //});

        }

        private void cbIn1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIn1.SelectedIndex == -1) return;
            String name = cbIn1.Text;
            if (cbIn1.Text.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(0, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[0], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(0, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[0] = name;
            }
            ChangeDatasource(0, name);

            //RefreshComboBoxIn(cbIn2);
            //RefreshComboBoxIn(cbIn3);
            //RefreshComboBoxIn(cbIn4);
            //RefreshComboBoxIn(cbIn5);
            //RefreshComboBoxIn(cbIn6);
            //RefreshComboBoxIn(cbIn7);
            //RefreshComboBoxIn(cbIn8);
        }

        private void comIO_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.Port = comIO.Text;
        }

        private void cbBaurate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.Baurate =Convert.ToInt32( cbBaurate.Text);
        }

        private void slaveID_ValueChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.SlaveID =(byte) slaveID.Value;
        }

        private void btnConectIO_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.Connect();
            if (Global.ParaCommon.Comunication.IO.IsConnected)
            {
                btnConectIO.Text = "Connected";
                btnConectIO.IsCLick = true;
               
                Global.ParaCommon.Comunication.IO.IsBypass = false;
                Global.ParaCommon.Comunication.IO.StartRead();
            }
            else
            {
                btnConectIO.Text = "Fail Connect";
                Global.ParaCommon.Comunication.IO.IsBypass = true;
                MessageBox.Show("Fail Connect to Module I/O");
               
            }
            btnBypass.IsCLick = Global.ParaCommon.Comunication.IO.IsBypass;
        }

        private void timerRead_ValueChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.timeRead = (int)timerRead.Value;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            comIO.DataSource = SerialPort.GetPortNames();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData.Config(Global.Config);

        }

        private void DO0_Click(object sender, EventArgs e)
        {
            int index=Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO0.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if(index>-1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress,DO0.IsCLick);//LIGHT 2
               if(! Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    DO0.IsCLick = false;
                }    
            }    
               
        }
    }
}
