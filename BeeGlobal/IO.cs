
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class IO
    {
        [NonSerialized]
        private  CancellationTokenSource _cts = new CancellationTokenSource
            ();

        public void StartRead()
        {
            _cts = new CancellationTokenSource();
            if (valueInput == null)
                valueInput = new IntArrayWithEvent(16);
            if (valueOutput == null)
                valueOutput = new IntArrayWithEvent(16);
            // Khởi chạy task nền
            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    DoWork();                            // ========== Công việc chính
                    await Task.Delay(timeRead, _cts.Token);  // ========== Chờ 1s (1000 ms)
                }
            }, _cts.Token);
        }

        public void StopRead() => _cts.Cancel();

        private async void DoWork()
        {
            Read();
            if (IsConnected)
            {

              
                if (Global.IsSendRS)
                {
                    WriteIO(IO_Processing.Result, Global.TotalOK, Global.Config.DelayOutput);
                  
                    Global.IsSendRS = false;
                }
                if (!Global.IsRun)
                    if (Global.ParaCommon.IsOnLight != Convert.ToBoolean(valueOutput[5]))
                    {
                        WriteIO(IO_Processing.Light, Global.ParaCommon.IsOnLight);
                    }


              

                if (Global.IsRun && Global.Config.IsExternal)
                {
                    if (CheckReady())
                    {Global.StatusProcessing= StatusProcessing.Trigger;
                        WriteIO(IO_Processing.Trigger);
                        Global.StatusMode = StatusMode.Once;
                        await Task.Delay(Global.Config.delayTrigger);
                        Global.StatusProcessing = StatusProcessing.Read;


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


                //G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
            }

            Console.WriteLine($"Work at {DateTime.Now:HH:mm:ss.fff}");
        }
        public bool IsBusy = false;
        public String[] nameInput = new String[16];
        public String[] nameOutput = new String[16];
        [NonSerialized]
        public IntArrayWithEvent valueInput = new IntArrayWithEvent(16);
        [NonSerialized]
        public IntArrayWithEvent valueOutput = new IntArrayWithEvent(16);


        public int[] AddressStarts;
        public int[] LenReads;
        public String Port = "COM8";
        public int Baurate = 115200;
        public byte SlaveID=1;
        public bool IsBypass=true;
        public IO() { }
        public int timeRead = 0;
        public List<ParaIO> paraIOs = new List<ParaIO>();
        public bool IsConnected = false,IsWriting=false;
        public bool AddInPut(int index,I_O_Input Input)
        {   int ix= paraIOs.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Input && a.I_O_Input != Input);
            if (paraIOs.FindIndex(a=>a.Adddress== index && a.TypeIO==TypeIO.Input)==-1)
            {
                paraIOs.Add(new ParaIO(TypeIO.Input, Input, index));
                return true;
            }
            else if (ix >= 0)
            {
                paraIOs.RemoveAt(ix);
                paraIOs.Add(new ParaIO(TypeIO.Input, Input, index));
                return true;
            }
            else
                return false;
        }
        public bool AddOutPut(int index, I_O_Output Output)
        {
            if (paraIOs.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Output) == -1)
            {
                paraIOs.Add(new ParaIO(TypeIO.Output, Output, index));
                return true;
            }
            else
                return false;
           
        }
        public bool RemoveInPut(int index, I_O_Input Input)
        {
            int indexs = paraIOs.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Input);
            if (indexs>=0)
            {
                paraIOs.RemoveAt(indexs);
                return true;
            }
            else
                return false;
        }
        public bool RemoveOutPut(int index, I_O_Output Output)
        {
            int indexs = paraIOs.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Output);
            if (indexs >= 0)
            {
                paraIOs.RemoveAt(indexs);
                return true;
            }
            else
                return false;
          
        }
        public bool Connect(  )
        {

            try
            {
                IsConnected = Modbus.ConnectPLC(Port, Baurate, SlaveID);
                if (IsConnected) valueOutput.ReplaceAll(Modbus.ReadBit(2));
            }
            catch(Exception)
            {
                return false;
            }
            //  Modbus.ReadHolding(0, 10);
           
            return IsConnected;
        }
        public void Disconnect()
        {
             Modbus.DisconnectPLC();
            IsConnected = false;
        }
        public  bool Read()
        {
            if (!IsConnected) return false;
            if (IsWriting)
            {
               return false;
            }
             valueInput.ReplaceAll( Modbus.ReadBit(1));
            for(int i=0;i<valueInput.Length; i++)
            {
                int index = paraIOs.FindIndex(a => a.Adddress == i);
                if(index>-1)
                {
                    paraIOs[index].Value=valueInput[i];
                }    
            }
          
            //   valueOutput = Modbus.ReadBit(2);
            //  valueInput = new int[dataBytes.Length / 2];

            // valueInput =  Modbus.ReadHolding(AddressStarts[0]);
            //  if(IsReadOut)
            //valueOutput = Modbus.ReadHolding(AddressStarts[1]);
            //if (valueInput.Count()==1||valueOutput.Count()==1) IsConnected = false;
            return IsConnected;
        }
        public IO_Processing IO_Processing = IO_Processing.None;
        public async void WriteIO(IO_Processing Processing,bool Is=false,int Delay=1)
        {   if (!IsConnected) return;
            IsWriting = true;
            switch (Processing )
            {
                case IO_Processing.Trigger:
         
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Ready && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false);//Ready false
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light1 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//LIGHT 1
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light2 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//LIGHT 2
                    WriteOutPut();
                    break;
                case IO_Processing.Close:
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //T.Result
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Ready && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Ready
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light1 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light2 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic1 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic2 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic3 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic4 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true); //Busy
                  
                    WriteOutPut();
                    Disconnect();
                    break;
                case IO_Processing.Error:
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//CCD Err
                   WriteOutPut();
                    break;
                case IO_Processing.NoneErr:
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false);//CCD Err
                   WriteOutPut();
                    break;
                case IO_Processing.Result:
                    if (Is)
                    {
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light1 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Light
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light2 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Light

                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                       WriteOutPut();
                        await Task.Delay(Delay);
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Ready && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//Ready false
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                       WriteOutPut();
                    }
                    else
                    {

                        if (valueInput[3] == 1) //Bypass
                        {
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light1 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Light
                            SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light2 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Light
                            SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                           WriteOutPut();
                           await Task.Delay(Delay);
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Ready && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//Ready false
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                           WriteOutPut();
                        }
                        else
                        {
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true); //NG
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light1 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Light
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light2 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Light2
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                           WriteOutPut();
                            await Task.Delay(Delay);
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Ready && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//Ready false
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //False
                           WriteOutPut();
                        }
                    }
                    break;
                case IO_Processing.ChangeMode:
                    if(Is)
                   {
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                       WriteOutPut();
                    }
                    else
                    {
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true); //Not Busy
                       WriteOutPut();
                    }
                        break;
                case IO_Processing.Light:
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light1 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, Is); //Busy
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Light2 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, Is); //Busy

                       WriteOutPut();
                    break;
                case IO_Processing.ChangeProg:
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true); //Busy
                   WriteOutPut();
                    break;
                case IO_Processing.Reset:
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Ready && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true); //Ready
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Err
                   WriteOutPut();
                    break;

            }
            IsWriting = false;
        }
        public bool CheckReady()
        {
        if (valueInput[paraIOs.Find(a => a.I_O_Input == I_O_Input.Trigger && a.TypeIO == TypeIO.Input)?.Adddress ?? -1] == 1&& valueOutput[paraIOs.Find(a => a.I_O_Output == I_O_Output.Busy && a.TypeIO == TypeIO.Output)?.Adddress ?? -1] == 0)
            {
                return true;
            }
            else
            {
              
                return false;
            }
            return false;
        }
        public bool CheckErr(bool IsCameraConnected)
        {
            if (!IsCameraConnected)
            {
                if (valueOutput[paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1] == 0)
                {
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//CCD Err
                   WriteOutPut();
                    return false;
                }
                return true;
            }
            else
            {
                if (valueOutput[paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1] == 1)
                {
                   IO_Processing = IO_Processing.Error;
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false);//CCD Err
                   WriteOutPut();
                    return true;
                }
                return true;
            }
            return false;
        }
        public int ReadPara(int Add )
        {
      
           return Modbus.ReadHolding(Add,1)[0];
        
        }
        public bool WritePara(int Add, int Value)
        {
            IsWriting = true;
            IsConnected = Modbus.WritePLC(Add, Value);

            IsWriting = false;
            return IsConnected;
        }
        public   bool WriteInPut(int Add,bool Value)
        {
            IsWriting = true;
            IsConnected = Modbus.WritePLC(AddressStarts[0]+Add,Convert.ToInt16(Value));

            IsWriting = false;
            return IsConnected;
        }
        public void SetOutPut(int Add, bool Value)
        {
            if (Add < 0) return;
            valueOutput[Add] = Convert.ToInt32(Value);
           
            // Mảng bit (16 bit: 0 hoặc 1), bit 15 là MSB, bit 0 là LSB
           
        }
        public bool WriteOutPut()
        {
            int[] bitArray = new int[16] {
                0, 0, 0, 0, 0, 0, 0, 0,   // Bit 15 đến 8
                0, 0, 0, 0, 0, 0, 0, 0   // Bit 7 đến 0
            };
            for (int i = 0; i < 16; i++)
            {
                bitArray[15 - i] = valueOutput[i]; // bit 15 là MSB

            }
            int Val = 0;

            for (int i = 0; i < 16; i++)
            {
                Val |= (bitArray[i] & 1) << (15 - i); // bit 15 là MSB

            }
            IsWriting = true;
            IsConnected = Modbus.WriteBit(Val);

            IsWriting = false;
            return IsConnected;
        }
    }
}
