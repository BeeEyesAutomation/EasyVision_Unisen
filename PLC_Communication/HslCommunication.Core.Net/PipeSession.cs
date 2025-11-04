using System;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Core.Net;

public class PipeSession
{
	public DateTime OnlineTime { get; set; } = DateTime.Now;

	public DateTime HeartTime { get; set; } = DateTime.Now;

	public object Tag { get; set; }

	public CommunicationPipe Communication { get; set; }

	public string SessionID { get; set; }

	public virtual void Close()
	{
		Communication?.CloseCommunication();
	}

	public override string ToString()
	{
		if (Communication == null)
		{
			return "Session<NULL>";
		}
		return $"Session<{Communication}>";
	}
}
