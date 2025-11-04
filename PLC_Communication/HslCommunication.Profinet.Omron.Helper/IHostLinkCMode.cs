using HslCommunication.Core;

namespace HslCommunication.Profinet.Omron.Helper;

public interface IHostLinkCMode : IReadWriteNet
{
	OperateResult<string> ReadPlcType();

	OperateResult<string> ReadPlcType(byte unitNumber);

	OperateResult<int> ReadPlcMode();

	OperateResult<int> ReadPlcMode(byte unitNumber);

	OperateResult ChangePlcMode(byte mode);

	OperateResult ChangePlcMode(byte unitNumber, byte mode);
}
