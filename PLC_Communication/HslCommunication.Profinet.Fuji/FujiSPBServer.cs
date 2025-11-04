using System;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Fuji;

public class FujiSPBServer : DeviceServer
{
	private SoftBuffer xBuffer;

	private SoftBuffer yBuffer;

	private SoftBuffer mBuffer;

	private SoftBuffer dBuffer;

	private SoftBuffer rBuffer;

	private SoftBuffer wBuffer;

	private const int DataPoolLength = 65536;

	private int station = 1;

	public DataFormat DataFormat
	{
		get
		{
			return base.ByteTransform.DataFormat;
		}
		set
		{
			base.ByteTransform.DataFormat = value;
		}
	}

	public bool IsStringReverse
	{
		get
		{
			return base.ByteTransform.IsStringReverseByteWord;
		}
		set
		{
			base.ByteTransform.IsStringReverseByteWord = value;
		}
	}

	public int Station
	{
		get
		{
			return station;
		}
		set
		{
			station = value;
		}
	}

	public FujiSPBServer()
	{
		xBuffer = new SoftBuffer(65536);
		yBuffer = new SoftBuffer(65536);
		mBuffer = new SoftBuffer(65536);
		dBuffer = new SoftBuffer(131072);
		rBuffer = new SoftBuffer(131072);
		wBuffer = new SoftBuffer(131072);
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
		LogMsgFormatBinary = false;
	}

	protected override byte[] SaveToBytes()
	{
		byte[] array = new byte[589824];
		xBuffer.GetBytes().CopyTo(array, 0);
		yBuffer.GetBytes().CopyTo(array, 65536);
		mBuffer.GetBytes().CopyTo(array, 131072);
		dBuffer.GetBytes().CopyTo(array, 196608);
		rBuffer.GetBytes().CopyTo(array, 327680);
		wBuffer.GetBytes().CopyTo(array, 458752);
		return array;
	}

