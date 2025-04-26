using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Heroje_Debug_Tool.SubForm;

namespace Heroje_Debug_Tool.BaseClass
{
	internal class ParameterTraining
	{
		private enum AllBarcodeType
		{
			EAN13,
			EAN8,
			UPCA,
			UPCE,
			UPCE1,
			CODE128,
			CODE39,
			CODE93,
			CODABAR,
			IL25,
			ID25,
			MT25,
			CODE11,
			MSI,
			DATABAR,
			DATABAR_LIM,
			DATABAR_EXP,
			QR,
			DM,
			PDF417,
			AZTEC,
			HAXIN,
			MPDF417,
			TRIOPTIC,
			CODEBLOCK_F,
			STRAIGHT25,
			TELEPEN,
			MAXICODE,
			PHARMA,
			CODE32
		}

		public delegate void TrainingInfoCB(TrainingStateDef Act, int Val_1, string Val_2);

		public enum TrainingStateDef
		{
			ChangeIntoContinueMode,
			UpdateLabStateString,
			UpdateAdjProcessValue,
			ShowExposureVal,
			ShowSensorGainVal,
			ShowBarcodeSwitch,
			ShowMainLightVal,
			OpenDefaultBarcodeType,
			TrainFinish
		}

		private static int[] BarcodeCfgAddr = new int[32];

		private static int AutoAdjustCount = 0;

		private static int decode_ok_type = 0;

		private static bool decode_ok_flag = false;

		private static bool adjust_ok_flag = false;

		private static int exp_time;

		private static int gain_set;

		private static Random rand = new Random(0);

		private static int exp_idx = 0;

		private static int gain_rand_idx = 0;

		private static int last_light_max = 0;

		private static int rec_now_exp = 0;

		private static int rec_now_gain = 0;

		private static int[] rec_last_exp = new int[5];

		private static int[] rec_last_gain = new int[5];

		private static int[] rec_ok_exp = new int[5];

		private static int[] rec_ok_gain = new int[5];

		private static int adj_ok_count = 0;

		private static int adj_check_count = 0;

		private static bool is_small_exp = true;

		private static int[] gain_array = new int[22]
		{
			1, 8, 16, 31, 48, 63, 88, 127, 160, 200,
			255, 10, 20, 28, 38, 50, 61, 96, 100, 115,
			180, 235
		};

		private static int[] exp_array = new int[34]
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			12, 14, 16, 18, 20, 23, 26, 29, 34, 39,
			45, 52, 60, 70, 82, 98, 116, 130, 158, 192,
			230, 242, 250, 255
		};

		private static Point[] code_rect;

		private const int OK_VALUE_SET = 100;

		private System.Timers.Timer ParameterAdjustTimer;

		private bool IsParameterAdjustTimerInitOk = false;

		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private TrainingInfoCB TrainingInfoCBFuncCB;

		private Image ImageShow = new Bitmap(376, 240);

		private string LabStateADPText = "";

		public ProgressBar PgbAutoAdjProcess;

		public bool IsTraining = false;

		private void ParaAdjTimer_Init()
		{
			if (!IsParameterAdjustTimerInitOk)
			{
				ParameterAdjustTimer = new System.Timers.Timer(300.0);
				ParameterAdjustTimer.Elapsed += ParameterAdjustTimer_Elapsed;
				ParameterAdjustTimer.Stop();
				IsParameterAdjustTimerInitOk = true;
			}
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB, TrainingInfoCB trainingInfoCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
			TrainingInfoCBFuncCB = trainingInfoCB;
		}

		public void UpdateDecodeState(DeviceConnectForm.Polygon BarCodeRegion0, bool DecOkFlag, Image img)
		{
			if (DecOkFlag)
			{
				code_rect = BarCodeRegion0.ToPointArray();
			}
			decode_ok_flag = DecOkFlag;
			if (img != null)
			{
				ImageShow = (Image)img.Clone();
			}
		}

