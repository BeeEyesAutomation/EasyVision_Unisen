using HslCommunication.Core;

namespace HslCommunication.Instrument.CJT.Helper;

public interface ICjt188 : IReadWriteDevice, IReadWriteNet
{
	byte InstrumentType { get; set; }

	string Station { get; set; }

	bool EnableCodeFE { get; set; }

	OperateResult ActiveDeveice();

	OperateResult WriteAddress(string address);

	OperateResult<string> ReadAddress();

	OperateResult<string[]> ReadStringArray(string address);
}
