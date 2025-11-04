using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Instrument.RKC.Helper;

namespace HslCommunication.Instrument.RKC;

public class TemperatureController : DeviceSerialPort
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

	public TemperatureController()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
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
		return await Task.Run(() => ReadDouble(address, length));
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await Task.Run(() => Write(address, values));
	}

	public override string ToString()
	{
		return $"RkcTemperatureController[{base.PortName}:{base.BaudRate}]";
	}
}
