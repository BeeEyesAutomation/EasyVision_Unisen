using System.Collections.Generic;
using System.Threading.Tasks;

namespace HslCommunication.Core;

public interface IReadWriteDevice : IReadWriteNet
{
	IByteTransform ByteTransform { get; set; }

	OperateResult<byte[]> ReadFromCoreServer(byte[] send);

	OperateResult<byte[]> ReadFromCoreServer(IEnumerable<byte[]> send);

	Task<OperateResult<byte[]>> ReadFromCoreServerAsync(byte[] send);

	Task<OperateResult<byte[]>> ReadFromCoreServerAsync(IEnumerable<byte[]> send);
}
