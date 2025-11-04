using HslCommunication.Core;

namespace HslCommunication.Profinet.YASKAWA.Helper;

public interface IMemobus : IReadWriteDevice, IReadWriteNet
{
	byte CpuTo { get; set; }

	byte CpuFrom { get; set; }
}
