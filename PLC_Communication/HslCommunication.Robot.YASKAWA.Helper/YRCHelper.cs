using System;
using System.Text.RegularExpressions;

namespace HslCommunication.Robot.YASKAWA.Helper;

public class YRCHelper
{
	public static string GetErrorMessage(int err)
	{
		if (1 == 0)
		{
		}
		string result = err switch
		{
			1010 => StringResources.Language.YRC1010, 
			1011 => StringResources.Language.YRC1011, 
			1012 => StringResources.Language.YRC1012, 
			1013 => StringResources.Language.YRC1013, 
			1020 => StringResources.Language.YRC1020, 
			2010 => StringResources.Language.YRC2010, 
			2020 => StringResources.Language.YRC2020, 
			2030 => StringResources.Language.YRC2030, 
			2040 => StringResources.Language.YRC2040, 
			2050 => StringResources.Language.YRC2050, 
			2060 => StringResources.Language.YRC2060, 
			2070 => StringResources.Language.YRC2070, 
			2080 => StringResources.Language.YRC2080, 
			2090 => StringResources.Language.YRC2090, 
			2100 => StringResources.Language.YRC2100, 
			2110 => StringResources.Language.YRC2110, 
			2120 => StringResources.Language.YRC2120, 
			2130 => StringResources.Language.YRC2130, 
			2150 => StringResources.Language.YRC2150, 
			3010 => StringResources.Language.YRC3010, 
			3040 => StringResources.Language.YRC3040, 
			3050 => StringResources.Language.YRC3050, 
			3070 => StringResources.Language.YRC3070, 
			3220 => StringResources.Language.YRC3220, 
			3230 => StringResources.Language.YRC3230, 
			3350 => StringResources.Language.YRC3350, 
			3360 => StringResources.Language.YRC3360, 
			3370 => StringResources.Language.YRC3370, 
			3380 => StringResources.Language.YRC3380, 
			3390 => StringResources.Language.YRC3390, 
			3400 => StringResources.Language.YRC3400, 
			3410 => StringResources.Language.YRC3410, 
			3420 => StringResources.Language.YRC3420, 
			3430 => StringResources.Language.YRC3430, 
			3450 => StringResources.Language.YRC3450, 
			3460 => StringResources.Language.YRC3460, 
			4010 => StringResources.Language.YRC4010, 
			4012 => StringResources.Language.YRC4012, 
			4020 => StringResources.Language.YRC4020, 
			4030 => StringResources.Language.YRC4030, 
			4040 => StringResources.Language.YRC4040, 
			4060 => StringResources.Language.YRC4060, 
			4120 => StringResources.Language.YRC4120, 
			4130 => StringResources.Language.YRC4130, 
			4140 => StringResources.Language.YRC4140, 
			4150 => StringResources.Language.YRC4150, 
			4170 => StringResources.Language.YRC4170, 
			4190 => StringResources.Language.YRC4190, 
			4200 => StringResources.Language.YRC4200, 
			4230 => StringResources.Language.YRC4230, 
			4420 => StringResources.Language.YRC4420, 
			4430 => StringResources.Language.YRC4430, 
			4480 => StringResources.Language.YRC4480, 
			4490 => StringResources.Language.YRC4490, 
			5110 => StringResources.Language.YRC5110, 
			5120 => StringResources.Language.YRC5120, 
			5130 => StringResources.Language.YRC5130, 
			5170 => StringResources.Language.YRC5170, 
			5180 => StringResources.Language.YRC5180, 
			5200 => StringResources.Language.YRC5200, 
			5310 => StringResources.Language.YRC5310, 
			5340 => StringResources.Language.YRC5340, 
			5370 => StringResources.Language.YRC5370, 
			5390 => StringResources.Language.YRC5390, 
			5430 => StringResources.Language.YRC5430, 
			5480 => StringResources.Language.YRC5480, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<string> ExtraErrorMessage(string errText)
	{
		Match match = Regex.Match(errText, "\\([0-9]+\\)\\.$");
		if (match.Success)
		{
			string s = match.Value.Substring(1, match.Value.Length - 3);
			if (int.TryParse(s, out var result))
			{
				return new OperateResult<string>(errText + Environment.NewLine + GetErrorMessage(result));
			}
			return new OperateResult<string>(errText);
		}
		return new OperateResult<string>(errText);
	}
}
