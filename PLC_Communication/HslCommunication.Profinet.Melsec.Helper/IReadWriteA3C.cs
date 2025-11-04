using HslCommunication.Core;

namespace HslCommunication.Profinet.Melsec.Helper;

public interface IReadWriteA3C : IReadWriteDevice, IReadWriteNet
{
	byte Station { get; set; }

	bool SumCheck { get; set; }

	int Format { get; set; }

	bool EnableWriteBitToWordRegister { get; set; }
}
