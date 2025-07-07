using BeeCore;
using BeeGlobal;
using BeeInterface;
using BeeUi.Commons;
using BeeUi.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BeeUi.Data
{
    public class ClassProject
    {
        public static void Load(String NameProject)
        {
            NameProject = NameProject.Replace(".prog", "");
            Global.ParaCommon = LoadData.Para(NameProject);
            List<ParaCamera> paraCameras = LoadData.ParaCamera(NameProject);
            if (paraCameras.Count() > 0)
            {
               Global.listParaCamera = paraCameras;
                BeeCore.Common.listCamera = new List<Camera>();
                int indexCCD = 0;
                foreach (ParaCamera paraCamera in paraCameras)
                {if (paraCamera != null)
                        BeeCore.Common.listCamera.Add(new Camera(paraCamera, indexCCD));
                    else
                        BeeCore.Common.listCamera.Add(null);
                    indexCCD++;
                }
                if (BeeCore.Common.listCamera.Count() > Global.IndexChoose)
                    if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                        Global.ParaCommon.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
            }
            //Global.ParaCommon.SizeCCD = Camera.GetSzCCD();
            BeeCore.Common.PropetyTools = LoadData.Project(NameProject);


            if(BeeCore.Common.PropetyTools.Count==0)
            {
                BeeCore.Common.PropetyTools = new List<List<PropetyTool>> { new List<PropetyTool>(), new List<PropetyTool>(),  new List<PropetyTool>(), new List<PropetyTool>() };
                G.listAlltool = new List<List<Tools>> { new List<Tools>(), new List<Tools>(), new List<Tools>(), new List<Tools>() };

            }
            else
                G.listAlltool = new List<List<Tools>>();


            if (G.ToolSettings == null)
            {
                G.ToolSettings = new ToolSettings();

            }
            G.ToolSettings.Y = 10; G.ToolSettings.X = 5;
            int indexThread = 0;

            foreach (List< BeeCore.PropetyTool>ListTool in BeeCore.Common.PropetyTools)
            {
                if (ListTool != null)
                {
                    G.listAlltool.Add(new List<Tools>());
                    int indexTool = 0;
                    foreach (BeeCore.PropetyTool tool in ListTool)
                    {
                        BeeInterface.Tools tool2 = DataTool.CreateControl(tool, indexTool, indexThread, new Point(G.ToolSettings.X, G.ToolSettings.Y));
                       
                        G.ToolSettings.Y += tool2.ItemTool.Height + 10;
                        indexTool++;
                        if (tool2 != null)
                        {
                            tool2.tool.Propety.IndexThread = indexThread;
                            G.listAlltool[indexThread].Add(tool2);
                        }
                     
                    }
                      
                }
                else
                {
                    G.listAlltool.Add(null);
                }


                indexThread++;

            }
            foreach (List<Tools> ListTool in G.listAlltool)
            {
                if (ListTool == null) continue;
                foreach (Tools Tool in ListTool)
                DataTool.LoadPropety(Tool.tool);
            
         
            }
            //foreach (Tools tool in G.listAlltool)
            //{
            //    DataTool.LoadPropety(tool.tool);
            //   //X: if(tool.tool.Propety.StatusTool != StatusTool.Initialed)
            //   // {
            //   //     Task.Delay(10);
            //   //     goto X;
            //   // }
            //   //else
            //   // {
            //   //     continue;
            //   // }
            //}
            foreach (List<PropetyTool> ListTool in BeeCore.Common.PropetyTools)
            {

                Parallel.ForEach(ListTool, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, propety =>
                {
                    if (propety != null)
                        if (propety.Propety != null)
                            propety.Propety.SetModel();
                });
            }
          
            G.IsLoad = true;
          
        }
        int indexToolPosition = 0;
        public StatusProcessing ProcessingAll(StatusProcessing StatusProcessing, int indexThread = 0)
        {
            if (!G.Initial)
                return StatusProcessing;
            if (!Global.Comunication.IO.IsConnected && !Global.Comunication.IO.IsBypass)
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

                        if (!G.listAlltool[indexThread][indexToolPosition].tool.worker.IsBusy)
                            G.listAlltool[indexThread][indexToolPosition].tool.worker.RunWorkerAsync();

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
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety.StatusTool == StatusTool.Done)
                    {
                        dynamic Propety = BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety;

                        StatusProcessing = StatusProcessing.Processing;

                        if (Propety.IsOK)
                        {
                            if (G.rotOriginAdj == null) return StatusProcessing;
                            G.X_Adjustment = Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rectRotates[0]._PosCenter.X - G.rotOriginAdj._PosCenter.X;
                            G.Y_Adjustment = Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rectRotates[0]._PosCenter.Y - G.rotOriginAdj._PosCenter.Y;
                            G.angle_Adjustment = Propety.rotArea._rectRotation + Propety.rectRotates[0]._rectRotation - G.rotOriginAdj._rectRotation;
                        }
                        foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                        {
                            if (propetyTool.TypeTool == TypeTool.Position_Adjustment)
                                continue;
                            if (G.rotOriginAdj != null)
                            {
                                propetyTool.Propety.rotAreaAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety.rotArea, G.rotOriginAdj);
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
                    G.ResultBar.lbStatus.Text = "---";
                    G.ResultBar.lbStatus.BackColor = Color.Gray;
                    foreach (Tools Tools in G.listAlltool[indexThread])
                    {
                        Tools.ItemTool.lbStatus.Text = "---";
                        Tools.ItemTool.Score.ColorTrack = Color.Gray;
                        Tools.ItemTool.lbScore.ForeColor = Color.Gray;
                        Tools.ItemTool.lbStatus.BackColor = Color.Gray;
                        Tools.ItemTool.Refresh();
                        PropetyTool propetyTool = BeeCore.Common.PropetyTools[indexThread][BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.Name == Tools.tool.Name)];
                        if (propetyTool.TypeTool == TypeTool.Position_Adjustment) continue;
                        propetyTool.Propety.StatusTool = StatusTool.None;
                        if (!Tools.tool.worker.IsBusy)
                            Tools.tool.worker.RunWorkerAsync();
                    }
                    StatusProcessing = StatusProcessing.WaitingDone;
                    break;
                case StatusProcessing.WaitingDone:
                    StatusProcessing = StatusProcessing.Done;
                    Parallel.For(0, G.listAlltool[indexThread].Count, i =>
                    {
                        Tools Tools = G.listAlltool[indexThread][i];


                        PropetyTool propetyTool = BeeCore.Common.PropetyTools[indexThread][BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.Name == Tools.tool.Name)];

                        if (propetyTool.Propety.StatusTool != StatusTool.Done)
                        {
                            StatusProcessing = StatusProcessing.WaitingDone;

                        }
                        else
                        {
                            //if (Tools.tool.Propety.IsOK)
                            //{

                            //    if (G.Config.ConditionOK == ConditionOK.Logic)
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

                            //    if (G.Config.ConditionOK == ConditionOK.Logic)
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
