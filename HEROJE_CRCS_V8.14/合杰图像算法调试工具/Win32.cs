using System;
using System.Runtime.InteropServices;

namespace 合杰图像算法调试工具
{
	internal class Win32
	{
		public const uint SHGFI_ICON = 256u;

		public const uint SHGFI_LARGEICON = 0u;

		public const uint SHGFI_SMALLICON = 1u;

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("shell32.dll")]
		public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);
	}
}
