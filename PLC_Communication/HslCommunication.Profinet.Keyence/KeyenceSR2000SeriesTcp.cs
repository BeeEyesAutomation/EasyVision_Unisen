using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Keyence;

public class KeyenceSR2000SeriesTcp : NetworkDoubleBase, IKeyenceSR2000Series
{
	public KeyenceSR2000SeriesTcp()
	{
		base.ReceiveTimeOut = 10000;
		base.SleepTime = 20;
	}

	public KeyenceSR2000SeriesTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	[HslMqttApi]
	public OperateResult<string> ReadBarcode()
	{
		return KeyenceSR2000Helper.ReadBarcode(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult Reset()
	{
		return KeyenceSR2000Helper.Reset(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult OpenIndicator()
	{
		return KeyenceSR2000Helper.OpenIndicator(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult CloseIndicator()
	{
		return KeyenceSR2000Helper.CloseIndicator(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult<string> ReadVersion()
	{
		return KeyenceSR2000Helper.ReadVersion(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult<string> ReadCommandState()
	{
		return KeyenceSR2000Helper.ReadCommandState(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult<string> ReadErrorState()
	{
		return KeyenceSR2000Helper.ReadErrorState(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult<bool> CheckInput(int number)
	{
		return KeyenceSR2000Helper.CheckInput(number, ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult SetOutput(int number, bool value)
	{
		return KeyenceSR2000Helper.SetOutput(number, value, ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult<int[]> ReadRecord()
	{
		return KeyenceSR2000Helper.ReadRecord(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult Lock()
	{
		return KeyenceSR2000Helper.Lock(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult UnLock()
	{
		return KeyenceSR2000Helper.UnLock(ReadFromCoreServer);
	}

	[HslMqttApi]
	public OperateResult<string> ReadCustomer(string command)
	{
		return KeyenceSR2000Helper.ReadCustomer(command, ReadFromCoreServer);
	}

	public override string ToString()
	{
		return $"KeyenceSR2000SeriesTcp[{IpAddress}:{Port}]";
	}
}
