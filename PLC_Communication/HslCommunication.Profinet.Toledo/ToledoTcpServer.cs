using System;
using HslCommunication.Core.Net;

namespace HslCommunication.Profinet.Toledo;

public class ToledoTcpServer : CommunicationServer
{
	public delegate void ToledoStandardDataReceivedDelegate(object sender, ToledoStandardData toledoStandardData);

	public bool HasChk { get; set; } = true;

	public event ToledoStandardDataReceivedDelegate OnToledoStandardDataReceived;

	public ToledoTcpServer()
	{
		base.OnPipeMessageReceived += ToledoTcpServer_OnPipeMessageReceived;
	}

	private void ToledoTcpServer_OnPipeMessageReceived(PipeSession session, byte[] buffer)
	{
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + buffer.ToHexString(' '));
		ToledoStandardData toledoStandardData = null;
		try
		{
			toledoStandardData = new ToledoStandardData(buffer);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), "ToledoStandardData new failed: " + buffer.ToHexString(' '), ex);
		}
		if (toledoStandardData != null)
		{
			this.OnToledoStandardDataReceived?.Invoke(session, toledoStandardData);
		}
	}

	public override string ToString()
	{
		return $"ToledoTcpServer[{base.Port}]";
	}
}
