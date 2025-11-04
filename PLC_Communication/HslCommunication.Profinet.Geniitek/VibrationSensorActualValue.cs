namespace HslCommunication.Profinet.Geniitek;

public struct VibrationSensorActualValue
{
	public float AcceleratedSpeedX { get; set; }

	public float AcceleratedSpeedY { get; set; }

	public float AcceleratedSpeedZ { get; set; }

	public override string ToString()
	{
		return $"ActualValue[{AcceleratedSpeedX},{AcceleratedSpeedY},{AcceleratedSpeedZ}]";
	}
}
