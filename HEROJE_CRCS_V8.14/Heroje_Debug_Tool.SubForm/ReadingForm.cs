using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CoordinateConv_N;
using FTPManager_N;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.Properties;
using HJ_Controls_Lib;
using 合杰图像算法调试工具;
using 图像算法调试工具;

namespace Heroje_Debug_Tool.SubForm
{
	public class ReadingForm : Form
	{
		public enum SetControlsValDef
		{
			SetFocus,
			SetExposure,
			SetGain,
			SetDecodeModeA,
			SetDecodeModeB,
			SetDecodeModeC,
			TrainCheck_Y,
			TrainCheck_N,
			BtnTrainAction,
			BtnTrainTextChange
		}

		public delegate void SetControlsValueCB(SetControlsValDef act, int val_1, string val_2);

		public delegate void ChartDelegate(Chart _Chart, uint data);

		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private Image ImageShow = new Bitmap(376, 240);

		private Bitmap MaskImg = new Bitmap(10, 10);

		private SolidBrush MaskBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));

		private Pen ROIPen = new Pen(new SolidBrush(Color.FromArgb(200, 0, 0, 200)), 2f);

		public int ratio_value_cfg;

		private int temp_exp_val;

		public DeviceSettingForm DeviceSettingPage = new DeviceSettingForm();

		private bool IsDrawingFocusROI = false;

		private bool IsDrawingTemplateRoi = false;

		private bool IsDrawingImageRoi = false;

		private object[] invokeChartData = new object[2];

		private uint MaxDecodeTime = 0u;

		private object thisLock = new object();

		private double CurrentMax = 0.0;

		private double Y_Interval = 0.0;

		public Pen ImageRoiPen = new Pen(new SolidBrush(Color.FromArgb(200, 0, 200, 0)), 2f);

		private int ReSetImageRoiCount = 0;

		private int PanelWidth;

		private int PanelHeight;

		private int Roi_Start_x;

		private int Roi_Start_y;

		private int Roi_End_x;

		private int Roi_End_y;

		private int ImgRoiWid;

		private int ImgRoiHgt;

		private bool LastTrigIsTrigMode = false;

		private string LastBtnString;

		private int FocusRoiX0;

		private int FocusRoiX1;

		private int FocusRoiY0;

		private int FocusRoiY1;

		private DeviceConnectForm.Polygon BarCodeRegion0;

		private ParameterTraining AiTrain = new ParameterTraining();

		private Image old_img;

		private int rect_x0;

		private int rect_y0;

		private int rect_w;

		private int rect_h;

		private bool IsShowAllTemplateRoi = false;

		private bool IsMouseDrawImageRoi = false;

		private IContainer components = null;

		private SplitContainer MainContainer;

		private SplitContainer TopContainer;

		private SplitContainer BottomContainer;

		private GroupBox GrbDecodeData;

		private Label LabBarcodeLen;

		private Label LabBarcodeLenDisp;

		private Label LabBarcodeType;

		private TextBox TxbDecodeData;

		private Label LabBarcodeTypeDisp;

		private Button BtnSensorLensFocusAdd;

		private Button BtnSensorLensFocusDec;

		private Button BtnSensorGainSetDec;

		private Button BtnExpSetRatioDec;

		private Button BtnSensorGainSetAdd;

		private Button BtnExpSetRatioAdd;

		private TrackBar TrbSensorGainSet;

		private TrackBar TrbExpSetRatio;

		private TrackBar TrbSensorLensFocusSet;

		private Panel PanArrow_3;

		private Panel PanArrow_2;

		private Panel PanArrow_1;

		private Button BtnEnterADP;

		private Button BtnTrigAutoFocus;

		private Button BtnReadContinuous;

		private Label LabFramePerSec;

		private Label LabFrameRate;

		private Label LabFrameCountDisp;

		private Label LabImageCount;

		private Label LabImageCountDips;

		private Label LabImageSize;

		private ComboBox CobImageSize;

		private ContextMenuStrip CMS_ImageRightKey;

		private ToolStripMenuItem TSI_Zoom;

		private ToolStripMenuItem TSI_FitZoom;

		private PanelEx PanImage;

		private TabControl TabSmallForm;

		private Label LabFocus;

		private Label LabSensorGain;

		private Label LabExposure;

		private TabPage Tp_DecodeTime;

		private TabPage Tp_DecStatistics;

		private Panel PanDecodeTime;

		private Chart ChartDecodeTime;

		private Label LabCurrentDecodeTime;

		private Label LabAveDecodeTime;

		private Label LabAveDecodeTimeDisp;

		private Label LabNowDecodeTime;

		private Panel PanDecStatistics;

		private CheckBox ChkRealTimeUpdate;

		private Label LabNGCount;

		private Label LabOKCount;

		private Label LabTrigCount;

		private Button BtnResetStatic;

		private Label LabTotalDisp;

		private Label LabDispNG;

		private Label LabOKDisp;

		private Label LabOKRatio;

		private Label LabStateADP;

		private ToolStripMenuItem TSI_SaveCurrentImage;

		private ToolStripMenuItem TSI_ShowNetGrid;

		private Button BtnSetting;

		private Button BtnPerformOneTrig;

		private GroupBox GrbImageROI;

		private Button BtnImageRoiSet;

		private Button BtnImageRoiCancel;

		private TextBox TxbRoiH;

		private TextBox TxbRoiW;

		private TextBox TxbRoiY0;

		private TextBox TxbRoiX0;

		private Label LabRoiH;

		private Label LabRoiW;

		private Label LabRoiY0;

		private Label LabRoiX0;

		private Label LabGainVal;

		private Label LabExposureVal;

		private Label LabFocusVal;

		private Button BtnSelectFocusROI;

		private Panel PanArrow_4;

		private ToolStripMenuItem TSI_DrawRectangle;

		private ToolStripMenuItem TSI_ShowAllTemplateRoi;

		private ProgressBar PgbAutoAdjProcess;

		private Panel PanManualFocus;

		private Panel PanAutoFocus;

		public int TempExpVal
		{
			get
			{
				ratio_value_cfg = 0;
				if (GetCfgCBFuncCB(7296u) == 0)
				{
					ratio_value_cfg = 2;
					temp_exp_val = TrbExpSetRatio.Value;
				}
				else
				{
					ratio_value_cfg = 32;
					temp_exp_val = DeviceSettingPage.ExpValueVal;
				}
				return temp_exp_val;
			}
			set
			{
				temp_exp_val = value;
				if (ratio_value_cfg == 2)
				{
					DeviceSettingPage.IsRatioExp = true;
					TrbExpSetRatio.Value = temp_exp_val;
				}
				else
				{
					DeviceSettingPage.IsRatioExp = false;
					DeviceSettingPage.ExpValueVal = temp_exp_val;
				}
			}
		}

		public int TempGainVal
		{
			get
			{
				return TrbSensorGainSet.Value;
			}
			set
			{
				TrbSensorGainSet.Value = value;
			}
		}

		public int TempFocusVal
		{
			get
			{
				return TrbSensorLensFocusSet.Value;
			}
			set
			{
				TrbSensorLensFocusSet.Value = value;
			}
		}

		public ReadingForm()
		{
			InitializeComponent();
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			ChkRealTimeUpdate.Checked = GetCfgCBFuncCB(78081u) == 1;
			if (para.ParaDataLen >= 4096)
			{
				BtnTrigAutoFocus.Enabled = true;
				int num = GetCfgCBFuncCB(74751u) * 256 + GetCfgCBFuncCB(74495u);
				if (num <= TrbSensorLensFocusSet.Maximum)
				{
					TrbSensorLensFocusSet.Value = num;
				}
			}
			try
			{
				if (GetCfgCBFuncCB(11272u) == 8)
				{
					int num2 = GetCfgCBFuncCB(7951u);
					if (num2 < 0 || num2 > 32)
					{
						num2 = 3;
					}
					int num3 = (GetCfgCBFuncCB(54527u) * 8 - ToolCfg.width_offset) / num2;
					int num4 = (GetCfgCBFuncCB(54783u) * 8 - ToolCfg.height_offset) / num2;
					int num5 = GetCfgCBFuncCB(55039u) * 8 / num2;
					int num6 = GetCfgCBFuncCB(55295u) * 8 / num2;
				}
				else
				{
					PanImage.Invalidate();
				}
			}
			catch (Exception)
			{
			}
			TrbExpSetRatio.Value = GetCfgCBFuncCB(4607u);
			TrbSensorGainSet.Value = GetCfgCBFuncCB(51455u);
			int num7 = GetCfgCBFuncCB(7951u);
			CobImageSize.SelectedIndex = ((GetCfgCBFuncCB(7951u) > 0 && GetCfgCBFuncCB(7951u) < 5) ? (GetCfgCBFuncCB(7951u) - 1) : 2);
			if (GetCfgCBFuncCB(7951u) < 0 || GetCfgCBFuncCB(7951u) > 10)
			{
				CobImageSize.Visible = false;
				LabImageSize.Visible = false;
				CobImageSize.SelectedIndex = 2;
			}
			else
			{
				LabImageSize.Visible = true;
				CobImageSize.Visible = true;
			}
			try
			{
				if (GetCfgCBFuncCB(11272u) == 8)
				{
					BtnImageRoiCancel.Enabled = true;
				}
				else
				{
					BtnImageRoiCancel.Enabled = false;
				}
			}
			catch (Exception)
			{
			}
			DeviceSettingPage.UpdateUiDisplay(para);
		}

		public void FunctionOnOff(bool[] CapacityArr, string DeviceName)
		{
			PanAutoFocus.Enabled = CapacityArr[0];
			PanManualFocus.Enabled = CapacityArr[0];
			DeviceSettingPage.FunctionOnOff(CapacityArr, DeviceName);
		}

		public void UpdateLanguageUI()
		{
			int selectedIndex = CobImageSize.SelectedIndex;
			CobImageSize.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobImageSize.Items.AddRange(new object[3] { "高清", "正常", "快速" });
			}
			else
			{
				CobImageSize.Items.AddRange(new object[3] { "HD", "Normal", "Quick" });
			}
			CobImageSize.SelectedIndex = selectedIndex;
		}

		private void SmallFormInit()
		{
			DeviceSettingPage.SetCBFunc(SetCfgCBFuncCB, GetCfgCBFuncCB, SendCfgDataCBFuncCB, SetControlsValueFuncCB, ImageRoiActionCBFunc);
			DeviceSettingPage.Page_Init();
		}

		public void Page_Init()
		{
			ImageShow = PanImage.BackgroundImage;
			for (int i = 0; i < 20; i++)
			{
				ToolCfg.DecoderTimeQueue.Enqueue(0.0);
			}
			ChartDecodeTime.Series[0].Points.Clear();
			UpdateQueueValue(0u);
			SmallFormInit();
		}

		public void ReadingPage_Update(ReadingPageParaStu ParaData, ReadingPageActDef act)
		{
			ProtocolExtraDataStu protocolExtraData = ParaData.ProtocolExtraData;
			switch (act)
			{
			case ReadingPageActDef.UpdateDecodeCount:
				LabTrigCount.Text = (protocolExtraData.OKCount + protocolExtraData.NGCount).ToString();
				LabOKCount.Text = protocolExtraData.OKCount.ToString();
				LabNGCount.Text = protocolExtraData.NGCount.ToString();
				if (protocolExtraData.OKCount + protocolExtraData.NGCount == 0)
				{
					LabOKRatio.Text = "100.00%";
				}
				else
				{
					LabOKRatio.Text = ((double)protocolExtraData.OKCount / (double)(protocolExtraData.OKCount + protocolExtraData.NGCount)).ToString("0.00%");
				}
				break;
			case ReadingPageActDef.AF_OK:
			{
				int num = GetCfgCBFuncCB(74751u) * 256 + GetCfgCBFuncCB(74495u);
				if (num <= TrbSensorLensFocusSet.Maximum)
				{
					TrbSensorLensFocusSet.Value = num;
				}
				BtnTrigAutoFocus.Enabled = true;
				break;
			}
			case ReadingPageActDef.AP_START:
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					LabStateADP.Text = "调参中...";
				}
				else
				{
					LabStateADP.Text = "Adjusting..";
				}
				break;
			case ReadingPageActDef.AP_PROC:
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					LabStateADP.Text = "适配解码...";
				}
				else
				{
					LabStateADP.Text = "Continue...";
				}
				break;
			case ReadingPageActDef.ADC_PROC:
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					LabStateADP.Text = "解码训练...";
				}
				else
				{
					LabStateADP.Text = "Trainning.";
				}
				break;
			case ReadingPageActDef.AP_OK_END:
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					LabStateADP.Text = "调参完成!";
				}
				else
				{
					LabStateADP.Text = "ADJ OK!";
				}
				AiTrain.IsTraining = false;
				BtnEnterADP.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.AiTrain);
				break;
			case ReadingPageActDef.AP_NG_END:
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					LabStateADP.Text = "调参失败!";
				}
				else
				{
					LabStateADP.Text = "ADJ NG!";
				}
				AiTrain.IsTraining = false;
				BtnEnterADP.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.AiTrain);
				break;
			case ReadingPageActDef.UpdateDecodeTimeInfo:
				LabNowDecodeTime.Text = ParaData.DecodeTime + "ms";
				UpdateQueueValue(ParaData.DecodeTime);
				LabBarcodeLen.Text = ParaData.BarcodeLen.ToString();
				break;
			case ReadingPageActDef.UpdateBarcodeStr:
				if (ParaData.barcode_str != "")
				{
					LabBarcodeType.Text = ParaData.barcode_type;
					TxbDecodeData.AppendText(ParaData.barcode_str + Environment.NewLine);
					BarCodeRegion0 = ParaData.BarCodeRegion;
					AiTrain.UpdateDecodeState(BarCodeRegion0, true, null);
				}
				else
				{
					AiTrain.UpdateDecodeState(BarCodeRegion0, false, null);
				}
				break;
			case ReadingPageActDef.UpdateFrameRate:
				LabFrameRate.Text = ParaData.FrameRate;
				break;
			case ReadingPageActDef.UpdateTrigCount:
				LabImageCount.Text = ParaData.TrigCount.ToString();
				break;
			case ReadingPageActDef.UpdateImage:
				ImageShow = (Image)ParaData.ImageShow.Clone();
				AiTrain.UpdateDecodeState(BarCodeRegion0, false, ParaData.ImageShow);
				DrawRoiAndShowImage();
				break;
			}
		}

		private void SetControlsValueFuncCB(SetControlsValDef act, int val_1, string val_2)
		{
			switch (act)
			{
			case SetControlsValDef.SetFocus:
				TrbSensorLensFocusSet.Value = val_1;
				break;
			case SetControlsValDef.SetExposure:
				TrbExpSetRatio.Value = val_1;
				break;
			case SetControlsValDef.SetGain:
				TrbSensorGainSet.Value = val_1;
				break;
			}
		}

		public void ImageRoiActionCBFunc(DeviceSettingForm.ImageRoiActionNum num)
		{
			switch (num)
			{
			case DeviceSettingForm.ImageRoiActionNum.DrawRoiStart:
				PanImage.IsDefaultMode = false;
				PanImage.SetDrawWhenMove(false);
				IsDrawingImageRoi = true;
				break;
			case DeviceSettingForm.ImageRoiActionNum.DrawRoiOk:
				PanImage.IsDefaultMode = true;
				IsDrawingImageRoi = false;
				if (GetCfgCBFuncCB(11272u) != 8)
				{
				}
				break;
			case DeviceSettingForm.ImageRoiActionNum.DrawRoiCancel:
				PanImage.IsDefaultMode = true;
				IsDrawingImageRoi = false;
				break;
			case DeviceSettingForm.ImageRoiActionNum.DrawTemplateRoi:
				PanImage.IsDefaultMode = false;
				PanImage.SetDrawWhenMove(false);
				PanImage.ActionNum = 0;
				TSI_DrawRectangle.Checked = true;
				TSI_Zoom.Checked = false;
				ToolCfg.TemplateRoiX = 0;
				ToolCfg.TemplateRoiY = 0;
				ToolCfg.TemplateRoiW = 0;
				ToolCfg.TemplateRoiH = 0;
				IsDrawingTemplateRoi = true;
				break;
			case DeviceSettingForm.ImageRoiActionNum.DrawTemplateRoiOk:
				PanImage.IsDefaultMode = true;
				TSI_DrawRectangle.Checked = false;
				TSI_Zoom.Checked = true;
				IsDrawingTemplateRoi = false;
				break;
			}
		}

		private void UpdateQueueValue(uint dat)
		{
			invokeChartData[0] = ChartDecodeTime;
			invokeChartData[1] = dat;
			ChartDecodeTime.BeginInvoke(new ChartDelegate(ChartDelegateMethod), invokeChartData);
		}

		public void ChartDelegateMethod(Chart _Chart, uint data)
		{
			if (data > MaxDecodeTime)
			{
				MaxDecodeTime = data;
			}
			try
			{
				lock (thisLock)
				{
					double num = 0.0;
					_Chart.Series[0].Points.Clear();
					ToolCfg.DecoderTimeQueue.Dequeue();
					ToolCfg.DecoderTimeQueue.Enqueue(data);
					CurrentMax = 0.0;
					for (int i = 0; i < 20; i++)
					{
						double num2 = ToolCfg.DecoderTimeQueue.ElementAt(i);
						num += num2;
						_Chart.Series[0].Points.AddXY(i, num2);
						if (CurrentMax < num2)
						{
							CurrentMax = num2;
						}
					}
					LabAveDecodeTime.Text = (int)(num / 20.0) + "ms";
					ChartDecodeTime.ChartAreas[0].AxisY.Maximum = CurrentMax * 1.5;
					Y_Interval = ChartDecodeTime.ChartAreas[0].AxisY.Maximum / 10.0;
					if (Y_Interval > 0.0)
					{
						ChartDecodeTime.ChartAreas[0].AxisY.Interval = Y_Interval;
					}
				}
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		private void ReadingForm_Load(object sender, EventArgs e)
		{
			TrbSensorLensFocusSet.MouseWheel += delegate(object _sender, MouseEventArgs _e)
			{
				((HandledMouseEventArgs)_e).Handled = true;
			};
			TrbExpSetRatio.MouseWheel += delegate(object _sender, MouseEventArgs _e)
			{
				((HandledMouseEventArgs)_e).Handled = true;
			};
			TrbSensorGainSet.MouseWheel += delegate(object _sender, MouseEventArgs _e)
			{
				((HandledMouseEventArgs)_e).Handled = true;
			};
			PanImage.MouseDoubleClickEvent = MouseDoubleClickEventDo;
			PanImage.MouseUpEvent = MouseUpEventDo;
		}

		private void ShowImageRoi(Graphics g)
		{
			if (GetCfgCBFuncCB(11272u) != 8)
			{
				return;
			}
			if (ToolCfg.MaskRect.Width == 0)
			{
				ReSetImageRoiCount = 0;
			}
			if (ReSetImageRoiCount < 15)
			{
				int num = GetCfgCBFuncCB(7951u);
				if (num < 0 || num > 32)
				{
					num = 3;
				}
				int num2 = (GetCfgCBFuncCB(54527u) * 8 - ToolCfg.width_offset) / num;
				int num3 = (GetCfgCBFuncCB(54783u) * 8 - ToolCfg.height_offset) / num;
				int num4 = GetCfgCBFuncCB(55039u) * 8 / num;
				int num5 = GetCfgCBFuncCB(55295u) * 8 / num;
				ToolCfg.MaskRect = new Rectangle(num2, num3, num4, num5);
				ReSetImageRoiCount++;
			}
			g.DrawRectangle(ImageRoiPen, ToolCfg.MaskRect);
		}

		private void ShowTemplateRoi(Graphics g)
		{
			try
			{
				int num = 18;
				int num2 = GetCfgCBFuncCB(7951u);
				num /= num2;
				for (int i = 0; i < ToolCfg.TemplateRoiList.Count; i++)
				{
					Rectangle rect = new Rectangle(ToolCfg.TemplateRoiList[i].Rect.X / num2, ToolCfg.TemplateRoiList[i].Rect.Y / num2, ToolCfg.TemplateRoiList[i].Rect.Width / num2, ToolCfg.TemplateRoiList[i].Rect.Height / num2);
					g.DrawString(ToolCfg.TemplateRoiList[i].RoiName, new Font("宋体", num, FontStyle.Bold), new SolidBrush(ToolCfg.TemplateColor[(ToolCfg.TemplateRoiList[i].Temp_ID - 1) % 20]), rect.X, rect.Y);
					g.DrawRectangle(new Pen(new SolidBrush(ToolCfg.TemplateColor[(ToolCfg.TemplateRoiList[i].Temp_ID - 1) % 20]), 2f), rect);
				}
			}
			catch (Exception)
			{
			}
		}

		private void DrawRoiAndShowImage()
		{
			try
			{
				int num = GetCfgCBFuncCB(7951u);
				PanImage.ImageZoomRate = num;
				Image image = (Image)ImageShow.Clone();
				Graphics graphics = Graphics.FromImage(image);
				int num2 = ImageShow.Width;
				int num3 = ((num2 / 500 <= 0) ? 1 : (num2 / 500));
				Pen pen = new Pen(Color.Blue, num3);
				float num4 = ((num2 / 55 <= 0) ? 1 : (num2 / 55));
				try
				{
					if (IsDrawingImageRoi || IsDrawingTemplateRoi || IsDrawingFocusROI)
					{
						int num5 = 0;
						int num6 = 0;
						int num7 = 0;
						int num8 = 0;
						if (PanImage.ActionNum == 0)
						{
							PanImage.GetRoiCoordinate(out PanelWidth, out PanelHeight, out Roi_Start_x, out Roi_Start_y, out Roi_End_x, out Roi_End_y);
							Rectangle rect = PanImage.GetRect();
							CoordinateConv_N.CoordinateConv.PtbToImg(ToolCfg.SensorWidth, ToolCfg.SensorHeight, PanelWidth, PanelHeight, rect.X, rect.Y, ref num5, ref num6);
							CoordinateConv_N.CoordinateConv.PtbToImg(ToolCfg.SensorWidth, ToolCfg.SensorHeight, PanelWidth, PanelHeight, rect.X + rect.Width, rect.Y + rect.Height, ref num7, ref num8);
							int num9 = Math.Abs(num5 - num7);
							int num10 = Math.Abs(num6 - num8);
							graphics.DrawRectangle(pen, num5 / num, num6 / num, num9 / num, num10 / num);
							Roi_Start_x = num5;
							Roi_Start_y = num6;
							Roi_End_x = num7;
							Roi_End_y = num8;
							bool flag = true;
							if (GetCfgCBFuncCB(55297u) != 0)
							{
							}
							ImgRoiWid = Roi_End_x - Roi_Start_x;
							ImgRoiHgt = Roi_End_y - Roi_Start_y;
							if (ImgRoiWid < 1)
							{
								ImgRoiWid = ((ImgRoiWid == 0) ? 128 : Math.Abs(ImgRoiWid));
							}
							if (ImgRoiHgt < 1)
							{
								ImgRoiHgt = ((ImgRoiHgt == 0) ? 128 : Math.Abs(ImgRoiHgt));
							}
							Roi_Start_x += ToolCfg.width_offset;
							Roi_Start_y += ToolCfg.height_offset;
							if (IsDrawingImageRoi)
							{
								ShowImageRoiCoord(Roi_Start_x, Roi_Start_y, ImgRoiWid, ImgRoiHgt);
							}
							else if (IsDrawingTemplateRoi)
							{
								ToolCfg.TemplateRoiX = Roi_Start_x;
								ToolCfg.TemplateRoiY = Roi_Start_y;
								ToolCfg.TemplateRoiW = ImgRoiWid;
								ToolCfg.TemplateRoiH = ImgRoiHgt;
							}
							else if (IsDrawingFocusROI)
							{
								FocusRoiX0 = Roi_Start_x;
								FocusRoiX1 = Roi_Start_x + ImgRoiWid;
								FocusRoiY0 = Roi_Start_y;
								FocusRoiY1 = Roi_Start_y + ImgRoiHgt;
							}
						}
					}
					ShowImageRoi(graphics);
					ShowTemplateRoi(graphics);
					graphics.Dispose();
				}
				catch (Exception)
				{
				}
				PanImage.BackgroundImage = image;
			}
			catch (Exception)
			{
			}
		}

		private void TSI_DrawRectangle_Click(object sender, EventArgs e)
		{
			ImageRoiActionCBFunc(DeviceSettingForm.ImageRoiActionNum.DrawTemplateRoi);
			DeviceSettingPage.TemplateSettingPage.IsDrawingTemplateRoi = true;
		}

		private void TSI_Zoom_Click(object sender, EventArgs e)
		{
			ImageRoiActionCBFunc(DeviceSettingForm.ImageRoiActionNum.DrawTemplateRoiOk);
			DeviceSettingPage.TemplateSettingPage.IsDrawingTemplateRoi = false;
		}

		private void TSI_FitZoom_Click(object sender, EventArgs e)
		{
			PanImage.ActionNum = 6;
		}

		public void TSI_SaveCurrentImage_Click(object sender, EventArgs e)
		{
			try
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				Image image = ((ToolCfg.SaveImg != null) ? ToolCfg.SaveImg : ImageShow);
				saveFileDialog.Filter = "bitmap|*.bmp";
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					FileStream fileStream = File.Create(saveFileDialog.FileName);
					image.Save(fileStream, ImageFormat.Bmp);
					fileStream.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		private void BtnReadContinuous_Click(object sender, EventArgs e)
		{
			DeviceSettingPage.RdbContinuousTrig_Click(null, null);
			LastTrigIsTrigMode = false;
		}

		private void BtnPerformOneTrig_Click(object sender, EventArgs e)
		{
			if (GetCfgCBFuncCB(3u) == 2)
			{
				DeviceSettingPage.RdbTrigExtern_Click(null, null);
				LastTrigIsTrigMode = true;
			}
			else
			{
				DeviceSettingPage.BtnTrigTest_Click(null, null);
			}
		}

		private void BtnTrigAutoFocus_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(73987u, 1u);
				SendCfgDataCBFuncCB(0u);
				BtnTrigAutoFocus.Enabled = false;
			}
		}

		private void BtnSelectFocusROI_Click(object sender, EventArgs e)
		{
			IsDrawingFocusROI = !IsDrawingFocusROI;
			if (IsDrawingFocusROI)
			{
				LastBtnString = BtnSelectFocusROI.Text;
				BtnSelectFocusROI.Text = "OK";
				PanImage.IsDefaultMode = false;
				PanImage.SetDrawWhenMove(false);
				PanImage.ActionNum = 0;
				return;
			}
			PanImage.IsDefaultMode = true;
			BtnSelectFocusROI.Text = LastBtnString;
			if (FocusRoiX1 - FocusRoiX0 + 1 < 30 || FocusRoiY1 - FocusRoiY0 + 1 < 30)
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("当前选择的区域太小了(<30)，请重新选择", "提示");
				}
				else
				{
					MessageBox.Show("The current selection is too small(<30),Please Reselect", "Tips");
				}
			}
			else
			{
				SetCfgCBFuncCB(91647u, (ushort)(FocusRoiX0 / 8));
				SetCfgCBFuncCB(91903u, (ushort)(FocusRoiY0 / 8));
				SetCfgCBFuncCB(92159u, (ushort)(FocusRoiX1 / 8));
				SetCfgCBFuncCB(92415u, (ushort)(FocusRoiY1 / 8));
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void BtnEnterADP_Click(object sender, EventArgs e)
		{
			LabStateADP.Visible = true;
			if (!AiTrain.IsTraining)
			{
				AiTrain.SetCBFunc(SetCfgCBFuncCB, GetCfgCBFuncCB, SendCfgDataCBFuncCB, TrainingInfoCBFunc);
				AiTrain.PgbAutoAdjProcess = PgbAutoAdjProcess;
				AiTrain.Do_Ai_Training();
				BtnEnterADP.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.StopTrain);
			}
			else
			{
				AiTrain.Stop_Ai_Training();
				BtnEnterADP.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.AiTrain);
			}
		}

		private void TrainingInfoCBFunc(ParameterTraining.TrainingStateDef Act, int Val_1, string Val_2)
		{
			Invoke((MethodInvoker)delegate
			{
				switch (Act)
				{
				case ParameterTraining.TrainingStateDef.ChangeIntoContinueMode:
					BtnReadContinuous_Click(null, null);
					break;
				case ParameterTraining.TrainingStateDef.UpdateLabStateString:
					LabStateADP.Text = Val_2;
					break;
				case ParameterTraining.TrainingStateDef.UpdateAdjProcessValue:
					break;
				case ParameterTraining.TrainingStateDef.ShowExposureVal:
					TrbExpSetRatio.Value = Val_1;
					break;
				case ParameterTraining.TrainingStateDef.ShowSensorGainVal:
					TrbSensorGainSet.Value = Val_1;
					break;
				case ParameterTraining.TrainingStateDef.ShowBarcodeSwitch:
					DeviceSettingPage.BarcodeTypePage.ShowBarcodeType();
					break;
				case ParameterTraining.TrainingStateDef.ShowMainLightVal:
					DeviceSettingPage.LightingPage.Light_Pwm_1 = Val_1;
					break;
				case ParameterTraining.TrainingStateDef.OpenDefaultBarcodeType:
					DeviceSettingPage.BarcodeTypePage.BtnOpenDefaultBarcodeType_Click(null, null);
					break;
				case ParameterTraining.TrainingStateDef.TrainFinish:
					break;
				}
			});
		}

		private void BtnSetting_Click(object sender, EventArgs e)
		{
			DeviceSettingPage.ShowInTaskbar = false;
			DeviceSettingPage.Show();
		}

		private void CobImageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(7951u, (ushort)(CobImageSize.SelectedIndex + 1));
				SendCfgDataCBFuncCB(0u);
				if (GetCfgCBFuncCB(11272u) == 8)
				{
				}
				ToolCfg.MaskRect.Width = 0;
			}
		}

		private void TrbSensorLensFocusSet_MouseUp(object sender, MouseEventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int value = TrbSensorLensFocusSet.Value;
				SetCfgCBFuncCB(74495u, (byte)((uint)value & 0xFFu));
				SetCfgCBFuncCB(74751u, (byte)((value & 0xFF00) >> 8));
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TrbSensorLensFocusSet_ValueChanged(object sender, EventArgs e)
		{
			DeviceSettingPage.LenFocusVal = TrbSensorLensFocusSet.Value;
			LabFocusVal.Text = TrbSensorLensFocusSet.Value.ToString();
		}

		private void BtnSensorLensFocusDec_Click(object sender, EventArgs e)
		{
			if (TrbSensorLensFocusSet.Value > 0)
			{
				TrbSensorLensFocusSet.Value--;
				if (!ToolCfg.UpdateAdjState)
				{
					int value = TrbSensorLensFocusSet.Value;
					SetCfgCBFuncCB(74495u, (byte)((uint)value & 0xFFu));
					SetCfgCBFuncCB(74751u, (byte)((value & 0xFF00) >> 8));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void BtnSensorLensFocusAdd_Click(object sender, EventArgs e)
		{
			if (TrbSensorLensFocusSet.Value < 1008)
			{
				TrbSensorLensFocusSet.Value++;
				if (!ToolCfg.UpdateAdjState)
				{
					int value = TrbSensorLensFocusSet.Value;
					SetCfgCBFuncCB(74495u, (byte)((uint)value & 0xFFu));
					SetCfgCBFuncCB(74751u, (byte)((value & 0xFF00) >> 8));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void TrbExpSetRatio_MouseUp(object sender, MouseEventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(4607u, (ushort)TrbExpSetRatio.Value);
				SendCfgDataCBFuncCB(128u);
			}
		}

		private void TrbExpSetRatio_ValueChanged(object sender, EventArgs e)
		{
			ToolCfg.Temp_ExpVal = TrbExpSetRatio.Value;
			DeviceSettingPage.ExposureRatioVal = TrbExpSetRatio.Value;
			LabExposureVal.Text = TrbExpSetRatio.Value.ToString();
		}

		private void BtnExpSetRatioDec_Click(object sender, EventArgs e)
		{
			if (TrbExpSetRatio.Value > 0)
			{
				TrbExpSetRatio.Value--;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(4607u, (ushort)TrbExpSetRatio.Value);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private void BtnExpSetRatioAdd_Click(object sender, EventArgs e)
		{
			if (TrbExpSetRatio.Value < 255)
			{
				TrbExpSetRatio.Value++;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(4607u, (ushort)TrbExpSetRatio.Value);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private void TrbSensorGainSet_MouseUp(object sender, MouseEventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(51455u, (ushort)TrbSensorGainSet.Value);
				SendCfgDataCBFuncCB(128u);
			}
		}

		private void CMS_ImageRightKey_Opening(object sender, CancelEventArgs e)
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				TSI_DrawRectangle.Text = "画ROI(模板配置)";
				TSI_Zoom.Text = "缩放拖动图像";
				TSI_FitZoom.Text = "图像缩放至合适大小";
				TSI_SaveCurrentImage.Text = "图像另存为";
				if (ToolCfg.IsDispNetGrid)
				{
					TSI_ShowNetGrid.Text = "隐藏标线";
				}
				else
				{
					TSI_ShowNetGrid.Text = "显示标线";
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				TSI_DrawRectangle.Text = "畫ROI(範本配置)";
				TSI_Zoom.Text = "縮放拖動圖像";
				TSI_FitZoom.Text = "圖像縮放至合適大小";
				TSI_SaveCurrentImage.Text = "圖像另存為";
				if (ToolCfg.IsDispNetGrid)
				{
					TSI_ShowNetGrid.Text = "隱藏標線";
				}
				else
				{
					TSI_ShowNetGrid.Text = "顯示標線";
				}
			}
			else
			{
				TSI_DrawRectangle.Text = "Draw ROI(For Template Configuration)";
				TSI_Zoom.Text = "Zoom And Drag Image ";
				TSI_FitZoom.Text = "Zoom Image to Appropriate Size ";
				TSI_SaveCurrentImage.Text = "Save Image As";
				if (ToolCfg.IsDispNetGrid)
				{
					TSI_ShowNetGrid.Text = "Hide Grid";
				}
				else
				{
					TSI_ShowNetGrid.Text = "Show Grid";
				}
			}
		}

		public void TSI_ShowNetGrid_Click(object sender, EventArgs e)
		{
			ToolCfg.IsDispNetGrid = !ToolCfg.IsDispNetGrid;
			if (ToolCfg.IsDispNetGrid)
			{
				old_img = (Image)PanImage.BackgroundImage.Clone();
				Graphics graphics = Graphics.FromImage(PanImage.BackgroundImage);
				graphics.DrawLine(new Pen(Color.Blue), 0, old_img.Height / 2, old_img.Width - 1, old_img.Height / 2);
				graphics.DrawLine(new Pen(Color.Blue), old_img.Width / 2, 0, old_img.Width / 2, old_img.Height - 1);
				graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, old_img.Height / 4, old_img.Width - 1, old_img.Height / 4);
				graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, old_img.Height * 3 / 4, old_img.Width - 1, old_img.Height * 3 / 4);
				graphics.DrawLine(new Pen(Color.CornflowerBlue), old_img.Width / 4, 0, old_img.Width / 4, old_img.Height - 1);
				graphics.DrawLine(new Pen(Color.CornflowerBlue), old_img.Width * 3 / 4, 0, old_img.Width * 3 / 4, old_img.Height - 1);
				graphics.Dispose();
				try
				{
					TopContainer.SplitterDistance++;
					TopContainer.SplitterDistance--;
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			if (old_img != null && old_img.Width == PanImage.BackgroundImage.Width)
			{
				PanImage.BackgroundImage = old_img;
				return;
			}
			old_img = (Image)PanImage.BackgroundImage.Clone();
			Graphics graphics2 = Graphics.FromImage(PanImage.BackgroundImage);
			graphics2.DrawLine(new Pen(Color.White), 0, old_img.Height / 2, old_img.Width - 1, old_img.Height / 2);
			graphics2.DrawLine(new Pen(Color.White), old_img.Width / 2, 0, old_img.Width / 2, old_img.Height - 1);
			graphics2.DrawLine(new Pen(Color.White), 0, old_img.Height / 4, old_img.Width - 1, old_img.Height / 4);
			graphics2.DrawLine(new Pen(Color.White), 0, old_img.Height * 3 / 4, old_img.Width - 1, old_img.Height * 3 / 4);
			graphics2.DrawLine(new Pen(Color.White), old_img.Width / 4, 0, old_img.Width / 4, old_img.Height - 1);
			graphics2.DrawLine(new Pen(Color.White), old_img.Width * 3 / 4, 0, old_img.Width * 3 / 4, old_img.Height - 1);
			graphics2.Dispose();
		}

		private void TrbSensorGainSet_ValueChanged(object sender, EventArgs e)
		{
			ToolCfg.Temp_GainVal = TrbSensorGainSet.Value;
			DeviceSettingPage.SensorGainVal = TrbSensorGainSet.Value;
			LabGainVal.Text = TrbSensorGainSet.Value.ToString();
		}

		private void BtnSensorGainSetDec_Click(object sender, EventArgs e)
		{
			if (TrbSensorGainSet.Value > 0)
			{
				TrbSensorGainSet.Value--;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(51455u, (ushort)TrbSensorGainSet.Value);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private List<ToolCfg.TempRoiRectInfo> Get_Template_File(string ftpPath)
		{
			List<ToolCfg.TempRoiRectInfo> list = new List<ToolCfg.TempRoiRectInfo>();
			DirItemInfo[] array = ToolCfg.ftp.GetFtpFileInfos(ftpPath).ToArray();
			string text = "";
			DirItemInfo[] array2 = array;
			foreach (DirItemInfo dirItemInfo in array2)
			{
				string text2 = dirItemInfo.FullFileName.ToString();
				if (dirItemInfo.FileType == 0)
				{
					list.AddRange(Get_Template_File(text2));
				}
				else
				{
					if (!(Path.GetExtension(text2) == ".hjt"))
					{
						continue;
					}
					byte[] array3 = ToolCfg.ftp.DownloadFile(text2);
					template_config_t template_config_t = default(template_config_t);
					template_config_t.imgproc_config = new imgproc_config_t[20];
					if (array3.Length != Marshal.SizeOf(template_config_t))
					{
						continue;
					}
					template_config_t = (template_config_t)ToolCfg.BytesToStruct(array3, template_config_t.GetType());
					if (template_config_t.head_magic == 3405692655u && ((ulong)template_config_t.config_flag & 1uL) == 1)
					{
						rect_x0 = template_config_t.roi_config.left - ToolCfg.width_offset;
						rect_y0 = template_config_t.roi_config.top - ToolCfg.height_offset;
						rect_w = template_config_t.roi_config.width;
						rect_h = template_config_t.roi_config.height;
						try
						{
							text = text2.Substring(text2.LastIndexOf("/template/")).Replace("/template/", "").Replace(".hjt", "");
						}
						catch (Exception)
						{
							text = template_config_t.template_id.ToString();
						}
						list.Add(new ToolCfg.TempRoiRectInfo(template_config_t.template_id, text, new Rectangle(rect_x0, rect_y0, rect_w, rect_h)));
					}
				}
			}
			return list;
		}

		private void TSI_ShowAllTemplateRoi_Click(object sender, EventArgs e)
		{
			IsShowAllTemplateRoi = !IsShowAllTemplateRoi;
			TSI_ShowAllTemplateRoi.Checked = IsShowAllTemplateRoi;
			UpdateTemplateRoiList();
		}

		public void UpdateTemplateRoiList()
		{
			if (IsShowAllTemplateRoi)
			{
				ToolCfg.TemplateRoiList.Clear();
				try
				{
					string ftpPath = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template";
					ToolCfg.TemplateRoiList.AddRange(Get_Template_File(ftpPath));
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			ToolCfg.TemplateRoiList.Clear();
		}

		private void BtnSensorGainSetAdd_Click(object sender, EventArgs e)
		{
			if (TrbSensorGainSet.Value < 255)
			{
				TrbSensorGainSet.Value++;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(51455u, (ushort)TrbSensorGainSet.Value);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private void BtnResetStatic_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				string text;
				string caption;
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					text = "确定要重置统计数据？";
					caption = "提示";
				}
				else
				{
					text = "Is sure to reset data?";
					caption = "Pay Attention";
				}
				if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					SendCfgDataCBFuncCB(131072u);
					LabTrigCount.Text = "0";
					LabOKCount.Text = "0";
					LabNGCount.Text = "0";
					LabOKRatio.Text = "100.00%";
				}
			}
		}

		private void ChkRealTimeUpdate_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkRealTimeUpdate.Checked)
				{
					SetCfgCBFuncCB(78081u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(78081u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbDecodeData_DoubleClick(object sender, EventArgs e)
		{
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "是否清除输出信息?";
				caption = "注意";
			}
			else
			{
				text = "Clear output information?";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				TxbDecodeData.Clear();
			}
		}

		public void ShowImageRoiCoord(int ImageRoi_X_Start, int ImageRoi_Y_Start, int ImageRoiWidth, int ImageRoiHeight)
		{
			TxbRoiX0.Text = ImageRoi_X_Start.ToString();
			TxbRoiY0.Text = ImageRoi_Y_Start.ToString();
			TxbRoiW.Text = ImageRoiWidth.ToString();
			TxbRoiH.Text = ImageRoiHeight.ToString();
		}

		private void MouseUpEventDo(object sender, MouseEventArgs e)
		{
		}

		private void MouseDoubleClickEventDo(object sender, MouseEventArgs e)
		{
			if (IsMouseDrawImageRoi)
			{
				BtnImageRoiSet_Click(null, null);
			}
			if (IsDrawingTemplateRoi)
			{
				TSI_Zoom_Click(null, null);
			}
		}

		private void BtnImageRoiSet_Click(object sender, EventArgs e)
		{
			IsMouseDrawImageRoi = !IsMouseDrawImageRoi;
			if (BtnImageRoiSet.Text.Equals(MultLanguageText.Get_Content(MultLanguageText.TextDef.DrawRioTips)))
			{
				ImageRoiActionCBFunc(DeviceSettingForm.ImageRoiActionNum.DrawRoiStart);
				BtnImageRoiSet.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.DrawRoiOkTips);
				BtnImageRoiSet.ForeColor = Color.Red;
				BtnImageRoiCancel.Enabled = true;
				return;
			}
			ImageRoiActionCBFunc(DeviceSettingForm.ImageRoiActionNum.DrawRoiOk);
			BtnImageRoiSet.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.DrawRioTips);
			BtnImageRoiSet.ForeColor = Color.Black;
			uint num = uint.Parse(TxbRoiX0.Text);
			uint num2 = uint.Parse(TxbRoiW.Text);
			uint num3 = uint.Parse(TxbRoiY0.Text);
			uint num4 = uint.Parse(TxbRoiH.Text);
			if (num2 >= 64 && num4 >= 32)
			{
				SetCfgCBFuncCB(54527u, num / 8);
				SetCfgCBFuncCB(55039u, num2 / 8);
				SetCfgCBFuncCB(54783u, num3 / 8);
				SetCfgCBFuncCB(55295u, num4 / 8);
				SetCfgCBFuncCB(55297u, 1u);
				SendCfgDataCBFuncCB(256u);
				ToolCfg.MaskRect.Width = 0;
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("当前选择的区域太小了(w<64 或 h<32)，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				MessageBox.Show("The current selection is too small(w<64 or h<32),Please Reselect", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void BtnImageRoiCancel_Click(object sender, EventArgs e)
		{
			ImageRoiActionCBFunc(DeviceSettingForm.ImageRoiActionNum.DrawRoiCancel);
			BtnImageRoiSet.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.DrawRioTips);
			BtnImageRoiSet.ForeColor = Color.Black;
			BtnImageRoiCancel.Enabled = false;
			SetCfgCBFuncCB(55297u, 0u);
			SendCfgDataCBFuncCB(256u);
			if (GetCfgCBFuncCB(3u) != 2)
			{
				ToolCfg.SendReadBackCMD();
			}
		}

		public void Get_SplitDistance_h_v(ref int h, ref int v)
		{
			v = TopContainer.SplitterDistance;
			h = MainContainer.SplitterDistance;
		}

		public void Set_SplitDistance_h_v(int h, int v)
		{
			TopContainer.SplitterDistance = v;
			MainContainer.SplitterDistance = h;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Heroje_Debug_Tool.SubForm.ReadingForm));
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.MainContainer = new System.Windows.Forms.SplitContainer();
			this.TopContainer = new System.Windows.Forms.SplitContainer();
			this.PanAutoFocus = new System.Windows.Forms.Panel();
			this.BtnSelectFocusROI = new System.Windows.Forms.Button();
			this.BtnTrigAutoFocus = new System.Windows.Forms.Button();
			this.PanArrow_2 = new System.Windows.Forms.Panel();
			this.PgbAutoAdjProcess = new System.Windows.Forms.ProgressBar();
			this.GrbImageROI = new System.Windows.Forms.GroupBox();
			this.BtnImageRoiSet = new System.Windows.Forms.Button();
			this.BtnImageRoiCancel = new System.Windows.Forms.Button();
			this.TxbRoiH = new System.Windows.Forms.TextBox();
			this.TxbRoiW = new System.Windows.Forms.TextBox();
			this.TxbRoiY0 = new System.Windows.Forms.TextBox();
			this.TxbRoiX0 = new System.Windows.Forms.TextBox();
			this.LabRoiH = new System.Windows.Forms.Label();
			this.LabRoiW = new System.Windows.Forms.Label();
			this.LabRoiY0 = new System.Windows.Forms.Label();
			this.LabRoiX0 = new System.Windows.Forms.Label();
			this.BtnPerformOneTrig = new System.Windows.Forms.Button();
			this.BtnSetting = new System.Windows.Forms.Button();
			this.LabStateADP = new System.Windows.Forms.Label();
			this.BtnEnterADP = new System.Windows.Forms.Button();
			this.BtnReadContinuous = new System.Windows.Forms.Button();
			this.PanArrow_1 = new System.Windows.Forms.Panel();
			this.PanArrow_4 = new System.Windows.Forms.Panel();
			this.PanArrow_3 = new System.Windows.Forms.Panel();
			this.PanManualFocus = new System.Windows.Forms.Panel();
			this.BtnSensorLensFocusDec = new System.Windows.Forms.Button();
			this.LabFocusVal = new System.Windows.Forms.Label();
			this.BtnSensorLensFocusAdd = new System.Windows.Forms.Button();
			this.LabFocus = new System.Windows.Forms.Label();
			this.TrbSensorLensFocusSet = new System.Windows.Forms.TrackBar();
			this.LabGainVal = new System.Windows.Forms.Label();
			this.LabExposureVal = new System.Windows.Forms.Label();
			this.LabSensorGain = new System.Windows.Forms.Label();
			this.LabExposure = new System.Windows.Forms.Label();
			this.PanImage = new HJ_Controls_Lib.PanelEx();
			this.CMS_ImageRightKey = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.TSI_DrawRectangle = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_Zoom = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_FitZoom = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_SaveCurrentImage = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_ShowNetGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_ShowAllTemplateRoi = new System.Windows.Forms.ToolStripMenuItem();
			this.LabFramePerSec = new System.Windows.Forms.Label();
			this.LabFrameRate = new System.Windows.Forms.Label();
			this.LabFrameCountDisp = new System.Windows.Forms.Label();
			this.LabImageCount = new System.Windows.Forms.Label();
			this.LabImageCountDips = new System.Windows.Forms.Label();
			this.CobImageSize = new System.Windows.Forms.ComboBox();
			this.BtnSensorGainSetDec = new System.Windows.Forms.Button();
			this.BtnExpSetRatioDec = new System.Windows.Forms.Button();
			this.BtnSensorGainSetAdd = new System.Windows.Forms.Button();
			this.BtnExpSetRatioAdd = new System.Windows.Forms.Button();
			this.TrbSensorGainSet = new System.Windows.Forms.TrackBar();
			this.TrbExpSetRatio = new System.Windows.Forms.TrackBar();
			this.LabImageSize = new System.Windows.Forms.Label();
			this.BottomContainer = new System.Windows.Forms.SplitContainer();
			this.TabSmallForm = new System.Windows.Forms.TabControl();
			this.Tp_DecodeTime = new System.Windows.Forms.TabPage();
			this.PanDecodeTime = new System.Windows.Forms.Panel();
			this.LabCurrentDecodeTime = new System.Windows.Forms.Label();
			this.LabAveDecodeTime = new System.Windows.Forms.Label();
			this.LabAveDecodeTimeDisp = new System.Windows.Forms.Label();
			this.LabNowDecodeTime = new System.Windows.Forms.Label();
			this.ChartDecodeTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.Tp_DecStatistics = new System.Windows.Forms.TabPage();
			this.PanDecStatistics = new System.Windows.Forms.Panel();
			this.LabTrigCount = new System.Windows.Forms.Label();
			this.ChkRealTimeUpdate = new System.Windows.Forms.CheckBox();
			this.LabNGCount = new System.Windows.Forms.Label();
			this.LabOKCount = new System.Windows.Forms.Label();
			this.BtnResetStatic = new System.Windows.Forms.Button();
			this.LabTotalDisp = new System.Windows.Forms.Label();
			this.LabDispNG = new System.Windows.Forms.Label();
			this.LabOKDisp = new System.Windows.Forms.Label();
			this.LabOKRatio = new System.Windows.Forms.Label();
			this.GrbDecodeData = new System.Windows.Forms.GroupBox();
			this.LabBarcodeLen = new System.Windows.Forms.Label();
			this.LabBarcodeLenDisp = new System.Windows.Forms.Label();
			this.LabBarcodeType = new System.Windows.Forms.Label();
			this.TxbDecodeData = new System.Windows.Forms.TextBox();
			this.LabBarcodeTypeDisp = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)this.MainContainer).BeginInit();
			this.MainContainer.Panel1.SuspendLayout();
			this.MainContainer.Panel2.SuspendLayout();
			this.MainContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TopContainer).BeginInit();
			this.TopContainer.Panel1.SuspendLayout();
			this.TopContainer.Panel2.SuspendLayout();
			this.TopContainer.SuspendLayout();
			this.PanAutoFocus.SuspendLayout();
			this.GrbImageROI.SuspendLayout();
			this.PanManualFocus.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbSensorLensFocusSet).BeginInit();
			this.CMS_ImageRightKey.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbSensorGainSet).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.TrbExpSetRatio).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.BottomContainer).BeginInit();
			this.BottomContainer.Panel1.SuspendLayout();
			this.BottomContainer.Panel2.SuspendLayout();
			this.BottomContainer.SuspendLayout();
			this.TabSmallForm.SuspendLayout();
			this.Tp_DecodeTime.SuspendLayout();
			this.PanDecodeTime.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ChartDecodeTime).BeginInit();
			this.Tp_DecStatistics.SuspendLayout();
			this.PanDecStatistics.SuspendLayout();
			this.GrbDecodeData.SuspendLayout();
			base.SuspendLayout();
			this.MainContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainContainer.Location = new System.Drawing.Point(0, 0);
			this.MainContainer.Name = "MainContainer";
			this.MainContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.MainContainer.Panel1.Controls.Add(this.TopContainer);
			this.MainContainer.Panel2.Controls.Add(this.BottomContainer);
			this.MainContainer.Panel2MinSize = 140;
			this.MainContainer.Size = new System.Drawing.Size(834, 689);
			this.MainContainer.SplitterDistance = 537;
			this.MainContainer.SplitterWidth = 5;
			this.MainContainer.TabIndex = 0;
			this.TopContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TopContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TopContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.TopContainer.Location = new System.Drawing.Point(0, 0);
			this.TopContainer.Name = "TopContainer";
			this.TopContainer.Panel1.AutoScroll = true;
			this.TopContainer.Panel1.Controls.Add(this.PanAutoFocus);
			this.TopContainer.Panel1.Controls.Add(this.PgbAutoAdjProcess);
			this.TopContainer.Panel1.Controls.Add(this.GrbImageROI);
			this.TopContainer.Panel1.Controls.Add(this.BtnPerformOneTrig);
			this.TopContainer.Panel1.Controls.Add(this.BtnSetting);
			this.TopContainer.Panel1.Controls.Add(this.LabStateADP);
			this.TopContainer.Panel1.Controls.Add(this.BtnEnterADP);
			this.TopContainer.Panel1.Controls.Add(this.BtnReadContinuous);
			this.TopContainer.Panel1.Controls.Add(this.PanArrow_1);
			this.TopContainer.Panel1.Controls.Add(this.PanArrow_4);
			this.TopContainer.Panel1.Controls.Add(this.PanArrow_3);
			this.TopContainer.Panel2.Controls.Add(this.PanManualFocus);
			this.TopContainer.Panel2.Controls.Add(this.LabGainVal);
			this.TopContainer.Panel2.Controls.Add(this.LabExposureVal);
			this.TopContainer.Panel2.Controls.Add(this.LabSensorGain);
			this.TopContainer.Panel2.Controls.Add(this.LabExposure);
			this.TopContainer.Panel2.Controls.Add(this.PanImage);
			this.TopContainer.Panel2.Controls.Add(this.LabFramePerSec);
			this.TopContainer.Panel2.Controls.Add(this.LabFrameRate);
			this.TopContainer.Panel2.Controls.Add(this.LabFrameCountDisp);
			this.TopContainer.Panel2.Controls.Add(this.LabImageCount);
			this.TopContainer.Panel2.Controls.Add(this.LabImageCountDips);
			this.TopContainer.Panel2.Controls.Add(this.CobImageSize);
			this.TopContainer.Panel2.Controls.Add(this.BtnSensorGainSetDec);
			this.TopContainer.Panel2.Controls.Add(this.BtnExpSetRatioDec);
			this.TopContainer.Panel2.Controls.Add(this.BtnSensorGainSetAdd);
			this.TopContainer.Panel2.Controls.Add(this.BtnExpSetRatioAdd);
			this.TopContainer.Panel2.Controls.Add(this.TrbSensorGainSet);
			this.TopContainer.Panel2.Controls.Add(this.TrbExpSetRatio);
			this.TopContainer.Panel2.Controls.Add(this.LabImageSize);
			this.TopContainer.Size = new System.Drawing.Size(834, 537);
			this.TopContainer.SplitterDistance = 235;
			this.TopContainer.SplitterWidth = 5;
			this.TopContainer.TabIndex = 0;
			this.PanAutoFocus.Controls.Add(this.BtnSelectFocusROI);
			this.PanAutoFocus.Controls.Add(this.BtnTrigAutoFocus);
			this.PanAutoFocus.Controls.Add(this.PanArrow_2);
			this.PanAutoFocus.Location = new System.Drawing.Point(18, 75);
			this.PanAutoFocus.Name = "PanAutoFocus";
			this.PanAutoFocus.Size = new System.Drawing.Size(102, 97);
			this.PanAutoFocus.TabIndex = 48;
			this.BtnSelectFocusROI.Location = new System.Drawing.Point(0, 1);
			this.BtnSelectFocusROI.Name = "BtnSelectFocusROI";
			this.BtnSelectFocusROI.Size = new System.Drawing.Size(101, 35);
			this.BtnSelectFocusROI.TabIndex = 46;
			this.BtnSelectFocusROI.Text = "对焦ROI";
			this.BtnSelectFocusROI.UseVisualStyleBackColor = true;
			this.BtnSelectFocusROI.Click += new System.EventHandler(BtnSelectFocusROI_Click);
			this.BtnTrigAutoFocus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.BtnTrigAutoFocus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.BtnTrigAutoFocus.Location = new System.Drawing.Point(0, 62);
			this.BtnTrigAutoFocus.Name = "BtnTrigAutoFocus";
			this.BtnTrigAutoFocus.Size = new System.Drawing.Size(101, 35);
			this.BtnTrigAutoFocus.TabIndex = 25;
			this.BtnTrigAutoFocus.Text = "自动对焦";
			this.BtnTrigAutoFocus.UseVisualStyleBackColor = true;
			this.BtnTrigAutoFocus.Click += new System.EventHandler(BtnTrigAutoFocus_Click);
//			this.PanArrow_2.BackgroundImage = Heroje_Debug_Tool.Properties.Resources.arrow_short;
			//this.PanArrow_2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.PanArrow_2.Location = new System.Drawing.Point(36, 28);
			this.PanArrow_2.Name = "PanArrow_2";
			this.PanArrow_2.Size = new System.Drawing.Size(24, 35);
			this.PanArrow_2.TabIndex = 29;
			this.PgbAutoAdjProcess.Location = new System.Drawing.Point(18, 233);
			this.PgbAutoAdjProcess.Name = "PgbAutoAdjProcess";
			this.PgbAutoAdjProcess.Size = new System.Drawing.Size(118, 5);
			this.PgbAutoAdjProcess.Step = 1;
			this.PgbAutoAdjProcess.TabIndex = 47;
			this.PgbAutoAdjProcess.Visible = false;
			this.GrbImageROI.BackColor = System.Drawing.Color.Transparent;
			this.GrbImageROI.Controls.Add(this.BtnImageRoiSet);
			this.GrbImageROI.Controls.Add(this.BtnImageRoiCancel);
			this.GrbImageROI.Controls.Add(this.TxbRoiH);
			this.GrbImageROI.Controls.Add(this.TxbRoiW);
			this.GrbImageROI.Controls.Add(this.TxbRoiY0);
			this.GrbImageROI.Controls.Add(this.TxbRoiX0);
			this.GrbImageROI.Controls.Add(this.LabRoiH);
			this.GrbImageROI.Controls.Add(this.LabRoiW);
			this.GrbImageROI.Controls.Add(this.LabRoiY0);
			this.GrbImageROI.Controls.Add(this.LabRoiX0);
			this.GrbImageROI.Location = new System.Drawing.Point(10, 301);
			this.GrbImageROI.Name = "GrbImageROI";
			this.GrbImageROI.Size = new System.Drawing.Size(217, 120);
			this.GrbImageROI.TabIndex = 45;
			this.GrbImageROI.TabStop = false;
			this.GrbImageROI.Text = "图像ROI设置";
			this.BtnImageRoiSet.Location = new System.Drawing.Point(25, 75);
			this.BtnImageRoiSet.Name = "BtnImageRoiSet";
			this.BtnImageRoiSet.Size = new System.Drawing.Size(85, 30);
			this.BtnImageRoiSet.TabIndex = 31;
			this.BtnImageRoiSet.Text = "框选ROI";
			this.BtnImageRoiSet.UseVisualStyleBackColor = true;
			this.BtnImageRoiSet.Click += new System.EventHandler(BtnImageRoiSet_Click);
			this.BtnImageRoiCancel.Enabled = false;
			this.BtnImageRoiCancel.Location = new System.Drawing.Point(117, 75);
			this.BtnImageRoiCancel.Name = "BtnImageRoiCancel";
			this.BtnImageRoiCancel.Size = new System.Drawing.Size(85, 30);
			this.BtnImageRoiCancel.TabIndex = 36;
			this.BtnImageRoiCancel.Text = "关闭ROI";
			this.BtnImageRoiCancel.UseVisualStyleBackColor = true;
			this.BtnImageRoiCancel.Click += new System.EventHandler(BtnImageRoiCancel_Click);
			this.TxbRoiH.Location = new System.Drawing.Point(162, 46);
			this.TxbRoiH.Name = "TxbRoiH";
			this.TxbRoiH.ReadOnly = true;
			this.TxbRoiH.Size = new System.Drawing.Size(40, 23);
			this.TxbRoiH.TabIndex = 35;
			this.TxbRoiH.Text = "800";
			this.TxbRoiW.Location = new System.Drawing.Point(162, 20);
			this.TxbRoiW.Name = "TxbRoiW";
			this.TxbRoiW.ReadOnly = true;
			this.TxbRoiW.Size = new System.Drawing.Size(40, 23);
			this.TxbRoiW.TabIndex = 34;
			this.TxbRoiW.Text = "1280";
			this.TxbRoiY0.Location = new System.Drawing.Point(45, 46);
			this.TxbRoiY0.Name = "TxbRoiY0";
			this.TxbRoiY0.ReadOnly = true;
			this.TxbRoiY0.Size = new System.Drawing.Size(40, 23);
			this.TxbRoiY0.TabIndex = 33;
			this.TxbRoiY0.Text = "0";
			this.TxbRoiX0.Location = new System.Drawing.Point(45, 20);
			this.TxbRoiX0.Name = "TxbRoiX0";
			this.TxbRoiX0.ReadOnly = true;
			this.TxbRoiX0.Size = new System.Drawing.Size(40, 23);
			this.TxbRoiX0.TabIndex = 32;
			this.TxbRoiX0.Text = "0";
			this.LabRoiH.AutoSize = true;
			this.LabRoiH.Location = new System.Drawing.Point(107, 50);
			this.LabRoiH.Name = "LabRoiH";
			this.LabRoiH.Size = new System.Drawing.Size(56, 14);
			this.LabRoiH.TabIndex = 30;
			this.LabRoiH.Text = "Height:";
			this.LabRoiW.AutoSize = true;
			this.LabRoiW.Location = new System.Drawing.Point(114, 24);
			this.LabRoiW.Name = "LabRoiW";
			this.LabRoiW.Size = new System.Drawing.Size(49, 14);
			this.LabRoiW.TabIndex = 29;
			this.LabRoiW.Text = "Width:";
			this.LabRoiY0.AutoSize = true;
			this.LabRoiY0.Location = new System.Drawing.Point(20, 50);
			this.LabRoiY0.Name = "LabRoiY0";
			this.LabRoiY0.Size = new System.Drawing.Size(28, 14);
			this.LabRoiY0.TabIndex = 28;
			this.LabRoiY0.Text = "Y0:";
			this.LabRoiX0.AutoSize = true;
			this.LabRoiX0.Location = new System.Drawing.Point(20, 24);
			this.LabRoiX0.Name = "LabRoiX0";
			this.LabRoiX0.Size = new System.Drawing.Size(28, 14);
			this.LabRoiX0.TabIndex = 27;
			this.LabRoiX0.Text = "X0:";
			this.BtnPerformOneTrig.Location = new System.Drawing.Point(146, 14);
			this.BtnPerformOneTrig.Name = "BtnPerformOneTrig";
			this.BtnPerformOneTrig.Size = new System.Drawing.Size(75, 35);
			this.BtnPerformOneTrig.TabIndex = 33;
			this.BtnPerformOneTrig.Text = "单次触发";
			this.BtnPerformOneTrig.UseVisualStyleBackColor = true;
			this.BtnPerformOneTrig.Click += new System.EventHandler(BtnPerformOneTrig_Click);
//			this.BtnSetting.Image = Heroje_Debug_Tool.Properties.Resources.setting_26x26;
			this.BtnSetting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.BtnSetting.Location = new System.Drawing.Point(121, 260);
			this.BtnSetting.Name = "BtnSetting";
			this.BtnSetting.Size = new System.Drawing.Size(100, 35);
			this.BtnSetting.TabIndex = 32;
			this.BtnSetting.Text = "基本设置";
			this.BtnSetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.BtnSetting.UseVisualStyleBackColor = true;
			this.BtnSetting.Click += new System.EventHandler(BtnSetting_Click);
			this.LabStateADP.AutoSize = true;
			this.LabStateADP.Location = new System.Drawing.Point(17, 241);
			this.LabStateADP.Name = "LabStateADP";
			this.LabStateADP.Size = new System.Drawing.Size(70, 14);
			this.LabStateADP.TabIndex = 31;
			this.LabStateADP.Text = "调参中...";
			this.LabStateADP.Visible = false;
			this.BtnEnterADP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.BtnEnterADP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.BtnEnterADP.Location = new System.Drawing.Point(18, 197);
			this.BtnEnterADP.Name = "BtnEnterADP";
			this.BtnEnterADP.Size = new System.Drawing.Size(118, 35);
			this.BtnEnterADP.TabIndex = 26;
			this.BtnEnterADP.Text = "AI调参";
			this.BtnEnterADP.UseVisualStyleBackColor = true;
			this.BtnEnterADP.Click += new System.EventHandler(BtnEnterADP_Click);
			this.BtnReadContinuous.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.BtnReadContinuous.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.BtnReadContinuous.Location = new System.Drawing.Point(18, 14);
			this.BtnReadContinuous.Name = "BtnReadContinuous";
			this.BtnReadContinuous.Size = new System.Drawing.Size(118, 35);
			this.BtnReadContinuous.TabIndex = 24;
			this.BtnReadContinuous.Text = "实时预览";
			this.BtnReadContinuous.UseVisualStyleBackColor = true;
			this.BtnReadContinuous.Click += new System.EventHandler(BtnReadContinuous_Click);
//			this.PanArrow_1.BackgroundImage = Heroje_Debug_Tool.Properties.Resources.arrow_long;
			this.PanArrow_1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.PanArrow_1.Location = new System.Drawing.Point(115, 46);
			this.PanArrow_1.Name = "PanArrow_1";
			this.PanArrow_1.Size = new System.Drawing.Size(21, 156);
			this.PanArrow_1.TabIndex = 28;
//			this.PanArrow_4.BackgroundImage = Heroje_Debug_Tool.Properties.Resources.arrow_short;
			this.PanArrow_4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.PanArrow_4.Location = new System.Drawing.Point(54, 41);
			this.PanArrow_4.Name = "PanArrow_4";
			this.PanArrow_4.Size = new System.Drawing.Size(24, 35);
			this.PanArrow_4.TabIndex = 30;
//			this.PanArrow_3.BackgroundImage = Heroje_Debug_Tool.Properties.Resources.arrow_short;
			this.PanArrow_3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.PanArrow_3.Location = new System.Drawing.Point(54, 162);
			this.PanArrow_3.Name = "PanArrow_3";
			this.PanArrow_3.Size = new System.Drawing.Size(24, 35);
			this.PanArrow_3.TabIndex = 30;
			this.PanManualFocus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.PanManualFocus.Controls.Add(this.BtnSensorLensFocusDec);
			this.PanManualFocus.Controls.Add(this.LabFocusVal);
			this.PanManualFocus.Controls.Add(this.BtnSensorLensFocusAdd);
			this.PanManualFocus.Controls.Add(this.LabFocus);
			this.PanManualFocus.Controls.Add(this.TrbSensorLensFocusSet);
			this.PanManualFocus.Location = new System.Drawing.Point(3, 500);
			this.PanManualFocus.Name = "PanManualFocus";
			this.PanManualFocus.Size = new System.Drawing.Size(541, 35);
			this.PanManualFocus.TabIndex = 48;
			this.BtnSensorLensFocusDec.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.BtnSensorLensFocusDec.Location = new System.Drawing.Point(41, 2);
			this.BtnSensorLensFocusDec.Name = "BtnSensorLensFocusDec";
			this.BtnSensorLensFocusDec.Size = new System.Drawing.Size(30, 30);
			this.BtnSensorLensFocusDec.TabIndex = 24;
			this.BtnSensorLensFocusDec.Text = "-";
			this.BtnSensorLensFocusDec.UseVisualStyleBackColor = true;
			this.BtnSensorLensFocusDec.Click += new System.EventHandler(BtnSensorLensFocusDec_Click);
			this.LabFocusVal.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.LabFocusVal.AutoSize = true;
			this.LabFocusVal.Location = new System.Drawing.Point(519, 13);
			this.LabFocusVal.Name = "LabFocusVal";
			this.LabFocusVal.Size = new System.Drawing.Size(14, 14);
			this.LabFocusVal.TabIndex = 38;
			this.LabFocusVal.Text = "0";
			this.BtnSensorLensFocusAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.BtnSensorLensFocusAdd.Font = new System.Drawing.Font("宋体", 9f);
			this.BtnSensorLensFocusAdd.Location = new System.Drawing.Point(484, 2);
			this.BtnSensorLensFocusAdd.Name = "BtnSensorLensFocusAdd";
			this.BtnSensorLensFocusAdd.Size = new System.Drawing.Size(30, 30);
			this.BtnSensorLensFocusAdd.TabIndex = 25;
			this.BtnSensorLensFocusAdd.Text = "+";
			this.BtnSensorLensFocusAdd.UseVisualStyleBackColor = true;
			this.BtnSensorLensFocusAdd.Click += new System.EventHandler(BtnSensorLensFocusAdd_Click);
			this.LabFocus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabFocus.AutoSize = true;
			this.LabFocus.Location = new System.Drawing.Point(3, 10);
			this.LabFocus.Name = "LabFocus";
			this.LabFocus.Size = new System.Drawing.Size(35, 14);
			this.LabFocus.TabIndex = 35;
			this.LabFocus.Text = "焦距";
			this.TrbSensorLensFocusSet.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TrbSensorLensFocusSet.AutoSize = false;
			this.TrbSensorLensFocusSet.Location = new System.Drawing.Point(69, 4);
			this.TrbSensorLensFocusSet.Maximum = 1008;
			this.TrbSensorLensFocusSet.Name = "TrbSensorLensFocusSet";
			this.TrbSensorLensFocusSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TrbSensorLensFocusSet.RightToLeftLayout = true;
			this.TrbSensorLensFocusSet.Size = new System.Drawing.Size(418, 28);
			this.TrbSensorLensFocusSet.TabIndex = 26;
			this.TrbSensorLensFocusSet.TickFrequency = 20;
			this.TrbSensorLensFocusSet.ValueChanged += new System.EventHandler(TrbSensorLensFocusSet_ValueChanged);
			this.TrbSensorLensFocusSet.MouseUp += new System.Windows.Forms.MouseEventHandler(TrbSensorLensFocusSet_MouseUp);
			this.LabGainVal.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.LabGainVal.AutoSize = true;
			this.LabGainVal.Location = new System.Drawing.Point(557, 478);
			this.LabGainVal.Name = "LabGainVal";
			this.LabGainVal.Size = new System.Drawing.Size(14, 14);
			this.LabGainVal.TabIndex = 40;
			this.LabGainVal.Text = "0";
			this.LabExposureVal.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.LabExposureVal.AutoSize = true;
			this.LabExposureVal.Location = new System.Drawing.Point(521, 479);
			this.LabExposureVal.Name = "LabExposureVal";
			this.LabExposureVal.Size = new System.Drawing.Size(14, 14);
			this.LabExposureVal.TabIndex = 39;
			this.LabExposureVal.Text = "0";
			this.LabSensorGain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.LabSensorGain.AutoSize = true;
			this.LabSensorGain.Location = new System.Drawing.Point(548, 7);
			this.LabSensorGain.Name = "LabSensorGain";
			this.LabSensorGain.Size = new System.Drawing.Size(35, 14);
			this.LabSensorGain.TabIndex = 37;
			this.LabSensorGain.Text = "增益";
			this.LabExposure.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.LabExposure.AutoSize = true;
			this.LabExposure.Location = new System.Drawing.Point(514, 7);
			this.LabExposure.Name = "LabExposure";
			this.LabExposure.Size = new System.Drawing.Size(35, 14);
			this.LabExposure.TabIndex = 36;
			this.LabExposure.Text = "曝光";
			this.PanImage.ActionNum = 0;
			this.PanImage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.PanImage.ArcStartAngle = 45;
			this.PanImage.ArcSweepAngle = 90;
			this.PanImage.BackColor = System.Drawing.Color.Silver;
			this.PanImage.BackgroundImage = (System.Drawing.Image)resources.GetObject("PanImage.BackgroundImage");
			this.PanImage.ContextMenuStrip = this.CMS_ImageRightKey;
			this.PanImage.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.PanImage.ImageZoomRate = 1;
			this.PanImage.IsDefaultMode = true;
			this.PanImage.Location = new System.Drawing.Point(3, 24);
			this.PanImage.Name = "PanImage";
			this.PanImage.PanelBackColor = System.Drawing.Color.FromArgb(189, 189, 189);
			this.PanImage.PanelForeColor = System.Drawing.Color.FromArgb(189, 189, 189);
			this.PanImage.Size = new System.Drawing.Size(512, 475);
			this.PanImage.TabIndex = 34;
			this.CMS_ImageRightKey.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.TSI_DrawRectangle, this.TSI_Zoom, this.TSI_FitZoom, this.TSI_SaveCurrentImage, this.TSI_ShowNetGrid, this.TSI_ShowAllTemplateRoi });
			this.CMS_ImageRightKey.Name = "CMS_ImageRightKey";
			this.CMS_ImageRightKey.Size = new System.Drawing.Size(207, 136);
			this.CMS_ImageRightKey.Opening += new System.ComponentModel.CancelEventHandler(CMS_ImageRightKey_Opening);
			this.TSI_DrawRectangle.Name = "TSI_DrawRectangle";
			this.TSI_DrawRectangle.Size = new System.Drawing.Size(206, 22);
			this.TSI_DrawRectangle.Text = "画ROI（模板配置）";
			this.TSI_DrawRectangle.Click += new System.EventHandler(TSI_DrawRectangle_Click);
			this.TSI_Zoom.Checked = true;
			this.TSI_Zoom.CheckState = System.Windows.Forms.CheckState.Checked;
			this.TSI_Zoom.Name = "TSI_Zoom";
			this.TSI_Zoom.Size = new System.Drawing.Size(206, 22);
			this.TSI_Zoom.Text = "缩放拖动图像";
			this.TSI_Zoom.Click += new System.EventHandler(TSI_Zoom_Click);
			this.TSI_FitZoom.Name = "TSI_FitZoom";
			this.TSI_FitZoom.Size = new System.Drawing.Size(206, 22);
			this.TSI_FitZoom.Text = "图像缩放至合适大小";
			this.TSI_FitZoom.Click += new System.EventHandler(TSI_FitZoom_Click);
			this.TSI_SaveCurrentImage.Name = "TSI_SaveCurrentImage";
			this.TSI_SaveCurrentImage.Size = new System.Drawing.Size(206, 22);
			this.TSI_SaveCurrentImage.Text = "图像另存为";
			this.TSI_SaveCurrentImage.Click += new System.EventHandler(TSI_SaveCurrentImage_Click);
			this.TSI_ShowNetGrid.Name = "TSI_ShowNetGrid";
			this.TSI_ShowNetGrid.Size = new System.Drawing.Size(206, 22);
			this.TSI_ShowNetGrid.Text = "显示标线";
			this.TSI_ShowNetGrid.Click += new System.EventHandler(TSI_ShowNetGrid_Click);
			this.TSI_ShowAllTemplateRoi.Name = "TSI_ShowAllTemplateRoi";
			this.TSI_ShowAllTemplateRoi.Size = new System.Drawing.Size(206, 22);
			this.TSI_ShowAllTemplateRoi.Text = "显示所有启用的模板ROI";
			this.TSI_ShowAllTemplateRoi.Click += new System.EventHandler(TSI_ShowAllTemplateRoi_Click);
			this.LabFramePerSec.AutoSize = true;
			this.LabFramePerSec.Location = new System.Drawing.Point(370, 7);
			this.LabFramePerSec.Name = "LabFramePerSec";
			this.LabFramePerSec.Size = new System.Drawing.Size(28, 14);
			this.LabFramePerSec.TabIndex = 33;
			this.LabFramePerSec.Text = "fps";
			this.LabFrameRate.AutoSize = true;
			this.LabFrameRate.Location = new System.Drawing.Point(342, 7);
			this.LabFrameRate.Name = "LabFrameRate";
			this.LabFrameRate.Size = new System.Drawing.Size(28, 14);
			this.LabFrameRate.TabIndex = 32;
			this.LabFrameRate.Text = "000";
			this.LabFrameRate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.LabFrameCountDisp.AutoSize = true;
			this.LabFrameCountDisp.Location = new System.Drawing.Point(306, 7);
			this.LabFrameCountDisp.Name = "LabFrameCountDisp";
			this.LabFrameCountDisp.Size = new System.Drawing.Size(42, 14);
			this.LabFrameCountDisp.TabIndex = 31;
			this.LabFrameCountDisp.Text = "帧率:";
			this.LabImageCount.AutoSize = true;
			this.LabImageCount.Location = new System.Drawing.Point(260, 7);
			this.LabImageCount.Name = "LabImageCount";
			this.LabImageCount.Size = new System.Drawing.Size(14, 14);
			this.LabImageCount.TabIndex = 30;
			this.LabImageCount.Text = "0";
			this.LabImageCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.LabImageCountDips.AutoSize = true;
			this.LabImageCountDips.Location = new System.Drawing.Point(195, 7);
			this.LabImageCountDips.Name = "LabImageCountDips";
			this.LabImageCountDips.Size = new System.Drawing.Size(70, 14);
			this.LabImageCountDips.TabIndex = 29;
			this.LabImageCountDips.Text = "图像计数:";
			this.CobImageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobImageSize.FormattingEnabled = true;
			this.CobImageSize.Items.AddRange(new object[3] { "原图", "正常", "快速" });
			this.CobImageSize.Location = new System.Drawing.Point(112, 2);
			this.CobImageSize.Name = "CobImageSize";
			this.CobImageSize.Size = new System.Drawing.Size(67, 22);
			this.CobImageSize.TabIndex = 28;
			this.CobImageSize.SelectedIndexChanged += new System.EventHandler(CobImageSize_SelectedIndexChanged);
			this.BtnSensorGainSetDec.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.BtnSensorGainSetDec.Location = new System.Drawing.Point(550, 442);
			this.BtnSensorGainSetDec.Name = "BtnSensorGainSetDec";
			this.BtnSensorGainSetDec.Size = new System.Drawing.Size(30, 30);
			this.BtnSensorGainSetDec.TabIndex = 23;
			this.BtnSensorGainSetDec.Text = "-";
			this.BtnSensorGainSetDec.UseVisualStyleBackColor = true;
			this.BtnSensorGainSetDec.Click += new System.EventHandler(BtnSensorGainSetDec_Click);
			this.BtnExpSetRatioDec.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.BtnExpSetRatioDec.Location = new System.Drawing.Point(516, 442);
			this.BtnExpSetRatioDec.Name = "BtnExpSetRatioDec";
			this.BtnExpSetRatioDec.Size = new System.Drawing.Size(30, 30);
			this.BtnExpSetRatioDec.TabIndex = 22;
			this.BtnExpSetRatioDec.Text = "-";
			this.BtnExpSetRatioDec.UseVisualStyleBackColor = true;
			this.BtnExpSetRatioDec.Click += new System.EventHandler(BtnExpSetRatioDec_Click);
			this.BtnSensorGainSetAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.BtnSensorGainSetAdd.Location = new System.Drawing.Point(550, 24);
			this.BtnSensorGainSetAdd.Name = "BtnSensorGainSetAdd";
			this.BtnSensorGainSetAdd.Size = new System.Drawing.Size(30, 30);
			this.BtnSensorGainSetAdd.TabIndex = 21;
			this.BtnSensorGainSetAdd.Text = "+";
			this.BtnSensorGainSetAdd.UseVisualStyleBackColor = true;
			this.BtnSensorGainSetAdd.Click += new System.EventHandler(BtnSensorGainSetAdd_Click);
			this.BtnExpSetRatioAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.BtnExpSetRatioAdd.Location = new System.Drawing.Point(516, 24);
			this.BtnExpSetRatioAdd.Name = "BtnExpSetRatioAdd";
			this.BtnExpSetRatioAdd.Size = new System.Drawing.Size(30, 30);
			this.BtnExpSetRatioAdd.TabIndex = 20;
			this.BtnExpSetRatioAdd.Text = "+";
			this.BtnExpSetRatioAdd.UseVisualStyleBackColor = true;
			this.BtnExpSetRatioAdd.Click += new System.EventHandler(BtnExpSetRatioAdd_Click);
			this.TrbSensorGainSet.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.TrbSensorGainSet.AutoSize = false;
			this.TrbSensorGainSet.Location = new System.Drawing.Point(557, 55);
			this.TrbSensorGainSet.Maximum = 255;
			this.TrbSensorGainSet.Name = "TrbSensorGainSet";
			this.TrbSensorGainSet.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.TrbSensorGainSet.Size = new System.Drawing.Size(28, 387);
			this.TrbSensorGainSet.TabIndex = 19;
			this.TrbSensorGainSet.TickFrequency = 20;
			this.TrbSensorGainSet.ValueChanged += new System.EventHandler(TrbSensorGainSet_ValueChanged);
			this.TrbSensorGainSet.MouseUp += new System.Windows.Forms.MouseEventHandler(TrbSensorGainSet_MouseUp);
			this.TrbExpSetRatio.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.TrbExpSetRatio.AutoSize = false;
			this.TrbExpSetRatio.Location = new System.Drawing.Point(516, 55);
			this.TrbExpSetRatio.Maximum = 255;
			this.TrbExpSetRatio.Name = "TrbExpSetRatio";
			this.TrbExpSetRatio.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.TrbExpSetRatio.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.TrbExpSetRatio.RightToLeftLayout = true;
			this.TrbExpSetRatio.Size = new System.Drawing.Size(28, 387);
			this.TrbExpSetRatio.TabIndex = 18;
			this.TrbExpSetRatio.TickFrequency = 20;
			this.TrbExpSetRatio.ValueChanged += new System.EventHandler(TrbExpSetRatio_ValueChanged);
			this.TrbExpSetRatio.MouseUp += new System.Windows.Forms.MouseEventHandler(TrbExpSetRatio_MouseUp);
			this.LabImageSize.AutoSize = true;
			this.LabImageSize.Location = new System.Drawing.Point(3, 7);
			this.LabImageSize.Name = "LabImageSize";
			this.LabImageSize.Size = new System.Drawing.Size(112, 14);
			this.LabImageSize.TabIndex = 27;
			this.LabImageSize.Text = "图像显示清晰度:";
			this.BottomContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BottomContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BottomContainer.Location = new System.Drawing.Point(0, 0);
			this.BottomContainer.Name = "BottomContainer";
			this.BottomContainer.Panel1.Controls.Add(this.TabSmallForm);
			this.BottomContainer.Panel2.Controls.Add(this.GrbDecodeData);
			this.BottomContainer.Size = new System.Drawing.Size(834, 147);
			this.BottomContainer.SplitterDistance = 381;
			this.BottomContainer.SplitterWidth = 5;
			this.BottomContainer.TabIndex = 0;
			this.TabSmallForm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TabSmallForm.Controls.Add(this.Tp_DecodeTime);
			this.TabSmallForm.Controls.Add(this.Tp_DecStatistics);
			this.TabSmallForm.Location = new System.Drawing.Point(2, -1);
			this.TabSmallForm.Name = "TabSmallForm";
			this.TabSmallForm.SelectedIndex = 0;
			this.TabSmallForm.Size = new System.Drawing.Size(373, 151);
			this.TabSmallForm.TabIndex = 0;
			this.Tp_DecodeTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Tp_DecodeTime.Controls.Add(this.PanDecodeTime);
			this.Tp_DecodeTime.Location = new System.Drawing.Point(4, 24);
			this.Tp_DecodeTime.Name = "Tp_DecodeTime";
			this.Tp_DecodeTime.Padding = new System.Windows.Forms.Padding(3);
			this.Tp_DecodeTime.Size = new System.Drawing.Size(365, 123);
			this.Tp_DecodeTime.TabIndex = 0;
			this.Tp_DecodeTime.Text = "解码耗时";
			this.Tp_DecodeTime.UseVisualStyleBackColor = true;
			this.PanDecodeTime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.PanDecodeTime.Controls.Add(this.LabCurrentDecodeTime);
			this.PanDecodeTime.Controls.Add(this.LabAveDecodeTime);
			this.PanDecodeTime.Controls.Add(this.LabAveDecodeTimeDisp);
			this.PanDecodeTime.Controls.Add(this.LabNowDecodeTime);
			this.PanDecodeTime.Controls.Add(this.ChartDecodeTime);
			this.PanDecodeTime.Location = new System.Drawing.Point(3, 2);
			this.PanDecodeTime.Name = "PanDecodeTime";
			this.PanDecodeTime.Size = new System.Drawing.Size(359, 122);
			this.PanDecodeTime.TabIndex = 0;
			this.LabCurrentDecodeTime.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabCurrentDecodeTime.AutoSize = true;
			this.LabCurrentDecodeTime.Location = new System.Drawing.Point(10, 103);
			this.LabCurrentDecodeTime.Name = "LabCurrentDecodeTime";
			this.LabCurrentDecodeTime.Size = new System.Drawing.Size(77, 14);
			this.LabCurrentDecodeTime.TabIndex = 21;
			this.LabCurrentDecodeTime.Text = "解码耗时：";
			this.LabAveDecodeTime.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabAveDecodeTime.AutoSize = true;
			this.LabAveDecodeTime.Location = new System.Drawing.Point(258, 103);
			this.LabAveDecodeTime.Name = "LabAveDecodeTime";
			this.LabAveDecodeTime.Size = new System.Drawing.Size(28, 14);
			this.LabAveDecodeTime.TabIndex = 24;
			this.LabAveDecodeTime.Text = "0ms";
			this.LabAveDecodeTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.LabAveDecodeTimeDisp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabAveDecodeTimeDisp.AutoSize = true;
			this.LabAveDecodeTimeDisp.Location = new System.Drawing.Point(148, 103);
			this.LabAveDecodeTimeDisp.Name = "LabAveDecodeTimeDisp";
			this.LabAveDecodeTimeDisp.Size = new System.Drawing.Size(105, 14);
			this.LabAveDecodeTimeDisp.TabIndex = 23;
			this.LabAveDecodeTimeDisp.Text = "平均解码时间：";
			this.LabNowDecodeTime.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabNowDecodeTime.AutoSize = true;
			this.LabNowDecodeTime.Location = new System.Drawing.Point(94, 103);
			this.LabNowDecodeTime.Name = "LabNowDecodeTime";
			this.LabNowDecodeTime.Size = new System.Drawing.Size(28, 14);
			this.LabNowDecodeTime.TabIndex = 22;
			this.LabNowDecodeTime.Text = "0ms";
			this.LabNowDecodeTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.ChartDecodeTime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.ChartDecodeTime.BackColor = System.Drawing.Color.Transparent;
			chartArea.AxisX.Crossing = double.MinValue;
			chartArea.AxisX.InterlacedColor = System.Drawing.Color.Transparent;
			chartArea.AxisX.Interval = 2.0;
			chartArea.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
			chartArea.AxisX.MajorGrid.Enabled = false;
			chartArea.AxisX.Maximum = 20.0;
			chartArea.AxisX.Minimum = 0.0;
			chartArea.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
			chartArea.AxisY.Interval = 30.0;
			chartArea.AxisY.IsStartedFromZero = false;
			chartArea.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			chartArea.AxisY.Maximum = 300.0;
			chartArea.AxisY.Minimum = 0.0;
			chartArea.BackColor = System.Drawing.Color.Transparent;
			chartArea.Name = "ChartArea1";
			this.ChartDecodeTime.ChartAreas.Add(chartArea);
			legend.BackColor = System.Drawing.Color.Transparent;
			legend.Enabled = false;
			legend.IsDockedInsideChartArea = false;
			legend.Name = "Legend1";
			this.ChartDecodeTime.Legends.Add(legend);
			this.ChartDecodeTime.Location = new System.Drawing.Point(-11, -3);
			this.ChartDecodeTime.Margin = new System.Windows.Forms.Padding(1);
			this.ChartDecodeTime.Name = "ChartDecodeTime";
			series.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			series.ChartArea = "ChartArea1";
			series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			series.Legend = "Legend1";
			series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
			series.Name = "DecodeTime";
			series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
			series.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
			this.ChartDecodeTime.Series.Add(series);
			this.ChartDecodeTime.Size = new System.Drawing.Size(379, 111);
			this.ChartDecodeTime.TabIndex = 25;
			this.ChartDecodeTime.Text = "DecodeTimeStatistics";
			this.Tp_DecStatistics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Tp_DecStatistics.Controls.Add(this.PanDecStatistics);
			this.Tp_DecStatistics.Location = new System.Drawing.Point(4, 22);
			this.Tp_DecStatistics.Name = "Tp_DecStatistics";
			this.Tp_DecStatistics.Padding = new System.Windows.Forms.Padding(3);
			this.Tp_DecStatistics.Size = new System.Drawing.Size(364, 123);
			this.Tp_DecStatistics.TabIndex = 1;
			this.Tp_DecStatistics.Text = "数据统计";
			this.Tp_DecStatistics.UseVisualStyleBackColor = true;
			this.PanDecStatistics.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.PanDecStatistics.Controls.Add(this.LabTrigCount);
			this.PanDecStatistics.Controls.Add(this.ChkRealTimeUpdate);
			this.PanDecStatistics.Controls.Add(this.LabNGCount);
			this.PanDecStatistics.Controls.Add(this.LabOKCount);
			this.PanDecStatistics.Controls.Add(this.BtnResetStatic);
			this.PanDecStatistics.Controls.Add(this.LabTotalDisp);
			this.PanDecStatistics.Controls.Add(this.LabDispNG);
			this.PanDecStatistics.Controls.Add(this.LabOKDisp);
			this.PanDecStatistics.Controls.Add(this.LabOKRatio);
			this.PanDecStatistics.Location = new System.Drawing.Point(3, 5);
			this.PanDecStatistics.Name = "PanDecStatistics";
			this.PanDecStatistics.Size = new System.Drawing.Size(358, 125);
			this.PanDecStatistics.TabIndex = 0;
			this.LabTrigCount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.LabTrigCount.Font = new System.Drawing.Font("黑体", 20f, System.Drawing.FontStyle.Bold);
			this.LabTrigCount.ForeColor = System.Drawing.Color.FromArgb(0, 115, 255);
			this.LabTrigCount.Location = new System.Drawing.Point(3, 39);
			this.LabTrigCount.Name = "LabTrigCount";
			this.LabTrigCount.Size = new System.Drawing.Size(130, 49);
			this.LabTrigCount.TabIndex = 21;
			this.LabTrigCount.Text = "0";
			this.LabTrigCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ChkRealTimeUpdate.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.ChkRealTimeUpdate.Location = new System.Drawing.Point(255, 99);
			this.ChkRealTimeUpdate.Name = "ChkRealTimeUpdate";
			this.ChkRealTimeUpdate.Size = new System.Drawing.Size(54, 18);
			this.ChkRealTimeUpdate.TabIndex = 25;
			this.ChkRealTimeUpdate.Text = "实时";
			this.ChkRealTimeUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ChkRealTimeUpdate.UseVisualStyleBackColor = true;
			this.ChkRealTimeUpdate.CheckedChanged += new System.EventHandler(ChkRealTimeUpdate_CheckedChanged);
			this.LabNGCount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.LabNGCount.Font = new System.Drawing.Font("黑体", 20f, System.Drawing.FontStyle.Bold);
			this.LabNGCount.ForeColor = System.Drawing.Color.LightGray;
			this.LabNGCount.Location = new System.Drawing.Point(209, 39);
			this.LabNGCount.Name = "LabNGCount";
			this.LabNGCount.Size = new System.Drawing.Size(138, 49);
			this.LabNGCount.TabIndex = 23;
			this.LabNGCount.Text = "0";
			this.LabNGCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LabOKCount.Font = new System.Drawing.Font("黑体", 20f, System.Drawing.FontStyle.Bold);
			this.LabOKCount.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
			this.LabOKCount.Location = new System.Drawing.Point(209, 8);
			this.LabOKCount.Name = "LabOKCount";
			this.LabOKCount.Size = new System.Drawing.Size(139, 33);
			this.LabOKCount.TabIndex = 22;
			this.LabOKCount.Text = "0";
			this.LabOKCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnResetStatic.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.BtnResetStatic.FlatAppearance.BorderSize = 0;
			this.BtnResetStatic.Location = new System.Drawing.Point(150, 87);
			this.BtnResetStatic.Name = "BtnResetStatic";
			this.BtnResetStatic.Size = new System.Drawing.Size(103, 30);
			this.BtnResetStatic.TabIndex = 20;
			this.BtnResetStatic.Text = "重置数据";
			this.BtnResetStatic.UseVisualStyleBackColor = true;
			this.BtnResetStatic.Click += new System.EventHandler(BtnResetStatic_Click);
			this.LabTotalDisp.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
			this.LabTotalDisp.ForeColor = System.Drawing.Color.FromArgb(0, 115, 255);
			this.LabTotalDisp.Location = new System.Drawing.Point(16, 8);
			this.LabTotalDisp.Name = "LabTotalDisp";
			this.LabTotalDisp.Size = new System.Drawing.Size(93, 33);
			this.LabTotalDisp.TabIndex = 19;
			this.LabTotalDisp.Text = "Total";
			this.LabTotalDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LabDispNG.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.LabDispNG.Font = new System.Drawing.Font("黑体", 20f, System.Drawing.FontStyle.Bold);
			this.LabDispNG.ForeColor = System.Drawing.Color.LightGray;
			this.LabDispNG.Location = new System.Drawing.Point(129, 39);
			this.LabDispNG.Name = "LabDispNG";
			this.LabDispNG.Size = new System.Drawing.Size(82, 49);
			this.LabDispNG.TabIndex = 18;
			this.LabDispNG.Text = "NG×";
			this.LabDispNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LabOKDisp.Font = new System.Drawing.Font("黑体", 20f, System.Drawing.FontStyle.Bold);
			this.LabOKDisp.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
			this.LabOKDisp.Location = new System.Drawing.Point(129, 8);
			this.LabOKDisp.Name = "LabOKDisp";
			this.LabOKDisp.Size = new System.Drawing.Size(82, 33);
			this.LabOKDisp.TabIndex = 17;
			this.LabOKDisp.Text = "OK√";
			this.LabOKDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LabOKRatio.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabOKRatio.Font = new System.Drawing.Font("黑体", 20f, System.Drawing.FontStyle.Bold);
			this.LabOKRatio.ForeColor = System.Drawing.Color.Gray;
			this.LabOKRatio.Location = new System.Drawing.Point(9, 84);
			this.LabOKRatio.Name = "LabOKRatio";
			this.LabOKRatio.Size = new System.Drawing.Size(136, 33);
			this.LabOKRatio.TabIndex = 24;
			this.LabOKRatio.Text = "100.00%";
			this.LabOKRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.GrbDecodeData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbDecodeData.Controls.Add(this.LabBarcodeLen);
			this.GrbDecodeData.Controls.Add(this.LabBarcodeLenDisp);
			this.GrbDecodeData.Controls.Add(this.LabBarcodeType);
			this.GrbDecodeData.Controls.Add(this.TxbDecodeData);
			this.GrbDecodeData.Controls.Add(this.LabBarcodeTypeDisp);
			this.GrbDecodeData.Location = new System.Drawing.Point(3, 1);
			this.GrbDecodeData.Name = "GrbDecodeData";
			this.GrbDecodeData.Size = new System.Drawing.Size(404, 147);
			this.GrbDecodeData.TabIndex = 25;
			this.GrbDecodeData.TabStop = false;
			this.GrbDecodeData.Text = "解码结果";
			this.LabBarcodeLen.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabBarcodeLen.AutoSize = true;
			this.LabBarcodeLen.Location = new System.Drawing.Point(266, 125);
			this.LabBarcodeLen.Name = "LabBarcodeLen";
			this.LabBarcodeLen.Size = new System.Drawing.Size(14, 14);
			this.LabBarcodeLen.TabIndex = 15;
			this.LabBarcodeLen.Text = "0";
			this.LabBarcodeLen.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.LabBarcodeLenDisp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabBarcodeLenDisp.AutoSize = true;
			this.LabBarcodeLenDisp.Location = new System.Drawing.Point(175, 125);
			this.LabBarcodeLenDisp.Name = "LabBarcodeLenDisp";
			this.LabBarcodeLenDisp.Size = new System.Drawing.Size(77, 14);
			this.LabBarcodeLenDisp.TabIndex = 14;
			this.LabBarcodeLenDisp.Text = "条码长度：";
			this.LabBarcodeType.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabBarcodeType.Location = new System.Drawing.Point(82, 125);
			this.LabBarcodeType.Name = "LabBarcodeType";
			this.LabBarcodeType.Size = new System.Drawing.Size(108, 14);
			this.LabBarcodeType.TabIndex = 13;
			this.LabBarcodeType.Text = "Datamatrix";
			this.TxbDecodeData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TxbDecodeData.Location = new System.Drawing.Point(9, 24);
			this.TxbDecodeData.Multiline = true;
			this.TxbDecodeData.Name = "TxbDecodeData";
			this.TxbDecodeData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxbDecodeData.Size = new System.Drawing.Size(387, 92);
			this.TxbDecodeData.TabIndex = 6;
			this.TxbDecodeData.DoubleClick += new System.EventHandler(TxbDecodeData_DoubleClick);
			this.LabBarcodeTypeDisp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabBarcodeTypeDisp.AutoSize = true;
			this.LabBarcodeTypeDisp.Location = new System.Drawing.Point(7, 125);
			this.LabBarcodeTypeDisp.Name = "LabBarcodeTypeDisp";
			this.LabBarcodeTypeDisp.Size = new System.Drawing.Size(77, 14);
			this.LabBarcodeTypeDisp.TabIndex = 12;
			this.LabBarcodeTypeDisp.Text = "条码类型：";
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(834, 689);
			base.Controls.Add(this.MainContainer);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "ReadingForm";
			this.Text = "ReadingForm";
			base.Load += new System.EventHandler(ReadingForm_Load);
			this.MainContainer.Panel1.ResumeLayout(false);
			this.MainContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.MainContainer).EndInit();
			this.MainContainer.ResumeLayout(false);
			this.TopContainer.Panel1.ResumeLayout(false);
			this.TopContainer.Panel1.PerformLayout();
			this.TopContainer.Panel2.ResumeLayout(false);
			this.TopContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.TopContainer).EndInit();
			this.TopContainer.ResumeLayout(false);
			this.PanAutoFocus.ResumeLayout(false);
			this.GrbImageROI.ResumeLayout(false);
			this.GrbImageROI.PerformLayout();
			this.PanManualFocus.ResumeLayout(false);
			this.PanManualFocus.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbSensorLensFocusSet).EndInit();
			this.CMS_ImageRightKey.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.TrbSensorGainSet).EndInit();
			((System.ComponentModel.ISupportInitialize)this.TrbExpSetRatio).EndInit();
			this.BottomContainer.Panel1.ResumeLayout(false);
			this.BottomContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.BottomContainer).EndInit();
			this.BottomContainer.ResumeLayout(false);
			this.TabSmallForm.ResumeLayout(false);
			this.Tp_DecodeTime.ResumeLayout(false);
			this.PanDecodeTime.ResumeLayout(false);
			this.PanDecodeTime.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.ChartDecodeTime).EndInit();
			this.Tp_DecStatistics.ResumeLayout(false);
			this.PanDecStatistics.ResumeLayout(false);
			this.GrbDecodeData.ResumeLayout(false);
			this.GrbDecodeData.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
