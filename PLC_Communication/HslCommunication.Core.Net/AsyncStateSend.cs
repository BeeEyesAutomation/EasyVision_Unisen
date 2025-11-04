using System.Net.Sockets;

namespace HslCommunication.Core.Net;

internal class AsyncStateSend
{
	internal Socket WorkSocket { get; set; }

	internal byte[] Content { get; set; }

	internal int AlreadySendLength { get; set; }

	internal SimpleHybirdLock HybirdLockSend { get; set; }

	internal string Key { get; set; }

	internal string ClientId { get; set; }
}
