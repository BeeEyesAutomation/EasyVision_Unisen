using System;

namespace HslCommunication.Core;

public class MqttFileOperateInfo
{
	public string Operate { get; set; }

	public string Groups { get; set; }

	public string[] FileNames { get; set; }

	public string[] MappingNames { get; set; }

	public TimeSpan TimeCost { get; set; }
}
