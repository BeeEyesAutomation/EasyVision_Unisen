using System;
using HslCommunication.ModBus;
using HslCommunication.Reflection;

namespace HslCommunication.Instrument.Delixi;

public class DTSU6606Serial : ModbusRtu
{
	public DTSU6606Serial()
	{
	}

	public DTSU6606Serial(byte station = 1)
		: base(station)
	{
	}

	[HslMqttApi(ApiTopic = "ReadElectricalParameters", Description = "读取电表的电参数类，主要包含电压，电流，频率，有功功率，无功功率，视在功率，功率因数")]
	public OperateResult<ElectricalParameters> ReadElectricalParameters()
	{
		OperateResult<byte[]> operateResult = Read("768", 23);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<ElectricalParameters>();
		}
		try
		{
			return OperateResult.CreateSuccessResult(ElectricalParameters.ParseFromDelixi(operateResult.Content, base.ByteTransform));
		}
		catch (Exception ex)
		{
			return new OperateResult<ElectricalParameters>(ex.Message);
		}
	}

	public override string ToString()
	{
		return "DTSU6606Serial[" + base.PortName + "]";
	}
}
