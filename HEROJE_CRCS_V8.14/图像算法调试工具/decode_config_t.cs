using System.Runtime.InteropServices;

namespace 图像算法调试工具
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct decode_config_t
	{
		public int config_flag;

		public int timeout;

		public ulong type_switch;

		public int decode_lib;

		public int mulit_codes;

		public int decode_level;

		public int exconfig_flag;
	}
}
