using System;

namespace HslCommunication.Profinet.OpenProtocol;

public class JobItem
{
	public int ChannelID { get; set; }

	public int TypeID { get; set; }

	public int AutoValue { get; set; }

	public int BatchSize { get; set; }

	public JobItem()
	{
	}

	public JobItem(string data)
	{
		if (data.Length == 12)
		{
			data = data.Substring(0, 11);
		}
		string[] array = data.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
		ChannelID = Convert.ToInt32(array[0]);
		TypeID = Convert.ToInt32(array[1]);
		AutoValue = Convert.ToInt32(array[2]);
		BatchSize = Convert.ToInt32(array[3]);
	}
}
