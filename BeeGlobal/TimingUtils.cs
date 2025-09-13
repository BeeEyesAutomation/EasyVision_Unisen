using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BeeGlobal
{
  
    public static class TimingUtils
    {
        // (tuỳ chọn) bật High-Resolution timer của Windows để cải thiện granularity
        public static void EnableHighResolutionTimer()
        {
            try { timeBeginPeriod(1); } catch { /* ignore if not supported */ }
        }
        public static void DisableHighResolutionTimer()
        {
            try { timeEndPeriod(1); } catch { /* ignore */ }
        }

        [DllImport("winmm.dll")]
        private static extern uint timeBeginPeriod(uint uMilliseconds);
        [DllImport("winmm.dll")]
        private static extern uint timeEndPeriod(uint uMilliseconds);

        /// <summary>
        /// Delay chính xác cao, không bận CPU: dùng Stopwatch + nhường CPU có kiểm soát.
        /// </summary>
        public static async Task DelayAccurateAsync(int milliseconds, CancellationToken ct = default)
        {
            if (milliseconds <= 0) return;

            var sw = Stopwatch.StartNew();

            // Pha “ngủ thô”: nếu còn nhiều thời gian, ngủ 1ms để tiết kiệm CPU
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                long remain = milliseconds - sw.ElapsedMilliseconds;
                if (remain <= 2) break;                 // chuyển sang pha tinh
                await Task.Delay(1, ct).ConfigureAwait(false);
            }

            // Pha “tinh”: bám sát thời gian đích, nhường context rất ngắn
            while (sw.ElapsedMilliseconds < milliseconds)
            {
                ct.ThrowIfCancellationRequested();
                // Task.Yield() giúp tránh block UI/scheduler, chính xác hơn Sleep(1) ở biên
                await Task.Yield();
            }
        }

        /// <summary>
        /// Bản đồng bộ: chính xác cao, CPU rất nhẹ nhờ SpinWait.
        /// </summary>
        public static void DelayAccurate(int milliseconds, CancellationToken ct = default)
        {
            if (milliseconds <= 0) return;

            var sw = Stopwatch.StartNew();

            // Pha thô: ngủ 1ms khi còn xa đích
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                long remain = milliseconds - sw.ElapsedMilliseconds;
                if (remain <= 2) break;
                Thread.Sleep(1);
            }

            // Pha tinh: quay rất nhẹ để cán đích
            var spinner = new SpinWait();
            while (sw.ElapsedMilliseconds < milliseconds)
            {
                ct.ThrowIfCancellationRequested();
                spinner.SpinOnce(); // tự điều tiết, không full CPU
            }
        }
    }

}
