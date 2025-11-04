using System;
using System.Collections.Generic;

namespace HslCommunication.Profinet.OpenProtocol;

public class JobMessage
{
	private OpenProtocolNet openProtocol;

	public JobMessage(OpenProtocolNet openProtocol)
	{
		this.openProtocol = openProtocol;
	}

	public OperateResult<int[]> JobIDUpload(int revision = 1)
	{
		OperateResult<string> operateResult = openProtocol.ReadCustomer(30, revision, -1, -1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int[]>(operateResult);
		}
		return PraseMID0031(operateResult.Content);
	}

	public OperateResult<JobData> JobDataUpload(int id)
	{
		OperateResult<string> operateResult = openProtocol.ReadCustomer(32, 1, -1, -1, new List<string> { id.ToString("D2") });
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<JobData>(operateResult);
		}
		return PraseMID0033(operateResult.Content);
	}

	public OperateResult JobInfoSubscribe()
	{
		return openProtocol.ReadCustomer(34, 1, -1, -1, null);
	}

	public OperateResult JobInfoUnsubscribe()
	{
		return openProtocol.ReadCustomer(37, 1, -1, -1, null);
	}

	public OperateResult SelectJob(int id, int revision = 1)
	{
		return openProtocol.ReadCustomer(38, revision, -1, -1, new List<string> { (revision == 1) ? id.ToString("D2") : id.ToString("D4") });
	}

	public OperateResult JobRestart(int id)
	{
		return openProtocol.ReadCustomer(39, 1, -1, -1, new List<string> { id.ToString("D2") });
	}

	private OperateResult<int[]> PraseMID0031(string reply)
	{
		try
		{
			int num = Convert.ToInt32(reply.Substring(8, 3));
			int num2 = ((num == 1) ? 2 : 4);
			int num3 = Convert.ToInt32(reply.Substring(20, num2));
			int[] array = new int[num3];
			for (int i = 0; i < num3; i++)
			{
				array[i] = Convert.ToInt32(reply.Substring(20 + num2 + i * num2, num2));
			}
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<int[]>("MID0031 prase failed: " + ex.Message + Environment.NewLine + "Source: " + reply);
		}
	}

	private OperateResult<JobData> PraseMID0033(string reply)
	{
		try
		{
			JobData jobData = new JobData();
			jobData.JobID = Convert.ToInt32(reply.Substring(22, 2));
			jobData.JobName = reply.Substring(26, 25).Trim();
			jobData.ForcedOrder = Convert.ToInt32(reply.Substring(53, 1));
			jobData.MaxTimeForFirstTightening = Convert.ToInt32(reply.Substring(56, 4));
			jobData.MaxTimeToCompleteJob = Convert.ToInt32(reply.Substring(62, 5));
			jobData.JobBatchMode = Convert.ToInt32(reply.Substring(69, 1));
			jobData.LockAtJobDone = reply[72] == '1';
			jobData.UseLineControl = reply[75] == '1';
			jobData.RepeatJob = reply[78] == '1';
			jobData.ToolLoosening = Convert.ToInt32(reply.Substring(81, 1));
			jobData.Reserved = Convert.ToInt32(reply.Substring(86, 1));
			jobData.JobList = new List<JobItem>();
			int num = Convert.ToInt32(reply.Substring(89, 2));
			for (int i = 0; i < num; i++)
			{
				jobData.JobList.Add(new JobItem(reply.Substring(92 + i * 12, 11)));
			}
			return OperateResult.CreateSuccessResult(jobData);
		}
		catch (Exception ex)
		{
			return new OperateResult<JobData>("MID0033 prase failed: " + ex.Message + Environment.NewLine + "Source: " + reply);
		}
	}
}
