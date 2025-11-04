using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Robot.ABB;

public class ABBWebApiClient : NetworkWebApiRobotBase, IRobotNet
{
	public ABBWebApiClient(string ipAddress)
		: base(ipAddress)
	{
	}

	public ABBWebApiClient(string ipAddress, int port)
		: base(ipAddress, port)
	{
	}

	public ABBWebApiClient(string ipAddress, int port, string name, string password)
		: base(ipAddress, port, name, password)
	{
	}

	[HslMqttApi(ApiTopic = "ReadRobotByte", Description = "Read the other side of the data information, usually designed for the GET method information.If you start with url=, you are using native address access")]
	public override OperateResult<byte[]> Read(string address)
	{
		return base.Read(address);
	}

	[HslMqttApi(ApiTopic = "ReadRobotString", Description = "The string data information that reads the other party information, usually designed for the GET method information.If you start with url=, you are using native address access")]
	public override OperateResult<string> ReadString(string address)
	{
		return base.ReadString(address);
	}

	[HslMqttApi(ApiTopic = "WriteRobotByte", Description = "Using POST to request data information from the other party, we need to start with url= to indicate that we are using native address access")]
	public override OperateResult Write(string address, byte[] value)
	{
		return base.Write(address, value);
	}

	[HslMqttApi(ApiTopic = "WriteRobotString", Description = "Using POST to request data information from the other party, we need to start with url= to indicate that we are using native address access")]
	public override OperateResult Write(string address, string value)
	{
		return base.Write(address, value);
	}

	protected override OperateResult<string> ReadByAddress(string address)
	{
		if (address.ToUpper() == "ErrorState".ToUpper())
		{
			return GetErrorState();
		}
		if (address.ToUpper() == "jointtarget".ToUpper())
		{
			return GetJointTarget();
		}
		if (address.ToUpper() == "PhysicalJoints".ToUpper())
		{
			return GetJointTarget();
		}
		if (address.ToUpper() == "SpeedRatio".ToUpper())
		{
			return GetSpeedRatio();
		}
		if (address.ToUpper() == "OperationMode".ToUpper())
		{
			return GetOperationMode();
		}
		if (address.ToUpper() == "CtrlState".ToUpper())
		{
			return GetCtrlState();
		}
		if (address.ToUpper() == "ioin".ToUpper())
		{
			return GetIOIn();
		}
		if (address.ToUpper() == "ioout".ToUpper())
		{
			return GetIOOut();
		}
		if (address.ToUpper() == "io2in".ToUpper())
		{
			return GetIO2In();
		}
		if (address.ToUpper() == "io2out".ToUpper())
		{
			return GetIO2Out();
		}
		if (address.ToUpper().StartsWith("log".ToUpper()))
		{
			if (address.Length > 3 && int.TryParse(address.Substring(3), out var result))
			{
				return GetLog(result);
			}
			return GetLog();
		}
		if (address.ToUpper() == "system".ToUpper())
		{
			return GetSystem();
		}
		if (address.ToUpper() == "robtarget".ToUpper())
		{
			return GetRobotTarget();
		}
		if (address.ToUpper() == "ServoEnable".ToUpper())
		{
			return GetServoEnable();
		}
		if (address.ToUpper() == "RapidExecution".ToUpper())
		{
			return GetRapidExecution();
		}
		if (address.ToUpper() == "RapidTasks".ToUpper())
		{
			return GetRapidTasks();
		}
		return base.ReadByAddress(address);
	}

