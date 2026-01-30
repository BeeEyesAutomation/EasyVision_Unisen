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
                    ProcessingAll();                            // === Công việc chính
                    if (StatusProcessing == StatusProcessing.Done)
                    {
                     //   BeeCore.Common.listCamera[indexThread].DrawResult();
                        break;
                    }    
                       
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
      
            if (!Global.Comunication.Protocol.IsConnected && !Global.Comunication.Protocol.IsBypass)
            {
                Global.PLCStatus= PLCStatus.ErrorConnect;
                return StatusProcessing.Done;
            }
          
            if (BeeCore.Common.PropetyTools[indexThread].Count == 0)
                return StatusProcessing.Done;
            switch (StatusProcessing)
            {
                case StatusProcessing.None:
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        PropetyTool.StatusTool = StatusTool.WaitCheck;
                    }
                        indexToolPosition = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.Position_Adjustment);
                    if (indexToolPosition == -1)
                    {
                        StatusProcessing = StatusProcessing.Checking;
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
                        StatusProcessing = StatusProcessing.Checking;
                    }

                    break;
                case StatusProcessing.Adjusting:
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].StatusTool == StatusTool.Done)
                    {
                        dynamic Propety = BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety;



                        if (Global.Config.IsAutoTrigger)
                        {
                            if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Results == Results.NG)
                            {
                                StatusProcessing = StatusProcessing.Done;
                                return StatusProcessing;
                            }

                        }

                     //if (Global.StatusMode==StatusMode.Continuous)
                     //   {
                     //       if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Results == Results.NG)
                     //       {
                     //           StatusProcessing = StatusProcessing.Done;
                     //           return StatusProcessing;
                     //          }   
                              
                     //   }    

                        if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Results == Results.OK)
                        {
                            if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj == null) return StatusProcessing;
                             float AngAdj = 0, X_Adj = 0, Y_Adj = 0;
                            X_Adj = Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rectRotates[0]._PosCenter.X - BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj._PosCenter.X;
                            Y_Adj= Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rectRotates[0]._PosCenter.Y - BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj._PosCenter.Y;
                            AngAdj = Propety.rotArea._rectRotation + Propety.rectRotates[0]._rectRotation - BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj._rectRotation;

                            foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                            {
                                if (propetyTool.TypeTool == TypeTool.Position_Adjustment)
                                    continue;
                                if (propetyTool.TypeTool == TypeTool.Measure)
                                    continue;
                                if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj != null)
                                {//  propetyTool.Propety.rotAreaAdjustment = BeeCore.Common.TransformToolRect(Global.rotAreaAdj,Global.rotOriginAdj,Global.rotCurrentAdj,propetyTool.Propety.rotArea);

                                    propetyTool.Propety.rotAreaAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety.rotArea, BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj,X_Adj,Y_Adj,AngAdj);
                                    if(propetyTool.Propety.rotMask!=null)
                                    propetyTool.Propety.rotMaskAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety.rotMask, BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj, X_Adj, Y_Adj, AngAdj);
                                    if(propetyTool.TypeTool==TypeTool.Learning)
                                    {
                                        if (propetyTool.Propety.rotCrop != null)
                                      
                                            propetyTool.Propety.rotCropAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety.rotCrop, BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.rotOriginAdj, X_Adj, Y_Adj, AngAdj);

                                    }

                                    //if (propetyTool.TypeTool == TypeTool.MultiPattern)
                                    //{
                                    //    List<ResultMulti> ResultMulti = propetyTool.Propety.ResultMulti;
                                    //    foreach (ResultMulti resultMulti in ResultMulti)

                                    //    {
                                    //        resultMulti.rotAdj=BeeCore.Common.GetPositionAdjustment(resultMulti.RotCalib, Global.rotOriginAdj);
                                    //    }    

                                    //}


                                }
                                else
                                {
                                    propetyTool.Propety.rotAreaAdjustment = propetyTool.Propety.rotArea;
                                    propetyTool.Propety.rotCropAdjustment = propetyTool.Propety.rotCrop;
                                    propetyTool.Propety.rotMaskAdjustment = propetyTool.Propety.rotMask;
                                }    
                                   


                            }
                            StatusProcessing = StatusProcessing.Checking;
                        }
                        else
                         StatusProcessing = StatusProcessing.Checking;
                    }
                    break;
                case StatusProcessing.Checking:
                   // G.StatusDashboard.StatusText = "---";
                   // G.StatusDashboard.StatusBlockBackColor = Color.Gray;
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        //PropetyTool.ItemTool.Status = "---";
                        //// Tools.ItemTool.Score.ColorTrack = Color.Gray;
                        //PropetyTool.ItemTool.ClScore = Color.Gray;
                        //PropetyTool.ItemTool.ClStatus = Color.Gray;
                      
                        if (PropetyTool.TypeTool == TypeTool.Position_Adjustment) continue;
                        PropetyTool.StatusTool = StatusTool.WaitCheck;
                        if (!PropetyTool.worker.IsBusy)
                            PropetyTool.worker.RunWorkerAsync();
                    }
                    StatusProcessing = StatusProcessing.WaitingDone;
                    break;
                case StatusProcessing.WaitingDone:
                    StatusProcessing Status = StatusProcessing.Done;
                    Parallel.For(0, BeeCore.Common.PropetyTools[indexThread].Count, i =>
                    {
                        PropetyTool PropetyTool = BeeCore.Common.PropetyTools[indexThread][i];



                        if (PropetyTool.StatusTool != StatusTool.Done)
                        {
                            Status = StatusProcessing.WaitingDone;
                            return;
                        }
                        else
                        {
                           
                        }

                    }
                    );
                    StatusProcessing= Status;
                   
                    break;
            }
            return StatusProcessing;


        }

    }
}
