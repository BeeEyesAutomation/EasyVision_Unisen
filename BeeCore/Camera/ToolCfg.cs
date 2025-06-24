using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;


namespace BeeCore
{
	public class ToolCfg
	{
		public struct TempRoiRectInfo
		{
			public int Temp_ID;

			public string RoiName;

			public Rectangle Rect;

			public TempRoiRectInfo(int id, string roi_name, Rectangle rec)
			{
				Temp_ID = id;
				RoiName = roi_name;
				Rect = rec;
			}
		}

		public enum SoftwareDef
		{
			Heroje_Standard,
			Hide_Heroje_Logo
		}

		public static string ConfigPath = "ChineseS.xml";

		public static bool UpdateAdjState = true;

		public static bool is_stop = false;

		public static uint DeviceTypeRecord = 0u;

		public static int ParaDataLen;

		public static Rectangle MaskRect;

		public static List<TempRoiRectInfo> TemplateRoiList = new List<TempRoiRectInfo>();

		public static Pen MaskPen = new Pen(new SolidBrush(Color.FromArgb(200, 0, 200, 0)), 2f);

		public static ushort SensorWidth;

		public static ushort SensorHeight;

		public static int width_offset = 0;

		public static int height_offset = 0;

		public static Image SaveImg = null;

		public static Image TemplateImg = null;

		public static bool is_exit_and_save = true;

		public static bool is_manual_trig = false;

		public static bool is_quit_debug = false;

		public static bool IsDispNetGrid = false;

		public static DeviceFindAndCom.DeviceFound CurrentDevice;

		public static Queue<double> DecoderTimeQueue = new Queue<double>(100);

	//	public static ConfigBarcodeCheck ConfigBarcodeCheckForm = new ConfigBarcodeCheck();

		//public static CMD_Tool FormCMD_Tool = new CMD_Tool();

		public static bool is_RdbTrigExtern_checked = false;

		public static bool is_RdbOuputByCOM_checked = false;

		public static bool is_get_rawimg = true;

		public static int IpAddrSeg0;

		public static int IpAddrSeg1;

		public static int IpAddrSeg2;

		public static int IpAddrSeg3;

		//public static FormDeviceIP DeviceIPSettingPage;

		//public static FtpUtil ftp = new FtpUtil();

		public static int Temp_MainLight;

		public static int Temp_SecondLight;

		public static int Temp_ExpVal;

		public static int Temp_GainVal;

		public static int TemplateRoiX;

		public static int TemplateRoiY;

		public static int TemplateRoiW;

		public static int TemplateRoiH;

		public static int ThisScreenWidth;

		public static int ThisScreenHeight;

		public static int SplitDisConnectPage;

		public static int SplitDisReadingPage_h;

		public static int SplitDisReadingPage_v;

		public static Color[] TemplateColor = new Color[20]
		{
			Color.Blue,
			Color.Navy,
			Color.CornflowerBlue,
			Color.MediumSlateBlue,
			Color.Crimson,
			Color.DarkRed,
			Color.DarkSalmon,
			Color.Orange,
			Color.OrangeRed,
			Color.PaleGreen,
			Color.MediumSeaGreen,
			Color.CadetBlue,
			Color.DarkSlateGray,
			Color.DarkMagenta,
			Color.MediumVioletRed,
			Color.Brown,
			Color.Bisque,
			Color.Cyan,
			Color.Green,
			Color.LightGray
		};

		public static SoftwareDef ThisSoftware = SoftwareDef.Heroje_Standard;

		public static string VersionInfo = "Ver8.14";

		public static void SendReadBackCMD()
		{
			if (CurrentDevice != null && CurrentDevice.IsConnect)
			{
				byte[] array = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
				CurrentDevice.SendData(CurrentDevice.DeviceHandle, array, array.Length);
			}
		}

		public static object BytesToStruct(byte[] bytes, Type strcutType)
		{
			int num = Marshal.SizeOf(strcutType);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.Copy(bytes, 0, intPtr, num);
				return Marshal.PtrToStructure(intPtr, strcutType);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		public static byte[] StructToBytes(object obj)
		{
			int num = Marshal.SizeOf(obj);
			byte[] array = new byte[num];
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(obj, intPtr, false);
			Marshal.Copy(intPtr, array, 0, num);
			Marshal.FreeHGlobal(intPtr);
			return array;
		}
	}
}
