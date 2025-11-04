namespace HslCommunication.Enthernet;

public class HttpApiCalledInfo
{
	public string Url { get; set; }

	public string Body { get; set; }

	public string HttpMethod { get; set; }

	public double CostTime { get; set; }

	public string Result { get; set; }

	public long CalledCount { get; set; }

	public override string ToString()
	{
		return Url;
	}
}
