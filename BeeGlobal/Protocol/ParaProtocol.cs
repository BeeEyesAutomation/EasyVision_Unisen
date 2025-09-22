
using OpenCvSharp;
using OpenCvSharp.Flann;
using OpenCvSharp.ML;
using PlcLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
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
    public class ParaProtocol
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
        public String AddRead = "D100";
        public String AddWrite = "D102";
        public float DelayTrigger = 1;
        public float DelayOutput= 1;
        public bool IsLight1,IsLight2,IsLight3;
        [NonSerialized]
        public ModbusService ModbusService;
        public PlcBrand PlcBrand = PlcBrand.Mitsubishi;
        public ConnectionType ConnectionType = ConnectionType.Serial;
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
        public bool[] valueInput = new bool[16];
        [NonSerialized]
        public bool[] valueOutput = new bool[16];
        [NonSerialized]
        public int[] AddressInput=new int[30];
        public int[] AddressOutPut = new int[30];
        public int[] LenReads;
        public String ComSerial = "COM8";
        public int PortIP = 502;
        public int Baurate = 115200;
        public byte SlaveID=1;
        public bool IsBypass=true;
        public ParaProtocol() {
            //if (AddRead == 0 && AddWrite == 0)
            //{
            //    AddRead = 1;
            //    AddWrite = 2;
            //}
        }
        public int timeRead = 0;
        public List<ParaBit> ParaBits = new List<ParaBit>();
        [NonSerialized]
        public bool IsConnected = false,IsWriting=false;
        public void Arrange()
        {
            if (valueInput == null)
                valueInput = new bool[16];
            if (valueOutput == null)
                valueOutput = new bool[16];
            if (AddressInput == null)
                AddressInput = new int[30];
            if (AddressOutPut == null)
                AddressOutPut = new int[30];

            foreach (I_O_Input DI in Enum.GetValues(typeof(I_O_Input)))
            {
                int index = ParaBits.FindIndex(a => a.I_O_Input== DI && a.TypeIO == TypeIO.Input);
                int value = -1;
                if (index > -1) value = ParaBits[index].Adddress;
                AddressInput[(int)(DI)] = value;
            }
            foreach (I_O_Output DO in Enum.GetValues(typeof(I_O_Output)))
            {
                int index = ParaBits.FindIndex(a => a.I_O_Output== DO && a.TypeIO == TypeIO.Output);
                int value = -1;
                if (index > -1) value = ParaBits[index].Adddress;
                AddressOutPut[(int)(DO)] = value;
            }
        }
        public bool AddInPut(int index,I_O_Input Input)
        {   int ix= ParaBits.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Input && a.I_O_Input != Input);
            if (ParaBits.FindIndex(a=>a.Adddress== index && a.TypeIO==TypeIO.Input)==-1)
            {
                ParaBits.Add(new ParaBit(TypeIO.Input, Input, index)); Arrange();
                return true;
            }
            else if (ix >= 0)
            {
                ParaBits.RemoveAt(ix);
                ParaBits.Add(new ParaBit(TypeIO.Input, Input, index)); Arrange();
                return true;
            }
            else
                return false;
        }
        public bool AddOutPut(int index, I_O_Output Output)
        {
            int ix = ParaBits.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Output);
            if(ix == -1)
            {
                ParaBits.Add(new ParaBit(TypeIO.Output, Output, index)); Arrange();
               
            }
            else
            {
                ParaBits[ix].I_O_Output = Output;
            }    
                return true;
           
        }
        public bool RemoveInPut(int index, I_O_Input Input)
        {
            int indexs = ParaBits.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Input);
            if (indexs>=0)
            {
                ParaBits.RemoveAt(indexs); Arrange();
                return true;
            }
            else
                return false;
        }
        public bool RemoveOutPut(int index, I_O_Output Output)
        {
            int indexs = ParaBits.FindIndex(a => a.Adddress == index && a.TypeIO == TypeIO.Output);
            if (indexs >= 0)
            {
                ParaBits.RemoveAt(indexs); Arrange();
                return true;
            }
            else
                return false;
          
        }
        [NonSerialized]
        PlcClient PlcClient ;
        public int timeOut = 2000;
        public string sIP = "";
        public bool IsAlive = false;
        public bool IsChangeAlive = false;
        [NonSerialized]
        public System.Windows.Forms.Timer timeAlive = new System.Windows.Forms.Timer();
        public Parity Parity= Parity.Even;
        public StopBits StopBits = StopBits.Two;
        public int DataBit = 7;
        public bool DtrEnable, RtsEnable;
        public async Task<bool> Connect(  )
        {
            try
            {
                PlcClient = new PlcLib.PlcClient(
                PlcBrand,
                ConnectionType,
                 ip: sIP,
                comPort: ComSerial,
                baudRate: Baurate,
                port: PortIP,
                _SlaveID:this.SlaveID,
                 retryCount: 3,
                  parity: Parity,
                  stopBits: StopBits,  
                  databit: DataBit,
                  DtrEnable: DtrEnable,
                  RtsEnable: RtsEnable,
                timeoutMs: timeOut);

                IsConnected= PlcClient.Connect();

             
               // IsConnected = ModbusService.IsConnected;// Modbus.ConnectPLC(Port, Baurate, SlaveID);
                if (valueInput == null)
                    valueInput = new bool[16];
                if (valueOutput == null)
                    valueOutput = new bool[16];
                if (AddressInput == null)
                    AddressInput = new int[30];
                if (AddressOutPut == null)
                    AddressOutPut = new int[30];
                
                Arrange();
               
                 if (IsConnected)
                {
                    Global.PLCStatus = PLCStatus.Ready;
                    Global.IsAllowReadPLC = true;
                    IO_Processing = IO_Processing.Reset;
                    //SetOutPut(15, true);
                    //  await  WriteOutPut();
                //   PlcClient.WriteBit(Global.ParaCommon.Comunication.Protocol.AddWrite + ".15", true);
                    timeAlive = new System.Windows.Forms.Timer();
                    timeAlive.Interval = 1000;
                    timeAlive.Enabled = true;
                    timeAlive.Tick += TimeAlive_Tick;
                    PlcClient.OnBitsRead += async (vals, addrs) =>
                    {
                        if (!PlcClient.IsConnect)
                        {
                            Global.PLCStatus = PLCStatus.ErrorConnect;
                            Global.StatusIO = StatusIO.NotConnect;
                            Global.ParaCommon.Comunication.Protocol.Disconnect();

                        }
                        valueInput = vals;
                        for (int i = 0; i < valueInput.Length; i++)
                        {
                            int ix = ParaBits.FindIndex(a => a.Adddress == i && a.TypeIO == TypeIO.Input);
                            if (ix >= 0)
                            {
                                ParaBits[ix].Value =Convert.ToInt32( valueInput[i]);
                            }
                        }


                        if (Global.IsRun && Global.ParaCommon.IsExternal )
                        {
                            if (AddressInput[(int)I_O_Input.Trigger4] != -1)
                            {
                                if (valueInput[AddressInput[(int)I_O_Input.Trigger4]] == true && Global.StatusProcessing == StatusProcessing.None)
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 4..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.TriggerNum = TriggerNum.Trigger4;
                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;

                                }
                            }
                            if (AddressInput[(int)I_O_Input.Trigger3] != -1)
                            {
                                if (valueInput[AddressInput[(int)I_O_Input.Trigger3]] == true && Global.StatusProcessing == StatusProcessing.None)
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 3..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.TriggerNum = TriggerNum.Trigger3;
                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;

                                }
                            }
                            if (AddressInput[(int)I_O_Input.Trigger2] != -1)
                            {
                                if (valueInput[AddressInput[(int)I_O_Input.Trigger2]] == true && Global.StatusProcessing == StatusProcessing.None)
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 2..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.TriggerNum = TriggerNum.Trigger2;
                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;

                                }
                            }
                            if (AddressInput[(int)I_O_Input.Trigger] != -1)
                            {
                                if (valueInput[AddressInput[(int)I_O_Input.Trigger]] == true && Global.StatusProcessing == StatusProcessing.None)
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 1..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    if(Global.ParaCommon.IsOnlyTrigger)
                                    {
                                        switch (Global.TriggerNum)
                                        {
                                            case TriggerNum.Trigger0:
                                                Global.TriggerNum = TriggerNum.Trigger1;
                                                break;
                                            case TriggerNum.Trigger1:
                                                Global.TriggerNum = TriggerNum.Trigger2;
                                                break;
                                            case TriggerNum.Trigger2:
                                                Global.TriggerNum = TriggerNum.Trigger3;
                                                break;
                                            case TriggerNum.Trigger3:
                                                Global.TriggerNum = TriggerNum.Trigger4;
                                                break;
                                         
                                        }
                                    }
                                    else
                                        Global.TriggerNum = TriggerNum.Trigger1;

                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;

                                }
                            }
                        
                            
                           
                        }
                        else if(Global.IsRun && Global.TriggerInternal)
                        {
                            switch(Global.TriggerNum)
                            {
                                case TriggerNum.Trigger0:
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.TriggerNum = TriggerNum.Trigger1;
                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;
                                    break;
                                case TriggerNum.Trigger1:
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.TriggerNum = TriggerNum.Trigger2;
                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;
                                    break;
                                case TriggerNum.Trigger2:
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.TriggerNum = TriggerNum.Trigger3;
                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;
                                    break;
                                case TriggerNum.Trigger3:
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.TriggerNum = TriggerNum.Trigger4;
                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                    if (Global.IsByPassResult)
                                        Global.EditTool.lbBypass.ForeColor = Color.White;
                                    break;
                            }

                        }    
                          
                          
                         
                        
                        //if (Global.ParaCommon.Comunication.Protocol.IO_Processing != IO_ProcessingOld)
                        //{

                        //    if (Global.StatusIO == StatusIO.None)
                        //    {

                        //        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO_WRITE", Global.ParaCommon.Comunication.Protocol.IO_Processing.ToString()));
                        //        if (Global.ParaCommon.Comunication.Protocol.IO_Processing == IO_Processing.ByPass)
                        //            Global.EditTool.lbBypass.ForeColor = Color.Green;
                        //        await Global.ParaCommon.Comunication.Protocol.WriteIO();
                        //        IO_ProcessingOld = Global.ParaCommon.Comunication.Protocol.IO_Processing;
                        //        lbWrite.Text = Math.Round(Global.ParaCommon.Comunication.Protocol.CTWrite) + "";

                        //    }
                        //}
                        //if (!IsChangeAlive)
                        //{
                        //    if (valueInput[15] == true)//Alive
                        //    {
                        //        SetOutPut(15, false);
                        //      await   WriteOutPut();
                        //       PlcClient.WriteBit(Global.ParaCommon.Comunication.Protocol.AddRead + ".15", false);
                        //        IsAlive = true;
                        //        IsChangeAlive = true;

                        //    }
                        //}
                 
                        int addr = ParaBits.Find(a => a.I_O_Input == I_O_Input.ByPass && a.TypeIO == TypeIO.Input)?.Adddress ?? -1;

                        if (addr > -1)
                        {
                          
                                if (valueInput[addr] == true && !Global.IsByPassResult)
                            {
                                Global.IsByPassResult = true;
                                Global.EditTool.lbBypass.Visible = true;
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO_READ", "BYPASS"));
                            }
                            else if (valueInput[addr] == false && Global.IsByPassResult)
                            {
                                Global.IsByPassResult = false;
                                Global.EditTool.lbBypass.Visible = false;
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO_READ", "NO BYPASS"));
                            }
                        }
                       
                    };
                    if (!PlcClient.StartOneBitReadLoop(AddRead, timeRead))
                        IsConnected = false;
                    TimingUtils.EnableHighResolutionTimer();
                }
              
               
                    if (valueInput.Length < 16)
                {
                    IsConnected = false;
                    return false;
                }
                    foreach(ParaBit paraIO in ParaBits)
                {
                    paraIO.Value = 0;
                }
            }
            catch(Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "IO", ex.Message));
                return false;
            }
           if(IsConnected)
            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO","Connected Success"));
           else
            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO", "Connect Fail"));
            return IsConnected;
        }
        private int numTimeOut=0;
        public void Disconnect()
        {
            Global.IsAllowReadPLC = false;
            PlcClient.StopOneBitReadLoop();
            timeAlive.Enabled = false;
            Global.StatusIO = StatusIO.NotConnect;
            Global.StatusMode = StatusMode.None;
            IsConnected = false;
            PlcClient.Disconnect();
        }
       
        private async void TimeAlive_Tick(object sender, EventArgs e)
        {
            if (Global.IsAllowReadPLC)
            {
                IsAlive = !IsAlive;
                SetOutPut(15, IsAlive);
            }
           if(IsConnected)
             PlcClient.WriteBit(Global.ParaCommon.Comunication.Protocol.AddWrite + ".15", IsAlive);
          
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
     
        bool IsCanWrite = false;
        [NonSerialized]
        Stopwatch CT =new Stopwatch();
        [NonSerialized]
       public float CTMid = 0,CTMin=0,CTMax=0;
        public float CTRead,CTWrite;
        //public  async Task Read()
        //{

        //    if (!IsConnected) return ;
        //    numRead++;
        //        CT.Restart();
        //    Global.StatusIO = StatusIO.Reading;
        //    valueInput =await ModbusService.ReadCoilsHoldingAsync(AddRead, 16); //await Modbus.ReadBit(1);await Modbus.ReadBit(AddRead);
        //    Global.StatusIO = StatusIO.None;
        //    CT.Stop();
            
        //    CTRead = (float) CT.Elapsed.TotalMilliseconds;
        //   if(CTRead > CTMax)
        //        CTMax = CTRead;
        //    if (CTRead < CTMin)
        //        CTMin = CTRead;

        //  for(int i=0; i<valueInput.Length; i ++)
        //      {
        //          int ix = ParaBits.FindIndex(a => a.Adddress == i && a.TypeIO == TypeIO.Input);
        //          if (ix >= 0)
        //          {
        //              ParaBits[ix].Value = valueInput[i];
        //          }
        //      }
        //    numRead--;
        //    //for (int i = 0; i < valueInput.Length; i++)
        //    //{
        //    //    int ix = ParaBits.FindIndex(a => a.Adddress == i && a.TypeIO == TypeIO.Input);
        //    //    if (ix >= 0)
        //    //    {
        //    //        ParaBits[ix].Value = valueInput[i];
        //    //    }
        //    //}


        //    //   valueOutput = Modbus.ReadBit(2);
        //    //  valueInput = new int[dataBytes.Length / 2];

        //    // valueInput =  Modbus.ReadHolding(AddressStarts[0]);
        //    //  if(IsReadOut)
        //    //valueOutput = Modbus.ReadHolding(AddressStarts[1]);
        //    //if (valueInput.Count()==1||valueOutput.Count()==1) IsConnected = false;
        //    return ;
        //}
        bool IsWait = false;
        public IO_Processing _IO_Processing = IO_Processing.None;
        [NonSerialized]
        private  SemaphoreSlim _ioLock = new SemaphoreSlim(1, 1);
        public IO_Processing IO_Processing
        {
            get => _IO_Processing;

            set
            {
                if (_IO_Processing != value)
                {
                   _IO_Processing = value;
                    // Fire-and-forget có bắt lỗi, không chặn UI
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            if(_ioLock==null) _ioLock = new SemaphoreSlim(1, 1);
                            await _ioLock.WaitAsync();
                            await WriteIO(value);
                        }
                        catch (Exception ex)
                        {
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", ex.Message));
                        }
                        finally
                        {
                            _ioLock.Release();
                        }
                    });
                   
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
        [NonSerialized]
        private Stopwatch tmReadCT = new Stopwatch();
        public async Task<bool>  WriteIO(IO_Processing _IO_Processing)
        {   if (!IsConnected) return false;

            if (_IO_Processing == IO_Processing.None)
            {

                return false;
            }
            Global.StatusIO = StatusIO.Writing;

            switch (_IO_Processing)
            {
                case IO_Processing.Trigger:
                  
                    switch(Global.TriggerNum)
                    {
                        case TriggerNum.Trigger1:
                            if (DelayTrigger == 0)
                            {
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                            SetLight(true);
                            await WriteOutPut();
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                            if (DelayTrigger > 0)
                            {
                                await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            break;
                        case TriggerNum.Trigger2:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                if (DelayTrigger == 0)
                                {
                                    Global.StatusMode = StatusMode.Once;
                                    Global.StatusProcessing = StatusProcessing.Read;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                                SetLight(true);
                                await WriteOutPut();
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                                if (DelayTrigger > 0)
                                {
                                    await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                    Global.StatusMode = StatusMode.Once;
                                    Global.StatusProcessing = StatusProcessing.Read;
                                }
                                break;
                            }    
                                if (DelayTrigger == 0)
                            {
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], true);//Busy
                            SetLight(true);
                            await WriteOutPut();
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], false);//Ready false
                            if (DelayTrigger > 0)
                            {
                                await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            break;
                        case TriggerNum.Trigger3:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                if (DelayTrigger == 0)
                                {
                                    Global.StatusMode = StatusMode.Once;
                                    Global.StatusProcessing = StatusProcessing.Read;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                                SetLight(true);
                                await WriteOutPut();
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                                if (DelayTrigger > 0)
                                {
                                    await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                    Global.StatusMode = StatusMode.Once;
                                    Global.StatusProcessing = StatusProcessing.Read;
                                }
                                break;
                            }
                            if (DelayTrigger == 0)
                            {
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], true);//Busy
                            SetLight(true);
                            await WriteOutPut();
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], false);//Ready false
                            if (DelayTrigger > 0)
                            {
                                await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            break;
                        case TriggerNum.Trigger4:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                if (DelayTrigger == 0)
                                {
                                    Global.StatusMode = StatusMode.Once;
                                    Global.StatusProcessing = StatusProcessing.Read;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                                SetLight(true);
                                await WriteOutPut();
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                                if (DelayTrigger > 0)
                                {
                                    await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                    Global.StatusMode = StatusMode.Once;
                                    Global.StatusProcessing = StatusProcessing.Read;
                                }
                                break;
                            }
                            if (DelayTrigger == 0)
                            {
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], true);//Busy
                            SetLight(true);
                            await WriteOutPut();
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], false);//Ready false
                            if (DelayTrigger > 0)
                            {
                                await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            break;

                    }
                   
                    break;
            
                case IO_Processing.Close:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //T.Result1
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false); //T.Result1
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false); //T.Result1
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false); //T.Result1
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], false); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], false); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], false); //Ready

                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic1], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic2], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic3], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic4], false); //Busy

                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], true); //Busy
                    await WriteOutPut();
                    Disconnect();
                    break;
                case IO_Processing.ByPass:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], true); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result2], true); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result3], true); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Result4], true); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true);//Ready false
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], true);//Ready false
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], true);//Ready false
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], true);//Ready false
                    SetLight(false);
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], false); //Busy
                    IsWait = true;
                    await WriteOutPut();
                    //if (IsBlink)
                    //{
                    //    await Task.Delay((int)DelayOutput);
                    //    SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG                           // SetOutPut(AddressOutPut[(int)I_O_Output.Result1], false); //False
                    //    await WriteOutPut();
                    //}
                    break;
                case IO_Processing.Error:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Error], true); //Err
                  
                    await WriteOutPut();
                    break;
                case IO_Processing.NoneErr:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Error], false); //Err
                    await WriteOutPut();
                    break;
                case IO_Processing.Result:
                  
                        int ix = AddressInput[(int)I_O_Input.ByPass];
                        if (ix > -1)
                        {
                            if (valueInput[ix] == true)
                            {
                            switch(Global.TriggerNum)
                            {
                                case TriggerNum.Trigger1:
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], true); //NG
                                    break;
                                case TriggerNum.Trigger2:
                                    if (Global.ParaCommon.IsOnlyTrigger)
                                    {
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], true); //NG
                                        break;
                                    }    
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result2], true); //NG
                                    break;
                                case TriggerNum.Trigger3:
                                    if (Global.ParaCommon.IsOnlyTrigger)
                                    {
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], true); //NG
                                        break;
                                    }
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result3], true); //NG
                                    break;
                                case TriggerNum.Trigger4:
                                    if (Global.ParaCommon.IsOnlyTrigger)
                                    {
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], true); //NG
                                        break;
                                    }
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result4], true); //NG
                                    break;
                            }    
                          
                        
                             }
                            else
                            {
                            switch (Global.TriggerNum)
                            {
                                case TriggerNum.Trigger1:

                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                    break;
                                case TriggerNum.Trigger2:
                                    if (Global.ParaCommon.IsOnlyTrigger)
                                    {
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                        break;
                                    }
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result2], Global.TotalOK); //NG
                                    break;
                                case TriggerNum.Trigger3:
                                    if (Global.ParaCommon.IsOnlyTrigger)
                                    {
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                        break;
                                    }
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result3], Global.TotalOK); //NG
                                    break;
                                case TriggerNum.Trigger4:
                                    if (Global.ParaCommon.IsOnlyTrigger)
                                    {
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                        break;
                                    }
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result4], Global.TotalOK); //NG
                                    break;
                            }
                        }    
                        }
                        else
                        {
                        switch (Global.TriggerNum)
                        {
                            case TriggerNum.Trigger1:
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                break;
                            case TriggerNum.Trigger2:
                                if (Global.ParaCommon.IsOnlyTrigger)
                                {
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                    break;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result2], Global.TotalOK); //NG
                                break;
                            case TriggerNum.Trigger3:
                                if (Global.ParaCommon.IsOnlyTrigger)
                                {
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                    break;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result3], Global.TotalOK); //NG
                                break;
                            case TriggerNum.Trigger4:
                                if (Global.ParaCommon.IsOnlyTrigger)
                                {
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], Global.TotalOK); //NG
                                    break;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result4], Global.TotalOK); //NG
                                break;
                        }
                    }
                    switch (Global.TriggerNum)
                    {
                        case TriggerNum.Trigger1:
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //NG
                            break;
                        case TriggerNum.Trigger2:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //NG
                                break;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], true); //NG
                            break;
                        case TriggerNum.Trigger3:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //NG
                                break;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], true); //NG
                            break;
                        case TriggerNum.Trigger4:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //NG
                                break;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], true); //NG
                            break;
                    }
                  
                            SetLight(false);
                    switch (Global.TriggerNum)
                    {
                        case TriggerNum.Trigger1:

                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //NG
                            break;
                        case TriggerNum.Trigger2:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //NG
                                break;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], false); //NG
                            break;
                        case TriggerNum.Trigger3:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //NG
                                break;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], false); //NG
                            break;
                        case TriggerNum.Trigger4:
                            if (Global.ParaCommon.IsOnlyTrigger)
                            {
                                SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //NG
                                break;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], false); //NG
                            break;
                    }
                  
                            IsWait = true;
                            await WriteOutPut();
                            if (IsBlink)
                            {
                            if (DelayOutput == 0)
                                await TimingUtils.DelayAccurateAsync((int)DelayOutput); // thay cho Task.Delay

                        switch (Global.TriggerNum)
                        {
                            case TriggerNum.Trigger1:
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result],false); //NG
                                break;
                            case TriggerNum.Trigger2:
                                if (Global.ParaCommon.IsOnlyTrigger)
                                {
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG
                                    break;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false); //NG
                                break;
                            case TriggerNum.Trigger3:
                                if (Global.ParaCommon.IsOnlyTrigger)
                                {
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG
                                    break;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false); //NG
                                break;
                            case TriggerNum.Trigger4:
                                if (Global.ParaCommon.IsOnlyTrigger)
                                {
                                    SetOutPut(AddressOutPut[(int)I_O_Output.Result], false); //NG
                                    break;
                                }
                                SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false); //NG
                                break;
                        }      
                        await WriteOutPut();
                            }


                    Global.NumSend++;
                    Global.StatusProcessing = StatusProcessing.Drawing;
                    break;
                case IO_Processing.ChangeMode:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], !Global.IsRun); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], !Global.IsRun); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], !Global.IsRun); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], !Global.IsRun); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], Global.IsRun); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], Global.IsRun); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], Global.IsRun); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], Global.IsRun); //Ready
                    SetLight(false);

                   // SetOutPut(AddressOutPut[(int)I_O_Output.Error], false); //Err
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
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], true); //Busy
                    await WriteOutPut();
                    break;
                case IO_Processing.Reset:
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Error], false); //Err
                   SetLight(false);
                    await WriteOutPut();
                   break;

            }
            valueInput = new bool[16];
            IO_Processing = IO_Processing.None;
            Global.StatusIO = StatusIO.None;

            return false;
        }
       
        public async Task<bool> CheckErr(bool IsCameraConnected)
        {
            if (!IsCameraConnected)
            {
                if (valueOutput[ParaBits.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1] == false)
                {
                   SetOutPut(ParaBits.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, true);//CCD Err
                   await WriteOutPut();
                    return false;
                }
                return true;
            }
            else
            {
                if (valueOutput[ParaBits.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1] == true)
                {
                   IO_Processing = IO_Processing.Error;
                   SetOutPut(ParaBits.Find(a => a.I_O_Output == I_O_Output.Error && a.TypeIO == TypeIO.Output)?.Adddress ?? -1, false);//CCD Err
                  await WriteOutPut();
                    return true;
                }
                return true;
            }
            return false;
        }
        //public int ReadPara(int Add )
        //{
        //    return 0;
      
        //   //return Modbus.ReadHolding(Add,1)[0];
        
        //}
        //public bool WritePara(int Add, int Value)
        //{
        //    IsWriting = true;
        //  //  IsConnected = Modbus.WritePLC(Add, Value);

        //    IsWriting = false;
        //    return IsConnected;
        //}
        public   bool WriteInPut(int Add,bool Value)
        {
            IsWriting = true;
         //   IsConnected = Modbus.WritePLC(Add,Convert.ToInt16(Value));//AddressStarts[0]+

            IsWriting = false;
            return IsConnected;
        }
        public void SetOutPut(int Add, bool Value)
        {
            if (!IsConnected) return;
            if (Add < 0) return;
            valueOutput[Add] =Value;
         int ix=   ParaBits.FindIndex(a => a.Adddress ==Add && a.TypeIO == TypeIO.Output);
            if (ix >= 0)
            {
                ParaBits[ix].Value =Convert.ToInt32( Value);
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
        public  short BoolsToShort(bool[] bits)
        {
            if (bits == null) throw new ArgumentNullException(nameof(bits));
            if (bits.Length > 16)
                throw new ArgumentException("Mảng bool[] tối đa 16 phần tử cho 1 short");

            short value = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                    value |= (short)(1 << i); // set bit thứ i
            }
            return value;
        }
        public async Task WriteResultBits(String Address,bool[] Value)       
        {
            PlcClient.WriteWord(Address, BoolsToShort(Value));
        }
        public async Task WriteResultFloat(String Address,float Value)
        {
            PlcClient.WriteFloat(Address,Value);
        }
        public async Task WriteResultString(String Address, string Value)
        {
            PlcClient.WriteString(Address, Value,100);
        }
        public async Task<bool> WriteOutPut()
        {
            numWrite++;
            
            if (!IsConnected)
                return false;
           // CT.Restart();
            short value = BoolsToShort(valueOutput);
            PlcClient.WriteWord(AddWrite, value);
         //   await  ModbusService.WriteSingleRegisterAsync(AddWrite, Val); //Modbus.WriteBit(AddWrite, Val);
         //else
         //       ModbusService.SetCoil(AddWrite, Convert.ToBoolean(Val));

            if (Global.StatusIO == StatusIO.ErrWrite)
            {
                await Task.Delay(50);
                Global.StatusIO = StatusIO.Writing;
                // goto X;
            }

            numWrite--;
           // CT.Stop();
            //CTWrite = (float)CT.Elapsed.TotalMilliseconds;
            //if (CTWrite > CTMax)
            //    CTMax = CTWrite;
            //if (CTWrite < CTMin)
            //    CTMin = CTWrite;
            return IsConnected;
        }
    }
}
