
using OpenCvSharp;
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
        [NonSerialized]
        private Timer _timer;

        //public void StartRead()
        //{
        //    _cts = new CancellationTokenSource();
        //    //if (valueInput == null)
        //    //    valueInput = new IntArrayWithEvent(16);
        //    //if (valueOutput == null)
        //    //    valueOutput = new IntArrayWithEvent(16);
        //    _timer = new Timer(_ => DoWork(), null, dueTime: 0, period: 10000);
        //}

        public void StopTimer()
        {
            _timer?.Dispose();
        }
        public void StartRead()
        {
            _cts = new CancellationTokenSource();
            Global.StatusIO = StatusIO.Reading;
            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                   await DoWork();                            // ========== Công việc chính
                  //  await Task.Delay(timeRead, _cts.Token);  // ========== Chờ 1s (1000 ms)
                }
            }, _cts.Token);
        }

        public void StopRead() => _cts.Cancel();
        public float DealayTrigger = 1;
        public float DealayOutput= 1;
        public bool IsLight1,IsLight2,IsLight3;
        private  async Task DoWork()
        {
            if (!Global.Initialed) return;
            if(Global.StatusIO == StatusIO.ErrRead)
            {
                //await Task.Delay(50);
                Global.StatusIO = StatusIO.Reading;
            }
            if (Global.StatusIO==StatusIO.Reading)
            {
                Read();
              
            }
           
                
           
        }
        public bool IsBusy = false;
        public String[] nameInput = new String[16];
        public String[] nameOutput = new String[16];
       
        [NonSerialized]
        public int[] valueInput = new int[16];
        [NonSerialized]
        public int[] valueOutput = new int[16];
        [NonSerialized]
        public int[] AddressInput=new int[30];
        public int[] AddressOutPut = new int[30];
        public int[] LenReads;
        public String Port = "COM8";
        public int Baurate = 115200;
        public byte SlaveID=1;
        public bool IsBypass=true;
        public IO() { }
        public int timeRead = 0;
        public List<ParaIO> paraIOs = new List<ParaIO>();
        [NonSerialized]
        public bool IsConnected = false,IsWriting=false;
        public void Arrange()
        {
          
            foreach (I_O_Input DI in Enum.GetValues(typeof(I_O_Input)))
            {
                int index = paraIOs.FindIndex(a => a.I_O_Input== DI && a.TypeIO == TypeIO.Input);
                int value = -1;
                if (index > -1) value = paraIOs[index].Adddress;
                AddressInput[(int)(DI)] = value;
            }
            foreach (I_O_Output DO in Enum.GetValues(typeof(I_O_Output)))
            {
                int index = paraIOs.FindIndex(a => a.I_O_Output== DO && a.TypeIO == TypeIO.Output);
                int value = -1;
                if (index > -1) value = paraIOs[index].Adddress;
                AddressOutPut[(int)(DO)] = value;
            }
        }
        public bool AddInPut(int index,I_O_Input Input)
        {   int ix= paraIOs.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Input && a.I_O_Input != Input);
            if (paraIOs.FindIndex(a=>a.Adddress== index && a.TypeIO==TypeIO.Input)==-1)
            {
                paraIOs.Add(new ParaIO(TypeIO.Input, Input, index)); Arrange();
                return true;
            }
            else if (ix >= 0)
            {
                paraIOs.RemoveAt(ix);
                paraIOs.Add(new ParaIO(TypeIO.Input, Input, index)); Arrange();
                return true;
            }
            else
                return false;
        }
        public bool AddOutPut(int index, I_O_Output Output)
        {
            if (paraIOs.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Output) == -1)
            {
                paraIOs.Add(new ParaIO(TypeIO.Output, Output, index)); Arrange();
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
                paraIOs.RemoveAt(indexs); Arrange();
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
                paraIOs.RemoveAt(indexs); Arrange();
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
                if (valueInput == null)
                    valueInput = new int[16];
                if (valueOutput == null)
                    valueOutput = new int[16];
                if (AddressInput == null)
                    AddressInput = new int[30];
                if (AddressOutPut == null)
                    AddressOutPut = new int[30];
                
                Arrange();
                if (IsConnected) 
                    valueInput=Modbus.ReadBit(1);
                if (valueInput.Length < 16)
                {
                    IsConnected = false;
                    return false;
                }
                    
            }
            catch(Exception ex)
            {
                return false;
            }
            //  Modbus.ReadHolding(0, 10);
           
            return IsConnected;
        }
        [field: NonSerialized]
        private StringBuilder _logBuilder = new StringBuilder();
        [field: NonSerialized]
        public event Action<StringBuilder> LogChanged;
        public StringBuilder logBuilder
        {
            get => _logBuilder;
           
            set
            {
                        if (_logBuilder != value)
                {
                    _logBuilder = value ?? new StringBuilder();
                    LogChanged?.Invoke(_logBuilder);
                }
            }
        }
        public void LogError(string message)
        {if (logBuilder == null) logBuilder = new StringBuilder();
            string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}";
            logBuilder.AppendLine(logLine);
            LogChanged?.Invoke(_logBuilder);
        }
        public void Disconnect()
        {
          //  StopRead();
             Modbus.DisconnectPLC();
            Global.StatusIO = StatusIO.NotConnect;
            Global.StatusMode = StatusMode.None;
            IsConnected = false;
        }
    
        bool IsCanWrite = false;
        public  bool Read()
        {
            if (!IsConnected) return false;
            
           
             valueInput=Modbus.ReadBit(1);
            for (int i = 0; i < valueInput.Length; i++)
            {
                int ix = paraIOs.FindIndex(a => a.Adddress == i && a.TypeIO == TypeIO.Input);
                if (ix >= 0)
                {
                    paraIOs[ix].Value = valueInput[i];
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
            
            switch (Processing )
            {
                case IO_Processing.Trigger:
                    Global.StatusIO = StatusIO.Writing;
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Light1], true);//LIGHT 1
                    SetOutPut(AddressOutPut[(int)I_O_Output.Light2], true);//LIGHT 2
                   await WriteOutPut();
                    break;
                case IO_Processing.Close:
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //T.Result
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Light1], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Light2], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic1], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic2], false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic3 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic4 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true); //Busy

                    await WriteOutPut();
                    Disconnect();
                    break;
                case IO_Processing.Error:
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//CCD Err
                    await WriteOutPut();
                    break;
                case IO_Processing.NoneErr:
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false);//CCD Err
                    await WriteOutPut();
                    break;
                case IO_Processing.Result:
                    if (Is)
                    {
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                       SetOutPut(AddressOutPut[(int)I_O_Output.Light1], false); //Light
                       SetOutPut(AddressOutPut[(int)I_O_Output.Light2], false); //Light

                       SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                        await WriteOutPut();
                        await Task.Delay(Delay);
                       SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true);//Ready false
                       SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                        await WriteOutPut();
                    }
                    else
                    {

                        if (valueInput[3] == 1) //Bypass
                        {
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                           SetOutPut(AddressOutPut[(int)I_O_Output.Light1], false); //Light
                            SetOutPut(AddressOutPut[(int)I_O_Output.Light2], false); //Light
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                            await WriteOutPut();
                           await Task.Delay(Delay);
                           SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true);//Ready false
                           SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //OK
                            await WriteOutPut();
                        }
                        else
                        {
                           SetOutPut(AddressOutPut[(int)I_O_Output.Result], true); //NG
                           SetOutPut(AddressOutPut[(int)I_O_Output.Light1], false); //Light
                           SetOutPut(AddressOutPut[(int)I_O_Output.Light2], false); //Light2
                           SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                          // await WriteOutPut();
                           //await Task.Delay(1000);
                           SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true);//Ready false
                         // SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //False
                            await WriteOutPut();
                        }
                    }
                    valueInput = new int[16]; 
                    Global.StatusProcessing = StatusProcessing.Done;
                    Global.StatusIO = StatusIO.Reading;
                    break;
                case IO_Processing.ChangeMode:
                    if(Is)
                   {
                       SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                        await WriteOutPut();
                    }
                    else
                    {
                       SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true); //Not Busy
                        await WriteOutPut();
                    }
                        break;
                case IO_Processing.Light:
                       SetOutPut(AddressOutPut[(int)I_O_Output.Light1], Is); //Busy
                       SetOutPut(AddressOutPut[(int)I_O_Output.Light2], Is); //Busy

                    await WriteOutPut();
                    break;
                case IO_Processing.ChangeProg:
                   SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true); //Busy
                    await WriteOutPut();
                    break;
                case IO_Processing.Reset:
                   SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //Ready
                   SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                   SetOutPut(AddressOutPut[(int)I_O_Output.Error], false); //Err
                    await WriteOutPut();
                    break;

            }
            IsWriting = false;
        }
        public bool CheckReady()
        {
        if (valueInput[AddressInput[(int)I_O_Input.Trigger] ]== 1&&Global.StatusProcessing==StatusProcessing.None)
            {
                return true;
            }
            else
            {
              
                return false;
            }
            return false;
        }
        public async Task<bool> CheckErr(bool IsCameraConnected)
        {
            if (!IsCameraConnected)
            {
                if (valueOutput[paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1] == 0)
                {
                   SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//CCD Err
                   await WriteOutPut();
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
                  await WriteOutPut();
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
            IsConnected = Modbus.WritePLC(Add,Convert.ToInt16(Value));//AddressStarts[0]+

            IsWriting = false;
            return IsConnected;
        }
        public void SetOutPut(int Add, bool Value)
        {
            if (!IsConnected) return;
            if (Add < 0) return;
            valueOutput[Add] = Convert.ToInt32(Value);
         int ix=   paraIOs.FindIndex(a => a.Adddress ==Add && a.TypeIO == TypeIO.Output);
            if (ix >= 0)
            {
                paraIOs[ix].Value =Convert.ToInt32( Value);
            }
            // Mảng bit (16 bit: 0 hoặc 1), bit 15 là MSB, bit 0 là LSB
           
        }
        public async Task<bool> WriteOutPut()
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
        X: IsConnected = Modbus.WriteBit(Val);
           if(Global.StatusIO == StatusIO.ErrWrite)
            {
                await Task.Delay(50);
                Global.StatusIO = StatusIO.Writing;
                goto X;
            }
           
            IsWriting = false;
            return IsConnected;
        }
    }
}
