
using OpenCvSharp;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeGlobal
{
    [Serializable()]
    public class IO
    {
        [NonSerialized]
        private  CancellationTokenSource _cts = new CancellationTokenSource
            ();
        //[NonSerialized]
        //private Timer _timer;

        //public void StartRead()
        //{
        //    _cts = new CancellationTokenSource();
        //    //if (valueInput == null)
        //    //    valueInput = new IntArrayWithEvent(16);
        //    //if (valueOutput == null)
        //    //    valueOutput = new IntArrayWithEvent(16);
        //    _timer = new Timer(_ => DoWork(), null, dueTime: 0, period: 10000);
        //}

        //public void StopTimer()
        //{
        //    _timer?.Dispose();
        //}
        //public void StartRead()
        //{
        //    _cts = new CancellationTokenSource();
        //    Global.StatusIO = StatusIO.Reading;
        //    Task.Run(async () =>
        //    {
        //        while (!_cts.Token.IsCancellationRequested)
        //        {
        //           await DoWork();                            // ========== Công việc chính
        //          //  await Task.Delay(timeRead, _cts.Token);  // ========== Chờ 1s (1000 ms)
        //        }
        //    }, _cts.Token);
        //}

        //public void StopRead() => _cts.Cancel();
        public int AddRead = 1;
        public int AddWrite = 2;
        public float DelayTrigger = 1;
        public float DelayOutput= 1;
        public bool IsLight1,IsLight2,IsLight3;
        //private  async Task DoWork()
        //{
        //    if (!Global.Initialed) return;
        //    if(Global.StatusIO == StatusIO.ErrRead)
        //    {
        //        //await Task.Delay(50);
        //        Global.StatusIO = StatusIO.Reading;
        //    }
        //    if (Global.StatusIO==StatusIO.Reading)
        //    {
        //        Read();
              
        //    }
           
                
           
        //}
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
        public IO() {
            if (AddRead == 0 && AddWrite == 0)
            {
                AddRead = 1;
                AddWrite = 2;
            }
        }
        public int timeRead = 0;
        public List<ParaIO> paraIOs = new List<ParaIO>();
        [NonSerialized]
        public bool IsConnected = false,IsWriting=false;
        public void Arrange()
        {
            if (valueInput == null)
                valueInput = new int[16];
            if (valueOutput == null)
                valueOutput = new int[16];
            if (AddressInput == null)
                AddressInput = new int[30];
            if (AddressOutPut == null)
                AddressOutPut = new int[30];

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
            int ix = paraIOs.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Output);
            if(ix == -1)
            {
                paraIOs.Add(new ParaIO(TypeIO.Output, Output, index)); Arrange();
               
            }
            else
            {
                paraIOs[ix].I_O_Output = Output;
            }    
                return true;
           
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
        public async Task<bool> Connect(  )
        {

            try
            {
                if (AddRead == 0 && AddWrite == 0)
                {
                    AddRead = 1;
                    AddWrite = 2;
                }

                CT = new Stopwatch();
                CTMid = 0;
                CTMin = 1000; CTMax = 0;
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
                    valueInput=await Modbus.ReadBit(1);
                if (valueInput.Length < 16)
                {
                    IsConnected = false;
                    return false;
                }
                    foreach(ParaIO paraIO in paraIOs)
                {
                    paraIO.Value = 0;
                }
            }
            catch(Exception ex)
            {
                Global.Ex = "CON_IO_" + ex.Message;
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
        [NonSerialized]
        Stopwatch CT =new Stopwatch();
        [NonSerialized]
       public float CTMid = 0,CTMin=0,CTMax=0;
        public float CTRead,CTWrite;
        public  async Task Read()
        {

            if (!IsConnected) return ;
            numRead++;
                CT.Restart();
            Global.StatusIO = StatusIO.Reading;
            valueInput =await Modbus.ReadBit(AddRead);
            Global.StatusIO = StatusIO.None;
            CT.Stop();
            
            CTRead = (float) CT.Elapsed.TotalMilliseconds;
           if(CTRead > CTMax)
                CTMax = CTRead;
            if (CTRead < CTMin)
                CTMin = CTRead;

          for(int i=0; i<valueInput.Length; i ++)
              {
                  int ix = paraIOs.FindIndex(a => a.Adddress == i && a.TypeIO == TypeIO.Input);
                  if (ix >= 0)
                  {
                      paraIOs[ix].Value = valueInput[i];
                  }
              }
            numRead--;
            //for (int i = 0; i < valueInput.Length; i++)
            //{
            //    int ix = paraIOs.FindIndex(a => a.Adddress == i && a.TypeIO == TypeIO.Input);
            //    if (ix >= 0)
            //    {
            //        paraIOs[ix].Value = valueInput[i];
            //    }
            //}


            //   valueOutput = Modbus.ReadBit(2);
            //  valueInput = new int[dataBytes.Length / 2];

            // valueInput =  Modbus.ReadHolding(AddressStarts[0]);
            //  if(IsReadOut)
            //valueOutput = Modbus.ReadHolding(AddressStarts[1]);
            //if (valueInput.Count()==1||valueOutput.Count()==1) IsConnected = false;
            return ;
        }
        bool IsWait = false;
        public IO_Processing _IO_Processing = IO_Processing.None;
        public IO_Processing IO_Processing
        {
            get => _IO_Processing;

            set
            {
                if (_IO_Processing != value)
                {
                   _IO_Processing = value;
                    
                //    ;
                //    WriteIO(_IO_Processing);
               }
            }
        }
        private void SetLight(bool IsOn)
        {
            if (IsLight1)
                SetOutPut(AddressOutPut[(int)I_O_Output.Light1], IsOn);
            if (IsLight2)
                SetOutPut(AddressOutPut[(int)I_O_Output.Light2], IsOn);
            if (IsLight3)
                SetOutPut(AddressOutPut[(int)I_O_Output.Light3], IsOn);
        }
        public bool IsBlink = false;
        public bool IsLogic1, IsLogic2, IsLogic3, IsLogic4, IsLogic5, IsLogic6;
        public async Task<bool>  WriteIO()
        {   //if (!IsConnected) return false;

            if (IO_Processing == IO_Processing.None)
            {

                return false;
            }
            Global.StatusIO = StatusIO.Writing;

            switch (IO_Processing )
            {
                case IO_Processing.Trigger:
                    
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                    SetLight(true);
                    await WriteOutPut();
                    Global.StatusMode = StatusMode.Once;
                    await Task.Delay((int)DelayTrigger);
                    Global.StatusProcessing = StatusProcessing.Read;
                    break;
                case IO_Processing.Close:
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Result && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //T.Result
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false); //Ready
                  
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic1], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic2], false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic3 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(paraIOs.Find(a => a.I_O_Output == I_O_Output.Logic4 && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true); //Busy
                    await WriteOutPut();
                    Disconnect();
                    break;
                case IO_Processing.ByPass:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true);//Ready false
                    SetLight(false);
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                    IsWait = true;
                    await WriteOutPut();
                    if (IsBlink)
                    {
                        await Task.Delay((int)DelayOutput);
                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG                           // SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //False
                        await WriteOutPut();
                    }
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
                    bool IsOK = false;
                    if (Global.TotalOK)
                    {
                        IsOK = true;
                    }
                    else
                    {
                        int ix = AddressInput[(int)I_O_Input.ByPass];
                        if (ix > -1)
                        {
                            if (valueInput[ix] == 1)
                            {
                                IsOK = true;
                            }
                        }

                     }
                        if (IsOK)
                    {
                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG
                        SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true);//Ready false
                        SetLight(false);
                        SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                        IsWait = true;
                        await WriteOutPut();
                        if(IsBlink)
                        {
                            await Task.Delay((int)DelayOutput);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG                           // SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //False
                            await WriteOutPut();
                        }
                      
                    }
                    else
                    {
                      
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result], true); //NG
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true);//Ready false
                            SetLight(false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                            await WriteOutPut();
                            if (IsBlink)
                            {
                                await Task.Delay((int)DelayOutput);
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG                           // SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //False
                                await WriteOutPut();
                            }




                    }
                    Global.NumSend++;
                    Global.StatusProcessing = StatusProcessing.Drawing;
                    break;
                case IO_Processing.ChangeMode:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], !Global.IsRun); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], Global.IsRun); //Ready
                    SetLight(false);

                    SetOutPut(AddressOutPut[(int)I_O_Output.Error], false); //Err
                    Global.StatusProcessing= StatusProcessing.None;
                    IO_Processing = IO_Processing.None;
                    await WriteOutPut();
                   
                        break;
                case IO_Processing.Light:
                    SetLight(Global.ParaCommon.IsOnLight);
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
                   SetLight(false);
                    await WriteOutPut();
                   break;

            }
            valueInput = new int[16];
            IO_Processing = IO_Processing.None;
           
            // await Task.Delay(timeRead);
            return false;
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
        public int _numWrite = 0;
        public int _numRead = 0;
        [field: NonSerialized]
        public event Action<int> numReadChanged;
        public int numRead
        {
            get => _numRead;

            set
            {
                if (_numRead != value)
                {
                    _numRead = value ;
                    numReadChanged?.Invoke(_numRead);
                }
            }
        }
        [field: NonSerialized]
        public event Action<int> numWriteChanged;
        public int numWrite
        {
            get => _numWrite;

            set
            {
                if (_numWrite != value)
                {
                    _numWrite = value;
                    numWriteChanged?.Invoke(_numWrite);
                }
            }
        }
        public async Task<bool> WriteOutPut()
        {
            numWrite++;
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
            if (!IsConnected)
                return false;
            CT.Restart();
            X: IsConnected =await Modbus.WriteBit(AddWrite, Val);
           if(Global.StatusIO == StatusIO.ErrWrite)
            {
                await Task.Delay(50);
                Global.StatusIO = StatusIO.Writing;
                goto X;
            }
            numWrite--;
            CT.Stop();
            CTWrite = (float)CT.Elapsed.TotalMilliseconds;
            if (CTWrite > CTMax)
                CTMax = CTWrite;
            if (CTWrite < CTMin)
                CTMin = CTWrite;
            return IsConnected;
        }
    }
}
