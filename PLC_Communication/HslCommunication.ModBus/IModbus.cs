using System;
using HslCommunication.Core;

namespace HslCommunication.ModBus;

public interface IModbus : IReadWriteDevice, IReadWriteNet
{
	bool AddressStartWithZero { get; set; }

	byte Station { get; set; }

	DataFormat DataFormat { get; set; }

	bool IsStringReverse { get; set; }

	int BroadcastStation { get; set; }

	bool EnableWriteMaskCode { get; set; }

	int WordReadBatchLength { get; set; }

	OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode);

	void RegisteredAddressMapping(Func<string, byte, OperateResult<string>> mapping);

	OperateResult<byte[]> ReadWrite(string readAddress, ushort length, string writeAddress, byte[] value);

	OperateResult<byte[]> ReadFile(ushort fileNumber, ushort address, ushort length);

	OperateResult WriteFile(ushort fileNumber, ushort address, byte[] data);
}
