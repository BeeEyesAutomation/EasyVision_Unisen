using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using FTPManager_N;
using Heroje_Debug_Tool.BaseClass;
using HJ_Controls_Lib;
using 合杰图像算法调试工具;
using 图像算法调试工具;

namespace Heroje_Debug_Tool.SubForm
{
	public class TemplateSettingForm : Form
	{
		public delegate void TemplateActCB(ref template_config_t tmp, TemplateActionDef act);

		public delegate void TemplatePageDrawROICB(DeviceSettingForm.ImageRoiActionNum Act);

		private TemplateActCB TemplateActFuncCB;

		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private TemplatePageDrawROICB TemplatePageDrawROIFuncCB;

		private bool IsDrawingTemplateRoi_v = false;

		private int TemplateColorIdx = 0;

		private IContainer components = null;

		private GroupBox GrbNewTemplate;

		private ComboBox CobTemplateIspFunc2;

		private Label LabTemplateIndex;

		private ComboBox CobTemplateIndex;

		private CheckBox ChbTemplateLightGroup;

		private CheckBox ChbTemplateEnable;

		private CheckBox ChbTemplateBarcodeType;

		private ComboBox CobTemplateIspFunc1;

		private CheckBox ChbImagePreProc2;

		private CheckBox ChbImagePreProc1;

		private ComboBox CobTemplateOutputLogic;

		private Label LabTemplateOutputLogic;

		private CheckBox ChbTemplateLensFocus;

		private Panel PanTemplateRoi;

		private TextBox TxbTemplateRoiH;

		private TextBox TxbTemplateRoiW;

		private TextBox TxbTemplateRoiY;

		private TextBox TxbTemplateRoiX;

		private Label LabTempH;

		private Label LabTempW;

		private Label LabTempY;

		private Label LabTempX;

		private CheckBox ChbTemplateLightConfig;

		private CheckBox ChbTemplateAboutROI;

		private CheckBox ChbTemplateAboutSensor;

		private GroupBox GrbTemplateOp;

		private Button BtnReadTemplate1;

		private Button BtnReadTemplate2;

		private Button BtnReadTemplate3;

		private Button BtnSaveTemplate3;

		private Button BtnSaveTemplate1;

		private Button BtnSaveTemplate2;

		private GroupBox GrbTemplateFiles;

		private CheckBox ChbTempNameUseIdx;

		private ListView ListViewTemplate;

		private Label LabTemplateName;

		private TextBox TxbTemplateName;

		private Button BtnTemplateSave;

		private Button BtnReadAllTemplate;

		private GroupBox GrbTemplateSetting;

		private CheckBox ChbEnableTempFloop;

		private ComboBox CobTempDPMLevel;

		private Label LabTempDPMLevel;

		private TextBox TxbSensorGainDisp;

		private Label LabSensorGainDisp;

		private TextBox TxbSensorExpDisp;

		private Label LabSensorExpDisp;

		private TextBox TxbSecondLightDisp;

		private Label LabSecondLightDisp;

		private TextBox TxbMainLightDisp;

		private Label LabMainLightDisp;

		private Label LabLineSeparator;

		private TextBox TxbGroupForderIdx;

		private TextBox TxbGroupForderName;

		private Label LabGroupForder;

		private Button BtnGroupTemplateSet;

		private TextBox TxbGroupTemplateSet;

		private CheckBox ChbGroupTemplate;

		private Button BtnGetTempPara;

		private ImageList ImgListTemplate;

		private ContextMenuStrip Cms_Template;

		private ToolStripMenuItem TsmTemplateDelete;

		private ToolStripMenuItem TsmTemplateUpload;

		private ToolStripMenuItem TsmTemplateDownload;

		private UCSplitLine_H ucSplitLine_H1;

		private Button BtnDrawTemplateRoi;

		private Button BtnDrawMultRoi;

