using System.Net;
using System.Net.Sockets;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core.Net;

public class AppSession : SessionBase
{
	internal EndPoint UdpEndPoint = null;

	public string LoginAlias { get; set; }

	public string ClientUniqueID { get; set; }

	internal byte[] BytesBuffer { get; set; }

	internal string KeyGroup { get; set; }

	public object Tag { get; set; }

	public AppSession()
	{
		ClientUniqueID = SoftBasic.GetUniqueStringByGuidAndRandom();
	}

	public AppSession(Socket socket)
		: base(socket)
	{
		ClientUniqueID = SoftBasic.GetUniqueStringByGuidAndRandom();
	}

	public override bool Equals(object obj)
	{
		return this == obj;
	}

	public override string ToString()
	{
		return string.IsNullOrEmpty(LoginAlias) ? $"AppSession[{base.IpEndPoint}]" : $"AppSession[{base.IpEndPoint}] [{LoginAlias}]";
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
