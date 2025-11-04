using System.Threading;

namespace HslCommunication.Core.Net;

internal class StateOneBase
{
	public int DataLength { get; set; } = 32;

	public int AlreadyDealLength { get; set; }

	public ManualResetEvent WaitDone { get; set; }

	public byte[] Buffer { get; set; }

	public bool IsError { get; set; }

	public string ErrerMsg { get; set; }
}
