using System.Runtime.InteropServices;

namespace 图像算法调试工具
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct imgproc_config_t
	{
		public int self_id;

		public int proc_id;

		public int config_flag;

		public int arg_flag;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] in_id;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] out_id;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
		public int[] res;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public int[] args;
	}
}
