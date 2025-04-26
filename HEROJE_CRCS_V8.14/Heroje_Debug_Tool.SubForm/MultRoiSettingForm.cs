using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CoordinateConv_N;
using Heroje_Debug_Tool.BaseClass;
using HJ_Controls_Lib;
using 合杰图像算法调试工具;
using 图像算法调试工具;

namespace Heroje_Debug_Tool.SubForm
{
	public class MultRoiSettingForm : Form
	{
		public Image TemplateImage;

		public int ImgZoom = 0;

		public bool IsTemplateEnable = false;

		public bool IsGroupTemplate = false;

		public string GroupForderIdx;

		public string GroupForderName;

		private int PanelWidth;

		private int PanelHeight;

		private int Roi_Start_x;

		private int Roi_Start_y;

		private int Roi_End_x;

		private int Roi_End_y;

		private int xstart = 0;

		private int ystart = 0;

		private int xend = 0;

		private int yend = 0;

		private bool IsSaveOk = false;

		private IContainer components = null;

		private Label LabRoiXyCount;

		private TextBox TxbRoiX_Count;

		private TextBox TxbRoiY_Count;

		private Label LabMultiplay;

		private Button BtnGenerateRoi;

		private Button BtnSaveAndExit;

		private PanelEx PanImageTemplate;

		private ComboBox CobTemplateName;

		private Button BtnDeleteRoi;

		private Panel PabGenerateRoi;

		private Panel PanDeleteRoi;

		private Label LabTemplateName;

		private Button BtnAdjustSingleRoi;

		private Label LabColumn;

		private Label LabRow;

		private Label LabInfos;

		public MultRoiSettingForm()
		{
			InitializeComponent();
		}

