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
        List<String> OldIn = new List<string> { "", "", "", "", "", "", "", "" };
        List<String> OldOut= new List<string> { "", "", "", "", "", "", "", "" };
        public SettingPLC()
        {
            InitializeComponent();
           
            OldIn = new List<string> { "None", "None", "None", "None", "None", "None", "None", "None" };
            OldOut = new List<string> { "None", "None", "None", "None", "None", "None", "None", "None" };
            comIO.DataSource = SerialPort.GetPortNames();
            Parallel.For(0, 8, i =>
            {
                RefreshComboBoxIn(i, "");
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
                            Global.ParaCommon.Comunication.IO.RemoveInPut(0, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn1.Text, ignoreCase: true));
                            cbIn1.SelectedIndex = 0;
                            cbIn1.Text = "None";
                            return;
                        }

                        break;
                    case 1:
                        if (i_O_Input.ToString() == cbIn2.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveInPut(1, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn2.Text, ignoreCase: true));

                            cbIn2.SelectedIndex = 0;
                            cbIn2.Text = "None";
                            return;
                        }
                        break;
                    case 2:
                        if (i_O_Input.ToString() == cbIn3.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveInPut(2, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn3.Text, ignoreCase: true));

                            cbIn3.SelectedIndex = 0;
                            cbIn3.Text = "None";
                            return;
                        }
                        break;
                    case 3:
                        if (i_O_Input.ToString() == cbIn4.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveInPut(3, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn4.Text, ignoreCase: true));

                            cbIn4.SelectedIndex = 0;
                            cbIn4.Text = "None";
                            return;
                        }
                        break;
                    case 4:
                        if (i_O_Input.ToString() == cbIn5.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveInPut(4, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn5.Text, ignoreCase: true));

                            cbIn5.SelectedIndex = 0;
                            cbIn5.Text = "None";
                            return;
                        }
                        break;
                    case 5:
                        if (i_O_Input.ToString() == cbIn6.Text)
                        {Global.ParaCommon.Comunication.IO.RemoveInPut(5, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn6.Text, ignoreCase: true));
                            cbIn6.SelectedIndex = 0;
                            cbIn6.Text = "None";
                            return;
                        }
                        break;
                    case 6:
                        if (i_O_Input.ToString() == cbIn7.Text)
                        {Global.ParaCommon.Comunication.IO.RemoveInPut(6, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn7.Text, ignoreCase: true));
                            cbIn7.SelectedIndex = 0;
                            cbIn7.Text = "None";
                            return;
                        }
                        break;
                    case 7:
                        if (i_O_Input.ToString() == cbIn8.Text)
                        {   Global.ParaCommon.Comunication.IO.RemoveInPut(7, (I_O_Input)Enum.Parse(typeof(I_O_Input), cbIn8.Text, ignoreCase: true));
                            cbIn8.SelectedIndex = 0;
                            cbIn8.Text = "None";
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
                        if (i_O_Output.ToString() == cbO0.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(0, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO0.Text, ignoreCase: true));
                            cbO0.SelectedIndex = 0;
                            cbO0.Text = "None";
                          
                            return;
                        }

                        break;
                    case 1:
                        if (i_O_Output.ToString() == cbO1.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(1, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO1.Text, ignoreCase: true));
                            cbO1.SelectedIndex = 0;
                            cbO1.Text = "None";
                            return;
                        }
                        break;
                    case 2:
                        if (i_O_Output.ToString() == cbO2.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(2, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO2.Text, ignoreCase: true));
                            cbO2.SelectedIndex = 0;
                            cbO2.Text = "None";
                            return;
                        }
                        break;
                    case 3:
                        if (i_O_Output.ToString() == cbIn3.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(3, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO3.Text, ignoreCase: true));
                            cbO3.SelectedIndex = 0;
                            cbO3.Text = "None";
                            return;
                        }
                        break;
                    case 4:
                        if (i_O_Output.ToString() == cbO4.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(4, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO4.Text, ignoreCase: true));
                            cbO4.SelectedIndex = 0;
                            cbO4.Text = "None";
                            return;
                        }
                        break;
                    case 5:
                        if (i_O_Output.ToString() == cbO5.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(5, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO5.Text, ignoreCase: true));
                            cbO5.SelectedIndex = 0;
                            cbO5.Text = "None";
                            return;
                        }
                        break;
                    case 6:
                        if (i_O_Output.ToString() == cbO6.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(6, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO6.Text, ignoreCase: true));
                            cbO6.SelectedIndex = 0;
                            cbO6.Text = "None";
                            return;
                        }
                        break;
                    case 7:
                        if (i_O_Output.ToString() == cbO7.Text)
                        {
                            Global.ParaCommon.Comunication.IO.RemoveOutPut(7, (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO7.Text, ignoreCase: true));
                            cbO7.SelectedIndex = 0;
                            cbO7.Text = "None";
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
            //foreach (Control c1 in LayIntput.Controls)
            //{
            //    foreach (Control c in c1.Controls)
            //    { 
            //        if ((c is RJButton))
            //        {
            //            RJButton btn = c as RJButton;
            //            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

            //            btn.IsCLick = Convert.ToBoolean(Global.ParaCommon.Comunication.IO.valueInput[numAdd]);
            //        }
            //   }
            //}
            if (Global.ParaCommon.Comunication.IO.valueOutput == null) return;
            if (Global.ParaCommon.Comunication.IO.valueOutput.Length < 16) return;
            //foreach (Control c1 in LayOutput.Controls)
            //{
            //    foreach (Control c in c1.Controls)
            //    {
            //        if ((c is RJButton))
            //        {
            //            RJButton btn = c as RJButton;
            //            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

            //            btn.IsCLick = Convert.ToBoolean(Global.ParaCommon.Comunication.IO.valueOutput[numAdd]);
            //        }
            //    }
            //}
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
            await Task.Run(() =>  Global.ParaCommon.Comunication.IO.WriteOutPut());
           
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
            try

            {
                btnBypass.IsCLick = Global.ParaCommon.Comunication.IO.IsBypass;
         
            var paraIOs = Global.ParaCommon.Comunication.IO.paraIOs;

            cbO0.Text = paraIOs.Find(x => x.Adddress == 0&&x.TypeIO==TypeIO.Output)? .I_O_Output .ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO1.Text = paraIOs.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO2.Text = paraIOs.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO3.Text = paraIOs.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO4.Text = paraIOs.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO5.Text = paraIOs.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO6.Text = paraIOs.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO7.Text = paraIOs.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn1.Text = paraIOs.Find(x => x.Adddress == 0 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbIn2.Text = paraIOs.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn3.Text = paraIOs.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn4.Text = paraIOs.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn5.Text = paraIOs.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn6.Text = paraIOs.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn7.Text = paraIOs.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn8.Text = paraIOs.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Input) ? .I_O_Input.ToString();      // giữ nguyên text cũ nếu không tìm thấy
            timerRead.Value=Global.ParaCommon.Comunication.IO.timeRead;
            cbBaurate.Text = Global.ParaCommon.Comunication.IO.Baurate + "";
            slaveID.Value = Global.ParaCommon.Comunication.IO.SlaveID;
            comIO.Text = Global.ParaCommon.Comunication.IO.Port;
                if (Global.ParaCommon.Comunication.IO.AddRead == 0 && Global.ParaCommon.Comunication.IO.AddWrite == 0)
                {
                    Global.ParaCommon.Comunication.IO.AddRead = 1;
                    Global.ParaCommon.Comunication.IO.AddWrite = 2;
                }
                AddRead.Value=Global.ParaCommon.Comunication.IO.AddRead;
            AddWrite.Value = Global.ParaCommon.Comunication.IO.AddWrite;
                listLabelsIn = new List<RJButton> { DI0, DI1, DI2, DI3, DI4, DI5, DI6, DI7 };
                listLabelsOut = new List<RJButton> { DO0, DO1, DO2, D3, DO4, DO5, DO6, DO7 };
                foreach (ParaIO paraIO in Global.ParaCommon.Comunication.IO.paraIOs)
                {
                    if (paraIO.TypeIO == TypeIO.Input)
                    {
                        listLabelsIn[paraIO.Adddress].IsCLick = Convert.ToBoolean(paraIO.Value);// + "";
                        listLabelsIn[paraIO.Adddress].Text = paraIO.Value + "";
                        listLabelsIn[paraIO.Adddress].Refresh();
                    }
                    else
                    {
                        listLabelsOut[paraIO.Adddress].Text = paraIO.Value + "";
                        listLabelsOut[paraIO.Adddress].IsCLick = Convert.ToBoolean(paraIO.Value);// + "";
                        listLabelsOut[paraIO.Adddress].Refresh();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //if (!Global.ParaCommon.Comunication.IO.IsBypass)
            //{
            //    if (Global.ParaCommon.Comunication.IO.IsConnected)
            //    {
            //        btnConectIO.Text = "Connected";
            //        btnConectIO.IsCLick = true;

            //        Global.ParaCommon.Comunication.IO.IsBypass = false;
            //        Global.ParaCommon.Comunication.IO.StartRead();
            //    }
            //    else
            //    {
            //        btnConectIO.Text = "Fail Connect";
            //        Global.ParaCommon.Comunication.IO.IsBypass = true;
            //        MessageBox.Show("Fail Connect to Module I/O");

            //    }
            //}
            //// if (Global.ParaCommon.Comunication.IO.valueInput == null)
            //     Global.ParaCommon.Comunication.IO.valueInput = new IntArrayWithEvent(16);
            // if (Global.ParaCommon.Comunication.IO.valueOutput == null)
            //     Global.ParaCommon.Comunication.IO.valueOutput = new IntArrayWithEvent(16);
            // // 2) Subscribe sự kiện
            // Global.ParaCommon.Comunication.IO.valueInput.BulkChanged += ValueInput_BulkChanged;
            //Global.ParaCommon.Comunication.IO.valueOutput.ItemChanged += ValueOutput_ItemChanged;
            int index = 0;
           foreach(ParaIO paraIO in  Global.ParaCommon.Comunication.IO.paraIOs )
            {
                paraIO.ValueChanged += ParaIO_ValueChanged;
                
               
            }
         
            btnBypass.IsCLick = Global.ParaCommon.Comunication.IO.IsBypass;
            Global.StatusIOChanged += Global_StatusIOChanged;
            Global.ParaCommon.Comunication.IO.LogChanged += IO_LogChanged;
            if (Global.ParaCommon.Comunication.IO.IsConnected)
            {
                pComIO.Enabled = false;
                btnConectIO.Text = "Connected";
                btnConectIO.IsCLick = true;
                btnConectIO.Enabled = false;
                btnBypass.Enabled = true;
            }
            else
            {
                pComIO.Enabled = true;
                btnConectIO.Text = "Fail Connect";
                btnConectIO.IsCLick = false;
                btnConectIO.Enabled = true;
                btnBypass.Enabled = false;
            }
         //   Global.ParaCommon.Comunication.IO.numReadChanged += IO_numReadChanged;
          //  Global.ParaCommon.Comunication.IO.numWriteChanged += IO_numWriteChanged;
        }

        //private void IO_numWriteChanged(int obj)
        //{
        //    this.Invoke((Action)(() =>
        //    {   if(obj>1)
        //        {
        //            lbOut.Text = obj.ToString();
        //            lbOut.Refresh();
        //        }
                
        //    }));
        //}

        //private void IO_numReadChanged(int obj)
        //{
        //    this.Invoke((Action)(() =>
        //    {
        //        if (obj > 1)
        //        {
        //            lbLog.Text = obj.ToString();
        //            lbLog.Refresh();
        //        }
        //    }));
        //}

        private void IO_LogChanged(StringBuilder obj)
        {
            this.Invoke((Action)(() =>
            {
            txtLog1.Text = Global.ParaCommon.Comunication.IO.logBuilder.ToString();
                txtLog1.Refresh();
            }));
        }

        private void Global_StatusIOChanged(StatusIO obj)
        {
            this.Invoke((Action)(() =>
            {
                StatusIObtn.Text = obj.ToString();
                StatusIObtn.Refresh();
            }));
         }

        private void ParaIO_ValueChanged(object arg1, int arg2)
        {
            ParaIO paraIO =arg1 as ParaIO;
            this.Invoke((Action)(() =>
            {
                if (paraIO.TypeIO == TypeIO.Input)
                {

                    listLabelsIn[paraIO.Adddress].Text = arg2 + "";
                    listLabelsIn[paraIO.Adddress].Refresh();
                }
                else
                {
                    listLabelsOut[paraIO.Adddress].Text = arg2 + "";
                    listLabelsOut[paraIO.Adddress].IsCLick = Convert.ToBoolean(arg2);// + "";
                    listLabelsOut[paraIO.Adddress].Refresh();
                }
            }));
        }

      
        List<RJButton> listLabelsIn = new List<RJButton>();
        List<RJButton> listLabelsOut = new List<RJButton>();
      

      
        private async void btnConnect_Click(object sender, EventArgs e)
        {
         await  Global.ParaCommon.Comunication.IO.Connect();
        
        }
       

        private void SettingPLC_VisibleChanged(object sender, EventArgs e)
        {
           
           //     this.LayIntput.Enabled = Global.ParaCommon.Comunication.IO.IsConnected;
         //  this.LayOutput.Enabled = Global.ParaCommon.Comunication.IO.IsConnected;

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
     

    
        private void comIO_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cbBaurate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.Baurate =Convert.ToInt32( cbBaurate.Text);
        }

        private void slaveID_ValueChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.SlaveID =(byte) slaveID.Value;
        }

        private async void btnConectIO_Click(object sender, EventArgs e)
        {
           await Global.ParaCommon.Comunication.IO.Connect();
            if (Global.ParaCommon.Comunication.IO.IsConnected)
            {
                btnConectIO.Text = "Connected";
                btnConectIO.IsCLick = true;
                pComIO.Enabled = false;
             
               
                Global.ParaCommon.Comunication.IO.IsBypass = false;
                tmRead.Enabled = true;
            }
            else
            {
                btnConectIO.Text = "Fail Connect";
                pComIO.Enabled = true;
                Global.ParaCommon.Comunication.IO.IsBypass = true;
                MessageBox.Show("Fail Connect to Module I/O");
               
            }
            btnBypass.IsCLick = Global.ParaCommon.Comunication.IO.IsBypass;
        }

        private void timerRead_ValueChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.timeRead = (int)timerRead.Value;
            tmRead.Interval = Global.ParaCommon.Comunication.IO.timeRead;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            comIO.DataSource = SerialPort.GetPortNames();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData.Config(Global.Config);

        }

        private async void DO0_Click(object sender, EventArgs e)
        {
            int index=Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO0.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if(index>-1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress,DO0.IsCLick);
               if(! await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                   DO0.IsCLick = !DO0.IsCLick;
                }
                DO0.Text = DO0.IsCLick.ToString();
            }    
               
        }

        private void cbIn1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn1.SelectedIndex == -1) return;
            String name = cbIn1.SelectedValue.ToString(); if (name == "") return;
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

        }

        private void cbIn2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn2.SelectedIndex == -1) return;
            String name = cbIn2.SelectedValue.ToString(); if (name == "") return;
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
        }

        private void cbIn3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn3.SelectedIndex == -1) return;
            String name = cbIn3.SelectedValue.ToString(); if (name == "") return;
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
        }

        private void cbIn4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn4.SelectedIndex == -1) return;
            String name = cbIn4.SelectedValue.ToString(); if (name == "") return;
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
        }

        private void cbIn5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn5.SelectedIndex == -1) return;
            String name = cbIn5.SelectedValue.ToString(); if (name == "") return;
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
        }

        private void cbIn6_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn6.SelectedIndex == -1) return;
            String name = cbIn6.SelectedValue.ToString(); if (name == "") return;
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
        }

        private void cbIn7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn7.SelectedIndex == -1) return;
            String name = cbIn7.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveInPut(6, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[6], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddInPut(6, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[6] = name;
            }
            ChangeDatasource(6, name);
        }

        private void cbIn8_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbIn8.SelectedIndex == -1) return;
            String name = cbIn8.SelectedValue.ToString(); if (name == "") return;
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
        }

        private void cbO0_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO0.SelectedIndex == -1) return;
            String name = cbO0.SelectedValue.ToString();
            if (name == "") return;
            if (cbO0.Text.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(0, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[0], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(0, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[0] = name;
            }
            ChangeDatasourceOut(0, name);

        }

        private void cbO1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO1.SelectedIndex == -1) return;
            String name = cbO1.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(1, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[1], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(1, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[1] = name;
            }
            ChangeDatasourceOut(1, name);
        }

        private void cbO2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO2.SelectedIndex == -1) return;
            String name = cbO2.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(2, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[2], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(2, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[2] = name;
            }
            ChangeDatasourceOut(2, name);
        }

        private void cbO3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO3.SelectedIndex == -1) return;
            String name = cbO3.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(3, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[3], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(3, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[3] = name;
            }
            ChangeDatasourceOut(3, name);
        }

        private void cbO4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO4.SelectedIndex == -1) return;
            String name = cbO4.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(4, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[4], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(4, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[0] = name;
            }
            ChangeDatasourceOut(4, name);
        }

        private void cbO5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO5.SelectedIndex == -1) return;
            String name = cbO5.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(5, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[5], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(5, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[5] = name;
            }
            ChangeDatasourceOut(5, name);
        }

        private void cbO6_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO6.SelectedIndex == -1) return;
            String name = cbO6.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(6, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[6], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(6, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[6] = name;
            }
            ChangeDatasourceOut(6, name);
        }

        private void cbO7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbO7.SelectedIndex == -1) return;
            String name = cbO7.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.IO.RemoveOutPut(7, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[7], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.IO.AddOutPut(7, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
                OldOut[7] = name;
            }
            ChangeDatasourceOut(7, name);
        }

        private async void DO1_Click(object sender, EventArgs e)
        {
            int index = Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO1.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress, DO1.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    DO1.IsCLick = !DO1.IsCLick;
                }
                DO1.Text = DO1.IsCLick.ToString();
            }
        }

        private async void DO2_Click(object sender, EventArgs e)
        {
            int index = Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO2.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress, DO2.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    DO2.IsCLick = !DO2.IsCLick;
                }
                DO2.Text = DO2.IsCLick.ToString();
            }
        }

        private async void D3_Click(object sender, EventArgs e)
        {
            int index = Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO3.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress, D3.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    D3.IsCLick = !D3.IsCLick;
                }
                D3.Text = D3.IsCLick.ToString();
            }
        }

        private async void DO4_Click(object sender, EventArgs e)
        {
            int index = Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO4.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress, DO4.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    DO4.IsCLick = !DO4.IsCLick;
                }
                DO4.Text = DO4.IsCLick.ToString();
            }
        }

        private async void DO5_Click(object sender, EventArgs e)
        {
            int index = Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO5.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress, DO5.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    DO5.IsCLick = !DO5.IsCLick;
                }
                DO5.Text = DO5.IsCLick.ToString();
            }
        }

        private async void DO6_Click(object sender, EventArgs e)
        {
            int index = Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO6.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress, DO6.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    DO6.IsCLick = !DO6.IsCLick;
                }
                DO6.Text = DO6.IsCLick.ToString();
            }
        }

        private async void DO7_Click(object sender, EventArgs e)
        {
            int index = Global.ParaCommon.Comunication.IO.paraIOs.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO7.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                Global.ParaCommon.Comunication.IO.SetOutPut(Global.ParaCommon.Comunication.IO.paraIOs[index].Adddress, DO7.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.IO.WriteOutPut())
                {
                    DO7.IsCLick = !DO7.IsCLick;
                }
                DO7.Text = DO7.IsCLick.ToString();
            }
        }

        private void btnBypass_Click(object sender, EventArgs e)
        {
            if(Global.ParaCommon.Comunication.IO.IsConnected)
            {
                tmRead.Enabled = false;
                Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.None;
                Global.ParaCommon.Comunication.IO.Disconnect();
             
                btnConectIO.Text = "No Connect";
            }
            Modbus.IsWrite = false; Modbus.IsReading = false;
            btnConectIO.IsCLick = false;
            btnConectIO.Enabled = true;
            btnBypass.IsCLick = true;
            pComIO.Enabled = true;


        }

        private void comIO_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.Port = comIO.SelectedValue.ToString() ;
        }

        private void tmCheck_Tick(object sender, EventArgs e)
        {
           
        }
        IO_Processing IO_ProcessingOld = IO_Processing.None;
        private async void tmRead_Tick(object sender, EventArgs e)
        {
            if (!Global.ParaCommon.Comunication.IO.IsConnected)
            {
                tmRead.Enabled = false;
                tmConnect.Enabled = true;
                return;
            }    
                if (!Global.Initialed) return;
           // if (Global.StatusIO == StatusIO.Writing|| Global.StatusIO == StatusIO.Reading) return;
            if (Global.ParaCommon.Comunication.IO.IsConnected)
            {

                if (Global.StatusProcessing == StatusProcessing.SendResult)
                {
                    Global.ParaCommon.Comunication.IO.IsLogic1 = false;
                    Global.ParaCommon.Comunication.IO.IsLogic2 = false;
                    Global.ParaCommon.Comunication.IO.IsLogic3 = false;
                    Global.ParaCommon.Comunication.IO.IsLogic4 = false;
                    Global.ParaCommon.Comunication.IO.IsLogic5 = false;
                    Global.ParaCommon.Comunication.IO.IsLogic6 = false;
                    foreach (int ix in Global.ParaCommon.indexLogic1)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                         {
                                Global.ParaCommon.Comunication.IO.IsLogic1 = true;
                                break;
                         }
                    foreach (int ix in Global.ParaCommon.indexLogic2)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.IO.IsLogic2 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic3)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.IO.IsLogic3 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic4)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.IO.IsLogic4 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic5)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.IO.IsLogic4 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic6)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.IO.IsLogic6 = true;
                            break;
                        }
                    Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.Result;


                }

                else if (Global.IsRun && Global.ParaCommon.IsExternal || Global.TriggerInternal)
                {
                    if (Global.ParaCommon.Comunication.IO.CheckReady() || Global.TriggerInternal)
                    {
                        Global.TriggerInternal = false;
                        Global.StatusProcessing = StatusProcessing.Trigger;
                        Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.Trigger;
                        if(Global.IsByPassResult)
                        Global.EditTool.lbBypass.ForeColor = Color.White;



                    }

                }
                if (Global.ParaCommon.Comunication.IO.IO_Processing != IO_ProcessingOld)
                {
                    
                    if (Global.StatusIO == StatusIO.None)
                    {
                        if (Global.ParaCommon.Comunication.IO.IO_Processing == IO_Processing.ByPass)
                            Global.EditTool.lbBypass.ForeColor = Color.Green; 
                        await Global.ParaCommon.Comunication.IO.WriteIO();
                        IO_ProcessingOld = Global.ParaCommon.Comunication.IO.IO_Processing;
                    }
                }
               
                lbmin.Text = Math.Round(Global.ParaCommon.Comunication.IO.CTMin) + "";
                lbMax.Text = Math.Round(Global.ParaCommon.Comunication.IO.CTMax) + "";
                lbMid.Text = Math.Round(Global.ParaCommon.Comunication.IO.CTMid) + "";
                //if (btnEnQrCode.IsCLick)
                //{
                //    if (valueOutput[6] == 0)
                //    {
                //        int[] bits = new int[] { valueInput[4], valueInput[5], valueInput[6], valueInput[7] };  // MSB -> LSB (bit3 bit2 bit1 bit0)

                //        int value = 0;
                //        for (int i = 0; i < 4; i++)
                //        {
                //            value |= (bits[i] & 1) << (3 - i);  // bit 3 là cao nhất
                //        }
                //        int id = listFilter.FindIndex(a => a == Global.Project);
                //        if (id != value)
                //        {

                //            WriteIO(IO_Processing.ChangeProg);
                //            tmReadPLC.Enabled = false;
                //            Global.Project = listFilter[value];
                //            txtQrCode.Text = Global.Project.ToString();
                //            txtQrCode.Enabled = false;
                //            btnShowList.Enabled = false;

                //            workLoadProgram.RunWorkerAsync();
                //        }
                //    }
                //}


                if (Global.StatusIO == StatusIO.None&& Global.StatusProcessing==StatusProcessing.None)
                {
                    await Global.ParaCommon.Comunication.IO.Read();
                    int ix = Global.ParaCommon.Comunication.IO. AddressInput[(int)I_O_Input.ByPass];
                    if (ix > -1)
                    {
                        if (Global.ParaCommon.Comunication.IO.valueInput[ix] == 1 && !Global.IsByPassResult)
                        {
                            Global.IsByPassResult = true;
                            Global.EditTool.lbBypass.Visible = true;
                        }
                        else if (Global.ParaCommon.Comunication.IO.valueInput[ix] == 0 && Global.IsByPassResult)

                        {
                            Global.IsByPassResult = false;
                            Global.EditTool.lbBypass.Visible = false;
                        }        
                    }
                }    
              
                if (Global.StatusIO == StatusIO.Writing && Global.ParaCommon.Comunication.IO.IO_Processing == IO_Processing.None)
                    Global.StatusIO = StatusIO.None;
                StatusIObtn.Text = Global.StatusIO.ToString();
            }
            else

            {
                Global.StatusProcessing = StatusProcessing.None;
                Global.StatusIO = StatusIO.None;
                Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.None;
            }
            
           
        


        }

        private async void tmConnect_Tick(object sender, EventArgs e)
        {if (!Global.Initialed) return;
            tmConnect.Enabled = false;

            if (Global.ParaCommon.Comunication.IO.IsBypass) return;
         await   Global.ParaCommon.Comunication.IO.Connect();

            if (Global.ParaCommon.Comunication.IO.IsConnected)
            {
                Global.StatusIO = StatusIO.None;
                Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.Reset;
             await   Global.ParaCommon.Comunication.IO.WriteIO();
                await Task.Delay(500);
                tmRead.Enabled = true;
                if (Global.ParaCommon.Comunication.IO.timeRead == 0) Global.ParaCommon.Comunication.IO.timeRead = 1;
                tmRead.Interval = Global.ParaCommon.Comunication.IO.timeRead;
                //  tmCheck.Enabled = true;
                // G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;

            }

            else
            {
              //  G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                if (!Global.ParaCommon.Comunication.IO.IsBypass)
                {
                  await  Global.ParaCommon.Comunication.IO.Connect();
                    if (Global.ParaCommon.Comunication.IO.IsConnected)
                        tmRead.Enabled = true;
                    else
                    {
                        MessageBox.Show("Check connect I_O");
                    }
                }
            }
        }

        private void timerRead_Load(object sender, EventArgs e)
        {

        }

        private void workRead_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Global.Initialed) return;
            if (Global.StatusIO == StatusIO.ErrRead)
            {
                //await Task.Delay(50);
                Global.StatusIO = StatusIO.Reading;
            }
            if (Global.StatusIO == StatusIO.Reading)
            {
                Global.ParaCommon.Comunication.IO.Read();

            }
        }

        private async void  workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)

        {
           await Task.Delay( Global.ParaCommon.Comunication.IO.timeRead);
            if (Global.ParaCommon.Comunication.IO.IsConnected)
                workRead.RunWorkerAsync();
            else
                tmConnect.Enabled = true;
            //if (Global.ParaCommon.Comunication.IO.logBuilder != null)
            //{
            //    txtLog1.Text = Global.ParaCommon.Comunication.IO.logBuilder.ToString();
            //    txtLog1.Refresh();
            //}


        }

        private void btnClear_Click(object sender, EventArgs e)
        {if(Global.ParaCommon.Comunication.IO.logBuilder!=null)
            {
                Global.ParaCommon.Comunication.IO.logBuilder.Clear();
                txtLog1.Text = "";
            }
          
        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void AddRead_ValueChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.AddRead= (int)AddRead.Value  ;
           
        }

        private void AddWrite_ValueChanged(object sender, EventArgs e)
        {
             Global.ParaCommon.Comunication.IO.AddWrite=(int)AddWrite.Value ;
        }
    }
}
