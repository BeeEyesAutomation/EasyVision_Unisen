using System;
using System.Text;
using HslCommunication.Core;

namespace HslCommunication.Robot.FANUC;

public class FanucTask
{
	public string ProgramName { get; set; }

	public short LineNumber { get; set; }

	public short State { get; set; }

	public string ParentProgramName { get; set; }

	public void LoadByContent(IByteTransform byteTransform, byte[] content, int index, Encoding encoding)
	{
		ProgramName = encoding.GetString(content, index, 16).Trim(default(char));
		LineNumber = BitConverter.ToInt16(content, index + 16);
		State = BitConverter.ToInt16(content, index + 18);
		ParentProgramName = encoding.GetString(content, index + 20, 16).Trim(default(char));
	}

	public override string ToString()
	{
		return $"ProgramName[{ProgramName}] LineNumber[{LineNumber}] State[{State}] ParentProgramName[{ParentProgramName}]";
	}

	public static FanucTask ParseFrom(IByteTransform byteTransform, byte[] content, int index, Encoding encoding)
	{
		FanucTask fanucTask = new FanucTask();
		fanucTask.LoadByContent(byteTransform, content, index, encoding);
		return fanucTask;
	}
}
