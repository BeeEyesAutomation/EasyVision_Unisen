using HslCommunication.Core;

namespace HslCommunication.Profinet.Melsec.Helper;

public interface IReadWriteFxLinks : IReadWriteDevice, IReadWriteNet
{
	byte Station { get; set; }

	byte WaittingTime { get; set; }

	bool SumCheck { get; set; }

	int Format { get; set; }

	OperateResult StartPLC(string parameter = "");

	OperateResult StopPLC(string parameter = "");

	OperateResult<string> ReadPlcType(string parameter = "");
}
