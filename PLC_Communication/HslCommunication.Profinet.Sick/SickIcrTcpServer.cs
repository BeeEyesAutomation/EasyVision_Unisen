using System.Text;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Profinet.Sick;

public class SickIcrTcpServer : CommunicationServer
{
	public delegate void ReceivedBarCodeDelegate(string ipAddress, string barCode);

	public event ReceivedBarCodeDelegate OnReceivedBarCode;

	public SickIcrTcpServer()
	{
		base.OnPipeMessageReceived += SickIcrTcpServer_OnPipeMessageReceived;
	}

	private void SickIcrTcpServer_OnPipeMessageReceived(PipeSession session, byte[] buffer)
	{
		string ipAddress = string.Empty;
		if (session.Communication is PipeTcpNet pipeTcpNet)
		{
			ipAddress = pipeTcpNet.IpAddress;
		}
		if (session != null)
		{
			base.LogNet?.WriteDebug(ToString(), $"<{session.Communication}> Recv: " + buffer.ToHexString(' '));
		}
		this.OnReceivedBarCode?.Invoke(ipAddress, TranslateCode(Encoding.ASCII.GetString(buffer)));
	}

	private string TranslateCode(string code)
	{
		StringBuilder stringBuilder = new StringBuilder("");
		for (int i = 0; i < code.Length; i++)
		{
			if (char.IsLetterOrDigit(code, i))
			{
				stringBuilder.Append(code[i]);
			}
		}
		return stringBuilder.ToString();
	}

	public override string ToString()
	{
		return $"SickIcrTcpServer[{base.Port}]";
	}
}
