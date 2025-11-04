namespace HslCommunication.Profinet.OpenProtocol;

public class ParameterSetData
{
	public int ParameterSetID { get; set; }

	public string ParameterSetName { get; set; }

	public string RotationDirection { get; set; }

	public int BatchSize { get; set; }

	public double TorqueMin { get; set; }

	public double TorqueMax { get; set; }

	public double TorqueFinalTarget { get; set; }

	public int AngleMin { get; set; }

	public int AngleMax { get; set; }

	public int AngleFinalTarget { get; set; }
}
