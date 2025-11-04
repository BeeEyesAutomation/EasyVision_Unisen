using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Toyota;

public class ToyoPuc : DeviceTcpNet
{
	private class WordAddress
	{
		public string Address { get; set; }

		public int BitIndex { get; set; }

		public ushort WordLength { get; set; }
	}

	public ToyoPuc()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
	}

	public ToyoPuc(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new ToyoPucMessage();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		byte[] array = new byte[command.Length + 4];
		array[2] = BitConverter.GetBytes(command.Length)[0];
		array[3] = BitConverter.GetBytes(command.Length)[1];
		command.CopyTo(array, 4);
		return array;
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		if (response == null || response.Length < 4)
		{
			return new OperateResult<byte[]>("Receive data too short: " + response.ToHexString(' '));
		}
		if (response[0] != 128)
		{
			return new OperateResult<byte[]>("FT check failed: " + response.ToHexString(' '));
		}
		if (response[1] != 0)
		{
			return new OperateResult<byte[]>((response.Length == 4) ? GetErrorText(response[1]) : GetErrorText(response[4]));
		}
		if (response.Length > 5)
		{
			return OperateResult.CreateSuccessResult(response.RemoveBegin(5));
		}
		return OperateResult.CreateSuccessResult(new byte[0]);
	}

	private static string GetErrorText(byte code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			17 => StringResources.Language.ToyoPuc11, 
			32 => StringResources.Language.ToyoPuc20, 
			33 => StringResources.Language.ToyoPuc21, 
			35 => StringResources.Language.ToyoPuc23, 
			36 => StringResources.Language.ToyoPuc24, 
			37 => StringResources.Language.ToyoPuc25, 
			52 => StringResources.Language.ToyoPuc34, 
			62 => StringResources.Language.ToyoPuc3E, 
			63 => StringResources.Language.ToyoPuc3F, 
			64 => StringResources.Language.ToyoPuc40, 
			65 => StringResources.Language.ToyoPuc41, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private OperateResult<WordAddress> ExtraWordAddress(string address, ushort length)
	{
		try
		{
			int num = 0;
			int num2 = address.IndexOf('.');
			if (num2 > 0)
			{
				num = Convert.ToInt32(address.Substring(num2 + 1), 16);
				address = address.Substring(0, num2);
			}
			else
			{
				num = Convert.ToInt32(address.Substring(address.Length - 1), 16);
				address = address.Substring(0, address.Length - 1);
				if (address.StartsWith("EK", StringComparison.OrdinalIgnoreCase) || address.StartsWith("EV", StringComparison.OrdinalIgnoreCase) || address.StartsWith("ET", StringComparison.OrdinalIgnoreCase) || address.StartsWith("EC", StringComparison.OrdinalIgnoreCase) || address.StartsWith("EL", StringComparison.OrdinalIgnoreCase) || address.StartsWith("EX", StringComparison.OrdinalIgnoreCase) || address.StartsWith("EY", StringComparison.OrdinalIgnoreCase) || address.StartsWith("EM", StringComparison.OrdinalIgnoreCase))
				{
					if (address.Length == 2)
					{
						address += "0";
					}
				}
				else if (address.Length == 1)
				{
					address += "0";
				}
			}
			int num3 = (num + length + 15) / 16;
			return OperateResult.CreateSuccessResult(new WordAddress
			{
				Address = address,
				BitIndex = num,
				WordLength = (ushort)num3
			});
		}
		catch (Exception ex)
		{
			return new OperateResult<WordAddress>("ExtraWordAddress failed: " + ex.Message);
		}
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (address.IndexOf(".") > 0)
		{
			return HslHelper.ReadBool(this, address, length);
		}
		OperateResult<WordAddress> operateResult = ExtraWordAddress(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = Read(operateResult.Content.Address, operateResult.Content.WordLength);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.ToBoolArray().SelectMiddle(operateResult.Content.BitIndex, length));
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		if (Regex.IsMatch(address, "prg=[0-9+];", RegexOptions.IgnoreCase))
		{
			return HslHelper.WriteBool(this, address, new bool[1] { value }, 16, reverseByWord: false, insertPoint: true);
		}
		if (address.IndexOf('.') > 0)
		{
			return HslHelper.WriteBool(this, address, new bool[1] { value });
		}
		OperateResult<byte[]> operateResult = BuildWriteBoolCommand(address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return HslHelper.WriteBool(this, address, value, 16, reverseByWord: false, insertPoint: true);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = BuildReadWordCommand(address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteWordCommand(address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("ReadRandom", "")]
	public OperateResult<byte[]> ReadRandom(string[] address)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadRandomCommand(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		OperateResult<WordAddress> analysis = ExtraWordAddress(address, length);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(analysis);
		}
		OperateResult<byte[]> read = await ReadAsync(analysis.Content.Address, analysis.Content.WordLength);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.ToBoolArray().SelectMiddle(analysis.Content.BitIndex, length));
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		if (Regex.IsMatch(address, "prg=[0-9+];", RegexOptions.IgnoreCase))
		{
			return await HslHelper.WriteBoolAsync(this, address, new bool[1] { value }, 16, reverseByWord: false, insertPoint: true);
		}
		if (address.IndexOf('.') > 0)
		{
			return await HslHelper.WriteBoolAsync(this, address, new bool[1] { value });
		}
		OperateResult<byte[]> command = BuildWriteBoolCommand(address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await HslHelper.WriteBoolAsync(this, address, value, 16, reverseByWord: false, insertPoint: true);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte[]> command = BuildReadWordCommand(address, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command = BuildWriteWordCommand(address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public async Task<OperateResult<byte[]>> ReadRandomAsync(string[] address)
	{
		OperateResult<List<byte[]>> command = BuildReadRandomCommand(address);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override string ToString()
	{
		return $"ToyoPuc[{IpAddress}:{Port}]";
	}

	private static OperateResult<byte[]> BuildReadBoolCommand(string address)
	{
		OperateResult<ToyoPucAddress> operateResult = ToyoPucAddress.ParseFrom(address, 1, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		ToyoPucAddress content = operateResult.Content;
		if (content.PRG >= 0)
		{
			return new OperateResult<byte[]>();
		}
		return OperateResult.CreateSuccessResult(new byte[3]
		{
			32,
			BitConverter.GetBytes(content.AddressStart)[0],
			BitConverter.GetBytes(content.AddressStart)[1]
		});
	}

	private static OperateResult<byte[]> BuildReadWordCommand(string address, ushort length)
	{
		OperateResult<ToyoPucAddress> operateResult = ToyoPucAddress.ParseFrom(address, length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		ToyoPucAddress content = operateResult.Content;
		if (content.PRG >= 0)
		{
			return OperateResult.CreateSuccessResult(new byte[6]
			{
				148,
				(byte)content.PRG,
				BitConverter.GetBytes(content.AddressStart)[0],
				BitConverter.GetBytes(content.AddressStart)[1],
				BitConverter.GetBytes(length)[0],
				BitConverter.GetBytes(length)[1]
			});
		}
		return OperateResult.CreateSuccessResult(new byte[5]
		{
			28,
			BitConverter.GetBytes(content.AddressStart)[0],
			BitConverter.GetBytes(content.AddressStart)[1],
			BitConverter.GetBytes(length)[0],
			BitConverter.GetBytes(length)[1]
		});
	}

	private static OperateResult<byte[]> BuildWriteWordCommand(string address, byte[] value)
	{
		OperateResult<ToyoPucAddress> operateResult = ToyoPucAddress.ParseFrom(address, 1, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		ToyoPucAddress content = operateResult.Content;
		if (content.PRG >= 0)
		{
			byte[] array = new byte[4 + value.Length];
			array[0] = 149;
			array[1] = (byte)content.PRG;
			array[2] = BitConverter.GetBytes(content.AddressStart)[0];
			array[3] = BitConverter.GetBytes(content.AddressStart)[1];
			value.CopyTo(array, 4);
			return OperateResult.CreateSuccessResult(array);
		}
		byte[] array2 = new byte[3 + value.Length];
		array2[0] = 29;
		array2[1] = BitConverter.GetBytes(content.AddressStart)[0];
		array2[2] = BitConverter.GetBytes(content.AddressStart)[1];
		value.CopyTo(array2, 3);
		return OperateResult.CreateSuccessResult(array2);
	}

	private static OperateResult<byte[]> BuildWriteBoolCommand(string address, bool value)
	{
		OperateResult<ToyoPucAddress> operateResult = ToyoPucAddress.ParseFrom(address, 1, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (!operateResult.Content.WriteBitEnabled)
		{
			return new OperateResult<byte[]>("Address: " + address + "  is not support write bit, because it is word address");
		}
		ToyoPucAddress content = operateResult.Content;
		if (content.PRG >= 0)
		{
			return new OperateResult<byte[]>("Not supported prg write bool");
		}
		byte[] array = new byte[4]
		{
			33,
			BitConverter.GetBytes(content.AddressStart)[0],
			BitConverter.GetBytes(content.AddressStart)[1],
			0
		};
		if (value)
		{
			array[3] = 1;
		}
		return OperateResult.CreateSuccessResult(array);
	}

	private static OperateResult<List<byte[]>> BuildReadRandomCommand(string[] address)
	{
		List<string[]> list = SoftBasic.ArraySplitByLength(address, 80);
		List<byte[]> list2 = new List<byte[]>();
		for (int i = 0; i < list.Count; i++)
		{
			string[] array = list[i];
			byte[] array2 = new byte[1 + array.Length * 2];
			array2[0] = 34;
			for (int j = 0; j < array.Length; j++)
			{
				OperateResult<ToyoPucAddress> operateResult = ToyoPucAddress.ParseFrom(array[j], 1, isBit: false);
				if (!operateResult.IsSuccess)
				{
					return OperateResult.CreateFailedResult<List<byte[]>>(operateResult);
				}
				ToyoPucAddress content = operateResult.Content;
				array2[j * 2 + 1] = BitConverter.GetBytes(content.AddressStart)[0];
				array2[j * 2 + 2] = BitConverter.GetBytes(content.AddressStart)[1];
			}
			list2.Add(array2);
		}
		return OperateResult.CreateSuccessResult(list2);
	}
}
