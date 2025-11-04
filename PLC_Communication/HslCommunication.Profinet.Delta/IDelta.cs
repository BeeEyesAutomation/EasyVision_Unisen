using HslCommunication.Core;

namespace HslCommunication.Profinet.Delta;

public interface IDelta : IReadWriteDevice, IReadWriteNet
{
	DeltaSeries Series { get; set; }
}
