using System;
using System.Collections.Generic;

namespace HslCommunication.Profinet.OpenProtocol;

public class ParameterSetMessages
{
	private OpenProtocolNet openProtocol;

	public ParameterSetMessages(OpenProtocolNet openProtocol)
	{
		this.openProtocol = openProtocol;
	}

	public OperateResult<int[]> ParameterSetIDUpload()
	{
		OperateResult<string> operateResult = openProtocol.ReadCustomer(10, 1, -1, -1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int[]>(operateResult);
		}
		return PraseMID0011(operateResult.Content);
	}

	public OperateResult<ParameterSetData> ParameterSetDataUpload(int id)
	{
		OperateResult<string> operateResult = openProtocol.ReadCustomer(12, 1, -1, -1, new List<string> { id.ToString("D3") });
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ParameterSetData>(operateResult);
		}
		return PraseMID0012(operateResult.Content);
	}

	public OperateResult ParameterSetSelectedSubscribe()
	{
		return openProtocol.ReadCustomer(14, 1, -1, -1, null);
	}

	public OperateResult ParameterSetSelectedUnsubscribe()
	{
		return openProtocol.ReadCustomer(17, 1, -1, -1, null);
	}

	public OperateResult SelectParameterSet(int id)
	{
		return openProtocol.ReadCustomer(18, 1, -1, -1, new List<string> { id.ToString("D3") });
	}

	public OperateResult SetParameterSetBatchSize(int id, int batchSize)
	{
		return openProtocol.ReadCustomer(19, 1, -1, -1, new List<string>
		{
			id.ToString("D3"),
			batchSize.ToString("D2")
		});
	}

	public OperateResult ResetParameterSetBatchCounter(int id)
	{
		return openProtocol.ReadCustomer(20, 1, -1, -1, new List<string> { id.ToString("D3") });
	}

	private OperateResult<int[]> PraseMID0011(string reply)
	{
		try
		{
			int num = Convert.ToInt32(reply.Substring(20, 3));
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = Convert.ToInt32(reply.Substring(23 + i * 3, 3));
			}
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<int[]>("MID0011 prase failed: " + ex.Message + Environment.NewLine + "Source: " + reply);
		}
	}

	private OperateResult<ParameterSetData> PraseMID0012(string reply)
	{
		try
		{
			ParameterSetData parameterSetData = new ParameterSetData();
			parameterSetData.ParameterSetID = Convert.ToInt32(reply.Substring(22, 3));
			parameterSetData.ParameterSetName = reply.Substring(27, 25).Trim();
			parameterSetData.RotationDirection = ((reply[54] == '1') ? "CW" : "CCW");
			parameterSetData.BatchSize = Convert.ToInt32(reply.Substring(57, 2));
			parameterSetData.TorqueMin = Convert.ToDouble(reply.Substring(61, 6)) / 100.0;
			parameterSetData.TorqueMax = Convert.ToDouble(reply.Substring(69, 6)) / 100.0;
			parameterSetData.TorqueFinalTarget = Convert.ToDouble(reply.Substring(77, 6)) / 100.0;
			parameterSetData.AngleMin = Convert.ToInt32(reply.Substring(85, 5));
			parameterSetData.AngleMax = Convert.ToInt32(reply.Substring(92, 5));
			parameterSetData.AngleFinalTarget = Convert.ToInt32(reply.Substring(99, 5));
			return OperateResult.CreateSuccessResult(parameterSetData);
		}
		catch (Exception ex)
		{
			return new OperateResult<ParameterSetData>("MID0013 prase failed: " + ex.Message + Environment.NewLine + "Source: " + reply);
		}
	}
}
