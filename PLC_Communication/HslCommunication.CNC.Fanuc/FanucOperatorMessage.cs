using System.Text;
using HslCommunication.Core;

namespace HslCommunication.CNC.Fanuc;

public class FanucOperatorMessage
{
	public short Number { get; set; }

	public short Type { get; set; }

	public string Data { get; set; }

	public static FanucOperatorMessage CreateMessage(IByteTransform byteTransform, byte[] buffer, Encoding encoding)
	{
		FanucOperatorMessage fanucOperatorMessage = new FanucOperatorMessage();
		fanucOperatorMessage.Number = byteTransform.TransInt16(buffer, 2);
		fanucOperatorMessage.Type = byteTransform.TransInt16(buffer, 6);
		short num = byteTransform.TransInt16(buffer, 10);
		if (num + 12 <= buffer.Length)
		{
			fanucOperatorMessage.Data = encoding.GetString(buffer, 12, num);
		}
		else
		{
			fanucOperatorMessage.Data = encoding.GetString(buffer, 12, buffer.Length - 12).TrimEnd(default(char));
		}
		return fanucOperatorMessage;
	}
}