	protected override void LoadFromBytes(byte[] content)
	{
		if (content.Length < 589824)
		{
			throw new Exception("File is not correct");
		}
		xBuffer.SetBytes(content, 0, 65536);
		yBuffer.SetBytes(content, 65536, 65536);
		mBuffer.SetBytes(content, 131072, 65536);
		dBuffer.SetBytes(content, 196608, 131072);
		rBuffer.SetBytes(content, 327680, 131072);
		wBuffer.SetBytes(content, 458752, 131072);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = new OperateResult<byte[]>();
		try
		{
			switch (address[0])
			{
			case 'X':
			case 'x':
				return OperateResult.CreateSuccessResult(xBuffer.GetBytes(Convert.ToInt32(address.Substring(1)) * 2, length * 2));
			case 'Y':
			case 'y':
				return OperateResult.CreateSuccessResult(yBuffer.GetBytes(Convert.ToInt32(address.Substring(1)) * 2, length * 2));
			case 'M':
			case 'm':
				return OperateResult.CreateSuccessResult(mBuffer.GetBytes(Convert.ToInt32(address.Substring(1)) * 2, length * 2));
			case 'D':
			case 'd':
				return OperateResult.CreateSuccessResult(dBuffer.GetBytes(Convert.ToInt32(address.Substring(1)) * 2, length * 2));
			case 'R':
			case 'r':
				return OperateResult.CreateSuccessResult(rBuffer.GetBytes(Convert.ToInt32(address.Substring(1)) * 2, length * 2));
			case 'W':
			case 'w':
				return OperateResult.CreateSuccessResult(wBuffer.GetBytes(Convert.ToInt32(address.Substring(1)) * 2, length * 2));
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
		}
		catch (Exception ex)
		{
			operateResult.Message = ex.Message;
			return operateResult;
		}
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = new OperateResult<byte[]>();
		try
		{
			switch (address[0])
			{
			case 'X':
			case 'x':
				xBuffer.SetBytes(value, Convert.ToInt32(address.Substring(1)) * 2);
				return OperateResult.CreateSuccessResult();
			case 'Y':
			case 'y':
				yBuffer.SetBytes(value, Convert.ToInt32(address.Substring(1)) * 2);
				return OperateResult.CreateSuccessResult();
			case 'M':
			case 'm':
				mBuffer.SetBytes(value, Convert.ToInt32(address.Substring(1)) * 2);
				return OperateResult.CreateSuccessResult();
			case 'D':
			case 'd':
				dBuffer.SetBytes(value, Convert.ToInt32(address.Substring(1)) * 2);
				return OperateResult.CreateSuccessResult();
			case 'R':
			case 'r':
				rBuffer.SetBytes(value, Convert.ToInt32(address.Substring(1)) * 2);
				return OperateResult.CreateSuccessResult();
			case 'W':
			case 'w':
				wBuffer.SetBytes(value, Convert.ToInt32(address.Substring(1)) * 2);
				return OperateResult.CreateSuccessResult();
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
		}
		catch (Exception ex)
		{
			operateResult.Message = ex.Message;
			return operateResult;
		}
	}

	private int GetBitIndex(string address)
	{
		int num = 0;
		if (address.LastIndexOf('.') > 0)
		{
			num = HslHelper.GetBitIndexInformation(ref address);
			return Convert.ToInt32(address.Substring(1)) * 16 + num;
		}
		if (address[0] == 'X' || address[0] == 'x' || address[0] == 'Y' || address[0] == 'y' || address[0] == 'M' || address[0] == 'm')
		{
			return Convert.ToInt32(address.Substring(1));
		}
		return Convert.ToInt32(address.Substring(1)) * 16;
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		try
		{
			int bitIndex = GetBitIndex(address);
			switch (address[0])
			{
			case 'X':
			case 'x':
				return OperateResult.CreateSuccessResult(xBuffer.GetBool(bitIndex, length));
			case 'Y':
			case 'y':
				return OperateResult.CreateSuccessResult(yBuffer.GetBool(bitIndex, length));
			case 'M':
			case 'm':
				return OperateResult.CreateSuccessResult(mBuffer.GetBool(bitIndex, length));
			case 'D':
			case 'd':
				return OperateResult.CreateSuccessResult(dBuffer.GetBool(bitIndex, length));
			case 'R':
			case 'r':
				return OperateResult.CreateSuccessResult(rBuffer.GetBool(bitIndex, length));
			case 'W':
			case 'w':
				return OperateResult.CreateSuccessResult(wBuffer.GetBool(bitIndex, length));
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<bool[]>(ex.Message);
		}
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		try
		{
			int bitIndex = GetBitIndex(address);
			switch (address[0])
			{
			case 'X':
			case 'x':
				xBuffer.SetBool(value, bitIndex);
				return OperateResult.CreateSuccessResult();
			case 'Y':
			case 'y':
				yBuffer.SetBool(value, bitIndex);
				return OperateResult.CreateSuccessResult();
			case 'M':
			case 'm':
				mBuffer.SetBool(value, bitIndex);
				return OperateResult.CreateSuccessResult();
			case 'D':
			case 'd':
				dBuffer.SetBool(value, bitIndex);
				return OperateResult.CreateSuccessResult();
			case 'R':
			case 'r':
				rBuffer.SetBool(value, bitIndex);
				return OperateResult.CreateSuccessResult();
			case 'W':
			case 'w':
				wBuffer.SetBool(value, bitIndex);
				return OperateResult.CreateSuccessResult();
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<bool[]>(ex.Message);
		}
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FujiSPBMessage();
	}

	protected override OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		if (receive.Length < 5)
		{
			return new OperateResult<byte[]>("Uknown message: " + receive.ToHexString(' '));
		}
		if (receive[0] != 58)
		{
			return new OperateResult<byte[]>("Message must start with 0x3A: " + receive.ToHexString(' '));
		}
		if (Encoding.ASCII.GetString(receive, 1, 2) != station.ToString("X2"))
		{
			return new OperateResult<byte[]>($"Station not match , Except: {station:X2} , Actual: {Encoding.ASCII.GetString(receive, 1, 2)}");
		}
		return OperateResult.CreateSuccessResult(ReadFromSPBCore(receive));
	}

	private byte[] CreateResponseBack(byte err, string command, byte[] data, bool addLength = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(':');
		stringBuilder.Append(Station.ToString("X2"));
		stringBuilder.Append("00");
		stringBuilder.Append(command.Substring(9, 4));
		stringBuilder.Append(err.ToString("X2"));
		if (err == 0 && data != null)
		{
			if (addLength)
			{
				stringBuilder.Append(FujiSPBHelper.AnalysisIntegerAddress(data.Length / 2));
			}
			stringBuilder.Append(data.ToHexString());
		}
		stringBuilder[3] = ((stringBuilder.Length - 5) / 2).ToString("X2")[0];
		stringBuilder[4] = ((stringBuilder.Length - 5) / 2).ToString("X2")[1];
		stringBuilder.Append("\r\n");
		return Encoding.ASCII.GetBytes(stringBuilder.ToString());
	}

	private int AnalysisAddress(string address)
	{
		string value = address.Substring(2) + address.Substring(0, 2);
		return Convert.ToInt32(value);
	}

	private byte[] ReadFromSPBCore(byte[] receive)
	{
		if (receive.Length < 15)
		{
			return null;
		}
		if (receive[receive.Length - 2] == 13 && receive[receive.Length - 1] == 10)
		{
			receive = receive.RemoveLast(2);
		}
		string text = Encoding.ASCII.GetString(receive);
		int num = Convert.ToInt32(text.Substring(3, 2), 16);
		if (num != (text.Length - 5) / 2)
		{
			return CreateResponseBack(3, text, null);
		}
		if (text.Substring(9, 4) == "0000")
		{
			return ReadByCommand(text);
		}
		if (text.Substring(9, 4) == "0100")
		{
			return WriteByCommand(text);
		}
		if (text.Substring(9, 4) == "0102")
		{
			return WriteBitByCommand(text);
		}
		return null;
	}

	private byte[] ReadByCommand(string command)
	{
		string text = command.Substring(13, 2);
		int num = AnalysisAddress(command.Substring(15, 4));
		int num2 = AnalysisAddress(command.Substring(19, 4));
		if (num2 > 105)
		{
			CreateResponseBack(3, command, null);
		}
		if (1 == 0)
		{
		}
		byte[] result = text switch
		{
			"0C" => CreateResponseBack(0, command, dBuffer.GetBytes(num * 2, num2 * 2)), 
			"0D" => CreateResponseBack(0, command, rBuffer.GetBytes(num * 2, num2 * 2)), 
			"0E" => CreateResponseBack(0, command, wBuffer.GetBytes(num * 2, num2 * 2)), 
			"01" => CreateResponseBack(0, command, xBuffer.GetBytes(num * 2, num2 * 2)), 
			"00" => CreateResponseBack(0, command, yBuffer.GetBytes(num * 2, num2 * 2)), 
			"02" => CreateResponseBack(0, command, mBuffer.GetBytes(num * 2, num2 * 2)), 
			_ => CreateResponseBack(2, command, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private byte[] WriteByCommand(string command)
	{
		if (!base.EnableWrite)
		{
			return CreateResponseBack(2, command, null);
		}
		string text = command.Substring(13, 2);
		int num = AnalysisAddress(command.Substring(15, 4));
		int num2 = AnalysisAddress(command.Substring(19, 4));
		if (num2 * 4 != command.Length - 23)
		{
			return CreateResponseBack(3, command, null);
		}
		byte[] data = command.Substring(23).ToHexBytes();
		switch (text)
		{
		case "0C":
			dBuffer.SetBytes(data, num * 2);
			return CreateResponseBack(0, command, null);
		case "0D":
			rBuffer.SetBytes(data, num * 2);
			return CreateResponseBack(0, command, null);
		case "0E":
			wBuffer.SetBytes(data, num * 2);
			return CreateResponseBack(0, command, null);
		case "00":
			yBuffer.SetBytes(data, num * 2);
			return CreateResponseBack(0, command, null);
		case "02":
			mBuffer.SetBytes(data, num * 2);
			return CreateResponseBack(0, command, null);
		default:
			return CreateResponseBack(2, command, null);
		}
	}

	private byte[] WriteBitByCommand(string command)
	{
		if (!base.EnableWrite)
		{
			return CreateResponseBack(2, command, null);
		}
		string text = command.Substring(13, 2);
		int num = AnalysisAddress(command.Substring(15, 4));
		int num2 = Convert.ToInt32(command.Substring(19, 2));
		bool value = command.Substring(21, 2) != "00";
		switch (text)
		{
		case "0C":
			dBuffer.SetBool(value, num * 8 + num2);
			return CreateResponseBack(0, command, null);
		case "0D":
			rBuffer.SetBool(value, num * 8 + num2);
			return CreateResponseBack(0, command, null);
		case "0E":
			wBuffer.SetBool(value, num * 8 + num2);
			return CreateResponseBack(0, command, null);
		case "00":
			yBuffer.SetBool(value, num * 8 + num2);
			return CreateResponseBack(0, command, null);
		case "02":
			mBuffer.SetBool(value, num * 8 + num2);
			return CreateResponseBack(0, command, null);
		default:
			return CreateResponseBack(2, command, null);
		}
	}

	protected override bool CheckSerialReceiveDataComplete(byte[] buffer, int receivedLength)
	{
		return ModbusInfo.CheckAsciiReceiveDataComplete(buffer, receivedLength);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			xBuffer.Dispose();
			yBuffer.Dispose();
			mBuffer.Dispose();
			dBuffer.Dispose();
			rBuffer.Dispose();
			wBuffer.Dispose();
		}
		base.Dispose(disposing);
	}

	public override string ToString()
	{
		return $"FujiSPBServer[{base.Port}]";
	}
}
