namespace HslCommunication.Profinet.Beckhoff.Helper;

public class AdsTagItem
{
	public string TagName { get; set; }

	public byte[] Buffer { get; set; }

	public uint Location { get; set; }

	public int TypeLength { get; private set; }

	public AdsTagItem(string name, byte[] buffer, int typeLength)
	{
		TagName = name;
		Buffer = buffer;
		TypeLength = typeLength;
	}
}
