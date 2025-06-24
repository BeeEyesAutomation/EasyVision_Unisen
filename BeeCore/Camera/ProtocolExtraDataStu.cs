using System;
using System.Runtime.InteropServices;

namespace BeeCore
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ProtocolExtraDataStu
	{
		public uint TrigNum;

		public uint TrigCount;

		public uint OKCount;

		public uint NGCount;

		public uint DeviceNameLen;

		public uint FullNameLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public byte[] ExtDeviceName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] ExtFullName;

		public uint ClientCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] ClientIPs;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Rfu;

		public void DataFormByteArray(byte[] dat)
		{
			if (dat.Length >= 256)
			{
				TrigNum = (uint)(dat[0] | (dat[1] << 8) | (dat[2] << 16) | (dat[3] << 24));
				TrigCount = (uint)(dat[4] | (dat[5] << 8) | (dat[6] << 16) | (dat[7] << 24));
				OKCount = (uint)(dat[8] | (dat[9] << 8) | (dat[10] << 16) | (dat[11] << 24));
				NGCount = (uint)(dat[12] | (dat[13] << 8) | (dat[14] << 16) | (dat[15] << 24));
				DeviceNameLen = (uint)(dat[16] | (dat[17] << 8) | (dat[18] << 16) | (dat[19] << 24));
				FullNameLen = (uint)(dat[20] | (dat[21] << 8) | (dat[22] << 16) | (dat[23] << 24));
				ExtDeviceName = new byte[64];
				Array.Copy(dat, 24, ExtDeviceName, 0, 64);
				ExtFullName = new byte[128];
				Array.Copy(dat, 88, ExtFullName, 0, 128);
				ClientCount = (uint)(dat[216] | (dat[217] << 8) | (dat[218] << 16) | (dat[219] << 24));
				ClientIPs = new byte[32];
				Array.Copy(dat, 220, ClientIPs, 0, 32);
			}
		}
	}
}
