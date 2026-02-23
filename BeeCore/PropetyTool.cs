using BeeCore.Funtion;
using BeeGlobal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace BeeCore
{
    [Serializable()]
    public  class PropetyTool :  ICloneable, IDisposable
    {
        private bool _disposed;
        private static void TryDisposeObject(object obj)
        {
            if (obj == null) return;
            try
            {
                if (obj is IDisposable d) d.Dispose();
            }
            catch { }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                // ====== stop timer/worker ======
                try { timer?.Stop(); } catch { }
                TryDisposeObject(worker); // BackgroundWorker implements IDisposable
                worker = null;
                timer = null;

                // ====== UI control (phải UI thread) ======
                SafeRemoveAndDisposeControl(Control);
                Control = null;

                // ====== Dispose các NonSerialized ItemTool ======
                TryDisposeObject(ItemTool);
                TryDisposeObject(ItemTool2);
                TryDisposeObject(ItemTool3);
                TryDisposeObject(ItemTool4);
                ItemTool = ItemTool2 = ItemTool3 = ItemTool4 = null;

                // ====== Dispose Propety (dynamic) nếu có IDisposable ======
                // (Nếu Propety giữ Mat/Bitmap/Timer/... mà có Dispose thì sẽ được gọi)
                TryDisposeObject((object)Propety);
                Propety = null;

                // ====== clear event để cắt reference ======
                ScoreChanged = null;
                PercentChange = null;
                StatusToolChanged = null;
            }
            catch { }
        }

        private static void SafeRemoveAndDisposeControl(object ctlObj)
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

        }
        public int IndexCamera = 0;
        public String Name = "";
        public dynamic Propety;
        public TypeTool TypeTool;
        public bool[] IndexLogics = new bool[6];
        public UsedTool UsedTool=UsedTool.NotUsed;
        [NonSerialized]
        public ItemTool ItemTool;
        [NonSerialized]
        public ItemTool ItemTool2;
        [NonSerialized]
        public ItemTool ItemTool3;
        [NonSerialized]
        public ItemTool ItemTool4;
        [NonSerialized]
        public dynamic Control;
        public bool IsSendResult = false;
        public int IndexImgRegis=0;
        private float _Score = 0;
        [field: NonSerialized]
        public event Action<float> ScoreChanged;
        public float Score
        {
            get => _Score;
            set
            {
                if (_Score != value)
                {
                    _Score = value;
                    ScoreChanged?.Invoke(_Score); // Gọi event
                }
            }
        }

        public int _Percent = 0;//note
        [field: NonSerialized]
        public event Action<int> PercentChange;

        public  int Percent
        {
            get => _Percent;
            set
            {
                if (_Percent != value)
                {
                    _Percent = value;
                    PercentChange?.Invoke(_Percent);
                }
            }
        }

        public String Location = "";
        public float CycleTime = 0;
        public float ScoreResult = 0;
        public float MinValue = 0;
        public float MaxValue = 0;
        public float StepValue = 0;
        private  StatusTool _StatusTool = StatusTool.WaitCheck;
        [field: NonSerialized]
        public  event Action<StatusTool> StatusToolChanged;
        public  StatusTool StatusTool
        {
            get => _StatusTool;
            set
            {
                if (_StatusTool != value)
                {
                    _StatusTool = value;
                    //if (Global.StatusMode == StatusMode.Continuous && TypeTool == TypeTool.Position_Adjustment && Results == Results.NG)
                    //    return;
                       
                    StatusToolChanged?.Invoke(_StatusTool); // Gọi event
                }
            }
        }
        public Results Results = Results.None;

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();
        [NonSerialized]
        public BackgroundWorker worker = new BackgroundWorker();
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
            Results=Results.None;
            try
            {
                StatusTool = StatusTool.Processing;
                timer.Restart();
                if (UsedTool == UsedTool.NotUsed && Global.IsRun)
                    return;
                if (!Global.IsRun)
                    Propety.rotAreaAdjustment = Propety.rotArea;
                if (Propety.rotAreaAdjustment == null)
                    Propety.rotAreaAdjustment = Propety.rotArea;
                if (!Global.IsRun)
                    Propety.rotMaskAdjustment = Propety.rotMask;
                Propety.DoWork(Propety.rotAreaAdjustment, Propety.rotMaskAdjustment);
            }
            catch (Exception ex)
            {
                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, Name, ex.Message));
            }
           
        }
        public void Complete()
        {
           
            if (UsedTool==UsedTool.NotUsed && Global.IsRun)
            {
                Results = Results.None;
                StatusTool = StatusTool.Done;
                return;
            }
            else
                Propety.Complete();
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
        
        }
        public PropetyTool(dynamic Propety, TypeTool TypeTool,String Name)
        {
            this.Name = Name;
            this.TypeTool = TypeTool;
            this.Propety = Propety;
            //worker = new BackgroundWorker();
            //worker.DoWork += (sender, e) =>
            //{
            //    StatusTool = StatusTool.Processing;
            //    timer.Restart();
            //    if (!Global.IsRun)
            //        Propety.rotAreaAdjustment = Propety.rotArea;
            //    Propety.DoWork(Propety.rotAreaAdjustment);
            //};
            //worker.RunWorkerCompleted += (sender, e) =>
            //{
            //    Propety.Complete();
            //    if (!Global.IsRun)
            //        Global.StatusDraw = StatusDraw.Check;
            //    StatusTool = StatusTool.Done;
            //    timer.Stop();
            //    CycleTime = (int)timer.Elapsed.TotalMilliseconds;
            //};

        }
      

        public object Clone()
        {
            PropetyTool propety = new PropetyTool(this.Propety.Clone(), this.TypeTool, this.Name);
            return propety;
        }
    }
}