		public bool IsDrawingTemplateRoi
		{
			get
			{
				return IsDrawingTemplateRoi_v;
			}
			set
			{
				IsDrawingTemplateRoi_v = value;
				if (IsDrawingTemplateRoi_v)
				{
					BtnDrawTemplateRoi.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TempRoiEndDraw);
					BtnDrawTemplateRoi.ForeColor = Color.Red;
				}
				else
				{
					BtnDrawTemplateRoi.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TempRoiStartDraw);
					BtnDrawTemplateRoi.ForeColor = Color.Black;
				}
			}
		}

		public TemplateSettingForm()
		{
			InitializeComponent();
		}

		private void TemplateSettingForm_Load(object sender, EventArgs e)
		{
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB, TemplateActCB templateActCB, TemplatePageDrawROICB templatePage_Draw_ROI_CB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
			TemplateActFuncCB = templateActCB;
			TemplatePageDrawROIFuncCB = templatePage_Draw_ROI_CB;
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			if (GetCfgCBFuncCB(61443u) == 1)
			{
				ChbEnableTempFloop.Checked = true;
			}
			else
			{
				ChbEnableTempFloop.Checked = false;
			}
			ChbGroupTemplate.Checked = GetCfgCBFuncCB(139272u) == 8;
			TxbGroupTemplateSet.Text = GetCfgCBFuncCB(140543u).ToString();
			if (para.DeviceTypeRecord >= 6)
			{
				GrbTemplateSetting.Enabled = true;
				GrbTemplateOp.Enabled = true;
				GrbNewTemplate.Enabled = true;
				GrbTemplateFiles.Enabled = true;
				if (GetCfgCBFuncCB(139271u) == 0)
				{
					CobTemplateOutputLogic.SelectedIndex = 0;
				}
				else if (GetCfgCBFuncCB(139271u) == 3)
				{
					CobTemplateOutputLogic.SelectedIndex = 2;
				}
				else if (GetCfgCBFuncCB(139271u) == 2)
				{
					CobTemplateOutputLogic.SelectedIndex = 3;
				}
				else if (GetCfgCBFuncCB(139271u) == 4)
				{
					CobTemplateOutputLogic.SelectedIndex = 4;
				}
				else
				{
					CobTemplateOutputLogic.SelectedIndex = 1;
				}
			}
			else
			{
				GrbTemplateSetting.Enabled = true;
				GrbTemplateOp.Enabled = true;
				GrbNewTemplate.Enabled = false;
				GrbTemplateFiles.Enabled = false;
			}
		}

		public void FunctionOnOff(bool[] CapacityArr)
		{
			GrbNewTemplate.Enabled = CapacityArr[15];
			GrbTemplateFiles.Enabled = CapacityArr[15];
		}

		public void UpdateLanguageUI()
		{
			int selectedIndex = CobTempDPMLevel.SelectedIndex;
			CobTempDPMLevel.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobTempDPMLevel.Items.AddRange(new object[9] { "正常模式", "DPM模式1", "DPM模式2", "DPM模式3", "DPM模式4", "DPM模式5", "DPM模式6", "DPM模式7", "DPM模式8" });
			}
			else
			{
				CobTempDPMLevel.Items.AddRange(new object[9] { "Normal", "Mode 1", "Mode 2", "Mode 3", "Mode 4", "Mode 5", "Mode 6", "Mode 7", "Mode 8" });
			}
			CobTempDPMLevel.SelectedIndex = selectedIndex;
			CobTemplateOutputLogic.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobTemplateOutputLogic.Items.AddRange(new object[5] { "解一个就退(输出一个)", "解至少一个退(NG不输出)", "解至少一个退(NG输出)", "解全部才退出(调试用)", "解全部才退出(实际用)" });
			}
			else
			{
				CobTemplateOutputLogic.Items.AddRange(new object[5] { "DecodeAnyOne", "AtLeastOne(non-NG)", "AtLeastOne(NG-Out)", "DecodeAll(OnePic)", "DecodeAll(Timeout)" });
			}
			CobTemplateIspFunc2.Items.Clear();
			CobTemplateIspFunc1.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobTemplateIspFunc2.Items.AddRange(new object[8] { "高斯滤波", "形态学膨胀", "形态学腐蚀", "对比度增强", "图像反色", "图像镜像", "中值滤波", "维纳滤波" });
				CobTemplateIspFunc1.Items.AddRange(new object[8] { "高斯滤波", "形态学膨胀", "形态学腐蚀", "对比度增强", "图像反色", "图像镜像", "中值滤波", "维纳滤波" });
			}
			else
			{
				CobTemplateIspFunc2.Items.AddRange(new object[8] { "GaussFilter", "DilateFilter", "ErosionFilter", "ContrastEnhance", "ImgOpposition", "ImageMirror", "MedianFiler", "WienerFilter" });
				CobTemplateIspFunc1.Items.AddRange(new object[8] { "GaussFilter", "DilateFilter", "ErosionFilter", "ContrastEnhance", "ImgOpposition", "ImageMirror", "MedianFiler", "WienerFilter" });
			}
		}

		private void ChbEnableTempFloop_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbEnableTempFloop.Checked)
				{
					SetCfgCBFuncCB(61443u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(61443u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void BtnReadTemplate1_Click(object sender, EventArgs e)
		{
			ToolCfg.UpdateAdjState = true;
			int mainLightDutyVal = GetCfgCBFuncCB(63487u);
			int expSetRatioVal = GetCfgCBFuncCB(62719u);
			int sensorGainSetVal = GetCfgCBFuncCB(61951u);
			int secondLightDuty = GetCfgCBFuncCB(64255u);
			int sel_index = GetCfgCBFuncCB(65008u);
			ApplyTempOldToDevice(mainLightDutyVal, expSetRatioVal, sensorGainSetVal, secondLightDuty, sel_index);
		}

		private void ApplyTempOldToDevice(int MainLightDutyVal, int ExpSetRatioVal, int SensorGainSetVal, int SecondLightDuty, int sel_index)
		{
			if (sel_index >= CobTempDPMLevel.Items.Count)
			{
				sel_index = 0;
			}
			CobTempDPMLevel.SelectedIndex = sel_index;
			TxbMainLightDisp.Text = MainLightDutyVal.ToString();
			TxbSecondLightDisp.Text = SecondLightDuty.ToString();
			TxbSensorExpDisp.Text = ExpSetRatioVal.ToString();
			TxbSensorGainDisp.Text = SensorGainSetVal.ToString();
			ToolCfg.UpdateAdjState = false;
			if (GetCfgCBFuncCB(7296u) == 0)
			{
				SetCfgCBFuncCB(4607u, (ushort)ExpSetRatioVal);
			}
			SetCfgCBFuncCB(51471u, (ushort)CobTempDPMLevel.SelectedIndex);
			SetCfgCBFuncCB(1279u, (ushort)MainLightDutyVal);
			SetCfgCBFuncCB(51455u, (ushort)SensorGainSetVal);
			SetCfgCBFuncCB(5631u, (ushort)SecondLightDuty);
			SendCfgDataCBFuncCB(640u);
			template_config_t tmp = default(template_config_t);
			tmp.sensor_config.light_pwm1 = MainLightDutyVal;
			tmp.sensor_config.light_pwm2 = SecondLightDuty;
			tmp.sensor_config.exp_time = ExpSetRatioVal;
			tmp.sensor_config.config_flag |= 2;
			tmp.sensor_config.gain_set = SensorGainSetVal;
			TemplateActFuncCB(ref tmp, TemplateActionDef.Show_Old_Template_Value);
		}

		private void BtnSaveTemplate1_Click(object sender, EventArgs e)
		{
			BtnGetTempPara_Click(null, null);
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			ushort result = 0;
			if (ushort.TryParse(TxbMainLightDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(63487u, result);
			}
			else
			{
				TxbMainLightDisp.Text = GetCfgCBFuncCB(63487u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSecondLightDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(64255u, result);
			}
			else
			{
				TxbSecondLightDisp.Text = GetCfgCBFuncCB(64255u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSensorExpDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(62719u, result);
			}
			else
			{
				TxbSensorExpDisp.Text = GetCfgCBFuncCB(62719u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSensorGainDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(61951u, result);
			}
			else
			{
				TxbSensorGainDisp.Text = GetCfgCBFuncCB(61951u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			SetCfgCBFuncCB(65008u, (ushort)CobTempDPMLevel.SelectedIndex);
			SendCfgDataCBFuncCB(640u);
		}

		private void BtnReadTemplate2_Click(object sender, EventArgs e)
		{
			ToolCfg.UpdateAdjState = true;
			int mainLightDutyVal = GetCfgCBFuncCB(63743u);
			int expSetRatioVal = GetCfgCBFuncCB(62975u);
			int sensorGainSetVal = GetCfgCBFuncCB(62207u);
			int secondLightDuty = GetCfgCBFuncCB(64511u);
			int sel_index = GetCfgCBFuncCB(65264u);
			ApplyTempOldToDevice(mainLightDutyVal, expSetRatioVal, sensorGainSetVal, secondLightDuty, sel_index);
		}

		private void BtnSaveTemplate2_Click(object sender, EventArgs e)
		{
			BtnGetTempPara_Click(null, null);
			ushort result = 0;
			if (ushort.TryParse(TxbMainLightDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(63743u, result);
			}
			else
			{
				TxbMainLightDisp.Text = GetCfgCBFuncCB(63743u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSecondLightDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(64511u, result);
			}
			else
			{
				TxbSecondLightDisp.Text = GetCfgCBFuncCB(64511u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSensorExpDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(62975u, result);
			}
			else
			{
				TxbSensorExpDisp.Text = GetCfgCBFuncCB(62975u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSensorGainDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(62207u, result);
			}
			else
			{
				TxbSensorGainDisp.Text = GetCfgCBFuncCB(62207u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			SetCfgCBFuncCB(65264u, (ushort)CobTempDPMLevel.SelectedIndex);
			SendCfgDataCBFuncCB(640u);
		}

		private void BtnReadTemplate3_Click(object sender, EventArgs e)
		{
			ToolCfg.UpdateAdjState = true;
			int mainLightDutyVal = GetCfgCBFuncCB(63999u);
			int expSetRatioVal = GetCfgCBFuncCB(63231u);
			int sensorGainSetVal = GetCfgCBFuncCB(62463u);
			int secondLightDuty = GetCfgCBFuncCB(64767u);
			int sel_index = GetCfgCBFuncCB(65520u);
			ApplyTempOldToDevice(mainLightDutyVal, expSetRatioVal, sensorGainSetVal, secondLightDuty, sel_index);
		}

		private void BtnSaveTemplate3_Click(object sender, EventArgs e)
		{
			BtnGetTempPara_Click(null, null);
			ushort result = 0;
			if (ushort.TryParse(TxbMainLightDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(63999u, result);
			}
			else
			{
				TxbMainLightDisp.Text = GetCfgCBFuncCB(63999u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSecondLightDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(64767u, result);
			}
			else
			{
				TxbSecondLightDisp.Text = GetCfgCBFuncCB(64767u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSensorExpDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(63231u, result);
			}
			else
			{
				TxbSensorExpDisp.Text = GetCfgCBFuncCB(63231u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			if (ushort.TryParse(TxbSensorGainDisp.Text, out result) && result < 256)
			{
				SetCfgCBFuncCB(62463u, result);
			}
			else
			{
				TxbSensorGainDisp.Text = GetCfgCBFuncCB(62463u).ToString();
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("错误:数据填写有误，已更正", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Error:Data error，Has been corrected", "Tips", MessageBoxButtons.OK);
				}
			}
			SetCfgCBFuncCB(65520u, (ushort)CobTempDPMLevel.SelectedIndex);
			SendCfgDataCBFuncCB(640u);
		}

		private void BtnDrawTemplateRoi_Click(object sender, EventArgs e)
		{
			IsDrawingTemplateRoi_v = !IsDrawingTemplateRoi_v;
			if (IsDrawingTemplateRoi_v)
			{
				TemplatePageDrawROIFuncCB(DeviceSettingForm.ImageRoiActionNum.DrawTemplateRoi);
				BtnDrawTemplateRoi.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TempRoiEndDraw);
				BtnDrawTemplateRoi.ForeColor = Color.Red;
				return;
			}
			TemplatePageDrawROIFuncCB(DeviceSettingForm.ImageRoiActionNum.DrawTemplateRoiOk);
			TxbTemplateRoiX.Text = ToolCfg.TemplateRoiX.ToString();
			TxbTemplateRoiY.Text = ToolCfg.TemplateRoiY.ToString();
			TxbTemplateRoiW.Text = ToolCfg.TemplateRoiW.ToString();
			TxbTemplateRoiH.Text = ToolCfg.TemplateRoiH.ToString();
			BtnDrawTemplateRoi.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TempRoiStartDraw);
			BtnDrawTemplateRoi.ForeColor = Color.Black;
		}

		private void ChbTemplateAboutROI_CheckedChanged(object sender, EventArgs e)
		{
			if (!ChbTemplateAboutROI.Checked)
			{
			}
		}

		private void CobTemplateOutputLogic_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				switch (CobTemplateOutputLogic.SelectedIndex)
				{
				case 0:
					SetCfgCBFuncCB(139271u, 0u);
					break;
				case 1:
					SetCfgCBFuncCB(139271u, 1u);
					break;
				case 2:
					SetCfgCBFuncCB(139271u, 3u);
					break;
				case 3:
					SetCfgCBFuncCB(139271u, 2u);
					break;
				case 4:
					SetCfgCBFuncCB(139271u, 4u);
					break;
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbGroupTemplate_CheckedChanged(object sender, EventArgs e)
		{
			if (ChbGroupTemplate.Checked)
			{
				TxbGroupTemplateSet.Enabled = true;
				BtnGroupTemplateSet.Enabled = true;
				LabGroupForder.Enabled = true;
				TxbGroupForderName.Enabled = true;
				TxbGroupForderIdx.Enabled = true;
			}
			else
			{
				TxbGroupTemplateSet.Enabled = false;
				BtnGroupTemplateSet.Enabled = false;
				LabGroupForder.Enabled = false;
				TxbGroupForderName.Enabled = false;
				TxbGroupForderIdx.Enabled = false;
			}
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbGroupTemplate.Checked)
				{
					SetCfgCBFuncCB(139272u, 8u);
				}
				else
				{
					SetCfgCBFuncCB(139272u, 0u);
				}
				SendCfgDataCBFuncCB(32768u);
			}
		}

		private void TxbGroupTemplateSet_TextChanged(object sender, EventArgs e)
		{
			BtnGroupTemplateSet.Visible = true;
		}

		private void BtnGroupTemplateSet_Click(object sender, EventArgs e)
		{
			BtnGroupTemplateSet.Visible = false;
			if (!ToolCfg.UpdateAdjState)
			{
				uint result;
				if (uint.TryParse(TxbGroupTemplateSet.Text, out result) && result < 255)
				{
					SetCfgCBFuncCB(140543u, (ushort)result);
					SendCfgDataCBFuncCB(32768u);
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("模板组选择的参数非法!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show("Template group value illegal!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void CobTemplateIndex_SelectedIndexChanged(object sender, EventArgs e)
		{
			TemplateColorIdx = CobTemplateIndex.SelectedIndex;
			ChbTemplateEnable.ForeColor = ToolCfg.TemplateColor[TemplateColorIdx];
			ToolCfg.MaskPen = new Pen(new SolidBrush(ToolCfg.TemplateColor[TemplateColorIdx]), 2f);
			if (ChbTempNameUseIdx.Checked)
			{
				TxbTemplateName.Text = (CobTemplateIndex.SelectedIndex + 1).ToString("D2");
			}
		}

		private void ShowAllTemplateFile(string ftpPath)
		{
			if (ToolCfg.CurrentDevice == null || ToolCfg.CurrentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK || !ToolCfg.CurrentDevice.IsCommunicate)
			{
				return;
			}
			ListViewTemplate.BeginUpdate();
			ListViewTemplate.Items.Clear();
			try
			{
				DirItemInfo[] array = ToolCfg.ftp.GetFtpFileInfos(ftpPath).ToArray();
				DirItemInfo[] array2 = array;
				foreach (DirItemInfo dirItemInfo in array2)
				{
					if (dirItemInfo.FileType != 0)
					{
						ListViewItem listViewItem = ListViewTemplate.Items.Add(dirItemInfo.Name);
						listViewItem.ImageIndex = 0;
						listViewItem.Tag = dirItemInfo;
						listViewItem.SubItems.Add("file");
					}
					else if (dirItemInfo.FileType == 0)
					{
						ListViewItem listViewItem2 = ListViewTemplate.Items.Add(dirItemInfo.Name);
						listViewItem2.ImageIndex = 1;
						listViewItem2.Tag = dirItemInfo;
						listViewItem2.SubItems.Add("dir");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			ListViewTemplate.EndUpdate();
		}

		private void BtnReadAllTemplate_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice != null)
			{
				string ftpPath = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template";
				ShowAllTemplateFile(ftpPath);
			}
		}

		private void BtnTemplateSave_Click(object sender, EventArgs e)
		{
			if (ChbTemplateAboutROI.Checked)
			{
				TxbTemplateRoiX.Text = ToolCfg.TemplateRoiX.ToString();
				TxbTemplateRoiY.Text = ToolCfg.TemplateRoiY.ToString();
				TxbTemplateRoiW.Text = ToolCfg.TemplateRoiW.ToString();
				TxbTemplateRoiH.Text = ToolCfg.TemplateRoiH.ToString();
			}
			if (string.IsNullOrWhiteSpace(TxbTemplateName.Text))
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TextTemplateNameNullTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TextTemplateNameNullTips));
				return;
			}
			if (TxbTemplateName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TextTemplateNameInvalidTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TextTemplateNameInvalidTips));
				return;
			}
			template_config_t tmp = default(template_config_t);
			TemplateActFuncCB(ref tmp, TemplateActionDef.Get_New_Template_Value);
			template_config_t template_config_t = default(template_config_t);
			template_config_t.imgproc_config = new imgproc_config_t[CobTemplateIndex.Items.Count];
			template_config_t.config_flag = 0;
			template_config_t.template_type = 1;
			template_config_t.template_id = CobTemplateIndex.SelectedIndex + 1;
			if (ChbTemplateAboutROI.Checked)
			{
				template_config_t.config_flag |= 1;
				int result;
				if (int.TryParse(TxbTemplateRoiX.Text, out result))
				{
					template_config_t.roi_config.left = result;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("模板roi x参数非法!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show("Template roi x value illegal!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				if (int.TryParse(TxbTemplateRoiY.Text, out result))
				{
					template_config_t.roi_config.top = result;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("模板roi y参数非法!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show("Template roi y value illegal!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				if (int.TryParse(TxbTemplateRoiW.Text, out result))
				{
					if (result < 64)
					{
						if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
						{
							MessageBox.Show("当前选择的区域太小了(w<64 )，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						else
						{
							MessageBox.Show("The current selection is too small(w<64),Please Reselect", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						return;
					}
					template_config_t.roi_config.width = result;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("模板roi w参数非法!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show("Template roi w value illegal!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				if (int.TryParse(TxbTemplateRoiH.Text, out result))
				{
					if (result < 32)
					{
						if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
						{
							MessageBox.Show("当前选择的区域太小了(h<32)，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						else
						{
							MessageBox.Show("The current selection is too small(h<32),Please Reselect", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						return;
					}
					template_config_t.roi_config.height = result;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("模板roi h参数非法!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show("Template roi h value illegal!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				template_config_t.config_flag &= -2;
			}
			if (ChbTemplateBarcodeType.Checked)
			{
				template_config_t.config_flag |= 4;
				template_config_t.decode_config.decode_lib = tmp.decode_config.decode_lib;
				template_config_t.decode_config.type_switch = 0uL;
				template_config_t.decode_config.type_switch = tmp.decode_config.type_switch;
			}
			else
			{
				template_config_t.config_flag &= -5;
			}
			if (ChbTemplateLightConfig.Checked)
			{
				template_config_t.config_flag |= 8;
				template_config_t.sensor_config.config_flag |= 1;
				template_config_t.template_type = 0;
				template_config_t.sensor_config.light_pwm1 = tmp.sensor_config.light_pwm1;
				template_config_t.sensor_config.light_pwm2 = tmp.sensor_config.light_pwm2;
			}
			else
			{
				template_config_t.sensor_config.config_flag &= -2;
			}
			if (ChbTemplateLightGroup.Checked)
			{
				template_config_t.config_flag |= 8;
				template_config_t.sensor_config.config_flag |= 16;
				template_config_t.sensor_config.light_ctrl = 0;
				template_config_t.template_type = 0;
				template_config_t.sensor_config.light_ctrl = tmp.sensor_config.light_ctrl;
			}
			else
			{
				template_config_t.sensor_config.config_flag &= -17;
			}
			if (ChbTemplateAboutSensor.Checked)
			{
				template_config_t.config_flag |= 8;
				template_config_t.template_type = 0;
				template_config_t.sensor_config.config_flag |= 4;
				if (GetCfgCBFuncCB(7296u) == 0)
				{
					template_config_t.sensor_config.config_flag |= 2;
					template_config_t.sensor_config.exp_time = tmp.sensor_config.exp_time;
				}
				else
				{
					template_config_t.sensor_config.config_flag |= 32;
					template_config_t.sensor_config.exp_time = tmp.sensor_config.exp_time;
				}
				template_config_t.sensor_config.gain_set = tmp.sensor_config.gain_set;
			}
			else
			{
				template_config_t.sensor_config.config_flag &= -5;
				template_config_t.sensor_config.config_flag &= -3;
				template_config_t.sensor_config.config_flag &= -33;
			}
			if (ChbTemplateLensFocus.Checked)
			{
				template_config_t.config_flag |= 8;
				template_config_t.template_type = 0;
				template_config_t.sensor_config.config_flag |= 8;
				template_config_t.sensor_config.focus_set = tmp.sensor_config.focus_set;
			}
			else
			{
				template_config_t.sensor_config.config_flag &= -9;
			}
			if (ChbImagePreProc1.Checked || ChbImagePreProc2.Checked)
			{
				template_config_t.config_flag |= 2;
				template_config_t.img_proc_cnt = 0;
				if (ChbImagePreProc1.Checked)
				{
					template_config_t.imgproc_config[template_config_t.img_proc_cnt].proc_id = CobTemplateIspFunc1.SelectedIndex;
					template_config_t.img_proc_cnt++;
				}
				if (ChbImagePreProc2.Checked)
				{
					template_config_t.imgproc_config[template_config_t.img_proc_cnt].proc_id = CobTemplateIspFunc2.SelectedIndex;
					template_config_t.img_proc_cnt++;
				}
			}
			else
			{
				template_config_t.config_flag &= -3;
			}
			if (ChbTemplateEnable.Checked)
			{
				template_config_t.head_magic = 3405692655u;
			}
			else
			{
				template_config_t.head_magic = 3735943886u;
			}
			byte[] array = ToolCfg.StructToBytes(template_config_t);
			string text = Path.GetTempPath() + "\\" + TxbTemplateName.Text + ".hjt";
			FileStream fileStream = new FileStream(text, FileMode.OpenOrCreate);
			fileStream.Write(array, 0, array.Length);
			fileStream.Close();
			if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK && ToolCfg.CurrentDevice.IsCommunicate)
			{
				if (ChbGroupTemplate.Checked)
				{
					ToolCfg.ftp.UploadFile(text, "ftp://" + ToolCfg.CurrentDevice.IpAddrStr, "/template/" + TxbGroupForderIdx.Text + "-" + TxbGroupForderName.Text, TxbTemplateName.Text + ".hjt");
				}
				else
				{
					ToolCfg.ftp.UploadFile(text, "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template/" + TxbTemplateName.Text + ".hjt");
				}
				SendCfgDataCBFuncCB(32768u);
				string ftpPath = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template";
				ShowAllTemplateFile(ftpPath);
			}
		}

		private void TemplateFileUploadProc(object state)
		{
			string text = (string)state;
			string ftpFile = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template/" + Path.GetFileName(text);
			ToolCfg.ftp.UploadFile(text, ftpFile);
			SendCfgDataCBFuncCB(32768u);
			Invoke((MethodInvoker)delegate
			{
				string ftpPath = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template";
				ShowAllTemplateFile(ftpPath);
			});
		}

		private void ListViewTemplate_DragDrop(object sender, DragEventArgs e)
		{
			int length = ((Array)e.Data.GetData(DataFormats.FileDrop)).Length;
			for (int i = 0; i < length; i++)
			{
				string text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(i).ToString();
				if (text.ToLower().Contains(".hjt"))
				{
					ThreadPool.QueueUserWorkItem(TemplateFileUploadProc, text);
				}
			}
		}

		private void ListViewTemplate_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void ShowTemplateToInterface(template_config_t tmp)
		{
			template_config_t tmp2 = tmp;
			TemplateActFuncCB(ref tmp2, TemplateActionDef.Show_New_Template_Value);
			ChbTemplateEnable.Checked = tmp.head_magic == 3405692655u;
			if (tmp.template_id <= CobTemplateIndex.Items.Count)
			{
				CobTemplateIndex.SelectedIndex = tmp.template_id - 1;
			}
			else
			{
				CobTemplateIndex.SelectedIndex = -1;
			}
			if (((ulong)tmp.config_flag & 1uL) == 1)
			{
				ToolCfg.TemplateRoiX = tmp.roi_config.left;
				ToolCfg.TemplateRoiY = tmp.roi_config.top;
				ToolCfg.TemplateRoiW = tmp.roi_config.width;
				ToolCfg.TemplateRoiH = tmp.roi_config.height;
				ChbTemplateAboutROI.Checked = true;
				TxbTemplateRoiX.Text = tmp.roi_config.left.ToString();
				TxbTemplateRoiY.Text = tmp.roi_config.top.ToString();
				TxbTemplateRoiW.Text = tmp.roi_config.width.ToString();
				TxbTemplateRoiH.Text = tmp.roi_config.height.ToString();
				int num = GetCfgCBFuncCB(7951u);
				if (num < 0 || num > 32)
				{
					num = 3;
				}
				try
				{
					int num2 = tmp.roi_config.left - ToolCfg.width_offset;
					int num3 = tmp.roi_config.top - ToolCfg.height_offset;
					int num4 = tmp.roi_config.width;
					int num5 = tmp.roi_config.height;
					ToolCfg.TemplateRoiList.Clear();
					ToolCfg.TemplateRoiList.Add(new ToolCfg.TempRoiRectInfo(tmp.template_id, tmp.template_id.ToString(), new Rectangle(num2, num3, num4, num5)));
				}
				catch (Exception ex)
				{
					LogRecord.WriteError(ex);
				}
				Application.DoEvents();
			}
			else
			{
				ChbTemplateAboutROI.Checked = false;
			}
			if (((ulong)tmp.config_flag & 8uL) == 8)
			{
				bool @checked = false;
				if (((ulong)tmp.sensor_config.config_flag & 2uL) == 2)
				{
					@checked = true;
					ChbTemplateAboutSensor.Checked = true;
				}
				else
				{
					SetCfgCBFuncCB(7295u, (byte)((tmp.sensor_config.exp_time & 0xFF00) >> 8));
					SetCfgCBFuncCB(7167u, (byte)((uint)tmp.sensor_config.exp_time & 0xFFu));
					SendCfgDataCBFuncCB(0u);
				}
				if (((ulong)tmp.sensor_config.config_flag & 4uL) == 4)
				{
					@checked = true;
				}
				ChbTemplateAboutSensor.Checked = @checked;
				if (((ulong)tmp.sensor_config.config_flag & 1uL) == 1)
				{
					ChbTemplateLightConfig.Checked = true;
				}
				else
				{
					ChbTemplateLightConfig.Checked = false;
				}
				if (((ulong)tmp.sensor_config.config_flag & 0x10uL) == 16)
				{
					ChbTemplateLightGroup.Checked = true;
				}
				else
				{
					ChbTemplateLightGroup.Checked = false;
				}
				if (((ulong)tmp.sensor_config.config_flag & 8uL) == 8)
				{
					ChbTemplateLensFocus.Checked = true;
				}
				else
				{
					ChbTemplateLensFocus.Checked = false;
				}
			}
			else
			{
				ChbTemplateAboutSensor.Checked = false;
				ChbTemplateLightConfig.Checked = false;
				ChbTemplateLightGroup.Checked = false;
				ChbTemplateLensFocus.Checked = false;
			}
			if (((ulong)tmp.config_flag & 2uL) == 2)
			{
				if (tmp.img_proc_cnt < 1)
				{
					ChbImagePreProc1.Checked = false;
					ChbImagePreProc2.Checked = false;
					CobTemplateIspFunc1.SelectedIndex = -1;
					CobTemplateIspFunc2.SelectedIndex = -1;
				}
				else if (tmp.img_proc_cnt == 1)
				{
					ChbImagePreProc1.Checked = true;
					ChbImagePreProc2.Checked = false;
					CobTemplateIspFunc1.SelectedIndex = tmp.imgproc_config[0].proc_id;
					CobTemplateIspFunc2.SelectedIndex = -1;
				}
				else
				{
					ChbImagePreProc1.Checked = true;
					ChbImagePreProc2.Checked = true;
					CobTemplateIspFunc1.SelectedIndex = tmp.imgproc_config[0].proc_id;
					CobTemplateIspFunc2.SelectedIndex = tmp.imgproc_config[1].proc_id;
				}
			}
			else
			{
				ChbImagePreProc1.Checked = false;
				ChbImagePreProc2.Checked = false;
			}
			if (((ulong)tmp.config_flag & 4uL) == 4)
			{
				ChbTemplateBarcodeType.Checked = true;
			}
			else
			{
				ChbTemplateBarcodeType.Checked = false;
			}
		}

		private void ListViewTemplate_ItemActivate(object sender, EventArgs e)
		{
			if (ListViewTemplate.SelectedItems.Count <= 0)
			{
				return;
			}
			DirItemInfo dirItemInfo = (DirItemInfo)ListViewTemplate.SelectedItems[0].Tag;
			string text = dirItemInfo.FullFileName.ToString();
			try
			{
				if (dirItemInfo.FileType == 0)
				{
					int num = 0;
					string text2 = ListViewTemplate.SelectedItems[0].Text;
					for (int i = 0; i < 4; i++)
					{
						if (text2[i] < '0' || text2[i] > '9')
						{
							num = i;
							break;
						}
					}
					if (num > 0)
					{
						TxbGroupForderIdx.Text = text2.Substring(0, num);
						TxbGroupForderName.Text = text2.Substring(num + 1);
					}
					ShowAllTemplateFile(text);
				}
				else if (Path.GetExtension(text) == ".hjt")
				{
					byte[] array = ToolCfg.ftp.DownloadFile(text);
					template_config_t template_config_t = default(template_config_t);
					template_config_t.imgproc_config = new imgproc_config_t[CobTemplateIndex.Items.Count];
					if (array.Length == Marshal.SizeOf(template_config_t))
					{
						template_config_t = (template_config_t)ToolCfg.BytesToStruct(array, template_config_t.GetType());
						TxbTemplateName.Text = Path.GetFileNameWithoutExtension(dirItemInfo.Name);
						ShowTemplateToInterface(template_config_t);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void BtnGetTempPara_Click(object sender, EventArgs e)
		{
			TxbMainLightDisp.Text = ToolCfg.Temp_MainLight.ToString();
			TxbSecondLightDisp.Text = ToolCfg.Temp_SecondLight.ToString();
			TxbSensorExpDisp.Text = ToolCfg.Temp_ExpVal.ToString();
			TxbSensorGainDisp.Text = ToolCfg.Temp_GainVal.ToString();
		}

		private void Cms_Template_Opening(object sender, CancelEventArgs e)
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				TsmTemplateDelete.Text = "删除";
				TsmTemplateUpload.Text = "上传";
				TsmTemplateDownload.Text = "下载";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				TsmTemplateDelete.Text = "刪除";
				TsmTemplateUpload.Text = "上傳";
				TsmTemplateDownload.Text = "下載";
			}
			else
			{
				TsmTemplateDelete.Text = "Delete ";
				TsmTemplateUpload.Text = "Upload ";
				TsmTemplateDownload.Text = "Download ";
			}
		}

		private void TsmTemplateDelete_Click(object sender, EventArgs e)
		{
			if (ListViewTemplate.SelectedItems.Count <= 0)
			{
				return;
			}
			DialogResult dialogResult = ((!ToolCfg.ConfigPath.Contains("ChineseS") && !ToolCfg.ConfigPath.Contains("ChineseT")) ? MessageBox.Show("Are you sure you want to delete it?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) : MessageBox.Show("确定要删除吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation));
			if (dialogResult == DialogResult.No)
			{
				return;
			}
			try
			{
				foreach (ListViewItem selectedItem in ListViewTemplate.SelectedItems)
				{
					DirItemInfo dirItemInfo = (DirItemInfo)ListViewTemplate.SelectedItems[0].Tag;
					string text = dirItemInfo.FullFileName.ToString();
					if (dirItemInfo.FileType == 1)
					{
						ToolCfg.ftp.DeleteFile(dirItemInfo.FullFileName.Replace(dirItemInfo.Name, ""), dirItemInfo.Name);
					}
					else if (dirItemInfo.FileType == 0)
					{
						ToolCfg.ftp.DeleteDir(dirItemInfo.FullFileName.Replace(dirItemInfo.Name, ""), dirItemInfo.Name);
					}
					ListViewTemplate.Items.Remove(selectedItem);
				}
				SendCfgDataCBFuncCB(32768u);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void TsmTemplateUpload_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK || ToolCfg.CurrentDevice == null || !ToolCfg.CurrentDevice.IsConnect)
			{
				return;
			}
			string[] fileNames = openFileDialog.FileNames;
			foreach (string text in fileNames)
			{
				if (text.ToLower().Contains(".hjt"))
				{
					ThreadPool.QueueUserWorkItem(TemplateFileUploadProc, text);
				}
			}
		}

		private void TsmTemplateDownload_Click(object sender, EventArgs e)
		{
			if (ListViewTemplate.SelectedItems.Count <= 0 || ToolCfg.CurrentDevice == null || !ToolCfg.CurrentDevice.IsConnect)
			{
				return;
			}
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			try
			{
				foreach (ListViewItem selectedItem in ListViewTemplate.SelectedItems)
				{
					DirItemInfo dirItemInfo = (DirItemInfo)selectedItem.Tag;
					string fileName = dirItemInfo.FullFileName.ToString();
					string path = folderBrowserDialog.SelectedPath + "\\" + dirItemInfo.Name;
					if (dirItemInfo.FileType == 1)
					{
						byte[] array = ToolCfg.ftp.DownloadFile(fileName);
						if (array != null)
						{
							FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
							fileStream.Write(array, 0, array.Length);
							fileStream.Flush();
							fileStream.Close();
						}
						else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
						{
							MessageBox.Show("文件保存失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						else
						{
							MessageBox.Show("File save failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void BtnDrawMultRoi_Click(object sender, EventArgs e)
		{
			try
			{
				if (ToolCfg.TemplateImg != null)
				{
					int num = GetCfgCBFuncCB(7951u);
					if (num <= 0 || num > 32)
					{
						num = 3;
					}
					MultRoiSettingForm multRoiSettingForm = new MultRoiSettingForm();
					multRoiSettingForm.TemplateImage = (Image)ToolCfg.TemplateImg.Clone();
					multRoiSettingForm.ImgZoom = num;
					multRoiSettingForm.IsTemplateEnable = ChbTemplateEnable.Checked;
					multRoiSettingForm.IsGroupTemplate = ChbGroupTemplate.Checked;
					multRoiSettingForm.GroupForderIdx = TxbGroupForderIdx.Text;
					multRoiSettingForm.GroupForderName = TxbGroupForderName.Text;
					multRoiSettingForm.ShowDialog();
					SendCfgDataCBFuncCB(32768u);
					string ftpPath = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template";
					ShowAllTemplateFile(ftpPath);
				}
				else
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TempCaptureTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TempCaptureTips), MessageBoxButtons.OK);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
			}
		}

		protected override void Dispose(bool disposing)
		{
			Hide();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Heroje_Debug_Tool.SubForm.TemplateSettingForm));
			this.GrbNewTemplate = new System.Windows.Forms.GroupBox();
			this.BtnDrawMultRoi = new System.Windows.Forms.Button();
			this.BtnDrawTemplateRoi = new System.Windows.Forms.Button();
			this.LabLineSeparator = new System.Windows.Forms.Label();
			this.TxbGroupForderIdx = new System.Windows.Forms.TextBox();
			this.TxbGroupForderName = new System.Windows.Forms.TextBox();
			this.LabGroupForder = new System.Windows.Forms.Label();
			this.BtnGroupTemplateSet = new System.Windows.Forms.Button();
			this.TxbGroupTemplateSet = new System.Windows.Forms.TextBox();
			this.ChbGroupTemplate = new System.Windows.Forms.CheckBox();
			this.CobTemplateIspFunc2 = new System.Windows.Forms.ComboBox();
			this.CobTemplateIndex = new System.Windows.Forms.ComboBox();
			this.ChbTemplateLightGroup = new System.Windows.Forms.CheckBox();
			this.ChbTemplateEnable = new System.Windows.Forms.CheckBox();
			this.CobTemplateIspFunc1 = new System.Windows.Forms.ComboBox();
			this.ChbImagePreProc2 = new System.Windows.Forms.CheckBox();
			this.ChbImagePreProc1 = new System.Windows.Forms.CheckBox();
			this.CobTemplateOutputLogic = new System.Windows.Forms.ComboBox();
			this.LabTemplateOutputLogic = new System.Windows.Forms.Label();
			this.ChbTemplateLensFocus = new System.Windows.Forms.CheckBox();
			this.PanTemplateRoi = new System.Windows.Forms.Panel();
			this.TxbTemplateRoiH = new System.Windows.Forms.TextBox();
			this.TxbTemplateRoiW = new System.Windows.Forms.TextBox();
			this.TxbTemplateRoiY = new System.Windows.Forms.TextBox();
			this.TxbTemplateRoiX = new System.Windows.Forms.TextBox();
			this.LabTempH = new System.Windows.Forms.Label();
			this.LabTempW = new System.Windows.Forms.Label();
			this.LabTempY = new System.Windows.Forms.Label();
			this.LabTempX = new System.Windows.Forms.Label();
			this.ChbTemplateLightConfig = new System.Windows.Forms.CheckBox();
			this.ChbTemplateAboutROI = new System.Windows.Forms.CheckBox();
			this.ChbTemplateAboutSensor = new System.Windows.Forms.CheckBox();
			this.LabTemplateIndex = new System.Windows.Forms.Label();
			this.ChbTemplateBarcodeType = new System.Windows.Forms.CheckBox();
			this.GrbTemplateOp = new System.Windows.Forms.GroupBox();
			this.BtnReadTemplate1 = new System.Windows.Forms.Button();
			this.BtnReadTemplate2 = new System.Windows.Forms.Button();
			this.BtnReadTemplate3 = new System.Windows.Forms.Button();
			this.BtnSaveTemplate3 = new System.Windows.Forms.Button();
			this.BtnSaveTemplate1 = new System.Windows.Forms.Button();
			this.BtnSaveTemplate2 = new System.Windows.Forms.Button();
			this.GrbTemplateFiles = new System.Windows.Forms.GroupBox();
			this.ChbTempNameUseIdx = new System.Windows.Forms.CheckBox();
			this.ListViewTemplate = new System.Windows.Forms.ListView();
			this.Cms_Template = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.TsmTemplateDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTemplateUpload = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTemplateDownload = new System.Windows.Forms.ToolStripMenuItem();
			this.ImgListTemplate = new System.Windows.Forms.ImageList(this.components);
			this.LabTemplateName = new System.Windows.Forms.Label();
			this.TxbTemplateName = new System.Windows.Forms.TextBox();
			this.BtnTemplateSave = new System.Windows.Forms.Button();
			this.BtnReadAllTemplate = new System.Windows.Forms.Button();
			this.GrbTemplateSetting = new System.Windows.Forms.GroupBox();
			this.BtnGetTempPara = new System.Windows.Forms.Button();
			this.ChbEnableTempFloop = new System.Windows.Forms.CheckBox();
			this.CobTempDPMLevel = new System.Windows.Forms.ComboBox();
			this.LabTempDPMLevel = new System.Windows.Forms.Label();
			this.TxbSensorGainDisp = new System.Windows.Forms.TextBox();
			this.LabSensorGainDisp = new System.Windows.Forms.Label();
			this.TxbSensorExpDisp = new System.Windows.Forms.TextBox();
			this.LabSensorExpDisp = new System.Windows.Forms.Label();
			this.TxbSecondLightDisp = new System.Windows.Forms.TextBox();
			this.LabSecondLightDisp = new System.Windows.Forms.Label();
			this.TxbMainLightDisp = new System.Windows.Forms.TextBox();
			this.LabMainLightDisp = new System.Windows.Forms.Label();
			this.ucSplitLine_H1 = new HJ_Controls_Lib.UCSplitLine_H();
			this.GrbNewTemplate.SuspendLayout();
			this.PanTemplateRoi.SuspendLayout();
			this.GrbTemplateOp.SuspendLayout();
			this.GrbTemplateFiles.SuspendLayout();
			this.Cms_Template.SuspendLayout();
			this.GrbTemplateSetting.SuspendLayout();
			base.SuspendLayout();
			this.GrbNewTemplate.Controls.Add(this.BtnDrawMultRoi);
			this.GrbNewTemplate.Controls.Add(this.BtnDrawTemplateRoi);
			this.GrbNewTemplate.Controls.Add(this.LabLineSeparator);
			this.GrbNewTemplate.Controls.Add(this.TxbGroupForderIdx);
			this.GrbNewTemplate.Controls.Add(this.TxbGroupForderName);
			this.GrbNewTemplate.Controls.Add(this.LabGroupForder);
			this.GrbNewTemplate.Controls.Add(this.BtnGroupTemplateSet);
			this.GrbNewTemplate.Controls.Add(this.TxbGroupTemplateSet);
			this.GrbNewTemplate.Controls.Add(this.ChbGroupTemplate);
			this.GrbNewTemplate.Controls.Add(this.CobTemplateIspFunc2);
			this.GrbNewTemplate.Controls.Add(this.CobTemplateIndex);
			this.GrbNewTemplate.Controls.Add(this.ChbTemplateLightGroup);
			this.GrbNewTemplate.Controls.Add(this.ChbTemplateEnable);
			this.GrbNewTemplate.Controls.Add(this.CobTemplateIspFunc1);
			this.GrbNewTemplate.Controls.Add(this.ChbImagePreProc2);
			this.GrbNewTemplate.Controls.Add(this.ChbImagePreProc1);
			this.GrbNewTemplate.Controls.Add(this.CobTemplateOutputLogic);
			this.GrbNewTemplate.Controls.Add(this.LabTemplateOutputLogic);
			this.GrbNewTemplate.Controls.Add(this.ChbTemplateLensFocus);
			this.GrbNewTemplate.Controls.Add(this.PanTemplateRoi);
			this.GrbNewTemplate.Controls.Add(this.ChbTemplateLightConfig);
			this.GrbNewTemplate.Controls.Add(this.ChbTemplateAboutROI);
			this.GrbNewTemplate.Controls.Add(this.ChbTemplateAboutSensor);
			this.GrbNewTemplate.Controls.Add(this.LabTemplateIndex);
			this.GrbNewTemplate.Controls.Add(this.ChbTemplateBarcodeType);
			this.GrbNewTemplate.Location = new System.Drawing.Point(2, 208);
			this.GrbNewTemplate.Name = "GrbNewTemplate";
			this.GrbNewTemplate.Size = new System.Drawing.Size(458, 255);
			this.GrbNewTemplate.TabIndex = 7;
			this.GrbNewTemplate.TabStop = false;
			this.GrbNewTemplate.Text = "模板设置(新)";
			this.BtnDrawMultRoi.Location = new System.Drawing.Point(37, 219);
			this.BtnDrawMultRoi.Name = "BtnDrawMultRoi";
			this.BtnDrawMultRoi.Size = new System.Drawing.Size(118, 30);
			this.BtnDrawMultRoi.TabIndex = 73;
			this.BtnDrawMultRoi.Text = "制作多ROI";
			this.BtnDrawMultRoi.UseVisualStyleBackColor = true;
			this.BtnDrawMultRoi.Click += new System.EventHandler(BtnDrawMultRoi_Click);
			this.BtnDrawTemplateRoi.Location = new System.Drawing.Point(54, 87);
			this.BtnDrawTemplateRoi.Name = "BtnDrawTemplateRoi";
			this.BtnDrawTemplateRoi.Size = new System.Drawing.Size(116, 30);
			this.BtnDrawTemplateRoi.TabIndex = 72;
			this.BtnDrawTemplateRoi.Text = "开始绘制ROI";
			this.BtnDrawTemplateRoi.UseVisualStyleBackColor = true;
			this.BtnDrawTemplateRoi.Click += new System.EventHandler(BtnDrawTemplateRoi_Click);
			this.LabLineSeparator.AutoSize = true;
			this.LabLineSeparator.Location = new System.Drawing.Point(331, 200);
			this.LabLineSeparator.Name = "LabLineSeparator";
			this.LabLineSeparator.Size = new System.Drawing.Size(14, 14);
			this.LabLineSeparator.TabIndex = 71;
			this.LabLineSeparator.Text = "-";
			this.TxbGroupForderIdx.Location = new System.Drawing.Point(308, 196);
			this.TxbGroupForderIdx.MaxLength = 2;
			this.TxbGroupForderIdx.Name = "TxbGroupForderIdx";
			this.TxbGroupForderIdx.Size = new System.Drawing.Size(22, 23);
			this.TxbGroupForderIdx.TabIndex = 70;
			this.TxbGroupForderIdx.Text = "1";
			this.TxbGroupForderName.Location = new System.Drawing.Point(345, 196);
			this.TxbGroupForderName.MaxLength = 16;
			this.TxbGroupForderName.Name = "TxbGroupForderName";
			this.TxbGroupForderName.Size = new System.Drawing.Size(53, 23);
			this.TxbGroupForderName.TabIndex = 69;
			this.TxbGroupForderName.Text = "Group";
			this.LabGroupForder.AutoSize = true;
			this.LabGroupForder.Location = new System.Drawing.Point(245, 200);
			this.LabGroupForder.Name = "LabGroupForder";
			this.LabGroupForder.Size = new System.Drawing.Size(56, 14);
			this.LabGroupForder.TabIndex = 68;
			this.LabGroupForder.Text = "创建组:";
			this.BtnGroupTemplateSet.Location = new System.Drawing.Point(189, 196);
			this.BtnGroupTemplateSet.Name = "BtnGroupTemplateSet";
			this.BtnGroupTemplateSet.Size = new System.Drawing.Size(47, 23);
			this.BtnGroupTemplateSet.TabIndex = 66;
			this.BtnGroupTemplateSet.Text = "确认";
			this.BtnGroupTemplateSet.UseVisualStyleBackColor = true;
			this.BtnGroupTemplateSet.Visible = false;
			this.BtnGroupTemplateSet.Click += new System.EventHandler(BtnGroupTemplateSet_Click);
			this.TxbGroupTemplateSet.Location = new System.Drawing.Point(159, 196);
			this.TxbGroupTemplateSet.MaxLength = 2;
			this.TxbGroupTemplateSet.Name = "TxbGroupTemplateSet";
			this.TxbGroupTemplateSet.Size = new System.Drawing.Size(30, 23);
			this.TxbGroupTemplateSet.TabIndex = 67;
			this.TxbGroupTemplateSet.TextChanged += new System.EventHandler(TxbGroupTemplateSet_TextChanged);
			this.ChbGroupTemplate.AutoSize = true;
			this.ChbGroupTemplate.Location = new System.Drawing.Point(37, 198);
			this.ChbGroupTemplate.Name = "ChbGroupTemplate";
			this.ChbGroupTemplate.Size = new System.Drawing.Size(117, 18);
			this.ChbGroupTemplate.TabIndex = 65;
			this.ChbGroupTemplate.Text = "分组模板模式:";
			this.ChbGroupTemplate.UseVisualStyleBackColor = true;
			this.ChbGroupTemplate.CheckedChanged += new System.EventHandler(ChbGroupTemplate_CheckedChanged);
			this.CobTemplateIspFunc2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobTemplateIspFunc2.FormattingEnabled = true;
			this.CobTemplateIspFunc2.Items.AddRange(new object[8] { "高斯滤波", "形态学膨胀", "形态学腐蚀", "对比度增强", "图像反色", "图像镜像", "中值滤波", "双边滤波" });
			this.CobTemplateIspFunc2.Location = new System.Drawing.Point(161, 172);
			this.CobTemplateIspFunc2.Name = "CobTemplateIspFunc2";
			this.CobTemplateIspFunc2.Size = new System.Drawing.Size(140, 22);
			this.CobTemplateIspFunc2.TabIndex = 57;
			this.CobTemplateIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobTemplateIndex.FormattingEnabled = true;
			this.CobTemplateIndex.Items.AddRange(new object[20]
			{
				"1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
				"11", "12", "13", "14", "15", "16", "17", "18", "19", "20"
			});
			this.CobTemplateIndex.Location = new System.Drawing.Point(394, 148);
			this.CobTemplateIndex.Name = "CobTemplateIndex";
			this.CobTemplateIndex.Size = new System.Drawing.Size(48, 22);
			this.CobTemplateIndex.TabIndex = 55;
			this.CobTemplateIndex.SelectedIndexChanged += new System.EventHandler(CobTemplateIndex_SelectedIndexChanged);
			this.ChbTemplateLightGroup.AutoSize = true;
			this.ChbTemplateLightGroup.Location = new System.Drawing.Point(36, 44);
			this.ChbTemplateLightGroup.Name = "ChbTemplateLightGroup";
			this.ChbTemplateLightGroup.Size = new System.Drawing.Size(138, 18);
			this.ChbTemplateLightGroup.TabIndex = 54;
			this.ChbTemplateLightGroup.Text = "照明分组开关控制";
			this.ChbTemplateLightGroup.UseVisualStyleBackColor = true;
			this.ChbTemplateEnable.AutoSize = true;
			this.ChbTemplateEnable.ForeColor = System.Drawing.Color.Blue;
			this.ChbTemplateEnable.Location = new System.Drawing.Point(345, 174);
			this.ChbTemplateEnable.Name = "ChbTemplateEnable";
			this.ChbTemplateEnable.Size = new System.Drawing.Size(82, 18);
			this.ChbTemplateEnable.TabIndex = 53;
			this.ChbTemplateEnable.Text = "启用模板";
			this.ChbTemplateEnable.UseVisualStyleBackColor = true;
			this.CobTemplateIspFunc1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobTemplateIspFunc1.FormattingEnabled = true;
			this.CobTemplateIspFunc1.Items.AddRange(new object[8] { "高斯滤波", "形态学膨胀", "形态学腐蚀", "对比度增强", "图像反色", "图像镜像", "中值滤波", "维纳滤波" });
			this.CobTemplateIspFunc1.Location = new System.Drawing.Point(160, 147);
			this.CobTemplateIspFunc1.Name = "CobTemplateIspFunc1";
			this.CobTemplateIspFunc1.Size = new System.Drawing.Size(140, 22);
			this.CobTemplateIspFunc1.TabIndex = 50;
			this.ChbImagePreProc2.AutoSize = true;
			this.ChbImagePreProc2.Location = new System.Drawing.Point(36, 174);
			this.ChbImagePreProc2.Name = "ChbImagePreProc2";
			this.ChbImagePreProc2.Size = new System.Drawing.Size(131, 18);
			this.ChbImagePreProc2.TabIndex = 49;
			this.ChbImagePreProc2.Text = "图像算法处理2：";
			this.ChbImagePreProc2.UseVisualStyleBackColor = true;
			this.ChbImagePreProc1.AutoSize = true;
			this.ChbImagePreProc1.Location = new System.Drawing.Point(36, 151);
			this.ChbImagePreProc1.Name = "ChbImagePreProc1";
			this.ChbImagePreProc1.Size = new System.Drawing.Size(131, 18);
			this.ChbImagePreProc1.TabIndex = 48;
			this.ChbImagePreProc1.Text = "图像算法处理1：";
			this.ChbImagePreProc1.UseVisualStyleBackColor = true;
			this.CobTemplateOutputLogic.FormattingEnabled = true;
			this.CobTemplateOutputLogic.Items.AddRange(new object[5] { "解一个就退(输出一个)", "解至少一个退(NG不输出)", "解至少一个退(NG输出)", "解全部才退出(调试用)", "解全部才退出(实际用)" });
			this.CobTemplateOutputLogic.Location = new System.Drawing.Point(150, 123);
			this.CobTemplateOutputLogic.Name = "CobTemplateOutputLogic";
			this.CobTemplateOutputLogic.Size = new System.Drawing.Size(185, 22);
			this.CobTemplateOutputLogic.TabIndex = 47;
			this.CobTemplateOutputLogic.SelectedIndexChanged += new System.EventHandler(CobTemplateOutputLogic_SelectedIndexChanged);
			this.LabTemplateOutputLogic.AutoSize = true;
			this.LabTemplateOutputLogic.Location = new System.Drawing.Point(33, 127);
			this.LabTemplateOutputLogic.Name = "LabTemplateOutputLogic";
			this.LabTemplateOutputLogic.Size = new System.Drawing.Size(98, 14);
			this.LabTemplateOutputLogic.TabIndex = 46;
			this.LabTemplateOutputLogic.Text = "模板运行逻辑:";
			this.ChbTemplateLensFocus.AutoSize = true;
			this.ChbTemplateLensFocus.Location = new System.Drawing.Point(204, 23);
			this.ChbTemplateLensFocus.Name = "ChbTemplateLensFocus";
			this.ChbTemplateLensFocus.Size = new System.Drawing.Size(180, 18);
			this.ChbTemplateLensFocus.TabIndex = 45;
			this.ChbTemplateLensFocus.Text = "摄像头焦距(当前的焦距)";
			this.ChbTemplateLensFocus.UseVisualStyleBackColor = true;
			this.PanTemplateRoi.Controls.Add(this.TxbTemplateRoiH);
			this.PanTemplateRoi.Controls.Add(this.TxbTemplateRoiW);
			this.PanTemplateRoi.Controls.Add(this.TxbTemplateRoiY);
			this.PanTemplateRoi.Controls.Add(this.TxbTemplateRoiX);
			this.PanTemplateRoi.Controls.Add(this.LabTempH);
			this.PanTemplateRoi.Controls.Add(this.LabTempW);
			this.PanTemplateRoi.Controls.Add(this.LabTempY);
			this.PanTemplateRoi.Controls.Add(this.LabTempX);
			this.PanTemplateRoi.Location = new System.Drawing.Point(176, 89);
			this.PanTemplateRoi.Name = "PanTemplateRoi";
			this.PanTemplateRoi.Size = new System.Drawing.Size(270, 28);
			this.PanTemplateRoi.TabIndex = 44;
			this.TxbTemplateRoiH.Location = new System.Drawing.Point(224, 2);
			this.TxbTemplateRoiH.Name = "TxbTemplateRoiH";
			this.TxbTemplateRoiH.ReadOnly = true;
			this.TxbTemplateRoiH.Size = new System.Drawing.Size(41, 23);
			this.TxbTemplateRoiH.TabIndex = 43;
			this.TxbTemplateRoiH.Text = "800";
			this.TxbTemplateRoiW.Location = new System.Drawing.Point(157, 2);
			this.TxbTemplateRoiW.Name = "TxbTemplateRoiW";
			this.TxbTemplateRoiW.ReadOnly = true;
			this.TxbTemplateRoiW.Size = new System.Drawing.Size(41, 23);
			this.TxbTemplateRoiW.TabIndex = 42;
			this.TxbTemplateRoiW.Text = "1280";
			this.TxbTemplateRoiY.Location = new System.Drawing.Point(92, 2);
			this.TxbTemplateRoiY.Name = "TxbTemplateRoiY";
			this.TxbTemplateRoiY.ReadOnly = true;
			this.TxbTemplateRoiY.Size = new System.Drawing.Size(41, 23);
			this.TxbTemplateRoiY.TabIndex = 41;
			this.TxbTemplateRoiY.Text = "0";
			this.TxbTemplateRoiX.Location = new System.Drawing.Point(23, 2);
			this.TxbTemplateRoiX.Name = "TxbTemplateRoiX";
			this.TxbTemplateRoiX.ReadOnly = true;
			this.TxbTemplateRoiX.Size = new System.Drawing.Size(41, 23);
			this.TxbTemplateRoiX.TabIndex = 40;
			this.TxbTemplateRoiX.Text = "0";
			this.LabTempH.AutoSize = true;
			this.LabTempH.Location = new System.Drawing.Point(205, 6);
			this.LabTempH.Name = "LabTempH";
			this.LabTempH.Size = new System.Drawing.Size(21, 14);
			this.LabTempH.TabIndex = 39;
			this.LabTempH.Text = "H:";
			this.LabTempW.AutoSize = true;
			this.LabTempW.Location = new System.Drawing.Point(138, 6);
			this.LabTempW.Name = "LabTempW";
			this.LabTempW.Size = new System.Drawing.Size(21, 14);
			this.LabTempW.TabIndex = 38;
			this.LabTempW.Text = "W:";
			this.LabTempY.AutoSize = true;
			this.LabTempY.Location = new System.Drawing.Point(71, 6);
			this.LabTempY.Name = "LabTempY";
			this.LabTempY.Size = new System.Drawing.Size(21, 14);
			this.LabTempY.TabIndex = 37;
			this.LabTempY.Text = "Y:";
			this.LabTempX.AutoSize = true;
			this.LabTempX.Location = new System.Drawing.Point(5, 6);
			this.LabTempX.Name = "LabTempX";
			this.LabTempX.Size = new System.Drawing.Size(21, 14);
			this.LabTempX.TabIndex = 36;
			this.LabTempX.Text = "X:";
			this.ChbTemplateLightConfig.AutoSize = true;
			this.ChbTemplateLightConfig.Location = new System.Drawing.Point(204, 44);
			this.ChbTemplateLightConfig.Name = "ChbTemplateLightConfig";
			this.ChbTemplateLightConfig.Size = new System.Drawing.Size(194, 18);
			this.ChbTemplateLightConfig.TabIndex = 2;
			this.ChbTemplateLightConfig.Text = "照明亮度控制(照明栏设置)";
			this.ChbTemplateLightConfig.UseVisualStyleBackColor = true;
			this.ChbTemplateAboutROI.AutoSize = true;
			this.ChbTemplateAboutROI.Location = new System.Drawing.Point(36, 66);
			this.ChbTemplateAboutROI.Name = "ChbTemplateAboutROI";
			this.ChbTemplateAboutROI.Size = new System.Drawing.Size(103, 18);
			this.ChbTemplateAboutROI.TabIndex = 1;
			this.ChbTemplateAboutROI.Text = "图像ROI开启";
			this.ChbTemplateAboutROI.UseVisualStyleBackColor = true;
			this.ChbTemplateAboutROI.CheckedChanged += new System.EventHandler(ChbTemplateAboutROI_CheckedChanged);
			this.ChbTemplateAboutSensor.AutoSize = true;
			this.ChbTemplateAboutSensor.Location = new System.Drawing.Point(36, 23);
			this.ChbTemplateAboutSensor.Name = "ChbTemplateAboutSensor";
			this.ChbTemplateAboutSensor.Size = new System.Drawing.Size(166, 18);
			this.ChbTemplateAboutSensor.TabIndex = 0;
			this.ChbTemplateAboutSensor.Text = "传感器曝光和增益配置";
			this.ChbTemplateAboutSensor.UseVisualStyleBackColor = true;
			this.LabTemplateIndex.AutoSize = true;
			this.LabTemplateIndex.Location = new System.Drawing.Point(339, 152);
			this.LabTemplateIndex.Name = "LabTemplateIndex";
			this.LabTemplateIndex.Size = new System.Drawing.Size(56, 14);
			this.LabTemplateIndex.TabIndex = 56;
			this.LabTemplateIndex.Text = "模板号:";
			this.ChbTemplateBarcodeType.AutoSize = true;
			this.ChbTemplateBarcodeType.Location = new System.Drawing.Point(204, 65);
			this.ChbTemplateBarcodeType.Name = "ChbTemplateBarcodeType";
			this.ChbTemplateBarcodeType.Size = new System.Drawing.Size(82, 18);
			this.ChbTemplateBarcodeType.TabIndex = 52;
			this.ChbTemplateBarcodeType.Text = "解码配置";
			this.ChbTemplateBarcodeType.UseVisualStyleBackColor = true;
			this.GrbTemplateOp.Controls.Add(this.BtnReadTemplate1);
			this.GrbTemplateOp.Controls.Add(this.BtnReadTemplate2);
			this.GrbTemplateOp.Controls.Add(this.BtnReadTemplate3);
			this.GrbTemplateOp.Controls.Add(this.BtnSaveTemplate3);
			this.GrbTemplateOp.Controls.Add(this.BtnSaveTemplate1);
			this.GrbTemplateOp.Controls.Add(this.BtnSaveTemplate2);
			this.GrbTemplateOp.Location = new System.Drawing.Point(2, 112);
			this.GrbTemplateOp.Name = "GrbTemplateOp";
			this.GrbTemplateOp.Size = new System.Drawing.Size(458, 82);
			this.GrbTemplateOp.TabIndex = 6;
			this.GrbTemplateOp.TabStop = false;
			this.GrbTemplateOp.Text = "模板操作(老)";
			this.BtnReadTemplate1.Location = new System.Drawing.Point(36, 21);
			this.BtnReadTemplate1.Name = "BtnReadTemplate1";
			this.BtnReadTemplate1.Size = new System.Drawing.Size(105, 27);
			this.BtnReadTemplate1.TabIndex = 0;
			this.BtnReadTemplate1.Text = "读取模板1";
			this.BtnReadTemplate1.UseVisualStyleBackColor = true;
			this.BtnReadTemplate1.Click += new System.EventHandler(BtnReadTemplate1_Click);
			this.BtnReadTemplate2.Location = new System.Drawing.Point(163, 21);
			this.BtnReadTemplate2.Name = "BtnReadTemplate2";
			this.BtnReadTemplate2.Size = new System.Drawing.Size(105, 27);
			this.BtnReadTemplate2.TabIndex = 1;
			this.BtnReadTemplate2.Text = "读取模板2";
			this.BtnReadTemplate2.UseVisualStyleBackColor = true;
			this.BtnReadTemplate2.Click += new System.EventHandler(BtnReadTemplate2_Click);
			this.BtnReadTemplate3.Location = new System.Drawing.Point(290, 21);
			this.BtnReadTemplate3.Name = "BtnReadTemplate3";
			this.BtnReadTemplate3.Size = new System.Drawing.Size(105, 27);
			this.BtnReadTemplate3.TabIndex = 2;
			this.BtnReadTemplate3.Text = "读取模板3";
			this.BtnReadTemplate3.UseVisualStyleBackColor = true;
			this.BtnReadTemplate3.Click += new System.EventHandler(BtnReadTemplate3_Click);
			this.BtnSaveTemplate3.Location = new System.Drawing.Point(290, 51);
			this.BtnSaveTemplate3.Name = "BtnSaveTemplate3";
			this.BtnSaveTemplate3.Size = new System.Drawing.Size(105, 27);
			this.BtnSaveTemplate3.TabIndex = 5;
			this.BtnSaveTemplate3.Text = "保存模板3";
			this.BtnSaveTemplate3.UseVisualStyleBackColor = true;
			this.BtnSaveTemplate3.Click += new System.EventHandler(BtnSaveTemplate3_Click);
			this.BtnSaveTemplate1.Location = new System.Drawing.Point(36, 51);
			this.BtnSaveTemplate1.Name = "BtnSaveTemplate1";
			this.BtnSaveTemplate1.Size = new System.Drawing.Size(105, 27);
			this.BtnSaveTemplate1.TabIndex = 3;
			this.BtnSaveTemplate1.Text = "保存模板1";
			this.BtnSaveTemplate1.UseVisualStyleBackColor = true;
			this.BtnSaveTemplate1.Click += new System.EventHandler(BtnSaveTemplate1_Click);
			this.BtnSaveTemplate2.Location = new System.Drawing.Point(163, 51);
			this.BtnSaveTemplate2.Name = "BtnSaveTemplate2";
			this.BtnSaveTemplate2.Size = new System.Drawing.Size(105, 27);
			this.BtnSaveTemplate2.TabIndex = 4;
			this.BtnSaveTemplate2.Text = "保存模板2";
			this.BtnSaveTemplate2.UseVisualStyleBackColor = true;
			this.BtnSaveTemplate2.Click += new System.EventHandler(BtnSaveTemplate2_Click);
			this.GrbTemplateFiles.Controls.Add(this.ChbTempNameUseIdx);
			this.GrbTemplateFiles.Controls.Add(this.ListViewTemplate);
			this.GrbTemplateFiles.Controls.Add(this.LabTemplateName);
			this.GrbTemplateFiles.Controls.Add(this.TxbTemplateName);
			this.GrbTemplateFiles.Controls.Add(this.BtnTemplateSave);
			this.GrbTemplateFiles.Controls.Add(this.BtnReadAllTemplate);
			this.GrbTemplateFiles.Location = new System.Drawing.Point(3, 469);
			this.GrbTemplateFiles.Name = "GrbTemplateFiles";
			this.GrbTemplateFiles.Size = new System.Drawing.Size(457, 176);
			this.GrbTemplateFiles.TabIndex = 5;
			this.GrbTemplateFiles.TabStop = false;
			this.GrbTemplateFiles.Text = "模板文件列表(新)";
			this.ChbTempNameUseIdx.AutoSize = true;
			this.ChbTempNameUseIdx.Checked = true;
			this.ChbTempNameUseIdx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbTempNameUseIdx.Location = new System.Drawing.Point(248, 23);
			this.ChbTempNameUseIdx.Name = "ChbTempNameUseIdx";
			this.ChbTempNameUseIdx.Size = new System.Drawing.Size(68, 18);
			this.ChbTempNameUseIdx.TabIndex = 58;
			this.ChbTempNameUseIdx.Text = "模板号";
			this.ChbTempNameUseIdx.UseVisualStyleBackColor = true;
			this.ListViewTemplate.AllowDrop = true;
			this.ListViewTemplate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.ListViewTemplate.ContextMenuStrip = this.Cms_Template;
			this.ListViewTemplate.HideSelection = false;
			this.ListViewTemplate.Location = new System.Drawing.Point(36, 79);
			this.ListViewTemplate.Name = "ListViewTemplate";
			this.ListViewTemplate.Size = new System.Drawing.Size(414, 91);
			this.ListViewTemplate.SmallImageList = this.ImgListTemplate;
			this.ListViewTemplate.StateImageList = this.ImgListTemplate;
			this.ListViewTemplate.TabIndex = 4;
			this.ListViewTemplate.UseCompatibleStateImageBehavior = false;
			this.ListViewTemplate.View = System.Windows.Forms.View.SmallIcon;
			this.ListViewTemplate.ItemActivate += new System.EventHandler(ListViewTemplate_ItemActivate);
			this.ListViewTemplate.DragDrop += new System.Windows.Forms.DragEventHandler(ListViewTemplate_DragDrop);
			this.ListViewTemplate.DragEnter += new System.Windows.Forms.DragEventHandler(ListViewTemplate_DragEnter);
			this.Cms_Template.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.TsmTemplateDelete, this.TsmTemplateUpload, this.TsmTemplateDownload });
			this.Cms_Template.Name = "contextMenuStrip1";
			this.Cms_Template.Size = new System.Drawing.Size(101, 70);
			this.Cms_Template.Opening += new System.ComponentModel.CancelEventHandler(Cms_Template_Opening);
			this.TsmTemplateDelete.Name = "TsmTemplateDelete";
			this.TsmTemplateDelete.Size = new System.Drawing.Size(100, 22);
			this.TsmTemplateDelete.Text = "删除";
			this.TsmTemplateDelete.Click += new System.EventHandler(TsmTemplateDelete_Click);
			this.TsmTemplateUpload.Name = "TsmTemplateUpload";
			this.TsmTemplateUpload.Size = new System.Drawing.Size(100, 22);
			this.TsmTemplateUpload.Text = "上传";
			this.TsmTemplateUpload.Click += new System.EventHandler(TsmTemplateUpload_Click);
			this.TsmTemplateDownload.Name = "TsmTemplateDownload";
			this.TsmTemplateDownload.Size = new System.Drawing.Size(100, 22);
			this.TsmTemplateDownload.Text = "下载";
			this.TsmTemplateDownload.Click += new System.EventHandler(TsmTemplateDownload_Click);
			this.ImgListTemplate.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImgListTemplate.ImageStream");
			this.ImgListTemplate.TransparentColor = System.Drawing.Color.Transparent;
			this.ImgListTemplate.Images.SetKeyName(0, "Template.png");
			this.ImgListTemplate.Images.SetKeyName(1, "TemplateFolder.png");
			this.LabTemplateName.AutoSize = true;
			this.LabTemplateName.Location = new System.Drawing.Point(175, 26);
			this.LabTemplateName.Name = "LabTemplateName";
			this.LabTemplateName.Size = new System.Drawing.Size(63, 14);
			this.LabTemplateName.TabIndex = 3;
			this.LabTemplateName.Text = "模板名字";
			this.TxbTemplateName.Location = new System.Drawing.Point(173, 43);
			this.TxbTemplateName.Name = "TxbTemplateName";
			this.TxbTemplateName.Size = new System.Drawing.Size(143, 23);
			this.TxbTemplateName.TabIndex = 2;
			this.BtnTemplateSave.Location = new System.Drawing.Point(322, 24);
			this.BtnTemplateSave.Name = "BtnTemplateSave";
			this.BtnTemplateSave.Size = new System.Drawing.Size(128, 48);
			this.BtnTemplateSave.TabIndex = 1;
			this.BtnTemplateSave.Text = "模板保存";
			this.BtnTemplateSave.UseVisualStyleBackColor = true;
			this.BtnTemplateSave.Click += new System.EventHandler(BtnTemplateSave_Click);
			this.BtnReadAllTemplate.Location = new System.Drawing.Point(34, 24);
			this.BtnReadAllTemplate.Name = "BtnReadAllTemplate";
			this.BtnReadAllTemplate.Size = new System.Drawing.Size(128, 48);
			this.BtnReadAllTemplate.TabIndex = 0;
			this.BtnReadAllTemplate.Text = "读取设备模板";
			this.BtnReadAllTemplate.UseVisualStyleBackColor = true;
			this.BtnReadAllTemplate.Click += new System.EventHandler(BtnReadAllTemplate_Click);
			this.GrbTemplateSetting.Controls.Add(this.BtnGetTempPara);
			this.GrbTemplateSetting.Controls.Add(this.ChbEnableTempFloop);
			this.GrbTemplateSetting.Controls.Add(this.CobTempDPMLevel);
			this.GrbTemplateSetting.Controls.Add(this.LabTempDPMLevel);
			this.GrbTemplateSetting.Controls.Add(this.TxbSensorGainDisp);
			this.GrbTemplateSetting.Controls.Add(this.LabSensorGainDisp);
			this.GrbTemplateSetting.Controls.Add(this.TxbSensorExpDisp);
			this.GrbTemplateSetting.Controls.Add(this.LabSensorExpDisp);
			this.GrbTemplateSetting.Controls.Add(this.TxbSecondLightDisp);
			this.GrbTemplateSetting.Controls.Add(this.LabSecondLightDisp);
			this.GrbTemplateSetting.Controls.Add(this.TxbMainLightDisp);
			this.GrbTemplateSetting.Controls.Add(this.LabMainLightDisp);
			this.GrbTemplateSetting.Location = new System.Drawing.Point(2, 5);
			this.GrbTemplateSetting.Name = "GrbTemplateSetting";
			this.GrbTemplateSetting.Size = new System.Drawing.Size(458, 101);
			this.GrbTemplateSetting.TabIndex = 4;
			this.GrbTemplateSetting.TabStop = false;
			this.GrbTemplateSetting.Text = "模板参数(老)";
			this.BtnGetTempPara.Location = new System.Drawing.Point(345, 17);
			this.BtnGetTempPara.Name = "BtnGetTempPara";
			this.BtnGetTempPara.Size = new System.Drawing.Size(106, 50);
			this.BtnGetTempPara.TabIndex = 40;
			this.BtnGetTempPara.Text = "重新获取值";
			this.BtnGetTempPara.UseVisualStyleBackColor = true;
			this.BtnGetTempPara.Click += new System.EventHandler(BtnGetTempPara_Click);
			this.ChbEnableTempFloop.AutoSize = true;
			this.ChbEnableTempFloop.Location = new System.Drawing.Point(260, 73);
			this.ChbEnableTempFloop.Name = "ChbEnableTempFloop";
			this.ChbEnableTempFloop.Size = new System.Drawing.Size(138, 18);
			this.ChbEnableTempFloop.TabIndex = 39;
			this.ChbEnableTempFloop.Text = "开启模板循环(老)";
			this.ChbEnableTempFloop.UseVisualStyleBackColor = true;
			this.ChbEnableTempFloop.CheckedChanged += new System.EventHandler(ChbEnableTempFloop_CheckedChanged);
			this.CobTempDPMLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobTempDPMLevel.FormattingEnabled = true;
			this.CobTempDPMLevel.Items.AddRange(new object[9] { "正常模式", "DPM模式1", "DPM模式2", "DPM模式3", "DPM模式4", "DPM模式5", "DPM模式6", "DPM模式7", "DPM模式8" });
			this.CobTempDPMLevel.Location = new System.Drawing.Point(132, 71);
			this.CobTempDPMLevel.Name = "CobTempDPMLevel";
			this.CobTempDPMLevel.Size = new System.Drawing.Size(116, 22);
			this.CobTempDPMLevel.TabIndex = 38;
			this.CobTempDPMLevel.Visible = false;
			this.LabTempDPMLevel.AutoSize = true;
			this.LabTempDPMLevel.Location = new System.Drawing.Point(34, 75);
			this.LabTempDPMLevel.Name = "LabTempDPMLevel";
			this.LabTempDPMLevel.Size = new System.Drawing.Size(91, 14);
			this.LabTempDPMLevel.TabIndex = 37;
			this.LabTempDPMLevel.Text = "DPM识读模式:";
			this.LabTempDPMLevel.Visible = false;
			this.TxbSensorGainDisp.Enabled = false;
			this.TxbSensorGainDisp.Location = new System.Drawing.Point(288, 44);
			this.TxbSensorGainDisp.Name = "TxbSensorGainDisp";
			this.TxbSensorGainDisp.Size = new System.Drawing.Size(46, 23);
			this.TxbSensorGainDisp.TabIndex = 34;
			this.LabSensorGainDisp.AutoSize = true;
			this.LabSensorGainDisp.Location = new System.Drawing.Point(191, 48);
			this.LabSensorGainDisp.Name = "LabSensorGainDisp";
			this.LabSensorGainDisp.Size = new System.Drawing.Size(98, 14);
			this.LabSensorGainDisp.TabIndex = 35;
			this.LabSensorGainDisp.Text = "传感器增益值:";
			this.TxbSensorExpDisp.Enabled = false;
			this.TxbSensorExpDisp.Location = new System.Drawing.Point(135, 44);
			this.TxbSensorExpDisp.Name = "TxbSensorExpDisp";
			this.TxbSensorExpDisp.Size = new System.Drawing.Size(46, 23);
			this.TxbSensorExpDisp.TabIndex = 32;
			this.LabSensorExpDisp.AutoSize = true;
			this.LabSensorExpDisp.Location = new System.Drawing.Point(34, 48);
			this.LabSensorExpDisp.Name = "LabSensorExpDisp";
			this.LabSensorExpDisp.Size = new System.Drawing.Size(98, 14);
			this.LabSensorExpDisp.TabIndex = 33;
			this.LabSensorExpDisp.Text = "传感器曝光值:";
			this.TxbSecondLightDisp.Enabled = false;
			this.TxbSecondLightDisp.Location = new System.Drawing.Point(288, 18);
			this.TxbSecondLightDisp.Name = "TxbSecondLightDisp";
			this.TxbSecondLightDisp.Size = new System.Drawing.Size(46, 23);
			this.TxbSecondLightDisp.TabIndex = 30;
			this.LabSecondLightDisp.AutoSize = true;
			this.LabSecondLightDisp.Location = new System.Drawing.Point(191, 23);
			this.LabSecondLightDisp.Name = "LabSecondLightDisp";
			this.LabSecondLightDisp.Size = new System.Drawing.Size(98, 14);
			this.LabSecondLightDisp.TabIndex = 31;
			this.LabSecondLightDisp.Text = "辅助照明亮度:";
			this.TxbMainLightDisp.Enabled = false;
			this.TxbMainLightDisp.Location = new System.Drawing.Point(135, 18);
			this.TxbMainLightDisp.Name = "TxbMainLightDisp";
			this.TxbMainLightDisp.Size = new System.Drawing.Size(46, 23);
			this.TxbMainLightDisp.TabIndex = 28;
			this.LabMainLightDisp.AutoSize = true;
			this.LabMainLightDisp.Location = new System.Drawing.Point(34, 23);
			this.LabMainLightDisp.Name = "LabMainLightDisp";
			this.LabMainLightDisp.Size = new System.Drawing.Size(98, 14);
			this.LabMainLightDisp.TabIndex = 29;
			this.LabMainLightDisp.Text = "主要照明亮度:";
			this.ucSplitLine_H1.BackColor = System.Drawing.Color.FromArgb(232, 232, 232);
			this.ucSplitLine_H1.Location = new System.Drawing.Point(2, 198);
			this.ucSplitLine_H1.Name = "ucSplitLine_H1";
			this.ucSplitLine_H1.Size = new System.Drawing.Size(455, 5);
			this.ucSplitLine_H1.TabIndex = 8;
			this.ucSplitLine_H1.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(464, 689);
			base.Controls.Add(this.GrbNewTemplate);
			base.Controls.Add(this.ucSplitLine_H1);
			base.Controls.Add(this.GrbTemplateOp);
			base.Controls.Add(this.GrbTemplateFiles);
			base.Controls.Add(this.GrbTemplateSetting);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TemplateSettingForm";
			this.Text = "模板配置";
			base.Load += new System.EventHandler(TemplateSettingForm_Load);
			this.GrbNewTemplate.ResumeLayout(false);
			this.GrbNewTemplate.PerformLayout();
			this.PanTemplateRoi.ResumeLayout(false);
			this.PanTemplateRoi.PerformLayout();
			this.GrbTemplateOp.ResumeLayout(false);
			this.GrbTemplateFiles.ResumeLayout(false);
			this.GrbTemplateFiles.PerformLayout();
			this.Cms_Template.ResumeLayout(false);
			this.GrbTemplateSetting.ResumeLayout(false);
			this.GrbTemplateSetting.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
