
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
using System.Web.UI.WebControls;
using System.Windows.Forms;
namespace BeeGlobal
{
    [Serializable()]
    public class ParaProtocol
    {
        [NonSerialized]
        private  CancellationTokenSource _cts = new CancellationTokenSource
            ();
        
        public TypeControler TypeControler = TypeControler.PLC;
        public String AddProg = "D104";
        [NonSerialized]
        public  String _ValueProg = "";
         public String ValueProg;
        [NonSerialized]
        public int _NoProg;
        [field: NonSerialized]
        public  event Action<int> ValueProgChanged;
        public  int NoProg
        {
            get => _NoProg;
            set
            {
                if (_NoProg != value)
                {
                    _NoProg = value;
                    ValueProgChanged?.Invoke(_NoProg); // Gọi event
                }
            }
        }
        public String AddRead = "D100";
        public String AddWrite = "D102";
        public float DelayTrigger = 1;
        public float DelayOutput= 1;
        public bool IsLight1,IsLight2,IsLight3;
     
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
        public int[] AddressInput=new int[60];
        [NonSerialized]
        public int[] AddressOutPut = new int[60];
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
          
                AddressInput = new int[60];
         
                AddressOutPut = new int[60];
          
            ParaBits.RemoveAll(b => b.I_O_Input == I_O_Input.None&&b.TypeIO==TypeIO.Input || b.I_O_Output == I_O_Output.None && b.TypeIO == TypeIO.Output);
            var seen = new HashSet<string>();
            var filtered = new List<ParaBit>();

            foreach (var b in ParaBits)
            {
                // TODO: thay Key theo tiêu chí bạn muốn
                string key = $"{b.Adddress}|{b.TypeIO}|{b.I_O_Input}|{b.I_O_Output}";

                if (seen.Add(key)) // Add trả true nếu chưa tồn tại
                    filtered.Add(b);
            }