	protected override async Task<OperateResult<string>> ReadByAddressAsync(string address)
	{
		if (address.ToUpper() == "ErrorState".ToUpper())
		{
			return await GetErrorStateAsync();
		}
		if (address.ToUpper() == "jointtarget".ToUpper())
		{
			return await GetJointTargetAsync();
		}
		if (address.ToUpper() == "PhysicalJoints".ToUpper())
		{
			return await GetJointTargetAsync();
		}
		if (address.ToUpper() == "SpeedRatio".ToUpper())
		{
			return await GetSpeedRatioAsync();
		}
		if (address.ToUpper() == "OperationMode".ToUpper())
		{
			return await GetOperationModeAsync();
		}
		if (address.ToUpper() == "CtrlState".ToUpper())
		{
			return await GetCtrlStateAsync();
		}
		if (address.ToUpper() == "ioin".ToUpper())
		{
			return await GetIOInAsync();
		}
		if (address.ToUpper() == "ioout".ToUpper())
		{
			return await GetIOOutAsync();
		}
		if (address.ToUpper() == "io2in".ToUpper())
		{
			return await GetIO2InAsync();
		}
		if (address.ToUpper() == "io2out".ToUpper())
		{
			return await GetIO2OutAsync();
		}
		if (address.ToUpper().StartsWith("log".ToUpper()))
		{
			if (address.Length > 3 && int.TryParse(address.Substring(3), out var length))
			{
				return await GetLogAsync(length);
			}
			return await GetLogAsync();
		}
		if (address.ToUpper() == "system".ToUpper())
		{
			return await GetSystemAsync();
		}
		if (address.ToUpper() == "robtarget".ToUpper())
		{
			return await GetRobotTargetAsync();
		}
		if (address.ToUpper() == "ServoEnable".ToUpper())
		{
			return await GetServoEnableAsync();
		}
		if (address.ToUpper() == "RapidExecution".ToUpper())
		{
			return await GetRapidExecutionAsync();
		}
		if (address.ToUpper() == "RapidTasks".ToUpper())
		{
			return await GetRapidTasksAsync();
		}
		return await base.ReadByAddressAsync(address);
	}

	public static List<string> GetSelectStrings()
	{
		return new List<string>
		{
			"ErrorState", "jointtarget", "PhysicalJoints", "SpeedRatio", "OperationMode", "CtrlState", "ioin", "ioout", "io2in", "io2out",
			"log", "system", "robtarget", "ServoEnable", "RapidExecution", "RapidTasks"
		};
	}

	private OperateResult<string> ParseSpanByClass(string content, string className)
	{
		Match match = Regex.Match(content, "<span class=\"" + className + "\">[^<]+");
		if (!match.Success)
		{
			return new OperateResult<string>("Parse None class [" + className + "] Span\r\n" + content);
		}
		return OperateResult.CreateSuccessResult(match.Value.Substring(match.Value.IndexOf('>') + 1));
	}

	private OperateResult<double[]> ParseDoubleListSpanByClass(string content, string className)
	{
		MatchCollection matchCollection = Regex.Matches(content, "<span class=\"" + className + "\">[^<]+");
		double[] array = new double[matchCollection.Count];
		for (int i = 0; i < matchCollection.Count; i++)
		{
			array[i] = Convert.ToDouble(matchCollection[i].Value.Substring(matchCollection[i].Value.IndexOf('>') + 1));
		}
		return OperateResult.CreateSuccessResult(array);
	}

	private OperateResult<string> ParseListSpanByClass<T>(string content, string className, Func<string, T> trans)
	{
		MatchCollection matchCollection = Regex.Matches(content, "<span class=\"" + className + "\">[^<]+");
		JArray jArray = new JArray();
		for (int i = 0; i < matchCollection.Count; i++)
		{
			jArray.Add(trans(matchCollection[i].Value.Substring(matchCollection[i].Value.IndexOf('>') + 1)));
		}
		return OperateResult.CreateSuccessResult(jArray.ToString());
	}

	private JObject ParseListByClass(string content)
	{
		XElement xElement = XElement.Parse(content);
		JObject jObject = new JObject();
		foreach (XElement item in xElement.Elements("span"))
		{
			jObject.Add(item.Attribute("class").Value, item.Value);
		}
		return jObject;
	}

	private OperateResult<string> ParseJObjectByClass(string content, string className)
	{
		Match match = Regex.Match(content, "<li class=\"" + className + "\"[\\S\\s]+?</li>");
		if (!match.Success)
		{
			return new OperateResult<string>("Parse None class [" + className + "] List\r\n" + content);
		}
		return OperateResult.CreateSuccessResult(ParseListByClass(match.Value).ToString());
	}

	private OperateResult<string> ParseJArrayByClass(string content, string className, int maxCount = int.MaxValue)
	{
		MatchCollection matchCollection = Regex.Matches(content, "<li class=\"" + className + "\"[\\S\\s]+?</li>");
		JArray jArray = new JArray();
		for (int i = 0; i < matchCollection.Count && i < maxCount; i++)
		{
			jArray.Add(ParseListByClass(matchCollection[i].Value));
		}
		return OperateResult.CreateSuccessResult(jArray.ToString());
	}

	[HslMqttApi(Description = "Get the current control state. The Content attribute is the control information of the robot")]
	public OperateResult<string> GetCtrlState()
	{
		return ReadString("url=/rw/panel/ctrlstate").Then((string m) => ParseSpanByClass(m, "ctrlstate"));
	}

