using System.Text;

namespace HslCommunication.WebSocket;

public class WebSocketMessage
{
	public bool HasMask { get; set; }

	public int OpCode { get; set; }

	public byte[] Payload { get; set; }

	public override string ToString()
	{
		return $"OpCode[{OpCode}] HasMask[{HasMask}] Payload: {Encoding.UTF8.GetString(Payload)}";
	}
}
