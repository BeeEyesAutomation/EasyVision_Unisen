using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.SubForm;
using HJ_CRC32_n;

namespace 合杰图像算法调试工具
{
	public class ConfigBarcodeCheck : Form
	{
		private GetCfgCB GetCfgCBFunc;

		private static bool cmd_ack = false;

		private int[] GroundLen = new int[8];

		private bool rec_complete = true;

		private List<byte>[] StringBuf = new List<byte>[8]
		{
			new List<byte>(),
			new List<byte>(),
			new List<byte>(),
			new List<byte>(),
			new List<byte>(),
			new List<byte>(),
			new List<byte>(),
			new List<byte>()
		};

		private int cur_num = 0;

		private int end_len = 0;

		private IContainer components = null;

		private GroupBox GrbExpectCode;

		private Label LabTips1;

		private Button BtnWriteCfg;

		private Button BtnReadCfg;

		private TextBox TxbGround8;

		private CheckBox ChbGround8;

		private TextBox TxbGround7;

		private CheckBox ChbGround7;

		private TextBox TxbGround6;

		private CheckBox ChbGround6;

		private TextBox TxbGround5;

		private CheckBox ChbGround5;

		private TextBox TxbGround4;

		private CheckBox ChbGround4;

		private TextBox TxbGround3;

		private CheckBox ChbGround3;

		private TextBox TxbGround2;

		private CheckBox ChbGround2;

		private TextBox TxbGround1;

		private CheckBox ChbGround1;

		private Label LabRuleDescTips;

		public ConfigBarcodeCheck()
		{
			InitializeComponent();
		}

		public void SetCBFunc(GetCfgCB getCfgCB)
		{
			GetCfgCBFunc = getCfgCB;
		}

		private void CodeToCMD_Load(object sender, EventArgs e)
		{
			ControlAndXML controlAndXML = new ControlAndXML();
			controlAndXML.XMLToControlByLinq(ToolCfg.ConfigPath, this);
		}

		public void CMD_Ack_CallBack(byte[] dat, int len)
		{
			cmd_ack = true;
		}

