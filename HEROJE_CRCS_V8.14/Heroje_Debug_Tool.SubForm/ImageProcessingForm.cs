using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class ImageProcessingForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private int PointOx = 47;

		private int PointOy = 272;

		private Color LastColor;

		private DataPoint LastPoint = null;

		private bool IsChangePoint = false;

		private IContainer components = null;

		private CheckBox ChbIsNewVerImgProc;

		private GroupBox GrbNewVersionImgProc;

		private ComboBox CobImgProcContrastEnhance2;

		private Label LabImgProcContrastEnhance2;

		private ComboBox CobImgProcContrastEnhance1;

		private Label LabImgProcContrastEnhance1;

		private ComboBox CobMorphErode2;

		private Label LabMorphErode2;

		private ComboBox CobMorphDilate2;

		private Label LabMorphDilate2;

		private ComboBox CobGaussBlur2;

		private Label LabGaussBlur2;

		private ComboBox CobEqualizeHist2;

		private Label LabEqualizeHist2;

		private ComboBox CobMedianFilter2;

		private Label LabMedianFilter2;

		private ComboBox CobMorphErode1;

		private Label LabMorphErode1;

		private ComboBox CobMorphDilate1;

		private Label LabMorphDilate1;

		private ComboBox CobGaussBlur1;

		private Label LabGaussBlur1;

		private ComboBox CobEqualizeHist1;

		private Label LabEqualizeHist1;

		private ComboBox CobMedianFilter1;

		private CheckBox ChbImageInverseNew;

		private CheckBox ChbImageMirrorNew;

		private Label LabMedianFilter1;

		private GroupBox GrbOldVersionImgProc;

		private Chart ChartGrayEnhance;

		private CheckBox ChbImageInverse;

		private CheckBox ChbEqualizeHist;

		private CheckBox ChbImageMirror;

		private CheckBox ChbContrastEnhance;

		public ImageProcessingForm()
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
			ChbContrastEnhance.Checked = GetCfgCBFuncCB(2312u) == 8;
			ChbEqualizeHist.Checked = GetCfgCBFuncCB(2336u) == 32;
			ChbImageMirror.Checked = GetCfgCBFuncCB(2307u) == 1;
			ChbImageInverse.Checked = GetCfgCBFuncCB(2308u) == 4;
			ChbImageMirrorNew.Checked = GetCfgCBFuncCB(2307u) == 1;
			ChbImageInverseNew.Checked = GetCfgCBFuncCB(2308u) == 4;
			int num = GetCfgCBFuncCB(69647u);
			if (num >= 16)
			{
				num = -1;
			}
			CobMedianFilter1.SelectedIndex = num;
			num = GetCfgCBFuncCB(69903u);
			if (num >= 16)
			{
				num = -1;
			}
			CobEqualizeHist1.SelectedIndex = num;
			num = GetCfgCBFuncCB(70159u);
			if (num >= 16)
			{
				num = -1;
			}
			CobGaussBlur1.SelectedIndex = num;
			num = GetCfgCBFuncCB(70415u);
			if (num >= 16)
			{
				num = -1;
			}
			CobMorphDilate1.SelectedIndex = num;
			num = GetCfgCBFuncCB(70671u);
			if (num >= 16)
			{
				num = -1;
			}
			CobMorphErode1.SelectedIndex = num;
			num = GetCfgCBFuncCB(70927u);
			if (num >= 16)
			{
				num = -1;
			}
			CobImgProcContrastEnhance1.SelectedIndex = num;
			num = GetCfgCBFuncCB(69872u) / 16;
			if (num >= 16)
			{
				num = -1;
			}
			CobMedianFilter2.SelectedIndex = num;
			num = GetCfgCBFuncCB(70128u) / 16;
			if (num >= 16)
			{
				num = -1;
			}
			CobEqualizeHist2.SelectedIndex = num;
			num = GetCfgCBFuncCB(70384u) / 16;
			if (num >= 16)
			{
				num = -1;
			}
			CobGaussBlur2.SelectedIndex = num;
			num = GetCfgCBFuncCB(70640u) / 16;
			if (num >= 16)
			{
				num = -1;
			}
			CobMorphDilate2.SelectedIndex = num;
			num = GetCfgCBFuncCB(70896u) / 16;
			if (num >= 16)
			{
				num = -1;
			}
			CobMorphErode2.SelectedIndex = num;
			num = GetCfgCBFuncCB(71152u) / 16;
			if (num >= 16)
			{
				num = -1;
			}
			CobImgProcContrastEnhance2.SelectedIndex = num;
			if (para.ParaDataLen >= 4096)
			{
				ChbIsNewVerImgProc.Enabled = true;
				GrbNewVersionImgProc.Enabled = ChbIsNewVerImgProc.Checked;
				GrbOldVersionImgProc.Enabled = !GrbNewVersionImgProc.Enabled;
				ChbIsNewVerImgProc.Checked = GetCfgCBFuncCB(2320u) == 0;
			}
			else
			{
				ChbIsNewVerImgProc.Enabled = false;
				GrbNewVersionImgProc.Enabled = false;
				ChbIsNewVerImgProc.Checked = false;
			}
		}

		private void ImageProcessingForm_Load(object sender, EventArgs e)
		{
		}

		private void ChbIsNewVerImgProc_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbIsNewVerImgProc.Checked)
				{
					SetCfgCBFuncCB(2320u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(2320u, 16u);
				}
				SendCfgDataCBFuncCB(0u);
				GrbNewVersionImgProc.Enabled = ChbIsNewVerImgProc.Checked;
				GrbOldVersionImgProc.Enabled = !GrbNewVersionImgProc.Enabled;
			}
		}

		private void ChbContrastEnhance_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbContrastEnhance.Checked)
				{
					SetCfgCBFuncCB(2312u, 8u);
				}
				else
				{
					SetCfgCBFuncCB(2312u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbEqualizeHist_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbEqualizeHist.Checked)
				{
					SetCfgCBFuncCB(2336u, 32u);
				}
				else
				{
					SetCfgCBFuncCB(2336u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbImageMirror_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbImageMirror.Checked)
				{
					SetCfgCBFuncCB(2307u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(2307u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbImageInverse_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbImageInverse.Checked)
				{
					SetCfgCBFuncCB(2308u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(2308u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChartGrayEnhance_MouseMove(object sender, MouseEventArgs e)
		{
			if (LastPoint != null && LastColor != LastPoint.MarkerColor)
			{
				LastPoint.MarkerColor = LastColor;
			}
			HitTestResult hitTestResult = ChartGrayEnhance.HitTest(e.X, e.Y);
			if (hitTestResult.ChartElementType == ChartElementType.DataPoint)
			{
				int pointIndex = hitTestResult.PointIndex;
				DataPoint dataPoint = hitTestResult.Series.Points[pointIndex];
				if (dataPoint.XValue != 255.0 && dataPoint.YValues[0] != 255.0 && dataPoint.XValue != 0.0 && dataPoint.YValues[0] != 0.0)
				{
					LastPoint = dataPoint;
					LastColor = LastPoint.MarkerColor;
					dataPoint.MarkerColor = Color.Red;
				}
			}
			if (e.Button == MouseButtons.Left && LastPoint != null)
			{
				IsChangePoint = true;
				LastPoint.XValue = e.X - PointOx;
				LastPoint.YValues[0] = PointOy - e.Y;
			}
		}

		private void ChartGrayEnhance_MouseUp(object sender, MouseEventArgs e)
		{
			if (IsChangePoint)
			{
				IsChangePoint = false;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(56575u, (ushort)ChartGrayEnhance.Series[0].Points[1].XValue);
					SetCfgCBFuncCB(56831u, (ushort)ChartGrayEnhance.Series[0].Points[1].YValues[0]);
					SetCfgCBFuncCB(57087u, (ushort)ChartGrayEnhance.Series[0].Points[2].XValue);
					SetCfgCBFuncCB(57343u, (ushort)ChartGrayEnhance.Series[0].Points[2].YValues[0]);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void ChbImageMirrorNew_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbImageMirrorNew.Checked)
				{
					SetCfgCBFuncCB(2307u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(2307u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbImageInverseNew_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbImageInverseNew.Checked)
				{
					SetCfgCBFuncCB(2308u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(2308u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobMedianFilter1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobMedianFilter1.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(69647u, (ushort)selectedIndex);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobEqualizeHist1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobEqualizeHist1.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(69903u, (ushort)selectedIndex);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobGaussBlur1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobGaussBlur1.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70159u, (ushort)selectedIndex);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobMorphDilate1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobMorphDilate1.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70415u, (ushort)selectedIndex);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobMorphErode1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobMorphErode1.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70671u, (ushort)selectedIndex);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobImgProcContrastEnhance1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobImgProcContrastEnhance1.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70927u, (ushort)selectedIndex);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobMedianFilter2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobMedianFilter2.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(69872u, (ushort)(selectedIndex * 16));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobEqualizeHist2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobEqualizeHist2.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70128u, (ushort)(selectedIndex * 16));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobGaussBlur2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobGaussBlur2.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70384u, (ushort)(selectedIndex * 16));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobMorphDilate2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobMorphDilate2.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70640u, (ushort)(selectedIndex * 16));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobMorphErode2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int selectedIndex = CobMorphErode2.SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < 16)
				{
					SetCfgCBFuncCB(70896u, (ushort)(selectedIndex * 16));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobImgProcContrastEnhance2_SelectedIndexChanged(object sender, EventArgs e)
		{
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0, "0,0");
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(70.0, "40,0");
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(180.0, "240,0");
			System.Windows.Forms.DataVisualization.Charting.DataPoint item = new System.Windows.Forms.DataVisualization.Charting.DataPoint(255.0, "255,0");
			this.ChbIsNewVerImgProc = new System.Windows.Forms.CheckBox();
			this.GrbNewVersionImgProc = new System.Windows.Forms.GroupBox();
			this.CobImgProcContrastEnhance2 = new System.Windows.Forms.ComboBox();
			this.LabImgProcContrastEnhance2 = new System.Windows.Forms.Label();
			this.CobImgProcContrastEnhance1 = new System.Windows.Forms.ComboBox();
			this.LabImgProcContrastEnhance1 = new System.Windows.Forms.Label();
			this.CobMorphErode2 = new System.Windows.Forms.ComboBox();
			this.LabMorphErode2 = new System.Windows.Forms.Label();
			this.CobMorphDilate2 = new System.Windows.Forms.ComboBox();
			this.LabMorphDilate2 = new System.Windows.Forms.Label();
			this.CobGaussBlur2 = new System.Windows.Forms.ComboBox();
			this.LabGaussBlur2 = new System.Windows.Forms.Label();
			this.CobEqualizeHist2 = new System.Windows.Forms.ComboBox();
			this.LabEqualizeHist2 = new System.Windows.Forms.Label();
			this.CobMedianFilter2 = new System.Windows.Forms.ComboBox();
			this.LabMedianFilter2 = new System.Windows.Forms.Label();
			this.CobMorphErode1 = new System.Windows.Forms.ComboBox();
			this.LabMorphErode1 = new System.Windows.Forms.Label();
			this.CobMorphDilate1 = new System.Windows.Forms.ComboBox();
			this.LabMorphDilate1 = new System.Windows.Forms.Label();
			this.CobGaussBlur1 = new System.Windows.Forms.ComboBox();
			this.LabGaussBlur1 = new System.Windows.Forms.Label();
			this.CobEqualizeHist1 = new System.Windows.Forms.ComboBox();
			this.LabEqualizeHist1 = new System.Windows.Forms.Label();
			this.CobMedianFilter1 = new System.Windows.Forms.ComboBox();
			this.ChbImageInverseNew = new System.Windows.Forms.CheckBox();
			this.ChbImageMirrorNew = new System.Windows.Forms.CheckBox();
			this.LabMedianFilter1 = new System.Windows.Forms.Label();
			this.GrbOldVersionImgProc = new System.Windows.Forms.GroupBox();
			this.ChartGrayEnhance = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.ChbImageInverse = new System.Windows.Forms.CheckBox();
			this.ChbEqualizeHist = new System.Windows.Forms.CheckBox();
			this.ChbImageMirror = new System.Windows.Forms.CheckBox();
			this.ChbContrastEnhance = new System.Windows.Forms.CheckBox();
			this.GrbNewVersionImgProc.SuspendLayout();
			this.GrbOldVersionImgProc.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ChartGrayEnhance).BeginInit();
			base.SuspendLayout();
			this.ChbIsNewVerImgProc.AutoSize = true;
			this.ChbIsNewVerImgProc.Location = new System.Drawing.Point(14, 10);
			this.ChbIsNewVerImgProc.Name = "ChbIsNewVerImgProc";
			this.ChbIsNewVerImgProc.Size = new System.Drawing.Size(180, 18);
			this.ChbIsNewVerImgProc.TabIndex = 5;
			this.ChbIsNewVerImgProc.Text = "采用新版本图像处理方式";
			this.ChbIsNewVerImgProc.UseVisualStyleBackColor = true;
			this.ChbIsNewVerImgProc.CheckedChanged += new System.EventHandler(ChbIsNewVerImgProc_CheckedChanged);
			this.GrbNewVersionImgProc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbNewVersionImgProc.Controls.Add(this.CobImgProcContrastEnhance2);
			this.GrbNewVersionImgProc.Controls.Add(this.LabImgProcContrastEnhance2);
			this.GrbNewVersionImgProc.Controls.Add(this.CobImgProcContrastEnhance1);
			this.GrbNewVersionImgProc.Controls.Add(this.LabImgProcContrastEnhance1);
			this.GrbNewVersionImgProc.Controls.Add(this.CobMorphErode2);
			this.GrbNewVersionImgProc.Controls.Add(this.LabMorphErode2);
			this.GrbNewVersionImgProc.Controls.Add(this.CobMorphDilate2);
			this.GrbNewVersionImgProc.Controls.Add(this.LabMorphDilate2);
			this.GrbNewVersionImgProc.Controls.Add(this.CobGaussBlur2);
			this.GrbNewVersionImgProc.Controls.Add(this.LabGaussBlur2);
			this.GrbNewVersionImgProc.Controls.Add(this.CobEqualizeHist2);
			this.GrbNewVersionImgProc.Controls.Add(this.LabEqualizeHist2);
			this.GrbNewVersionImgProc.Controls.Add(this.CobMedianFilter2);
			this.GrbNewVersionImgProc.Controls.Add(this.LabMedianFilter2);
			this.GrbNewVersionImgProc.Controls.Add(this.CobMorphErode1);
			this.GrbNewVersionImgProc.Controls.Add(this.LabMorphErode1);
			this.GrbNewVersionImgProc.Controls.Add(this.CobMorphDilate1);
			this.GrbNewVersionImgProc.Controls.Add(this.LabMorphDilate1);
			this.GrbNewVersionImgProc.Controls.Add(this.CobGaussBlur1);
			this.GrbNewVersionImgProc.Controls.Add(this.LabGaussBlur1);
			this.GrbNewVersionImgProc.Controls.Add(this.CobEqualizeHist1);
			this.GrbNewVersionImgProc.Controls.Add(this.LabEqualizeHist1);
			this.GrbNewVersionImgProc.Controls.Add(this.CobMedianFilter1);
			this.GrbNewVersionImgProc.Controls.Add(this.ChbImageInverseNew);
			this.GrbNewVersionImgProc.Controls.Add(this.ChbImageMirrorNew);
			this.GrbNewVersionImgProc.Controls.Add(this.LabMedianFilter1);
			this.GrbNewVersionImgProc.Location = new System.Drawing.Point(14, 190);
			this.GrbNewVersionImgProc.Name = "GrbNewVersionImgProc";
			this.GrbNewVersionImgProc.Size = new System.Drawing.Size(391, 431);
			this.GrbNewVersionImgProc.TabIndex = 4;
			this.GrbNewVersionImgProc.TabStop = false;
			this.GrbNewVersionImgProc.Text = "新版本图像处理";
			this.CobImgProcContrastEnhance2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobImgProcContrastEnhance2.FormattingEnabled = true;
			this.CobImgProcContrastEnhance2.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobImgProcContrastEnhance2.Location = new System.Drawing.Point(137, 389);
			this.CobImgProcContrastEnhance2.Name = "CobImgProcContrastEnhance2";
			this.CobImgProcContrastEnhance2.Size = new System.Drawing.Size(103, 22);
			this.CobImgProcContrastEnhance2.TabIndex = 54;
			this.CobImgProcContrastEnhance2.SelectedIndexChanged += new System.EventHandler(CobImgProcContrastEnhance2_SelectedIndexChanged);
			this.LabImgProcContrastEnhance2.AutoSize = true;
			this.LabImgProcContrastEnhance2.Location = new System.Drawing.Point(35, 393);
			this.LabImgProcContrastEnhance2.Name = "LabImgProcContrastEnhance2";
			this.LabImgProcContrastEnhance2.Size = new System.Drawing.Size(91, 14);
			this.LabImgProcContrastEnhance2.TabIndex = 53;
			this.LabImgProcContrastEnhance2.Text = "对比度增强2:";
			this.CobImgProcContrastEnhance1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobImgProcContrastEnhance1.FormattingEnabled = true;
			this.CobImgProcContrastEnhance1.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobImgProcContrastEnhance1.Location = new System.Drawing.Point(136, 203);
			this.CobImgProcContrastEnhance1.Name = "CobImgProcContrastEnhance1";
			this.CobImgProcContrastEnhance1.Size = new System.Drawing.Size(103, 22);
			this.CobImgProcContrastEnhance1.TabIndex = 52;
			this.CobImgProcContrastEnhance1.SelectedIndexChanged += new System.EventHandler(CobImgProcContrastEnhance1_SelectedIndexChanged);
			this.LabImgProcContrastEnhance1.AutoSize = true;
			this.LabImgProcContrastEnhance1.Location = new System.Drawing.Point(35, 207);
			this.LabImgProcContrastEnhance1.Name = "LabImgProcContrastEnhance1";
			this.LabImgProcContrastEnhance1.Size = new System.Drawing.Size(91, 14);
			this.LabImgProcContrastEnhance1.TabIndex = 51;
			this.LabImgProcContrastEnhance1.Text = "对比度增强1:";
			this.CobMorphErode2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMorphErode2.FormattingEnabled = true;
			this.CobMorphErode2.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobMorphErode2.Location = new System.Drawing.Point(137, 360);
			this.CobMorphErode2.Name = "CobMorphErode2";
			this.CobMorphErode2.Size = new System.Drawing.Size(103, 22);
			this.CobMorphErode2.TabIndex = 50;
			this.CobMorphErode2.SelectedIndexChanged += new System.EventHandler(CobMorphErode2_SelectedIndexChanged);
			this.LabMorphErode2.AutoSize = true;
			this.LabMorphErode2.Location = new System.Drawing.Point(35, 364);
			this.LabMorphErode2.Name = "LabMorphErode2";
			this.LabMorphErode2.Size = new System.Drawing.Size(91, 14);
			this.LabMorphErode2.TabIndex = 49;
			this.LabMorphErode2.Text = "形态学腐蚀2:";
			this.CobMorphDilate2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMorphDilate2.FormattingEnabled = true;
			this.CobMorphDilate2.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobMorphDilate2.Location = new System.Drawing.Point(137, 332);
			this.CobMorphDilate2.Name = "CobMorphDilate2";
			this.CobMorphDilate2.Size = new System.Drawing.Size(103, 22);
			this.CobMorphDilate2.TabIndex = 48;
			this.CobMorphDilate2.SelectedIndexChanged += new System.EventHandler(CobMorphDilate2_SelectedIndexChanged);
			this.LabMorphDilate2.AutoSize = true;
			this.LabMorphDilate2.Location = new System.Drawing.Point(35, 335);
			this.LabMorphDilate2.Name = "LabMorphDilate2";
			this.LabMorphDilate2.Size = new System.Drawing.Size(91, 14);
			this.LabMorphDilate2.TabIndex = 47;
			this.LabMorphDilate2.Text = "形态学膨胀2:";
			this.CobGaussBlur2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobGaussBlur2.FormattingEnabled = true;
			this.CobGaussBlur2.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobGaussBlur2.Location = new System.Drawing.Point(137, 302);
			this.CobGaussBlur2.Name = "CobGaussBlur2";
			this.CobGaussBlur2.Size = new System.Drawing.Size(103, 22);
			this.CobGaussBlur2.TabIndex = 46;
			this.CobGaussBlur2.SelectedIndexChanged += new System.EventHandler(CobGaussBlur2_SelectedIndexChanged);
			this.LabGaussBlur2.AutoSize = true;
			this.LabGaussBlur2.Location = new System.Drawing.Point(35, 305);
			this.LabGaussBlur2.Name = "LabGaussBlur2";
			this.LabGaussBlur2.Size = new System.Drawing.Size(77, 14);
			this.LabGaussBlur2.TabIndex = 45;
			this.LabGaussBlur2.Text = "高斯滤波2:";
			this.CobEqualizeHist2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobEqualizeHist2.FormattingEnabled = true;
			this.CobEqualizeHist2.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobEqualizeHist2.Location = new System.Drawing.Point(137, 273);
			this.CobEqualizeHist2.Name = "CobEqualizeHist2";
			this.CobEqualizeHist2.Size = new System.Drawing.Size(103, 22);
			this.CobEqualizeHist2.TabIndex = 44;
			this.CobEqualizeHist2.SelectedIndexChanged += new System.EventHandler(CobEqualizeHist2_SelectedIndexChanged);
			this.LabEqualizeHist2.AutoSize = true;
			this.LabEqualizeHist2.Location = new System.Drawing.Point(35, 277);
			this.LabEqualizeHist2.Name = "LabEqualizeHist2";
			this.LabEqualizeHist2.Size = new System.Drawing.Size(77, 14);
			this.LabEqualizeHist2.TabIndex = 43;
			this.LabEqualizeHist2.Text = "图像均衡2:";
			this.CobMedianFilter2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMedianFilter2.FormattingEnabled = true;
			this.CobMedianFilter2.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobMedianFilter2.Location = new System.Drawing.Point(137, 245);
			this.CobMedianFilter2.Name = "CobMedianFilter2";
			this.CobMedianFilter2.Size = new System.Drawing.Size(103, 22);
			this.CobMedianFilter2.TabIndex = 42;
			this.CobMedianFilter2.SelectedIndexChanged += new System.EventHandler(CobMedianFilter2_SelectedIndexChanged);
			this.LabMedianFilter2.AutoSize = true;
			this.LabMedianFilter2.Location = new System.Drawing.Point(35, 250);
			this.LabMedianFilter2.Name = "LabMedianFilter2";
			this.LabMedianFilter2.Size = new System.Drawing.Size(77, 14);
			this.LabMedianFilter2.TabIndex = 41;
			this.LabMedianFilter2.Text = "中值滤波2:";
			this.CobMorphErode1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMorphErode1.FormattingEnabled = true;
			this.CobMorphErode1.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobMorphErode1.Location = new System.Drawing.Point(136, 174);
			this.CobMorphErode1.Name = "CobMorphErode1";
			this.CobMorphErode1.Size = new System.Drawing.Size(103, 22);
			this.CobMorphErode1.TabIndex = 40;
			this.CobMorphErode1.SelectedIndexChanged += new System.EventHandler(CobMorphErode1_SelectedIndexChanged);
			this.LabMorphErode1.AutoSize = true;
			this.LabMorphErode1.Location = new System.Drawing.Point(35, 179);
			this.LabMorphErode1.Name = "LabMorphErode1";
			this.LabMorphErode1.Size = new System.Drawing.Size(91, 14);
			this.LabMorphErode1.TabIndex = 39;
			this.LabMorphErode1.Text = "形态学腐蚀1:";
			this.CobMorphDilate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMorphDilate1.FormattingEnabled = true;
			this.CobMorphDilate1.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobMorphDilate1.Location = new System.Drawing.Point(136, 146);
			this.CobMorphDilate1.Name = "CobMorphDilate1";
			this.CobMorphDilate1.Size = new System.Drawing.Size(103, 22);
			this.CobMorphDilate1.TabIndex = 38;
			this.CobMorphDilate1.SelectedIndexChanged += new System.EventHandler(CobMorphDilate1_SelectedIndexChanged);
			this.LabMorphDilate1.AutoSize = true;
			this.LabMorphDilate1.Location = new System.Drawing.Point(35, 150);
			this.LabMorphDilate1.Name = "LabMorphDilate1";
			this.LabMorphDilate1.Size = new System.Drawing.Size(91, 14);
			this.LabMorphDilate1.TabIndex = 37;
			this.LabMorphDilate1.Text = "形态学膨胀1:";
			this.CobGaussBlur1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobGaussBlur1.FormattingEnabled = true;
			this.CobGaussBlur1.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobGaussBlur1.Location = new System.Drawing.Point(136, 118);
			this.CobGaussBlur1.Name = "CobGaussBlur1";
			this.CobGaussBlur1.Size = new System.Drawing.Size(103, 22);
			this.CobGaussBlur1.TabIndex = 36;
			this.CobGaussBlur1.SelectedIndexChanged += new System.EventHandler(CobGaussBlur1_SelectedIndexChanged);
			this.LabGaussBlur1.AutoSize = true;
			this.LabGaussBlur1.Location = new System.Drawing.Point(35, 123);
			this.LabGaussBlur1.Name = "LabGaussBlur1";
			this.LabGaussBlur1.Size = new System.Drawing.Size(77, 14);
			this.LabGaussBlur1.TabIndex = 35;
			this.LabGaussBlur1.Text = "高斯滤波1:";
			this.CobEqualizeHist1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobEqualizeHist1.FormattingEnabled = true;
			this.CobEqualizeHist1.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobEqualizeHist1.Location = new System.Drawing.Point(136, 91);
			this.CobEqualizeHist1.Name = "CobEqualizeHist1";
			this.CobEqualizeHist1.Size = new System.Drawing.Size(103, 22);
			this.CobEqualizeHist1.TabIndex = 34;
			this.CobEqualizeHist1.SelectedIndexChanged += new System.EventHandler(CobEqualizeHist1_SelectedIndexChanged);
			this.LabEqualizeHist1.AutoSize = true;
			this.LabEqualizeHist1.Location = new System.Drawing.Point(35, 96);
			this.LabEqualizeHist1.Name = "LabEqualizeHist1";
			this.LabEqualizeHist1.Size = new System.Drawing.Size(77, 14);
			this.LabEqualizeHist1.TabIndex = 33;
			this.LabEqualizeHist1.Text = "图像均衡1:";
			this.CobMedianFilter1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMedianFilter1.FormattingEnabled = true;
			this.CobMedianFilter1.Items.AddRange(new object[16]
			{
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
				"10", "11", "12", "13", "14", "15"
			});
			this.CobMedianFilter1.Location = new System.Drawing.Point(136, 64);
			this.CobMedianFilter1.Name = "CobMedianFilter1";
			this.CobMedianFilter1.Size = new System.Drawing.Size(103, 22);
			this.CobMedianFilter1.TabIndex = 32;
			this.CobMedianFilter1.SelectedIndexChanged += new System.EventHandler(CobMedianFilter1_SelectedIndexChanged);
			this.ChbImageInverseNew.AutoSize = true;
			this.ChbImageInverseNew.Location = new System.Drawing.Point(198, 29);
			this.ChbImageInverseNew.Name = "ChbImageInverseNew";
			this.ChbImageInverseNew.Size = new System.Drawing.Size(82, 18);
			this.ChbImageInverseNew.TabIndex = 31;
			this.ChbImageInverseNew.Text = "黑白反色";
			this.ChbImageInverseNew.UseVisualStyleBackColor = true;
			this.ChbImageInverseNew.CheckedChanged += new System.EventHandler(ChbImageInverseNew_CheckedChanged);
			this.ChbImageMirrorNew.AutoSize = true;
			this.ChbImageMirrorNew.Location = new System.Drawing.Point(37, 29);
			this.ChbImageMirrorNew.Name = "ChbImageMirrorNew";
			this.ChbImageMirrorNew.Size = new System.Drawing.Size(82, 18);
			this.ChbImageMirrorNew.TabIndex = 30;
			this.ChbImageMirrorNew.Text = "图像镜像";
			this.ChbImageMirrorNew.UseVisualStyleBackColor = true;
			this.ChbImageMirrorNew.CheckedChanged += new System.EventHandler(ChbImageMirrorNew_CheckedChanged);
			this.LabMedianFilter1.AutoSize = true;
			this.LabMedianFilter1.Location = new System.Drawing.Point(35, 68);
			this.LabMedianFilter1.Name = "LabMedianFilter1";
			this.LabMedianFilter1.Size = new System.Drawing.Size(77, 14);
			this.LabMedianFilter1.TabIndex = 0;
			this.LabMedianFilter1.Text = "中值滤波1:";
			this.GrbOldVersionImgProc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbOldVersionImgProc.Controls.Add(this.ChartGrayEnhance);
			this.GrbOldVersionImgProc.Controls.Add(this.ChbImageInverse);
			this.GrbOldVersionImgProc.Controls.Add(this.ChbEqualizeHist);
			this.GrbOldVersionImgProc.Controls.Add(this.ChbImageMirror);
			this.GrbOldVersionImgProc.Controls.Add(this.ChbContrastEnhance);
			this.GrbOldVersionImgProc.Location = new System.Drawing.Point(14, 39);
			this.GrbOldVersionImgProc.Name = "GrbOldVersionImgProc";
			this.GrbOldVersionImgProc.Size = new System.Drawing.Size(391, 143);
			this.GrbOldVersionImgProc.TabIndex = 3;
			this.GrbOldVersionImgProc.TabStop = false;
			this.GrbOldVersionImgProc.Text = "旧版本图像处理";
			this.ChartGrayEnhance.BackColor = System.Drawing.Color.Transparent;
			chartArea.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
			chartArea.AxisX.Crossing = double.MinValue;
			chartArea.AxisX.InterlacedColor = System.Drawing.Color.Transparent;
			chartArea.AxisX.Interval = 64.0;
			chartArea.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
			chartArea.AxisX.IsLabelAutoFit = false;
			chartArea.AxisX.MajorGrid.Interval = 16.0;
			chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
			chartArea.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			chartArea.AxisX.Maximum = 256.0;
			chartArea.AxisX.Minimum = 0.0;
			chartArea.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
			chartArea.AxisY.Interval = 32.0;
			chartArea.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea.AxisY.IsStartedFromZero = false;
			chartArea.AxisY.MajorGrid.Interval = 16.0;
			chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
			chartArea.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			chartArea.AxisY.Maximum = 256.0;
			chartArea.AxisY.Minimum = 0.0;
			chartArea.BackColor = System.Drawing.Color.Transparent;
			chartArea.Name = "ChartArea1";
			this.ChartGrayEnhance.ChartAreas.Add(chartArea);
			legend.BackColor = System.Drawing.Color.Transparent;
			legend.Enabled = false;
			legend.IsDockedInsideChartArea = false;
			legend.Name = "Legend1";
			this.ChartGrayEnhance.Legends.Add(legend);
			this.ChartGrayEnhance.Location = new System.Drawing.Point(120, 5);
			this.ChartGrayEnhance.Name = "ChartGrayEnhance";
			series.BorderWidth = 2;
			series.ChartArea = "ChartArea1";
			series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series.Legend = "Legend1";
			series.MarkerBorderColor = System.Drawing.Color.Transparent;
			series.MarkerColor = System.Drawing.Color.Salmon;
			series.MarkerSize = 10;
			series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
			series.Name = "拐点";
			dataPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold);
			dataPoint.IsValueShownAsLabel = false;
			dataPoint.Label = "";
			dataPoint.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			dataPoint.LabelBorderWidth = 1;
			dataPoint.LabelForeColor = System.Drawing.Color.Red;
			dataPoint.LegendText = "";
			dataPoint2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold);
			dataPoint2.Label = "(#VALX，#VALY)";
			dataPoint2.LabelBorderWidth = 1;
			dataPoint2.LabelForeColor = System.Drawing.Color.Red;
			dataPoint3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold);
			dataPoint3.IsValueShownAsLabel = true;
			dataPoint3.Label = "(#VALX，#VALY)";
			dataPoint3.LabelForeColor = System.Drawing.Color.Red;
			dataPoint3.LabelFormat = "";
			series.Points.Add(dataPoint);
			series.Points.Add(dataPoint2);
			series.Points.Add(dataPoint3);
			series.Points.Add(item);
			series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
			series.YValuesPerPoint = 2;
			series.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
			this.ChartGrayEnhance.Series.Add(series);
			this.ChartGrayEnhance.Size = new System.Drawing.Size(183, 140);
			this.ChartGrayEnhance.TabIndex = 29;
			this.ChartGrayEnhance.Visible = false;
			this.ChartGrayEnhance.MouseMove += new System.Windows.Forms.MouseEventHandler(ChartGrayEnhance_MouseMove);
			this.ChartGrayEnhance.MouseUp += new System.Windows.Forms.MouseEventHandler(ChartGrayEnhance_MouseUp);
			this.ChbImageInverse.AutoSize = true;
			this.ChbImageInverse.Location = new System.Drawing.Point(37, 115);
			this.ChbImageInverse.Name = "ChbImageInverse";
			this.ChbImageInverse.Size = new System.Drawing.Size(82, 18);
			this.ChbImageInverse.TabIndex = 28;
			this.ChbImageInverse.Text = "黑白反色";
			this.ChbImageInverse.UseVisualStyleBackColor = true;
			this.ChbImageInverse.CheckedChanged += new System.EventHandler(ChbImageInverse_CheckedChanged);
			this.ChbEqualizeHist.AutoSize = true;
			this.ChbEqualizeHist.Location = new System.Drawing.Point(37, 57);
			this.ChbEqualizeHist.Name = "ChbEqualizeHist";
			this.ChbEqualizeHist.Size = new System.Drawing.Size(82, 18);
			this.ChbEqualizeHist.TabIndex = 27;
			this.ChbEqualizeHist.Text = "图像均衡";
			this.ChbEqualizeHist.UseVisualStyleBackColor = true;
			this.ChbEqualizeHist.CheckedChanged += new System.EventHandler(ChbEqualizeHist_CheckedChanged);
			this.ChbImageMirror.AutoSize = true;
			this.ChbImageMirror.Location = new System.Drawing.Point(37, 86);
			this.ChbImageMirror.Name = "ChbImageMirror";
			this.ChbImageMirror.Size = new System.Drawing.Size(82, 18);
			this.ChbImageMirror.TabIndex = 26;
			this.ChbImageMirror.Text = "图像镜像";
			this.ChbImageMirror.UseVisualStyleBackColor = true;
			this.ChbImageMirror.CheckedChanged += new System.EventHandler(ChbImageMirror_CheckedChanged);
			this.ChbContrastEnhance.AutoSize = true;
			this.ChbContrastEnhance.Location = new System.Drawing.Point(37, 29);
			this.ChbContrastEnhance.Name = "ChbContrastEnhance";
			this.ChbContrastEnhance.Size = new System.Drawing.Size(82, 18);
			this.ChbContrastEnhance.TabIndex = 25;
			this.ChbContrastEnhance.Text = "灰度增强";
			this.ChbContrastEnhance.UseVisualStyleBackColor = true;
			this.ChbContrastEnhance.CheckedChanged += new System.EventHandler(ChbContrastEnhance_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(414, 689);
			base.Controls.Add(this.ChbIsNewVerImgProc);
			base.Controls.Add(this.GrbNewVersionImgProc);
			base.Controls.Add(this.GrbOldVersionImgProc);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "ImageProcessingForm";
			this.Text = "ImageProcessingForm";
			base.Load += new System.EventHandler(ImageProcessingForm_Load);
			this.GrbNewVersionImgProc.ResumeLayout(false);
			this.GrbNewVersionImgProc.PerformLayout();
			this.GrbOldVersionImgProc.ResumeLayout(false);
			this.GrbOldVersionImgProc.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.ChartGrayEnhance).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