		public void Do_Ai_Training()
		{
			if (!ToolCfg.UpdateAdjState)
			{
				IsTraining = true;
				if (ToolCfg.DeviceTypeRecord == 6 || ToolCfg.DeviceTypeRecord == 7 || ToolCfg.DeviceTypeRecord == 12 || ToolCfg.DeviceTypeRecord == 16 || ToolCfg.DeviceTypeRecord == 18)
				{
					SendCfgDataCBFuncCB(262144u);
					return;
				}
				ParaAdjTimer_Init();
				BtnEnterADP_Click(null, null);
			}
		}

		public void Stop_Ai_Training()
		{
			IsTraining = false;
			PgbAutoAdjProcess.Invoke((MethodInvoker)delegate
			{
				TrainingInfoCBFuncCB(TrainingStateDef.TrainFinish, 0, "");
				PgbAutoAdjProcess.Visible = false;
				PgbAutoAdjProcess.Value = 0;
			});
			if (ToolCfg.DeviceTypeRecord == 6 || ToolCfg.DeviceTypeRecord == 7 || ToolCfg.DeviceTypeRecord == 12 || ToolCfg.DeviceTypeRecord == 16 || ToolCfg.DeviceTypeRecord == 18)
			{
				SendCfgDataCBFuncCB(524288u);
				return;
			}
			try
			{
				ParameterAdjustTimer.Stop();
			}
			catch (Exception)
			{
			}
		}

