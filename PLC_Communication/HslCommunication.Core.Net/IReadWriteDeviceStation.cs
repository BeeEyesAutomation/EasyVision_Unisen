namespace HslCommunication.Core.Net;

public interface IReadWriteDeviceStation : IReadWriteDevice, IReadWriteNet
{
	byte Station { get; set; }
}
