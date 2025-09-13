using System;
using System.Runtime.InteropServices;

namespace BeeGlobal
{


    public sealed class QpcTimer
    {
        private static readonly double s_ticksToMs;
        private long _start;     // counter lúc Start
        private double _lastMs;  // CT lần đo gần nhất (ms)
        private bool _running;

        static QpcTimer()
        {
            if (!QueryPerformanceFrequency(out long freq) || freq <= 0)
                throw new InvalidOperationException("QueryPerformanceFrequency failed.");
            s_ticksToMs = 1000.0 / freq; // chuyển ticks -> milliseconds
        }

        /// <summary>Bắt đầu đo CT.</summary>
        public void Start()
        {
            if (!QueryPerformanceCounter(out _start))
                throw new InvalidOperationException("QueryPerformanceCounter failed (start).");
            _running = true;
        }

        /// <summary>Kết thúc đo CT, trả về giá trị ms.</summary>
        public double Stop()
        {
            if (!_running)
                return _lastMs;

            if (!QueryPerformanceCounter(out long end))
                throw new InvalidOperationException("QueryPerformanceCounter failed (stop).");

            long delta = end - _start;
            _lastMs = delta * s_ticksToMs;
            _running = false;
            return _lastMs;
        }

        /// <summary>Giá trị CT lần gần nhất (ms).</summary>
        public double LastCT => _lastMs;

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
    }

}
