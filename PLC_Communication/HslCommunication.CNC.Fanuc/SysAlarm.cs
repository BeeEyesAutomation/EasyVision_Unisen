namespace HslCommunication.CNC.Fanuc;

public class SysAlarm
{
	public int AlarmId { get; set; }

	public short Type { get; set; }

	public short Axis { get; set; }

	public string Message { get; set; }
}
