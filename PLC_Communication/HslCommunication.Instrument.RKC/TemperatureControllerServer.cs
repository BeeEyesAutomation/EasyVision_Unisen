using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;

namespace HslCommunication.Instrument.RKC;

public class TemperatureControllerServer : DeviceServer
{
	private List<string> tagList;

	private string readTag = string.Empty;

	private byte station = 0;

	private Dictionary<string, double> tagValues;

	public byte Station
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

	public TemperatureControllerServer()
	{
		string[] array = new string[26]
		{
			"M1", "M2", "M3", "AA", "AB", "B1", "ER", "SR", "G1", "S1",
			"A1", "A2", "A3", "A4", "A5", "P1", "I1", "D1", "W1", "P2",
			"V1", "T0", "T1", "G2", "PB", "LK"
		};
		tagValues = new Dictionary<string, double>();
		tagList = new List<string>(array);
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string key in array3)
		{
			tagValues.Add(key, 0.0);
		}
		LogMsgFormatBinary = false;
	}

	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		string[] array = address.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		double[] array2 = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (tagValues.ContainsKey(array[i]))
			{
				array2[i] = tagValues[array[i]];
				continue;
			}
			return new OperateResult<double[]>("Tag[" + array[i] + "] is not exist");
		}
		return OperateResult.CreateSuccessResult(array2);
	}

	public override OperateResult Write(string address, double[] values)
	{
		string[] array = address.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			if (tagValues.ContainsKey(array[i]))
			{
				tagValues[array[i]] = values[i];
				continue;
			}
			return new OperateResult<double[]>("Tag[" + array[i] + "] is not exist");
		}
		return OperateResult.CreateSuccessResult();
	}

	private OperateResult<byte[]> CreateResponseByAddress(string add)
	{
		if (!tagValues.ContainsKey(add))
		{
			return new OperateResult<byte[]>("Read tag [" + add + "] is not exist");
		}
		readTag = add;
		List<byte> list = new List<byte>(20);
		list.Add(2);
		list.AddRange(Encoding.ASCII.GetBytes(add));
		list.AddRange(Encoding.ASCII.GetBytes(tagValues[add].ToString().PadLeft(6, '0').Substring(0, 6)));
		list.Add(3);
		int num = list[3];
		for (int i = 4; i < list.Count; i++)
		{
			num ^= list[i];
		}
		list.Add((byte)num);
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	protected override OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		if (receive.Length == 1)
		{
			if (!string.IsNullOrEmpty(readTag))
			{
				if (receive[0] == 6)
				{
					int num = tagList.IndexOf(readTag);
					num++;
					if (num >= tagList.Count)
					{
						num = 0;
					}
					return CreateResponseByAddress(tagList[num]);
				}
				if (receive[0] == 21)
				{
					return CreateResponseByAddress(readTag);
				}
			}
			return new OperateResult<byte[]>("not legal: " + receive.ToHexString(' '));
		}
		byte b = byte.Parse(Encoding.ASCII.GetString(receive, 1, 2));
		if (b != Station)
		{
			return new OperateResult<byte[]>($"Station not match, need [{Station}] but [{b}]");
		}
		if (receive[3] == 2)
		{
			if (!base.EnableWrite)
			{
				return new OperateResult<byte[]>("Not allow client write");
			}
			string text = Encoding.ASCII.GetString(receive, 4, 2);
			double value = double.Parse(Encoding.ASCII.GetString(receive, 6, receive.Length - 8));
			if (!tagValues.ContainsKey(text))
			{
				return new OperateResult<byte[]>("Write tag [" + text + "] is not exist");
			}
			tagValues[text] = value;
			return OperateResult.CreateSuccessResult(new byte[1] { 6 });
		}
		string add = Encoding.ASCII.GetString(receive, 3, 2);
		return CreateResponseByAddress(add);
	}

	protected override bool CheckSerialReceiveDataComplete(byte[] buffer, int receivedLength)
	{
		MemoryStream ms = new MemoryStream();
		ms.Write(buffer.SelectBegin(receivedLength));
		RkcTemperatureMessage rkcTemperatureMessage = new RkcTemperatureMessage();
		return rkcTemperatureMessage.CheckReceiveDataComplete(null, ms);
	}

	public override string ToString()
	{
		return $"TemperatureControllerServer[{base.Port}]";
	}
}
