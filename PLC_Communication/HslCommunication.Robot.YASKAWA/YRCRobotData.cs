using System;
using HslCommunication.BasicFramework;

namespace HslCommunication.Robot.YASKAWA;

public class YRCRobotData
{
	public string SpeedPercent { get; set; }

	public int Frame { get; set; }

	public float X { get; set; }

	public float Y { get; set; }

	public float Z { get; set; }

	public float Rx { get; set; }

	public float Ry { get; set; }

	public float Rz { get; set; }

	public float Re { get; set; }

	public bool[] Status { get; set; }

	public int ToolNumber { get; set; }

	public int Axis7PulseNumber { get; set; }

	public int Axis8PulseNumber { get; set; }

	public int Axis9PulseNumber { get; set; }

	public int Axis10PulseNumber { get; set; }

	public int Axis11PulseNumber { get; set; }

	public int Axis12PulseNumber { get; set; }

	public YRCRobotData()
	{
		SpeedPercent = "100.0%";
		Status = new bool[6];
	}

	public YRCRobotData(YRCType type, string value)
		: this()
	{
		Parse(type, value);
	}

	public string ToWriteString(YRCType type)
	{
		if (type == YRCType.YRC100)
		{
			return $"{SpeedPercent},{Frame},{X},{Y},{Z},{Rx},{Ry},{Rz},{Re},{SoftBasic.BoolArrayToByte(Status)[0]}," + $"{ToolNumber},{Axis7PulseNumber},{Axis8PulseNumber},{Axis9PulseNumber},{Axis10PulseNumber},{Axis11PulseNumber},{Axis12PulseNumber}";
		}
		return $"{SpeedPercent},{Frame},{X},{Y},{Z},{Rx},{Ry},{Rz},{SoftBasic.BoolArrayToByte(Status)[0]}," + $"{ToolNumber},{Axis7PulseNumber},{Axis8PulseNumber},{Axis9PulseNumber},{Axis10PulseNumber},{Axis11PulseNumber},{Axis12PulseNumber}";
	}

	public void Parse(YRCType type, string value)
	{
		string[] array = value.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		int num = 0;
		X = float.Parse(array[num++]);
		Y = float.Parse(array[num++]);
		Z = float.Parse(array[num++]);
		Rx = float.Parse(array[num++]);
		Ry = float.Parse(array[num++]);
		Rz = float.Parse(array[num++]);
		if (type == YRCType.YRC100)
		{
			Re = float.Parse(array[num++]);
		}
		Status = new byte[1] { byte.Parse(array[num++]) }.ToBoolArray().SelectBegin(6);
		ToolNumber = int.Parse(array[num++]);
		if (array.Length > num)
		{
			Axis7PulseNumber = int.Parse(array[num++]);
			Axis8PulseNumber = int.Parse(array[num++]);
			Axis9PulseNumber = int.Parse(array[num++]);
			Axis10PulseNumber = int.Parse(array[num++]);
			Axis11PulseNumber = int.Parse(array[num++]);
			Axis12PulseNumber = int.Parse(array[num++]);
		}
	}

	public override string ToString()
	{
		return $"[{X},{Y},{Z},{Rx},{Ry},{Rz}]";
	}
}
