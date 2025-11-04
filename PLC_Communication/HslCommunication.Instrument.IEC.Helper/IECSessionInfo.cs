using System.Threading;

namespace HslCommunication.Instrument.IEC.Helper;

public class IECSessionInfo
{
	private int sendMessageID = 0;

	private int recvMessageID = 0;

	public int RecvMessageID => recvMessageID;

	public int GetSendMessageID()
	{
		int result = sendMessageID;
		Interlocked.Increment(ref sendMessageID);
		return result;
	}

	public int IncrRecvMessageID()
	{
		int result = Interlocked.Increment(ref recvMessageID);
		if (recvMessageID > 32767)
		{
			recvMessageID = 0;
		}
		return result;
	}
}
