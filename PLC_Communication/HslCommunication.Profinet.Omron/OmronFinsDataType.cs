namespace HslCommunication.Profinet.Omron;

public class OmronFinsDataType
{
	public static readonly OmronFinsDataType DM = new OmronFinsDataType(2, 130);

	public static readonly OmronFinsDataType CIO = new OmronFinsDataType(48, 176);

	public static readonly OmronFinsDataType WR = new OmronFinsDataType(49, 177);

	public static readonly OmronFinsDataType HR = new OmronFinsDataType(50, 178);

	public static readonly OmronFinsDataType AR = new OmronFinsDataType(51, 179);

	public static readonly OmronFinsDataType TIM = new OmronFinsDataType(9, 137);

	public byte BitCode { get; private set; }

	public byte WordCode { get; private set; }

	public OmronFinsDataType(byte bitCode, byte wordCode)
	{
		BitCode = bitCode;
		WordCode = wordCode;
	}
}
