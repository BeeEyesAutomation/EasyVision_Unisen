using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Robot.Estun;

public class EstunData
{
	public bool ManualMode { get; set; }

	public bool AutoMode { get; set; }

	public bool RemoteMode { get; set; }

	public bool EnableStatus { get; set; }

	public bool RunStatus { get; set; }

	public bool ErrorStatus { get; set; }

	public bool ProgramRunStatus { get; set; }

	public bool RobotMoving { get; set; }

	public string ProjectName { get; set; }

	public bool[] DO { get; set; }

	public ushort RobotCommandStatus { get; set; }

	public float[] AO { get; set; }

	public short GlobalSpeedValue { get; set; }

	public bool[] DI { get; set; }

	public float[] AI { get; set; }

	public short ReadWriteFlag { get; set; }

	public EstunData()
	{
	}

	public EstunData(byte[] source, IByteTransform byteTransform)
	{
		LoadBySourceData(source, byteTransform);
	}

	public void LoadBySourceData(byte[] source, IByteTransform byteTransform)
	{
		ManualMode = source[7].GetBoolByIndex(0);
		AutoMode = source[7].GetBoolByIndex(1);
		RemoteMode = source[7].GetBoolByIndex(2);
		EnableStatus = source[7].GetBoolByIndex(3);
		RunStatus = source[7].GetBoolByIndex(4);
		ErrorStatus = source[7].GetBoolByIndex(5);
		ProgramRunStatus = source[7].GetBoolByIndex(6);
		RobotMoving = source[7].GetBoolByIndex(7);
		GlobalSpeedValue = byteTransform.TransInt16(source, 2);
		ProjectName = Encoding.ASCII.GetString(SoftBasic.BytesReverseByWord(source.SelectMiddle(8, 20))).TrimEnd(default(char));
		DO = SoftBasic.BytesReverseByWord(source.SelectMiddle(28, 8)).ToBoolArray();
		RobotCommandStatus = byteTransform.TransUInt16(source, 36);
		AO = byteTransform.TransSingle(source, 38, 16);
		DI = SoftBasic.BytesReverseByWord(source.SelectMiddle(126, 8)).ToBoolArray();
		AI = byteTransform.TransSingle(source, 134, 16);
		ReadWriteFlag = byteTransform.TransInt16(source, 198);
	}
}
