using System;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Omron.Helper;

public interface IOmronFins : IReadWriteDevice, IReadWriteNet
{
	byte ICF { get; set; }

	byte RSV { get; }

	byte GCT { get; set; }

	byte DNA { get; set; }

	byte DA1 { get; set; }

	byte DA2 { get; set; }

	byte SNA { get; set; }

	byte SA1 { get; set; }

	byte SA2 { get; set; }

	byte SID { get; set; }

	int ReadSplits { get; set; }

	OmronPlcType PlcType { get; set; }

	OperateResult Run();

	OperateResult Stop();

	OperateResult<OmronCpuUnitData> ReadCpuUnitData();

	OperateResult<OmronCpuUnitStatus> ReadCpuUnitStatus();

	OperateResult<DateTime> ReadCpuTime();
}
