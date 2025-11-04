using HslCommunication.Core;

namespace HslCommunication.Profinet.Siemens.Helper;

public interface ISiemensPPI : IReadWriteNet
{
	OperateResult Start(string parameter = "");

	OperateResult Stop(string parameter = "");

	OperateResult<string> ReadPlcType(string parameter = "");
}
