using BeeCore;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using OpenCvSharp.Flann;
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
        List<String> OldOut= new List<string>(16);
        
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
            cbIn0.DataSource= listIn[0];
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
            cbParity.DataSource= (Parity[])Enum.GetValues(typeof(Parity));
            cbStopBits.DataSource= (StopBits[])Enum.GetValues(typeof(StopBits));
            cbDataBits.DataSource = new List<String> { "7", "8" };
            if(Global.ParaCommon.Comunication.Protocol==null)
            {
                Global.ParaCommon.Comunication.Protocol = new ParaProtocol();
            }    
            var ParaBits = Global.ParaCommon.Comunication.Protocol.ParaBits;
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
        private void RefreshComboBoxIn(int index ,String text)
        {

            listIn[index] = new List<string>();
            List<String> list = new List<string>();
            foreach (I_O_Input io in Enum.GetValues(typeof(I_O_Input)))
            {
             if(io!=I_O_Input.Alive)
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
      
        public void ChangeDatasource(int ix,String i_O_Input)
        {
            for(int i=0;i<15;i++)
            {
                if (ix == i) continue;
                if (i_O_Input.ToString() == nameInput[i])
                {
                    Global.ParaCommon.Comunication.Protocol.RemoveInPut(i, (I_O_Input)Enum.Parse(typeof(I_O_Input), nameInput[i], ignoreCase: true));
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
        //        if (Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == io) == -1)
        //            cb.Items.Add(io);
        //    }
        //    // (tuỳ chọn) nếu không có item nào thì disable
        //    cb.Enabled = cb.Items.Count > 0;
        //}
        public void ChangeDatasourceOut(int ix, String i_O_Output)
        {

            for (int i = 0; i < 7; i++)
            {
                if (ix == i) continue;
                if (i_O_Output.ToString() == nameOut[i])
                {
                  
                    Global.ParaCommon.Comunication.Protocol.RemoveOutPut(i, (I_O_Output)Enum.Parse(typeof(I_O_Output), nameOut[i], ignoreCase: true));
                  switch(i)
                    {
                        case 0:cbO0.Text= "None";
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
        {if (IsPress) return;
            if (!this.Visible) return;
            if (Global.ParaCommon.Comunication.Protocol.valueInput.Length < 16) return;
            //foreach (Control c1 in LayIntput.Controls)
            //{
            //    foreach (Control c in c1.Controls)
            //    { 
            //        if ((c is RJButton))
            //        {
            //            RJButton btn = c as RJButton;
            //            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

            //            btn.IsCLick = Convert.ToBoolean(Global.ParaCommon.Comunication.Protocol.valueInput[numAdd]);
            //        }
            //   }
            //}
            if (Global.ParaCommon.Comunication.Protocol.valueOutput == null) return;
            if (Global.ParaCommon.Comunication.Protocol.valueOutput.Length < 16) return;
            //foreach (Control c1 in LayOutput.Controls)
            //{
            //    foreach (Control c in c1.Controls)
            //    {
            //        if ((c is RJButton))
            //        {
            //            RJButton btn = c as RJButton;
            //            int numAdd = Convert.ToInt32(btn.Name.Substring(2).Trim()) - 1;

            //            btn.IsCLick = Convert.ToBoolean(Global.ParaCommon.Comunication.Protocol.valueOutput[numAdd]);
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
           // if(Global.ParaCommon.Comunication.Protocol.valueInput!=null)
           //Global.ParaCommon.Comunication.Protocol.valueInput.ReplaceAll( new int[16] { index, index, index, index, index, index, index, index, index, index, index, index, index, index, index, index });

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

            await Task.Run(() => Global.ParaCommon.Comunication.Protocol.WriteInPut(numAdd, btn.IsCLick));
            
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
            Global.ParaCommon.Comunication.Protocol.SetOutPut(numAdd, btn.IsCLick);
            await Task.Run(() =>  Global.ParaCommon.Comunication.Protocol.WriteOutPut());
           
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

      
        private void SettingPLC_Load(object sender, EventArgs e)
        {// 1) Khởi tạo và bind:
            try

            {
                if (Global.ParaCommon.Comunication.Protocol == null) Global.ParaCommon.Comunication.Protocol = new ParaProtocol();
                btnBypass.IsCLick = Global.ParaCommon.Comunication.Protocol.IsBypass;
                btnDtrEnable.IsCLick = Global.ParaCommon.Comunication.Protocol.DtrEnable;
                btnRtsEnable.IsCLick = Global.ParaCommon.Comunication.Protocol.RtsEnable;
                cbParity.Text = Global.ParaCommon.Comunication.Protocol.Parity.ToString();
                cbStopBits.Text = Global.ParaCommon.Comunication.Protocol.StopBits.ToString();
                cbDataBits.Text = Global.ParaCommon.Comunication.Protocol.DataBit.ToString();

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

                  timerRead.Value=Global.ParaCommon.Comunication.Protocol.timeRead;
            cbBaurate.Text = Global.ParaCommon.Comunication.Protocol.Baurate + "";
           
            cbCom.Text = Global.ParaCommon.Comunication.Protocol.ComSerial;
            
                txtAddRead.Text=Global.ParaCommon.Comunication.Protocol.AddRead;
            txtAddWrite.Text = Global.ParaCommon.Comunication.Protocol.AddWrite;
                listLabelsIn = new List<RJButton> { DI0, DI1, DI2, DI3, DI4, DI5, DI6, DI7 , DI8, DI9, DI10, DI11, DI12, DI13, DI14, DI15 };
                listLabelsOut = new List<RJButton> { DO0, DO1, DO2, D3, DO4, DO5, DO6, DO7, DO8, DO9, DO10, DO11, DO12, DO13, DO14, DO15 };
                foreach (ParaBit paraIO in Global.ParaCommon.Comunication.Protocol.ParaBits)
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
            //if (!Global.ParaCommon.Comunication.Protocol.IsBypass)
            //{
            //    if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            //    {
            //        btnConectIO.Text = "Connected";
            //        btnConectIO.IsCLick = true;

            //        Global.ParaCommon.Comunication.Protocol.IsBypass = false;
            //        Global.ParaCommon.Comunication.Protocol.StartRead();
            //    }
            //    else
            //    {
            //        btnConectIO.Text = "Fail Connect";
            //        Global.ParaCommon.Comunication.Protocol.IsBypass = true;
            //        MessageBox.Show("Fail Connect to Module I/O");

            //    }
            //}
            //// if (Global.ParaCommon.Comunication.Protocol.valueInput == null)
            //     Global.ParaCommon.Comunication.Protocol.valueInput = new IntArrayWithEvent(16);
            // if (Global.ParaCommon.Comunication.Protocol.valueOutput == null)
            //     Global.ParaCommon.Comunication.Protocol.valueOutput = new IntArrayWithEvent(16);
            // // 2) Subscribe sự kiện
            // Global.ParaCommon.Comunication.Protocol.valueInput.BulkChanged += ValueInput_BulkChanged;
            //Global.ParaCommon.Comunication.Protocol.valueOutput.ItemChanged += ValueOutput_ItemChanged;
            int index = 0;
           foreach(ParaBit paraIO in  Global.ParaCommon.Comunication.Protocol.ParaBits )
            {
                paraIO.ValueChanged += ParaIO_ValueChanged;
                
               
            }
         
            btnBypass.IsCLick = Global.ParaCommon.Comunication.Protocol.IsBypass;
            Global.StatusIOChanged += Global_StatusIOChanged;
         
            if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            {
              //  pComIO.Enabled = false;
                btnConectIO.Text = "Connected";
                btnConectIO.IsCLick = true;
                btnConectIO.Enabled = false;
                btnBypass.Enabled = true;
            }
            else
            {
             //   pComIO.Enabled = true;
                btnConectIO.Text = "Fail Connect";
                btnConectIO.IsCLick = false;
                btnConectIO.Enabled = true;
                btnBypass.Enabled = false;
            }
         //   Global.ParaCommon.Comunication.Protocol.numReadChanged += IO_numReadChanged;
          //  Global.ParaCommon.Comunication.Protocol.numWriteChanged += IO_numWriteChanged;
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
            ParaBit paraIO =arg1 as ParaBit;
            if(paraIO!=null)
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
         await  Global.ParaCommon.Comunication.Protocol.Connect();
        
        }
       

        private void SettingPLC_VisibleChanged(object sender, EventArgs e)
        {
           
           //     this.LayIntput.Enabled = Global.ParaCommon.Comunication.Protocol.IsConnected;
         //  this.LayOutput.Enabled = Global.ParaCommon.Comunication.Protocol.IsConnected;

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
            Global.ParaCommon.Comunication.Protocol.Baurate =Convert.ToInt32( cbBaurate.Text);
        }

        private void slaveID_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private async void btnConectIO_Click(object sender, EventArgs e)
        {
           await Global.ParaCommon.Comunication.Protocol.Connect();
            if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            {
                btnConectIO.Text = "Connected";
                btnConectIO.IsCLick = true;
                //pComIO.Enabled = false;
             
               
                Global.ParaCommon.Comunication.Protocol.IsBypass = false;
                tmRead.Enabled = true;
            }
            else
            {
                btnConectIO.Text = "Fail Connect";
               // pComIO.Enabled = true;
                Global.ParaCommon.Comunication.Protocol.IsBypass = true;
                MessageBox.Show("Fail Connect to Module I/O");
               
            }
            btnBypass.IsCLick = Global.ParaCommon.Comunication.Protocol.IsBypass;
        }

      

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cbCom.DataSource = SerialPort.GetPortNames();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData.Config(Global.Config);

        }

       

   
        public String[] nameInput =new String[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
        private void cbIn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            String name = cb.SelectedValue.ToString();
            var m = Regex.Match(cb.Name, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42
            if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(value, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[value], ignoreCase: true));
            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(value, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[value] = name;
            }
            nameInput[value] = name;
            ChangeDatasource(value, name);
        }
        public String[] nameOut = new String[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
        private void cbOut_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb=  sender  as ComboBox;
            String name = cb.SelectedValue.ToString();
            var m = Regex.Match(cb.Name, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42
            if (name == "") return;
            if (cb.Text.Contains("None")&&OldOut[value]!=null)
                Global.ParaCommon.Comunication.Protocol.RemoveOutPut(value, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldOut[value], ignoreCase: true));
            else
                Global.ParaCommon.Comunication.Protocol.AddOutPut(value, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true));
            OldOut[value] = name;
            nameOut[value] = name;
            ChangeDatasourceOut(value, name);

        }

        private async void DOutClick(object sender, EventArgs e)
        {
            RJButton btn = sender as RJButton;
         
            var m = Regex.Match(btn.Name, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42
          
            if (Global.ParaCommon.IsExternal)
            {
                btn.IsCLick = !btn.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Enum.TryParse<I_O_Output>(nameOut[value], true, out var outputEnum)
           ? Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == outputEnum && a.TypeIO == TypeIO.Output)
           : -1;
            //int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), nameOut[value], ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, btn.IsCLick);
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    btn.IsCLick = !btn.IsCLick;
                }
                tmRead.Enabled = true;
                btn.Text = Convert.ToInt16(btn.IsCLick).ToString();
            }
            else
                btn.IsCLick = false;

        }
        private async void DO0_Click(object sender, EventArgs e)
        {
            if (Global.ParaCommon.IsExternal)
            {
                DO0.IsCLick = !DO0.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO0.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, DO0.IsCLick);
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    DO0.IsCLick = !DO0.IsCLick;
                }
                tmRead.Enabled = true;
                DO0.Text =Convert.ToInt16( DO0.IsCLick).ToString();
            }

        }
        private async void DO1_Click(object sender, EventArgs e)
        {
            if(Global.ParaCommon.IsExternal)
            {
                DO1.IsCLick = !DO1.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }    
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO1.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, DO1.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    DO1.IsCLick = !DO1.IsCLick;
                }
                tmRead.Enabled = true;
                DO1.Text =Convert.ToInt16( DO1.IsCLick).ToString();
            }
        }

        private async void DO2_Click(object sender, EventArgs e)
        {
            if (Global.ParaCommon.IsExternal)
            {
                DO2.IsCLick = !DO2.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO2.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, DO2.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    DO2.IsCLick = !DO2.IsCLick;
                }
                tmRead.Enabled = true;
                DO2.Text =Convert.ToInt16( DO2.IsCLick).ToString();
            }
        }

        private async void D3_Click(object sender, EventArgs e)
        {
            if (Global.ParaCommon.IsExternal)
            {
                D3.IsCLick = !D3.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO3.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, D3.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    D3.IsCLick = !D3.IsCLick;
                }
                tmRead.Enabled = true;
                D3.Text =Convert.ToInt16( D3.IsCLick).ToString();
            }
        }

        private async void DO4_Click(object sender, EventArgs e)
        {
            if (Global.ParaCommon.IsExternal)
            {
                DO4.IsCLick = !DO4.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO4.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, DO4.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    DO4.IsCLick = !DO4.IsCLick;
                }
                tmRead.Enabled = true;
                DO4.Text =Convert.ToInt16( DO4.IsCLick).ToString();
            }
        }

        private async void DO5_Click(object sender, EventArgs e)
        {
            if (Global.ParaCommon.IsExternal)
            {
                DO5.IsCLick = !DO5.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO5.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, DO5.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    DO5.IsCLick = !DO5.IsCLick;
                }
                tmRead.Enabled = true;
                DO5.Text = Convert.ToInt16(DO5.IsCLick).ToString();
            }
        }

        private async void DO6_Click(object sender, EventArgs e)
        {
            if (Global.ParaCommon.IsExternal)
            {
                DO6.IsCLick = !DO6.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO6.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, DO6.IsCLick);//LIGHT 2
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    DO6.IsCLick = !DO6.IsCLick;
                }
                tmRead.Enabled = true;
                DO6.Text = Convert.ToInt16(DO6.IsCLick).ToString();
                
            }
        }

        private async void DO7_Click(object sender, EventArgs e)
        {
            if (Global.ParaCommon.IsExternal)
            {
                DO7.IsCLick = !DO7.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), cbO7.Text, ignoreCase: true) && a.TypeIO == TypeIO.Output);
            if (index > -1)
            {
                tmRead.Enabled = false;
                await Task.Delay(100);
                Global.ParaCommon.Comunication.Protocol.SetOutPut(Global.ParaCommon.Comunication.Protocol.ParaBits[index].Adddress, DO7.IsCLick);//LIGHT 2
                
                if (!await Global.ParaCommon.Comunication.Protocol.WriteOutPut())
                {
                    DO7.IsCLick = !DO7.IsCLick;
                }
                tmRead.Enabled = true;
                DO7.Text =Convert.ToInt16( DO7.IsCLick).ToString();
            }
        }

        private async void btnBypass_Click(object sender, EventArgs e)
        {
            if(Global.ParaCommon.Comunication.Protocol.IsConnected)
            {
                tmRead.Enabled = false;
                await Task.Delay(500);
                Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.None;
                Global.ParaCommon.Comunication.Protocol.Disconnect();
             
                btnConectIO.Text = "No Connect";
            }
            Modbus.IsWrite = false; Modbus.IsReading = false;
            btnConectIO.IsCLick = false;
            btnConectIO.Enabled = true;
            btnBypass.IsCLick = true;
            //pComIO.Enabled = true;


        }

        private void comIO_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.ComSerial = cbCom.SelectedValue.ToString() ;
        }

        private void tmCheck_Tick(object sender, EventArgs e)
        {
           
        }
        IO_Processing IO_ProcessingOld = IO_Processing.None;
        private async void tmRead_Tick(object sender, EventArgs e)
        {
            if (!Global.ParaCommon.Comunication.Protocol.IsConnected)
            {
               // MessageBox.Show("Err");
                tmRead.Enabled = false;
                tmConnect.Enabled = true;
                return;
            }    
                if (!Global.Initialed) return;
           // if (Global.StatusIO == StatusIO.Writing|| Global.StatusIO == StatusIO.Reading) return;
            if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            {

                if (Global.StatusProcessing == StatusProcessing.SendResult)
                {
                    Global.ParaCommon.Comunication.Protocol.IsLogic1 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic2 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic3 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic4 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic5 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic6 = false;
                    foreach (int ix in Global.ParaCommon.indexLogic1)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                         {
                                Global.ParaCommon.Comunication.Protocol.IsLogic1 = true;
                                break;
                         }
                    foreach (int ix in Global.ParaCommon.indexLogic2)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.Protocol.IsLogic2 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic3)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.Protocol.IsLogic3 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic4)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.Protocol.IsLogic4 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic5)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.Protocol.IsLogic4 = true;
                            break;
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic6)
                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG)
                        {
                            Global.ParaCommon.Comunication.Protocol.IsLogic6 = true;
                            break;
                        }
                    Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Result;


                }

                else if (Global.IsRun && Global.ParaCommon.IsExternal || Global.TriggerInternal)
                {
                    if (Global.ParaCommon.Comunication.Protocol.CheckReady() || Global.TriggerInternal)
                    {
                        
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO"," Trigger OK"));
                        Global.TriggerInternal = false;
                        Global.StatusProcessing = StatusProcessing.Trigger;
                        Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Trigger;
                        if(Global.IsByPassResult)
                        Global.EditTool.lbBypass.ForeColor = Color.White;
                        tmRead.Enabled = false;



                    }
                  

                }
                if (Global.ParaCommon.Comunication.Protocol.IO_Processing != IO_ProcessingOld)
                {
                    
                    if (Global.StatusIO == StatusIO.None)
                    {
                        
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO_WRITE", Global.ParaCommon.Comunication.Protocol.IO_Processing.ToString()));
                        if (Global.ParaCommon.Comunication.Protocol.IO_Processing == IO_Processing.ByPass)
                            Global.EditTool.lbBypass.ForeColor = Color.Green; 
                        await Global.ParaCommon.Comunication.Protocol.WriteIO();
                        IO_ProcessingOld = Global.ParaCommon.Comunication.Protocol.IO_Processing;
                        lbWrite.Text = Math.Round(Global.ParaCommon.Comunication.Protocol.CTWrite) + "";
                       
                    }
                }
              
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
              
                        
                        //    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO_READ","Start.."));
                  
                    //    await Global.ParaCommon.Comunication.Protocol.Read();
                    //int ix = Global.ParaCommon.Comunication.Protocol. AddressInput[(int)I_O_Input.ByPass];
                    //if (ix > -1)
                    //{
                    //    if (Global.ParaCommon.Comunication.Protocol.valueInput[ix] == 1 && !Global.IsByPassResult)
                    //    {
                    //        Global.IsByPassResult = true;
                    //        Global.EditTool.lbBypass.Visible = true;
                            
                    //            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO_READ", "BYPASS"));
                    //    }
                    //    else if (Global.ParaCommon.Comunication.Protocol.valueInput[ix] == 0 && Global.IsByPassResult)

                    //    {
                    //        Global.IsByPassResult = false;
                    //        Global.EditTool.lbBypass.Visible = false;
                            
                    //            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO_READ", "NO BYPASS"));
                    //    }        
                    //}
                }    
              
                if (Global.StatusIO == StatusIO.Writing && Global.ParaCommon.Comunication.Protocol.IO_Processing == IO_Processing.None)
                    Global.StatusIO = StatusIO.None;
                //StatusIObtn.Text = Global.StatusIO.ToString();
            }
            else

            {
                Global.StatusProcessing = StatusProcessing.None;
                Global.StatusIO = StatusIO.None;
                Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.None;
            }


            Global.ParaCommon.Comunication.Protocol.CTMid = (Global.ParaCommon.Comunication.Protocol.CTMin + Global.ParaCommon.Comunication.Protocol.CTMax) / 2;
            lbRead.Text = Math.Round(Global.ParaCommon.Comunication.Protocol.CTRead) + "";
            lbMax.Text = Math.Round(Global.ParaCommon.Comunication.Protocol.CTMid) + "";
            lbWrite.Text = Math.Round(Global.ParaCommon.Comunication.Protocol.CTWrite) + "";


        }

        private async void tmConnect_Tick(object sender, EventArgs e)
        {if (!Global.Initialed) return;
            tmConnect.Enabled = false;
            if (Global.ParaCommon.Comunication.Protocol == null)Global.ParaCommon.Comunication.Protocol = new ParaProtocol();
            if (Global.ParaCommon.Comunication.Protocol.IsBypass) return;
         await   Global.ParaCommon.Comunication.Protocol.Connect();

            if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            {
                Global.StatusIO = StatusIO.None;
                Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Reset;
             await   Global.ParaCommon.Comunication.Protocol.WriteIO();
                await Task.Delay(500);
                tmRead.Enabled = true;
                if (Global.ParaCommon.Comunication.Protocol.timeRead == 0) Global.ParaCommon.Comunication.Protocol.timeRead = 1;
                tmRead.Interval = Global.ParaCommon.Comunication.Protocol.timeRead;
                //  tmCheck.Enabled = true;
                // G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;

            }

            else
            {
              //  G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                if (!Global.ParaCommon.Comunication.Protocol.IsBypass)
                {
                  await  Global.ParaCommon.Comunication.Protocol.Connect();
                    if (Global.ParaCommon.Comunication.Protocol.IsConnected)
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
               // Global.ParaCommon.Comunication.Protocol.Read();

            }
        }

        private async void  workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)

        {
           await Task.Delay( Global.ParaCommon.Comunication.Protocol.timeRead);
            if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                workRead.RunWorkerAsync();
            else
                tmConnect.Enabled = true;
            //if (Global.ParaCommon.Comunication.Protocol.logBuilder != null)
            //{
            //    txtLog1.Text = Global.ParaCommon.Comunication.Protocol.logBuilder.ToString();
            //    txtLog1.Refresh();
            //}


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.CTMin = 100000;
            Global.ParaCommon.Comunication.Protocol.CTMax = 0;
           
        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

    

   

  
        private void timerRead_ValueChanged(float obj)
        {
            Global.ParaCommon.Comunication.Protocol.timeRead = (int)timerRead.Value;
            tmRead.Interval = Global.ParaCommon.Comunication.Protocol.timeRead;
        }

      

        private void btnRTU_Click(object sender, EventArgs e)
        {
           
        }

      
        private void txtLog1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnMitsu_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.PlcBrand=PlcLib.PlcBrand.Mitsubishi;
            btnKeyence.IsCLick = false;
         
        }

        private void btnKeyence_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.PlcBrand = PlcLib.PlcBrand.Keyence;
            btnMitsu.IsCLick = false;
        }

        private void btnSerial_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.ConnectionType = PlcLib.ConnectionType.Serial;
            cbCom.Enabled = true;
            cbBaurate.Enabled = true;
            txtIP.Enabled = false;
            txtPort.Enabled = false;
            btnTCP.IsCLick = false;
            lbRTU1.Enabled = true;
            lbRTU2.Enabled = true;
            lbTCP1.Enabled = false;
            lbTCP2.Enabled = false;
        }

        private void btnTCP_Click_1(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.ConnectionType = PlcLib.ConnectionType.Tcp;
            cbCom.Enabled = false;
            cbBaurate.Enabled = false;
            txtIP.Enabled = true;
            txtPort.Enabled = true;
            btnSerial.IsCLick = false;
            lbRTU1.Enabled = false;
            lbRTU2.Enabled = false;
            lbTCP1.Enabled = true;
            lbTCP2.Enabled = true;
        }

        private void txtAddRead_TextChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.AddRead = txtAddRead.Text.Trim();
        }

        private void txtAddWrite_TextChanged(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.AddWrite = txtAddWrite.Text.Trim();
        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbParity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedValue.ToString()) ;
        }

        private void cbStopBits_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBits.SelectedValue.ToString());
        }

        private void cbDataBits_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbDataBits.SelectedValue == null)
                return;
            Global.ParaCommon.Comunication.Protocol.DataBit = Convert.ToInt32(cbDataBits.SelectedValue.ToString());
        }

        private void btnDtrEnable_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.DtrEnable = btnDtrEnable.IsCLick;
        }

        private void btnRtsEnable_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.RtsEnable = btnRtsEnable.IsCLick;
        }

        private void btnReScan_Click(object sender, EventArgs e)
        {
            cbCom.DataSource = SerialPort.GetPortNames();
        }
    }
}
