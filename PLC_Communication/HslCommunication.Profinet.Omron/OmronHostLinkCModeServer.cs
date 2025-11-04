using System;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;

namespace HslCommunication.Profinet.Omron;

public class OmronHostLinkCModeServer : OmronFinsServer
{
	private byte operationMode = 1;

	public byte UnitNumber { get; set; }

	public OmronHostLinkCModeServer()
	{
		LogMsgFormatBinary = false;
		connectionInitialization = false;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13);
	}

	protected override OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		if (receive.Length < 9)
		{
			return new OperateResult<byte[]>("Uknown Data：" + SoftBasic.GetAsciiStringRender(receive));
		}
		return OperateResult.CreateSuccessResult(ReadFromFinsCore(receive));
	}

	protected override byte[] ReadFromFinsCore(byte[] finsCore)
	{
		string text = Encoding.ASCII.GetString(finsCore, 3, 2);
		switch (text)
		{
		default:
		{
			if (text == "RE")
			{
				break;
			}
			switch (text)
			{
			default:
				if (text == "WE")
				{
					break;
				}
				switch (text)
				{
				case "MM":
					return PackCommand(0, finsCore, new byte[1] { 48 });
				case "MS":
					return PackCommand(0, finsCore, new byte[2] { operationMode, 48 });
				case "SC":
				{
					byte b = Convert.ToByte(Encoding.ASCII.GetString(finsCore, 5, 2), 16);
					if (b >= 0 && b <= 2)
					{
						operationMode = b;
					}
					return PackCommand(0, finsCore, null);
				}
				default:
					return PackCommand(22, finsCore, null);
				}
			case "WD":
			case "WR":
			case "WL":
			case "WH":
			case "WJ":
				break;
			}
			SoftBuffer softBuffer = null;
			switch (text)
			{
			case "WD":
				softBuffer = dBuffer;
				break;
			case "WR":
				softBuffer = cioBuffer;
				break;
			case "WL":
				softBuffer = cioBuffer;
				break;
			case "WH":
				softBuffer = hBuffer;
				break;
			case "WJ":
				softBuffer = arBuffer;
				break;
			case "WE":
				softBuffer = emBuffer;
				break;
			default:
				return PackCommand(22, finsCore, null);
			}
			if (text == "WE")
			{
				int num = Convert.ToInt32(Encoding.ASCII.GetString(finsCore, 7, 4));
				byte[] data = Encoding.ASCII.GetString(finsCore, 11, finsCore.Length - 11).ToHexBytes();
				softBuffer.SetBytes(data, num * 2);
				return PackCommand(0, finsCore, null);
			}
			int num2 = Convert.ToInt32(Encoding.ASCII.GetString(finsCore, 5, 4));
			byte[] data2 = Encoding.ASCII.GetString(finsCore, 9, finsCore.Length - 9).ToHexBytes();
			softBuffer.SetBytes(data2, num2 * 2);
			return PackCommand(0, finsCore, null);
		}
		case "RD":
		case "RR":
		case "RL":
		case "RH":
		case "RJ":
			break;
		}
		SoftBuffer softBuffer2 = null;
		switch (text)
		{
		case "RD":
			softBuffer2 = dBuffer;
			break;
		case "RR":
			softBuffer2 = cioBuffer;
			break;
		case "RL":
			softBuffer2 = cioBuffer;
			break;
		case "RH":
			softBuffer2 = hBuffer;
			break;
		case "RJ":
			softBuffer2 = arBuffer;
			break;
		case "RE":
			softBuffer2 = emBuffer;
			break;
		default:
			return PackCommand(22, finsCore, null);
		}
		if (text == "RE")
		{
			int num3 = Convert.ToInt32(Encoding.ASCII.GetString(finsCore, 7, 4));
			ushort num4 = Convert.ToUInt16(Encoding.ASCII.GetString(finsCore, 11, 4));
			byte[] bytes = softBuffer2.GetBytes(num3 * 2, num4 * 2);
			return PackCommand(0, finsCore, bytes);
		}
		int num5 = Convert.ToInt32(Encoding.ASCII.GetString(finsCore, 5, 4));
		ushort num6 = Convert.ToUInt16(Encoding.ASCII.GetString(finsCore, 9, 4));
		byte[] bytes2 = softBuffer2.GetBytes(num5 * 2, num6 * 2);
		return PackCommand(0, finsCore, bytes2);
	}

	protected override byte[] PackCommand(int status, byte[] finsCore, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		data = SoftBasic.BytesToAsciiBytes(data);
		byte[] array = new byte[11 + data.Length];
		Encoding.ASCII.GetBytes("@0000").CopyTo(array, 0);
		Encoding.ASCII.GetBytes(UnitNumber.ToString("X2")).CopyTo(array, 1);
		Array.Copy(finsCore, 3, array, 3, 2);
		Encoding.ASCII.GetBytes(status.ToString("X2")).CopyTo(array, 5);
		if (data.Length != 0)
		{
			data.CopyTo(array, 7);
		}
		int num = array[0];
		for (int i = 1; i < array.Length - 4; i++)
		{
			num ^= array[i];
		}
		SoftBasic.BuildAsciiBytesFrom((byte)num).CopyTo(array, array.Length - 4);
		array[array.Length - 2] = 42;
		array[array.Length - 1] = 13;
		return array;
	}

	protected override bool CheckSerialReceiveDataComplete(byte[] buffer, int receivedLength)
	{
		if (receivedLength > 1)
		{
			return buffer[receivedLength - 1] == 13;
		}
		return false;
	}

	public override string ToString()
	{
		return $"OmronHostLinkCModeServer[{base.Port}]";
	}
}
