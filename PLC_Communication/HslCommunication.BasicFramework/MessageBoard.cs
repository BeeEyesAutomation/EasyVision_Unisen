using System;

namespace HslCommunication.BasicFramework;

public class MessageBoard
{
	public string NameSend { get; set; } = "";

	public string NameReceive { get; set; } = "";

	public DateTime SendTime { get; set; } = DateTime.Now;

	public string Content { get; set; } = "";

	public bool HasViewed { get; set; }
}
