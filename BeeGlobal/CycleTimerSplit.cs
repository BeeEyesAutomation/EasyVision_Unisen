using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BeeGlobal
{


    public class CycleTimerSplit
    {
        private static readonly long _freq;
        private static readonly double _tickToMs;
        private readonly List<(string Label, long Tick)> _marks = new List<(string Label, long Tick)>();
        private readonly long _t0;

        static CycleTimerSplit()
        {
            if (!QueryPerformanceFrequency(out _freq))
                throw new InvalidOperationException("QPF failed");
            _tickToMs = 1000.0 / _freq;
        }

        [DllImport("kernel32.dll")] private static extern bool QueryPerformanceCounter(out long v);
        [DllImport("kernel32.dll")] private static extern bool QueryPerformanceFrequency(out long v);

        private CycleTimerSplit()
        {
            QueryPerformanceCounter(out _t0);
        }

        public static CycleTimerSplit Start() => new CycleTimerSplit();

        /// <summary>Đánh dấu mốc với tên pha</summary>
        public void Split(string label)
        {
            QueryPerformanceCounter(out long t);
            _marks.Add((label, t));
        }

        public int TT;
        /// <summary>Kết thúc đo và trả chuỗi log, TT luôn = tổng các pha</summary>
        public string StopAndFormat()
        {
            QueryPerformanceCounter(out long tEnd);

            // Tạo danh sách điểm: start + các mốc + end
            var points = new List<(string Label, long Tick)>();
            points.Add(("START", _t0));
            points.AddRange(_marks);
            points.Add(("END", tEnd));

            // Tính các đoạn
            var seg = new List<(string Label, int Ms)>();
            for (int i = 1; i < points.Count; i++)
            {
                long delta = points[i].Tick - points[i - 1].Tick;
                int ms = (int)Math.Round(delta * _tickToMs);
                seg.Add((points[i].Label, ms));
            }

            int total = seg.Sum (s => s.Ms);
            TT = total;
            // Format
            var parts = new List<string> { $"TT:{total}" };
            parts.AddRange(seg.Where(s => s.Label != "END").Select(s => $"{s.Label}:{s.Ms}"));
            return string.Join("- ", parts) + " ms";
        }
    }

}
