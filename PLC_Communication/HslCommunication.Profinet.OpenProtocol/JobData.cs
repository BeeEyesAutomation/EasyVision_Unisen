using System.Collections.Generic;

namespace HslCommunication.Profinet.OpenProtocol;

public class JobData
{
	public int JobID { get; set; }

	public string JobName { get; set; }

	public int ForcedOrder { get; set; }

	public int MaxTimeForFirstTightening { get; set; }

	public int MaxTimeToCompleteJob { get; set; }

	public int JobBatchMode { get; set; }

	public bool LockAtJobDone { get; set; }

	public bool UseLineControl { get; set; }

	public bool RepeatJob { get; set; }

	public int ToolLoosening { get; set; }

	public int Reserved { get; set; }

	public List<JobItem> JobList { get; set; }
}
