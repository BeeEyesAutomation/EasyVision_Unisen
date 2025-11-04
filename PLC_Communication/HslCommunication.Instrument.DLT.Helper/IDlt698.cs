using System;
using HslCommunication.Core;

namespace HslCommunication.Instrument.DLT.Helper;

public interface IDlt698 : IReadWriteDevice, IReadWriteNet
{
	string Station { get; set; }

	bool EnableCodeFE { get; set; }

	bool UseSecurityResquest { get; set; }

	byte CA { get; set; }

	OperateResult<byte[]> ReadFromCoreServer(byte[] send, bool hasResponseData, bool usePackAndUnpack = true);

	OperateResult ActiveDeveice();

	OperateResult WriteAddress(string address);

	OperateResult<string> ReadAddress();

	OperateResult<string[]> ReadStringArray(string address);

	OperateResult<string[]> ReadStringArray(string[] address);

	OperateResult WriteDateTime(string address, DateTime time);
}
