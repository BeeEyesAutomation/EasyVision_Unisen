using System;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Robot.FANUC;

public class FanucPose
{
	public float[] Xyzwpr { get; set; }

	public string[] Config { get; set; }

	public float[] Joint { get; set; }

	public short UF { get; set; }

	public short UT { get; set; }

	public short ValidC { get; set; }

	public short ValidJ { get; set; }

	public void LoadByContent(IByteTransform byteTransform, byte[] content, int index)
	{
		Xyzwpr = new float[9];
		for (int i = 0; i < Xyzwpr.Length; i++)
		{
			Xyzwpr[i] = BitConverter.ToSingle(content, index + 4 * i);
		}
		Config = TransConfigStringArray(byteTransform.TransInt16(content, index + 36, 7));
		Joint = new float[9];
		for (int j = 0; j < Joint.Length; j++)
		{
			Joint[j] = BitConverter.ToSingle(content, index + 52 + 4 * j);
		}
		ValidC = BitConverter.ToInt16(content, index + 50);
		ValidJ = BitConverter.ToInt16(content, index + 88);
		UF = BitConverter.ToInt16(content, index + 90);
		UT = BitConverter.ToInt16(content, index + 92);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder($"FanucPose UF={UF} UT={UT}");
		if (ValidC != 0)
		{
			stringBuilder.Append("\r\nXyzwpr=" + SoftBasic.ArrayFormat(Xyzwpr) + "\r\nConfig=" + SoftBasic.ArrayFormat(Config));
		}
		if (ValidJ != 0)
		{
			stringBuilder.Append("\r\nJOINT=" + SoftBasic.ArrayFormat(Joint));
		}
		return stringBuilder.ToString();
	}

	public static FanucPose ParseFrom(IByteTransform byteTransform, byte[] content, int index)
	{
		FanucPose fanucPose = new FanucPose();
		fanucPose.LoadByContent(byteTransform, content, index);
		return fanucPose;
	}

	public static string[] TransConfigStringArray(short[] value)
	{
		return new string[7]
		{
			(value[0] != 0) ? "F" : "N",
			(value[1] != 0) ? "L" : "R",
			(value[2] != 0) ? "U" : "D",
			(value[3] != 0) ? "T" : "B",
			value[4].ToString(),
			value[5].ToString(),
			value[6].ToString()
		};
	}
}
