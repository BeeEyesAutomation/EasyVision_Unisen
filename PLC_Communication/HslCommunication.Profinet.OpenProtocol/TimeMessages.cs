using System;
using System.Collections.Generic;

namespace HslCommunication.Profinet.OpenProtocol;

public class TimeMessages
{
	private OpenProtocolNet openProtocol;

	public TimeMessages(OpenProtocolNet openProtocol)
	{
		this.openProtocol = openProtocol;
	}

	public OperateResult<DateTime> ReadTimeUpload()
	{
		OperateResult<string> operateResult = openProtocol.ReadCustomer(80, 1, -1, -1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult);
		}
		return OperateResult.CreateSuccessResult(DateTime.ParseExact(operateResult.Content.Substring(20, 19), "yyyy-MM-dd:HH:mm:ss", null));
	}

	public OperateResult SetTime(DateTime dateTime)
	{
		return openProtocol.ReadCustomer(82, 1, -1, -1, new List<string> { dateTime.ToString("yyyy-MM-dd:HH:mm:ss") });
	}
}
