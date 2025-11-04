using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;

namespace HslCommunication.Secs;

public class SecsGemSerial : DeviceSerialPort
{
	private SoftIncrementCount incrementCount;

	public SecsGemSerial()
	{
		incrementCount = new SoftIncrementCount(4294967295L, 0L);
		base.ByteTransform = new ReverseBytesTransform();
		base.WordLength = 2;
	}

	public OperateResult<byte[]> ExecuteCommand(byte[] command)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(new byte[1] { 5 });
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content[0] != 4)
		{
			return new OperateResult<byte[]>($"Send Enq to device, but receive [{operateResult.Content[0]}], need receive [EOT]");
		}
		OperateResult<byte[]> operateResult2;
		do
		{
			operateResult2 = ReadFromCoreServer(command);
		}
		while (operateResult2.IsSuccess);
		return operateResult2;
	}
}
