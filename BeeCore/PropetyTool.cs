using BeeCore.Funtion;
using BeeGlobal;
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
    public  class PropetyTool : ICloneable
    {
        public PropetyTool()
        {

        }
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
                    StatusToolChanged?.Invoke(_StatusTool); // Gọi event
                }
            }
        }
        public Results Results = Results.None;

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();
        [NonSerialized]
        public BackgroundWorker worker = new BackgroundWorker();
        public void DoWork()
        {
            Results=Results.None;
          
              StatusTool = StatusTool.Processing;
            timer.Restart();
            if (UsedTool == UsedTool.NotUsed&&Global.IsRun)
                return;
           if (!Global.IsRun)
            Propety.rotAreaAdjustment = Propety.rotArea;
           if (Propety.rotAreaAdjustment==null)
                Propety.rotAreaAdjustment = Propety.rotArea;
            Propety.DoWork(Propety.rotAreaAdjustment);
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
