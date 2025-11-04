using System;

namespace HslCommunication.Profinet.OpenProtocol;

public class OpenEventArgs : EventArgs
{
	public string Content { get; set; }

	public OpenEventArgs()
	{
	}

	public OpenEventArgs(string content)
	{
		Content = content;
	}
}
