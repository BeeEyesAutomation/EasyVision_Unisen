using System.Reflection;
using System.Threading;
using HslCommunication.Reflection;
using Newtonsoft.Json;

namespace HslCommunication.MQTT;

public class MqttRpcApiInfo
{
	private long calledCount = 0L;

	private long spendTotalTime = 0L;

	private MethodInfo method;

	private PropertyInfo property;

	public string ApiTopic { get; set; }

	public string Description { get; set; }

	public string MethodSignature { get; set; }

	public long CalledCount
	{
		get
		{
			return calledCount;
		}
		set
		{
			calledCount = value;
		}
	}

	public string ExamplePayload { get; set; }

	public double SpendTotalTime
	{
		get
		{
			return (double)spendTotalTime / 100000.0;
		}
		set
		{
			spendTotalTime = (long)(value * 100000.0);
		}
	}

	public bool IsMethodApi { get; set; }

	public string HttpMethod { get; set; } = "GET";

	[JsonIgnore]
	public bool IsOperateResultApi { get; set; }

	[JsonIgnore]
	public MethodInfo Method
	{
		get
		{
			return method;
		}
		set
		{
			method = value;
			IsMethodApi = true;
		}
	}

	[JsonIgnore]
	public PropertyInfo Property
	{
		get
		{
			return property;
		}
		set
		{
			property = value;
			IsMethodApi = false;
		}
	}

	[JsonIgnore]
	public HslMqttPermissionAttribute PermissionAttribute { get; set; }

	[JsonIgnore]
	public object SourceObject { get; set; }

	public void CalledCountAddOne(long timeSpend)
	{
		Interlocked.Increment(ref calledCount);
		Interlocked.Add(ref spendTotalTime, timeSpend);
	}

	public override string ToString()
	{
		return ApiTopic;
	}
}
