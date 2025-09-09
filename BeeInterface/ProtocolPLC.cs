using BeeCore;
using BeeCore.Func;
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

namespace BeeInterface
{
    public partial class ProtocolPLC : UserControl
    {
        List<String> OldIn = new List<string>(16);
        List<String> OldOut= new List<string>(16);
        List<ComboBox> listCbIn = new List<ComboBox>();
        List<ComboBox> listCbOut = new List<ComboBox>();
        public ProtocolPLC()
        {
            InitializeComponent();
           
            OldIn = new string[16].ToList();
            OldOut = new string[16].ToList();
            comIO.DataSource = SerialPort.GetPortNames();
            Parallel.For(0, 16, i =>
            {
                RefreshComboBoxIn(i, "");
            });
            Parallel.For(0, 16, i =>
            {
                RefreshComboBoxOut(i, "");
            });
            foreach(Control ctl in layIn.Controls)
            {
                if (ctl is ComboBox cb)
                    listCbIn.Add(cb);
            }
            foreach (Control ctl in layOut.Controls)
            {
                if (ctl is ComboBox cb)
                    listCbOut.Add(cb);
            }
            int ix = 0;
            foreach (ComboBox cb in listCbIn)
            {
               cb.DataSource = listIn[ix];
                ix++;
            }
            ix = 0;
            foreach (ComboBox cb in listCbOut)
            {
                cb.DataSource = listOut[ix];
                ix++;
            }
           
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
              //  if (Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a=>a.I_O_Input==io)==-1|| text.Contains(io.ToString()))
                    listIn[index].Add(io.ToString());
            }

        }
        private void RefreshComboBoxOut(int index, String text)
        {

            listOut[index] = new List<string>();
            List<String> list = new List<string>();
            foreach (I_O_Output io in Enum.GetValues(typeof(I_O_Output)))
            {
             //   if (Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == io) == -1 || text.Contains(io.ToString()))
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
        private void RefreshComboBoxOut(ComboBox cb)
        {

            cb.Items.Clear();
            foreach (I_O_Output io in Enum.GetValues(typeof(I_O_Output)))
            {
                if (Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == io) == -1)
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
         
            var ParaBits = Global.ParaCommon.Comunication.Protocol.ParaBits;

            cbO0.Text = ParaBits.Find(x => x.Adddress == 0&&x.TypeIO==TypeIO.Output)? .I_O_Output .ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO1.Text = ParaBits.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO2.Text = ParaBits.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbO3.Text = ParaBits.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO4.Text = ParaBits.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO5.Text = ParaBits.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO6.Text = ParaBits.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbO7.Text = ParaBits.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Output)?.I_O_Output.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn0.Text = ParaBits.Find(x => x.Adddress == 0 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString();   // giữ nguyên text cũ nếu không tìm thấy
            cbIn1.Text = ParaBits.Find(x => x.Adddress == 1 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn2.Text = ParaBits.Find(x => x.Adddress == 2 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn3.Text = ParaBits.Find(x => x.Adddress == 3 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn4.Text = ParaBits.Find(x => x.Adddress == 4 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn5.Text = ParaBits.Find(x => x.Adddress == 5 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn6.Text = ParaBits.Find(x => x.Adddress == 6 && x.TypeIO == TypeIO.Input)?.I_O_Input.ToString() ;   // giữ nguyên text cũ nếu không tìm thấy
            cbIn7.Text = ParaBits.Find(x => x.Adddress == 7 && x.TypeIO == TypeIO.Input) ? .I_O_Input.ToString();      // giữ nguyên text cũ nếu không tìm thấy
            timerRead.Value=Global.ParaCommon.Comunication.Protocol.timeRead;
            cbBaurate.Text = Global.ParaCommon.Comunication.Protocol.Baurate + "";
           
            comIO.Text = Global.ParaCommon.Comunication.Protocol.ComSerial;
            
                txtAddRead.Text=Global.ParaCommon.Comunication.Protocol.AddRead;
            txtAddWrite.Text = Global.ParaCommon.Comunication.Protocol.AddWrite;
                listLabelsIn = new List<RJButton> { DI0, DI1, DI2, DI3, DI4, DI5, DI6, DI7 };
                listLabelsOut = new List<RJButton> { DO0, DO1, DO2, D3, DO4, DO5, DO6, DO7 };
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
               // MessageBox.Show(ex.Message);
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
            comIO.DataSource = SerialPort.GetPortNames();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData.Config(Global.Config);

        }

       

        private void cbIn1_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
            String name = cbIn0.SelectedValue.ToString(); if (name == "") return;
            if (cbIn0.Text.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(0, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[0], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(0, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[0] = name;
            }
            ChangeDatasource(0, name);

        }

        private void cbIn2_SelectionChangeCommitted(object sender, EventArgs e)
        {
          
            String name = cbIn1.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(1, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[1], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(1, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[1] = name;
            }
            ChangeDatasource(1, name);
        }

        private void cbIn3_SelectionChangeCommitted(object sender, EventArgs e)
        {
          
            String name = cbIn2.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(2, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[2], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(2, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[2] = name;
            }
            ChangeDatasource(2, name);
        }

        private void cbIn4_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
            String name = cbIn3.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(3, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[3], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(3, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[3] = name;
            }
            ChangeDatasource(3, name);
        }

        private void cbIn5_SelectionChangeCommitted(object sender, EventArgs e)
        {
      
            String name = cbIn4.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(4, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[4], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(4, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[4] = name;
            }
            ChangeDatasource(4, name);
        }

        private void cbIn6_SelectionChangeCommitted(object sender, EventArgs e)
        {
         
            String name = cbIn5.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(5, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[5], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(5, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[5] = name;
            }
            ChangeDatasource(5, name);
        }

        private void cbIn7_SelectionChangeCommitted(object sender, EventArgs e)
        {
       
            String name = cbIn6.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(6, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[6], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(6, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[6] = name;
            }
            ChangeDatasource(6, name);
        }

        private void cbIn8_SelectionChangeCommitted(object sender, EventArgs e)
        {

            String name = cbIn7.SelectedValue.ToString(); if (name == "") return;
            if (name.Contains("None"))
            {
                Global.ParaCommon.Comunication.Protocol.RemoveInPut(7, (I_O_Input)Enum.Parse(typeof(I_O_Input), OldIn[7], ignoreCase: true));

            }
            else
            {
                Global.ParaCommon.Comunication.Protocol.AddInPut(7, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true));
                OldIn[7] = name;
            }
            ChangeDatasource(7, name);
        }
        public String[] nameInput =new  String[16];
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
        public String[] nameOut = new String[16];
        private void cbOut_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb=  sender  as ComboBox;
            String name = cb.SelectedValue.ToString();
            var m = Regex.Match(cb.Name, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42
            if (name == "") return;
            if (cb.Text.Contains("None"))
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
            int index = Global.ParaCommon.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Output == (I_O_Output)Enum.Parse(typeof(I_O_Output), nameOut[value], ignoreCase: true) && a.TypeIO == TypeIO.Output);
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
            Global.ParaCommon.Comunication.Protocol.ComSerial = comIO.SelectedValue.ToString() ;
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
              
                        
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO_READ","Start.."));
                  
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
            comIO.Enabled = true;
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
            comIO.Enabled = false;
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
    }
}
