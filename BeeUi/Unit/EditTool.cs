 using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using BeeUi.Commons;
using BeeUi.Tool;
using BeeUi.Unit;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using static BeeInterface.DashboardImages;
using Point = System.Drawing.Point;
using View = BeeInterface.View;

namespace BeeUi
{
    [Serializable()]
    public partial class EditTool : UserControl
    {
        private LayoutPersistence _layout;
        private ControlStylePersistence _styles;
        public EditTool()
        {
          


           

            InitializeComponent();
            //this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
           
            //this.AutoScaleMode = AutoScaleMode.Dpi; // ho?c AutoScaleMode.Font
            _layout = new LayoutPersistence(this, key: "MainLayout");
            _layout.LoadDelayMs = 300;        // tr? 500ms sau Form.Shown
            _layout.SplitterLocked = true;   // tu? ch?n
            _layout.EnableAuto();
           
        }
        public void RegisTer(String name,Control toolEdit)
        {
            if(!pEditTool.Show(name))
             {
                pEditTool.Register(name, () => toolEdit);

            }
            pEditTool.Show(name);
        }
      public void UnResgisTer()
        {
            int IndexProgChoose = 0;
            if (BeeCore.Common.PropetyTools == null) 
                return;
            foreach (List<PropetyTool> ListTool in BeeCore.Common.PropetyTools)
            {
                if (ListTool == null)
                {     
                    IndexProgChoose++;

                continue;
                }
                int i= 0;
                foreach (PropetyTool propety in ListTool)
                {
                    String name = "Tools" + IndexProgChoose+ Global.IndexProgChoose  + BeeCore.Common.TryGetTool(IndexProgChoose, i).Name;

                    pEditTool.Unregister(name);
                    i++;
                }
                IndexProgChoose++;
            }
        }
        public void Acccess(bool IsRun)
		{
            BtnHeaderBar.btnUser.Text = Global.Config.Users.ToString();
			View.btnLive.Enabled = !Global.IsRun;
        
            Global.ToolSettings.btnAdd.Enabled = !Global.IsRun;
            Global.ToolSettings.btnCopy.Enabled = !Global.IsRun;
            Global.ToolSettings.btnDelect.Enabled = !Global.IsRun;
            Global.ToolSettings.btnEnEdit.Enabled = Global.IsRun;
            Global.ToolSettings.btnRename.Enabled = !Global.IsRun;
             BeeInterface.G.Header.btnShowList.Enabled = true;
             BeeInterface.G.Header.txtQrCode.Enabled = true;
             BeeInterface.G.Header.pEdit.Enabled = true;
            switch (Global.Config.Users)
            {
                case Users.Admin:
					BeeInterface.  G.StatusDashboard.btnReset.Enabled = true;
					Global.EditTool.View.btnContinuous.Enabled = Global.IsRun;
					 BeeInterface.G.Header.btnMode.Enabled = true;
                    View.pBtn.Enabled= true;
					 BeeInterface.G.Header.pEdit.Enabled = true;
                     BeeInterface.G.Header.pModel.Enabled = true;
                     BeeInterface.G.Header.pPO.Enabled = true;
                    this.BtnHeaderBar.btnSetting.Enabled = true;
                    this.BtnHeaderBar.btncheck.Enabled = true;
                    this.BtnHeaderBar.btnReport.Enabled = true;
                     BeeInterface.G.Header.btnDummy.Enabled = true;
                     BeeInterface.G.Header.btnTraining.Enabled = true;
                    Global.ToolSettings.btnEnEdit.Enabled = true;
                    View.btnTypeTrig.Enabled = true;
					BarRight.btnFlowChart.Enabled = true;
					BarRight.btnHistory.Enabled = true;
					BarRight.btnHardware.Enabled = true;
					BarRight.btnLog.Enabled = true;
                    btnLogo.Enabled = true;
					break;
				case Users.Leader:

					Global.EditTool.View.btnContinuous.Enabled = false;
                     BeeInterface.G.Header.btnTraining.Enabled = false;
                     BeeInterface.G.Header.btnDummy.Enabled = true;
                   BeeInterface.  G.StatusDashboard.btnReset.Enabled = true;
                    this.BtnHeaderBar.btnSetting.Enabled = false;
                    this.BtnHeaderBar.btncheck.Enabled = false;
                    this.BtnHeaderBar.btnReport.Enabled = true;
                     BeeInterface.G.Header.btnMode.Enabled = false;
					View.pBtn.Enabled = true;
					View.btnTypeTrig.Enabled = false;
                     BeeInterface.G.Header.pEdit.Enabled = false;
                     BeeInterface.G.Header.pModel.Enabled = false;
                     BeeInterface.G.Header.pPO.Enabled = false;
                    Global.ToolSettings.btnEnEdit.Enabled = false;
                    BarRight.btnFlowChart.Enabled = true;
					BarRight.btnHistory.Enabled = true;
					BarRight.btnHardware.Enabled = false;
					BarRight.btnLog.Enabled = false;
                    btnLogo.Enabled = false;
					break;
				case Users.User:
					Global.EditTool.View.btnContinuous.Enabled = false;
                     BeeInterface.G.Header.btnDummy.Enabled = true;
                     BeeInterface.G.Header.btnMode.Enabled = false;
                     BeeInterface.G.Header.btnTraining.Enabled = false;
                    BeeInterface.G.StatusDashboard.btnReset.Enabled = false;
                    this.BtnHeaderBar.btnSetting.Enabled = false;
                    this.BtnHeaderBar.btncheck.Enabled = false;
                    this.BtnHeaderBar.btnReport.Enabled = false;
                    View.pBtn.Enabled = false;
					View.btnTypeTrig.Enabled = false;
                     BeeInterface.G.Header.pEdit.Enabled = false;
                     BeeInterface.G.Header.pModel.Enabled = false;
                     BeeInterface.G.Header.pPO.Enabled = false;
                    Global.ToolSettings.btnEnEdit.Enabled = false;
                    BarRight.btnFlowChart.Enabled = true;
					BarRight.btnHistory.Enabled = false;
					BarRight.btnHardware.Enabled = false;
					BarRight.btnLog.Enabled = false;
                    btnLogo.Enabled = false;
					break;
			
			}
			
		
			if (Global.Config.IsExternal)
			{
				Global.EditTool.View.btnCap.Enabled = false;
				Global.EditTool.View.btnContinuous.Enabled = false;
            }
            else
            {
                Global.EditTool.View.btnCap.Enabled = Global.IsRun;
                Global.EditTool.View.btnContinuous.Enabled = Global.IsRun;
            }    
          

                Global.EditTool.BtnHeaderBar.btnUser.Text = Global.Config.Users.ToString();
		}
		void EnableDoubleBuffer(Control c)
        {
            var t = c.GetType();
            var piDB = t.GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            piDB?.SetValue(c, true, null);

            // giúp redraw mu?t khi resize
            var piRR = t.GetProperty("ResizeRedraw",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            piRR?.SetValue(c, true, null);
        }
        public void RefreshGuiEdit( Step Step)
        {
            try
            {
                Global.Step= Step;
             
                if (BeeCore.Common.listCamera[Global.IndexCCCD] == null)
                {
                    BeeCore.Common.listCamera[Global.IndexCCCD] = new Camera(new ParaCamera(), Global.IndexProgChoose);
                    Global.ScanCCD.ShowDialog();
                    return;
                }
              
                Global.IndexToolSelected = -1;
                if(Global.EditTool.View.btnLive.IsCLick)
                {
                    Global.EditTool.View. btnLive.PerformClick();

                }
                layInforTool.Visible = false;
                BarRight.btnFlowChart.IsCLick = true;
                Global.StatusDraw = StatusDraw.None;
            X: switch (Step)
                {
                   
                    case Step.Run:
                     
                        foreach (List< PropetyTool> ListPropetyTool in BeeCore.Common.PropetyTools)
                        {if (ListPropetyTool == null) continue;
                            foreach (PropetyTool PropetyTool in ListPropetyTool)
                            {
                                if (PropetyTool.ItemTool != null)
                                    PropetyTool.ItemTool.IsCLick = false;
                            }
                        }
                        Global.IndexProgChoose = 0;
                        Global.Comunication.Protocol.IO_Processing = IO_Processing.ChangeMode;
                        Global.EditTool.View.btnChangeImg.Visible = true;
                        Global.EditTool.View.imgView.AutoCenter = true;
                        View.pMenu.Visible = false;
                        View.pImageShow.Visible = true;
                        Global.IsAllowReadPLC = true;
                        ShowTool.ShowAllChart(Global.ToolSettings.pAllTool);
                        Global.EditTool.View.btnLive.Enabled = false;
                        BarRight.Visible = true;
                        pHeader.Visible = true;
                        Global.IsRun = true; Acccess(Global.IsRun);
                     
                        pName.Visible = false;
                       
                        pInfor.Show("Dashboard");
                        pEditTool.Show("Tool");
                       
                        Mat matRegStep1=new Mat();
                        try
                        {



                            if (Global.Config.IsMultiProg)
                            {


                                if (Global.Config.NumTrig >1 && Global.NumProgFromPLC == Global.Config.NumTrig)
                                {
                                  //  MatMergerOptions opt = new MatMergerOptions();
                                  //  opt.Direction = MergeDirection.Vertical;
                                    var opt = new MatMergerOptions
                                    {
                                        Direction = MergeDirection.Vertical
                                    };
                                    Mat matRS=new Mat();
                        switch(Global.NumProgFromPLC)
                                    {
                                        case 2:
                                            matRS = MatMerger.MergeMany(new[]
                                              {
                                                Global.ParaCommon.matRegister.ToMat(),
                                                Global.ParaCommon.matRegister2.ToMat(),
                                               
                                            }, opt);
                                            break;
                                        case 3:
                                            matRS = MatMerger.MergeMany(new[]
                                           {
                                                Global.ParaCommon.matRegister.ToMat(),
                                                Global.ParaCommon.matRegister2.ToMat(),
                                                Global.ParaCommon.matRegister3.ToMat(),

                                            }, opt);
                                            break;
                                        case 4:
                                            matRS = MatMerger.MergeMany(new[]
                                                                                   {
                                                Global.ParaCommon.matRegister.ToMat(),
                                                Global.ParaCommon.matRegister2.ToMat(),
                                                Global.ParaCommon.matRegister3.ToMat(),
                                                 Global.ParaCommon.matRegister4.ToMat(),

                                            }, opt);
                                            break;
                                      
                                    }

                                    matRegStep1 = matRS;// MatMerger.Merge(Global.ParaCommon.matRegister.ToMat(), Global.ParaCommon.matRegister2.ToMat(), opt);
                                }
                                else
                                {
                                    matRegStep1 = Global.ParaCommon.matRegister.ToMat();

                                }
                            }
                            else
                                matRegStep1 = Global.ParaCommon.matRegister.ToMat();

                            if (matRegStep1 != null)
                                    if (BeeCore.Common.listCamera[Global.IndexCCCD] != null)
                                        if (!matRegStep1.IsDisposed)
                                        {
                                            BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = new Mat();
                                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = matRegStep1.Clone();
                                            G.IsCalib = false;
                                            Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                                            Global.EditTool.View.imgView.Invalidate();
                                            Global.EditTool.View.imgView.Update();
                                            ShowTool.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
                                            Global.Config.imgZoom = View.imgView.Zoom;
                                            Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                            Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                        }
                            foreach(PropetyTool propetyTool in BeeCore.Common.EnsureToolList(Global.IndexProgChoose))
                            {
                                if (propetyTool.ItemTool == null)
                                    continue;
                                propetyTool.ItemTool.IsCLick = false;
                              
                            }    
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            if (matRegStep1 != null)
                                if (!matRegStep1.IsDisposed)
                                    matRegStep1.Dispose();
                        }
                     
                      View.RefreshExternal(Global.Config.IsExternal);
                        BeeInterface.G.Header.btnMode.IsCLick = false;
                        BeeInterface.G.Header.btnMode.Text = "RUN";

                        Global.Comunication.Protocol.IO_Processing = IO_Processing.ChangeMode;
                        break;
                    case Step.Step1:
                        View.pImageShow.Visible = false;
                        foreach (PropetyTool PropetyTool in BeeCore.Common.EnsureToolList(Global.IndexProgChoose))
                        {
                            if (PropetyTool.ItemTool != null)
                                PropetyTool.ItemTool.IsEdit = false;
                        }
                        Global.Comunication.Protocol.IO_Processing = IO_Processing.ChangeMode;
                        View. imgView.Text = "";
                        View.pImg.Visible = false;
                       View.btnChangeImg.IsCLick = false;
                        View.btnChangeImg.Visible = false;
                        layInforTool.Visible = false;
                        Global.EditTool.View.imgView.AutoCenter = false;
                        View.pMenu.Visible = true;
                        Global.IsAllowReadPLC = false;
                       
                        pHeader.Visible = false;
                        BarRight.Visible = false;
                        Global.IsRun = false; Acccess(Global.IsRun);
                        Global.EditTool.View.btnLive.Enabled = true;
                     
                   
                        Global.StepEdit.btnStep1.IsCLick = true;

                        
                       
                       
                      

                        //Global.StepEdit.SettingStep1.Size = Global.EditTool.pEditTool.Size;
                        //Global.StepEdit.Visible = true;
                        //G.StatusDashboard.Visible = false;
                        pEditTool.Show("Step1");
                        pInfor.Show("StepEdit");
                       iconTool.Visible = false;
                      
                        lbTool.Text = "1.Setup Camera";

                        BeeInterface.G.Header.btnMode.IsCLick = true;
                        BeeInterface.G.Header.btnMode.Text = "EDIT";

                        Global.Comunication.Protocol.IO_Processing = IO_Processing.ChangeMode;
                        break;
                    case Step.Step2:
                        iconTool.Visible = false;
                        lbTool.Text = "STEP 2";
                        layInforTool.Visible = false;
                        Global.EditTool.View.imgView.AutoCenter = false;
                        Global.IsAllowReadPLC = false;
                        Global.EditTool.View.btnLive.Enabled = false;
                        Global.StepEdit.btnStep2.IsCLick = true;
                        //   pName.Visible = true;
                        G.IsCalib = false;
                        Global.StepEdit.SettingStep2.LoadImg();
                        pEditTool.Show("Step2");

                        //iconTool.BackgroundImage = Properties.Resources._2;
                        lbTool.Text = "2.Register Image";
                        Mat matReg = new Mat();
                        try
                        {
                          
                            
                                switch (Global.IndexProgChoose)
                                {
                                    case 0:
                                        matReg = Global.ParaCommon.matRegister.ToMat();
                                        break;
                                    case 1:
                                        matReg = Global.ParaCommon.matRegister2.ToMat();
                                    break;
                                    case 2:
                                        matReg = Global.ParaCommon.matRegister3.ToMat();
                                    break;
                                    case 3:
                                        matReg = Global.ParaCommon.matRegister4.ToMat();
                                    break;
                                }
                            
                            if (matReg != null)
                            {
                                if (!matReg.IsDisposed)
                                {
                                    if (matReg.Width != 0)
                                    {
                                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = matReg.Clone();
                                        G.IsCalib = false;
                                        Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                                        Global.EditTool.View.imgView.Invalidate();
                                        Global.EditTool.View.imgView.Update();
                                        ShowTool.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
                                        Global.Config.imgZoom = View.imgView.Zoom;
                                        Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                        Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                    }
                                }
                            }
                            else
                            {

                            }    
                          
                           
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (matReg != null)
                                if (!matReg.IsDisposed)
                                    matReg.Dispose();
                        }
                        break;
                    case Step.Step3:
                      
                        iconTool.Visible = false;
                        lbTool.Text = "3.Tools Setting";
                        layInforTool.Visible = false;
                        Global.EditTool.View.imgView.AutoCenter = false;
                        Global.IsAllowReadPLC = false;
                        Global.EditTool.View.btnLive.Enabled = false;
                        Global.StepEdit.btnStep3.IsCLick = true;
                        pName.Visible = true;
                       
                        pEditTool.Show("Tool");
                        ShowTool.ShowChart( Global.ToolSettings.pAllTool, BeeCore.Common.EnsureToolList(Global.IndexProgChoose));
                        Mat matReg2 = new Mat();
                        try
                        {
                            switch (Global.IndexProgChoose)
                            {
                                case 0:
                                    matReg2 = Global.ParaCommon.matRegister.ToMat();
                                    break;
                                case 1:
                                    matReg2 = Global.ParaCommon.matRegister2.ToMat();
                                    break;
                                case 2:
                                    matReg2 = Global.ParaCommon.matRegister3.ToMat();
                                    break;
                                case 3:
                                    matReg2 = Global.ParaCommon.matRegister4.ToMat();
                                    break;
                            }

                            if (matReg2 != null)
                            {
                                if (!matReg2.IsDisposed)
                                {
                                    if (matReg2.Width != 0)
                                    {
                                        G.IsCalib = false;
                                        pEditTool.Visible = true;
                                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = matReg2.Clone();
                                        G.IsCalib = false;
                                        Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                                        Global.EditTool.View.imgView.Invalidate();
                                        Global.EditTool.View.imgView.Update();
                                        ShowTool.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
                                        Global.Config.imgZoom = View.imgView.Zoom;
                                        Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                        Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                    }
                                }
                                else   
                                {
                                    FormWarning formWarning = new FormWarning("Image Master", "Please,Register Image!");
                                    formWarning.ShowDialog();
                                    Global.StepEdit.btnStep2.IsCLick = true;
                                    Global.Step = Step.Step2;
                                   
                                }
                            }
                           else
                            {
                                FormWarning formWarning = new FormWarning("Image Master", "Please,Register Image!");
                                formWarning.ShowDialog();
                                Global.StepEdit.btnStep2.IsCLick = true;
                                Global.Step = Step.Step2;
                             
                            }
                        }
                        catch (Exception ex)
                        {
                            FormWarning formWarning = new FormWarning("Image Master", "Please,Register Image!");
                            formWarning.ShowDialog();
                            Global.StepEdit.btnStep2.IsCLick = true;
                           Global. Step = Step.Step2; 
                          

                        }
                        finally
                        {
                            if (matReg2 != null)
                                if (!matReg2.IsDisposed)
                                    matReg2.Dispose();
                        }
                      
                        if (Global.ToolSettings.btnEnEdit.IsCLick)
                            Global.ToolSettings.btnEnEdit.PerformClick();

                        break;
                    case Step.Step4:
                        iconTool.Visible = false;
                        layInforTool.Visible = false;
                        Global.IsAllowReadPLC = false;
                        Global.EditTool.View.btnLive.Enabled = false;
                        Global.StepEdit.btnStep4.IsCLick = true;
                        pName.Visible = true;
                        G.IsCalib = false;

                        pEditTool.Show("Step4");

                        lbTool.Text = "4.Setup Status OutPut";
                        Global.StepEdit.SettingStep4.RefreshLogic();
                        break;
                }
                //if (Global.IsRun)
                //{

                   



                //    BeeInterface.G.Header. btnMode.Text = "RUN";
                //     BeeInterface.G.Header.btnMode.ForeColor = Color.FromArgb(101, 173, 245); ;// Color.DarkSlateGray;
                //    Global.Step = Step.Run;

                //}
                //else
                //{
                //    if (Global.EditTool.View.btnContinuous.IsCLick)
                //        if (Global.EditTool.View.btnContinuous.Enabled == true)
                //            Global.EditTool.View.btnContinuous.PerformClick();
                //    //     Global.EditTool.btnHeaderBar.btnSettingPLC.IsCLick = false;
                


                //}
           
         
               
            }
            
            catch (Exception ex)
            {
             //   MessageBox.Show(ex.Message);
            }
        
        }
      
        public async void DesTroy()
        {


            if (Global.Config.IsEnClock)
                clock.Stop();
        SaveData.Config(Global.Config);
            View.tmContinuous.Enabled = false;
            if(Global.LogsDashboard!=null)
            Global.LogsDashboard.Dispose();
           foreach (Camera camera in BeeCore.Common.listCamera)
                if(camera!=null)
				camera.DestroyAll();

            View.tmContinuous.Enabled = false;
           
            if (Global.Comunication.Protocol.IsConnected)
            {
                Global.Comunication.Protocol.IO_Processing = IO_Processing.Close;
            }

          
            await Task.Delay(100);
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
          
           
          
           
        }

        public View View;
     
     

      
        MultiDockHost DockHost = new MultiDockHost { Dock = DockStyle.Fill };
        public DashboardImages DashboardImages=new DashboardImages();
      public void LockSpilter(bool IsLock)
        {
			split0.Enabled = !IsLock;
            split1.Enabled = !IsLock;
			split2.Enabled = !IsLock;
			split3.Enabled = !IsLock;
			split4.Enabled = !IsLock;
			split5.Enabled = !IsLock;
			split6.Enabled = !IsLock;
            splitter5.Enabled = !IsLock;

            View.split2.Enabled = !IsLock;
			View.split3.Enabled = !IsLock;
			View.split4.Enabled = !IsLock;
			View.split5.Enabled = !IsLock;
			 BeeInterface.G.Header.split1.Enabled = !IsLock;
			 BeeInterface.G.Header.split2.Enabled = !IsLock;
			Global.ToolSettings.split1.Enabled = !IsLock;
			BeeInterface.G.StatusDashboard.IsLockSplit = IsLock;
		}
        private Version GetDllVersion(string filePath)
        {
            try
            {
                var info = FileVersionInfo.GetVersionInfo(filePath);
                return new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
            }
            catch
            {
                return null;
            }
        }
        private void EditTool_Load(object sender, EventArgs e)
        {
            String[] ListStep = Global.Config.ListNameStep.Split('\n');
            if (ListStep.Length > 0)
            {
               StepProccessBar.SetSteps(ListStep);
               StepProccessBar.DoneCount = 0;
            }
        else
            {
                StepProccessBar.SetSteps(new[] { "Start", "Marking 1", "Camera 1", "Marking 2", "Camera 2", "Done" });
                StepProccessBar.DoneCount = 0;

            }    
             
            // Chua ch?y gì


            lbVersion.Text= GetDllVersion("BeeUi.dll").ToString()??"----";

            
           _styles = new ControlStylePersistence(this, "MyPanelTheme")
            {
                LoadImmediately = true
            };
            tmReLoadSplit.Enabled = true;
            BeeCore.CustomGui.RoundRg(pInfor, 20);
            //   this.pInfor.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, Global.Config.colorGui);
            pInfor.Height = (int)(pInfor.Height * Global.PerScaleHeight);



          lbLicence.Text = "Licence: " + G.Licence;
            DashboardImages.ImageClicked += DashboardImages_ImageClicked;
            pHeader.Height = (int)(pHeader.Height * Global.PerScaleHeight);
            pTop.Height = (int)(pTop.Height * Global.PerScaleHeight);
            pEdit.Width = (int)(pEdit.Width * Global.PerScaleWidth);
            if (Global.ToolSettings == null)
            {
                Global.ToolSettings = new ToolSettings();
                Global.ToolSettings.Location = new Point(0, 0);
                // Global.ToolSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                Global.ToolSettings.pAllTool.Visible = true;
                Global.ToolSettings.Dock = DockStyle.Fill;
            }
            if (BeeInterface.G.StatusDashboard == null)
            {
               BeeInterface.  G.StatusDashboard = new StatusDashboard();
               BeeInterface.  G.StatusDashboard.InfoBlockBackColor = Color.FromArgb(Global.Config.AlphaBar - 50, Global.Config.colorGui.R, Global.Config.colorGui.G, Global.Config.colorGui.B);
               BeeInterface.  G.StatusDashboard.StatusBlockBackColor = Color.FromArgb(Global.Config.AlphaBar - 50, Global.Config.colorGui.R, Global.Config.colorGui.G, Global.Config.colorGui.B);
               BeeInterface.  G.StatusDashboard.MidHeaderBackColor = Color.FromArgb(Global.Config.AlphaBar, Global.Config.colorGui.R, Global.Config.colorGui.G, Global.Config.colorGui.B);
                  }
            BeeInterface.G.SettingPLC=new ProtocolPLC();
            Global.StepEdit=new StepEdit();
            pEditTool.Register("Tool", () => Global.ToolSettings);
            pEditTool.Register("Step1", () => Global.StepEdit.SettingStep1);
            pEditTool.Register("Step2", () => Global.StepEdit.SettingStep2);
            pEditTool.Register("PLC", () => BeeInterface.G.SettingPLC);
            pEditTool.Register("Step4", () => Global.StepEdit.SettingStep4);
            pEditTool.Register("Images", () => DashboardImages);
            pEditTool.Register("Logs", () => Global.LogsDashboard);
            pInfor.Register("Dashboard", () =>BeeInterface.  G.StatusDashboard);
            pInfor.Register("StepEdit", () => Global.StepEdit);
           progBarTool.Checked = Global.EditTool.pHeader.Visible ;
            btnShowTop.Checked = Global.EditTool.pTop.Visible;
            btnShowDashBoard.Checked = Global.EditTool.pInfor.Visible;
            btnMenu.Checked = Global.EditTool.View.pBtn.Visible;
            //   Global.LogsDashboard.AddLog(LeveLLog.INFO, "?ng d?ng kh?i d?ng", "Main");

            btnShowToolBar.Checked = btnShowToolBar.Checked;
            if (Global.EditTool.pEdit.Width == 0)
            {
                btnShowToolBar.Checked = false;
                // Global.EditTool.hideBar.btnShowToolBar.IsCLick = true;
            }
            else
            {
                btnShowToolBar.Checked = true;
            }
            if (Global.Config.IsForceByPassRS)
            {

                Global.EditTool.lbBypass.Visible = true;


            }
            else
            {
                Global.EditTool.lbBypass.Visible = false;

            }
            //  Global.ExChanged += Global_ExChanged;
            if(BeeCore.Common.listCamera.Count>0)
            if (BeeCore.Common.listCamera[Global.IndexCCCD] != null)
                BeeCore.Common.listCamera[Global.IndexCCCD].FrameChanged += EditTool_FrameChanged;
            Global.StepModeChanged += Global_StepModeChanged;
            Global.DisPLCChange += Global_DisPLCChange;
            StepProccessBar.Visible = Global.Config.IsShowProgressingPLC;
            Global.ByPassResultChanged += Global_ByPassResultChanged;

            clock = new FlipClockDashboard();
            BeeInterface.G.StatusDashboard.CycleTime = 0;
            BeeInterface.G.StatusDashboard.CamTime = 0;
            BeeInterface.G.StatusDashboard.TotalTimes = Global.Config.SumTime;
            BeeInterface.G.StatusDashboard.OkCount = Global.Config.SumOK;
            BeeInterface.G.StatusDashboard.NgCount = Global.Config.SumNG;
            Global.EStopChanged += Global_EStopChanged;

            //
        }
        FormWarning formWarning;
        private void Global_EStopChanged(bool obj)
        {
            this.Invoke((Action)(() =>
            {
                if (obj)
                {
                     formWarning = new FormWarning("ESOP", "Nút nh?n ESOP dã du?c Nh?n !!! " + Global.Ex);
                    formWarning.btnCancel.Visible = false;
                    formWarning.TopMost = true;
                    formWarning.Show();
                }
                else
                {
                    if(formWarning!=null)
                    formWarning.Close();
                }
            }));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Global.Config.IsEnClock)
            {
                clock.Width = 200;
                clock.Dock = DockStyle.Left;
                pTop.Controls.Add(clock);
                clock.BackColor = pTop.BackColor;
                clock.BringToFront();
                clock.Start();
            }
        }


