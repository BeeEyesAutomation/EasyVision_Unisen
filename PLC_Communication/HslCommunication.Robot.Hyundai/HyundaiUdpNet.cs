using System;
using System.Net;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;

namespace HslCommunication.Robot.Hyundai;

public class HyundaiUdpNet : CommunicationServer
{
	public delegate void OnHyundaiMessageReceiveDelegate(HyundaiData data);

	private HyundaiData hyundaiDataHistory;

	private SoftIncrementCount incrementCount;

	public event OnHyundaiMessageReceiveDelegate OnHyundaiMessageReceive;

	public HyundaiUdpNet()
	{
		incrementCount = new SoftIncrementCount(2147483647L, 0L);
		base.OnPipeMessageReceived += HyundaiUdpNet_OnPipeMessageReceived;
	}

	private void HyundaiUdpNet_OnPipeMessageReceived(PipeSession session, byte[] buffer)
	{
		int num = buffer.Length;
		if (num != 64)
		{
			base.LogNet?.WriteError(ToString(), $"<{session.Communication}> Receive Error Length[{num}]: {buffer.ToHexString()}");
			return;
		}
		base.LogNet?.WriteDebug(ToString(), $"<{session.Communication}> Receive: {buffer.ToHexString()}");
		HyundaiData hyundaiData = new HyundaiData(buffer);
		if (hyundaiData.Command == 'S')
		{
			HyundaiData hyundaiData2 = new HyundaiData(buffer);
			hyundaiData2.Command = 'S';
			hyundaiData2.Count = 0;
			hyundaiData2.State = 1;
			Write(hyundaiData2);
			base.LogNet?.WriteDebug(ToString(), $"<{session.Communication}> Send: {hyundaiData.ToBytes().ToHexString()}");
			base.LogNet?.WriteDebug(ToString(), $"<{session.Communication}> Online tracking is started by Hi5 controller.");
		}
		else if (hyundaiData.Command == 'P')
		{
			hyundaiDataHistory = hyundaiData;
			this.OnHyundaiMessageReceive?.Invoke(hyundaiData);
		}
		else if (hyundaiData.Command == 'F')
		{
			base.LogNet?.WriteDebug(ToString(), $"<{session.Communication}> Online tracking is finished by Hi5 controller.");
		}
	}

	[HslMqttApi]
	public OperateResult WriteIncrementPos(double x, double y, double z, double rx, double ry, double rz)
	{
		return WriteIncrementPos(new double[6] { x, y, z, rx, ry, rz });
	}

	[HslMqttApi]
	public OperateResult WriteIncrementPos(double[] pos)
	{
		HyundaiData hyundaiData = new HyundaiData
		{
			Command = 'P',
			State = 2,
			Count = (int)incrementCount.GetCurrentValue()
		};
		for (int i = 0; i < hyundaiData.Data.Length; i++)
		{
			hyundaiData.Data[i] = pos[i];
		}
		return Write(hyundaiData);
	}

	[HslMqttApi]
	public OperateResult Write(HyundaiData data)
	{
		if (!base.IsStarted)
		{
			return new OperateResult("Please Start Server First!");
		}
		try
		{
			PipeSession[] pipeSessions = GetPipeSessions();
			if (pipeSessions.Length == 0)
			{
				return new OperateResult("Please Wait Robot Connect!");
			}
			for (int i = 0; i < pipeSessions.Length; i++)
			{
				if (pipeSessions[i].Communication is PipeUdpNet pipeUdpNet && pipeUdpNet.IpAddress == IPAddress.Any.ToString())
				{
					return new OperateResult("Please Wait Robot Connect!");
				}
				OperateResult operateResult = pipeSessions[i].Communication.Send(data.ToBytes());
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
				base.LogNet?.WriteDebug(ToString(), $"<{pipeSessions[i].Communication}> Send: {data.ToBytes().ToHexString(' ')}");
			}
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
	}

	[HslMqttApi]
	public OperateResult MoveX(double value)
	{
		return WriteIncrementPos(value, 0.0, 0.0, 0.0, 0.0, 0.0);
	}

	[HslMqttApi]
	public OperateResult MoveY(double value)
	{
		return WriteIncrementPos(0.0, value, 0.0, 0.0, 0.0, 0.0);
	}

	[HslMqttApi]
	public OperateResult MoveZ(double value)
	{
		return WriteIncrementPos(0.0, 0.0, value, 0.0, 0.0, 0.0);
	}

	[HslMqttApi]
	public OperateResult RotateX(double value)
	{
		return WriteIncrementPos(0.0, 0.0, 0.0, value, 0.0, 0.0);
	}

	[HslMqttApi]
	public OperateResult RotateY(double value)
	{
		return WriteIncrementPos(0.0, 0.0, 0.0, 0.0, value, 0.0);
	}

	[HslMqttApi]
	public OperateResult RotateZ(double value)
	{
		return WriteIncrementPos(0.0, 0.0, 0.0, 0.0, 0.0, value);
	}

	public override string ToString()
	{
		return $"HyundaiUdpNet[{base.Port}]";
	}
}
