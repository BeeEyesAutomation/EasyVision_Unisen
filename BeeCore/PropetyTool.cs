using BeeCore.Funtion;
using BeeGlobal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class PropetyTool : ICloneable, IDisposable, ISerializable
    {
        private bool _disposed;

        public dynamic Propety;
        public dynamic Propety2 { get; set; }
        public ToolState State = new ToolState();

        internal static void TryDisposeObject(object obj)
        {
            if (obj == null) return;
            try
            {
                if (obj is IDisposable d) d.Dispose();
            }
            catch { }
        }

        internal static void SafeRemoveAndDisposeControl(object ctlObj)
        {
            if (ctlObj == null) return;

            // tránh dynamic gây exception => cast về Control trước
            var ctl = ctlObj as System.Windows.Forms.Control;
            if (ctl == null)
            {
                // nếu không phải Control nhưng có IDisposable thì dispose thường
                TryDisposeObject(ctlObj);
                return;
            }

            try
            {
                if (ctl.IsDisposed) return;

                if (ctl.InvokeRequired)
                {
                    ctl.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            ctl.Parent?.Controls.Remove(ctl);
                            if (!ctl.IsDisposed) ctl.Dispose();
                        }
                        catch { }
                    }));
                }
                else
                {
                    ctl.Parent?.Controls.Remove(ctl);
                    if (!ctl.IsDisposed) ctl.Dispose();
                }
            }
            catch { }
        }

        public PropetyTool()
        {
            AttachStateOwner();
        }

        protected PropetyTool(SerializationInfo info, StreamingContext context)
        {
            State = new ToolState();
            AttachStateOwner();

            _disposed = GetValueOrDefault(info, "_disposed", false);
            Propety = GetObjectOrDefault(info, "Propety", null);
            Propety2 = GetObjectOrDefault(info, "<Propety2>k__BackingField", GetObjectOrDefault(info, "Propety2", null));

            State.IndexCamera = GetValueOrDefault(info, "IndexCamera", State.IndexCamera);
            State.Name = GetValueOrDefault(info, "Name", State.Name);
            State.TypeTool = GetValueOrDefault(info, "TypeTool", State.TypeTool);
            State.IndexLogics = GetValueOrDefault(info, "IndexLogics", State.IndexLogics);
            State.UsedTool = GetValueOrDefault(info, "UsedTool", State.UsedTool);
            State.IsSendResult = GetValueOrDefault(info, "IsSendResult", State.IsSendResult);
            State.IndexImgRegis = GetValueOrDefault(info, "IndexImgRegis", State.IndexImgRegis);
            State.RestoreScore(GetValueOrDefault(info, "_Score", 0f));
            State.RestorePercent(GetValueOrDefault(info, "_Percent", 0));
            State.Location = GetValueOrDefault(info, "Location", State.Location);
            State.CycleTime = GetValueOrDefault(info, "CycleTime", State.CycleTime);
            State.ScoreResult = GetValueOrDefault(info, "ScoreResult", State.ScoreResult);
            State.MinValue = GetValueOrDefault(info, "MinValue", State.MinValue);
            State.MaxValue = GetValueOrDefault(info, "MaxValue", State.MaxValue);
            State.StepValue = GetValueOrDefault(info, "StepValue", State.StepValue);
            State.RestoreStatusTool(GetValueOrDefault(info, "_StatusTool", BeeGlobal.StatusTool.WaitCheck));
            State.Results = GetValueOrDefault(info, "Results", State.Results);
            State.EnsureRuntimeFields();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_disposed", _disposed);
            info.AddValue("Propety", (object)Propety);
            info.AddValue("<Propety2>k__BackingField", (object)Propety2);
            info.AddValue("IndexCamera", IndexCamera);
            info.AddValue("Name", Name);
            info.AddValue("TypeTool", TypeTool);
            info.AddValue("IndexLogics", IndexLogics);
            info.AddValue("UsedTool", UsedTool);
            info.AddValue("IsSendResult", IsSendResult);
            info.AddValue("IndexImgRegis", IndexImgRegis);
            info.AddValue("_Score", Score);
            info.AddValue("_Percent", Percent);
            info.AddValue("Location", Location);
            info.AddValue("CycleTime", CycleTime);
            info.AddValue("ScoreResult", ScoreResult);
            info.AddValue("MinValue", MinValue);
            info.AddValue("MaxValue", MaxValue);
            info.AddValue("StepValue", StepValue);
            info.AddValue("_StatusTool", StatusTool);
            info.AddValue("Results", Results);
        }

        private static T GetValueOrDefault<T>(SerializationInfo info, string name, T defaultValue)
        {
            try
            {
                object value = info.GetValue(name, typeof(T));
                if (value == null)
                    return defaultValue;
                return (T)value;
            }
            catch (SerializationException)
            {
                return defaultValue;
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
        }

        private static object GetObjectOrDefault(SerializationInfo info, string name, object defaultValue)
        {
            try
            {
                return info.GetValue(name, typeof(object)) ?? defaultValue;
            }
            catch (SerializationException)
            {
                return defaultValue;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            AttachStateOwner();
        }

        private ToolState EnsureState()
        {
            if (State == null)
                State = new ToolState();
            AttachStateOwner();
            State.EnsureRuntimeFields();
            return State;
        }

        private void AttachStateOwner()
        {
            if (State != null)
                State.Owner = this;
        }

        public System.Int32 IndexCamera { get => EnsureState().IndexCamera; set => EnsureState().IndexCamera = value; }
        public System.String Name { get => EnsureState().Name; set => EnsureState().Name = value; }
        public TypeTool TypeTool { get => EnsureState().TypeTool; set => EnsureState().TypeTool = value; }
        public bool[] IndexLogics { get => EnsureState().IndexLogics; set => EnsureState().IndexLogics = value; }
        public UsedTool UsedTool { get => EnsureState().UsedTool; set => EnsureState().UsedTool = value; }
        public ItemTool ItemTool { get => EnsureState().ItemTool; set => EnsureState().ItemTool = value; }
        public ItemTool ItemTool2 { get => EnsureState().ItemTool2; set => EnsureState().ItemTool2 = value; }
        public ItemTool ItemTool3 { get => EnsureState().ItemTool3; set => EnsureState().ItemTool3 = value; }
        public ItemTool ItemTool4 { get => EnsureState().ItemTool4; set => EnsureState().ItemTool4 = value; }
        public dynamic Control { get => EnsureState().Control; set => EnsureState().Control = value; }
        public bool IsSendResult { get => EnsureState().IsSendResult; set => EnsureState().IsSendResult = value; }
        public int IndexImgRegis { get => EnsureState().IndexImgRegis; set => EnsureState().IndexImgRegis = value; }
        public float Score { get => EnsureState().Score; set => EnsureState().Score = value; }
        public event Action<float> ScoreChanged
        {
            add => EnsureState().ScoreChanged += value;
            remove => EnsureState().ScoreChanged -= value;
        }
        public int Percent { get => EnsureState().Percent; set => EnsureState().Percent = value; }
        public event Action<int> PercentChange
        {
            add => EnsureState().PercentChange += value;
            remove => EnsureState().PercentChange -= value;
        }
        public string Location { get => EnsureState().Location; set => EnsureState().Location = value; }
        public float CycleTime { get => EnsureState().CycleTime; set => EnsureState().CycleTime = value; }
        public float ScoreResult { get => EnsureState().ScoreResult; set => EnsureState().ScoreResult = value; }
        public float MinValue { get => EnsureState().MinValue; set => EnsureState().MinValue = value; }
        public float MaxValue { get => EnsureState().MaxValue; set => EnsureState().MaxValue = value; }
        public float StepValue { get => EnsureState().StepValue; set => EnsureState().StepValue = value; }
        public StatusTool StatusTool { get => EnsureState().StatusTool; set => EnsureState().StatusTool = value; }
        public event Action<PropetyTool, StatusTool> StatusToolChanged
        {
            add => EnsureState().StatusToolChanged += value;
            remove => EnsureState().StatusToolChanged -= value;
        }
        public event Action<PropetyTool, StatusTool> ToolDoneChanged
        {
            add => EnsureState().ToolDoneChanged += value;
            remove => EnsureState().ToolDoneChanged -= value;
        }
        public Results Results { get => EnsureState().Results; set => EnsureState().Results = value; }
        public System.Diagnostics.Stopwatch timer { get => EnsureState().timer; set => EnsureState().timer = value; }
        public BackgroundWorker worker { get => EnsureState().worker; set => EnsureState().worker = value; }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                State?.Dispose();

                // ====== Dispose Propety (dynamic) nếu có IDisposable ======
                // (Nếu Propety giữ Mat/Bitmap/Timer/... mà có Dispose thì sẽ được gọi)
                TryDisposeObject((object)Propety);
                Propety = null;
            }
            catch { }
        }

        //public async Task RunToolAsync()
        //{
        //    Exception exOut = null;
        //    bool finished = false;

        //    var t = Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.DoWork();
        //            finished = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            exOut = ex;
        //        }
        //    });

        //    // đợi complete hoặc timeout
        //    if (await Task.WhenAny(t, Task.Delay(Global.timeOutWork)) != t)
        //    {
        //        // TIMEOUT
        //        this.Results = Results.NG;
        //        this.StatusTool = StatusTool.Done;

        //        this.Complete();
        //        return;
        //    }

        //    // nếu có exception thì ném lại
        //    if (exOut != null)
        //        throw exOut;

        //    if (!finished)
        //    {
        //        this.Complete();
        //        return;
        //    }

        //    // COMPLETE OK
        //    this.Complete();
        //}
        //public void  Runtool() => RunToolAsync().GetAwaiter().GetResult();

        public void DoWork()
        {
            Results = Results.None;
            try
            {
                StatusTool = StatusTool.Processing;
                State.NotifyToolDone(); // Gọi event
                if (timer == null)
                    timer = new Stopwatch();
                timer.Restart();
                if (UsedTool == UsedTool.NotUsed && Global.IsRun)
                    return;
                if (!Global.IsRun)
                    Propety2.rotAreaAdjustment = Propety2.rotArea.Clone();
                if (Propety2.rotAreaAdjustment == null)
                    Propety2.rotAreaAdjustment = Propety2.rotArea.Clone();
                if (!Global.IsRun)
                    Propety2.rotMaskAdjustment = Propety2.rotMask;
                Propety2.DoWork(Propety2.rotAreaAdjustment, Propety2.rotMaskAdjustment);
            }
            catch (Exception ex)
            {
                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, Name, ex.Message));
            }

        }
        public async Task DoWorkAsync()
        {
            await Task.Run(() =>
            {
                DoWork();
            });

        }
        public void RunToolAsync()
        {
            var key = this.Name.ToString();
            SemaphoreSlim schedulerSem;
            if (Propety2.MaxThread == 0) Propety2.MaxThread = 1;
            lock (Global._toolSchedulerLock)
            {
                if (Global._toolSchedulerSemaphore == null)
                {
                    int maxConcurrency = Global.ResolveToolSchedulerConcurrency();
                    Global._toolSchedulerSemaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
                }

                if (!Global._toolSemaphores.ContainsKey(key))
                    Global._toolSemaphores[key] = new SemaphoreSlim(Propety2.MaxThread);

                schedulerSem = Global._toolSchedulerSemaphore;
            }

            var sem = Global._toolSemaphores[key];

            _ = RunInternalAsync(schedulerSem, sem);
        }

        private async Task RunInternalAsync(SemaphoreSlim schedulerSem, SemaphoreSlim sem)
        {
            bool enteredScheduler = false;
            bool entered = false;

            try
            {
                await schedulerSem.WaitAsync();
                enteredScheduler = true;

                await sem.WaitAsync();
                entered = true;

                var workTask = DoWorkAsync();

                var completed = await Task.WhenAny(
                    workTask,
                    Task.Delay(Global.Config.TimerOutChecking)
                );

                if (completed != workTask)
                {
                    Results = Results.NG;
                    return;
                }

                await workTask;
            }
            catch
            {
                Results = Results.NG;
            }
            finally
            {
                if (entered)
                    sem.Release();

                if (enteredScheduler)
                    schedulerSem.Release();

                Complete();
            }
        }
        public void Complete()
        {

            if (UsedTool == UsedTool.NotUsed && Global.IsRun)
            {
                Results = Results.None;
                StatusTool = StatusTool.Done;
                State.NotifyToolDone(); // Gọi event
                return;
            }
            else
                Propety2.Complete();
            timer.Stop();
            CycleTime = (int)timer.Elapsed.TotalMilliseconds;
            switch (UsedTool)
            {
                case UsedTool.NotUsed:
                    Results = Results.None;
                    break;
                case UsedTool.Invertse:
                    if (Results == Results.OK) Results = Results.NG;
                    else Results = Results.OK;
                    break;

            }
            if (!Global.IsRun)
            {
                Global.StatusDraw = StatusDraw.None;
                Global.StatusDraw = StatusDraw.Check;
            }

            StatusTool = StatusTool.Done;
            State.NotifyToolDone(); // Gọi event

        }


        //public void RunToolAsync()
        //{
        //    // tạo semaphore theo tool (key có thể là Name hoặc Type)
        //    var key = this.Name;
        //    if (Propety2.MaxThread == 0) Propety2.MaxThread = 1;
        //    lock (Global. _toolSemaphores)
        //    {
        //        if (!Global._toolSemaphores.ContainsKey(key))
        //            Global._toolSemaphores[key] = new SemaphoreSlim(Propety2.MaxThread);
        //    }

        //    var sem = Global._toolSemaphores[key];
        //    Task.Run(async () =>
        //    {
        //        await sem.WaitAsync();

        //        try
        //        {
        //            var workTask = Task.Run(() => DoWork());

        //            var completed = await Task.WhenAny(
        //                workTask,
        //                Task.Delay(Global.Config.TimerOutChecking)
        //            );

        //            if (completed != workTask)
        //            {
        //                Results = Results.NG;
        //                return;
        //            }

        //            await workTask;
        //        }
        //        catch
        //        {
        //            Results = Results.NG;
        //        }
        //        finally
        //        {
        //            Complete();
        //            sem.Release();
        //        }
        //    });
        //}
        public PropetyTool(dynamic Propety, TypeTool TypeTool, String Name)
        {
            AttachStateOwner();
            this.Name = Name;
            this.TypeTool = TypeTool;
            this.Propety2 = Propety;

        }


        public object Clone()
        {
            PropetyTool propety = new PropetyTool(this.Propety2.Clone(), this.TypeTool, this.Name);
            return propety;
        }
    }
}