	[HslMqttApi(Description = "Gets the current error state. The Content attribute is the state information of the robot")]
	public OperateResult<string> GetErrorState()
	{
		return ReadString("url=/rw/motionsystem/errorstate").Then((string m) => ParseSpanByClass(m, "err-state"));
	}

	[HslMqttApi(Description = "Get the physical node information of the current robot and return the joint information in json format")]
	public OperateResult<string> GetJointTarget(string mechunit = "ROB_1")
	{
		return ReadString("url=/rw/motionsystem/mechunits/" + mechunit + "/jointtarget").Then((string m) => ParseListSpanByClass(m, "((rax_[0-9]+)|(eax_[a-z]))", (string n) => Convert.ToDouble(n)));
	}

	[HslMqttApi(Description = "Get the speed matching information of the current robot")]
	public OperateResult<string> GetSpeedRatio()
	{
		return ReadString("url=/rw/panel/speedratio").Then((string m) => ParseSpanByClass(m, "speedratio"));
	}

	[HslMqttApi(Description = "Gets the current working mode of the robot")]
	public OperateResult<string> GetOperationMode()
	{
		return ReadString("url=/rw/panel/opmode").Then((string m) => ParseSpanByClass(m, "opmode"));
	}

	[HslMqttApi(Description = "Gets the input IO of the current robot's native")]
	public OperateResult<string> GetIOIn()
	{
		return ReadString("url=/rw/iosystem/devices/D652_10").Then((string m) => ParseSpanByClass(m, "indata"));
	}

	[HslMqttApi(Description = "Gets the output IO of the current robot's native")]
	public OperateResult<string> GetIOOut()
	{
		return ReadString("url=/rw/iosystem/devices/D652_10").Then((string m) => ParseSpanByClass(m, "outdata"));
	}

	[HslMqttApi(Description = "Gets the input IO2 of the current robot's native")]
	public OperateResult<string> GetIO2In()
	{
		return ReadString("url=/rw/iosystem/devices/BK5250").Then((string m) => ParseSpanByClass(m, "indata"));
	}

	[HslMqttApi(Description = "Gets the output IO2 of the current robot's native")]
	public OperateResult<string> GetIO2Out()
	{
		return ReadString("url=/rw/iosystem/devices/BK5250").Then((string m) => ParseSpanByClass(m, "outdata"));
	}

	[HslMqttApi(Description = "Gets the log record for the current robot, which is 10 by default")]
	public OperateResult<string> GetLog(int logCount = 10)
	{
		return ReadString("url=/rw/elog/0?lang=zh&amp;resource=title").Then((string m) => ParseJArrayByClass(m, "elog-message-li", logCount));
	}

	[HslMqttApi(Description = "Get the current robot's system information, version number, unique ID and other information")]
	public OperateResult<string> GetSystem()
	{
		return ReadString("url=/rw/system").Then((string m) => ParseJObjectByClass(m, "sys-system-li"));
	}

	[HslMqttApi(Description = "Get the current robot's target information")]
	public OperateResult<string> GetRobotTarget()
	{
		return ReadString("url=/rw/motionsystem/mechunits/ROB_1/robtarget").Then((string m) => ParseJObjectByClass(m, "ms-robtargets"));
	}

	[HslMqttApi(Description = "Get the current robot servo enable state")]
	public OperateResult<string> GetServoEnable()
	{
		return ReadString("url=/rw/iosystem/signals/Local/DRV_1/DRV1K1").Then((string m) => ParseJObjectByClass(m, "ios-signal"));
	}

	[HslMqttApi(Description = "Get the current program running status of the current robot")]
	public OperateResult<string> GetRapidExecution()
	{
		return ReadString("url=/rw/rapid/execution").Then((string m) => ParseJObjectByClass(m, "rap-execution"));
	}

	[HslMqttApi(Description = "Get the task list of the current robot")]
	public OperateResult<string> GetRapidTasks()
	{
		return ReadString("url=/rw/rapid/tasks").Then((string m) => ParseJArrayByClass(m, "rap-task-li"));
	}

	public OperateResult<double[]> GetUserValue(string name)
	{
		return ReadString(name.StartsWith("url=", StringComparison.OrdinalIgnoreCase) ? name : ("url=/rw/rapid/symbol/data/RAPID/T_ROB1/user/" + name)).Then((string m) => ParseDoubleListSpanByClass(m, "value"));
	}

