namespace HslCommunication.Profinet.Keyence;

public class KeyenceDataType
{
	public static readonly KeyenceDataType X = new KeyenceDataType(156, 1, "X*", 16);

	public static readonly KeyenceDataType Y = new KeyenceDataType(157, 1, "Y*", 16);

	public static readonly KeyenceDataType B = new KeyenceDataType(160, 1, "B*", 16);

	public static readonly KeyenceDataType M = new KeyenceDataType(144, 1, "M*", 10);

	public static readonly KeyenceDataType L = new KeyenceDataType(146, 1, "L*", 10);

	public static readonly KeyenceDataType SM = new KeyenceDataType(145, 1, "SM", 10);

	public static readonly KeyenceDataType SD = new KeyenceDataType(169, 0, "SD", 10);

	public static readonly KeyenceDataType D = new KeyenceDataType(168, 0, "D*", 10);

	public static readonly KeyenceDataType R = new KeyenceDataType(175, 0, "R*", 10);

	public static readonly KeyenceDataType ZR = new KeyenceDataType(176, 0, "ZR", 16);

	public static readonly KeyenceDataType W = new KeyenceDataType(180, 0, "W*", 16);

	public static readonly KeyenceDataType TN = new KeyenceDataType(194, 0, "TN", 10);

	public static readonly KeyenceDataType TS = new KeyenceDataType(193, 1, "TS", 10);

	public static readonly KeyenceDataType CN = new KeyenceDataType(197, 0, "CN", 10);

	public static readonly KeyenceDataType CS = new KeyenceDataType(196, 1, "CS", 10);

	public byte DataCode { get; private set; } = 0;

	public byte DataType { get; private set; } = 0;

	public string AsciiCode { get; private set; }

	public int FromBase { get; private set; }

	public KeyenceDataType(byte code, byte type, string asciiCode, int fromBase)
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
