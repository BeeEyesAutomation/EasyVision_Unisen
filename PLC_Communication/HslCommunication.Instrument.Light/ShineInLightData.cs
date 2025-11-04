namespace HslCommunication.Instrument.Light;

public class ShineInLightData
{
	public byte Color { get; set; }

	public byte Light { get; set; }

	public byte LightDegree { get; set; }

	public byte WorkMode { get; set; }

	public byte Address { get; set; }

	public byte PulseWidth { get; set; }

	public byte Channel { get; set; }

	public ShineInLightData()
	{
		Color = 4;
		LightDegree = 1;
		PulseWidth = 1;
	}

	public ShineInLightData(byte[] data)
		: this()
	{
		ParseFrom(data);
	}

	public byte[] GetSourceData()
	{
		return new byte[7] { Color, Light, LightDegree, WorkMode, Address, PulseWidth, Channel };
	}

	public void ParseFrom(byte[] data)
	{
		if (data == null || data.Length >= 7)
		{
			Color = data[0];
			Light = data[1];
			LightDegree = data[2];
			WorkMode = data[3];
			Address = data[4];
			PulseWidth = data[5];
			Channel = data[6];
		}
	}

	public override string ToString()
	{
		return $"ShineInLightData[{Color}]";
	}
}
