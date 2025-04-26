using System;
using System.Runtime.InteropServices;

namespace 合杰图像算法调试工具
{
	public struct SHFILEINFO
	{
		public IntPtr hIcon;

		public IntPtr iIcon;

		public uint dwAttributes;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szDisplayName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string szTypeName;
	}
}
