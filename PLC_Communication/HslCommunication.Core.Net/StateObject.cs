using System.Net.Sockets;

namespace HslCommunication.Core.Net;

internal class StateObject : StateOneBase
{
	public string UniqueId { get; set; }

	public Socket WorkSocket { get; set; }

	public bool IsClose { get; set; }

	public StateObject()
	{
	}

	public StateObject(int length)
	{
		base.DataLength = length;
		base.Buffer = new byte[length];
	}

	public void Clear()
	{
		base.IsError = false;
		IsClose = false;
		base.AlreadyDealLength = 0;
		base.Buffer = null;
	}
}
