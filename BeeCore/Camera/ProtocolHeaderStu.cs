using System.Runtime.InteropServices;

namespace BeeCore
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ProtocolHeaderStu
	{
		public uint Header;

		public uint Crc32;

		public ushort BarCodeLen;

		public byte BarCodeType;

		public byte ImageType;

		public ushort ImageWidth;

		public ushort ImageHeight;

		public uint ImageSize;

		public uint DecodeTime;

		public uint DataSwitch;

		public uint ParaCrc32;

		public uint ParaAction;

		public uint DeviceID;

		public byte BarCodeNum;

		public byte ScanModeNum;

		public ushort SensorXStart;

		public ushort SensorYStart;

		public ushort SensorWidth;

		public ushort SensorHeight;

		public ushort ParaDataLen;

		public byte DeviceType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public byte[] DeviceName;

		public void DataFormByteArray(byte[] dat)
		{
			if (dat.Length >= 64)
			{
				Header = (uint)(dat[0] | (dat[1] << 8) | (dat[2] << 16) | (dat[3] << 24));
				Crc32 = (uint)(dat[4] | (dat[5] << 8) | (dat[6] << 16) | (dat[7] << 24));
				BarCodeLen = (ushort)(dat[8] | (dat[9] << 8));
				BarCodeType = dat[10];
				ImageType = dat[11];
				ImageWidth = (ushort)(dat[12] | (dat[13] << 8));
				ImageHeight = (ushort)(dat[14] | (dat[15] << 8));
				ImageSize = (uint)(dat[16] | (dat[17] << 8) | (dat[18] << 16) | (dat[19] << 24));
				DecodeTime = (uint)(dat[20] | (dat[21] << 8) | (dat[22] << 16) | (dat[23] << 24));
				DataSwitch = (uint)(dat[24] | (dat[25] << 8) | (dat[26] << 16) | (dat[27] << 24));
				ParaCrc32 = (uint)(dat[28] | (dat[29] << 8) | (dat[30] << 16) | (dat[31] << 24));
				ParaAction = (uint)(dat[32] | (dat[33] << 8) | (dat[34] << 16) | (dat[35] << 24));
				DeviceID = (uint)(dat[36] | (dat[37] << 8) | (dat[38] << 16) | (dat[39] << 24));
				BarCodeNum = dat[40];
				ScanModeNum = dat[41];
				SensorXStart = (ushort)(dat[42] | (dat[43] << 8));
				SensorYStart = (ushort)(dat[44] | (dat[45] << 8));
				SensorWidth = (ushort)(dat[46] | (dat[47] << 8));
				SensorHeight = (ushort)(dat[48] | (dat[49] << 8));
				ParaDataLen = (ushort)(dat[50] | (dat[51] << 8));
				DeviceType = dat[52];
				DeviceName = new byte[11];
				DeviceName[0] = dat[53];
				DeviceName[1] = dat[54];
				DeviceName[2] = dat[55];
				DeviceName[3] = dat[56];
				DeviceName[4] = dat[57];
				DeviceName[5] = dat[58];
				DeviceName[6] = dat[59];
				DeviceName[7] = dat[60];
				DeviceName[8] = dat[61];
				DeviceName[9] = dat[62];
				DeviceName[10] = dat[63];
			}
		}

		public byte[] DataToByteArray()
		{
			byte[] array = new byte[64];
			array[0] = (byte)(Header & 0xFFu);
			array[0] = (byte)(Header & 0xFFu);
			array[0] = (byte)(Header & 0xFFu);
			array[0] = (byte)(Header & 0xFFu);
			return array;
		}
	}
}