		private void MultRoiSettingForm_Load(object sender, EventArgs e)
		{
			PanImageTemplate.BackgroundImage = TemplateImage;
			LabInfos.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TempState) + MultLanguageText.Get_Content(MultLanguageText.TextDef.TempEnableRoi) + "(" + MultLanguageText.Get_Content(MultLanguageText.TextDef.TempEnable_Y) + ")； " + MultLanguageText.Get_Content(MultLanguageText.TextDef.TempEnable) + "(" + (IsTemplateEnable ? MultLanguageText.Get_Content(MultLanguageText.TextDef.TempEnable_Y) : MultLanguageText.Get_Content(MultLanguageText.TextDef.TempEnable_N)) + ")； " + MultLanguageText.Get_Content(MultLanguageText.TextDef.TempGroupMode) + "(" + (IsGroupTemplate ? MultLanguageText.Get_Content(MultLanguageText.TextDef.TempEnable_Y) : MultLanguageText.Get_Content(MultLanguageText.TextDef.TempEnable_N)) + ")" + (IsGroupTemplate ? ("：" + GroupForderIdx + "-" + GroupForderName) : "");
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				Text = "批量制作ROI";
				LabRow.Text = "行";
				LabColumn.Text = "列";
				BtnGenerateRoi.Text = "1.自动生成ROI";
				BtnAdjustSingleRoi.Text = "2.调整单个ROI";
				BtnSaveAndExit.Text = "3.调整OK,保存并退出";
				LabTemplateName.Text = "模板号";
				BtnDeleteRoi.Text = "删除ROI";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				Text = "批量製作ROI";
				LabRow.Text = "行";
				LabColumn.Text = "列";
				BtnGenerateRoi.Text = "1.自動生成ROI";
				BtnAdjustSingleRoi.Text = "2.調整單個ROI";
				BtnSaveAndExit.Text = "3.調整OK,保存並退出";
				LabTemplateName.Text = "範本號";
				BtnDeleteRoi.Text = "刪除ROI";
			}
			else
			{
				Text = "Draw ROIs";
				LabRow.Text = "Rows";
				LabColumn.Text = "Cols";
				BtnGenerateRoi.Text = "1.Generate ROI";
				BtnAdjustSingleRoi.Text = "2.Adjust ROI";
				BtnSaveAndExit.Text = "3.Adjust OK, save and exit ";
				LabTemplateName.Text = "Temp No. ";
				BtnDeleteRoi.Text = "Delete ROI";
			}
		}

		private void MultRoiSettingForm_Shown(object sender, EventArgs e)
		{
			BtnGenerateRoi_Click(null, null);
		}

		private void BtnGenerateRoi_Click(object sender, EventArgs e)
		{
			try
			{
				int num = Convert.ToInt32(TxbRoiX_Count.Text);
				int num2 = Convert.ToInt32(TxbRoiY_Count.Text);
				if (num <= 0 || num2 <= 0)
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TempRowColErrorTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TempRowColErrorTips), MessageBoxButtons.OK);
					return;
				}
				if (num * num2 > 20)
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TempRowColMaxTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TempRowColMaxTips), MessageBoxButtons.OK);
					return;
				}
				PanImageTemplate.IsDefaultMode = false;
				PanImageTemplate.ActionNum = 7;
				PanImageTemplate.MultRect_SetRowColumn(num, num2);
				PanImageTemplate.MultRect_IsAdjustAll(true);
				BtnAdjustSingleRoi.Enabled = true;
				PanDeleteRoi.Enabled = false;
				CobTemplateName.Items.Clear();
				for (int i = 0; i < num * num2; i++)
				{
					CobTemplateName.Items.Add((i + 1).ToString());
				}
				CobTemplateName.SelectedIndex = 0;
			}
			catch (Exception)
			{
			}
		}

		private void BtnAdjustSingleRoi_Click(object sender, EventArgs e)
		{
			PanImageTemplate.MultRect_IsAdjustAll(false);
			BtnAdjustSingleRoi.Enabled = false;
			PanDeleteRoi.Enabled = true;
		}

		private void BtnDeleteRoi_Click(object sender, EventArgs e)
		{
			try
			{
				int rectIdx = Convert.ToInt32(CobTemplateName.SelectedItem) - 1;
				PanImageTemplate.MultRect_DeleteRect(rectIdx);
				CobTemplateName.Items.Remove(CobTemplateName.SelectedItem);
			}
			catch (Exception)
			{
			}
		}

		private void BtnSaveAndExit_Click(object sender, EventArgs e)
		{
			try
			{
				PanImageTemplate.GetRoiCoordinate(out PanelWidth, out PanelHeight, out Roi_Start_x, out Roi_Start_y, out Roi_End_x, out Roi_End_y);
				List<Rectangle> list = PanImageTemplate.MultRect_GetRectList();
				int imageWidth = PanImageTemplate.BackgroundImage.Width;
				int imageHeight = PanImageTemplate.BackgroundImage.Height;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Width <= 0 || list[i].Height <= 0)
					{
						continue;
					}
					Rectangle rectangle = list[i];
					CoordinateConv_N.CoordinateConv.PtbToImg(imageWidth, imageHeight, PanelWidth, PanelHeight, rectangle.X, rectangle.Y, ref xstart, ref ystart);
					CoordinateConv_N.CoordinateConv.PtbToImg(imageWidth, imageHeight, PanelWidth, PanelHeight, rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, ref xend, ref yend);
					if (!SaveTemplate(i + 1, xstart, ystart, xend - xstart, yend - ystart, ImgZoom))
					{
						if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
						{
							MessageBox.Show("ROI(" + (i + 1) + ")的区域太小了(w<64 或 h<32)，请重新设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						else
						{
							MessageBox.Show("The ROI(" + (i + 1) + ") size is too small(w<64 or h<32),Please Check", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						return;
					}
				}
			}
			catch (Exception)
			{
			}
			IsSaveOk = true;
			Close();
		}

		private bool SaveTemplate(int Template_Id, int x0, int y0, int w, int h, int zoom)
		{
			template_config_t template_config_t = default(template_config_t);
			template_config_t.imgproc_config = new imgproc_config_t[20];
			template_config_t.config_flag = 0;
			template_config_t.template_type = 1;
			template_config_t.template_id = Template_Id;
			bool flag = true;
			template_config_t.config_flag |= 1;
			template_config_t.roi_config.left = x0 * zoom;
			template_config_t.roi_config.top = y0 * zoom;
			template_config_t.roi_config.width = w * zoom;
			template_config_t.roi_config.height = h * zoom;
			if (template_config_t.roi_config.width < 64 || template_config_t.roi_config.height < 32)
			{
				return false;
			}
			bool flag2 = false;
			template_config_t.config_flag &= -5;
			bool flag3 = false;
			template_config_t.sensor_config.config_flag &= -2;
			bool flag4 = false;
			template_config_t.sensor_config.config_flag &= -17;
			bool flag5 = false;
			template_config_t.sensor_config.config_flag &= -5;
			template_config_t.sensor_config.config_flag &= -3;
			template_config_t.sensor_config.config_flag &= -33;
			bool flag6 = false;
			template_config_t.sensor_config.config_flag &= -9;
			bool flag7 = false;
			template_config_t.config_flag &= -3;
			if (IsTemplateEnable)
			{
				template_config_t.head_magic = 3405692655u;
			}
			else
			{
				template_config_t.head_magic = 3735943886u;
			}
			byte[] array = ToolCfg.StructToBytes(template_config_t);
			string text = Path.GetTempPath() + "\\" + Template_Id.ToString("D2") + ".hjt";
			FileStream fileStream = new FileStream(text, FileMode.OpenOrCreate);
			fileStream.Write(array, 0, array.Length);
			fileStream.Close();
			if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK && ToolCfg.CurrentDevice.IsCommunicate)
			{
				if (IsGroupTemplate)
				{
					ToolCfg.ftp.UploadFile(text, "ftp://" + ToolCfg.CurrentDevice.IpAddrStr, "/template/" + GroupForderIdx + "-" + GroupForderName, Template_Id.ToString("D2") + ".hjt");
				}
				else
				{
					ToolCfg.ftp.UploadFile(text, "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/template/" + Template_Id.ToString("D2") + ".hjt");
				}
			}
			return true;
		}

		private void MultRoiSettingForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!IsSaveOk && MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TempSaveRoiTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TempSaveRoiTips), MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				e.Cancel = true;
			}
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
			this.LabRoiXyCount = new System.Windows.Forms.Label();
			this.TxbRoiX_Count = new System.Windows.Forms.TextBox();
			this.TxbRoiY_Count = new System.Windows.Forms.TextBox();
			this.LabMultiplay = new System.Windows.Forms.Label();
			this.BtnGenerateRoi = new System.Windows.Forms.Button();
			this.BtnSaveAndExit = new System.Windows.Forms.Button();
			this.CobTemplateName = new System.Windows.Forms.ComboBox();
			this.BtnDeleteRoi = new System.Windows.Forms.Button();
			this.PabGenerateRoi = new System.Windows.Forms.Panel();
			this.LabColumn = new System.Windows.Forms.Label();
			this.LabRow = new System.Windows.Forms.Label();
			this.PanDeleteRoi = new System.Windows.Forms.Panel();
			this.LabTemplateName = new System.Windows.Forms.Label();
			this.BtnAdjustSingleRoi = new System.Windows.Forms.Button();
			this.PanImageTemplate = new HJ_Controls_Lib.PanelEx();
			this.LabInfos = new System.Windows.Forms.Label();
			this.PabGenerateRoi.SuspendLayout();
			this.PanDeleteRoi.SuspendLayout();
			base.SuspendLayout();
			this.LabRoiXyCount.AutoSize = true;
			this.LabRoiXyCount.Location = new System.Drawing.Point(6, 10);
			this.LabRoiXyCount.Name = "LabRoiXyCount";
			this.LabRoiXyCount.Size = new System.Drawing.Size(42, 14);
			this.LabRoiXyCount.TabIndex = 0;
			this.LabRoiXyCount.Text = "ROI：";
			this.TxbRoiX_Count.Location = new System.Drawing.Point(41, 6);
			this.TxbRoiX_Count.Name = "TxbRoiX_Count";
			this.TxbRoiX_Count.Size = new System.Drawing.Size(35, 23);
			this.TxbRoiX_Count.TabIndex = 1;
			this.TxbRoiX_Count.Text = "1";
			this.TxbRoiX_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbRoiY_Count.Location = new System.Drawing.Point(135, 6);
			this.TxbRoiY_Count.Name = "TxbRoiY_Count";
			this.TxbRoiY_Count.Size = new System.Drawing.Size(35, 23);
			this.TxbRoiY_Count.TabIndex = 3;
			this.TxbRoiY_Count.Text = "2";
			this.TxbRoiY_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.LabMultiplay.AutoSize = true;
			this.LabMultiplay.Location = new System.Drawing.Point(108, 10);
			this.LabMultiplay.Name = "LabMultiplay";
			this.LabMultiplay.Size = new System.Drawing.Size(21, 14);
			this.LabMultiplay.TabIndex = 2;
			this.LabMultiplay.Text = "×";
			this.BtnGenerateRoi.Location = new System.Drawing.Point(214, 2);
			this.BtnGenerateRoi.Name = "BtnGenerateRoi";
			this.BtnGenerateRoi.Size = new System.Drawing.Size(130, 30);
			this.BtnGenerateRoi.TabIndex = 4;
			this.BtnGenerateRoi.Text = "1.自动生成ROI";
			this.BtnGenerateRoi.UseVisualStyleBackColor = true;
			this.BtnGenerateRoi.Click += new System.EventHandler(BtnGenerateRoi_Click);
			this.BtnSaveAndExit.Location = new System.Drawing.Point(540, 30);
			this.BtnSaveAndExit.Name = "BtnSaveAndExit";
			this.BtnSaveAndExit.Size = new System.Drawing.Size(207, 30);
			this.BtnSaveAndExit.TabIndex = 5;
			this.BtnSaveAndExit.Text = "3.调整OK,保存并退出";
			this.BtnSaveAndExit.UseVisualStyleBackColor = true;
			this.BtnSaveAndExit.Click += new System.EventHandler(BtnSaveAndExit_Click);
			this.CobTemplateName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobTemplateName.FormattingEnabled = true;
			this.CobTemplateName.Location = new System.Drawing.Point(72, 7);
			this.CobTemplateName.Name = "CobTemplateName";
			this.CobTemplateName.Size = new System.Drawing.Size(62, 22);
			this.CobTemplateName.TabIndex = 7;
			this.BtnDeleteRoi.Location = new System.Drawing.Point(150, 2);
			this.BtnDeleteRoi.Name = "BtnDeleteRoi";
			this.BtnDeleteRoi.Size = new System.Drawing.Size(91, 30);
			this.BtnDeleteRoi.TabIndex = 8;
			this.BtnDeleteRoi.Text = "删除ROI";
			this.BtnDeleteRoi.UseVisualStyleBackColor = true;
			this.BtnDeleteRoi.Click += new System.EventHandler(BtnDeleteRoi_Click);
			this.PabGenerateRoi.Controls.Add(this.LabRow);
			this.PabGenerateRoi.Controls.Add(this.TxbRoiY_Count);
			this.PabGenerateRoi.Controls.Add(this.LabColumn);
			this.PabGenerateRoi.Controls.Add(this.TxbRoiX_Count);
			this.PabGenerateRoi.Controls.Add(this.BtnGenerateRoi);
			this.PabGenerateRoi.Controls.Add(this.LabRoiXyCount);
			this.PabGenerateRoi.Controls.Add(this.LabMultiplay);
			this.PabGenerateRoi.Location = new System.Drawing.Point(19, 28);
			this.PabGenerateRoi.Name = "PabGenerateRoi";
			this.PabGenerateRoi.Size = new System.Drawing.Size(348, 34);
			this.PabGenerateRoi.TabIndex = 9;
			this.LabColumn.AutoSize = true;
			this.LabColumn.Location = new System.Drawing.Point(171, 10);
			this.LabColumn.Name = "LabColumn";
			this.LabColumn.Size = new System.Drawing.Size(21, 14);
			this.LabColumn.TabIndex = 12;
			this.LabColumn.Text = "列";
			this.LabRow.AutoSize = true;
			this.LabRow.Location = new System.Drawing.Point(78, 10);
			this.LabRow.Name = "LabRow";
			this.LabRow.Size = new System.Drawing.Size(21, 14);
			this.LabRow.TabIndex = 11;
			this.LabRow.Text = "行";
			this.PanDeleteRoi.Controls.Add(this.CobTemplateName);
			this.PanDeleteRoi.Controls.Add(this.BtnDeleteRoi);
			this.PanDeleteRoi.Controls.Add(this.LabTemplateName);
			this.PanDeleteRoi.Enabled = false;
			this.PanDeleteRoi.Location = new System.Drawing.Point(19, 61);
			this.PanDeleteRoi.Name = "PanDeleteRoi";
			this.PanDeleteRoi.Size = new System.Drawing.Size(348, 34);
			this.PanDeleteRoi.TabIndex = 10;
			this.LabTemplateName.AutoSize = true;
			this.LabTemplateName.Location = new System.Drawing.Point(3, 10);
			this.LabTemplateName.Name = "LabTemplateName";
			this.LabTemplateName.Size = new System.Drawing.Size(63, 14);
			this.LabTemplateName.TabIndex = 11;
			this.LabTemplateName.Text = "模板号：";
			this.BtnAdjustSingleRoi.Location = new System.Drawing.Point(384, 30);
			this.BtnAdjustSingleRoi.Name = "BtnAdjustSingleRoi";
			this.BtnAdjustSingleRoi.Size = new System.Drawing.Size(131, 30);
			this.BtnAdjustSingleRoi.TabIndex = 5;
			this.BtnAdjustSingleRoi.Text = "2.调整单个ROI";
			this.BtnAdjustSingleRoi.UseVisualStyleBackColor = true;
			this.BtnAdjustSingleRoi.Click += new System.EventHandler(BtnAdjustSingleRoi_Click);
			this.PanImageTemplate.ActionNum = 0;
			this.PanImageTemplate.ArcStartAngle = 45;
			this.PanImageTemplate.ArcSweepAngle = 90;
			this.PanImageTemplate.BackColor = System.Drawing.Color.Silver;
			this.PanImageTemplate.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.PanImageTemplate.ImageZoomRate = 1;
			this.PanImageTemplate.IsDefaultMode = true;
			this.PanImageTemplate.Location = new System.Drawing.Point(19, 100);
			this.PanImageTemplate.Name = "PanImageTemplate";
			this.PanImageTemplate.PanelBackColor = System.Drawing.Color.FromArgb(189, 189, 189);
			this.PanImageTemplate.PanelForeColor = System.Drawing.Color.FromArgb(189, 189, 189);
			this.PanImageTemplate.Size = new System.Drawing.Size(752, 505);
			this.PanImageTemplate.TabIndex = 6;
			this.LabInfos.AutoSize = true;
			this.LabInfos.Font = new System.Drawing.Font("宋体", 10.5f, System.Drawing.FontStyle.Bold);
			this.LabInfos.Location = new System.Drawing.Point(22, 9);
			this.LabInfos.Name = "LabInfos";
			this.LabInfos.Size = new System.Drawing.Size(52, 14);
			this.LabInfos.TabIndex = 11;
			this.LabInfos.Text = "状态：";
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(788, 618);
			base.Controls.Add(this.LabInfos);
			base.Controls.Add(this.BtnAdjustSingleRoi);
			base.Controls.Add(this.PanDeleteRoi);
			base.Controls.Add(this.PabGenerateRoi);
			base.Controls.Add(this.PanImageTemplate);
			base.Controls.Add(this.BtnSaveAndExit);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MultRoiSettingForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "制作多ROI";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MultRoiSettingForm_FormClosing);
			base.Load += new System.EventHandler(MultRoiSettingForm_Load);
			base.Shown += new System.EventHandler(MultRoiSettingForm_Shown);
			this.PabGenerateRoi.ResumeLayout(false);
			this.PabGenerateRoi.PerformLayout();
			this.PanDeleteRoi.ResumeLayout(false);
			this.PanDeleteRoi.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
