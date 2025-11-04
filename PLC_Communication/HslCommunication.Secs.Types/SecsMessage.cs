using System;
using System.Text;
using HslCommunication.Secs.Helper;

namespace HslCommunication.Secs.Types;

public class SecsMessage
{
	public ushort DeviceID { get; set; }

	public bool R { get; set; }

	public bool W { get; set; }

	public bool E { get; set; }

	public byte StreamNo { get; set; }

	public byte FunctionNo { get; set; }

	public int BlockNo { get; set; }

	public uint MessageID { get; set; }

	public byte[] Data { get; set; }

	public Encoding StringEncoding { get; set; } = Encoding.Default;

	public SecsMessage()
	{
	}

	public SecsMessage(byte[] message)
		: this(message, 0)
	{
	}

	public SecsMessage(byte[] message, int startIndex)
	{
		DeviceID = BitConverter.ToUInt16(new byte[2]
		{
			message[startIndex + 1],
			(byte)(message[startIndex] & 0x7F)
		}, 0);
		R = (message[startIndex] & 0x80) == 128;
		W = (message[startIndex + 2] & 0x80) == 128;
		E = (message[startIndex + 4] & 0x80) == 128;
		StreamNo = (byte)(message[startIndex + 2] & 0x7F);
		FunctionNo = message[startIndex + 3];
		byte[] buffer = new byte[2]
		{
			(byte)(message[startIndex + 4] & 0x7F),
			message[startIndex + 5]
		};
		BlockNo = Secs2.SecsTransform.TransInt16(buffer, 0);
		MessageID = Secs2.SecsTransform.TransUInt32(message, startIndex + 6);
		Data = message.RemoveBegin(startIndex + 10);
	}

	public SecsValue GetItemValues()
	{
		return Secs2.ExtraToSecsItemValue(Data, StringEncoding);
	}

	public SecsValue GetItemValues(Encoding encoding)
	{
		return Secs2.ExtraToSecsItemValue(Data, encoding);
	}

	public override string ToString()
	{
		SecsValue itemValues = GetItemValues(StringEncoding);
		if (StreamNo == 0 && FunctionNo == 0)
		{
			return string.Format("S{0}F{1}{2} B{3}", StreamNo, FunctionNo, W ? "W" : string.Empty, BlockNo);
		}
		if (itemValues == null)
		{
			return string.Format("S{0}F{1}{2}", StreamNo, FunctionNo, W ? "W" : string.Empty);
		}
		return string.Format("S{0}F{1}{2} {3}{4}", StreamNo, FunctionNo, W ? "W" : string.Empty, Environment.NewLine, itemValues);
	}
}
