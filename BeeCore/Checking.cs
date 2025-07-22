using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeeCore
{
    public class Checking
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource
            ();
        public Checking(int  index)
        {
            StatusProcessing = StatusProcessing.None;
            this.indexThread = index;

        }
        public void Start()
        {
            // Khởi chạy task nền
            Task.Run( () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    ProcessingAll();                            // ========== Công việc chính
                    if (StatusProcessing == StatusProcessing.Done)
                        break;
                }
            }, _cts.Token);
        }

        public void Stop() => _cts.Cancel();
        int indexToolPosition = 0;
        private StatusProcessing _StatusProcessing;
       
        public event Action<StatusProcessing> StatusProcessingChanged;
        public StatusProcessing StatusProcessing
        {
            get => _StatusProcessing;
            set
            {
                if (_StatusProcessing != value)
                {
                    _StatusProcessing = value;
                    StatusProcessingChanged?.Invoke(_StatusProcessing); // Gọi event
                }
            }
        }
        public int indexThread = 0;
        public StatusProcessing ProcessingAll()
        {
      
            if (!Global.ParaCommon.Comunication.IO.IsConnected && !Global.ParaCommon.Comunication.IO.IsBypass)
                return StatusProcessing;
            if (BeeCore.Common.PropetyTools[indexThread].Count == 0)
                return StatusProcessing.Done;
            switch (StatusProcessing)
            {
                case StatusProcessing.None:
                    // timer.Restart();
                    //int    indexToolPosition = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.Position_Adjustment);
                    if (indexToolPosition == -1)
                    {
                        StatusProcessing = StatusProcessing.Processing;
                        return StatusProcessing;
                    }
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].TypeTool == TypeTool.Position_Adjustment)
                    {

                        if (!BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.IsBusy)
                            BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.RunWorkerAsync();

                        StatusProcessing = StatusProcessing.Adjusting;
                    }
                    else
                    {
                        foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                        {

                            dynamic Propety = propetyTool.Propety;
                            Propety.rotAreaAdjustment = Propety.rotArea;


                        }
                        StatusProcessing = StatusProcessing.Processing;
                    }

                    break;
                case StatusProcessing.Adjusting:
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].StatusTool == StatusTool.Done)
                    {
                        dynamic Propety = BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety;

                        StatusProcessing = StatusProcessing.Processing;

                        if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Results == Results.OK)
                        {
                            if (Global.rotOriginAdj == null) return StatusProcessing;
                            Global.X_Adjustment = Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rectRotates[0]._PosCenter.X - Global.rotOriginAdj._PosCenter.X;
                            Global.Y_Adjustment = Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rectRotates[0]._PosCenter.Y - Global.rotOriginAdj._PosCenter.Y;
                            Global.angle_Adjustment = Propety.rotArea._rectRotation + Propety.rectRotates[0]._rectRotation - Global.rotOriginAdj._rectRotation;
                        }
                        foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                        {
                            if (propetyTool.TypeTool == TypeTool.Position_Adjustment)
                                continue;
                            if (Global.rotOriginAdj != null)
                            {
                                propetyTool.Propety.rotAreaAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety.rotArea, Global.rotOriginAdj);
                                if (propetyTool.TypeTool == TypeTool.Positions)
                                {
                                    propetyTool.Propety.pOrigin = new System.Drawing.Point(Global.pOrigin.X, Global.pOrigin.Y);
                                    propetyTool.Propety.AngleOrigin = Global.AngleOrigin;
                                }


                            }
                            else
                                propetyTool.Propety.rotAreaAdjustment = propetyTool.Propety.rotArea;


                        }
                    }
                    break;
                case StatusProcessing.Processing:
                   // G.StatusDashboard.StatusText = "---";
                   // G.StatusDashboard.StatusBlockBackColor = Color.Gray;
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        PropetyTool.ItemTool.Status = "---";
                        // Tools.ItemTool.Score.ColorTrack = Color.Gray;
                        PropetyTool.ItemTool.ClScore = Color.Gray;
                        PropetyTool.ItemTool.ClStatus = Color.Gray;
                        PropetyTool.ItemTool.Refresh();
                        if (PropetyTool.TypeTool == TypeTool.Position_Adjustment) continue;
                        PropetyTool.StatusTool = StatusTool.WaitCheck;
                        if (!PropetyTool.worker.IsBusy)
                            PropetyTool.worker.RunWorkerAsync();
                    }
                    StatusProcessing = StatusProcessing.WaitingDone;
                    break;
                case StatusProcessing.WaitingDone:
                    StatusProcessing = StatusProcessing.Done;
                    Parallel.For(0, BeeCore.Common.PropetyTools[indexThread].Count, i =>
                    {
                        PropetyTool PropetyTool = BeeCore.Common.PropetyTools[indexThread][i];



                        if (PropetyTool.StatusTool != StatusTool.Done)
                        {
                            StatusProcessing = StatusProcessing.WaitingDone;

                        }
                        else
                        {
                            //if (Tools.tool.Propety.IsOK)
                            //{

                            //    if (Global.Config.ConditionOK == ConditionOK.Logic)
                            //    {
                            //        if (propetyTool.UsedTool == UsedTool.Used)
                            //        {
                            //            Tools.ItemTool.lbStatus.Text = "OK";
                            //            Tools.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                            //        }
                            //        else
                            //        {
                            //            Tools.ItemTool.Score.ColorTrack = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.Text = "NG";
                            //            Tools.ItemTool.lbScore.ForeColor = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.BackColor = Color.DarkRed;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        Tools.ItemTool.lbStatus.Text = "OK";
                            //        Tools.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                            //        Tools.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                            //        Tools.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                            //    }
                            //}

                            //else
                            //{

                            //    if (Global.Config.ConditionOK == ConditionOK.Logic)
                            //    {
                            //        if (propetyTool.UsedTool != UsedTool.Used)
                            //        {
                            //            Tools.ItemTool.lbStatus.Text = "OK";
                            //            Tools.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                            //        }
                            //        else
                            //        {
                            //            Tools.ItemTool.Score.ColorTrack = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.Text = "NG";
                            //            Tools.ItemTool.lbScore.ForeColor = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.BackColor = Color.DarkRed;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        Tools.ItemTool.Score.ColorTrack = Color.DarkRed;
                            //        Tools.ItemTool.lbStatus.Text = "NG";
                            //        Tools.ItemTool.lbScore.ForeColor = Color.DarkRed;
                            //        Tools.ItemTool.lbStatus.BackColor = Color.DarkRed;
                            //    }

                            //}
                            //Tools.ItemTool.Refresh();
                        }

                    }
                    );

                    break;
            }
            return StatusProcessing;


        }

    }
}
