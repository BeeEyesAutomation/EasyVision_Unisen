// PlcClient_Compat73.cs  —  C# 7.3 compatible
using BeeGlobal;
using HslCommunication;
using HslCommunication.LogNet;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Keyence;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Siemens;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PlcLib
{
    [Serializable()]
    public enum PlcBrand { Mitsubishi,Keyence, Omron, Siemens, Modbus }
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
            string ip = null,
            int port = 0,
            string comPort = null, int baudRate = 9600,
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
        private int DataBits = 7;
        // ====== tạo driver theo hãng/kết nối (không dùng base type) ======
        private object CreateDriver()
        {
            switch (_brand)
            {
                case PlcBrand.Mitsubishi:
                    if (_connType == ConnectionType.Tcp)
                    {
                        var mc = new MelsecMcNet(_ip, _port);
                        TrySetProp(mc, "ReceiveTimeOut", _timeoutMs);
                        TrySetProp(mc, "ConnectTimeOut", _timeoutMs);
                        return mc;
                    }
                    else
                    {
                        // Ctor không tham số, cấu hình COM qua SerialPortInni
                        //var fx = new MelsecFxSerial();
                        //fx.SerialPortInni(sp =>
                        //{
                        //    sp.PortName = _com;
                        //    sp.BaudRate = _baud;
                        //    sp.DataBits = 7; // thường 7E1 cho FX (đổi nếu bạn cấu hình khác)
                        //    sp.Parity = System.IO.Ports.Parity.Even;
                        //    sp.StopBits = System.IO.Ports.StopBits.One;
                        //});
                        //return fx;
                        var fx = new MelsecFxLinks();
                        fx.LogNet = new LogNetSingle("fx485.log");
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

                case PlcBrand.Modbus:
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
                            sp.DataBits = 8;
                            sp.Parity = System.IO.Ports.Parity.None;
                            sp.StopBits = System.IO.Ports.StopBits.One;
                        });
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

            for (int i = 0; i < _retry; i++)
            {
                try
                {
                    if (_connType == ConnectionType.Tcp)
                    {
                        // gọi ConnectServer() nếu có, nếu không có thì coi như lazy connect
                        var ok = TryCall(_plc, "ConnectServer");
                        if (ok == null) return true;      // method không tồn tại → lazy connect
                        if (ok == true) return true;      // IsSuccess == true
                    }
                    else
                    {
                        // Serial: gọi Open()
                        var opened = TryCallVoid(_plc, "Open");
                        if (opened) return true;
                    }
                }
                catch (Exception ex)
                {
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
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadIO", wordAddr+": "+ r.Message));
                    
                       
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
                    EnsureConnected();
                    dynamic d = _plc;
                    OperateResult w = d.Write(wordAddr, value);
                    if (!w.IsSuccess)
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "WriteIO", wordAddr + ": " + w.Message));

                   
                }
            }, "WriteWord");
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
                        if (bit < 0 || bit > 15) throw new ArgumentException("Bit index 0..15");

                        var r = d.ReadInt16(wAddr);
                        if (!r.IsSuccess) throw new Exception("Read " + wAddr + " lỗi: " + r.Message);
                        short w = r.Content;
                        if (value) w = (short)(w | (1 << bit));
                        else w = (short)(w & ~(1 << bit));

                        var wres = d.Write(wAddr, w);
                        if (!wres.IsSuccess) throw new Exception("Write " + wAddr + " lỗi: " + wres.Message);
                    }
                    else
                    {
                        var res = d.Write(addrOrWordDotBit, value);
                        if (!res.IsSuccess) throw new Exception("Write " + addrOrWordDotBit + " lỗi: " + res.Message);
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

        // ====== OneBitRead loop (trả về mảng bits) ======
        public void StartOneBitReadLoop(string addresses, int cycleMs = 500)
        {
            StopOneBitReadLoop();
            if (addresses == null || addresses.Length == 0)
                throw new ArgumentException("Danh sách địa chỉ trống.");

            var addrs = (string)addresses.Clone();
            _loopCts = new CancellationTokenSource();
            var token = _loopCts.Token;

            _loopTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var values = ReadWordAsBits(addresses);
                        var handler = OnBitsRead;
                        if (handler != null) handler(values, addrs);
                    }
                    catch (Exception ex)
                    {
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadIO", "Fail Read"));
                        var onErr = OnError; if (onErr != null) onErr(ex.Message);

                        bool ok = false;
                        for (int i = 0; i < _retry; i++)
                        {
                            if (Reconnect()) { ok = true; var rcb = OnReconnected; if (rcb != null) rcb(); break; }
                            Thread.Sleep(800);
                        }
                        if (!ok)
                        {
                            var dc = OnDisconnected; if (dc != null) dc();
                            break;
                        }
                    }

                    Thread.Sleep(cycleMs < 50 ? 50 : cycleMs);
                }
            }, token);
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
