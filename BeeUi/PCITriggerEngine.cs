using BeeGlobal;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BeeCore
{
    public struct TriggerEvent
    {
        public long Timestamp;
        public int TriggerIndex;
        public uint RawValue;
    }

    public class PCITriggerEngine : IDisposable
    {
        private CancellationTokenSource _ctsRead;
        private CancellationTokenSource _ctsProcess;

        private Task _readTask;
        private Task _processTask;

        private volatile bool _isRunning = false;

        private readonly ConcurrentQueue<TriggerEvent> _triggerQueue = new ConcurrentQueue<TriggerEvent>();
        private readonly AutoResetEvent _triggerSignal = new AutoResetEvent(false);

        private int _sensorOn = 0;
        private uint _lastValue = 0;

        // debounce mềm: bỏ qua trigger quá sát nhau
        private long _lastTriggerTick = 0;
        private readonly long _debounceTicks;

        public event Action<TriggerEvent> TriggerReceived;
        public event Action<string> LogMessage;

        public bool IsRunning => _isRunning;

        /// <summary>
        /// debounceMs = 0 thì không debounce
        /// </summary>
        public PCITriggerEngine(int debounceMs = 0)
        {
            _debounceTicks = debounceMs <= 0
                ? 0
                : debounceMs * TimeSpan.TicksPerMillisecond;
        }

        public void StartReadPCI()
        {
            if (_isRunning) return;

            _ctsRead = new CancellationTokenSource();
            _ctsProcess = new CancellationTokenSource();

            _sensorOn = 0;
            _lastValue = 0;
            _lastTriggerTick = 0;

            _readTask = Task.Factory.StartNew(
                () => ReadLoop(_ctsRead.Token),
                _ctsRead.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            _processTask = Task.Factory.StartNew(
                () => ProcessLoop(_ctsProcess.Token),
                _ctsProcess.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            _isRunning = true;
            SafeLog("PCI reader started.");
        }

        public void StopReadPCI()
        {
            if (!_isRunning) return;

            try
            {
                _ctsRead?.Cancel();
                _ctsProcess?.Cancel();

                _triggerSignal.Set();

                try
                {
                    _readTask?.Wait(1000);
                }
                catch { }

                try
                {
                    _processTask?.Wait(1000);
                }
                catch { }
            }
            finally
            {
                _isRunning = false;

                _ctsRead?.Dispose();
                _ctsRead = null;

                _ctsProcess?.Dispose();
                _ctsProcess = null;

                _readTask = null;
                _processTask = null;

                SafeLog("PCI reader stopped.");
            }
        }

        private void ReadLoop(CancellationToken token)
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            ushort dev = 0;

            try
            {
                dev = (ushort)Global.Comunication.Protocol.PCI_Card1.m_dev;
            }
            catch (Exception ex)
            {
                SafeError("PCI", "Cannot get PCI device: " + ex.Message);
                return;
            }

            SpinWait spin = new SpinWait();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    uint value;
                    short ret = DASK.DI_ReadPort(dev, 0, out value);

                    if (ret >= 0)
                    {
                        HandleInputValue(value);
                    }
                    else
                    {
                        // lỗi đọc card thì chỉ nhả CPU chút, không log liên tục quá nhiều
                        spin.SpinOnce();
                    }
                }
                catch (Exception ex)
                {
                    SafeError("PCI", ex.Message);
                    Thread.Sleep(1);
                }

                // polling nhẹ, latency thấp hơn Sleep(1)
                spin.SpinOnce();

                // tránh spin quá mạnh quá lâu
                if (spin.Count > 50)
                    Thread.Yield();
            }
        }

        private void HandleInputValue(uint value)
        {
            // Logic gốc của bạn:
            // - thấy value == 1 thì đánh dấu sensor on
            // - khi đang on mà value về 0 thì phát trigger
            //
            // Viết lại gọn hơn, vẫn giữ đúng bản chất falling edge.

            if (value == 1)
            {
                _sensorOn = 1;
            }
            else if (_sensorOn == 1 && value == 0)
            {
                _sensorOn = 0;

                if (PassDebounce())
                {
                    int trigIndex = NextTriggerIndex();

                    TriggerEvent ev = new TriggerEvent
                    {
                        Timestamp = Stopwatch.GetTimestamp(),
                        TriggerIndex = trigIndex,
                        RawValue = value
                    };

                    _triggerQueue.Enqueue(ev);
                    _triggerSignal.Set();
                }
            }

            _lastValue = value;
        }

        private bool PassDebounce()
        {
            if (_debounceTicks <= 0)
                return true;

            long now = DateTime.UtcNow.Ticks;
            long last = Interlocked.Read(ref _lastTriggerTick);

            if (now - last < _debounceTicks)
                return false;

            Interlocked.Exchange(ref _lastTriggerTick, now);
            return true;
        }

        private void ProcessLoop(CancellationToken token)
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    _triggerSignal.WaitOne(10);

                    while (_triggerQueue.TryDequeue(out TriggerEvent ev))
                    {
                        ProcessTrigger(ev);
                    }
                }
                catch (Exception ex)
                {
                    SafeError("PCI_PROCESS", ex.Message);
                    Thread.Sleep(1);
                }
            }

            // flush queue trước khi thoát nếu muốn
            while (_triggerQueue.TryDequeue(out TriggerEvent ev2))
            {
                try
                {
                    ProcessTrigger(ev2);
                }
                catch { }
            }
        }

        private void ProcessTrigger(TriggerEvent ev)
        {
            try
            {
                Global.LogsDashboard.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", $"Trigger {ev.TriggerIndex}..."));

                Global.TriggerInternal = false;
                Global.IsAllowReadPLC = false;
                Global.StatusProcessing = StatusProcessing.Trigger;
                Global.Comunication.Protocol.IO_Processing = IO_Processing.Trigger;

                TriggerReceived?.Invoke(ev);
            }
            catch (Exception ex)
            {
                SafeError("PCI_TRIGGER", ex.Message);
            }
        }

        private int NextTriggerIndex()
        {
            switch (Global.TriggerNum)
            {
                case TriggerNum.Trigger0:
                    Global.TriggerNum = TriggerNum.Trigger1;
                    return 1;

                case TriggerNum.Trigger1:
                    Global.TriggerNum = TriggerNum.Trigger2;
                    return 2;

                case TriggerNum.Trigger2:
                    Global.TriggerNum = TriggerNum.Trigger3;
                    return 3;

                case TriggerNum.Trigger3:
                    Global.TriggerNum = TriggerNum.Trigger4;
                    return 4;

                case TriggerNum.Trigger4:
                default:
                    return 4;
            }
        }

        private void SafeLog(string msg)
        {
            try
            {
                LogMessage?.Invoke(msg);
            }
            catch { }
        }

        private void SafeError(string module, string msg)
        {
            try
            {
                Global.LogsDashboard.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, module, msg));
            }
            catch { }

            try
            {
                LogMessage?.Invoke($"{module}: {msg}");
            }
            catch { }
        }

        public void Dispose()
        {
            StopReadPCI();
            _triggerSignal?.Dispose();
        }
    }
}