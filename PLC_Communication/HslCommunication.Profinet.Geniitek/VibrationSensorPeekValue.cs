namespace HslCommunication.Profinet.Geniitek;

public class VibrationSensorPeekValue
{
	public float AcceleratedSpeedX { get; set; }

	public float AcceleratedSpeedY { get; set; }

	public float AcceleratedSpeedZ { get; set; }

	public float SpeedX { get; set; }

	public float SpeedY { get; set; }

	public float SpeedZ { get; set; }

	public int OffsetX { get; set; }

	public int OffsetY { get; set; }

	public int OffsetZ { get; set; }

	public float Temperature { get; set; }

	public float Voltage { get; set; }

	public int SendingInterval { get; set; }
}