        FlipClockDashboard clock;
        private void Global_ByPassResultChanged(bool obj)
        {
            this.Invoke((Action)(() =>
            {
                if (Global.Config.IsForceByPassRS) 
                    lbBypass.Visible = true;
                else
                    lbBypass.Visible = obj;
            }));
        }

        private void DashboardImages_ImageClicked(object sender, ImageClickedEventArgs e)
        {
            if (View.imgView.Image != null)
            {
                View.imgView.Image.Dispose();
                View.imgView.Image = null;
            }

            if (e.Image != null)
            {
               // View.imgView.SizeMode = PictureBoxSizeMode.Zoom;
                View.imgView.Image = e.Image;
            }
            else
            {
                // chua load xong ? load tr?c ti?p t? file (n?u mu?n)
                View.imgView.Image = System.Drawing.Image.FromFile(e.Path);
            }
            ShowTool.Full(View.imgView, View.imgView.Image.Size);
            // lblInfo.Text = $"{e.Caption}  ({e.OriginalSize.Width}×{e.OriginalSize.Height})";
        }

   
        private void Global_DisPLCChange(bool obj)
        {
            lbdisPLC.Visible = obj;
        }

        private void Global_StepModeChanged(Step obj)
        {
      
            RefreshGuiEdit(obj);
        }

