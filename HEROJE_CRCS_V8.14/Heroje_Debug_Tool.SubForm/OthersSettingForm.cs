using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using HJ_Controls_Lib;

namespace Heroje_Debug_Tool.SubForm
{
	public class OthersSettingForm : Form
	{
		private enum Image_Type
		{
			IMAGE_TYPE_JPG = 1,
			IMAGE_TYPE_BMP_8Bit,
			IMAGE_TYPE_RGB_16Bit,
			IMAGE_TYPE_BMP_24Bit,
			IMAGE_TYPE_YUV_16Bit,
			IMAGE_TYPE_GRAY_8Bit,
			IMAGE_TYPE_RAW_8Bit,
			IMAGE_TYPE_TIFF
		}

		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private static Image SaveImg = null;

		private byte[] DecodeData;

		private int CurrentImageWidth = 0;

		private int CurrentImageHeight = 0;

		private IContainer components = null;

		private TabControl TabProtocol;

		private TabPage TapForUploadImage;

		private Button BtnAddCurrentTraining;

		private Button BtnSelectUploadImage;

		private Button BtnUploadAndDecode;

		private PanelEx PanImageUploadShow;

		private Panel PanImageOutside;

		public OthersSettingForm()
		{
			InitializeComponent();
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		private void OthersSettingForm_Load(object sender, EventArgs e)
		{
		}

		public static byte[] Bitmap2Byte(Bitmap bitmap)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb))
				{
					Graphics.FromImage(bitmap2).DrawImage(bitmap, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
					bitmap2.Save(memoryStream, ImageFormat.Bmp);
					byte[] array = new byte[memoryStream.Length - 54];
					memoryStream.Seek(54L, SeekOrigin.Begin);
					memoryStream.Read(array, 0, Convert.ToInt32(memoryStream.Length - 54));
					byte[] array2 = new byte[bitmap.Width * bitmap.Height];
					int num = bitmap2.Height - 1;
					int num2 = 0;
					while (num > 0)
					{
						for (int i = 0; i < bitmap2.Width; i++)
						{
							array2[num2] = array[(num * bitmap2.Width + i) * 4];
							num2++;
						}
						num--;
					}
					return array2;
				}
			}
		}

		private static byte[] ConvertBayer8ToBGR(byte[] bayerImgDat, int width, int height)
		{
			byte[] array = new byte[width * height * 3];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < height / 2; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (num2 % 2 == 0)
					{
						num3 = ((i != 0) ? ((bayerImgDat[num2 + width] + bayerImgDat[num2 - width]) / 2) : bayerImgDat[num2 + width]);
						array[num] = (byte)num3;
						num++;
						array[num] = bayerImgDat[num2];
						num++;
						num4 = ((j != 0) ? ((bayerImgDat[num2 + 1] + bayerImgDat[num2 - 1]) / 2) : bayerImgDat[num2 + 1]);
						array[num] = (byte)num4;
						num++;
						num2++;
					}
					else
					{
						num3 = ((j != height / 2 - 1) ? ((bayerImgDat[num2 + 1 + width] + bayerImgDat[num2 - 1 + width]) / 2) : bayerImgDat[num2 - 1 + width]);
						array[num] = (byte)num3;
						num++;
						num5 = (bayerImgDat[num2 - 1] + bayerImgDat[num2 + width]) / 2;
						array[num] = (byte)num5;
						num++;
						array[num] = bayerImgDat[num2];
						num++;
						num2++;
					}
				}
				for (int k = 0; k < width; k++)
				{
					if (num2 % 2 == 0)
					{
						array[num] = bayerImgDat[num2];
						num++;
						num5 = (bayerImgDat[num2 + 1] + bayerImgDat[num2 - width]) / 2;
						array[num] = (byte)num5;
						num++;
						num4 = ((k != 0) ? ((bayerImgDat[num2 - 1 - width] + bayerImgDat[num2 + 1 - width]) / 2) : bayerImgDat[num2 + 1 - width]);
						array[num] = (byte)num4;
						num++;
						num2++;
					}
					else
					{
						num3 = ((k != width - 1) ? ((bayerImgDat[num2 + 1] + bayerImgDat[num2 - 1]) / 2) : bayerImgDat[num2 - 1]);
						array[num] = (byte)num3;
						num++;
						array[num] = bayerImgDat[num2];
						num++;
						num4 = ((i != height / 2 - 1) ? ((bayerImgDat[num2 + width] + bayerImgDat[num2 - width]) / 2) : bayerImgDat[num2 - width]);
						array[num] = (byte)num4;
						num++;
						num2++;
					}
				}
			}
			return array;
		}

		public static Bitmap Byte2Bitmap(byte[] data, int w, int h, PixelFormat pxl, int type)
		{
			switch (type)
			{
			case 1:
				if (data[0] == byte.MaxValue && data[1] == 216)
				{
					MemoryStream stream = new MemoryStream(w * h * 2 + 4096);
					MemoryStream stream2 = new MemoryStream(data);
					Bitmap bitmap2 = new Bitmap(stream2);
					bitmap2.Save(stream, ImageFormat.Jpeg);
					Bitmap bitmap3 = new Bitmap(stream);
					SaveImg = (Image)bitmap3.Clone();
					return bitmap3;
				}
				return new Bitmap(w, h);
			case 2:
				if (data[0] == 66 && data[1] == 77)
				{
					MemoryStream stream3 = new MemoryStream(w * h * 4 + 4096);
					MemoryStream stream4 = new MemoryStream(data);
					Bitmap bitmap6 = new Bitmap(stream4);
					bitmap6.Save(stream3, ImageFormat.Jpeg);
					Bitmap bitmap7 = new Bitmap(stream3);
					SaveImg = (Image)bitmap7.Clone();
					return bitmap7;
				}
				return new Bitmap(w, h);
			case 9:
			{
				Bitmap bitmap4 = new Bitmap(w, h, PixelFormat.Format24bppRgb);
				int num4 = 0;
				int num5 = 0;
				Rectangle rect2 = new Rectangle(0, 0, bitmap4.Width, bitmap4.Height);
				BitmapData bitmapData2 = bitmap4.LockBits(rect2, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
				IntPtr scan2 = bitmapData2.Scan0;
				Marshal.Copy(data, 0, scan2, w * h * 3);
				bitmap4.UnlockBits(bitmapData2);
				SaveImg = (Image)bitmap4.Clone();
				return bitmap4;
			}
			case 7:
			{
				int num6 = 0;
				int num7 = 0;
				byte[] array = ConvertBayer8ToBGR(data, w, h);
				Bitmap bitmap5 = new Bitmap(w, h, PixelFormat.Format32bppArgb);
				BitmapData bitmapData3 = bitmap5.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				IntPtr scan3 = bitmapData3.Scan0;
				for (num6 = 0; num6 < h; num6++)
				{
					for (num7 = 0; num7 < w; num7++)
					{
						Marshal.WriteInt32(scan3 + num6 * bitmapData3.Stride + 4 * num7, -16777216 | (array[(num6 * w + num7) * 3] << 16) | (array[(num6 * w + num7) * 3 + 1] << 8) | array[(num6 * w + num7) * 3 + 2]);
					}
				}
				bitmap5.UnlockBits(bitmapData3);
				return bitmap5;
			}
			default:
			{
				Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
				int num = 0;
				int num2 = 0;
				Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
				BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				IntPtr scan = bitmapData.Scan0;
				foreach (int num3 in data)
				{
					Marshal.WriteInt32(scan + num2 * bitmapData.Stride + 4 * num, -16777216 | (num3 << 16) | (num3 << 8) | num3);
					num++;
					if (num >= bitmap.Width)
					{
						num = 0;
						num2++;
						if (num2 >= bitmap.Height)
						{
							break;
						}
					}
				}
				bitmap.UnlockBits(bitmapData);
				SaveImg = (Image)bitmap.Clone();
				return bitmap;
			}
			}
		}

		private void SendImageToDevice()
		{
			try
			{
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPAddress address;
				if (!IPAddress.TryParse(ToolCfg.CurrentDevice.IpAddrStr, out address))
				{
					return;
				}
				BtnUploadAndDecode.Enabled = false;
				try
				{
					socket.Connect(new IPEndPoint(address.Address, 14328));
					byte[] array = new byte[8]
					{
						(byte)((uint)DecodeData.Length & 0xFFu),
						(byte)((DecodeData.Length & 0xFF00) >> 8),
						(byte)((DecodeData.Length & 0xFF0000) >> 16),
						(byte)((DecodeData.Length & 0xFF000000u) >> 24),
						(byte)((uint)CurrentImageWidth & 0xFFu),
						(byte)((CurrentImageWidth & 0xFF00) >> 8),
						(byte)((uint)CurrentImageHeight & 0xFFu),
						(byte)((CurrentImageHeight & 0xFF00) >> 8)
					};
					socket.Send(array, 0, array.Length, SocketFlags.None);
					Thread.Sleep(200);
					int num;
					for (int i = 0; i < DecodeData.Length; i += num)
					{
						num = ((i + 364000 > DecodeData.Length) ? (DecodeData.Length - i) : 364000);
						socket.Send(DecodeData, i, num, SocketFlags.None);
					}
				}
				catch (Exception ex)
				{
					socket.Close();
					MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
				}
				socket.Close();
				BtnUploadAndDecode.Enabled = true;
			}
			catch (Exception)
			{
			}
		}

		private void BtnSelectUploadImage_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "图像文件|*.bmp;*.jpg;*.png";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				PanImageUploadShow.BackgroundImageLayout = ImageLayout.Zoom;
				Bitmap bitmap = new Bitmap(openFileDialog.FileName);
				DecodeData = Bitmap2Byte(bitmap);
				PanImageUploadShow.BackgroundImage = Byte2Bitmap(DecodeData, bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb, 6);
				CurrentImageWidth = bitmap.Width;
				CurrentImageHeight = bitmap.Height;
			}
		}

		private void BtnUploadAndDecode_Click(object sender, EventArgs e)
		{
			SendCfgDataCBFuncCB(4194304u);
			SendImageToDevice();
		}

		private void BtnAddCurrentTraining_Click(object sender, EventArgs e)
		{
			SendCfgDataCBFuncCB(8388608u);
			SendImageToDevice();
		}

		private void PanImageUploadShow_DragDrop(object sender, DragEventArgs e)
		{
			string filename = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
			PanImageUploadShow.BackgroundImageLayout = ImageLayout.Zoom;
			Bitmap bitmap = new Bitmap(filename);
			DecodeData = Bitmap2Byte(bitmap);
			PanImageUploadShow.BackgroundImage = Byte2Bitmap(DecodeData, bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb, 6);
			CurrentImageWidth = bitmap.Width;
			CurrentImageHeight = bitmap.Height;
		}

		private void PanImageUploadShow_DragEnter(object sender, DragEventArgs e)
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

		private void PanImageOutside_DragDrop(object sender, DragEventArgs e)
		{
			PanImageUploadShow_DragDrop(sender, e);
		}

		private void PanImageOutside_DragEnter(object sender, DragEventArgs e)
		{
			PanImageUploadShow_DragEnter(sender, e);
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
			this.TabProtocol = new System.Windows.Forms.TabControl();
			this.TapForUploadImage = new System.Windows.Forms.TabPage();
			this.PanImageOutside = new System.Windows.Forms.Panel();
			this.PanImageUploadShow = new HJ_Controls_Lib.PanelEx();
			this.BtnAddCurrentTraining = new System.Windows.Forms.Button();
			this.BtnSelectUploadImage = new System.Windows.Forms.Button();
			this.BtnUploadAndDecode = new System.Windows.Forms.Button();
			this.TabProtocol.SuspendLayout();
			this.TapForUploadImage.SuspendLayout();
			this.PanImageOutside.SuspendLayout();
			base.SuspendLayout();
			this.TabProtocol.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TabProtocol.Controls.Add(this.TapForUploadImage);
			this.TabProtocol.Location = new System.Drawing.Point(12, 12);
			this.TabProtocol.Name = "TabProtocol";
			this.TabProtocol.SelectedIndex = 0;
			this.TabProtocol.Size = new System.Drawing.Size(711, 392);
			this.TabProtocol.TabIndex = 12;
			this.TapForUploadImage.Controls.Add(this.PanImageOutside);
			this.TapForUploadImage.Controls.Add(this.BtnAddCurrentTraining);
			this.TapForUploadImage.Controls.Add(this.BtnSelectUploadImage);
			this.TapForUploadImage.Controls.Add(this.BtnUploadAndDecode);
			this.TapForUploadImage.Location = new System.Drawing.Point(4, 24);
			this.TapForUploadImage.Name = "TapForUploadImage";
			this.TapForUploadImage.Padding = new System.Windows.Forms.Padding(3);
			this.TapForUploadImage.Size = new System.Drawing.Size(703, 364);
			this.TapForUploadImage.TabIndex = 2;
			this.TapForUploadImage.Text = "传图解码";
			this.TapForUploadImage.UseVisualStyleBackColor = true;
			this.PanImageOutside.AllowDrop = true;
			this.PanImageOutside.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanImageOutside.Controls.Add(this.PanImageUploadShow);
			this.PanImageOutside.Location = new System.Drawing.Point(6, 3);
			this.PanImageOutside.Name = "PanImageOutside";
			this.PanImageOutside.Size = new System.Drawing.Size(568, 355);
			this.PanImageOutside.TabIndex = 4;
			this.PanImageOutside.DragDrop += new System.Windows.Forms.DragEventHandler(PanImageOutside_DragDrop);
			this.PanImageOutside.DragEnter += new System.Windows.Forms.DragEventHandler(PanImageOutside_DragEnter);
			this.PanImageUploadShow.ActionNum = 0;
			this.PanImageUploadShow.ArcStartAngle = 45;
			this.PanImageUploadShow.ArcSweepAngle = 90;
			this.PanImageUploadShow.BackColor = System.Drawing.Color.Silver;
			this.PanImageUploadShow.ImageZoomRate = 1;
			this.PanImageUploadShow.IsDefaultMode = true;
			this.PanImageUploadShow.Location = new System.Drawing.Point(5, 6);
			this.PanImageUploadShow.Name = "PanImageUploadShow";
			this.PanImageUploadShow.PanelBackColor = System.Drawing.Color.FromArgb(189, 189, 189);
			this.PanImageUploadShow.PanelForeColor = System.Drawing.Color.FromArgb(189, 189, 189);
			this.PanImageUploadShow.Size = new System.Drawing.Size(555, 340);
			this.PanImageUploadShow.TabIndex = 13;
			this.PanImageUploadShow.DragDrop += new System.Windows.Forms.DragEventHandler(PanImageUploadShow_DragDrop);
			this.PanImageUploadShow.DragEnter += new System.Windows.Forms.DragEventHandler(PanImageUploadShow_DragEnter);
			this.BtnAddCurrentTraining.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnAddCurrentTraining.Location = new System.Drawing.Point(590, 194);
			this.BtnAddCurrentTraining.Name = "BtnAddCurrentTraining";
			this.BtnAddCurrentTraining.Size = new System.Drawing.Size(89, 39);
			this.BtnAddCurrentTraining.TabIndex = 3;
			this.BtnAddCurrentTraining.Text = "加入训练";
			this.BtnAddCurrentTraining.UseVisualStyleBackColor = true;
			this.BtnAddCurrentTraining.Click += new System.EventHandler(BtnAddCurrentTraining_Click);
			this.BtnSelectUploadImage.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnSelectUploadImage.Location = new System.Drawing.Point(590, 88);
			this.BtnSelectUploadImage.Name = "BtnSelectUploadImage";
			this.BtnSelectUploadImage.Size = new System.Drawing.Size(89, 39);
			this.BtnSelectUploadImage.TabIndex = 2;
			this.BtnSelectUploadImage.Text = "选择图片";
			this.BtnSelectUploadImage.UseVisualStyleBackColor = true;
			this.BtnSelectUploadImage.Click += new System.EventHandler(BtnSelectUploadImage_Click);
			this.BtnUploadAndDecode.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnUploadAndDecode.Location = new System.Drawing.Point(590, 141);
			this.BtnUploadAndDecode.Name = "BtnUploadAndDecode";
			this.BtnUploadAndDecode.Size = new System.Drawing.Size(89, 39);
			this.BtnUploadAndDecode.TabIndex = 1;
			this.BtnUploadAndDecode.Text = "上传并解码";
			this.BtnUploadAndDecode.UseVisualStyleBackColor = true;
			this.BtnUploadAndDecode.Click += new System.EventHandler(BtnUploadAndDecode_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(735, 689);
			base.Controls.Add(this.TabProtocol);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "OthersSettingForm";
			this.Text = "OthersSettingForm";
			base.Load += new System.EventHandler(OthersSettingForm_Load);
			this.TabProtocol.ResumeLayout(false);
			this.TapForUploadImage.ResumeLayout(false);
			this.PanImageOutside.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
