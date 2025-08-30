using EasyModbus;
using System;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public enum ModbusTransport { Tcp, SerialRtu }
    public enum ModbusRole { ClientMaster, ServerSlave }

    [Serializable()]
    public sealed class ModbusOptions
    {
        // Chung
        public ModbusTransport Transport { get; set; } = ModbusTransport.Tcp;
        public ModbusRole Role { get; set; } = ModbusRole.ClientMaster;

        // TCP
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 502;

        // RTU
        public string ComPort { get; set; } = "COM1";
        public int Baudrate { get; set; } = 9600;
        public Parity Parity { get; set; } = Parity.None;
        public StopBits StopBits { get; set; } = StopBits.One;
        public byte UnitId { get; set; } = 1;

        // Timeout & retry
        public int ConnectTimeoutMs { get; set; } = 1500;
        public int OperationTimeoutMs { get; set; } = 1500;
        public int Retries { get; set; } = 1;
        public int RetryDelayMs { get; set; } = 150;

        // Server monitor
        public int MonitorIntervalMs { get; set; } = 50;
        public int InitialMapSize { get; set; } = 1000; // số ô theo dõi/khởi tạo snapshot
    }

    [Serializable()]
    public sealed class ModbusService : IDisposable
    {
        private readonly ModbusOptions _opt;
        private readonly SemaphoreSlim _ioLock = new SemaphoreSlim(1, 1);

        private ModbusClient _client;   // khi Client/Master
        private ModbusServer _server;   // khi Server/Slave
        private volatile bool _started;
        private volatile bool _disposed;

        // Monitor (Server)
        private CancellationTokenSource _monitorCts;
        private Task _monitorTask;
        private int[] _lastHolding;
        private bool[] _lastCoils;
        private int[] _lastInput;
        private bool[] _lastDiscrete;

        // Sự kiện khi Master ghi
        public event Action<int, int> OnHoldingRegistersWritten; // (start, qty)
        public event Action<int, int> OnCoilsWritten;            // (start, qty)

        public bool IsConnected
        {
            get { return _opt.Role == ModbusRole.ClientMaster && _client != null && _client.Connected; }
        }

        public bool IsServerRunning
        {
            get { return _opt.Role == ModbusRole.ServerSlave && _server != null && _started; }
        }

        public ModbusService(ModbusOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _opt = options;
        }

        // ===================== Vòng đời =====================

        public async Task StartAsync(CancellationToken ct = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (_started) return;

            if (_opt.Role == ModbusRole.ServerSlave)
            {
                _server = new ModbusServer();
                _server.UnitIdentifier = _opt.UnitId;

                if (_opt.Transport == ModbusTransport.Tcp)
                {
                    _server.Port = _opt.Port;
                    await Task.Run(new Func<Task>(delegate { _server.Listen(); return Task.CompletedTask; }), ct);
                }
                else
                {
                    _server.SerialPort = _opt.ComPort;
                    _server.Baudrate = _opt.Baudrate;
                    _server.Parity = _opt.Parity;
                    _server.StopBits = _opt.StopBits;
                    _server.Listen(); // RTU
                }

                InitSnapshots();
                StartMonitor();

                _started = true;
            }
            else
            {
                // Client/Master: lazy connect khi I/O
                _started = true;
            }
        }

        public Task StopAsync()
        {
            ThrowIfDisposed();
            if (!_started) return Task.CompletedTask;

            if (_opt.Role == ModbusRole.ServerSlave)
            {
                StopMonitor();
                try { if (_server != null) _server.StopListening(); } catch { }
                _server = null;
            }
            else
            {
                try { if (_client != null) _client.Disconnect(); } catch { }
                _client = null;
            }

            _started = false;
            return Task.CompletedTask;
        }

        // ===================== Client/Master =====================

        private void EnsureClient()
        {
            if (_client != null) return;

            if (_opt.Transport == ModbusTransport.Tcp)
            {
                _client = new ModbusClient(_opt.Host, _opt.Port);
                _client.ConnectionTimeout = _opt.ConnectTimeoutMs;
                _client.UnitIdentifier = _opt.UnitId;
            }
            else
            {
                _client = new ModbusClient(_opt.ComPort);
                _client.Baudrate = _opt.Baudrate;
                _client.Parity = _opt.Parity;
                _client.StopBits = _opt.StopBits;
                _client.ConnectionTimeout = _opt.ConnectTimeoutMs;
                _client.UnitIdentifier = _opt.UnitId;
            }
        }

        private async Task EnsureConnectedAsync(CancellationToken ct)
        {
            EnsureClient();
            if (_client.Connected) return;

            await RunWithTimeoutAsync(new Func<Task>(delegate
            {
                _client.Connect();
                return Task.CompletedTask;
            }), _opt.OperationTimeoutMs, ct);
        }

        public async Task<bool[]> ReadCoilsAsync(int startAddress, int count, CancellationToken ct = default(CancellationToken))
        {
            return await WithIoLock<bool[]>(async delegate (CancellationToken token)
            {
                await EnsureConnectedAsync(token);
                return await RetryAsync<bool[]>(async delegate
                {
                    return await RunWithTimeoutAsync<bool[]>(delegate
                    {
                        bool[] data = _client.ReadCoils(startAddress, count);
                        return Task.FromResult<bool[]>(data);
                    }, _opt.OperationTimeoutMs, token);
                }, token);
            }, ct);
        }

        public async Task<bool[]> ReadDiscreteInputsAsync(int startAddress, int count, CancellationToken ct = default(CancellationToken))
        {
            return await WithIoLock<bool[]>(async delegate (CancellationToken token)
            {
                await EnsureConnectedAsync(token);
                return await RetryAsync<bool[]>(async delegate
                {
                    return await RunWithTimeoutAsync<bool[]>(delegate
                    {
                        bool[] data = _client.ReadDiscreteInputs(startAddress, count);
                        return Task.FromResult<bool[]>(data);
                    }, _opt.OperationTimeoutMs, token);
                }, token);
            }, ct);
        }

        public async Task<int[]> ReadHoldingRegistersAsync(int startAddress, int count, CancellationToken ct = default(CancellationToken))
        {
            return await WithIoLock<int[]>(async delegate (CancellationToken token)
            {
                await EnsureConnectedAsync(token);
                return await RetryAsync<int[]>(async delegate
                {
                    return await RunWithTimeoutAsync<int[]>(delegate
                    {
                        int[] data = _client.ReadHoldingRegisters(startAddress, count);
                        return Task.FromResult<int[]>(data);
                    }, _opt.OperationTimeoutMs, token);
                }, token);
            }, ct);
        }

        public async Task<int[]> ReadInputRegistersAsync(int startAddress, int count, CancellationToken ct = default(CancellationToken))
        {
            return await WithIoLock<int[]>(async delegate (CancellationToken token)
            {
                await EnsureConnectedAsync(token);
                return await RetryAsync<int[]>(async delegate
                {
                    return await RunWithTimeoutAsync<int[]>(delegate
                    {
                        int[] data = _client.ReadInputRegisters(startAddress, count);
                        return Task.FromResult<int[]>(data);
                    }, _opt.OperationTimeoutMs, token);
                }, token);
            }, ct);
        }

        public async Task WriteSingleCoilAsync(int address, bool value, CancellationToken ct = default(CancellationToken))
        {
            await WithIoLock<bool>(async delegate (CancellationToken token)
            {
                await EnsureConnectedAsync(token);
                return await RetryAsync<bool>(async delegate
                {
                    await RunWithTimeoutAsync(new Func<Task>(delegate
                    {
                        _client.WriteSingleCoil(address, value);
                        return Task.CompletedTask;
                    }), _opt.OperationTimeoutMs, token);
                    return true;
                }, token);
            }, ct);
        }

        public async Task WriteSingleRegisterAsync(int address, int value, CancellationToken ct = default(CancellationToken))
        {
            await WithIoLock<bool>(async delegate (CancellationToken token)
            {
                await EnsureConnectedAsync(token);
                return await RetryAsync<bool>(async delegate
                {
                    await RunWithTimeoutAsync(new Func<Task>(delegate
                    {
                        _client.WriteSingleRegister(address, value);
                        return Task.CompletedTask;
                    }), _opt.OperationTimeoutMs, token);
                    return true;
                }, token);
            }, ct);
        }

        public async Task<int[]> ReadHoldingRegisterBitsAsync(int address, CancellationToken ct = default(CancellationToken))
        {
            int[] reg = await ReadHoldingRegistersAsync(address, 1, ct);
            ushort v = (ushort)reg[0];
            int[] bits = new int[16];
            for (int i = 0; i < 16; i++) bits[i] = (v >> i) & 0x1;
            return bits;
        }

        // ===================== Server/Slave API =====================
        // Lưu ý: bản EasyModbus của bạn có thể trả về "wrapper classes" thay vì mảng.
        // Các hàm dưới đây dùng reflection để hỗ trợ cả 2 dạng.

        public void SetHolding(int address, int value)
        {
            EnsureServerModeStarted();
            SafeSetHolding(address, value);
        }

        public int[] GetHolding(int address, int count)
        {
            EnsureServerModeStarted();
            int[] res = new int[count];
            for (int i = 0; i < count; i++) res[i] = SafeGetHolding(address + i);
            return res;
        }

        public void SetInputRegister(int address, int value)
        {
            EnsureServerModeStarted();
            SafeSetInput(address, value);
        }

        public int[] GetInputRegisters(int address, int count)
        {
            EnsureServerModeStarted();
            int[] res = new int[count];
            for (int i = 0; i < count; i++) res[i] = SafeGetInput(address + i);
            return res;
        }

        public void SetCoil(int address, bool value)
        {
            EnsureServerModeStarted();
            SafeSetCoil(address, value);
        }

        public bool[] GetCoils(int address, int count)
        {
            EnsureServerModeStarted();
            bool[] res = new bool[count];
            for (int i = 0; i < count; i++) res[i] = SafeGetCoil(address + i);
            return res;
        }

        public void SetDiscreteInput(int address, bool value)
        {
            EnsureServerModeStarted();
            SafeSetDiscrete(address, value);
        }

        public bool[] GetDiscreteInputs(int address, int count)
        {
            EnsureServerModeStarted();
            bool[] res = new bool[count];
            for (int i = 0; i < count; i++) res[i] = SafeGetDiscrete(address + i);
            return res;
        }

        // ===================== Monitor thay đổi từ Master =====================

        private void StartMonitor()
        {
            if (_monitorCts != null) return;
            _monitorCts = new CancellationTokenSource();
            _monitorTask = Task.Run(new Action(delegate { MonitorLoop(_monitorCts.Token); }));
        }

        private void StopMonitor()
        {
            try
            {
                if (_monitorCts != null)
                {
                    _monitorCts.Cancel();
                    try { if (_monitorTask != null) _monitorTask.Wait(); } catch { }
                    _monitorTask = null;
                    _monitorCts.Dispose();
                    _monitorCts = null;
                }
            }
            catch { /* ignore */ }
        }

        private void InitSnapshots()
        {
            int n = Math.Max(1, _opt.InitialMapSize);
            _lastHolding = new int[n];
            _lastCoils = new bool[n];
            _lastInput = new int[n];
            _lastDiscrete = new bool[n];

            for (int i = 0; i < n; i++)
            {
                _lastHolding[i] = SafeGetHolding(i);
                _lastCoils[i] = SafeGetCoil(i);
                _lastInput[i] = SafeGetInput(i);
                _lastDiscrete[i] = SafeGetDiscrete(i);
            }
        }

        private void MonitorLoop(CancellationToken token)
        {
            int n = _lastHolding != null ? _lastHolding.Length : _opt.InitialMapSize;
            if (n <= 0) n = 1;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // HoldingRegisters
                    int first = -1, last = -1;
                    for (int i = 0; i < n; i++)
                    {
                        int v = SafeGetHolding(i);
                        if (v != _lastHolding[i])
                        {
                            if (first == -1) first = i;
                            last = i;
                            _lastHolding[i] = v;
                        }
                    }
                    if (first != -1)
                    {
                        Action<int, int> h = OnHoldingRegistersWritten;
                        if (h != null) h(first, last - first + 1);
                    }

                    // Coils
                    first = -1; last = -1;
                    for (int i = 0; i < n; i++)
                    {
                        bool v = SafeGetCoil(i);
                        if (v != _lastCoils[i])
                        {
                            if (first == -1) first = i;
                            last = i;
                            _lastCoils[i] = v;
                        }
                    }
                    if (first != -1)
                    {
                        Action<int, int> c = OnCoilsWritten;
                        if (c != null) c(first, last - first + 1);
                    }
                }
                catch
                {
                    // bỏ qua lỗi thoáng qua để vòng lặp không dừng
                }

                Thread.Sleep(_opt.MonitorIntervalMs);
            }
        }

        // ===================== Adapter (Server map) =====================

        // Dùng reflection để hỗ trợ cả 2 kiểu: field mảng (cũ) và wrapper class (mới).
        private int SafeGetHolding(int index)
        {
            try
            {
                // field int[] holdingRegisters
                FieldInfo fi = _server.GetType().GetField("holdingRegisters");
                if (fi != null && fi.FieldType == typeof(int[]))
                {
                    int[] arr = (int[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) return arr[index];
                    return 0;
                }

                // property HoldingRegisters (wrapper)
                PropertyInfo pi = _server.GetType().GetProperty("HoldingRegisters");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanRead)
                        {
                            object val = idxer.GetValue(wr, new object[] { index });
                            if (val is int iv) return iv;
                            if (val is short sv) return (int)sv;
                        }
                    }
                }
            }
            catch { }
            return 0;
        }

        private void SafeSetHolding(int index, int value)
        {
            try
            {
                FieldInfo fi = _server.GetType().GetField("holdingRegisters");
                if (fi != null && fi.FieldType == typeof(int[]))
                {
                    int[] arr = (int[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) arr[index] = value;
                    return;
                }

                PropertyInfo pi = _server.GetType().GetProperty("HoldingRegisters");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanWrite)
                        {
                            idxer.SetValue(wr, value, new object[] { index });
                            return;
                        }
                        MethodInfo setM = wr.GetType().GetMethod("Set") ??
                                          wr.GetType().GetMethod("SetHoldingRegister");
                        if (setM != null) { setM.Invoke(wr, new object[] { index, value }); return; }
                    }
                }
            }
            catch { }
        }

        private int SafeGetInput(int index)
        {
            try
            {
                FieldInfo fi = _server.GetType().GetField("inputRegisters");
                if (fi != null && fi.FieldType == typeof(int[]))
                {
                    int[] arr = (int[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) return arr[index];
                    return 0;
                }
                PropertyInfo pi = _server.GetType().GetProperty("InputRegisters");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanRead)
                        {
                            object val = idxer.GetValue(wr, new object[] { index });
                            if (val is int iv) return iv;
                            if (val is short sv) return (int)sv;
                        }
                    }
                }
            }
            catch { }
            return 0;
        }

        private void SafeSetInput(int index, int value)
        {
            try
            {
                FieldInfo fi = _server.GetType().GetField("inputRegisters");
                if (fi != null && fi.FieldType == typeof(int[]))
                {
                    int[] arr = (int[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) arr[index] = value;
                    return;
                }
                PropertyInfo pi = _server.GetType().GetProperty("InputRegisters");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanWrite)
                        {
                            idxer.SetValue(wr, value, new object[] { index });
                            return;
                        }
                        MethodInfo setM = wr.GetType().GetMethod("Set") ??
                                          wr.GetType().GetMethod("SetInputRegister");
                        if (setM != null) { setM.Invoke(wr, new object[] { index, value }); return; }
                    }
                }
            }
            catch { }
        }

        private bool SafeGetCoil(int index)
        {
            try
            {
                FieldInfo fi = _server.GetType().GetField("coils");
                if (fi != null && fi.FieldType == typeof(bool[]))
                {
                    bool[] arr = (bool[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) return arr[index];
                    return false;
                }
                PropertyInfo pi = _server.GetType().GetProperty("Coils");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanRead)
                        {
                            object val = idxer.GetValue(wr, new object[] { index });
                            if (val is bool bv) return bv;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        private void SafeSetCoil(int index, bool value)
        {
            try
            {
                FieldInfo fi = _server.GetType().GetField("coils");
                if (fi != null && fi.FieldType == typeof(bool[]))
                {
                    bool[] arr = (bool[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) arr[index] = value;
                    return;
                }
                PropertyInfo pi = _server.GetType().GetProperty("Coils");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanWrite)
                        {
                            idxer.SetValue(wr, value, new object[] { index });
                            return;
                        }
                        MethodInfo setM = wr.GetType().GetMethod("Set") ??
                                          wr.GetType().GetMethod("SetCoil");
                        if (setM != null) { setM.Invoke(wr, new object[] { index, value }); return; }
                    }
                }
            }
            catch { }
        }

        private bool SafeGetDiscrete(int index)
        {
            try
            {
                FieldInfo fi = _server.GetType().GetField("discreteInputs");
                if (fi != null && fi.FieldType == typeof(bool[]))
                {
                    bool[] arr = (bool[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) return arr[index];
                    return false;
                }
                PropertyInfo pi = _server.GetType().GetProperty("DiscreteInputs");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanRead)
                        {
                            object val = idxer.GetValue(wr, new object[] { index });
                            if (val is bool bv) return bv;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        private void SafeSetDiscrete(int index, bool value)
        {
            try
            {
                FieldInfo fi = _server.GetType().GetField("discreteInputs");
                if (fi != null && fi.FieldType == typeof(bool[]))
                {
                    bool[] arr = (bool[])fi.GetValue(_server);
                    if (index >= 0 && index < arr.Length) arr[index] = value;
                    return;
                }
                PropertyInfo pi = _server.GetType().GetProperty("DiscreteInputs");
                if (pi != null)
                {
                    object wr = pi.GetValue(_server, null);
                    if (wr != null)
                    {
                        PropertyInfo idxer = wr.GetType().GetProperty("Item");
                        if (idxer != null && idxer.CanWrite)
                        {
                            idxer.SetValue(wr, value, new object[] { index });
                            return;
                        }
                        MethodInfo setM = wr.GetType().GetMethod("Set") ??
                                          wr.GetType().GetMethod("SetDiscreteInput");
                        if (setM != null) { setM.Invoke(wr, new object[] { index, value }); return; }
                    }
                }
            }
            catch { }
        }

        // ===================== Hạ tầng chung =====================

        private async Task<T> WithIoLock<T>(Func<CancellationToken, Task<T>> body, CancellationToken ct)
        {
            ThrowIfDisposed();
            if (_opt.Role != ModbusRole.ClientMaster)
                throw new InvalidOperationException("Chỉ dùng API I/O ở chế độ Client/Master.");

            await _ioLock.WaitAsync(ct);
            try { return await body(ct); }
            finally { _ioLock.Release(); }
        }

        private async Task<T> RetryAsync<T>(Func<Task<T>> action, CancellationToken ct)
        {
            int attempts = 0;
            for (; ; )
            {
                ct.ThrowIfCancellationRequested();
                try { return await action(); }
                catch (Exception)
                {
                    attempts++;
                    if (attempts > _opt.Retries) throw;

                    await Task.Delay(_opt.RetryDelayMs, ct);
                    try { if (_client != null) _client.Disconnect(); } catch { }
                    await EnsureConnectedAsync(ct);
                }
            }
        }

        private static async Task<T> RunWithTimeoutAsync<T>(Func<Task<T>> action, int timeoutMs, CancellationToken ct)
        {
            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            try
            {
                cts.CancelAfter(timeoutMs);
                Task<T> task = action();
                Task completed = await Task.WhenAny(task, Task.Delay(Timeout.InfiniteTimeSpan, cts.Token));
                if (!object.ReferenceEquals(completed, task))
                    throw new TimeoutException("Modbus operation timed out.");
                return await task;
            }
            finally { cts.Dispose(); }
        }

        private static async Task RunWithTimeoutAsync(Func<Task> action, int timeoutMs, CancellationToken ct)
        {
            await RunWithTimeoutAsync<bool>(async delegate { await action(); return true; }, timeoutMs, ct);
        }

        private void EnsureServerModeStarted()
        {
            if (_opt.Role != ModbusRole.ServerSlave)
                throw new InvalidOperationException("Chỉ dùng API Server khi Role = ServerSlave.");
            if (!_started || _server == null)
                throw new InvalidOperationException("Server chưa Start().");
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ModbusService));
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try { StopMonitor(); } catch { }
            try { if (_server != null) _server.StopListening(); } catch { }
            try { if (_client != null) _client.Disconnect(); } catch { }
            _server = null;
            _client = null;
            _ioLock.Dispose();
        }
    }
}