		public void Data_Ack_CallBack(byte[] dat, int len, out bool is_complete)
		{
			CheckBox[] need_send = new CheckBox[8];
			need_send[0] = ChbGround1;
			need_send[1] = ChbGround2;
			need_send[2] = ChbGround3;
			need_send[3] = ChbGround4;
			need_send[4] = ChbGround5;
			need_send[5] = ChbGround6;
			need_send[6] = ChbGround7;
			need_send[7] = ChbGround8;
			TextBox[] txt_box = new TextBox[8];
			txt_box[0] = TxbGround1;
			txt_box[1] = TxbGround2;
			txt_box[2] = TxbGround3;
			txt_box[3] = TxbGround4;
			txt_box[4] = TxbGround5;
			txt_box[5] = TxbGround6;
			txt_box[6] = TxbGround7;
			txt_box[7] = TxbGround8;
			int num = 0;
			if ((dat[0] == 2 && dat[1] == 0 && dat[2] == 116 && dat[3] == 38) || !rec_complete)
			{
				if (rec_complete)
				{
					end_len = 0;
					StringBuf[0].Clear();
					StringBuf[1].Clear();
					StringBuf[2].Clear();
					StringBuf[3].Clear();
					StringBuf[4].Clear();
					StringBuf[5].Clear();
					StringBuf[6].Clear();
					StringBuf[7].Clear();
					GroundLen[0] = dat[4];
					GroundLen[1] = dat[5];
					GroundLen[2] = dat[6];
					GroundLen[3] = dat[7];
					GroundLen[4] = dat[8];
					GroundLen[5] = dat[9];
					GroundLen[6] = dat[10];
					GroundLen[7] = dat[11];
					num = GroundLen[0] + GroundLen[1] + GroundLen[2] + GroundLen[3] + GroundLen[4] + GroundLen[5] + GroundLen[6] + GroundLen[7];
					if (num + 14 <= len && dat[num + 12] == 85 && dat[num + 13] == 170)
					{
						int num2 = 12;
						int k;
						for (k = 0; k < 8; k++)
						{
							if (GroundLen[k] > 0)
							{
								byte[] strbuf3 = new byte[GroundLen[k]];
								Array.Copy(dat, num2, strbuf3, 0, strbuf3.Length);
								num2 += GroundLen[k];
								Invoke((MethodInvoker)delegate
								{
									need_send[k].Checked = true;
									txt_box[k].Text = Encoding.Default.GetString(strbuf3);
								});
							}
							else
							{
								Invoke((MethodInvoker)delegate
								{
									need_send[k].Checked = false;
									txt_box[k].Text = "";
								});
							}
						}
						rec_complete = true;
						is_complete = true;
						return;
					}
					int num3 = 12;
					end_len = 0;
					int j;
					for (j = 0; j < 8; j++)
					{
						if (num3 + GroundLen[j] > len)
						{
							byte[] array = new byte[len - num3];
							Array.Copy(dat, num3, array, 0, array.Length);
							StringBuf[j].AddRange(array);
							cur_num = j;
							end_len = GroundLen[j] - array.Length;
							break;
						}
						if (GroundLen[j] > 0)
						{
							byte[] strbuf2 = new byte[GroundLen[j]];
							Array.Copy(dat, num3, strbuf2, 0, strbuf2.Length);
							num3 += GroundLen[j];
							Invoke((MethodInvoker)delegate
							{
								need_send[j].Checked = true;
								txt_box[j].Text = Encoding.Default.GetString(strbuf2);
							});
						}
						else
						{
							Invoke((MethodInvoker)delegate
							{
								need_send[j].Checked = false;
								txt_box[j].Text = "";
							});
						}
					}
					rec_complete = false;
					is_complete = false;
					return;
				}
				int num4 = 0;
				if (end_len > 0)
				{
					byte[] array2 = new byte[end_len];
					Array.Copy(dat, num4, array2, 0, array2.Length);
					StringBuf[cur_num].AddRange(array2);
					num4 += end_len;
					end_len = 0;
					Invoke((MethodInvoker)delegate
					{
						need_send[cur_num].Checked = true;
						txt_box[cur_num].Text = Encoding.Default.GetString(StringBuf[cur_num].ToArray());
					});
				}
				int i;
				for (i = cur_num + 1; i < 8; i++)
				{
					if (num4 + GroundLen[i] > len)
					{
						byte[] array3 = new byte[len - num4];
						Array.Copy(dat, num4, array3, 0, array3.Length);
						StringBuf[i].AddRange(array3);
						cur_num = i;
						end_len = GroundLen[i] - array3.Length;
						break;
					}
					cur_num = 0;
					if (GroundLen[i] > 0)
					{
						byte[] strbuf = new byte[GroundLen[i]];
						Array.Copy(dat, num4, strbuf, 0, strbuf.Length);
						num4 += GroundLen[i];
						Invoke((MethodInvoker)delegate
						{
							need_send[i].Checked = true;
							txt_box[i].Text = Encoding.Default.GetString(strbuf);
						});
					}
					else
					{
						Invoke((MethodInvoker)delegate
						{
							need_send[i].Checked = false;
							txt_box[i].Text = "";
						});
					}
				}
				if (cur_num == 0)
				{
					rec_complete = true;
					is_complete = true;
				}
				else
				{
					rec_complete = false;
					is_complete = false;
				}
			}
			else
			{
				rec_complete = true;
				is_complete = true;
			}
		}

