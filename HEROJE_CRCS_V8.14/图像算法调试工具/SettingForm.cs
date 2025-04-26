using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using IniConfigFile_n;

namespace 图像算法调试工具
{
	public class SettingForm : Form
	{
		private bool is_update = false;

		private IContainer components = null;

		private CheckBox ChbChangeToManualTrig;

		private CheckBox ChkExitAndSave;

		private GroupBox GrbSoftwareSetting;

		private CheckBox CheckExitAndQuitDebugMode;

		public SettingForm()
		{
			InitializeComponent();
		}

		private void SettingForm_Load(object sender, EventArgs e)
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				GrbSoftwareSetting.Text = "软件退出时候动作";
				ChkExitAndSave.Text = "当软件退出时提示保存设置到设备";
				ChbChangeToManualTrig.Text = "当软件退出时切换成手动触发";
				CheckExitAndQuitDebugMode.Text = "当软件退出时退出调试模式(USB设备)";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				GrbSoftwareSetting.Text = "軟體退出時候動作";
				ChkExitAndSave.Text = "當軟體退出時提示保存設置到設備";
				ChbChangeToManualTrig.Text = "當軟體退出時切換成手動觸發";
				CheckExitAndQuitDebugMode.Text = "當軟體退出時退出調試模式(USB設備)";
			}
			else
			{
				GrbSoftwareSetting.Text = "Action When Exit";
				ChkExitAndSave.Text = "Save Setting to Device When Exit";
				ChbChangeToManualTrig.Text = "Change to Manual Trigger";
				CheckExitAndQuitDebugMode.Text = "Close Debug Mode When Exit";
			}
			ChkExitAndSave.Checked = ToolCfg.is_exit_and_save;
			if (ChkExitAndSave.Checked)
			{
				ChbChangeToManualTrig.Checked = ToolCfg.is_manual_trig;
				CheckExitAndQuitDebugMode.Checked = ToolCfg.is_quit_debug;
			}
			else
			{
				ChbChangeToManualTrig.Checked = false;
				ChbChangeToManualTrig.Enabled = false;
				CheckExitAndQuitDebugMode.Checked = false;
				CheckExitAndQuitDebugMode.Enabled = false;
			}
			is_update = true;
		}

		private void ChkExitAndSave_CheckedChanged(object sender, EventArgs e)
		{
			if (is_update)
			{
				if (ChkExitAndSave.Checked)
				{
					ChbChangeToManualTrig.Enabled = true;
					CheckExitAndQuitDebugMode.Enabled = true;
					ChbChangeToManualTrig.Checked = ToolCfg.is_manual_trig;
					CheckExitAndQuitDebugMode.Checked = ToolCfg.is_quit_debug;
				}
				else
				{
					ChbChangeToManualTrig.Checked = false;
					ChbChangeToManualTrig.Enabled = false;
					CheckExitAndQuitDebugMode.Checked = false;
					CheckExitAndQuitDebugMode.Enabled = false;
				}
				ToolCfg.is_exit_and_save = ChkExitAndSave.Checked;
				IniConfigFile iniConfigFile = new IniConfigFile();
				iniConfigFile.Config("config.ini");
				iniConfigFile.set("ExitAndSave", ChkExitAndSave.Checked ? "1" : "0");
				iniConfigFile.save();
			}
		}

		private void ChbChangeToManualTrig_CheckedChanged(object sender, EventArgs e)
		{
			if (is_update)
			{
				ToolCfg.is_manual_trig = ChbChangeToManualTrig.Checked;
				IniConfigFile iniConfigFile = new IniConfigFile();
				iniConfigFile.Config("config.ini");
				iniConfigFile.set("ManualTrig", ChbChangeToManualTrig.Checked ? "1" : "0");
				iniConfigFile.save();
			}
		}

		private void CheckExitAndQuitDebugMode_CheckedChanged(object sender, EventArgs e)
		{
			if (is_update)
			{
				ToolCfg.is_quit_debug = CheckExitAndQuitDebugMode.Checked;
				IniConfigFile iniConfigFile = new IniConfigFile();
				iniConfigFile.Config("config.ini");
				iniConfigFile.set("QuitDebug", CheckExitAndQuitDebugMode.Checked ? "1" : "0");
				iniConfigFile.save();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(图像算法调试工具.SettingForm));
			this.ChbChangeToManualTrig = new System.Windows.Forms.CheckBox();
			this.ChkExitAndSave = new System.Windows.Forms.CheckBox();
			this.GrbSoftwareSetting = new System.Windows.Forms.GroupBox();
			this.CheckExitAndQuitDebugMode = new System.Windows.Forms.CheckBox();
			this.GrbSoftwareSetting.SuspendLayout();
			base.SuspendLayout();
			this.ChbChangeToManualTrig.AutoSize = true;
			this.ChbChangeToManualTrig.Location = new System.Drawing.Point(22, 54);
			this.ChbChangeToManualTrig.Name = "ChbChangeToManualTrig";
			this.ChbChangeToManualTrig.Size = new System.Drawing.Size(208, 18);
			this.ChbChangeToManualTrig.TabIndex = 0;
			this.ChbChangeToManualTrig.Text = "当软件退出时切换成手动触发";
			this.ChbChangeToManualTrig.UseVisualStyleBackColor = true;
			this.ChbChangeToManualTrig.CheckedChanged += new System.EventHandler(ChbChangeToManualTrig_CheckedChanged);
			this.ChkExitAndSave.AutoSize = true;
			this.ChkExitAndSave.Location = new System.Drawing.Point(22, 28);
			this.ChkExitAndSave.Name = "ChkExitAndSave";
			this.ChkExitAndSave.Size = new System.Drawing.Size(236, 18);
			this.ChkExitAndSave.TabIndex = 1;
			this.ChkExitAndSave.Text = "当软件退出时提示保存设置到设备";
			this.ChkExitAndSave.UseVisualStyleBackColor = true;
			this.ChkExitAndSave.CheckedChanged += new System.EventHandler(ChkExitAndSave_CheckedChanged);
			this.GrbSoftwareSetting.Controls.Add(this.CheckExitAndQuitDebugMode);
			this.GrbSoftwareSetting.Controls.Add(this.ChkExitAndSave);
			this.GrbSoftwareSetting.Controls.Add(this.ChbChangeToManualTrig);
			this.GrbSoftwareSetting.Location = new System.Drawing.Point(22, 17);
			this.GrbSoftwareSetting.Name = "GrbSoftwareSetting";
			this.GrbSoftwareSetting.Size = new System.Drawing.Size(404, 117);
			this.GrbSoftwareSetting.TabIndex = 2;
			this.GrbSoftwareSetting.TabStop = false;
			this.GrbSoftwareSetting.Text = "软件退出时候动作";
			this.CheckExitAndQuitDebugMode.AutoSize = true;
			this.CheckExitAndQuitDebugMode.Location = new System.Drawing.Point(22, 79);
			this.CheckExitAndQuitDebugMode.Name = "CheckExitAndQuitDebugMode";
			this.CheckExitAndQuitDebugMode.Size = new System.Drawing.Size(257, 18);
			this.CheckExitAndQuitDebugMode.TabIndex = 2;
			this.CheckExitAndQuitDebugMode.Text = "当软件退出时退出调试模式(USB设备)";
			this.CheckExitAndQuitDebugMode.UseVisualStyleBackColor = true;
			this.CheckExitAndQuitDebugMode.CheckedChanged += new System.EventHandler(CheckExitAndQuitDebugMode_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(451, 148);
			base.Controls.Add(this.GrbSoftwareSetting);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SettingForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SoftwareSetting";
			base.Load += new System.EventHandler(SettingForm_Load);
			this.GrbSoftwareSetting.ResumeLayout(false);
			this.GrbSoftwareSetting.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