        private void EditTool_FrameChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invoke((Action)(() =>
            {
                lbFrameRate.Text = sender.ToString() ;

            }));
           
        }

      

        private void outLine_Load(object sender, EventArgs e)
        {

        }
       
       
    
        int indexTool = 0;
      
        public int targetWidth = 0;
      
        int value = -1;
        int valueOld = -1;
        DateTime dtOld, dt2;

        private void pView_SizeChanged(object sender, EventArgs e)
        {

         
            if (View == null)
                    return;
           
            View.Size = pView.Size;

            View.Dock = DockStyle.Fill;
           

        }

     
        private void toolStripPort_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure", "byPass PLC",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                toolStripPort.Text = "ByPass PLC";
                Global.Comunication.Protocol.IsBypass = true;
            }
        }

        private void lbLicence_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure", "Initial Python", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              
                //foreach (Tools tool in G.listAlltool[Global.IndexProgChoose])
                //    tool.tool.LoadPara();
              //   BeeInterface.G.Header.workLoadProgram.RunWorkerAsync();
            }
        }

      
        private void pInfor_SizeChanged(object sender, EventArgs e)
        {
            BeeCore.CustomGui.RoundRg(pInfor, 20);
        }

        private void resetUI_Click(object sender, EventArgs e)
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BeeInterface");
            if (MessageBox.Show("Are You Sure!", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                    AppRestart.Delayed(2000);
                   // Application.Restart(); Environment.Exit(0);
                }
            }
        }

        private void UnlockSpiltter_Click(object sender, EventArgs e)
        {
            _layout.SplitterLocked = !_layout.SplitterLocked;
			LockSpilter(_layout.SplitterLocked);
			if (!_layout.SplitterLocked)
                UnlockSpiltter.Text = "Lock UI";
            else
                UnlockSpiltter.Text = "UnLock";
        }

        private async void btnFull_Click(object sender, EventArgs e)
        {
            btnFull.Checked=!btnFull.Checked; 
            pTop.Visible = !btnFull.Checked;
           pHeader.Visible = !btnFull.Checked;
            View.pBtn.Visible = !btnFull.Checked;
            btnShowTop.Checked = Global.EditTool.pTop.Visible;
            btnShowDashBoard.Checked = Global.EditTool.pInfor.Visible;
            btnMenu.Checked = Global.EditTool.View.pBtn.Visible;
            await Task.Delay(100);
            ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
        }

        private void btnShowTop_Click(object sender, EventArgs e)
        {
            btnShowTop.Checked = !btnShowTop.Checked;
            Global.EditTool.pTop.Visible= btnShowTop.Checked ;
          
        }

        private void btnShowDashBoard_Click(object sender, EventArgs e)
        {
            btnShowDashBoard.Checked = !btnShowDashBoard.Checked;
            Global.EditTool.pInfor.Visible= btnShowDashBoard.Checked ;
            
        }

        private void btnMenu_Click_1(object sender, EventArgs e)
        {
            btnMenu.Checked = !btnMenu.Checked;
           Global.EditTool.View.pBtn.Visible = btnMenu.Checked ;
        }

        private void btnShowToolBar_Click(object sender, EventArgs e)
        {
            btnShowToolBar.Checked = !btnShowToolBar.Checked;
            if (!btnShowToolBar.Checked)
            {
                Global.WidthOldTools = Global.EditTool.pEdit.Width;
                Global.EditTool.pEdit.Width = 0;
                //    Global.EditTool.hideBar.btnShowToolBar.IsCLick = true;
            }
            else
            {
                if (Global.WidthOldTools == 0) Global.WidthOldTools = 400;
                Global.EditTool.pEdit.Width = Global.WidthOldTools;
            }

        }

        private void saveToolStrip_Click(object sender, EventArgs e)
        {
            SaveData.Project(Global.Project);
            MessageBox.Show("Save Complete");
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile=new SaveFileDialog();
            saveFile.Title = "Save As Program";
            saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Program";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
                Directory.CreateDirectory("Program\\" + Global.Project);
                Access.SaveProg("Program\\" + Global.Project + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
                //  Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);

                 BeeInterface.G.Header.RefreshListPJ();
                 BeeInterface.G.Header.ChangeProgram(Global.Project);
                //if (! BeeInterface.G.Header.workLoadProgram.IsBusy)
                //     BeeInterface.G.Header.workLoadProgram.RunWorkerAsync();
              
            }
        }
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void saveImageTool_Click(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw == null) return;
            saveFileDialog.Filter = " PNG|*.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Cv2.ImWrite(saveFileDialog.FileName, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw);
            }
        }

        private void openImageTool_Click(object sender, EventArgs e)
        {
            openFileTool.Enabled = false;
            openFile.Multiselect = true;
            openFile.Filter = "Image files (*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFile.ShowDialog() == DialogResult.OK)
            {

                View.Files = new List<string>();
                View.Files = openFile.FileNames.ToList();
                workLoadFile.RunWorkerAsync();
                //BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = Cv2.ImRead(View.Files[View.indexFile]);
                //View.listMat = new List<Mat>();
                //View.listMat.Add(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                //BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                //View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                //View.btnFile.Enabled = false;
                //Global.StatusMode = StatusMode.SimOne;
                //View.timer.Restart();
                //Global.StatusProcessing = StatusProcessing.Checking;

                //View.btnRunSim.Enabled = true;
            }
        }
        private Native Native = new Native();
        private void openFolderImage_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                View.indexFile = 0;
              View.  Files = new List<string>();
                View.Files = Directory.GetFiles(folderBrowserDialog1.SelectedPath).ToList(); ;
                if (View.Files.Count > 0)
                {
                    View.listMat = new List<Mat>();
                    foreach (string file in View.Files)
                    {
                        View.listMat.Add(new Mat(file));
                    }

                }


            }
            if (!Global.IsRun)
            {
                View.indexFile = 0;
                View.pathFileSeleted = View.Files[View.indexFile];

                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = View.listMat[View.indexFile]; ;// Cv2.ImRead(Files[indexFile]);
              Native.SetImg(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
            }
        }

  
        private void playTool_Click(object sender, EventArgs e)
        {
            if (View.Files == null)
            {
                playTool.Enabled = true; return;
            }
            if (View.Files.Count == 0)
            { playTool.Enabled = true; return; }
            playTool.Enabled = false;
            openFileTool.Enabled = false;
            openImageTool.Enabled = false;
            stopTool.Enabled = true; Global.IsSim = true;
            Global.StatusMode = Global.IsSim ? StatusMode.SimContinuous : StatusMode.None;
         
         

              
                if (View.indexFile >= View.listMat.Count) View.indexFile = 0;
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = View.listMat[View.indexFile];// Cv2.ImRead(Files[indexFile]);
                View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
            View.timer = CycleTimerSplit.Start();
            Global.TriggerNum = TriggerNum.Trigger1;
                Global.StatusProcessing = StatusProcessing.Checking;
     
            if (View.indexFile >= View.Files.Count)
                View.indexFile = 0;
        }

        private void stopTool_Click(object sender, EventArgs e)
        {
            openFileTool.Enabled = true;
            openImageTool.Enabled = true;

            Global.IsSim = false; 
            Global.StatusMode = Global.IsSim ? StatusMode.SimContinuous : StatusMode.None;

            stopTool.Enabled = false;
            playTool.Enabled = true;
        }

        private void openFileTool_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
            
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = Cv2.ImRead(openFile.FileName);
              //  View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                View.timer = CycleTimerSplit.Start();
                if (Global.IsRun)
                {
                    Global.TriggerNum = TriggerNum.Trigger1;
                    Global.StatusProcessing = StatusProcessing.Checking;
                }
                else
                {
                    BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = Cv2.ImRead(openFile.FileName);
                    View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                }
            }
            }

        private void dashboardList_Click(object sender, EventArgs e)
        {

        }

        private void debugTool_Click(object sender, EventArgs e)
        {
            debugTool.Checked = !debugTool.Checked;
            Global.IsDebug = debugTool.Checked;
        }

        private void workLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            if (View.Files.Count > 0)
            {
                View.listMat = new List<Mat>();
               
                View.indexFile = 0;
               
                foreach (string file in View.Files)
                {
                    View.listMat.Add(new Mat(file));
                }

             
            }
        }

        private void workLoadFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            openFileTool.Enabled = true;
            playTool.Enabled = true;
        }

        private void tmReLoadSplit_Tick(object sender, EventArgs e)
        {
			LockSpilter(true);
			split0.Height = 5;
			split1.Height = 5;
			split2.Height = 5;
			split3.Height = 5;
			split4.Height = 5;
			split5.Height = 5;
			split6.Height = 5;
			View.split2.Height = 5;
			View.split3.Height = 5;
			View.split4.Height = 5;
			View.split5.Height = 5;
			 BeeInterface.G.Header.split1.Height = 5;
			 BeeInterface.G.Header.split2.Height = 5;
			Global.ToolSettings.split1.Height = 5;
			
			tmReLoadSplit.Enabled = false;
        }

        private void pRight_SizeChanged(object sender, EventArgs e)
        {
         //   BeeCore.CustomGui.RoundRg(pRight, 20,Corner.Both);
        }

        private void EditTool_SizeChanged(object sender, EventArgs e)
        {

        }

        private void tmLoad_Tick(object sender, EventArgs e)
        {
            tmLoad.Enabled = false;
          
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            // L?y v? trí ngay du?i nút
            Point menuPoint = new Point(0, btnLogo.Height);

            //if(btnLogo.IsCLick)
            mouseLeft.Show(btnLogo, menuPoint);
            //else
            //    mouseLeft.Hide();
        }
        Camera CameraLive;
        private async void btnLiveCam_Click(object sender, EventArgs e)
        {
            numCam.Visible = !btnLive.IsCLick;
            CameraLive = new Camera(new ParaCamera(), 5);
            CameraLive.IndexCCD = 5;
            CameraLive.IndexConnect = (int)numCam.Value;
            CameraLive.Para.TypeCamera = TypeCamera.USB;
            CameraLive.Init(CameraLive.Para.TypeCamera);

            CameraLive.IsConnected=await CameraLive.Connect("", CameraLive.Para.TypeCamera);
            if(!CameraLive.IsConnected)
            {
                btnLive.IsCLick = false;
                MessageBox.Show("Fail live camera");
                return;
            }
            else
            {
                if(btnLive.IsCLick)
                {
                    StartLive();
                    if (!workLive.IsBusy)
                        workLive.RunWorkerAsync();
                }
                else
                {
                    StopLive();
                    CameraLive.DisConnect(TypeCamera.USB);
                   
                }    
              

            }    
        }
        private Thread _displayThread;
        private readonly AutoResetEvent _frameReady = new AutoResetEvent(false);
        private Bitmap _sharedFrame;
        private int _uiPending; // 0: idle, 1: dang d?y frame lên UI
        void PublishFrame(Bitmap src)
        {
            if (!Global.IsLive) { src.Dispose(); return; }
            // Clone 1 l?n ? producer, không clone trong display thread
            var clone = (Bitmap)src.Clone();
            var old = Interlocked.Exchange(ref _sharedFrame, clone); // gi? frame m?i nh?t, drop cu
            old?.Dispose();
            _frameReady.Set();
        }

        void StartLive()
        {

            _displayThread = new Thread(DisplayLoop) { IsBackground = true, Name = "DisplayLoop" };
            _displayThread.Start();
        }

        void StopLive()
        {

            _frameReady.Set();
            _displayThread?.Join();
            _displayThread = null;

            // Clear ?nh trên UI
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new Action(() =>
                {
                    var old = imgLive.Image;
                    imgLive.Image = null;
                    old?.Dispose();
                }));

            // D?n rác còn sót
            var leftover = Interlocked.Exchange(ref _sharedFrame, null);
            leftover?.Dispose();
            if (CameraLive.matRaw!= null)
                if (CameraLive.matRaw != null)
                    if (!CameraLive.matRaw.IsDisposed)
                        if (!CameraLive.matRaw.Empty())
                        {
                            CameraLive.Read();
                            imgLive.Image = CameraLive.matRaw.ToBitmap();
                            
                            ShowTool.Full(imgLive, CameraLive.GetSzCCD());
                        }

        }

        void DisplayLoop()
        {
            while (btnLive.IsCLick)
            {
                _frameReady.WaitOne(50);        // ch? tín hi?u có frame (ho?c timeout d? thoát nhanh)
                if (!btnLive.IsCLick) break;

                // L?y quy?n s? h?u frame m?i nh?t và làm r?ng buffer chung
                var frame = Interlocked.Exchange(ref _sharedFrame, null);
                if (frame == null) continue;

                // Ch? cho phép 1 c?p nh?t UI pending; n?u UI chua k?p x? lý ? drop frame
                if (Interlocked.Exchange(ref _uiPending, 1) == 1)
                {
                    frame.Dispose();
                    continue;
                }

                try
                {
                    if (IsHandleCreated && !IsDisposed)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                var old = imgLive.Image;
                                imgLive.Image = frame;   // chuy?n quy?n s? h?u cho PictureBox
                                old?.Dispose();          // h?y ?nh cu sau khi gán
                            }
                            finally
                            {
                                Interlocked.Exchange(ref _uiPending, 0);
                            }
                        }));
                    }
                    else
                    {
                        frame.Dispose();
                        Interlocked.Exchange(ref _uiPending, 0);
                    }
                }
                catch
                {
                    frame.Dispose();
                    Interlocked.Exchange(ref _uiPending, 0);
                }
            }
        }
        private void workLive_DoWork(object sender, DoWorkEventArgs e)
        {
            CameraLive.Read();
        }

        private void workLive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           

         

                if (CameraLive.matRaw != null)
                    if (!CameraLive.matRaw.IsDisposed)
                        if (!CameraLive.matRaw.Empty())
                        {
                           // Global.Config.SizeCCD = CameraLive.GetSzCCD();
                            // matRaw là OpenCvSharp.Mat
                            var bmp = BitmapConverter.ToBitmap(CameraLive.matRaw);

                            // Ð?y frame m?i nh?t và h?y frame cu m?t cách an toàn, không c?n lock
                            var old = Interlocked.Exchange(ref _sharedFrame, bmp);
                            old?.Dispose();

                            // (tu? ch?n) báo cho display thread là có frame m?i
                            _frameReady?.Set();
                            //using (Bitmap frame = BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw))
                            //{

                            //        _sharedFrame?.Dispose();
                            //        _sharedFrame = (Bitmap)frame.Clone(); // Clone d? thread-safe

                            //}
                        }

              
                workLive.RunWorkerAsync();
                return;
            
        }

        private void webCamTool_Click(object sender, EventArgs e)
        {
            pCamera.Visible=webCamTool.Checked;
        }

        private void autoFontLabel2_Click(object sender, EventArgs e)
        {

        }

        private void customGuiTool_Click(object sender, EventArgs e)
        {
            _styles.ShowEditor();
        }

        private void Logo_Click(object sender, EventArgs e)
        {
           
        }

        private void btnLogo_Click_1(object sender, EventArgs e)
        {
            if (Global.Config.Users == Users.Admin)
            {
                Point menuPoint = new Point(0, btnLogo.Height);

                //if(btnLogo.IsCLick)
                mouseLeft.Show(btnLogo, menuPoint);
            }
        }

        private void customFormLoadTool_Click(object sender, EventArgs e)
        {
            G.Load.Show();
            G.Load._styles.ShowEditor();

        }

        private void importTool_Click(object sender, EventArgs e)
        {
            openFile.Title = "Import Program";
            openFile.Filter = "Prog|*.tar";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string tarPath = openFile.FileName;
                TarProgramHelper.ImportToDefaultProgram(tarPath);
                Global.Project = Path.GetFileNameWithoutExtension(tarPath);
                 BeeInterface.G.Header.RefreshListPJ();
                 BeeInterface.G.Header.ChangeProgram(Global.Project);
                //if (! BeeInterface.G.Header.workLoadProgram.IsBusy)
                //     BeeInterface.G.Header.workLoadProgram.RunWorkerAsync();
            }

            //string tarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProgramBackup.tar");
            //// Import l?i vào \Program
            //TarProgramHelper.ImportToDefaultProgram(tarPath);
        }

        private void exportTool_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "Export Program";
            saveFileDialog.Filter = "Prog|*.tar";
            saveFileDialog.FileName = Global.Project+".tar";
            if (saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                string tarPath = saveFileDialog.FileName;
                TarProgramHelper.ExportProgramSubFolderWithRename(Global.Project, tarPath);
            }    
            

        }

        private void btnRJBtn_Click(object sender, EventArgs e)
        {
            Process.Start("osk.exe");
        }

        private void progBarTool_Click(object sender, EventArgs e)
        {
            progBarTool.Checked = !progBarTool.Checked;
           
            Global.EditTool.pHeader.Visible = progBarTool.Checked;
        }

        private void btnShowResult_Click(object sender, EventArgs e)
        {
           // Global.IsShowImageResult = btnShowResult.IsCLick;
        }

        private void exportListTool_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Export List";
            saveFile.Filter = " Text|*.txt";
            BeeCore.Common.SetToolList(Global.IndexProgChoose, new List<BeeCore.PropetyTool>());
            
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(saveFile.FileName,  BeeInterface.G.Header.listNameProg);
             }
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            Global.IsEstop = !Global.IsEstop;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "New Program";
            BeeCore.Common.SetToolList(Global.IndexProgChoose, new List<BeeCore.PropetyTool>());
            saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Program";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
                Directory.CreateDirectory("Program\\" + Global.Project);
                Access.SaveProg("Program\\" + Global.Project + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
                 BeeInterface.G.Header.RefreshListPJ();
                 BeeInterface.G.Header.ChangeProgram(Global.Project);
                //if (! BeeInterface.G.Header.workLoadProgram.IsBusy)
                //     BeeInterface.G.Header.workLoadProgram.RunWorkerAsync();
            }
        }

       
    }
}