		private void BtnReadCfg_Click(object sender, EventArgs e)
		{
			BtnReadCfg.Enabled = false;
			if (GetCfgCBFunc(3u) == 2 && ToolCfg.is_get_rawimg)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TypeTrigModeTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TypeTrigModeTips), MessageBoxButtons.OK);
			}
			else
			{
				List<byte> list = new List<byte>();
				list.Add(18);
				list.Add(0);
				list.Add(0);
				list.Add(0);
				list.Add(0);
				ushort num = HJ_CRC32.crc16(list.ToArray(), list.Count);
				list.Add((byte)((uint)(num >> 8) & 0xFFu));
				list.Add((byte)(num & 0xFFu));
				list.Insert(0, 0);
				list.Insert(0, 126);
				if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
				{
					ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, list.ToArray(), list.Count);
				}
			}
			BtnReadCfg.Enabled = true;
		}

		private void BtnWriteCfg_Click(object sender, EventArgs e)
		{
			BtnWriteCfg.Enabled = false;
			if (GetCfgCBFunc(3u) == 2 && ToolCfg.is_get_rawimg)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.TypeTrigModeTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.TypeTrigModeTips), MessageBoxButtons.OK);
			}
			else
			{
				bool[] array = new bool[8] { ChbGround1.Checked, ChbGround2.Checked, ChbGround3.Checked, ChbGround4.Checked, ChbGround5.Checked, ChbGround6.Checked, ChbGround7.Checked, ChbGround8.Checked };
				TextBox[] array2 = new TextBox[8] { TxbGround1, TxbGround2, TxbGround3, TxbGround4, TxbGround5, TxbGround6, TxbGround7, TxbGround8 };
				List<byte> list = new List<byte>();
				for (int i = 0; i < 8; i++)
				{
					if (array[i])
					{
						int textLength = array2[i].TextLength;
						list.Clear();
						list.Add(16);
						list.Add((byte)textLength);
						list.Add((byte)i);
						list.AddRange(Encoding.Default.GetBytes(array2[i].Text));
						ushort num = HJ_CRC32.crc16(list.ToArray(), list.Count);
						list.Add((byte)(num & 0xFFu));
						list.Add((byte)((uint)(num >> 8) & 0xFFu));
						list.Insert(0, 0);
						list.Insert(0, 126);
						if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
						{
							ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, list.ToArray(), list.Count);
						}
						Thread.Sleep(100);
					}
				}
				Invoke((MethodInvoker)delegate
				{
					if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
					{
						ToolCfg.UpdateAdjState = true;
						ToolCfg.SendReadBackCMD();
					}
				});
			}
			BtnWriteCfg.Enabled = true;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(合杰图像算法调试工具.ConfigBarcodeCheck));
			this.GrbExpectCode = new System.Windows.Forms.GroupBox();
			this.LabRuleDescTips = new System.Windows.Forms.Label();
			this.LabTips1 = new System.Windows.Forms.Label();
			this.BtnWriteCfg = new System.Windows.Forms.Button();
			this.BtnReadCfg = new System.Windows.Forms.Button();
			this.TxbGround8 = new System.Windows.Forms.TextBox();
			this.ChbGround8 = new System.Windows.Forms.CheckBox();
			this.TxbGround7 = new System.Windows.Forms.TextBox();
			this.ChbGround7 = new System.Windows.Forms.CheckBox();
			this.TxbGround6 = new System.Windows.Forms.TextBox();
			this.ChbGround6 = new System.Windows.Forms.CheckBox();
			this.TxbGround5 = new System.Windows.Forms.TextBox();
			this.ChbGround5 = new System.Windows.Forms.CheckBox();
			this.TxbGround4 = new System.Windows.Forms.TextBox();
			this.ChbGround4 = new System.Windows.Forms.CheckBox();
			this.TxbGround3 = new System.Windows.Forms.TextBox();
			this.ChbGround3 = new System.Windows.Forms.CheckBox();
			this.TxbGround2 = new System.Windows.Forms.TextBox();
			this.ChbGround2 = new System.Windows.Forms.CheckBox();
			this.TxbGround1 = new System.Windows.Forms.TextBox();
			this.ChbGround1 = new System.Windows.Forms.CheckBox();
			this.GrbExpectCode.SuspendLayout();
			base.SuspendLayout();
			this.GrbExpectCode.Controls.Add(this.LabRuleDescTips);
			this.GrbExpectCode.Controls.Add(this.LabTips1);
			this.GrbExpectCode.Controls.Add(this.BtnWriteCfg);
			this.GrbExpectCode.Controls.Add(this.BtnReadCfg);
			this.GrbExpectCode.Controls.Add(this.TxbGround8);
			this.GrbExpectCode.Controls.Add(this.ChbGround8);
			this.GrbExpectCode.Controls.Add(this.TxbGround7);
			this.GrbExpectCode.Controls.Add(this.ChbGround7);
			this.GrbExpectCode.Controls.Add(this.TxbGround6);
			this.GrbExpectCode.Controls.Add(this.ChbGround6);
			this.GrbExpectCode.Controls.Add(this.TxbGround5);
			this.GrbExpectCode.Controls.Add(this.ChbGround5);
			this.GrbExpectCode.Controls.Add(this.TxbGround4);
			this.GrbExpectCode.Controls.Add(this.ChbGround4);
			this.GrbExpectCode.Controls.Add(this.TxbGround3);
			this.GrbExpectCode.Controls.Add(this.ChbGround3);
			this.GrbExpectCode.Controls.Add(this.TxbGround2);
			this.GrbExpectCode.Controls.Add(this.ChbGround2);
			this.GrbExpectCode.Controls.Add(this.TxbGround1);
			this.GrbExpectCode.Controls.Add(this.ChbGround1);
			this.GrbExpectCode.Location = new System.Drawing.Point(1, 2);
			this.GrbExpectCode.Name = "GrbExpectCode";
			this.GrbExpectCode.Size = new System.Drawing.Size(563, 321);
			this.GrbExpectCode.TabIndex = 0;
			this.GrbExpectCode.TabStop = false;
			this.GrbExpectCode.Text = "预期条码设置";
			this.LabRuleDescTips.AutoSize = true;
			this.LabRuleDescTips.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.LabRuleDescTips.ForeColor = System.Drawing.Color.Red;
			this.LabRuleDescTips.Location = new System.Drawing.Point(69, 297);
			this.LabRuleDescTips.Name = "LabRuleDescTips";
			this.LabRuleDescTips.Size = new System.Drawing.Size(245, 12);
			this.LabRuleDescTips.TabIndex = 19;
			this.LabRuleDescTips.Text = "如需清除某组数据，在对应组数据留空并打勾";
			this.LabTips1.AutoSize = true;
			this.LabTips1.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.LabTips1.ForeColor = System.Drawing.Color.Red;
			this.LabTips1.Location = new System.Drawing.Point(34, 279);
			this.LabTips1.Name = "LabTips1";
			this.LabTips1.Size = new System.Drawing.Size(287, 12);
			this.LabTips1.TabIndex = 18;
			this.LabTips1.Text = "说明:通配符为'*'星号.前面打勾表明本次要配置的组";
			this.BtnWriteCfg.Location = new System.Drawing.Point(444, 233);
			this.BtnWriteCfg.Name = "BtnWriteCfg";
			this.BtnWriteCfg.Size = new System.Drawing.Size(110, 36);
			this.BtnWriteCfg.TabIndex = 17;
			this.BtnWriteCfg.Text = "写入配置";
			this.BtnWriteCfg.UseVisualStyleBackColor = true;
			this.BtnWriteCfg.Click += new System.EventHandler(BtnWriteCfg_Click);
			this.BtnReadCfg.Location = new System.Drawing.Point(318, 233);
			this.BtnReadCfg.Name = "BtnReadCfg";
			this.BtnReadCfg.Size = new System.Drawing.Size(110, 36);
			this.BtnReadCfg.TabIndex = 16;
			this.BtnReadCfg.Text = "回读配置";
			this.BtnReadCfg.UseVisualStyleBackColor = true;
			this.BtnReadCfg.Click += new System.EventHandler(BtnReadCfg_Click);
			this.TxbGround8.Location = new System.Drawing.Point(112, 203);
			this.TxbGround8.MaxLength = 59;
			this.TxbGround8.Name = "TxbGround8";
			this.TxbGround8.Size = new System.Drawing.Size(444, 23);
			this.TxbGround8.TabIndex = 15;
			this.ChbGround8.AutoSize = true;
			this.ChbGround8.Location = new System.Drawing.Point(36, 206);
			this.ChbGround8.Name = "ChbGround8";
			this.ChbGround8.Size = new System.Drawing.Size(82, 18);
			this.ChbGround8.TabIndex = 14;
			this.ChbGround8.Text = "第八组：";
			this.ChbGround8.UseVisualStyleBackColor = true;
			this.TxbGround7.Location = new System.Drawing.Point(112, 177);
			this.TxbGround7.MaxLength = 59;
			this.TxbGround7.Name = "TxbGround7";
			this.TxbGround7.Size = new System.Drawing.Size(444, 23);
			this.TxbGround7.TabIndex = 13;
			this.ChbGround7.AutoSize = true;
			this.ChbGround7.Location = new System.Drawing.Point(36, 180);
			this.ChbGround7.Name = "ChbGround7";
			this.ChbGround7.Size = new System.Drawing.Size(82, 18);
			this.ChbGround7.TabIndex = 12;
			this.ChbGround7.Text = "第七组：";
			this.ChbGround7.UseVisualStyleBackColor = true;
			this.TxbGround6.Location = new System.Drawing.Point(112, 151);
			this.TxbGround6.MaxLength = 59;
			this.TxbGround6.Name = "TxbGround6";
			this.TxbGround6.Size = new System.Drawing.Size(444, 23);
			this.TxbGround6.TabIndex = 11;
			this.ChbGround6.AutoSize = true;
			this.ChbGround6.Location = new System.Drawing.Point(36, 154);
			this.ChbGround6.Name = "ChbGround6";
			this.ChbGround6.Size = new System.Drawing.Size(82, 18);
			this.ChbGround6.TabIndex = 10;
			this.ChbGround6.Text = "第六组：";
			this.ChbGround6.UseVisualStyleBackColor = true;
			this.TxbGround5.Location = new System.Drawing.Point(112, 124);
			this.TxbGround5.MaxLength = 59;
			this.TxbGround5.Name = "TxbGround5";
			this.TxbGround5.Size = new System.Drawing.Size(444, 23);
			this.TxbGround5.TabIndex = 9;
			this.ChbGround5.AutoSize = true;
			this.ChbGround5.Location = new System.Drawing.Point(36, 128);
			this.ChbGround5.Name = "ChbGround5";
			this.ChbGround5.Size = new System.Drawing.Size(82, 18);
			this.ChbGround5.TabIndex = 8;
			this.ChbGround5.Text = "第五组：";
			this.ChbGround5.UseVisualStyleBackColor = true;
			this.TxbGround4.Location = new System.Drawing.Point(112, 98);
			this.TxbGround4.MaxLength = 59;
			this.TxbGround4.Name = "TxbGround4";
			this.TxbGround4.Size = new System.Drawing.Size(444, 23);
			this.TxbGround4.TabIndex = 7;
			this.ChbGround4.AutoSize = true;
			this.ChbGround4.Location = new System.Drawing.Point(36, 102);
			this.ChbGround4.Name = "ChbGround4";
			this.ChbGround4.Size = new System.Drawing.Size(82, 18);
			this.ChbGround4.TabIndex = 6;
			this.ChbGround4.Text = "第四组：";
			this.ChbGround4.UseVisualStyleBackColor = true;
			this.TxbGround3.Location = new System.Drawing.Point(112, 72);
			this.TxbGround3.MaxLength = 59;
			this.TxbGround3.Name = "TxbGround3";
			this.TxbGround3.Size = new System.Drawing.Size(444, 23);
			this.TxbGround3.TabIndex = 5;
			this.ChbGround3.AutoSize = true;
			this.ChbGround3.Location = new System.Drawing.Point(36, 76);
			this.ChbGround3.Name = "ChbGround3";
			this.ChbGround3.Size = new System.Drawing.Size(82, 18);
			this.ChbGround3.TabIndex = 4;
			this.ChbGround3.Text = "第三组：";
			this.ChbGround3.UseVisualStyleBackColor = true;
			this.TxbGround2.Location = new System.Drawing.Point(112, 46);
			this.TxbGround2.MaxLength = 59;
			this.TxbGround2.Name = "TxbGround2";
			this.TxbGround2.Size = new System.Drawing.Size(444, 23);
			this.TxbGround2.TabIndex = 3;
			this.ChbGround2.AutoSize = true;
			this.ChbGround2.Location = new System.Drawing.Point(36, 49);
			this.ChbGround2.Name = "ChbGround2";
			this.ChbGround2.Size = new System.Drawing.Size(82, 18);
			this.ChbGround2.TabIndex = 2;
			this.ChbGround2.Text = "第二组：";
			this.ChbGround2.UseVisualStyleBackColor = true;
			this.TxbGround1.Location = new System.Drawing.Point(112, 20);
			this.TxbGround1.MaxLength = 59;
			this.TxbGround1.Name = "TxbGround1";
			this.TxbGround1.Size = new System.Drawing.Size(444, 23);
			this.TxbGround1.TabIndex = 1;
			this.ChbGround1.AutoSize = true;
			this.ChbGround1.Location = new System.Drawing.Point(36, 23);
			this.ChbGround1.Name = "ChbGround1";
			this.ChbGround1.Size = new System.Drawing.Size(82, 18);
			this.ChbGround1.TabIndex = 0;
			this.ChbGround1.Text = "第一组：";
			this.ChbGround1.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(566, 325);
			base.Controls.Add(this.GrbExpectCode);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "ConfigBarcodeCheck";
			this.Text = "条码匹配设置";
			base.Load += new System.EventHandler(CodeToCMD_Load);
			this.GrbExpectCode.ResumeLayout(false);
			this.GrbExpectCode.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
