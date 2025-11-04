using HslCommunication.Core;

namespace HslCommunication.Instrument.Delixi;

public class ElectricalParameters
{
	public float VoltageA { get; set; }

	public float VoltageB { get; set; }

	public float VoltageC { get; set; }

	public float CurrentA { get; set; }

	public float CurrentB { get; set; }

	public float CurrentC { get; set; }

	public float InstantaneousActivePowerA { get; set; }

	public float InstantaneousActivePowerB { get; set; }

	public float InstantaneousActivePowerC { get; set; }

	public float InstantaneousTotalActivePower { get; set; }

	public float InstantaneousReactivePowerA { get; set; }

	public float InstantaneousReactivePowerB { get; set; }

	public float InstantaneousReactivePowerC { get; set; }

	public float InstantaneousTotalReactivePower { get; set; }

	public float InstantaneousApparentPowerA { get; set; }

	public float InstantaneousApparentPowerB { get; set; }

	public float InstantaneousApparentPowerC { get; set; }

	public float InstantaneousTotalApparentPower { get; set; }

	public float PowerFactorA { get; set; }

	public float PowerFactorB { get; set; }

	public float PowerFactorC { get; set; }

	public float TotalPowerFactor { get; set; }

	public float Frequency { get; set; }

	public static ElectricalParameters ParseFromDelixi(byte[] data, IByteTransform byteTransform)
	{
		ElectricalParameters electricalParameters = new ElectricalParameters();
		electricalParameters.VoltageA = (float)byteTransform.TransInt16(data, 0) / 10f;
		electricalParameters.VoltageB = (float)byteTransform.TransInt16(data, 2) / 10f;
		electricalParameters.VoltageC = (float)byteTransform.TransInt16(data, 4) / 10f;
		electricalParameters.CurrentA = (float)byteTransform.TransInt16(data, 6) / 100f;
		electricalParameters.CurrentB = (float)byteTransform.TransInt16(data, 8) / 100f;
		electricalParameters.CurrentC = (float)byteTransform.TransInt16(data, 10) / 100f;
		electricalParameters.InstantaneousActivePowerA = (float)byteTransform.TransInt16(data, 12) / 100f;
		electricalParameters.InstantaneousActivePowerB = (float)byteTransform.TransInt16(data, 14) / 100f;
		electricalParameters.InstantaneousActivePowerC = (float)byteTransform.TransInt16(data, 16) / 100f;
		electricalParameters.InstantaneousTotalActivePower = (float)byteTransform.TransInt16(data, 18) / 100f;
		electricalParameters.InstantaneousReactivePowerA = (float)byteTransform.TransInt16(data, 20) / 100f;
		electricalParameters.InstantaneousReactivePowerB = (float)byteTransform.TransInt16(data, 22) / 100f;
		electricalParameters.InstantaneousReactivePowerC = (float)byteTransform.TransInt16(data, 24) / 100f;
		electricalParameters.InstantaneousTotalReactivePower = (float)byteTransform.TransInt16(data, 26) / 100f;
		electricalParameters.InstantaneousApparentPowerA = (float)byteTransform.TransInt16(data, 28) / 100f;
		electricalParameters.InstantaneousApparentPowerB = (float)byteTransform.TransInt16(data, 30) / 100f;
		electricalParameters.InstantaneousApparentPowerC = (float)byteTransform.TransInt16(data, 32) / 100f;
		electricalParameters.InstantaneousTotalApparentPower = (float)byteTransform.TransInt16(data, 34) / 100f;
		electricalParameters.PowerFactorA = (float)byteTransform.TransInt16(data, 36) / 1000f;
		electricalParameters.PowerFactorB = (float)byteTransform.TransInt16(data, 38) / 1000f;
		electricalParameters.PowerFactorC = (float)byteTransform.TransInt16(data, 40) / 1000f;
		electricalParameters.TotalPowerFactor = (float)byteTransform.TransInt16(data, 42) / 1000f;
		electricalParameters.Frequency = (float)byteTransform.TransInt16(data, 44) / 100f;
		return electricalParameters;
	}
}
