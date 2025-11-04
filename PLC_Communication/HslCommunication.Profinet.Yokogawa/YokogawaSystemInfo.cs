using System;
using System.Text;

namespace HslCommunication.Profinet.Yokogawa;

public class YokogawaSystemInfo
{
	public string SystemID { get; set; }

	public string Revision { get; set; }

	public string CpuType { get; set; }

	public int ProgramAreaSize { get; set; }

	public override string ToString()
	{
		return "YokogawaSystemInfo[" + SystemID + "]";
	}

	public static OperateResult<YokogawaSystemInfo> Parse(byte[] content)
	{
		try
		{
			YokogawaSystemInfo yokogawaSystemInfo = new YokogawaSystemInfo();
			yokogawaSystemInfo.SystemID = Encoding.ASCII.GetString(content, 0, 16).Trim('\0', ' ');
			yokogawaSystemInfo.Revision = Encoding.ASCII.GetString(content, 16, 8).Trim('\0', ' ');
			if (content[25] == 1 || content[25] == 17)
			{
				yokogawaSystemInfo.CpuType = "Sequence";
			}
			else if (content[25] == 2 || content[25] == 18)
			{
				yokogawaSystemInfo.CpuType = "BASIC";
			}
			else
			{
				yokogawaSystemInfo.CpuType = StringResources.Language.UnknownError;
			}
			yokogawaSystemInfo.ProgramAreaSize = content[26] * 256 + content[27];
			return OperateResult.CreateSuccessResult(yokogawaSystemInfo);
		}
		catch (Exception ex)
		{
			return new OperateResult<YokogawaSystemInfo>("Parse YokogawaSystemInfo failed: " + ex.Message + Environment.NewLine + "Source: " + content.ToHexString(' '));
		}
	}
}
