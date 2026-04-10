using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
     
        public  void Start()
        {
         
            if(StatusProcessing==StatusProcessing.None)
            {
                doneCount = 0;
                Global._toolSemaphores = new Dictionary<string, SemaphoreSlim>();
                if (!Global.Comunication.Protocol.IsConnected && !Global.Comunication.Protocol.IsBypass)
                {
                    Global.PLCStatus = PLCStatus.ErrorConnect;
                    StatusProcessing = StatusProcessing.Done;
                }

                if (BeeCore.Common.PropetyTools[indexThread].Count == 0)
                    StatusProcessing = StatusProcessing.Done;
                foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        PropetyTool.StatusTool = StatusTool.WaitCheck;
                    }

                    if (Global.Config.IsAutoTrigger && Global.IsRun && !Global.IsDoneTrig)
                    {
                        Global.IndexToolAuto = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.AutoTrig);
                        if (Global.IndexToolAuto > -1)
                        {

                             BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].RunToolAsync();
                            BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].StatusToolChanged -= Checking_StatusToolChanged;
                            BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].StatusToolChanged += Checking_StatusToolChanged;
                            //if (!BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].worker.IsBusy)
                            //    BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].worker.RunWorkerAsync();
                            StatusProcessing = StatusProcessing.Waiting;

                        }
                    }
                    indexToolPosition = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.Position_Adjustment);
                    if (indexToolPosition == -1)
                    {
                        StatusProcessing = StatusProcessing.Checking;
                    return;
                    }
                    //if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].TypeTool == TypeTool.Position_Adjustment)
                    //{

                    //    if (!BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.IsBusy)
                    //        BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.RunWorkerAsync();

                    //    StatusProcessing = StatusProcessing.Adjusting;
                    //}
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].TypeTool == TypeTool.Position_Adjustment)
                    {
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].UsedTool != UsedTool.NotUsed)
                    {
                        StatusProcessing = StatusProcessing.Adjusting;


                        BeeCore.Common.PropetyTools[indexThread][indexToolPosition].StatusToolChanged -= Checking_StatusToolChanged;
                        BeeCore.Common.PropetyTools[indexThread][indexToolPosition].StatusToolChanged += Checking_StatusToolChanged;
                        BeeCore.Common.PropetyTools[indexThread][indexToolPosition].RunToolAsync();

                    }
                    else
                        StatusProcessing = StatusProcessing.Checking;

                        //if (!BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.IsBusy)
                        //    BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.RunWorkerAsync();


                    }
                    else
                    {
                        foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                        {

                            dynamic Propety = propetyTool.Propety2;
                            Propety.rotAreaAdjustment = Propety.rotArea;


                        }
                        StatusProcessing = StatusProcessing.Checking;
                    }
                }
        }

        private void Checking_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (obj == StatusTool.Done)
            {
                doneCount++;
                dynamic Propety = BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2;



                if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Results == Results.OK)
                {
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj == null)  StatusProcessing=StatusProcessing.Done;
                    float AngAdj = 0, X_Adj = 0, Y_Adj = 0;
                    X_Adj = Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rectRotates[0]._PosCenter.X - BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj._PosCenter.X;
                    Y_Adj = Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rectRotates[0]._PosCenter.Y - BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj._PosCenter.Y;
                    AngAdj = Propety.rotArea._rectRotation + Propety.rectRotates[0]._rectRotation - BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj._rectRotation;

                    foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        if (propetyTool.TypeTool == TypeTool.Position_Adjustment)
                            continue;
                        if (propetyTool.TypeTool == TypeTool.Measure)
                            continue;
                        if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj != null)
                        {//  propetyTool.Propety2.rotAreaAdjustment = BeeCore.Common.TransformToolRect(Global.rotAreaAdj,Global.rotOriginAdj,Global.rotCurrentAdj,propetyTool.Propety2.rotArea);
                            if (Global.IsAutoTemp)
                            {
                                if (propetyTool.TypeTool == TypeTool.Learning)
                                {
                                    int index = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.MultiPattern);
                                    if (index >= 0)
                                        propetyTool.Propety2.rotArea = BeeCore.Common.PropetyTools[indexThread][index].Propety2.rotArea;

                                }

                            }
                            propetyTool.Propety2.rotAreaAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety2.rotArea, BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj, X_Adj, Y_Adj, AngAdj);
                            if (propetyTool.Propety2.rotMask != null)
                                propetyTool.Propety2.rotMaskAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety2.rotMask, BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj, X_Adj, Y_Adj, AngAdj);

                            if (propetyTool.TypeTool == TypeTool.Learning)
                            {
                                if (propetyTool.Propety2.rotCrop != null)

                                    propetyTool.Propety2.rotCropAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety2.rotCrop, BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety2.rotOriginAdj, X_Adj, Y_Adj, AngAdj);

                            }

                        


                        }
                        else
                        {
                            propetyTool.Propety2.rotAreaAdjustment = propetyTool.Propety2.rotArea;
                            propetyTool.Propety2.rotCropAdjustment = propetyTool.Propety2.rotCrop;
                            propetyTool.Propety2.rotMaskAdjustment = propetyTool.Propety2.rotMask;
                        }



                    }
                    
                    StatusProcessing = StatusProcessing.Checking;
                }
                else
                    StatusProcessing = StatusProcessing.Checking;
            }
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
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            if (_StatusProcessing != StatusProcessing.Done &&
                                _StatusProcessing != StatusProcessing.None)
                            {
                                 ProcessingAll();
                            }
                        }
                        catch { }
                    });

                }
            }
        }
        public int indexThread = 0;

        public  void ProcessingAll()
        {
      
          
            switch (StatusProcessing)
            {
                case StatusProcessing.None:
                    //foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    //{
                    //    PropetyTool.StatusTool = StatusTool.WaitCheck;
                    //}
                      
                    //if (Global.Config.IsAutoTrigger && Global.IsRun&&!Global.IsDoneTrig)
                    //{
                    //    Global.IndexToolAuto = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.AutoTrig);
                    //    if (Global.IndexToolAuto > -1)
                    //    {
                            
                    //        await BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].RunToolAsync();

                    //        //if (!BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].worker.IsBusy)
                    //        //    BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].worker.RunWorkerAsync();
                    //        StatusProcessing = StatusProcessing.Waiting;
                    //        return StatusProcessing;
                    //    }
                    //}
                    //indexToolPosition = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.Position_Adjustment);
                    //if (indexToolPosition == -1)
                    //{
                    //    StatusProcessing = StatusProcessing.Checking;
                    //    return StatusProcessing;
                    //}
                  
                    //if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].TypeTool == TypeTool.Position_Adjustment)
                    //{
                    //    StatusProcessing = StatusProcessing.Adjusting;
                    //    await BeeCore.Common.PropetyTools[indexThread][indexToolPosition].RunToolAsync();

                    //    //if (!BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.IsBusy)
                    //    //    BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.RunWorkerAsync();

                       
                    //}
                    //else
                    //{
                    //    foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                    //    {

                    //        dynamic Propety = propetyTool.Propety2;
                    //        Propety.rotAreaAdjustment = Propety.rotArea;


                    //    }
                    //    StatusProcessing = StatusProcessing.Checking;
                    //}

                    break;
                case StatusProcessing.Waiting:
                    if (BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].StatusTool == StatusTool.Done)
                    {
                        if (!Global.IsDoneTrig)
                        {
                            if (BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].TypeTool == TypeTool.AutoTrig)
                            {


                                if (BeeCore.Common.PropetyTools[indexThread][Global.IndexToolAuto].Results == Results.NG)
                                {

                                    StatusProcessing = StatusProcessing.Done;
                                   
                                }
                                else
                                {
                                    Global.IsDoneTrig = true;
                                    StatusProcessing = StatusProcessing.Checking;
                                    
                                }
                            }
                        }
                        else
                        {
                            StatusProcessing = StatusProcessing.Checking;
                            
                        }
                    }

                    break;
                case StatusProcessing.Adjusting:
                    
                    break;
                case StatusProcessing.Checking:
                    doneCount = 0;
                    Global._toolSemaphores = new Dictionary<string, SemaphoreSlim>();
                    StatusProcessing = StatusProcessing.WaitingDone; totalTools = 0;
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        if (PropetyTool.UsedTool != UsedTool.NotUsed)
                            totalTools++;
                     
                    }    
                      
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        //PropetyTool.ItemTool.Status = "---";
                        //// Tools.ItemTool.Score.ColorTrack = Color.Gray;
                        //PropetyTool.ItemTool.ClScore = Color.Gray;
                        //PropetyTool.ItemTool.ClStatus = Color.Gray;
                        if (PropetyTool.TypeTool == TypeTool.AutoTrig)
                        {
                          //  PropetyTool.StatusTool = StatusTool.Done;
                            continue;
                        }

                        if (PropetyTool.TypeTool == TypeTool.Position_Adjustment)
                        {
                            doneCount++;
                            PropetyTool.StatusTool = StatusTool.Done;
                            continue;
                        }
                        PropetyTool.StatusTool = StatusTool.WaitCheck;
                        PropetyTool.Results = Results.None;

                        PropetyTool.ToolDoneChanged -= PropetyTool_StatusToolChanged;
                        PropetyTool.ToolDoneChanged += PropetyTool_StatusToolChanged;
                        PropetyTool.RunToolAsync();

                        //if (!PropetyTool.worker.IsBusy)
                        //    PropetyTool.worker.RunWorkerAsync();
                    }
             
                    break;
               
            }
        


        }
        private int totalTools = 0;
        private int doneCount = 0;
        private void PropetyTool_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (obj != StatusTool.Done) return;
            Interlocked.Increment(ref doneCount);

            if (doneCount == totalTools)
            {
                doneCount = 0;
                StatusProcessing = StatusProcessing.Done;
               
            }    
              
        }
    }
}
