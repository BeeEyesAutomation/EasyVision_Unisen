using System.Text;

namespace HslCommunication.Profinet.Omron;

public class OmronCpuUnitStatus
{
	public string Status { get; set; }

	public string BatteryStatus { get; set; }

	public string CpuStatus { get; set; }

	public string Mode { get; set; }

	public int ErrorCode { get; set; }

	public string ErrorMessage { get; set; }

	public OmronCpuUnitStatus()
	{
	}

	public OmronCpuUnitStatus(byte[] data)
	{
		Status = (data[0].GetBoolByIndex(0) ? "Run" : "Stop");
		BatteryStatus = (data[0].GetBoolByIndex(2) ? "Present" : "No");
		CpuStatus = (data[0].GetBoolByIndex(7) ? "Standby" : "Normal");
		Mode = ((data[1] == 0) ? "PROGRAM" : ((data[1] == 2) ? "MONITOR" : ((data[1] == 4) ? "RUN" : "")));
		ErrorCode = data[8] * 256 + data[9];
		if (ErrorCode > 0)
		{
			ErrorMessage = Encoding.ASCII.GetString(data, 10, 16).TrimEnd(' ', '\0');
		}
	}

	public override string ToString()
	{
		return "OmronCpuUnitStatus[" + Status + "]";
	}
}
