using HslCommunication.Core;

namespace HslCommunication.Profinet.FATEK.Helper;

public interface IFatekProgram : IReadWriteNet
{
	OperateResult Run(byte station);

	OperateResult Run();

	OperateResult Stop(byte station);

	OperateResult Stop();

	OperateResult<bool[]> ReadStatus(byte station);

	OperateResult<bool[]> ReadStatus();
}
