using System.Text;
using HslCommunication.BasicFramework;
using Newtonsoft.Json;

namespace HslCommunication.Profinet.Toledo;

public class ToledoStandardData
{
	public bool Suttle { get; set; }

	public bool Symbol { get; set; }

	public bool BeyondScope { get; set; }

	public bool DynamicState { get; set; }

	public string Unit { get; set; }

	public bool IsPrint { get; set; }

	public bool IsTenExtend { get; set; }

	public float Weight { get; set; }

	public float Tare { get; set; }

	public int TareType { get; set; }

	public bool DataValid { get; set; } = true;

	public bool IsExpandOutput { get; set; }

	[JsonIgnore]
	public byte[] SourceData { get; set; }

	public ToledoStandardData()
	{
	}

	public ToledoStandardData(byte[] buffer)
	{
		if (buffer[0] == 2)
		{
			ParseFromStandardOutput(buffer);
		}
		else if (buffer[0] == 1)
		{
			ParseFromExpandOutput(buffer);
		}
	}

	public override string ToString()
	{
		return $"ToledoStandardData[{Weight}]";
	}

	private void ParseFromStandardOutput(byte[] buffer)
	{
		Weight = float.Parse(Encoding.ASCII.GetString(buffer, 4, 6));
		Tare = float.Parse(Encoding.ASCII.GetString(buffer, 10, 6));
		switch (buffer[1] & 7)
		{
		case 0:
			Weight *= 100f;
			Tare *= 100f;
			break;
		case 1:
			Weight *= 10f;
			Tare *= 10f;
			break;
		case 3:
			Weight /= 10f;
			Tare /= 10f;
			break;
		case 4:
			Weight /= 100f;
			Tare /= 100f;
			break;
		case 5:
			Weight /= 1000f;
			Tare /= 1000f;
			break;
		case 6:
			Weight /= 10000f;
			Tare /= 10000f;
			break;
		case 7:
			Weight /= 100000f;
			Tare /= 100000f;
			break;
		}
		Suttle = SoftBasic.BoolOnByteIndex(buffer[2], 0);
		Symbol = SoftBasic.BoolOnByteIndex(buffer[2], 1);
		BeyondScope = SoftBasic.BoolOnByteIndex(buffer[2], 2);
		DynamicState = SoftBasic.BoolOnByteIndex(buffer[2], 3);
		switch (buffer[3] & 7)
		{
		case 0:
			Unit = (SoftBasic.BoolOnByteIndex(buffer[2], 4) ? "kg" : "lb");
			break;
		case 1:
			Unit = "g";
			break;
		case 2:
			Unit = "t";
			break;
		case 3:
			Unit = "oz";
			break;
		case 4:
			Unit = "ozt";
			break;
		case 5:
			Unit = "dwt";
			break;
		case 6:
			Unit = "ton";
			break;
		case 7:
			Unit = "newton";
			break;
		}
		IsPrint = SoftBasic.BoolOnByteIndex(buffer[3], 3);
		IsTenExtend = SoftBasic.BoolOnByteIndex(buffer[3], 4);
		SourceData = buffer;
	}

	private void ParseFromExpandOutput(byte[] buffer)
	{
		IsExpandOutput = true;
		string text = Encoding.ASCII.GetString(buffer, 6, 9).Replace(" ", "");
		if (!string.IsNullOrEmpty(text))
		{
			Weight = float.Parse(text);
		}
		string text2 = Encoding.ASCII.GetString(buffer, 15, 8).Replace(" ", "");
		if (!string.IsNullOrEmpty(text2))
		{
			Tare = float.Parse(text2);
		}
		switch (buffer[2] & 0xF)
		{
		case 0:
			Unit = "None";
			break;
		case 1:
			Unit = "lb";
			break;
		case 2:
			Unit = "kg";
			break;
		case 3:
			Unit = "g";
			break;
		case 4:
			Unit = "t";
			break;
		case 5:
			Unit = "ton";
			break;
		case 8:
			Unit = "oz";
			break;
		case 9:
			Unit = "newton";
			break;
		}
		DynamicState = SoftBasic.BoolOnByteIndex(buffer[2], 6);
		Suttle = SoftBasic.BoolOnByteIndex(buffer[3], 0);
		TareType = (buffer[3] & 6) >> 1;
		BeyondScope = SoftBasic.BoolOnByteIndex(buffer[4], 1) || SoftBasic.BoolOnByteIndex(buffer[4], 2);
		IsPrint = SoftBasic.BoolOnByteIndex(buffer[4], 4);
		DataValid = SoftBasic.BoolOnByteIndex(buffer[4], 0);
		SourceData = buffer;
	}
}
