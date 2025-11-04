using System.Threading.Tasks;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Melsec.Helper;

public interface IMelsecFxSerial : IReadWriteNet
{
	OperateResult ActivePlc();

	Task<OperateResult> ActivePlcAsync();
}
