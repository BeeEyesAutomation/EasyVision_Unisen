using System;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Core.Net;

public class AlienSession
{
	public PipeTcpNet Pipe { get; set; }

	public string DTU { get; set; }

	public string Pwd { get; set; }

	public bool IsStatusOk { get; set; }

	public DateTime OnlineTime { get; set; }

	public DateTime OfflineTime { get; set; }

	public AlienSession()
	{
		IsStatusOk = true;
		OnlineTime = DateTime.Now;
		OfflineTime = DateTime.MinValue;
	}

	public void Offline()
	{
		if (IsStatusOk)
		{
			IsStatusOk = false;
			OfflineTime = DateTime.Now;
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("DtuSession[" + DTU + "] [" + (IsStatusOk ? "Online" : "Offline") + "]");
		if (IsStatusOk)
		{
			stringBuilder.Append(" [" + SoftBasic.GetTimeSpanDescription(DateTime.Now - OnlineTime) + "]");
		}
		else if (OfflineTime == DateTime.MinValue)
		{
			stringBuilder.Append(" [----]");
		}
		else
		{
			stringBuilder.Append(" [" + SoftBasic.GetTimeSpanDescription(DateTime.Now - OfflineTime) + "]");
		}
		return stringBuilder.ToString();
	}
}