	[HslMqttApi(Description = "Get the current robot's target information")]
	public OperateResult<string> GetAnIOSignal(string network = "Local", string unit = "DRV_1", string signal = "DRV1K1")
	{
		return ReadString("url=/rw/iosystem/signals/" + network + "/" + unit + "/" + signal).Then((string m) => ParseJObjectByClass(m, "ios-signal"));
	}

	public async Task<OperateResult<string>> GetCtrlStateAsync()
	{
		return (await ReadStringAsync("url=/rw/panel/ctrlstate").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "ctrlstate"));
	}

	public async Task<OperateResult<string>> GetErrorStateAsync()
	{
		return (await ReadStringAsync("url=/rw/motionsystem/errorstate").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "err-state"));
	}

	public async Task<OperateResult<string>> GetJointTargetAsync(string mechunit = "ROB_1")
	{
		return (await ReadStringAsync("url=/rw/motionsystem/mechunits/" + mechunit + "/jointtarget").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseListSpanByClass(m, "((rax_[0-9]+)|(eax_[a-z]))", (string n) => Convert.ToDouble(n)));
	}

	public async Task<OperateResult<string>> GetSpeedRatioAsync()
	{
		return (await ReadStringAsync("url=/rw/panel/speedratio").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "speedratio"));
	}

	public async Task<OperateResult<string>> GetOperationModeAsync()
	{
		return (await ReadStringAsync("url=/rw/panel/opmode").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "opmode"));
	}

	public async Task<OperateResult<string>> GetIOInAsync()
	{
		return (await ReadStringAsync("url=/rw/iosystem/devices/D652_10").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "indata"));
	}

	public async Task<OperateResult<string>> GetIOOutAsync()
	{
		return (await ReadStringAsync("url=/rw/iosystem/devices/D652_10").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "outdata"));
	}

	public async Task<OperateResult<string>> GetIO2InAsync()
	{
		return (await ReadStringAsync("url=/rw/iosystem/devices/BK5250").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "indata"));
	}

	public async Task<OperateResult<string>> GetIO2OutAsync()
	{
		return (await ReadStringAsync("url=/rw/iosystem/devices/BK5250").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseSpanByClass(m, "outdata"));
	}

	public async Task<OperateResult<string>> GetLogAsync(int logCount = 10)
	{
		return (await ReadStringAsync("url=/rw/elog/0?lang=zh&amp;resource=title").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseJArrayByClass(m, "elog-message-li", logCount));
	}

	public async Task<OperateResult<string>> GetSystemAsync()
	{
		return (await ReadStringAsync("url=/rw/system").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseJObjectByClass(m, "sys-system-li"));
	}

	public async Task<OperateResult<string>> GetRobotTargetAsync()
	{
		return (await ReadStringAsync("url=/rw/motionsystem/mechunits/ROB_1/robtarget").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseJObjectByClass(m, "ms-robtargets"));
	}

	public async Task<OperateResult<string>> GetServoEnableAsync()
	{
		return (await ReadStringAsync("url=/rw/iosystem/signals/Local/DRV_1/DRV1K1").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseJObjectByClass(m, "ios-signal"));
	}

	public async Task<OperateResult<string>> GetRapidExecutionAsync()
	{
		return (await ReadStringAsync("url=/rw/rapid/execution").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseJObjectByClass(m, "rap-execution"));
	}

	public async Task<OperateResult<string>> GetRapidTasksAsync()
	{
		return (await ReadStringAsync("url=/rw/rapid/tasks").ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseJArrayByClass(m, "rap-task-li"));
	}

	public async Task<OperateResult<double[]>> GetUserValueAsync(string name)
	{
		return (await ReadStringAsync(name.StartsWith("url=", StringComparison.OrdinalIgnoreCase) ? name : ("url=/rw/rapid/symbol/data/RAPID/T_ROB1/user/" + name)).ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseDoubleListSpanByClass(m, "value"));
	}

	public async Task<OperateResult<string>> GetAnIOSignalAsync(string network = "Local", string unit = "DRV_1", string signal = "DRV1K1")
	{
		return (await ReadStringAsync("url=/rw/iosystem/signals/" + network + "/" + unit + "/" + signal).ConfigureAwait(continueOnCapturedContext: false)).Then((string m) => ParseJObjectByClass(m, "ios-signal"));
	}

	public override string ToString()
	{
		return $"ABBWebApiClient[{base.IpAddress}:{base.Port}]";
	}
}
