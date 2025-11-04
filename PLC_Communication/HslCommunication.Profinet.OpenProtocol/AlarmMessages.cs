namespace HslCommunication.Profinet.OpenProtocol;

public class AlarmMessages
{
	private OpenProtocolNet openProtocol;

	public AlarmMessages(OpenProtocolNet openProtocol)
	{
		this.openProtocol = openProtocol;
	}

	public OperateResult AlarmSubscrib()
	{
		return openProtocol.ReadCustomer(70, 1, -1, -1, null);
	}

	public OperateResult AlarmUnsubscribe()
	{
		return openProtocol.ReadCustomer(73, 1, -1, -1, null);
	}

	public OperateResult AcknowledgeAlarmRemotelyOnController()
	{
		return openProtocol.ReadCustomer(78, 1, -1, -1, null);
	}
}
