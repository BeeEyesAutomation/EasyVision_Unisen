using System.Text;

namespace HslCommunication.Profinet.Special;

public class EcFanData
{
	public bool EmergencyState { get; set; } = false;

	public bool RunState { get; set; }

	public bool LockState { get; set; }

	public bool OverHotState { get; set; }

	public bool LostSpeedState { get; set; }

	public int Speed { get; set; } = 0;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("应急状态: " + (EmergencyState ? "处于应急状态" : "正常状态"));
		stringBuilder.AppendLine("运行状态: " + (RunState ? "运行中" : "停止"));
		stringBuilder.AppendLine("堵转状态: " + (LockState ? "处于堵转中" : "正常状态"));
		stringBuilder.AppendLine("过热状态: " + (OverHotState ? "驱动器处于过热状态" : "驱动器处于正常状态"));
		stringBuilder.AppendLine("失速状态: " + (LostSpeedState ? "处于失速中" : "正常状态"));
		stringBuilder.Append("实际转速: " + Speed);
		return stringBuilder.ToString();
	}
}
