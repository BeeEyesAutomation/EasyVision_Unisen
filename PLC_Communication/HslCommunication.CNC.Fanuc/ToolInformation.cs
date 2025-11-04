using HslCommunication.Core;

namespace HslCommunication.CNC.Fanuc;

public class ToolInformation
{
	public int Life { get; set; }

	public int Use { get; set; }

	public ToolInformation()
	{
	}

	public ToolInformation(byte[] content, IByteTransform byteTransform)
	{
		Life = byteTransform.TransInt32(content, 26);
		Use = byteTransform.TransInt32(content, 34);
	}
}
