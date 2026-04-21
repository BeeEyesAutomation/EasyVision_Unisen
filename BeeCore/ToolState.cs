using BeeCore.Funtion;
using BeeGlobal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace BeeCore
{
    [Serializable]
    public class ToolState : IDisposable
    {
        [NonSerialized]
        internal PropetyTool Owner;

        public int IndexCamera = 0;
        public string Name = "";
        public TypeTool TypeTool;
        public bool[] IndexLogics = new bool[6];
        public UsedTool UsedTool = UsedTool.NotUsed;
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
        public int IndexImgRegis = 0;
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
                    ScoreChanged?.Invoke(_Score);
                }
            }
        }

        public int _Percent = 0;//note
        [field: NonSerialized]
        public event Action<int> PercentChange;

        public int Percent
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

        public string Location = "";
        public float CycleTime = 0;
        public float ScoreResult = 0;
        public float MinValue = 0;
        public float MaxValue = 0;
        public float StepValue = 0;
        private StatusTool _StatusTool = StatusTool.WaitCheck;
        [field: NonSerialized]
        public event Action<PropetyTool, StatusTool> StatusToolChanged;
        [field: NonSerialized]
        public event Action<PropetyTool, StatusTool> ToolDoneChanged;
        public StatusTool StatusTool
        {
            get => _StatusTool;
            set
            {
                if (_StatusTool != value)
                {
                    _StatusTool = value;
                    //if (Global.StatusMode == StatusMode.Continuous && TypeTool == TypeTool.Position_Adjustment && Results == Results.NG)
                    //    return;
                    StatusToolChanged?.Invoke(Owner, _StatusTool);
                }
            }
        }
        public Results Results = Results.None;

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();
        [NonSerialized]
        public BackgroundWorker worker = new BackgroundWorker();

        public void NotifyToolDone()
        {
            ToolDoneChanged?.Invoke(Owner, _StatusTool);
        }

        internal void RestoreScore(float value)
        {
            _Score = value;
        }

        internal void RestorePercent(int value)
        {
            _Percent = value;
        }

        internal void RestoreStatusTool(StatusTool value)
        {
            _StatusTool = value;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            EnsureRuntimeFields();
        }

        internal void EnsureRuntimeFields()
        {
            if (timer == null)
                timer = new Stopwatch();
            if (worker == null)
                worker = new BackgroundWorker();
        }

        public void Dispose()
        {
            try
            {
                // ====== stop timer/worker ======
                try { timer?.Stop(); } catch { }
                PropetyTool.TryDisposeObject(worker); // BackgroundWorker implements IDisposable
                worker = null;
                timer = null;

                // ====== UI control (phải UI thread) ======
                PropetyTool.SafeRemoveAndDisposeControl(Control);
                Control = null;

                // ====== Dispose các NonSerialized ItemTool ======
                PropetyTool.TryDisposeObject(ItemTool);
                PropetyTool.TryDisposeObject(ItemTool2);
                PropetyTool.TryDisposeObject(ItemTool3);
                PropetyTool.TryDisposeObject(ItemTool4);
                ItemTool = ItemTool2 = ItemTool3 = ItemTool4 = null;

                // ====== clear event để cắt reference ======
                ScoreChanged = null;
                PercentChange = null;
                StatusToolChanged = null;
                ToolDoneChanged = null;
            }
            catch { }
        }
    }
}
