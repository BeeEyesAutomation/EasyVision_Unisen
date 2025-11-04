using System;
using HslCommunication.Secs.Types;

namespace HslCommunication.Secs;

public class Gem
{
	private ISecs secs;

	public Gem(ISecs secs)
	{
		this.secs = secs;
	}

	public OperateResult<OnlineData> S1F1_AreYouThere()
	{
		OperateResult<SecsMessage> operateResult = secs.ReadSecsMessage(1, 1, new SecsValue(), back: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<OnlineData>(operateResult);
		}
		return OperateResult.CreateSuccessResult((OnlineData)operateResult.Content.GetItemValues());
	}

	public OperateResult<VariableName[]> S1F11_StatusVariableNamelist()
	{
		OperateResult<SecsMessage> operateResult = secs.ReadSecsMessage(1, 11, new SecsValue(), back: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<VariableName[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.GetItemValues().ToVaruableNames());
	}

	public OperateResult<VariableName[]> S1F11_StatusVariableNamelist(params int[] statusVaruableId)
	{
		OperateResult<SecsMessage> operateResult = secs.ReadSecsMessage(1, 11, new SecsValue(statusVaruableId), back: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<VariableName[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.GetItemValues().ToVaruableNames());
	}

	public OperateResult<OnlineData> S1F13_EstablishCommunications()
	{
		OperateResult<SecsMessage> operateResult = secs.ReadSecsMessage(1, 13, new SecsValue(), back: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<OnlineData>(operateResult);
		}
		SecsValue itemValues = operateResult.Content.GetItemValues();
		SecsValue[] array = itemValues.Value as SecsValue[];
		byte[] array2 = (byte[])array[0].Value;
		return (array2[0] == 0) ? OperateResult.CreateSuccessResult((OnlineData)array[1]) : new OperateResult<OnlineData>($"establish communications acknowledgement denied! source: {Environment.NewLine}{itemValues.ToXElement()}");
	}

	public OperateResult<byte> S1F15_OfflineRequest()
	{
		OperateResult<SecsMessage> operateResult = secs.ReadSecsMessage(1, 15, new SecsValue(), back: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte>(operateResult);
		}
		return OperateResult.CreateSuccessResult(((byte[])operateResult.Content.GetItemValues().Value)[0]);
	}

	public OperateResult<byte> S1F17_OnlineRequest()
	{
		OperateResult<SecsMessage> operateResult = secs.ReadSecsMessage(1, 17, new SecsValue(), back: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte>(operateResult);
		}
		return OperateResult.CreateSuccessResult(((byte[])operateResult.Content.GetItemValues().Value)[0]);
	}

	public OperateResult<SecsValue> S2F13_EquipmentConstantRequest(object[] list = null)
	{
		OperateResult<SecsMessage> operateResult = secs.ReadSecsMessage(2, 13, new SecsValue(list), back: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<SecsValue>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.GetItemValues());
	}
}
