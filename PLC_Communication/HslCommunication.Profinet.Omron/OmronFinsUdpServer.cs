using System;
using HslCommunication.BasicFramework;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;

namespace HslCommunication.Profinet.Omron;

public class OmronFinsUdpServer : OmronFinsServer
{
	public OmronFinsUdpServer()
	{
		connectionInitialization = false;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return null;
	}

	protected override byte[] PackCommand(int status, byte[] finsCore, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		byte[] array = new byte[14 + data.Length];
		SoftBasic.HexStringToBytes("C0 00 02 00 00 00 00 00 00 00 00 00 00 00").CopyTo(array, 0);
		if (data.Length != 0)
		{
			data.CopyTo(array, 14);
		}
		array[10] = finsCore[0];
		array[11] = finsCore[1];
		array[12] = BitConverter.GetBytes(status)[1];
		array[13] = BitConverter.GetBytes(status)[0];
		return array;
	}

	protected override OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		byte[] array = ReadFromFinsCore(receive.RemoveBegin(10));
		if (array != null)
		{
			array[4] = receive[7];
			array[7] = receive[4];
			array[9] = receive[9];
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public override string ToString()
	{
		return $"OmronFinsUdpServer[{base.Port}]";
	}
}