            ParaBits = filtered;
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
                if((int)(DO) >= AddressOutPut.Count())
                {
                    String A = DO.ToString();
                }
                else
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
        public PlcClient PlcClient ;
        public int timeOut = 2000;
        public string sIP = "";
        public bool IsAlive = false;
        public bool IsChangeAlive = false;
        public List<ParaValue> ListParaValue=new List<ParaValue>(); 
        [NonSerialized]
        public System.Windows.Forms.Timer timeAlive = new System.Windows.Forms.Timer();
        public Parity Parity= Parity.Even;
        public StopBits StopBits = StopBits.Two;
        public int DataBit = 7;
        public bool DtrEnable, RtsEnable;
        //hau
        public List<PLCResult> PLCResults = new List<PLCResult>();
        private bool IsPressLive = false;
        public String AddPO = "";
        public String AddCountProg = "";
        public int _ValueCountProg = 2;
        public String AddQty = "";
        public int _ValueQty = 0;
        [field: NonSerialized]
        public event Action<int> QtyChanged;
        public int ValueQty
        {
            get => _ValueQty;
            set
            {
                if (_ValueQty != value)
                {
                    _ValueQty = value;
                    if(_ValueQty!=0)
                     if (  _ValueQty != Global.Config.SumTime)
                        {
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Qty", "PLC:" + _ValueQty + "VS:" + Global.Config.SumTime));

                        }

                    QtyChanged?.Invoke(_ValueQty); // Gọi event
                }
            }
        }
        public String AddProgress = "";
        public int _ValueProgress = 0;
        [field: NonSerialized]
        public event Action<int> ProgressChanged;
        public int ValueProgress
        {
            get => _ValueProgress;
            set
            {
                if (_ValueProgress != value)
                {
                    _ValueProgress = value;
                    ProgressChanged?.Invoke(_ValueProgress); // Gọi event
                }
            }
        }
        [field: NonSerialized]
        public event Action<int> ValueCountProgChanged;
        public int ValueCountProg
        {
            get => _ValueCountProg;
            set
            {
                if (_ValueCountProg != value)
                {
                    _ValueCountProg = value;
                    Global.NumProgFromPLC= value; ;
                    ValueCountProgChanged?.Invoke(_ValueCountProg); // Gọi event
                }
            }
        }
        public String AddTotalTime = "";
        private String _ValuePO = "";
        [field: NonSerialized]
        public event Action<String> ValuePOChanged;
        public String ValuePO
        {
            get => _ValuePO;
            set
            {
                if (_ValuePO != value)
                {
                    _ValuePO = value;
                    Global.Config.POCurrent = value; ;
                    ValuePOChanged?.Invoke(_ValuePO); // Gọi event
                }
            }
        }
        public bool GetInPut(I_O_Input I_O_Input)
        {
            int ix = AddressInput[(int)I_O_Input];
            if (ix == -1 || ix >= valueInput.Length) return false;
            return valueInput[ix];
          
        }
        public  void WriteInput(I_O_Input I_O_Input,bool Value)
        {
            int ix = AddressInput[(int)I_O_Input];
            if (ix == -1 ) return ;
            if(!IsConnected) return ;
            PlcClient.WriteBit(AddRead + "." + AddressInput[(int)I_O_Input], Value);
        }
        [NonSerialized]
        public PCI_Card PCI_Card1 = new PCI_Card();
        public async Task<bool> Connect(  )
        {
            try
            {
                if(TypeControler == TypeControler.PCI)
                {
                    try
                    {
                        PCI_Card1 = new PCI_Card();
                        Global.NumProgFromPLC = Global.Config.NumTrig;
                        IsConnected = PCI_Card1.Connect();
                     //  IsConnected = true;
                        if (IsConnected)
                        {
                           PCI_Card1.Write(PCI_Write.LightOFF);
                            PCI_Card1.OnBitsRead += PCI_Card_OnBitsRead;
                            PCI_Card1.StartReadLoop(timeRead);
                            TimingUtils.EnableHighResolutionTimer();
                        }
                        else
                        {
                            Global.PLCStatus = PLCStatus.ErrorConnect;
                            Global.StatusIO = StatusIO.NotConnect;
                        }
                        if (valueInput == null)
                            valueInput = new bool[16];
                        if (valueOutput == null)
                            valueOutput = new bool[16];
                        if (AddressInput == null)
                            AddressInput = new int[60];
                        if (AddressOutPut == null)
                            AddressOutPut = new int[60];
                        Arrange();
                        foreach (ParaBit paraIO in ParaBits)
                        {
                            paraIO.Value = 0;
                        }
                        return IsConnected;
                    }
                    catch(Exception ex)
                    {
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "PCI", ex.Message));


                    }

                }    
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
                    AddressInput = new int[60];
                if (AddressOutPut == null)
                    AddressOutPut = new int[60];
                
                Arrange();
               
                 if (IsConnected)
                {
                    Global.NumProgFromPLC = Global.Config.NumTrig;
                    foreach (ParaBit paraIO in ParaBits)
                    {
                        paraIO.Value = 0;
                    }
                    IO_Processing = IO_Processing.None;
                    Global.PLCStatus = PLCStatus.Ready;
                    Global.IsAllowReadPLC = true;
                    //IO_Processing = IO_Processing.Reset;
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Error], false); //Err
                    SetOutPut(AddressOutPut[(int)I_O_Output.ByPass], false);
                    SetLight(false);
                    await WriteOutPut();
                    IO_Processing = IO_Processing.Reset;
                    if (TypeControler==TypeControler.PLC)
                    {
                        if(AddProg.Trim()!="")
                        {
                            NoProg = PlcClient.ReadInt(AddProg);
                            Global.IsChangeProg = false;
                            Global.IsPLCChangeProg = true;
                            Global.IsChangeProg = true;
                      

                            if (AddPO != null)
                                if (AddPO != "")
                                    ValuePO = PlcClient.ReadStringAsciiKey(AddPO, 16).Trim();
                            if (AddCountProg != null)
                                if (AddCountProg != "")
                                    ValueCountProg = PlcClient.ReadInt(AddCountProg);
                           if (AddQty != null)
                                if (AddQty != "")
                                    ValueQty = PlcClient.ReadInt(AddQty);
                           if(ValueQty!=0)
                            {
                                Global.Config.SumTime = ValueQty;
                                Global.Config.SumOK = Global.Config.SumTime - Global.Config.SumNG;
                            }    
                           Global.NumProgFromPLC = ValueCountProg;


                        }

                    }

