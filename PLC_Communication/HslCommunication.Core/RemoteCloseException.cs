using System;

namespace HslCommunication.Core;

public class RemoteCloseException : Exception
{
	public RemoteCloseException()
		: base("Remote Closed Exception")
	{
	}
}
