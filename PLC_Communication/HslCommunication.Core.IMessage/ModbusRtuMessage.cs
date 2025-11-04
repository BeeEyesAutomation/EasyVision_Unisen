using System.IO;
using HslCommunication.ModBus;

namespace HslCommunication.Core.IMessage;

public class ModbusRtuMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public bool StationCheckMatch { get; set; } = true;

	public ModbusRtuMessage(bool stationCheck)
	{
		StationCheckMatch = stationCheck;
	}

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return ModbusInfo.CheckRtuReceiveDataComplete(send, ms.ToArray());
	}

	public override int CheckMessageMatch(byte[] send, byte[] receive)
	{
		if (!StationCheckMatch)
		{
			return 1;
		}
		return ModbusInfo.CheckRtuMessageMatch(send, receive);
	}
}
