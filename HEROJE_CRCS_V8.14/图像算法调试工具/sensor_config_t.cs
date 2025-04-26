using System.Runtime.InteropServices;

namespace 图像算法调试工具
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sensor_config_t
	{
		public int config_flag;

		public int exp_time;

		public int gain_set;

		public int focus_set;

		public int light_ctrl;

		public int light_pwm1;

		public int light_pwm2;

		public int res;
	}
}
