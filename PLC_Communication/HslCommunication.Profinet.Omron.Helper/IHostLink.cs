using HslCommunication.Core;

namespace HslCommunication.Profinet.Omron.Helper;

public interface IHostLink : IReadWriteDevice, IReadWriteNet
{
	byte ICF { get; set; }

	byte DA2 { get; set; }

	byte SA2 { get; set; }

	byte SID { get; set; }

	byte ResponseWaitTime { get; set; }

	byte UnitNumber { get; set; }

	int ReadSplits { get; set; }

	OmronPlcType PlcType { get; set; }
}