                    PlcClient.OnBitsRead += async (vals, addrs) =>
                    {
                      //  if (Global.IsChangeProg) return;
                        if (!PlcClient.IsConnect)
                        {
                            Global.PLCStatus = PLCStatus.ErrorConnect;
                            Global.StatusIO = StatusIO.NotConnect;
                            PlcClient.StopOneBitReadLoop();
                            Disconnect();

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
                       // if ( Global.Config.IsForceByPassRS)
                       // {
                       //     Global.IsByPassResult = true;


                       //      //   Global.EditTool.lbBypass.Visible = true;
                       // }
                       // else
                       // {
                       //     Global.IsByPassResult = false;
                       ////     Global.EditTool.lbBypass.Visible = false;
                       
                       // }
                        if (Global.IsLive) return;
                        if (Global.IsRun  )
                        {
                            if (Global.Config.IsExternal)
                            {
                               

                                if (Global.StatusProcessing == StatusProcessing.None)
                                {
                                    if (AddProgress != null)
                                        if (AddProgress != "")
                                            ValueProgress = PlcClient.ReadInt(AddProgress);
                                    if (GetInPut(I_O_Input.Live) == true && !IsPressLive)
                                    {
                                        IsPressLive = true;

                                    }
                                    else if (GetInPut(I_O_Input.Live) == false && IsPressLive)
                                    {
                                        IsPressLive = false;

                                        Global.IsLive = !Global.IsLive;
                                        SetLight(Global.IsLive);
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Busy], Global.IsLive);//Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Ready], !Global.IsLive);//Ready false
                                        await WriteOutPut();
                                    }
                                    if (GetInPut(I_O_Input.ChangeProg) == true&& !Global.IsChangeProg)
                                    {
                                        if (AddCountProg != null)
                                            if (AddCountProg != "")
                                                ValueCountProg = PlcClient.ReadInt(AddCountProg);
                                        if (AddProg != null)
                                            if (AddProg != "")
                                                NoProg = PlcClient.ReadInt(AddProg);
                                        WriteInput(I_O_Input.ChangeProg, false);
                                        Global.Config.SumOK = 0;
                                        Global.Config.SumNG = 0;
                                        Global.Config.SumTime = 0;
                                        Global.IsPLCChangeProg = true;
                                        Global.IsChangeProg = true;

                                    }
                                    if (GetInPut(I_O_Input.Dummy) == true&&!Global.IsDummy)
                                    {
                                        Global.IsDummy = true;
                                    }
                                    else if (GetInPut(I_O_Input.Dummy) == false && Global.IsDummy)
                                    {
                                        Global.IsDummy = false;
                                    }
                                    if (GetInPut(I_O_Input.Shuttdown) == true)
                                    {
                                        WriteInput(I_O_Input.Shuttdown, false);
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
                                        SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD1], false); //Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD2], false); //Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD3], false); //Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD4], false); //Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true); //Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], true); //Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], true); //Busy
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], true); //Busy
                                        await WriteOutPut();
                                        Disconnect();
                                        Global.IsAutoShuttDown = true;
                                      

                                    }    
                                        if (GetInPut(I_O_Input.Reset) == true)
                                    {
                                        WriteInput(I_O_Input.Reset, false);
                                        //Global.Config.SumTime = 0;
                                        //Global.Config.SumOK = 0;
                                        //Global.Config.SumNG = 0;

                                    }
                                    if (GetInPut(I_O_Input.ByPass) == true && !Global.IsByPassResult)
                                    {
                                        Global.IsByPassResult = true;
                                       // Global.EditTool.lbBypass.Visible = true;
                                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO_READ", "BYPASS"));

                                    }
                                    else if (GetInPut(I_O_Input.ByPass) == false && Global.IsByPassResult)
                                    {
                                        Global.IsByPassResult = false;
                                     //   Global.EditTool.lbBypass.Visible = false;
                                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "IO_READ", "NO BYPASS"));

                                    }
                                  
                                    switch (Global.TriggerNum)
                                    {
                                        case TriggerNum.Trigger3:
                                            if (GetInPut(I_O_Input.Trigger4) == true)
                                            {
                                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 4..."));
                                                Global.TriggerInternal = false;
                                                Global.IsAllowReadPLC = false;
                                                Global.TriggerNum = TriggerNum.Trigger4;
                                                if (Global.Config.IsMultiProg)
                                                    Global.IndexChoose = 3;
                                                else
                                                    Global.IndexChoose = 0;
                                                Global.StatusProcessing = StatusProcessing.Trigger;

                                                IO_Processing = IO_Processing.Trigger;

                                            }
                                            break;
                                        case TriggerNum.Trigger2:
                                            if (GetInPut(I_O_Input.Trigger3) == true)
                                            {
                                                if (Global.Config.IsMultiProg)
                                                    Global.IndexChoose = 2;
                                                else
                                                    Global.IndexChoose = 0;
                                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 3..."));
                                                Global.TriggerInternal = false;
                                                Global.IsAllowReadPLC = false;
                                                Global.TriggerNum = TriggerNum.Trigger3;
                                                Global.StatusProcessing = StatusProcessing.Trigger;
                                                IO_Processing = IO_Processing.Trigger;

                                            }
                                            break;
                                        case TriggerNum.Trigger1:
                                            if (GetInPut(I_O_Input.Trigger2) == true)
                                            {
                                                if (Global.Config.IsMultiProg)
                                                    Global.IndexChoose = 1;
                                                else
                                                    Global.IndexChoose = 0;
                                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 2..."));
                                                Global.TriggerInternal = false;
                                                Global.IsAllowReadPLC = false;
                                                Global.TriggerNum = TriggerNum.Trigger2;
                                                Global.StatusProcessing = StatusProcessing.Trigger;
                                                IO_Processing = IO_Processing.None;
                                                IO_Processing = IO_Processing.Trigger;
                                            }
                                            break;
                                        case TriggerNum.Trigger0:
                                            if (GetInPut(I_O_Input.Trigger) == true)
                                            {
                                              
                                                Global.IndexChoose = 0;
                                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 1..."));
                                                Global.TriggerInternal = false;
                                                Global.IsAllowReadPLC = false;

                                                Global.TriggerNum = TriggerNum.Trigger1;

                                                Global.StatusProcessing = StatusProcessing.Trigger;
                                                IO_Processing = IO_Processing.None;
                                                IO_Processing = IO_Processing.Trigger;

                                                try
                                                {
                                                    if (AddPO != null)
                                                        if (AddPO != "")
                                                            ValuePO = PlcClient.ReadStringAsciiKey(AddPO, 16).Trim();
                                                    if (AddQty != null)
                                                        if (AddQty != "")
                                                            ValueQty = PlcClient.ReadInt(AddQty);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Read PO", ex.Message));
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if ( Global.TriggerInternal)
                                {
                                    Global.TriggerInternal = false;
                                    switch (Global.TriggerNum)
                                    {
                                        case TriggerNum.Trigger0:
                                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                           
                                            Global.IsAllowReadPLC = false;
                                            Global.TriggerNum = TriggerNum.Trigger1;
                                            Global.StatusProcessing = StatusProcessing.Trigger;
                                            IO_Processing = IO_Processing.Trigger;
                                            break;
                                        case TriggerNum.Trigger1:
                                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                          
                                            Global.IsAllowReadPLC = false;
                                            Global.TriggerNum = TriggerNum.Trigger2;
                                            Global.StatusProcessing = StatusProcessing.Trigger;
                                            IO_Processing = IO_Processing.Trigger;
                                            break;
                                        case TriggerNum.Trigger2:
                                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                          
                                            Global.IsAllowReadPLC = false;
                                            Global.TriggerNum = TriggerNum.Trigger3;
                                            Global.StatusProcessing = StatusProcessing.Trigger;
                                            IO_Processing = IO_Processing.Trigger;
                                            break;
                                        case TriggerNum.Trigger3:
                                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger..."));
                                           
                                            Global.IsAllowReadPLC = false;
                                            Global.TriggerNum = TriggerNum.Trigger4;
                                            Global.StatusProcessing = StatusProcessing.Trigger;
                                            IO_Processing = IO_Processing.Trigger;
                                            break;
                                    }

                                }
                            }    
                           
                          


                        }
                       
                        //if (AddressInput[(int)I_O_Input.ResetImg] != -1 && Global.StatusProcessing == StatusProcessing.None && Global.StatusProcessing != StatusProcessing.Trigger)
                        //{
                        //    if (valueInput[AddressInput[(int)I_O_Input.ResetImg]] == true)
                        //    {

                        //        PlcClient.WriteBit(AddRead + "." + AddressInput[(int)I_O_Input.ResetImg], false);
                        //        Global.StatusProcessing = StatusProcessing.ResetImg;

                        //    }

                        //}



                        //if (IO_Processing != IO_ProcessingOld)
                        //{

                        //    if (Global.StatusIO == StatusIO.None)
                        //    {

                        //        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO_WRITE", IO_Processing.ToString()));
                        //        if (IO_Processing == IO_Processing.ByPass)
                        //            Global.EditTool.lbBypass.ForeColor = Color.Green;
                        //        await WriteIO();
                        //        IO_ProcessingOld = IO_Processing;
                        //        lbWrite.Text = Math.Round(CTWrite) + "";

                        //    }
                        //}
                        //if (!IsChangeAlive)
                        //{
                        //    if (valueInput[15] == true)//Alive
                        //    {
                        //        SetOutPut(15, false);
                        //      await   WriteOutPut();
                        //       PlcClient.WriteBit(AddRead + ".15", false);
                        //        IsAlive = true;
                        //        IsChangeAlive = true;

                        //    }
                        //}

                       
                       
                    };
                    if (!PlcClient.StartOneBitReadLoop(AddRead, timeRead))
                        IsConnected = false;
                    TimingUtils.EnableHighResolutionTimer();
                }
              else
                {
                  //  Global.PLCStatus = PLCStatus.ErrorConnect;
                }

                    if (valueInput.Length < 16)
                {
                    IsConnected = false;
                    return false;
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

        private void PCI_Card_OnBitsRead(bool obj)
        {
            int index0 = ParaBits.FindIndex(a => a.I_O_Input == I_O_Input.Trigger && a.TypeIO == TypeIO.Input);
            if (index0 >= 0)
            {
                ParaBits[index0].Value = Convert.ToInt32(obj);
            }

            if (obj)
            {
                try
                {
                   
                   
                       
                    if (Global.IsRun && Global.Config.IsExternal)
                        if (AddressInput[(int)I_O_Input.Trigger] != -1)
                        {
                            int ix = ParaBits.FindIndex(a => a.I_O_Input == I_O_Input.Trigger && a.TypeIO == TypeIO.Input);
                            if (ix >= 0)
                            {
                                ParaBits[ix].Value = Convert.ToInt32(Convert.ToInt32(obj));
                                if (obj == true||Global.TriggerInternal)
                                {


                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 1..."));
                                    Global.TriggerInternal = false;
                                    Global.IsAllowReadPLC = false;


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

                                    Global.StatusProcessing = StatusProcessing.Trigger;
                                    IO_Processing = IO_Processing.Trigger;
                                }
                               
                            }
                        }
                }
                catch(Exception ex)
                {
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "PCI2", ex.Message));
                }
            }
        }

        private int numTimeOut=0;
        public void Disconnect()
        {
            Global.IsAllowReadPLC = false;
            PlcClient.StopOneBitReadLoop();
            if(timeAlive!=null)
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
            if (IsConnected)
              
             PlcClient.WriteBit(AddWrite + ".15", IsAlive);
          
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
                            if (_ioLock==null) _ioLock = new SemaphoreSlim(1, 1);
                            await _ioLock.WaitAsync();
                            await WriteIO(value);
                        }
                        catch (Exception ex)
                        {
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, _IO_Processing.ToString(), ex.Message));
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
      
        public TypeOutputRS TypeOutputRS = TypeOutputRS.AllTime;
        public int DelayOutOK=300, DelayOutNG=50;
        public bool IsLogic1, IsLogic2, IsLogic3, IsLogic4, IsLogic5, IsLogic6;
        [NonSerialized]
        private Stopwatch tmReadCT = new Stopwatch();
        public async Task<bool>  WriteIO(IO_Processing _IO_Processing)
        {   if (!IsConnected)
            {
                valueInput = new bool[16];
                IO_Processing = IO_Processing.None;
                Global.StatusIO = StatusIO.None;
                return false;
            }

            if (_IO_Processing == IO_Processing.None)
            {
                return false;
            }
            Global.StatusIO = StatusIO.Writing;
            switch (_IO_Processing)
            {
                case IO_Processing.SendValue:
                    foreach(PLCResult pLCResult in PLCResults)
                    {
                        switch(pLCResult.ValuePLC)
                        {
                            case ValuePLC.TotalOK:
                              await  WriteResultFloat(pLCResult.Add, Global.Config.SumOK);
                                break;
                            case ValuePLC.TotalNG:
                                await WriteResultFloat(pLCResult.Add, Global.Config.SumNG);
                                break;
                            case ValuePLC.Total:
                                await WriteResultFloat(pLCResult.Add, Global.Config.SumTime);
                                break;
                            case ValuePLC.Cycle:
                                await WriteResultFloat(pLCResult.Add, Global.Config.TotalTime);
                                break;
                        }
                        
                    }    
                    break;
                case IO_Processing.DoneCCD:
                    if (TypeControler == TypeControler.PCI)
                        break;
                    switch (Global.IndexCCCD)
                    {
                        case 0:
                            SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD1], true);//Busy
                            break;
                        case 1:
                            SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD2], true);//Busy
                            break;
                        case 2:
                            SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD3], true);//Busy
                            break;
                        case 3:
                            SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD4], true);//Busy
                            break;
                    }
                   
                    await WriteOutPut();
                    break;
                case IO_Processing.Trigger:
                   
                    if (TypeControler == TypeControler.PCI)
                    {
                        if (DelayTrigger == 0)
                        {
                            Global.StatusMode = StatusMode.Once;
                            Global.StatusProcessing = StatusProcessing.Read;
                        }
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Trigger", "OK"));

                        PCI_Card1.Write(PCI_Write.LightON);
                        if (DelayTrigger > 0)
                        {
                            await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                            Global.StatusMode = StatusMode.Once;
                            Global.StatusProcessing = StatusProcessing.Read;
                        }
                        break;
                    }
                    switch (Global.TriggerNum)
                    {
                      
                        case TriggerNum.Trigger1:
                           
                          
                            if(TypeControler == TypeControler.PCI)
                            {
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Trigger", "OK"));

                                PCI_Card1.Write(PCI_Write.LightON);
                                if (DelayTrigger == 0)
                                {
                                    Global.StatusMode = StatusMode.Once;
                                    Global.StatusProcessing = StatusProcessing.Read;
                                }
                                else if (DelayTrigger > 0)
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
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                            if(TypeControler==TypeControler.IO)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                            SetLight(true);
                           
                            if (TypeControler == TypeControler.PLC)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.ResultTotal], false);
                            await WriteOutPut();
                            if (DelayTrigger > 0)
                            {
                                await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            break;
                        case TriggerNum.Trigger2:
                            //if (Global.Config.IsOnlyTrigger)
                            //{
                            //    if (DelayTrigger == 0)
                            //    {
                            //        Global.StatusMode = StatusMode.Once;
                            //        Global.StatusProcessing = StatusProcessing.Read;
                            //    }
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                            //    SetLight(true);
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Result], false);
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false);
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false);
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false);
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.ResultTotal], false);
                            //    await WriteOutPut();
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                            //    if (DelayTrigger > 0)
                            //    {
                            //        await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                            //        Global.StatusMode = StatusMode.Once;
                            //        Global.StatusProcessing = StatusProcessing.Read;
                            //    }
                            //    break;
                            //}    
                                if (DelayTrigger == 0)
                            {
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], true);//Busy
                            SetLight(true);
                            if (TypeControler == TypeControler.IO)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], false);//Ready false
                            SetLight(true);

                            if (TypeControler == TypeControler.PLC)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], false);//Ready false
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.ResultTotal], false);
                            await WriteOutPut();
                            if (DelayTrigger > 0)
                            {
                                await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            break;
                        case TriggerNum.Trigger3:
                            //if (Global.Config.IsOnlyTrigger)
                            //{
                            //    if (DelayTrigger == 0)
                            //    {
                            //        Global.StatusMode = StatusMode.Once;
                            //        Global.StatusProcessing = StatusProcessing.Read;
                            //    }
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                            //    SetLight(true);
                            //    await WriteOutPut();
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                            //    if (DelayTrigger > 0)
                            //    {
                            //        await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                            //        Global.StatusMode = StatusMode.Once;
                            //        Global.StatusProcessing = StatusProcessing.Read;
                            //    }
                            //    break;
                            //}
                            //if (DelayTrigger == 0)
                            //{
                            //    Global.StatusMode = StatusMode.Once;
                            //    Global.StatusProcessing = StatusProcessing.Read;
                            //}

                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], true);//Busy
                            SetLight(true);
                            if (TypeControler == TypeControler.IO)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], false);//Ready false
                            SetLight(true);

                            if (TypeControler == TypeControler.PLC)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], false);//Ready false
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.ResultTotal], false);
                            await WriteOutPut();
                            if (DelayTrigger > 0)
                            {
                                await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            break;
                        case TriggerNum.Trigger4:
                            //if (Global.Config.IsOnlyTrigger)
                            //{
                            //    if (DelayTrigger == 0)
                            //    {
                            //        Global.StatusMode = StatusMode.Once;
                            //        Global.StatusProcessing = StatusProcessing.Read;
                            //    }
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true);//Busy
                            //    SetLight(true);
                            //    await WriteOutPut();
                            //    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], false);//Ready false
                            //    if (DelayTrigger > 0)
                            //    {
                            //        await TimingUtils.DelayAccurateAsync((int)DelayTrigger); // thay cho Task.Delay
                            //        Global.StatusMode = StatusMode.Once;
                            //        Global.StatusProcessing = StatusProcessing.Read;
                            //    }
                            //    break;
                            //}
                            if (DelayTrigger == 0)
                            {
                                Global.StatusMode = StatusMode.Once;
                                Global.StatusProcessing = StatusProcessing.Read;
                            }
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], true);//Busy
                            SetLight(true);
                            if (TypeControler == TypeControler.IO)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], false);//Ready false
                            SetLight(true);

                            if (TypeControler == TypeControler.PLC)
                                SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], false);//Ready false
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false);
                            SetOutPut(AddressOutPut[(int)I_O_Output.ResultTotal], false);
                            await WriteOutPut();
                           
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
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD1], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD2], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD3], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD4], false); //Busy
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
                    bool IsOK = Global.ListResult[Global.IndexChoose] == Results.OK ? true : false;
                    bool IsOKTotal = false;
                    if (Global.TotalOK == Results.OK) IsOKTotal = true;
                    try
                    {
                      
                        if (TypeControler == TypeControler.PCI)
                        {
                            if (IsOK||Global.Config.IsForceByPassRS)
                                PCI_Card1.Write(PCI_Write.OK);
                            else
                                PCI_Card1.Write(PCI_Write.NG);
                            Global.StatusProcessing = StatusProcessing.Drawing;
                            break;
                        }
                       
                    }
                  catch(Exception ex)
                  {
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "SendRS", ex.Message));

                    }
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD1], false);//Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD2], false);//Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD3], false);//Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.DoneCCD4], false);//Busy
                   
                
                    if(Global.Config.IsONNG)
                    {
                        IsOKTotal = !IsOKTotal;
                        IsOK =! IsOK;
                        if (Global.IsByPassResult || Global.Config.IsForceByPassRS)
                        {
                            IsOK = false;
                            IsOKTotal = false;

                        }    
                          
                    }
                    else
                    {
                        if (Global.IsByPassResult || Global.Config.IsForceByPassRS)
                        {
                            IsOKTotal = true;
                            IsOK = true;
                        }    
                          
                    }    
                        bool IsBlink = IsOK;
                    if(Global.TotalOK==Results.OK||Global.TotalOK==Results.NG)
                    {
                        SetOutPut(AddressOutPut[(int)I_O_Output.ResultTotal], IsOKTotal); //NG
                    }
                    switch (Global.TriggerNum)
                    {
                        case TriggerNum.Trigger1:
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result], IsOK); //NG
                            break;
                        case TriggerNum.Trigger2:
                           
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result2], IsOK); //NG
                            break;
                        case TriggerNum.Trigger3:
                            
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result3], IsOK); //NG
                            break;
                        case TriggerNum.Trigger4:
                            
                            SetOutPut(AddressOutPut[(int)I_O_Output.Result4], IsOK); //NG
                            break;
                    }
                 
                     
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic1], IsLogic1); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic2], IsLogic2); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic3], IsLogic3); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic4], IsLogic4); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic5], IsLogic5); //NG
                    SetOutPut(AddressOutPut[(int)I_O_Output.Logic6], IsLogic6); //NG
                    switch (Global.TriggerNum)
                    {
                        case TriggerNum.Trigger1:
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //NG
                            break;
                        case TriggerNum.Trigger2:
                            
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], true); //NG
                            break;
                        case TriggerNum.Trigger3:
                            
                            SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], true); //NG
                            break;
                        case TriggerNum.Trigger4:
                           
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
                           
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], false); //NG
                            break;
                        case TriggerNum.Trigger3:
                           
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], false); //NG
                            break;
                        case TriggerNum.Trigger4:
                           
                            SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], false); //NG
                            break;
                    }
                  
                            IsWait = true;
                            await WriteOutPut();
                   
                            if (TypeOutputRS==TypeOutputRS.Blink)
                            {
                            if (DelayOutput == 0)
                                await TimingUtils.DelayAccurateAsync((int)DelayOutput); // thay cho Task.Delay
                                if (Global.TotalOK == Results.OK || Global.TotalOK == Results.NG)
                                {
                                    SetOutPut(AddressOutPut[(int)I_O_Output.ResultTotal], false); //NG
                                }
                                switch (Global.TriggerNum)
                                {
                                    case TriggerNum.Trigger1:
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result],false); //NG
                                        break;
                                    case TriggerNum.Trigger2:
                                        
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result2], false); //NG
                                        break;
                                    case TriggerNum.Trigger3:
                                       
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result3], false); //NG
                                        break;
                                    case TriggerNum.Trigger4:
                                      
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result4], false); //NG
                                        break;
                                }      
                             await WriteOutPut();
                            }
                        else if (TypeOutputRS == TypeOutputRS.OKNG)
                        {
                            IsBlink = !IsBlink;
                            int num = 2;
                            int delayBlink = DelayOutOK;
                            if (!IsOK)
                            {
                                 num = 11;
                                 delayBlink = DelayOutNG;

                            }
                          
                            for (int i = 0; i < num; i++)
                            {   if (i == num - 1) IsBlink = false;
                                await TimingUtils.DelayAccurateAsync(delayBlink); // thay cho Task.Delay
                                switch (Global.TriggerNum)
                                {
                                    case TriggerNum.Trigger1:
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result], IsBlink); 
                                        break;
                                    case TriggerNum.Trigger2:
                                        
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result2], IsBlink); 
                                        break;
                                    case TriggerNum.Trigger3:
                                       
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result3], IsBlink); 
                                        break;
                                    case TriggerNum.Trigger4:
                                       
                                        SetOutPut(AddressOutPut[(int)I_O_Output.Result4], IsBlink); 
                                        break;
                                }
                                await WriteOutPut();
                                IsBlink = !IsBlink;
                        }
                    }


                        Global.NumSend++;
                    Global.StatusProcessing = StatusProcessing.Drawing;
                    break;
                case IO_Processing.ChangeMode:
                    if (TypeControler == TypeControler.PCI)
                        break;
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
                    if (TypeControler==TypeControler.PCI)
                    {   if(Global.Config.IsOnLight)
                            PCI_Card1.Write(PCI_Write.LightON);
                        else
                            PCI_Card1.Write(PCI_Write.LightOFF);
                       
                        break;
                    }
                  
                        SetLight(Global.Config.IsOnLight);
                    await WriteOutPut();
                    break;
                case IO_Processing.ChangeProg:
                    if (TypeControler == TypeControler.PCI)
                        break;
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], true); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], true); //Busy
                    await WriteOutPut();
                    break;
                case IO_Processing.Reset:
                    if (TypeControler == TypeControler.PCI)
                        break;
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready2], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready3], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Ready4], true); //Ready
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy2], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy3], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Busy4], false); //Busy
                    SetOutPut(AddressOutPut[(int)I_O_Output.Error], false); //Err
                    SetOutPut(AddressOutPut[(int)I_O_Output.ByPass], false);
                    SetLight(false);
                    await WriteOutPut();
                    WriteInput(I_O_Input.ChangeProg, false);

                    //if (AddCountProg != null)
                    //    if (AddCountProg != "")
                    //       ValueCountProg = PlcClient.ReadInt(AddCountProg);
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
        public async Task WriteResultFloatArr(String Address, float[] Value)
        {
            PlcClient.WriteFloatArray(Address, Value);
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
