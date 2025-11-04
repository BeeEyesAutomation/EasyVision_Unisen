using System.Net;
using System.Text;

namespace HslCommunication.Core.Net;

public class RemoteConnectInfo
{
	private bool ack = false;

	private string dtuId = string.Empty;

	public IPEndPoint EndPoint { get; set; }

	public PipeSession Session { get; set; }

	public byte[] DtuBytes { get; set; }

	public bool NeedAckResult => ack;

	public string DtuId => dtuId;

	public DtuStatus Status { get; set; } = DtuStatus.Create;

	public RemoteConnectInfo(string ipAddress, int port, byte[] dtu)
	{
		EndPoint = new IPEndPoint(IPAddress.Parse(HslHelper.GetIpAddressFromInput(ipAddress)), port);
		DtuBytes = dtu;
	}

	public RemoteConnectInfo(string ipAddress, int port, string dtuId, string password = "", bool needAckResult = true)
	{
		this.dtuId = dtuId;
		EndPoint = new IPEndPoint(IPAddress.Parse(HslHelper.GetIpAddressFromInput(ipAddress)), port);
		DtuBytes = CreateHslAlienMessage(dtuId, password, needAckResult);
		ack = needAckResult;
	}

	private byte[] CreateHslAlienMessage(string dtuId, string password, bool needAckResult)
	{
		if (dtuId.Length > 11)
		{
			dtuId = dtuId.Substring(11);
		}
		byte[] array = new byte[30]
		{
			72, 83, 76, 0, 25, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};
		if (dtuId.Length > 11)
		{
			dtuId = dtuId.Substring(0, 11);
		}
		Encoding.ASCII.GetBytes(dtuId).CopyTo(array, 5);
		if (!string.IsNullOrEmpty(password))
		{
			if (password.Length > 6)
			{
				password = password.Substring(6);
			}
			Encoding.ASCII.GetBytes(password).CopyTo(array, 16);
		}
		if (!needAckResult)
		{
			array[28] = 1;
		}
		return array;
	}

	public void Close()
	{
		Status = DtuStatus.Closed;
		Session?.Close();
	}
}
