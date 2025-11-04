namespace HslCommunication.CNC.Fanuc;

public class CutterInfo
{
	public double LengthSharpOffset { get; set; }

	public double LengthWearOffset { get; set; }

	public double RadiusSharpOffset { get; set; }

	public double RadiusWearOffset { get; set; }

	public override string ToString()
	{
		return $"LengthSharpOffset:{LengthSharpOffset:10} LengthWearOffset:{LengthWearOffset:10} RadiusSharpOffset:{RadiusSharpOffset:10} RadiusWearOffset:{RadiusWearOffset:10}";
	}
}
