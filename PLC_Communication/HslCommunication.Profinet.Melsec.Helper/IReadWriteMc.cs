using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Address;

namespace HslCommunication.Profinet.Melsec.Helper;

public interface IReadWriteMc : IReadWriteDevice, IReadWriteNet
{
	byte NetworkNumber { get; set; }

	byte PLCNumber { get; set; }

	ushort TargetIOStation { get; set; }

	byte NetworkStationNumber { get; set; }

	bool EnableWriteBitToWordRegister { get; set; }

	McType McType { get; }

	OperateResult<McAddressData> McAnalysisAddress(string address, ushort length, bool isBit);

	byte[] ExtractActualData(byte[] response, bool isBit);

	OperateResult RemoteRun();

	OperateResult RemoteStop();

	OperateResult RemoteReset();

	OperateResult<string> ReadPlcType();

	OperateResult ErrorStateReset();

	Task<OperateResult> RemoteRunAsync();

	Task<OperateResult> RemoteStopAsync();

	Task<OperateResult> RemoteResetAsync();

	Task<OperateResult<string>> ReadPlcTypeAsync();

	Task<OperateResult> ErrorStateResetAsync();
}
