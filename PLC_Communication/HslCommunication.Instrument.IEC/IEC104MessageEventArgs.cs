using System;

namespace HslCommunication.Instrument.IEC;

public class IEC104MessageEventArgs : EventArgs
{
	public byte[] ASDU { get; }

	public byte TypeID { get; set; }

	public bool IsAddressContinuous { get; set; }

	public int InfoObjectCount { get; set; }

	public int TransmissionReason { get; set; }

	public int StationAddress { get; set; }

	public byte[] Body { get; set; }

	public IEC104MessageEventArgs(byte[] asdu)
	{
		ASDU = asdu;
		if (asdu != null)
		{
			TypeID = asdu[0];
			IsAddressContinuous = (asdu[1] & 0x80) == 128;
			InfoObjectCount = asdu[1] & 0x7F;
			TransmissionReason = asdu[2] & 0x3F;
			StationAddress = BitConverter.ToUInt16(asdu, 4);
			Body = asdu.RemoveBegin(6);
		}
	}

	public string GetTransmissionReasonText()
	{
		int transmissionReason = TransmissionReason;
		if (1 == 0)
		{
		}
		string result = transmissionReason switch
		{
			1 => "周期循环", 
			2 => "背景扫描", 
			3 => "突发", 
			4 => "初始化", 
			5 => "请求或被请求", 
			6 => "激活", 
			7 => "激活确认", 
			8 => "停止激活", 
			9 => "停止激活确认", 
			10 => "激活终止", 
			11 => "远方命令引起的返送信息", 
			12 => "当地命令引起的返送信息", 
			20 => "响应站召唤", 
			21 => "响应第1组召唤", 
			44 => "未知的类型标识", 
			45 => "未知的传送原因", 
			46 => "未知的应用服务器数据单元公共地址", 
			47 => "未知的信息对象地址", 
			_ => TransmissionReason.ToString(), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public string GetTypeIDText()
	{
		switch (TypeID)
		{
		case 1:
			return "单点遥信";
		case 3:
			return "双点遥信";
		case 5:
			return "步位置信息";
		case 7:
			return "32比特串";
		case 9:
			return "归一化遥测值";
		case 11:
			return "标度化遥测值";
		case 13:
			return "单浮点遥测值";
		case 15:
			return "累计量";
		case 20:
			return "成组单点遥信";
		case 21:
			return "归一化遥测值";
		case 30:
			return "单点遥信带时标";
		case 31:
			return "双点遥信带时标";
		case 32:
			return "步位置遥信带时标";
		case 33:
			return "32比特串带时标";
		case 34:
			return "归一化遥测值带时标";
		case 35:
			return "标度化遥测值带时标";
		case 36:
			return "短浮点遥测值带时标";
		case 37:
			return "累计量带时标";
		case 38:
			return "继电保护装置事件";
		case 39:
			return "继电保护装置成组启动事件";
		case 40:
			return "继电保护装置成组出口信息";
		case 45:
		case 58:
			return "单点遥控";
		case 46:
		case 59:
			return "双点遥控";
		case 47:
		case 60:
			return "升降遥控";
		case 48:
		case 61:
		case 136:
			return "归一化设置值";
		case 49:
		case 62:
			return "标度化设定值";
		case 50:
		case 63:
			return "短浮点设定值";
		case 51:
		case 64:
			return "32比特串";
		case 70:
			return "初始化结束";
		case 100:
			return "总召唤";
		case 101:
			return "积累量召唤";
		case 102:
			return "读命令";
		case 103:
			return "时钟同步命令";
		case 105:
			return "复位进程命令";
		case 107:
			return "带时标的命令测试";
		default:
			return $"Unknown[{TypeID}]";
		}
	}

	public bool WithTimeInfo()
	{
		if (TypeID < 30)
		{
			return false;
		}
		if (TypeID < 45)
		{
			return true;
		}
		if (TypeID < 58)
		{
			return false;
		}
		if (TypeID < 70)
		{
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		return string.Format("{0} [{1}:{2}] Reason[{3}] Pub-Address[{4}]", GetTypeIDText(), IsAddressContinuous ? "Continuous" : "Uncontinuous", InfoObjectCount, GetTransmissionReasonText(), StationAddress);
	}
}
