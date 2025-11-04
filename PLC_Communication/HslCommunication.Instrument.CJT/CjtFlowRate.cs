using System;

namespace HslCommunication.Instrument.CJT;

public class CjtFlowRate
{
	public double CurrentFlowRate { get; set; }

	public string CurrentUnit { get; set; }

	public double SettlementDateFlowRate { get; set; }

	public string SettlementDateUnit { get; set; }

	public DateTime DateTime { get; set; }
}
