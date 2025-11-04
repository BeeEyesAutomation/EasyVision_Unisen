namespace HslCommunication.Core;

public class CuttingAddress
{
	public string DataType { get; set; }

	public int Address { get; set; }

	public int FromBase { get; set; } = 10;

	public CuttingAddress()
	{
	}

	public CuttingAddress(string type, int address, int fromBase = 10)
	{
		DataType = type;
		Address = address;
		FromBase = fromBase;
	}
}
