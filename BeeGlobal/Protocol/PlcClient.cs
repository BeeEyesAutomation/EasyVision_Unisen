// PlcClient_Compat73.cs  —  C# 7.3 compatible
using BeeGlobal;
using HslCommunication;
using HslCommunication.LogNet;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Delta;
using HslCommunication.Profinet.Keyence;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Panasonic;
using HslCommunication.Profinet.Siemens;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlcLib
{
    [Serializable()]
    public enum PlcBrand { Mitsubishi,Keyence, Omron, Siemens, ModbusRtu, ModbusAscii,Delta,Pana }
    public enum ConnectionType { Tcp, Serial }

    public sealed class PlcClient : IDisposable
    {
        public void Dispose()
        {
            // Dừng vòng đọc liên tục nếu đang chạy
            StopOneBitReadLoop();

            // Ngắt kết nối PLC nếu còn mở
            Disconnect();

            // Nếu _plc là IDisposable (ví dụ ModbusTcpNet, MelsecMcNet...)
            if (_plc is IDisposable disp)
            {
                disp.Dispose();
            }

            _plc = null;
        }
        // cấu hình
        private readonly PlcBrand _brand;
        private readonly ConnectionType _connType;
        private readonly string _ip;
        private readonly int _port;
        private readonly string _com;
        private readonly int _baud;
        private readonly int _retry;
        private readonly int _timeoutMs;

        // driver HSL (giữ kiểu object để tránh phụ thuộc base class khác phiên bản)
        private object _plc;
        private readonly object _commLock = new object();

        // read loop (one-bit list)
        private CancellationTokenSource _loopCts;
        private Task _loopTask;

        public event Action<bool[], string> OnBitsRead;
        public event Action<string> OnError;
        public event Action OnReconnected;
        public event Action OnDisconnected;
        private bool dtrEnable, rtsEnable;
        public PlcClient(
            PlcBrand brand,
            ConnectionType connType,
            string ip = "",
            int port = 0,
            string comPort = null, int baudRate = 9600,
            byte _SlaveID = 1,
            System.IO.Ports.Parity parity = System.IO.Ports.Parity.Even,
            System.IO.Ports.StopBits stopBits=System.IO.Ports.StopBits.Two,
            int databit=7,
            bool DtrEnable=true,
            bool RtsEnable=true,
            int retryCount = 3, int timeoutMs = 2000)
        {
            _brand = brand;
            _connType = connType;
            _ip = ip;
            _port = port;
            _com = comPort;
            _baud = baudRate;
            Parity = parity;
            SlaveID = _SlaveID;
            StopBits = stopBits;
            DataBits = databit;
            dtrEnable = DtrEnable;
            rtsEnable = RtsEnable;
            _retry = retryCount < 1 ? 1 : retryCount;
            _timeoutMs = timeoutMs < 500 ? 500 : timeoutMs;
        }
        HslCommunication.Authorization authorization = new Authorization();
        private System.IO.Ports.Parity Parity= System.IO.Ports.Parity.Even;
        private System.IO.Ports.StopBits StopBits= System.IO.Ports.StopBits.Two;
        private byte SlaveID = 0;
        private int DataBits = 7;
        // ====== tạo driver theo hãng/kết nối (không dùng base type) ======
        private object CreateDriver()
        {
            switch (_brand)
            {
                case PlcBrand.Keyence:
                    if (_connType == ConnectionType.Tcp)
                    {
                        var keyIP = new KeyenceMcNet(_ip, _port);
                        TrySetProp(keyIP, "ReceiveTimeOut", _timeoutMs);
                        TrySetProp(keyIP, "ConnectTimeOut", _timeoutMs);
                        return keyIP;
                    }
                    else
                    {

                        var keySp = new KeyenceNanoSerial();
                        //fx.LogNet = new LogNetSingle("fx485.log");
                        keySp.SerialPortInni(sp =>
                        {
                            sp.PortName = _com;
                            sp.BaudRate = _baud;
                            sp.DataBits = DataBits;                                  // 7
                            sp.Parity = Parity;        // Even
                            sp.StopBits = StopBits;       // 2   <-- đổi từ One -> Two
                            sp.RtsEnable = rtsEnable;
                            sp.DtrEnable = dtrEnable;
                            sp.ReadTimeout = _timeoutMs;
                            sp.WriteTimeout = _timeoutMs;
                        });
                        keySp.Station = SlaveID;            // đúng “PC No.” / Station no. đã set trong PLC (0..31)
                       // keySp.SumCheck = false;        // thử cả true/false (tùy PLC cấu hình checksum)
                        return keySp;
                    }
                case PlcBrand.Mitsubishi:
                    if (_connType == ConnectionType.Tcp)
                    {
                        var mc = new MelsecMcNet(_ip, _port);
                        TrySetProp(mc, "ReceiveTimeOut", _timeoutMs);
                        TrySetProp(mc, "ConnectTimeOut", _timeoutMs);

                       //  mc.NetworkNumber = 0x00;        // mạng nội bộ = 0
                      //  mc.NetworkStationNumber = 0xFF;   // thiết bị ngoài truy cập PLC qua ENET-ADP
                        return mc;
                    }
                    else
                    {
                     
                        var fx = new MelsecFxLinks();
                        //fx.LogNet = new LogNetSingle("fx485.log");
                        fx.SerialPortInni(sp =>
                        {
                            sp.PortName = _com;
                            sp.BaudRate = _baud;
                            sp.DataBits = DataBits;                                  // 7
                            sp.Parity = Parity;        // Even
                            sp.StopBits = StopBits;       // 2   <-- đổi từ One -> Two
                            sp.RtsEnable = rtsEnable;
                            sp.DtrEnable = dtrEnable;
                            sp.ReadTimeout = _timeoutMs;
                            sp.WriteTimeout = _timeoutMs;
                        });
                        fx.Station = 0;            // đúng “PC No.” / Station no. đã set trong PLC (0..31)
                        fx.SumCheck = false;        // thử cả true/false (tùy PLC cấu hình checksum)
                        return fx;
                    }
                case PlcBrand.Delta:
                    if (_connType == ConnectionType.Tcp)
                    { 
                        var mc = new DeltaTcpNet(_ip, _port);
                        TrySetProp(mc, "ReceiveTimeOut", _timeoutMs);
                        TrySetProp(mc, "ConnectTimeOut", _timeoutMs);
                        return mc;
                    }
                    else
                    {

                        var del = new DeltaSerial();
                        //fx.LogNet = new LogNetSingle("fx485.log");
                        del.SerialPortInni(sp =>
                        {
                            sp.PortName = _com;
                            sp.BaudRate = _baud;
                            sp.DataBits = DataBits;                                  // 7
                            sp.Parity = Parity;        // Even
                            sp.StopBits = StopBits;       // 2   <-- đổi từ One -> Two
                            sp.RtsEnable = rtsEnable;
                            sp.DtrEnable = dtrEnable;
                            sp.ReadTimeout = _timeoutMs;
                            sp.WriteTimeout = _timeoutMs;
                        });
                        del.Station = 0;            // đúng “PC No.” / Station no. đã set trong PLC (0..31)
                       
                        return del;
                    }
                case PlcBrand.Pana:
                    if (_connType == ConnectionType.Tcp)
                    {
                        var mc = new PanasonicMcNet(_ip, _port);
                        TrySetProp(mc, "ReceiveTimeOut", _timeoutMs);
                        TrySetProp(mc, "ConnectTimeOut", _timeoutMs);
                        return mc;
                    }
                    else
                    {

                        var del = new PanasonicMewtocol();
                        //fx.LogNet = new LogNetSingle("fx485.log");
                        del.SerialPortInni(sp =>
                        {
                            sp.PortName = _com;
                            sp.BaudRate = _baud;
                            sp.DataBits = DataBits;                                  // 7
                            sp.Parity = Parity;        // Even
                            sp.StopBits = StopBits;       // 2   <-- đổi từ One -> Two
                            sp.RtsEnable = rtsEnable;
                            sp.DtrEnable = dtrEnable;
                            sp.ReadTimeout = _timeoutMs;
                            sp.WriteTimeout = _timeoutMs;
                        });
                        del.Station = 0;            // đúng “PC No.” / Station no. đã set trong PLC (0..31)

                        return del;
                    }
                case PlcBrand.ModbusRtu:
                    if (_connType == ConnectionType.Tcp)
                    {
                        var mb = new ModbusTcpNet(_ip, _port);
                        // Station nếu cần: TrySetProp(mb, "Station", (byte)1);
                       
                        TrySetProp(mb, "ReceiveTimeOut", _timeoutMs);
                        return mb;
                    }
                    else
                    {
                        // ModbusRtu: tạo với cổng COM, cấu hình qua SerialPortInni
                        var rtu = new ModbusRtu();
                        rtu.SerialPortInni(sp =>
                        {
                           
                            sp.PortName = _com;
                            sp.BaudRate = _baud;
                            sp.DataBits = DataBits;                                  // 7
                            sp.Parity = Parity;        // Even
                            sp.StopBits = StopBits;       // 2   <-- đổi từ One -> Two
                            sp.RtsEnable = rtsEnable;
                            sp.DtrEnable = dtrEnable;
                            sp.ReadTimeout = _timeoutMs;
                            sp.WriteTimeout = _timeoutMs;
                        });
                        rtu.Station = SlaveID;

                        return rtu;
                    }
                case PlcBrand.ModbusAscii:
                    if (_connType == ConnectionType.Tcp)
                    {
                        var mb = new ModbusTcpNet(_ip, _port);
                        // Station nếu cần: TrySetProp(mb, "Station", (byte)1);

                        TrySetProp(mb, "ReceiveTimeOut", _timeoutMs);
                        return mb;
                    }
                    else
                    {
                        // ModbusRtu: tạo với cổng COM, cấu hình qua SerialPortInni
                        var rtu = new ModbusAscii();
                        rtu.SerialPortInni(sp =>
                        {

                            sp.PortName = _com;
                            sp.BaudRate = _baud;
                            sp.DataBits = DataBits;                                  // 7
                            sp.Parity = Parity;        // Even
                            sp.StopBits = StopBits;       // 2   <-- đổi từ One -> Two
                            sp.RtsEnable = rtsEnable;
                            sp.DtrEnable = dtrEnable;
                            sp.ReadTimeout = _timeoutMs;
                            sp.WriteTimeout = _timeoutMs;
                        });
                        rtu.Station = SlaveID;
                        return rtu;
                    }
                case PlcBrand.Omron:
                    // Chỉ TCP để giản lược (HostLink Serial khác class giữa các bản Hsl)
                    var fins = new OmronFinsNet(_ip, _port);
                    TrySetProp(fins, "ReceiveTimeOut", _timeoutMs);
                    return fins;

                case PlcBrand.Siemens:
                    var s7 = new SiemensS7Net(SiemensPLCS.S1200, _ip);
                    if (_port > 0) TrySetProp(s7, "Port", _port);
                    TrySetProp(s7, "ReceiveTimeOut", _timeoutMs);
                    return s7;

                default:
                    throw new NotSupportedException("PLC brand chưa hỗ trợ.");
            }
        }

        // ====== Connect / Disconnect (không dùng NetworkDeviceBase/SerialDeviceBase) ======
        public bool Connect()
        {
            _plc = CreateDriver();
            Global.PLCStatus = PLCStatus.NotConnect;
            for (int i = 0; i < _retry; i++)
            {
                try
                {
                    if (_connType == ConnectionType.Tcp)
                    {
                        // gọi ConnectServer() nếu có, nếu không có thì coi như lazy connect
                        var ok = TryCall(_plc, "ConnectServer");
                        if (ok == null) return false;      // method không tồn tại → lazy connect
                        if (ok == true)
                        {
                            Global.IsAllowReadPLC = true;
                            return true;      // IsSuccess == true
                        }
                    }
                    else
                    {
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Connect", "Reconnect: "+_com));
                        // Serial: gọi Open()
                        var opened = TryCallVoid(_plc, "Open");

                        if (opened)
                        {
                            Global.IsAllowReadPLC = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }    
                    }
                }
                catch (Exception ex)
                {
                    return false;
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "IO", "Connect Fail"));
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(300);
            }
            return false;
        }

        public void Disconnect()
        {
            try
            {
                if (_connType == ConnectionType.Tcp)
                    TryCallVoid(_plc, "ConnectClose");
                else
                    TryCallVoid(_plc, "Close");
            }
            catch { }
            _plc = null;
        }

        private bool Reconnect()
        {
            Disconnect();
            return Connect();
        }

        // ====== Read/Write helpers (có retry + lock) ======
        private T WithRetry<T>(Func<T> f, string op)
        {
            for (int i = 0; i < _retry; i++)
            {
                try { return f(); }
                catch
                {
                    if (i == _retry - 1) throw;
                    if (!Reconnect()) Thread.Sleep(400);
                }
            }
            throw new Exception(op + " failed");
        }
        private void WithRetry(Action f, string op) { WithRetry<object>(() => { f(); return null; }, op); }
        public bool IsConnect = false;
        bool IsReadErr = false;
        // ====== Public API ======
        public bool[] ReadWordAsBits(string wordAddr)
        {
            return WithRetry(() =>
            {
                lock (_commLock)
                {
                    EnsureConnected();
                    dynamic d = _plc;
                    OperateResult<short> r = d.ReadInt16(wordAddr);
                    if (!r.IsSuccess)
                    {
                        IsReadErr = true;


                    }
                    else
                    {
                        IsConnect = true;
                        IsReadErr = false;
                    }

                        short w = r.Content;
                    var bits = new bool[16];
                    for (int i = 0; i < 16; i++) bits[i] = ((w >> i) & 1) == 1;
                    return bits;
                }
            }, "ReadWordAsBits");
        }

        public void WriteWord(string wordAddr, short value)
        {
            WithRetry(() =>
            {
                lock (_commLock)
                {
                X: EnsureConnected();
                    dynamic d = _plc;
                  //  CT2.Restart();
                   
                   
                    OperateResult w = d.Write(wordAddr, value);
                    //  CT2.Stop();

                    //  CTWrite = (float)CT2.Elapsed.TotalMilliseconds;
                    if (!w.IsSuccess)
                    {
                        NumErr++;
                        if (NumErr > Global.ParaCommon.NumRetryPLC)
                        {
                            bool ok = false;
                            for (int i = 0; i < _retry; i++)
                            {
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteW", "Reconnect  " + _retry));

                                if (Reconnect()) { ok = true; var rcb = OnReconnected; if (rcb != null) rcb(); break; }
                                Thread.Sleep(100);
                            }
                            if (!ok)
                            {
                                var dc = OnDisconnected; if (dc != null) dc();
                                NumErr = 0;
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteW", "FailReconnect "));
                                IsConnect = false;
                                Global.IsAllowReadPLC = false;
                                Global.PLCStatus = PLCStatus.ErrorConnect;
                                return;
                            }
                            else
                                NumErr = 0;
                        }    
                            
                        else
                        {
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteW", "Retry  " + NumErr));
                            goto X;
                        }

                        //Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", wordAddr + ": " + w.Message));
                    }
                    else
                        NumErr = 0;




                }
            }, "WriteWord");
        }
        public void WriteFloatArray(string startAddr, float[] values)
        {
            if (values == null || values.Length == 0) return;

            WithRetry(() =>
            {
                lock (_commLock)
                {
                    EnsureConnected();
                    dynamic d = _plc;

                    // Nếu driver hỗ trợ, đây là one-shot write thật sự, không cần for
                    OperateResult w = d.Write(startAddr, values);

                    if (!w.IsSuccess)
                    {
                       // IsConnect = false;
                        Global.LogsDashboard.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "WriteFloatArray_OneShot", $"{startAddr} (N={values.Length}): {w.Message}"));
                    }
                    else IsConnect = true;
                }
            }, "WriteFloatArray_OneShot");
        }
        public void WriteFloat(string startAddr, float value)
        {
       

            WithRetry(() =>
            {
                lock (_commLock)
                {
                    EnsureConnected();
                    dynamic d = _plc;

                    // Nếu driver hỗ trợ, đây là one-shot write thật sự, không cần for
                    OperateResult w = d.Write(startAddr, value);

                    if (!w.IsSuccess)
                    {
                      //  IsConnect = false;
                        Global.LogsDashboard.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "WriteFloatArray_OneShot", $"{startAddr} : {w.Message}"));
                    }
                    else IsConnect = true;
                }
            }, "WriteFloat");
        }

        //public void WriteFloat(string startAddr, float value)
        //{
        //    WithRetry(() =>
        //    {
        //        lock (_commLock)
        //        {
        //            EnsureConnected();
        //            dynamic d = _plc;

        //            // Convert float -> 4 byte (little endian của .NET)
        //            byte[] bytes = BitConverter.GetBytes(value);

        //            // Tùy PLC: có thể phải đảo ngược thứ tự byte
        //            // Ví dụ Modbus big-endian: swap 2 byte trong mỗi word hoặc đảo cả 4 byte
        //            // Ở đây demo Little Endian (lo-hi, lo-hi)
        //            short low = BitConverter.ToInt16(bytes, 0);
        //            short high = BitConverter.ToInt16(bytes, 2);

        //            OperateResult w = d.Write(startAddr, new short[] { low, high });
        //            if (!w.IsSuccess)
        //            {
        //                IsConnect = false;
        //                Global.LogsDashboard.AddLog(
        //                    new LogEntry(DateTime.Now, LeveLLog.ERROR,
        //                    "WriteIO", startAddr + ": " + w.Message));
        //            }
        //            else IsConnect = true;
        //        }
        //    }, "WriteFloat");
        //}
        public void WriteString(string startAddr, string text, int maxLength)
        {
            WithRetry(() =>
            {
                lock (_commLock)
                {
                    EnsureConnected();
                    dynamic d = _plc;

                    // Giới hạn chiều dài
                    if (text.Length > maxLength)
                        text = text.Substring(0, maxLength);

                    // Encode ra bytes (ASCII hoặc UTF8 tùy PLC/HMI, đa số ASCII)
                    byte[] bytes = Encoding.ASCII.GetBytes(text);

                    // Đệm thêm 0 nếu chưa đủ số byte chẵn (để ghép thành word)
                    if (bytes.Length % 2 != 0)
                    {
                        Array.Resize(ref bytes, bytes.Length + 1);
                    }

                    // Chuyển byte -> short[]
                    short[] words = new short[bytes.Length / 2];
                    for (int i = 0; i < words.Length; i++)
                    {
                        words[i] = BitConverter.ToInt16(bytes, i * 2);
                    }

                    OperateResult w = d.Write(startAddr, words);
                    if (!w.IsSuccess)
                    {
                        Global.LogsDashboard.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "WriteIO", startAddr + ": " + w.Message));
                    }
                }
            }, "WriteString");
        }
        public bool ReadBit(string addrOrWordDotBit)
        {
            return WithRetry(() =>
            {
                lock (_commLock)
                {
                    EnsureConnected();
                    dynamic d = _plc;

                    int dot = addrOrWordDotBit.LastIndexOf('.');
                    if (dot > 0 && dot < addrOrWordDotBit.Length - 1)
                    {
                        string wAddr = addrOrWordDotBit.Substring(0, dot);
                        int bit = int.Parse(addrOrWordDotBit.Substring(dot + 1));
                        if (bit < 0 || bit > 15) throw new ArgumentException("Bit index 0..15");
                        var r = d.ReadInt16(wAddr);
                        if (!r.IsSuccess) throw new Exception("Read " + wAddr + " lỗi: " + r.Message);
                        short w = r.Content;
                        return ((w >> bit) & 1) == 1;
                    }
                    else
                    {
                        var r = d.ReadBool(addrOrWordDotBit);
                        if (!r.IsSuccess) throw new Exception("Read " + addrOrWordDotBit + " lỗi: " + r.Message);
                        return r.Content;
                    }
                }
            }, "ReadBit");
        }

        public void WriteBit(string addrOrWordDotBit, bool value)
        {
            WithRetry(() =>
            {
                lock (_commLock)
                {
                    EnsureConnected();
                    dynamic d = _plc;

                    int dot = addrOrWordDotBit.LastIndexOf('.');
                    if (dot > 0 && dot < addrOrWordDotBit.Length - 1)
                    {
                        string wAddr = addrOrWordDotBit.Substring(0, dot);
                        int bit = int.Parse(addrOrWordDotBit.Substring(dot + 1));
                        if (bit < 0 || bit > 15)
                        {
                            IsConnect = false;
                            Global.LogsDashboard.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "WriteIO", "Bit index 0..15"));
                        }


                    X: var r = d.ReadInt16(wAddr);
                        if (!r.IsSuccess)
                        {
                            Global.LogsDashboard.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "WriteIO", "Read " + wAddr + " lỗi: " + r.Message));
                            NumErr++;
                            if (NumErr > Global.ParaCommon.NumRetryPLC)
                            {
                                bool ok = false;
                                for (int i = 0; i < _retry; i++)
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", "Reconnect  " + _retry));
                                    if (Reconnect()) { ok = true; var rcb = OnReconnected; if (rcb != null) rcb(); break; }
                                    Thread.Sleep(100);

                                }
                                if (!ok)
                                {
                                    NumErr = 0;
                                   
                                    var dc = OnDisconnected; if (dc != null) dc();
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", "FailReconnect " ));
                                    IsConnect = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.PLCStatus = PLCStatus.ErrorConnect;
                                    return;
                                }
                                else
                                    NumErr = 0;
                            }
                            else
                            {
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR,"WriteIO", "Retry  " + NumErr));
                                goto X;
                            }    
                              
                           

                        }
                      
                        short w = r.Content;
                        if (value) w = (short)(w | (1 << bit));
                        else w = (short)(w & ~(1 << bit));

                        var wres = d.Write(wAddr, w);
                        if (!wres.IsSuccess)
                        {
                           
                            Global.LogsDashboard.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "WriteIO", "Write " + wAddr + " lỗi: " + wres.Message));
                            NumErr++;
                            if (NumErr > Global.ParaCommon.NumRetryPLC)
                            {
                                bool ok = false;
                                for (int i = 0; i < _retry; i++)
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", "Reconnect  " + _retry));
                                    if (Reconnect()) { ok = true; var rcb = OnReconnected; if (rcb != null) rcb(); break; }
                                    Thread.Sleep(100);

                                }
                                if (!ok)
                                {
                                    NumErr = 0;
                                  
                                    var dc = OnDisconnected; if (dc != null) dc();
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", "FailReconnect "));
                                    IsConnect = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.PLCStatus = PLCStatus.ErrorConnect;
                                    return;
                                }
                                else
                                    NumErr = 0;
                            }   
                            else
                            {
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", "Retry  " + NumErr));
                                goto X;
                            }
                        }
                        
                    }
                    else
                    {
                    Y: var res = d.Write(addrOrWordDotBit, value);
                        if (!res.IsSuccess)
                        {
                         
                            Global.LogsDashboard.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "WriteIO", "Write " + addrOrWordDotBit + " lỗi: " + res.Message));
                            NumErr++;
                            if (NumErr > Global.ParaCommon.NumRetryPLC)
                            {
                                bool ok = false;
                                for (int i = 0; i < _retry; i++)
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", "Reconnect  " + _retry));
                                    if (Reconnect()) { ok = true; var rcb = OnReconnected; if (rcb != null) rcb(); break; }
                                    Thread.Sleep(100);
                                }
                                if (!ok)
                                {
                                    
                                    var dc = OnDisconnected; if (dc != null) dc();
                                    NumErr = 0;
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", "FailReconnect "));
                                    IsConnect = false;
                                    Global.IsAllowReadPLC = false;
                                    Global.PLCStatus = PLCStatus.ErrorConnect;
                                    return;
                                }
                                else
                                    NumErr = 0;
                            }
                            else
                                goto Y;
                        }
                        
                    }
                }
            }, "WriteBit");
        }

        public bool[] ReadBits(string[] addresses)
        {
            if (addresses == null || addresses.Length == 0) return new bool[0];

            return WithRetry(() =>
            {
                lock (_commLock)
                {
                    EnsureConnected();
                    var vals = new bool[addresses.Length];
                    for (int i = 0; i < addresses.Length; i++)
                        vals[i] = ReadBit_NoRetry_NoLock(addresses[i]);
                    return vals;
                }
            }, "ReadBits");
        }
        [NonSerialized]
        Stopwatch CT = new Stopwatch();
        Stopwatch CT2 = new Stopwatch();
        [NonSerialized]
        public int NumErr = 0;
        [NonSerialized]
        public float CTRead, CTWrite;
      
        // ====== OneBitRead loop (trả về mảng bits) ======
        public bool  StartOneBitReadLoop(string addresses, int cycleMs = 500)
        {
            StopOneBitReadLoop();
            if (addresses == null || addresses.Length == 0)
            {
                IsConnect = false;
                Global.LogsDashboard.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR,
                    "ReadPLC", "Add Wrong"));
                return false;
            }    
               

            var addrs = (string)addresses.Clone();
            _loopCts = new CancellationTokenSource();
            var token = _loopCts.Token;

            _loopTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                X: try
                    {
                        if (Global.IsAllowReadPLC)
                        {
                            //  CT.Restart();
                            var values = ReadWordAsBits(addresses);
                            if (IsReadErr)
                            {
                                IsReadErr = false;
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Fail Read"+ NumErr));
                                NumErr++;
                                if (NumErr > Global.ParaCommon.NumRetryPLC)
                                {
                                    bool ok = false;
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Reconnect  " + _retry));
                                    if (Reconnect())
                                    {
                                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Reconnect sucess "));

                                        ok = true; var rcb = OnReconnected; if (rcb != null) rcb(); break; }
                                    if (!ok)
                                    {
                                       
                                       
                                        NumErr = 0;
                                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "FailReconnect "));
                                        IsConnect = false;
                                 
                                        Global.PLCStatus = PLCStatus.ErrorConnect;
                                        Global.IsAllowReadPLC = false;
                                    }
                                    else
                                        NumErr = 0;
                                }
                                else
                                {
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Retry  " + NumErr));
                                    goto X;
                                }
                            }
                            var handler = OnBitsRead;
                            if (handler != null) handler(values, addrs);
                        }
                        NumErr=0;
                    }
                    catch (Exception ex)
                    {
                      
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Fail Read"));
                        NumErr++;
                        if (NumErr > Global.ParaCommon.NumRetryPLC)
                        {
                            var onErr = OnError; if (onErr != null) onErr(ex.Message);
                            bool ok = false;
                            for (int i = 0; i < _retry; i++)
                            {
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Reconnect  " + _retry));
                                if (Reconnect()) { ok = true; var rcb = OnReconnected; if (rcb != null) rcb(); break; }
                                Thread.Sleep(100);
                            }
                            if (!ok)
                            {
                                Global.IsAllowReadPLC=false;

                                var dc = OnDisconnected; if (dc != null) dc();
                               NumErr = 0;
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "FailReconnect "));
                                IsConnect = false;
                                Global.IsAllowReadPLC = false;
                                Global.PLCStatus = PLCStatus.ErrorConnect;
                                break;
                            }
                            else
                                NumErr = 0;
                        }   
                        else
                        {
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Retry  " + NumErr));
                            goto X;
                        }

                    }

                    Thread.Sleep(cycleMs < 50 ? 50 : cycleMs);
                }
            }, token);
            return true;
        }

        public void StopOneBitReadLoop()
        {
            if (_loopCts == null) return;
            try { _loopCts.Cancel(); _loopTask?.Wait(800); } catch { }
            _loopTask = null;
            _loopCts = null;
        }

        // ====== helpers ======
        private void EnsureConnected()
        {
            if (_plc == null)
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "IO", "PLC chưa kết nối"));
         //   throw new InvalidOperationException("PLC chưa kết nối.");
        }

        private bool ReadBit_NoRetry_NoLock(string addr)
        {
            dynamic d = _plc;

            int dot = addr.LastIndexOf('.');
            if (dot > 0 && dot < addr.Length - 1)
            {
                string wAddr = addr.Substring(0, dot);
                int bit = int.Parse(addr.Substring(dot + 1));
                var r = d.ReadInt16(wAddr);
                if (!r.IsSuccess) throw new Exception("Read " + wAddr + " lỗi: " + r.Message);
                short w = r.Content;
                return ((w >> bit) & 1) == 1;
            }
            else
            {

                var r = d.ReadBool(addr);
                if (!r.IsSuccess) throw new Exception("Read " + addr + " lỗi: " + r.Message);
                return r.Content;
            }
        }

        // gọi ConnectServer() trả về bool? (null nếu method không tồn tại)
        private bool? TryCall(object obj, string method)
        {
            if (obj == null) return null;
            var mi = obj.GetType().GetMethod(method);
            if (mi == null) return null;
            var ret = mi.Invoke(obj, new object[0]);
            // với Hsl, ConnectServer trả OperateResult => check IsSuccess
            var prop = ret.GetType().GetProperty("IsSuccess");
            if (prop != null) return (bool)prop.GetValue(ret, null);
            return true;
        }

        private bool TryCallVoid(object obj, string method)
        {
            if (obj == null) return false;
            var mi = obj.GetType().GetMethod(method);
            if (mi == null) return false;
            mi.Invoke(obj, new object[0]);
            return true;
        }

        private void TrySetProp(object obj, string propName, object value)
        {
            if (obj == null) return;
            var p = obj.GetType().GetProperty(propName);
            if (p != null && p.CanWrite) p.SetValue(obj, value, null);
        }
    }
}
