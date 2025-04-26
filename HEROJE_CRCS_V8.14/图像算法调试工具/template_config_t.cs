using System.Runtime.InteropServices;

namespace 图像算法调试工具
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct template_config_t
	{
		public uint head_magic;

		public int template_type;

		public int template_id;

		public int config_flag;

		public int all_timeout;

		public int img_proc_cnt;

		public int parent_id;

		public int last_id;

		public int next_id;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public int[] res;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public byte[] name;

		public roi_config_t roi_config;

		public decode_config_t decode_config;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public imgproc_config_t[] imgproc_config;

		public sensor_config_t sensor_config;
	}
}
