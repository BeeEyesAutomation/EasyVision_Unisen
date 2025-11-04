namespace HslCommunication.Profinet.OpenProtocol;

public class TighteningResultMessages
{
	private OpenProtocolNet openProtocol;

	public TighteningResultMessages(OpenProtocolNet openProtocol)
	{
		this.openProtocol = openProtocol;
	}

	public OperateResult LastTighteningResultDataSubscribe(int revision)
	{
		return openProtocol.ReadCustomer(60, revision, -1, -1, null);
	}

	public OperateResult LastTighteningResultDataUnsubscribe()
	{
		return openProtocol.ReadCustomer(63, 1, -1, -1, null);
	}
}
