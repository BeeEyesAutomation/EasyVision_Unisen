using System.IO;

namespace HslCommunication.Core.Net;

internal class FileStateObject : StateOneBase
{
	public Stream Stream { get; set; }
}