		private void ParameterAdjustTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (AutoAdjustCount < 300)
			{
				if (GetCfgCBFuncCB(3u) != 2)
				{
					TrainingInfoCBFuncCB(TrainingStateDef.ChangeIntoContinueMode, 0, "");
				}
				if (decode_ok_flag && !adjust_ok_flag)
				{
					Image image = ((ToolCfg.SaveImg != null) ? ToolCfg.SaveImg : ImageShow);
					int num = CalcMaxLightPointInImage(code_rect, (Bitmap)image);
					if (num > 100 && num < 254)
					{
						adjust_ok_flag = true;
						if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
						{
							LabStateADPText = "查找最佳..";
						}
						else
						{
							LabStateADPText = "FindNine..";
						}
						TrainingInfoCBFuncCB(TrainingStateDef.UpdateLabStateString, 0, LabStateADPText);
					}
					else if (num < 254)
					{
						if (last_light_max < 254 && last_light_max < num)
						{
							last_light_max = num;
							rec_ok_exp = (int[])rec_last_exp.Clone();
							rec_ok_gain = (int[])rec_last_gain.Clone();
						}
					}
					else if (last_light_max > num && last_light_max > 254)
					{
						last_light_max = num;
						rec_ok_exp = (int[])rec_last_exp.Clone();
						rec_ok_gain = (int[])rec_last_gain.Clone();
					}
				}
				if (adjust_ok_flag)
				{
					adj_check_count++;
					if (adj_check_count > 50)
					{
						PgbAutoAdjProcess.Invoke((MethodInvoker)delegate
						{
							PgbAutoAdjProcess.Value = PgbAutoAdjProcess.Maximum;
						});
						if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
						{
							LabStateADPText = "调参不成功!";
						}
						else
						{
							LabStateADPText = "ADJFailed!";
						}
						TrainingInfoCBFuncCB(TrainingStateDef.UpdateLabStateString, 0, LabStateADPText);
						exp_idx = 0;
						AutoAdjustCount = 0;
						adj_ok_count = 0;
						Stop_Ai_Training();
						adj_check_count = 0;
						adjust_ok_flag = false;
					}
					else if (adj_check_count > 40)
					{
						rec_now_exp = rec_last_exp[4];
						rec_now_gain = rec_last_gain[4];
					}
					else if (adj_check_count > 30)
					{
						rec_now_exp = rec_last_exp[3];
						rec_now_gain = rec_last_gain[3];
					}
					else if (adj_check_count > 20)
					{
						rec_now_exp = rec_last_exp[2];
						rec_now_gain = rec_last_gain[2];
					}
					else if (adj_check_count > 10)
					{
						rec_now_exp = rec_last_exp[1];
						rec_now_gain = rec_last_gain[1];
					}
					else
					{
						rec_now_exp = rec_last_exp[0];
						rec_now_gain = rec_last_gain[0];
					}
					TrainingInfoCBFuncCB(TrainingStateDef.ShowExposureVal, rec_now_exp, "");
					TrainingInfoCBFuncCB(TrainingStateDef.ShowSensorGainVal, rec_now_gain, "");
					SetCfgCBFuncCB(4607u, (uint)rec_now_exp);
					SetCfgCBFuncCB(51455u, (uint)rec_now_gain);
					SendCfgDataCBFuncCB(128u);
					if (!decode_ok_flag)
					{
						return;
					}
					adj_ok_count++;
					if (adj_ok_count <= 3)
					{
						return;
					}
					PgbAutoAdjProcess.Invoke((MethodInvoker)delegate
					{
						PgbAutoAdjProcess.Value = PgbAutoAdjProcess.Maximum;
					});
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						LabStateADPText = "调参成功!";
					}
					else
					{
						LabStateADPText = "Success!";
					}
					TrainingInfoCBFuncCB(TrainingStateDef.UpdateLabStateString, 0, LabStateADPText);
					adj_ok_count = 0;
					adjust_ok_flag = false;
					Stop_Ai_Training();
					SetCfgCBFuncCB(3u, 0u);
					SendCfgDataCBFuncCB(64u);
					Thread.Sleep(500);
					if (decode_ok_type > 0)
					{
						SetCfgCBFuncCB(11777u, 0u);
						SetCfgCBFuncCB(13057u, 0u);
						SetCfgCBFuncCB(13825u, 0u);
						SetCfgCBFuncCB(14593u, 0u);
						SetCfgCBFuncCB(21505u, 0u);
						SetCfgCBFuncCB(16129u, 0u);
						SetCfgCBFuncCB(21761u, 0u);
						SetCfgCBFuncCB(21505u, 0u);
						if (BarcodeCfgAddr[decode_ok_type & 0x1F] > 0)
						{
							SetCfgCBFuncCB(0xFFu | (uint)(BarcodeCfgAddr[decode_ok_type & 0x1F] << 8), 1u);
						}
					}
					TrainingInfoCBFuncCB(TrainingStateDef.ShowBarcodeSwitch, 0, "");
					SetCfgCBFuncCB(4607u, (uint)rec_now_exp);
					SetCfgCBFuncCB(51455u, (uint)rec_now_gain);
					SetCfgCBFuncCB(3u, 2u);
					SendCfgDataCBFuncCB(1216u);
					exp_idx = 0;
					AutoAdjustCount = 0;
					last_light_max = 0;
					return;
				}
				adj_check_count = 0;
				if (gain_rand_idx == 0)
				{
					if (is_small_exp)
					{
						is_small_exp = !is_small_exp;
						exp_time = exp_array[exp_idx];
					}
					else
					{
						is_small_exp = !is_small_exp;
						exp_idx++;
						if (exp_idx == (exp_array.Length + 1) / 2)
						{
							exp_idx = 0;
							if (last_light_max > 0)
							{
								rec_last_exp = (int[])rec_ok_exp.Clone();
								rec_last_gain = (int[])rec_ok_gain.Clone();
								adjust_ok_flag = true;
								if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
								{
									LabStateADPText = "查找最佳..";
								}
								else
								{
									LabStateADPText = "FindNine..";
								}
								TrainingInfoCBFuncCB(TrainingStateDef.UpdateLabStateString, 0, LabStateADPText);
								return;
							}
							PgbAutoAdjProcess.Invoke((MethodInvoker)delegate
							{
								PgbAutoAdjProcess.Value = PgbAutoAdjProcess.Maximum;
							});
							if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
							{
								LabStateADPText = "调参失败!";
							}
							else
							{
								LabStateADPText = "Failed!";
							}
							TrainingInfoCBFuncCB(TrainingStateDef.UpdateLabStateString, 0, LabStateADPText);
							exp_idx = 0;
							AutoAdjustCount = 0;
							adj_ok_count = 0;
							Stop_Ai_Training();
						}
						else
						{
							exp_time = exp_array[exp_array.Length / 2 + exp_idx];
						}
					}
				}
				PgbAutoAdjProcess.Invoke((MethodInvoker)delegate
				{
					PgbAutoAdjProcess.PerformStep();
				});
				gain_rand_idx++;
				if (gain_rand_idx > 2)
				{
					gain_rand_idx = 0;
				}
				gain_set = gain_array[rand.Next(0, gain_array.Length)];
				TrainingInfoCBFuncCB(TrainingStateDef.ShowExposureVal, exp_time, "");
				TrainingInfoCBFuncCB(TrainingStateDef.ShowSensorGainVal, gain_set, "");
				rec_last_exp[4] = rec_last_exp[0];
				rec_last_gain[4] = rec_last_gain[0];
				rec_last_exp[3] = rec_last_exp[0];
				rec_last_gain[3] = rec_last_gain[0];
				rec_last_exp[2] = rec_last_exp[0];
				rec_last_gain[2] = rec_last_gain[0];
				rec_last_exp[1] = rec_last_exp[0];
				rec_last_gain[1] = rec_last_gain[0];
				rec_last_exp[0] = exp_time;
				rec_last_gain[0] = gain_set;
				SetCfgCBFuncCB(4607u, (uint)exp_time);
				SetCfgCBFuncCB(51455u, (uint)gain_set);
				SendCfgDataCBFuncCB(128u);
			}
			else
			{
				PgbAutoAdjProcess.Invoke((MethodInvoker)delegate
				{
					PgbAutoAdjProcess.Value = PgbAutoAdjProcess.Maximum;
				});
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					LabStateADPText = "调参失败!";
				}
				else
				{
					LabStateADPText = "Failed!";
				}
				TrainingInfoCBFuncCB(TrainingStateDef.UpdateLabStateString, 0, LabStateADPText);
				adj_ok_count = 0;
				exp_idx = 0;
				AutoAdjustCount = 0;
				Stop_Ai_Training();
			}
		}

		private void BtnEnterADP_Click(object sender, EventArgs e)
		{
			BarcodeCfgAddr[0] = 46;
			BarcodeCfgAddr[1] = 46;
			BarcodeCfgAddr[2] = 46;
			BarcodeCfgAddr[3] = 46;
			BarcodeCfgAddr[4] = 46;
			BarcodeCfgAddr[5] = 51;
			BarcodeCfgAddr[6] = 54;
			BarcodeCfgAddr[7] = 57;
			BarcodeCfgAddr[8] = 60;
			BarcodeCfgAddr[9] = 64;
			BarcodeCfgAddr[10] = 67;
			BarcodeCfgAddr[11] = 70;
			BarcodeCfgAddr[12] = 73;
			BarcodeCfgAddr[13] = 76;
			BarcodeCfgAddr[14] = 95;
			BarcodeCfgAddr[15] = 80;
			BarcodeCfgAddr[16] = 81;
			BarcodeCfgAddr[17] = 63;
			BarcodeCfgAddr[18] = 84;
			BarcodeCfgAddr[19] = 85;
			BarcodeCfgAddr[20] = 86;
			BarcodeCfgAddr[21] = 87;
			BarcodeCfgAddr[22] = 88;
			BarcodeCfgAddr[23] = 89;
			BarcodeCfgAddr[24] = 90;
			BarcodeCfgAddr[25] = 91;
			BarcodeCfgAddr[26] = 92;
			BarcodeCfgAddr[27] = 93;
			BarcodeCfgAddr[29] = 94;
			BarcodeCfgAddr[28] = 181;
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			if (ToolCfg.DeviceTypeRecord == 6 || ToolCfg.DeviceTypeRecord == 7 || ToolCfg.DeviceTypeRecord == 12 || ToolCfg.DeviceTypeRecord == 16 || ToolCfg.DeviceTypeRecord == 18)
			{
				SendCfgDataCBFuncCB(262144u);
				return;
			}
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				LabStateADPText = "智能调参...";
			}
			else
			{
				LabStateADPText = "Adjusting..";
			}
			TrainingInfoCBFuncCB(TrainingStateDef.UpdateLabStateString, 0, LabStateADPText);
			SetCfgCBFuncCB(1279u, 255u);
			TrainingInfoCBFuncCB(TrainingStateDef.ShowMainLightVal, 255, "");
			TrainingInfoCBFuncCB(TrainingStateDef.OpenDefaultBarcodeType, 0, "");
			Application.DoEvents();
			Thread.Sleep(1000);
			SetCfgCBFuncCB(11777u, 1u);
			SetCfgCBFuncCB(13057u, 1u);
			SetCfgCBFuncCB(13825u, 1u);
			SetCfgCBFuncCB(14593u, 1u);
			SetCfgCBFuncCB(16129u, 1u);
			SetCfgCBFuncCB(21761u, 0u);
			SetCfgCBFuncCB(21505u, 1u);
			SendCfgDataCBFuncCB(0u);
			PgbAutoAdjProcess.Visible = true;
			PgbAutoAdjProcess.Value = 0;
			PgbAutoAdjProcess.Maximum = exp_array.Length * 3;
			decode_ok_flag = false;
			exp_time = GetCfgCBFuncCB(4607u);
			gain_set = GetCfgCBFuncCB(51455u);
			rec_last_exp[2] = exp_time;
			rec_last_gain[2] = gain_set;
			ParameterAdjustTimer.Start();
		}

		private int CalcMaxLightPointInImage(Point[] roi, Bitmap img)
		{
			Rectangle rect = FindMaxRectFormPoint(roi);
			BitmapData bitmapData = img.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			IntPtr scan = bitmapData.Scan0;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < rect.Height; i++)
			{
				IntPtr intPtr = scan + i * bitmapData.Stride;
				for (int j = 0; j < rect.Width; j++)
				{
					num = Marshal.ReadInt32(intPtr + j * 3) & 0xFF;
					if (num > num2)
					{
						num2 = num;
					}
				}
			}
			img.UnlockBits(bitmapData);
			return num2;
		}

		private Rectangle FindMaxRectFormPoint(Point[] points)
		{
			int num = int.MaxValue;
			Point point = new Point(0, 0);
			for (int i = 0; i < points.Length; i++)
			{
				Point point2 = points[i];
				if (point2.X < num)
				{
					num = point2.X;
					point = point2;
				}
			}
			int num2 = int.MinValue;
			Point point3 = new Point(0, 0);
			for (int j = 0; j < points.Length; j++)
			{
				Point point4 = points[j];
				if (point4.X > num2)
				{
					num2 = point4.X;
					point3 = point4;
				}
			}
			int num3 = int.MaxValue;
			Point point5 = new Point(0, 0);
			for (int k = 0; k < points.Length; k++)
			{
				Point point6 = points[k];
				if (point6.Y < num3)
				{
					num3 = point6.Y;
					point5 = point6;
				}
			}
			int num4 = int.MinValue;
			Point point7 = new Point(0, 0);
			for (int l = 0; l < points.Length; l++)
			{
				Point point8 = points[l];
				if (point8.Y > num4)
				{
					num4 = point8.Y;
					point7 = point8;
				}
			}
			int width = point3.X - point.X;
			int height = point7.Y - point5.Y;
			return new Rectangle(point.X, point5.Y, width, height);
		}
	}
}
