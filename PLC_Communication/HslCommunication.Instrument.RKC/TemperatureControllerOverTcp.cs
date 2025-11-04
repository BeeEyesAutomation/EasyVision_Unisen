using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Instrument.RKC.Helper;

namespace HslCommunication.Instrument.RKC;

public class TemperatureControllerOverTcp : DeviceTcpNet
{
	private byte station = 1;

	public byte Station
	{
		get
		{
			return station;
		}
		set
		{
			station = value;
		}
	}

	public TemperatureControllerOverTcp()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		base.SleepTime = 20;
	}

	public TemperatureControllerOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new RkcTemperatureMessage();
	}

	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		OperateResult<double> operateResult = TemperatureControllerHelper.ReadDouble(this, station, address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(new double[1] { operateResult.Content });
	}

	public override OperateResult Write(string address, double[] values)
	{
		if (values == null || values.Length == 0)
		{
			return OperateResult.CreateSuccessResult();
		}
		return TemperatureControllerHelper.Write(this, station, address, values[0]);
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		OperateResult<double> read = await TemperatureControllerHelper.ReadDoubleAsync(this, station, address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double[]>(read);
		}
		return OperateResult.CreateSuccessResult(new double[1] { read.Content });
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		if (values == null || values.Length == 0)
		{
			return OperateResult.CreateSuccessResult();
		}
		return await TemperatureControllerHelper.WriteAsync(this, station, address, values[0]);
	}

	public override string ToString()
	{
		return $"RkcTemperatureControllerOverTcp[{IpAddress}:{Port}]";
	}
}
