using BeeCore;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using Microsoft.VisualBasic.Devices;
using OpenCvSharp.Flann;
using PlcLib;
using System;
using System.Collections;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace BeeInterface
{
    public partial class ProtocolPLC : UserControl
    {
        List<String> OldIn = new List<string>(16);
        List<String> OldOut = new List<string>(16);

        public ProtocolPLC()
        {
            InitializeComponent();

            OldIn = new string[16].ToList();
            OldOut = new string[16].ToList();
            cbCom.DataSource = SerialPort.GetPortNames();
            Parallel.For(0, 16, i =>
            {
                RefreshComboBoxIn(i, "");
            });
            Parallel.For(0, 16, i =>
            {
                RefreshComboBoxOut(i, "");
            });
            //foreach(Control ctl in layIn.Controls)
            //{
            //    if (ctl is ComboBox cb)
            //        listCbIn.Add(cb);
            //}
            //foreach (Control ctl in layOut.Controls)
            //{
            //    if (ctl is ComboBox cb)
            //        listCbOut.Add(cb);
            //}
            cbIn0.DataSource = listIn[0];
            cbIn1.DataSource = listIn[1];
            cbIn2.DataSource = listIn[2];
            cbIn3.DataSource = listIn[3];
            cbIn4.DataSource = listIn[4];
            cbIn5.DataSource = listIn[5];
            cbIn6.DataSource = listIn[6];
            cbIn7.DataSource = listIn[7];
            cbIn8.DataSource = listIn[8];
            cbIn9.DataSource = listIn[9];
            cbIn10.DataSource = listIn[10];
            cbIn11.DataSource = listIn[11];
            cbIn12.DataSource = listIn[12];
            cbIn13.DataSource = listIn[13];
            cbIn14.DataSource = listIn[14];
            cbIn15.DataSource = listIn[15];

            cbO0.DataSource = listOut[0];
            cbO1.DataSource = listOut[1];
            cbO2.DataSource = listOut[2];
            cbO3.DataSource = listOut[3];
            cbO4.DataSource = listOut[4];
            cbO5.DataSource = listOut[5];
            cbO6.DataSource = listOut[6];
            cbO7.DataSource = listOut[7];
            cbO8.DataSource = listOut[8];
            cbO9.DataSource = listOut[9];
            cbO10.DataSource = listOut[10];
            cbO11.DataSource = listOut[11];
            cbO12.DataSource = listOut[12];
            cbO13.DataSource = listOut[13];
            cbO14.DataSource = listOut[14];
            cbO15.DataSource = listOut[15];
            cbParity.DataSource = (Parity[])Enum.GetValues(typeof(Parity));
            cbStopBits.DataSource = (StopBits[])Enum.GetValues(typeof(StopBits));
            cbDataBits.DataSource = new List<String> { "7", "8" };
            if (Global.Comunication.Protocol == null)
            {
                Global.Comunication.Protocol = new ParaProtocol();
            }
            var ParaBits = Global.Comunication.Protocol.ParaBits;
            nameOut[0] = ParaBits.Find(x => x.Adddress == 0 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[1] = ParaBits.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[2] = ParaBits.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[3] = ParaBits.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[4] = ParaBits.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[5] = ParaBits.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[6] = ParaBits.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[7] = ParaBits.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[8] = ParaBits.Find(x => x.Adddress == 8 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[9] = ParaBits.Find(x => x.Adddress == 9 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[10] = ParaBits.Find(x => x.Adddress == 10 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[11] = ParaBits.Find(x => x.Adddress == 11 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[12] = ParaBits.Find(x => x.Adddress == 12 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[13] = ParaBits.Find(x => x.Adddress == 13 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[14] = ParaBits.Find(x => x.Adddress == 14 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            nameOut[15] = ParaBits.Find(x => x.Adddress == 15 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ?? string.Empty;
            if (nameOut[15] != I_O_Output.Alive.ToString())
            {
                nameOut[15] = I_O_Output.Alive.ToString();
                ParaBits.Add(new ParaBit(TypeIO.Output, I_O_Output.Alive, 15));
            }
            nameInput[0] = ParaBits.Find(x => x.Adddress == 0 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[1] = ParaBits.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[2] = ParaBits.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[3] = ParaBits.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[4] = ParaBits.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[5] = ParaBits.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[6] = ParaBits.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[7] = ParaBits.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[8] = ParaBits.Find(x => x.Adddress == 8 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[9] = ParaBits.Find(x => x.Adddress == 9 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[10] = ParaBits.Find(x => x.Adddress == 10 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[11] = ParaBits.Find(x => x.Adddress == 11 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[12] = ParaBits.Find(x => x.Adddress == 12 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[13] = ParaBits.Find(x => x.Adddress == 13 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[14] = ParaBits.Find(x => x.Adddress == 14 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            nameInput[15] = ParaBits.Find(x => x.Adddress == 15 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ?? string.Empty;
            if (nameInput[15] != I_O_Input.Alive.ToString())
            {
                nameInput[15] = I_O_Input.Alive.ToString();
                ParaBits.Add(new ParaBit(TypeIO.Input, I_O_Input.Alive, 15));
            }
            //int ix = 0;
            //for(int i=0;i<16;i++)
            //{
            //   cb.DataSource = listIn[ix];
            //    ix++;
            //}
            //ix = 0;
            //foreach (ComboBox cb in listCbOut)
            //{
            //    cb.DataSource = listOut[ix];
            //    ix++;
            //}

        }
        List<List<string>> listIn = new List<List<string>>{
            new List<string>(), new List<string>(), new List<string>(), new List<string>(),
             new List<string>(), new List<string>(), new List<string>(), new List<string>() ,
         new List<string>(), new List<string>(), new List<string>(), new List<string>(),
             new List<string>(), new List<string>(), new List<string>(), new List<string>()};
        List<List<string>> listOut = new List<List<string>>{
            new List<string>(), new List<string>(), new List<string>(), new List<string>(),
             new List<string>(), new List<string>(), new List<string>(), new List<string>() ,
         new List<string>(), new List<string>(), new List<string>(), new List<string>(),
             new List<string>(), new List<string>(), new List<string>(), new List<string>()};

        // 3. Đổ tất cả enum chưa chọn vào ComboBox
        private void RefreshComboBoxIn(int index, String text)
        {

            listIn[index] = new List<string>();
            List<String> list = new List<string>();
            foreach (I_O_Input io in Enum.GetValues(typeof(I_O_Input)))
            {
                if (io != I_O_Input.Alive)
                    listIn[index].Add(io.ToString());
            }

        }
        private void RefreshComboBoxOut(int index, String text)
        {

            listOut[index] = new List<string>();
            List<String> list = new List<string>();
            foreach (I_O_Output io in Enum.GetValues(typeof(I_O_Output)))
            {
                if (io != I_O_Output.Alive)
                    listOut[index].Add(io.ToString());
            }

        }

        public void ChangeDatasource(int ix, String i_O_Input)
        {
            for (int i = 0; i < 15; i++)
            {
                if (ix == i) continue;
                if (i_O_Input.ToString() == nameInput[i])
                {
                    Global.Comunication.Protocol.RemoveInPut(i, (I_O_Input)Enum.Parse(typeof(I_O_Input), nameInput[i], ignoreCase: true));
                    switch (i)
                    {
                        case 0:
                            cbIn0.Text = "None";
                            break;
                        case 1:
                            cbIn1.Text = "None";
                            break;
                        case 2:
                            cbIn2.Text = "None";
                            break;
                        case 3:
                            cbIn3.Text = "None";
                            break;
                        case 4:
                            cbIn4.Text = "None";
                            break;
                        case 5:
                            cbIn5.Text = "None";
                            break;
                        case 6:
                            cbIn6.Text = "None";
                            break;
                        case 7:
                            cbIn7.Text = "None";
                            break;
                        case 8:
                            cbIn8.Text = "None";
                            break;
                        case 9:
                            cbIn9.Text = "None";
                            break;
                        case 10:
                            cbIn10.Text = "None";
                            break;
                        case 11:
                            cbIn11.Text = "None";
                            break;
                        case 12:
                            cbIn12.Text = "None";
                            break;
                        case 13:
                            cbIn13.Text = "None";
                            break;
                        case 14:
                            cbIn14.Text = "None";
                            break;
                        case 15:
                            cbIn15.Text = "None";
                            break;

                    }
                    //  listCbIn[i].SelectedIndex = 0;
                    // listCbIn[i].Text = "None";

                }


            }


        }
        //private void RefreshComboBoxOut(ComboBox cb)
        //{

        //    cb.Items.Clear();
        //    foreach (I_O_Output io in Enum.GetValues(typeof(I_O_Output)))
        //    {
        //        if (Global.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == io) == -1)
        //            cb.Items.Add(io);
        //    }
        //    // (tuỳ chọn) nếu không có item nào thì disable
        //    cb.Enabled = cb.Items.Count > 0;
        //}
        public void ChangeDatasourceOut(int ix, String i_O_Output)
        {

            for (int i = 0; i < 15; i++)
            {
                if (ix == i) continue;
                if (i_O_Output.ToString() == nameOut[i])
                {

                    Global.Comunication.Protocol.RemoveOutPut(i, (I_O_Output)Enum.Parse(typeof(I_O_Output), nameOut[i], ignoreCase: true));
                    switch (i)
                    {
                        case 0:
                            cbO0.Text = "None";
                            break;
                        case 1:
                            cbO1.Text = "None";
                            break;
                        case 2:
                            cbO2.Text = "None";
                            break;
                        case 3:
                            cbO3.Text = "None";
                            break;
                        case 4:
                            cbO4.Text = "None";
                            break;
                        case 5:
                            cbO5.Text = "None";
                            break;
                        case 6:
                            cbO6.Text = "None";
                            break;
                        case 7:
                            cbO7.Text = "None";
                            break;
                        case 8:
                            cbO8.Text = "None";
                            break;
                        case 9:
                            cbO9.Text = "None";
                            break;
                        case 10:
                            cbO10.Text = "None";
                            break;
                        case 11:
                            cbO11.Text = "None";
                            break;
                        case 12:
                            cbO12.Text = "None";
                            break;
                        case 13:
                            cbO13.Text = "None";
                            break;
                        case 14:
                            cbO14.Text = "None";
                            break;
                        case 15:
                            cbO15.Text = "None";
                            break;

                    }
                    //listCbOut[i].SelectedIndex = 0;
                    //listCbOut[i].Text = "None";

                }


            }


        }
        public void RefreshValuePLC()
        {
            if (IsPress) return;
            if (!this.Visible) return;
            if (Global.Comunication.Protocol.valueInput.Length < 16) return;
            //foreach (Control c1 in LayIntput.Controls)
            //{
            //    foreach (Control c in c1.Controls)
            //    { 
            //        if ((c is RJButton))
            //        {
            //            RJButton btn = c as RJButton;
            //            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

            //            btn.IsCLick = Convert.ToBoolean(Global.Comunication.Protocol.valueInput[numAdd]);
            //        }
            //   }
            //}
            if (Global.Comunication.Protocol.valueOutput == null) return;
            if (Global.Comunication.Protocol.valueOutput.Length < 16) return;
            //foreach (Control c1 in LayOutput.Controls)
            //{
            //    foreach (Control c in c1.Controls)
            //    {
            //        if ((c is RJButton))
            //        {
            //            RJButton btn = c as RJButton;
            //            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

            //            btn.IsCLick = Convert.ToBoolean(Global.Comunication.Protocol.valueOutput[numAdd]);
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
            // if(Global.Comunication.Protocol.valueInput!=null)
            //Global.Comunication.Protocol.valueInput.ReplaceAll( new int[16] { index, index, index, index, index, index, index, index, index, index, index, index, index, index, index, index });

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
            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

            await Task.Run(() => Global.Comunication.Protocol.WriteInPut(numAdd, btn.IsCLick));

            //  G.Header.tmReadPLC.Enabled = true;
            IsPress = false;
            btn.Font = new Font("Arial", 14, FontStyle.Regular);
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
            Global.Comunication.Protocol.SetOutPut(numAdd, btn.IsCLick);
            await Task.Run(() => Global.Comunication.Protocol.WriteOutPut());

            //  G.Header.tmReadPLC.Enabled = true;
            IsPress = false;
            btn.Font = new Font("Arial", 14, FontStyle.Regular);
        }

        private void In_Click(object sender, EventArgs e)
        {
            BtnWriteInPLC((RJButton)sender);
        }

        private void Ou_Click(object sender, EventArgs e)
        {
            BtnWriteOutPLC((RJButton)sender);
        }


        private void SettingPLC_Load(object sender, EventArgs e)
        {// 1) Khởi tạo và bind:
            try

            {
                
                if (Global.Comunication.Protocol == null) Global.Comunication.Protocol = new ParaProtocol();
                btnBypass.IsCLick = Global.Comunication.Protocol.IsBypass;
                btnDtrEnable.IsCLick = Global.Comunication.Protocol.DtrEnable;
                btnRtsEnable.IsCLick = Global.Comunication.Protocol.RtsEnable;
                cbParity.Text = Global.Comunication.Protocol.Parity.ToString();
                cbStopBits.Text = Global.Comunication.Protocol.StopBits.ToString();
                cbDataBits.Text = Global.Comunication.Protocol.DataBit.ToString();

                btnIO.IsCLick = Global.Comunication.Protocol.TypeControler == TypeControler.IO ? true : false;
                btnIsPLC.IsCLick = Global.Comunication.Protocol.TypeControler == TypeControler.PLC ? true : false;
                cbO0.Text = nameOut[0];
                cbO1.Text = nameOut[1];
                cbO2.Text = nameOut[2];
                cbO3.Text = nameOut[3];
                cbO4.Text = nameOut[4];
                cbO5.Text = nameOut[5];
                cbO6.Text = nameOut[6];
                cbO7.Text = nameOut[7];
                cbO8.Text = nameOut[8];
                cbO9.Text = nameOut[9];
                cbO10.Text = nameOut[10];
                cbO11.Text = nameOut[11];
                cbO12.Text = nameOut[12];
                cbO13.Text = nameOut[13];
                cbO14.Text = nameOut[14];
                cbO15.Text = nameOut[15];




                cbIn0.Text = nameInput[0];
                cbIn1.Text = nameInput[1];
                cbIn2.Text = nameInput[2];
                cbIn3.Text = nameInput[3];
                cbIn4.Text = nameInput[4];
                cbIn5.Text = nameInput[5];
                cbIn6.Text = nameInput[6];
                cbIn7.Text = nameInput[7];
                cbIn8.Text = nameInput[8];
                cbIn9.Text = nameInput[9];
                cbIn10.Text = nameInput[10];
                cbIn11.Text = nameInput[11];
                cbIn12.Text = nameInput[12];
                cbIn13.Text = nameInput[13];
                cbIn14.Text = nameInput[14];
                cbIn15.Text = nameInput[15];
                tmOut.Value = Global.Comunication.Protocol.timeOut;
                timerRead.Value = Global.Comunication.Protocol.timeRead;
                cbBaurate.Text = Global.Comunication.Protocol.Baurate + "";
                cbCom.Text = Global.Comunication.Protocol.ComSerial;
                numSlaveID.Value = Global.Comunication.Protocol.SlaveID;
                txtAddRead.Text = Global.Comunication.Protocol.AddRead;
                txtAddWrite.Text = Global.Comunication.Protocol.AddWrite;
                txtAddQty.Text = Global.Comunication.Protocol.AddQty;
                txtAddProg.Text = Global.Comunication.Protocol.AddProg;
                txtAddCountProg.Text = Global.Comunication.Protocol.AddCountProg;
                txtProg.Text = "No" + Global.Comunication.Protocol.NoProg;
                txtAddPO.Text = Global.Comunication.Protocol.AddPO;
                txtAddProgress.Text = Global.Comunication.Protocol.AddProgress;
                listLabelsIn = new List<RJButton> { DI0, DI1, DI2, DI3, DI4, DI5, DI6, DI7, DI8, DI9, DI10, DI11, DI12, DI13, DI14, DI15 };
                listLabelsOut = new List<RJButton> { DO0, DO1, DO2, D3, DO4, DO5, DO6, DO7, DO8, DO9, DO10, DO11, DO12, DO13, DO14, DO15 };
                foreach (ParaBit paraIO in Global.Comunication.Protocol.ParaBits)
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
                if (Global.Comunication.Protocol.PlcBrand == PlcLib.PlcBrand.Keyence)
                    btnKeyence.IsCLick = true;
                if (Global.Comunication.Protocol.PlcBrand == PlcLib.PlcBrand.Mitsubishi)
                    btnMitsu.IsCLick = true;
                if (Global.Comunication.Protocol.PlcBrand == PlcLib.PlcBrand.Mitsubishi2)
                    btnMitsu2.IsCLick = true;
                if (Global.Comunication.Protocol.PlcBrand == PlcLib.PlcBrand.ModbusRtu)
                    btnRtu.IsCLick = true;

                txtIP.Text = Global.Comunication.Protocol.sIP;
                txtPort.Text = Global.Comunication.Protocol.PortIP.ToString();
              
                if (Global.Comunication.Protocol.ConnectionType == PlcLib.ConnectionType.Tcp)
                {
                    layCom.Enabled = false;
                    cbBaurate.Enabled = false;
                    txtIP.Enabled = true;
                    txtPort.Enabled = true;
                    btnTCP.IsCLick = true;
                    lbRTU1.Enabled = false;
                    lbRTU2.Enabled = false;
                    lbTCP1.Enabled = true;
                    lbTCP2.Enabled = true;
                }

                if (Global.Comunication.Protocol.ConnectionType == PlcLib.ConnectionType.Serial)
                {
                    btnSerial.IsCLick = true;
                    layCom.Enabled = true;
                    cbBaurate.Enabled = true;
                    txtIP.Enabled = false;
                    txtPort.Enabled = false;

                    lbRTU1.Enabled = true;
                    lbRTU2.Enabled = true;
                    lbTCP1.Enabled = false;
                    lbTCP2.Enabled = false;
                }
                btnRtu.Enabled = Global.Comunication.Protocol.TypeControler == TypeControler.PLC ? false : true;
                btnKeyence.Enabled = Global.Comunication.Protocol.TypeControler == TypeControler.PLC ? true : false;
                btnMitsu.Enabled = Global.Comunication.Protocol.TypeControler == TypeControler.PLC ? true : false;
                btnDelta.Enabled = Global.Comunication.Protocol.TypeControler == TypeControler.PLC ? true : false;
                layBrand.Enabled = Global.Comunication.Protocol.TypeControler == TypeControler.PCI ? false : true;
                layComunication.Enabled = Global.Comunication.Protocol.TypeControler == TypeControler.PCI ? false : true;
                laySetting.Enabled = Global.Comunication.Protocol.TypeControler == TypeControler.PCI ? false : true;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            foreach (ParaBit paraIO in Global.Comunication.Protocol.ParaBits)
            {
                paraIO.ValueChanged += ParaIO_ValueChanged;


            }

            btnBypass.IsCLick = Global.Comunication.Protocol.IsBypass;
            Global.StatusIOChanged += Global_StatusIOChanged;

            if (Global.Comunication.Protocol.IsConnected)
            {
                //  pComIO.Enabled = false;
                btnConectIO.IsCLick = true;
                btnConectIO.Enabled = false;
                // btnBypass.Enabled = true;
            }
            else
            {
                //   pComIO.Enabled = true;
                btnConectIO.IsCLick = false;
                btnConectIO.Enabled = true;
                //   btnBypass.Enabled = false;
            }
            
            Global.Comunication.Protocol.ValuePOChanged += Protocol_ValuePOChanged;
            Global.Comunication.Protocol.ValueProgChanged += Protocol_ValueProgChanged1;
            Global.Comunication.Protocol.ValueCountProgChanged += Protocol_ValueCountProgChanged;
            Global.Comunication.Protocol.QtyChanged += Protocol_QtyChanged;
            //   Global.Comunication.Protocol.numReadChanged += IO_numReadChanged;
            //  Global.Comunication.Protocol.numWriteChanged += IO_numWriteChanged;
        }

        private void Protocol_QtyChanged(int obj)
        {
            this.Invoke((Action)(() =>
            {
               
                txtValueQty.Text = "" + obj;
            }));
        }

        private void Protocol_ValueCountProgChanged(int obj)
        {
           
            this.Invoke((Action)(() =>
            {
                Global.NumProgFromPLC = obj;
                txtValueCountProg.Text = "" + obj;
            }));
        }

        private void Protocol_ValuePOChanged(string obj)
        {
            this.Invoke((Action)(() =>
            {
                txtValuePO.Text = "" + obj;
            }));
        }

        private void Protocol_ValueProgChanged1(int obj)
        {
            this.Invoke((Action)(() =>
            {
                txtProg.Text ="No"+ obj;
            }));
        }

       

        private void Global_StatusIOChanged(StatusIO obj)
        {
            this.Invoke((Action)(() =>
            {

                if (obj == StatusIO.NotConnect)
                {

                    btnConectIO.IsCLick = false;
                    btnConectIO.Enabled = true;

                }

                StatusIObtn.Text = obj.ToString();
                StatusIObtn.Refresh();
            }));
        }

        private void ParaIO_ValueChanged(object arg1, int arg2)
        {
            ParaBit paraIO = arg1 as ParaBit;
            if (paraIO != null)
                this.Invoke((Action)(() =>
                {
                    if (paraIO.TypeIO == TypeIO.Input)
                    {

                        listLabelsIn[paraIO.Adddress].Text = arg2 + "";
                        listLabelsIn[paraIO.Adddress].IsCLick = Convert.ToBoolean(arg2);
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
            await Global.Comunication.Protocol.Connect();

        }


        private void SettingPLC_VisibleChanged(object sender, EventArgs e)
        {

            //     this.LayIntput.Enabled = Global.Comunication.Protocol.IsConnected;
            //  this.LayOutput.Enabled = Global.Comunication.Protocol.IsConnected;

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




        private void cbBaurate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.Baurate = Convert.ToInt32(cbBaurate.Text);
        }

        private void slaveID_ValueChanged(object sender, EventArgs e)
        {

        }

        private async void btnConectIO_Click(object sender, EventArgs e)
        {
            Global.PLCStatus = PLCStatus.NotConnect;
            await Global.Comunication.Protocol.Connect();
            if (Global.Comunication.Protocol.IsConnected)
            {
                Global.PLCStatus = PLCStatus.Ready;

                btnConectIO.IsCLick = true;
                //pComIO.Enabled = false;


                Global.Comunication.Protocol.IsBypass = false;

            }
            else
            {
                Global.PLCStatus = PLCStatus.ErrorConnect;
                // pComIO.Enabled = true;
                Global.Comunication.Protocol.IsBypass = true;
                MessageBox.Show("Fail Connect to Module I/O");

            }
            btnBypass.IsCLick = Global.Comunication.Protocol.IsBypass;
        }



        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cbCom.DataSource = SerialPort.GetPortNames();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData.Config(Global.Config);

        }




        public String[] nameInput = new String[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
        private void cbIn_SelectionChangeCommitted(object sender, EventArgs e)
        {

            ComboBox cb = sender as ComboBox;
            String name = cb.SelectedValue.ToString();
            var m = Regex.Match(cb.Name, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42
            if (name == "") return;
            if (name.Contains("None"))
            {
                if (OldIn[value] == null) OldIn[value] = "None";
                Global.Comunication.Protocol.RemoveInPut(value, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[value], ignoreCase: true));
            }
            else
            {
                Global.Comunication.Protocol.AddInPut(value, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[value] = name;
            }
            nameInput[value] = name;
            ChangeDatasource(value, name);
            Global.Comunication.Protocol.Arrange();
        }
        public String[] nameOut = new String[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
        private void cbOut_SelectionChangeCommitted(object sender, EventArgs e)
        {

            ComboBox cb = sender as ComboBox;
            String name = cb.SelectedValue.ToString();
            var m = Regex.Match(cb.Name, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42

            if (name == "") return;
            if (name.Contains("None"))
            {
                if (OldOut[value] == null) OldOut[value] = "None";
                Global.Comunication.Protocol.RemoveOutPut(value, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[value], ignoreCase: true));

            }
            else
                Global.Comunication.Protocol.AddOutPut(value, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
            OldOut[value] = name;
            nameOut[value] = name;
            ChangeDatasourceOut(value, name);
            Global.Comunication.Protocol.Arrange();
        }

        private async void DOutClick(object sender, EventArgs e)
        {
            RJButton btn = sender as RJButton;

            var m = Regex.Match(btn.Name, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42

            if (Global.Config.IsExternal)
            {
                btn.IsCLick = !btn.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Enum.TryParse<I_O_Output>(nameOut[value], true, out var outputEnum)
           ? Global.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == outputEnum && a.TypeIO == TypeIO.Output)
           : -1;
            //int index = Global.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), nameOut[value], ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {

                Global.Comunication.Protocol.SetOutPut(Global.Comunication.Protocol.ParaBits[index].Adddress, btn.IsCLick);
                if (!await Global.Comunication.Protocol.WriteOutPut())
                {
                    btn.IsCLick = !btn.IsCLick;
                }

                btn.Text = Convert.ToInt16(btn.IsCLick).ToString();
            }
            else
                btn.IsCLick = false;

        }


        private async void btnBypass_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.IsConnected)
            {



                Global.Comunication.Protocol.IO_Processing = IO_Processing.Busy;
                Global.Comunication.Protocol.Disconnect();


            }
            btnConectIO.IsCLick = false;
            btnConectIO.Enabled = true;
            btnBypass.IsCLick = true;
            //pComIO.Enabled = true;


        }

        private void comIO_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.ComSerial = cbCom.SelectedValue.ToString();
        }

        private void tmCheck_Tick(object sender, EventArgs e)
        {

        }
        IO_Processing IO_ProcessingOld = IO_Processing.None;

        private async void tmConnect_Tick(object sender, EventArgs e)
        {
            if (!Global.Initialed) return;
            tmConnect.Enabled = false;
            if (Global.Comunication.Protocol == null) Global.Comunication.Protocol = new ParaProtocol();
            if (Global.Comunication.Protocol.IsBypass) return;
            if (Global.Comunication.Protocol.IsConnected)
                Global.Comunication.Protocol.Disconnect();
            await Global.Comunication.Protocol.Connect();

            if (Global.Comunication.Protocol.IsConnected)
            {
                Global.StatusIO = StatusIO.None;
                Global.Comunication.Protocol.IO_Processing = IO_Processing.Reset;





            }

            else
            {
                //  G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                if (!Global.Comunication.Protocol.IsBypass)
                {
                    await Global.Comunication.Protocol.Connect();
                    if (Global.Comunication.Protocol.IsConnected)
                    {
                        Global.StatusIO = StatusIO.None;
                        Global.Comunication.Protocol.IO_Processing = IO_Processing.Reset;


                    }

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




        private void btnClear_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.CTMin = 100000;
            Global.Comunication.Protocol.CTMax = 0;

        }

        private void label50_Click(object sender, EventArgs e)
        {

        }






        private void timerRead_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.timeRead = (int)timerRead.Value;

        }



        private void btnRTU_Click(object sender, EventArgs e)
        {

        }


        private void txtLog1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnMitsu_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.PlcBrand = PlcLib.PlcBrand.Mitsubishi;


        }

        private void btnKeyence_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.PlcBrand = PlcLib.PlcBrand.Keyence;

        }

        private void btnSerial_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.ConnectionType = PlcLib.ConnectionType.Serial;
            layCom.Enabled = true;
            cbBaurate.Enabled = true;
            txtIP.Enabled = false;
            txtPort.Enabled = false;

            lbRTU1.Enabled = true;
            lbRTU2.Enabled = true;
            lbTCP1.Enabled = false;
            lbTCP2.Enabled = false;
        }

        private void btnTCP_Click_1(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.ConnectionType = PlcLib.ConnectionType.Tcp;
            layCom.Enabled = false;
            cbBaurate.Enabled = false;
            txtIP.Enabled = true;
            txtPort.Enabled = true;

            lbRTU1.Enabled = false;
            lbRTU2.Enabled = false;
            lbTCP1.Enabled = true;
            lbTCP2.Enabled = true;
        }

        private void txtAddRead_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.AddRead = txtAddRead.Text.Trim();
        }

        private void txtAddWrite_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.AddWrite = txtAddWrite.Text.Trim();
        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbParity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedValue.ToString());
        }

        private void cbStopBits_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBits.SelectedValue.ToString());
        }

        private void cbDataBits_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbDataBits.SelectedValue == null)
                return;
            Global.Comunication.Protocol.DataBit = Convert.ToInt32(cbDataBits.SelectedValue.ToString());
        }

        private void btnDtrEnable_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.DtrEnable = btnDtrEnable.IsCLick;
        }

        private void btnRtsEnable_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.RtsEnable = btnRtsEnable.IsCLick;
        }

        private void btnReScan_Click(object sender, EventArgs e)
        {
            cbCom.DataSource = SerialPort.GetPortNames();
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.PortIP = Convert.ToInt32(txtPort.Text.Trim());
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.sIP = txtIP.Text.Trim();
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void label50_Click_1(object sender, EventArgs e)
        {

        }

        private void btnRS485_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.PlcBrand = PlcLib.PlcBrand.ModbusRtu;
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numSlaveID_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.SlaveID = (byte)numSlaveID.Value;

        }

        private void btnModbusASII_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.PlcBrand = PlcLib.PlcBrand.ModbusAscii;
        }

        private void btnDelta_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.PlcBrand = PlcLib.PlcBrand.Delta;
        }

        private void btnIsPLC_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.TypeControler = TypeControler.PLC;
            btnRtu.Enabled = false;
            btnModbusAscii.Enabled = false;
            btnKeyence.Enabled = true;
            btnMitsu.Enabled = true;
            btnDelta.Enabled = true;
            btnMitsu.PerformClick();
            layBrand.Enabled = true;
            laySetting.Enabled = true;
            layComunication.Enabled = true;
            Global.Config.IsResetReady = true;
        }

        private void btnIO_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.TypeControler = TypeControler.IO;
            Global.Config.IsResetReady = false;
            layBrand.Enabled = true;
            layComunication.Enabled = true;
            btnRtu.Enabled = true;
            btnModbusAscii.Enabled = true;
            btnKeyence.Enabled = false;
            btnMitsu.Enabled = false;
            btnDelta.Enabled = false;
            laySetting.Enabled = true;
            btnRtu.PerformClick();
        }

        private void numSlaveID2_ValueChanged(float obj)
        {

        }

        private void tmOut_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.timeOut = (int)tmOut.Value;
        }

        private void btnMitsu2_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.PlcBrand = PlcLib.PlcBrand.Mitsubishi2;
        }

        private void btnPCICard_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.TypeControler = TypeControler.PCI;
            layBrand.Enabled = false;
            layComunication.Enabled = false;
            laySetting.Enabled = false;
        }

        private void txtAddProg_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.AddProg= txtAddProg.Text.Trim();
        }

        private void tmReadProgNo_Tick(object sender, EventArgs e)
        {
            txtProg.Text ="No"+ Global.Comunication.Protocol.NoProg;
        }

        private void tabControl1_VisibleChanged(object sender, EventArgs e)
        {
           // tmReadProgNo.Enabled = !this.Visible;
        }

        private void txtPO_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.AddPO= txtAddPO.Text.Trim();
        }

        private void txtAddCountProg_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.AddCountProg= txtAddCountProg.Text.Trim();
        }

        private void btnReadCountProg_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.AddCountProg != null)
                if (Global.Comunication.Protocol.AddCountProg != "")
                {
                    Global.Comunication.Protocol.ValueCountProg = Global.Comunication.Protocol.PlcClient.ReadInt(Global.Comunication.Protocol.AddCountProg);
                    Global.NumProgFromPLC = Global.Comunication.Protocol.ValueCountProg;
                }    
                
            txtValueCountProg.Text=Global.NumProgFromPLC.ToString();
        }

        private void btnReadPO_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.AddPO != null)
                if (Global.Comunication.Protocol.AddPO != "")
                    Global.Comunication.Protocol.ValuePO = Global.Comunication.Protocol.PlcClient.ReadStringAsciiKey(Global.Comunication.Protocol.AddPO, 16).Trim();
       txtValuePO.Text= Global.Comunication.Protocol.ValuePO;
        }

        private void btnReadProg_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.AddProg != null)
                if (Global.Comunication.Protocol.AddProg != "")
                    Global.Comunication.Protocol.NoProg = Global.Comunication.Protocol.PlcClient.ReadInt(Global.Comunication.Protocol.AddProg);
            txtProg.Text = "No" + Global.Comunication.Protocol.NoProg;
        }

        private void btnReadQty_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.AddQty != null)
                if (Global.Comunication.Protocol.AddQty != "")
                    Global.Comunication.Protocol.ValueQty = Global.Comunication.Protocol.PlcClient.ReadInt(Global.Comunication.Protocol.AddQty);
            Global.Config.SumTime = Global.Comunication.Protocol.ValueQty;
            Global.Config.SumOK = Global.Config.SumTime - Global.Config.SumNG;
            txtValueQty.Text = Global.Comunication.Protocol.ValueQty+"";
        }

        private void txtAddQty_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.AddQty = txtAddQty.Text.Trim();
        }

        private void btnReadProgress_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.AddProgress != null)
                if (Global.Comunication.Protocol.AddProgress != "")
                    Global.Comunication.Protocol.ValueProgress = Global.Comunication.Protocol.PlcClient.ReadInt(Global.Comunication.Protocol.AddProgress);
            txtValueProgress.Text = Global.Comunication.Protocol.ValueProgress + "";

        }

        private void txtAddProgress_TextChanged(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.AddProgress=txtAddProgress.Text.Trim();
        }
    }
}
