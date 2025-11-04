using System.Text;
using System.Threading;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.ModBus;

namespace HslCommunication.Robot.Estun;

public class EstunTcpNet : ModbusTcpNet
{
	private Timer timer;

	public EstunTcpNet()
	{
		timer = new Timer(ThreadTimerTick, null, 3000, 10000);
		base.ByteTransform.DataFormat = DataFormat.CDAB;
	}

	public EstunTcpNet(string ipAddress, int port = 502, byte station = 1)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	private void ThreadTimerTick(object obj)
	{
		OperateResult<ushort> operateResult = ReadUInt16("0");
		if (operateResult.IsSuccess)
		{
		}
	}

	public OperateResult<EstunData> ReadRobotData()
	{
		OperateResult<byte[]> operateResult = Read("0", 100);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<EstunData>(operateResult);
		}
		return OperateResult.CreateSuccessResult(new EstunData(operateResult.Content, base.ByteTransform));
	}

	private OperateResult ExecuteCommand(short command)
	{
		OperateResult<short> operateResult = ReadInt16("99");
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<short> operateResult2 = ReadInt16("51");
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult.Content != 0)
		{
			return new OperateResult("Step1: check 40100 value 0 failed, actual is " + operateResult.Content);
		}
		if (operateResult2.Content != 0)
		{
			return new OperateResult("Step1: check 40052 value 0 failed, actual is " + operateResult2.Content);
		}
		OperateResult operateResult3 = Write("99", (short)17);
		if (!operateResult3.IsSuccess)
		{
			return new OperateResult("Step2: write 40100 0x11 failed, " + operateResult3.Message);
		}
		int num = 0;
		while (true)
		{
			OperateResult<short> operateResult4 = ReadInt16("18");
			if (!operateResult4.IsSuccess)
			{
				return new OperateResult("Step3: read 40019 failed, " + operateResult4.Message);
			}
			if (operateResult4.Content == 2049)
			{
				break;
			}
			num++;
			if (num >= 20)
			{
				return new OperateResult("Step3: wait 40019 0x801 timeout, timeout is 2s");
			}
			HslHelper.ThreadSleep(100);
		}
		OperateResult operateResult5 = Write("51", command);
		if (!operateResult5.IsSuccess)
		{
			return new OperateResult("Step4: write cmd to 40052 failed, " + operateResult5.Message);
		}
		HslHelper.ThreadSleep(100);
		OperateResult<short> operateResult6 = ReadInt16("18");
		if (!operateResult6.IsSuccess)
		{
			return new OperateResult("Step5: read cmd status failed, " + operateResult6.Message);
		}
		OperateResult operateResult7 = Write("99", (short)0);
		if (!operateResult7.IsSuccess)
		{
			return new OperateResult("Step6: clear 40100 failed, " + operateResult7.Message);
		}
		OperateResult operateResult8 = Write("51", (short)0);
		if (!operateResult8.IsSuccess)
		{
			return new OperateResult("Step6: clear 40052 failed, " + operateResult8.Message);
		}
		return OperateResult.CreateSuccessResult();
	}

	public OperateResult RobotStartPrograme()
	{
		return ExecuteCommand(4);
	}

	public OperateResult RobotStopPrograme()
	{
		return ExecuteCommand(8);
	}

	public OperateResult RobotResetError()
	{
		return ExecuteCommand(16);
	}

	public OperateResult RobotLoadProject(string projectName)
	{
		byte[] value = SoftBasic.ArrayExpandToLength(Encoding.ASCII.GetBytes(projectName), 20);
		OperateResult operateResult = Write("53", value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ExecuteCommand(128);
	}

	public OperateResult RobotUnregisterProject()
	{
		return ExecuteCommand(256);
	}

	public OperateResult RobotSetGlobalSpeedValue(short value)
	{
		OperateResult operateResult = Write("52", value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ExecuteCommand(512);
	}

	public OperateResult RobotCommandStatusRestart()
	{
		return ExecuteCommand(1024);
	}

	public override string ToString()
	{
		return $"EstunTcpNet[{IpAddress}:{Port}]";
	}
}
