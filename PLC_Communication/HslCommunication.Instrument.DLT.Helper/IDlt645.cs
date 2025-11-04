using System;
using HslCommunication.Core;

namespace HslCommunication.Instrument.DLT.Helper;

public interface IDlt645 : IReadWriteDevice, IReadWriteNet
{
	string Station { get; set; }

	bool EnableCodeFE { get; set; }

	DLT645Type DLTType { get; }

	string Password { get; set; }

	string OpCode { get; set; }

	bool CheckDataId { get; set; }

	OperateResult<byte[]> ReadFromCoreServer(byte[] send, bool hasResponseData, bool usePackAndUnpack = true);

	OperateResult ActiveDeveice();

	OperateResult BroadcastTime(DateTime dateTime);

	OperateResult WriteAddress(string address);

	OperateResult<string> ReadAddress();

	OperateResult<string[]> ReadStringArray(string address);

	OperateResult Trip(DateTime validTime);

	OperateResult Trip(string station, DateTime validTime);

	OperateResult SwitchingOn(DateTime validTime);

	OperateResult SwitchingOn(string station, DateTime validTime);
}
