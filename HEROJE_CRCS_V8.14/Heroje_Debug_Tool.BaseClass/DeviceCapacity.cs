using System;
using System.Runtime.InteropServices;
using 合杰图像算法调试工具;

namespace Heroje_Debug_Tool.BaseClass
{
	internal class DeviceCapacity
	{
		public enum DeviceTypeNumber
		{
			H291 = 1,
			HM2,
			HM3,
			HM4,
			HM535,
			HM6,
			HM7,
			H723,
			H296,
			Tiny,
			WL03,
			HR_X,
			HM270,
			HM2_V3,
			DEC_BOX,
			HR_XM,
			Micro,
			HRX_RGB,
			HM271,
			HM233
		}

		public enum CapacityIndex
		{
			AutoFocus,
			DecodeOkSound,
			FocusLed,
			SecondLight,
			MainLight,
			FarLight_LT,
			FarLight_LB,
			FarLight_RT,
			FarLight_RB,
			NearLight_Left,
			NearLight_Right,
			NearLight_Top,
			NearLight_Bottom,
			PolarizationLight,
			Dpm_Dm_Setting,
			TemplateSettingNew,
			OutputInternetSetting,
			DeviceSaveOkImage,
			DeviceSaveNgImage,
			FtpSaveOkImage,
			FtpSaveNgImage,
			UploadImageDecode,
			CodeQuality,
			GroupNet,
			FtpServer
		}

		public struct DeviceCapacityInfoStu
		{
			public DeviceTypeNumber DevTypeNum;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public bool[] CapacityArr;

			public bool IsNetworkDevice;

			public string DeviceName;

			public DeviceCapacityInfoStu(int a)
			{
				DevTypeNum = DeviceTypeNumber.HR_XM;
				CapacityArr = new bool[64];
				IsNetworkDevice = false;
				DeviceName = "None";
			}
		}

		public static DeviceCapacityInfoStu GetCapacityInfo(byte DeviceType, DeviceFindAndCom.DeviceType ConnectType, string DeviceName)
		{
			bool[] array = new bool[64];
			if (DeviceType == 16)
			{
				array[0] = true;
			}
			if (DeviceType == 9 || DeviceType == 2 || DeviceType == 20 || DeviceType == 13 || DeviceType == 19 || DeviceType == 10 || DeviceType == 16)
			{
				array[1] = true;
			}
			if (DeviceType == 9 || DeviceType == 17 || DeviceType == 2 || DeviceType == 20 || DeviceType == 13 || DeviceType == 19 || DeviceType == 16)
			{
				array[2] = true;
			}
			if (DeviceType == 17 || DeviceType == 2 || DeviceType == 20 || DeviceType == 13 || DeviceType == 19 || DeviceType == 10 || DeviceType == 16)
			{
				array[3] = false;
			}
			if (DeviceType == 9 || DeviceType == 17 || DeviceType == 2 || DeviceType == 20 || DeviceType == 13 || DeviceType == 19)
			{
				array[4] = true;
			}
			if (DeviceType == 9)
			{
				array[5] = true;
				array[6] = true;
				array[7] = true;
				array[8] = true;
			}
			if (DeviceType == 10)
			{
				array[11] = true;
				array[12] = true;
			}
			if (DeviceType == 13 || DeviceType == 19)
			{
				array[5] = true;
				array[7] = true;
				array[8] = true;
			}
			if (DeviceType == 16)
			{
				if (DeviceName.Contains("HRX-013F") || DeviceName.Contains("HRX-020"))
				{
					array[5] = true;
					array[6] = true;
					array[7] = true;
					array[8] = true;
				}
				else
				{
					array[5] = true;
					array[6] = true;
					array[7] = true;
					array[8] = true;
					array[12] = true;
				}
			}
			if (DeviceType == 16 && !DeviceName.Contains("HRX-013F"))
			{
				array[13] = true;
			}
			if (DeviceType == 20 || DeviceType == 13 || DeviceType == 19 || DeviceType == 10 || DeviceType == 16)
			{
				array[14] = true;
			}
			if ((DeviceType == 20 && ConnectType == DeviceFindAndCom.DeviceType.NETWORK) || DeviceType == 13 || DeviceType == 19 || DeviceType == 10 || DeviceType == 16)
			{
				array[15] = true;
			}
			if (DeviceType == 13 || DeviceType == 19 || DeviceType == 10 || DeviceType == 16)
			{
				array[16] = true;
			}
			if (DeviceType == 10)
			{
				array[19] = true;
				array[20] = true;
			}
			if (DeviceType == 16)
			{
				array[17] = true;
				array[18] = true;
				array[19] = true;
				array[20] = true;
			}
			if (DeviceType == 16)
			{
				array[21] = true;
				array[22] = true;
				array[23] = true;
			}
			if (DeviceType == 13 || DeviceType == 19 || DeviceType == 10 || DeviceType == 16)
			{
				array[24] = true;
			}
			if (DeviceType == 17 || DeviceType == 2 || DeviceType == 20 || DeviceType == 13 || DeviceType == 19 || DeviceType == 10 || DeviceType == 16)
			{
			}
			DeviceCapacityInfoStu result = new DeviceCapacityInfoStu(0);
			result.DevTypeNum = (DeviceTypeNumber)DeviceType;
			Array.Copy(array, result.CapacityArr, array.Length);
			if (ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
			{
				result.IsNetworkDevice = true;
			}
			result.DeviceName = DeviceName;
			return result;
		}
	}
}
