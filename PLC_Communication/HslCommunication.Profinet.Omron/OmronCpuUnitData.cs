using System.Text;

namespace HslCommunication.Profinet.Omron;

public class OmronCpuUnitData
{
	public string Model { get; set; }

	public string Version { get; set; }

	public int LargestEMNumber { get; set; }

	public int ProgramAreaSize { get; set; }

	public int IOMSize { get; set; }

	public int DMSize { get; set; }

	public int EMSize { get; set; }

	public int TCSize { get; set; }

	public OmronCpuUnitData()
	{
	}

	public OmronCpuUnitData(byte[] data)
	{
		Model = Encoding.ASCII.GetString(data, 0, 20).Trim(' ');
		Version = Encoding.ASCII.GetString(data, 20, 10).Trim(' ', '\0');
		LargestEMNumber = data[41];
		ProgramAreaSize = data[80] * 256 + data[81];
		IOMSize = data[82] * 1024;
		DMSize = data[83] * 256 + data[84];
		TCSize = data[85] * 1024;
		EMSize = data[86];
	}
}
