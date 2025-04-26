using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace 合杰图像算法调试工具
{
	internal class GetSystemIcon
	{
		public enum FileInfoFlags : uint
		{
			SHGFI_ICON = 256u,
			SHGFI_DISPLAYNAME = 512u,
			SHGFI_TYPENAME = 1024u,
			SHGFI_ATTRIBUTES = 2048u,
			SHGFI_ICONLOCATION = 4096u,
			SHGFI_EXETYPE = 8192u,
			SHGFI_SYSICONINDEX = 16384u,
			SHGFI_LINKOVERLAY = 32768u,
			SHGFI_SELECTED = 65536u,
			SHGFI_ATTR_SPECIFIED = 131072u,
			SHGFI_LARGEICON = 0u,
			SHGFI_SMALLICON = 1u,
			SHGFI_OPENICON = 2u,
			SHGFI_SHELLICONSIZE = 4u,
			SHGFI_PIDL = 8u,
			SHGFI_USEFILEATTRIBUTES = 16u,
			SHGFI_ADDOVERLAYS = 32u,
			SHGFI_OVERLAYINDEX = 64u
		}

		public enum FileAttributeFlags : uint
		{
			FILE_ATTRIBUTE_READONLY = 1u,
			FILE_ATTRIBUTE_HIDDEN = 2u,
			FILE_ATTRIBUTE_SYSTEM = 4u,
			FILE_ATTRIBUTE_DIRECTORY = 0x10u,
			FILE_ATTRIBUTE_ARCHIVE = 0x20u,
			FILE_ATTRIBUTE_DEVICE = 0x40u,
			FILE_ATTRIBUTE_NORMAL = 0x80u,
			FILE_ATTRIBUTE_TEMPORARY = 0x100u,
			FILE_ATTRIBUTE_SPARSE_FILE = 0x200u,
			FILE_ATTRIBUTE_REPARSE_POINT = 0x400u,
			FILE_ATTRIBUTE_COMPRESSED = 0x800u,
			FILE_ATTRIBUTE_OFFLINE = 0x1000u,
			FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x2000u,
			FILE_ATTRIBUTE_ENCRYPTED = 0x4000u
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;

			public int iIcon;

			public uint dwAttributes;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}

		[DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

		[DllImport("User32.dll")]
		public static extern int DestroyIcon(IntPtr hIcon);

		public static Icon GetIconByFileName(string fileName)
		{
			if (fileName == null || fileName.Equals(string.Empty))
			{
				return null;
			}
			SHFILEINFO psfi = default(SHFILEINFO);
			IntPtr intPtr = SHGetFileInfo(fileName, 0u, ref psfi, (uint)Marshal.SizeOf(psfi), 273u);
			Icon result = Icon.FromHandle(psfi.hIcon).Clone() as Icon;
			DestroyIcon(psfi.hIcon);
			return result;
		}

		public static Icon GetIconByFileType(string fileType, bool isLarge)
		{
			if (fileType == null || fileType.Equals(string.Empty))
			{
				return null;
			}
			RegistryKey registryKey = null;
			string text = null;
			string text2 = null;
			string text3 = Environment.SystemDirectory + "\\";
			if (fileType[0] == '.')
			{
				registryKey = Registry.ClassesRoot.OpenSubKey(fileType, true);
				if (registryKey != null)
				{
					text = registryKey.GetValue("") as string;
					registryKey.Close();
					registryKey = Registry.ClassesRoot.OpenSubKey(text + "\\DefaultIcon", true);
					if (registryKey != null)
					{
						text2 = registryKey.GetValue("") as string;
						registryKey.Close();
					}
				}
				if (text2 == null)
				{
					text2 = text3 + "shell32.dll,0";
				}
			}
			else
			{
				text2 = text3 + "shell32.dll,3";
			}
			string[] array = text2.Split(',');
			if (array.Length != 2)
			{
				array = new string[2]
				{
					text3 + "shell32.dll",
					"2"
				};
			}
			Icon result = null;
			try
			{
				int[] array2 = new int[1];
				int[] array3 = new int[1];
				uint num = Win32.ExtractIconEx(array[0], int.Parse(array[1]), array2, array3, 1u);
				IntPtr handle = new IntPtr(isLarge ? array2[0] : array3[0]);
				result = Icon.FromHandle(handle);
			}
			catch
			{
			}
			return result;
		}
	}
}
