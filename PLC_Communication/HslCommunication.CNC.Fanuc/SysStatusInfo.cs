namespace HslCommunication.CNC.Fanuc;

public class SysStatusInfo
{
	public short Dummy { get; set; }

	public short TMMode { get; set; }

	public CNCWorkMode WorkMode { get; set; }

	public CNCRunStatus RunStatus { get; set; }

	public short Motion { get; set; }

	public short MSTB { get; set; }

	public short Emergency { get; set; }

	public short Alarm { get; set; }

	public short Edit { get; set; }

	public override string ToString()
	{
		return $"Dummy: {Dummy}, TMMode:{TMMode}, WorkMode:{WorkMode}, RunStatus:{RunStatus}, " + $"Motion:{Motion}, MSTB:{MSTB}, Emergency:{Emergency}, Alarm:{Alarm}, Edit:{Edit}";
	}
}
