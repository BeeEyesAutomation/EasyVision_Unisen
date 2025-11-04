using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Keyence;

internal interface IKeyenceSR2000Series
{
	[HslMqttApi]
	OperateResult<string> ReadBarcode();

	[HslMqttApi]
	OperateResult Reset();

	[HslMqttApi]
	OperateResult OpenIndicator();

	[HslMqttApi]
	OperateResult CloseIndicator();

	[HslMqttApi]
	OperateResult<string> ReadVersion();

	[HslMqttApi]
	OperateResult<string> ReadCommandState();

	[HslMqttApi]
	OperateResult<string> ReadErrorState();

	[HslMqttApi]
	OperateResult<bool> CheckInput(int number);

	[HslMqttApi]
	OperateResult SetOutput(int number, bool value);

	[HslMqttApi]
	OperateResult<int[]> ReadRecord();

	[HslMqttApi]
	OperateResult Lock();

	[HslMqttApi]
	OperateResult UnLock();

	[HslMqttApi]
	OperateResult<string> ReadCustomer(string command);
}
