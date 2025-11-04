namespace HslCommunication.Profinet.Melsec;

public class MelsecA1EDataType
{
	public static readonly MelsecA1EDataType X = new MelsecA1EDataType(22560, 1, "X*", 16);

	public static readonly MelsecA1EDataType Y = new MelsecA1EDataType(22816, 1, "Y*", 16);

	public static readonly MelsecA1EDataType M = new MelsecA1EDataType(19744, 1, "M*", 10);

	public static readonly MelsecA1EDataType S = new MelsecA1EDataType(21280, 1, "S*", 10);

	public static readonly MelsecA1EDataType F = new MelsecA1EDataType(17952, 1, "F*", 10);

	public static readonly MelsecA1EDataType B = new MelsecA1EDataType(16928, 1, "B*", 16);

	public static readonly MelsecA1EDataType TS = new MelsecA1EDataType(21587, 1, "TS", 10);

	public static readonly MelsecA1EDataType TC = new MelsecA1EDataType(21571, 1, "TC", 10);

	public static readonly MelsecA1EDataType TN = new MelsecA1EDataType(21582, 0, "TN", 10);

	public static readonly MelsecA1EDataType CS = new MelsecA1EDataType(17235, 1, "CS", 10);

	public static readonly MelsecA1EDataType CC = new MelsecA1EDataType(17219, 1, "CC", 10);

	public static readonly MelsecA1EDataType CN = new MelsecA1EDataType(17230, 0, "CN", 10);

	public static readonly MelsecA1EDataType D = new MelsecA1EDataType(17440, 0, "D*", 10);

	public static readonly MelsecA1EDataType W = new MelsecA1EDataType(22304, 0, "W*", 16);

	public static readonly MelsecA1EDataType R = new MelsecA1EDataType(21024, 0, "R*", 10);

	public ushort DataCode { get; private set; } = 0;

	public byte DataType { get; private set; } = 0;

	public string AsciiCode { get; private set; }

	public int FromBase { get; private set; }

	public MelsecA1EDataType(ushort code, byte type, string asciiCode, int fromBase)
	{
		DataCode = code;
		AsciiCode = asciiCode;
		FromBase = fromBase;
		if (type < 2)
		{
			DataType = type;
		}
	}
}
