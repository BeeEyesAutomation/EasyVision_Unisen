namespace HslCommunication.Secs.Types;

public class OnlineData
{
	public string ModelType { get; set; }

	public string SoftVersion { get; set; }

	public OnlineData()
	{
	}

	public OnlineData(string model, string version)
	{
		ModelType = model;
		SoftVersion = version;
	}

	public static implicit operator OnlineData(SecsValue value)
	{
		TypeHelper.TypeListCheck(value);
		if (value.Value is SecsValue[] array)
		{
			return new OnlineData(array[0].Value.ToString(), array[1].Value.ToString());
		}
		return null;
	}

	public static implicit operator SecsValue(OnlineData value)
	{
		return new SecsValue(new object[2] { value.ModelType, value.SoftVersion });
	}
}
