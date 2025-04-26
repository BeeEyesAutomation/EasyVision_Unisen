using System.Runtime.InteropServices;

namespace 图像算法调试工具
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct roi_config_t
	{
		public int config_flag;

		public int timeout;

		public int top;

		public int left;

		public int width;

		public int height;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] res;
	}
}
