using System;

namespace HslCommunication.LogNet;

public class LogNetException : Exception
{
	public LogNetException(Exception innerException)
		: base(innerException.Message, innerException)
	{
	}
}
