using System;
using System.Threading.Tasks;
using HslCommunication.Core;

namespace HslCommunication.Profinet.AllenBradley;

public interface IReadWriteCip : IReadWriteNet
{
	IByteTransform ByteTransform { get; set; }

	OperateResult WriteTag(string address, ushort typeCode, byte[] value, int length = 1);

	OperateResult<ushort, byte[]> ReadTag(string address, ushort length = 1);

	OperateResult<string> ReadPlcType();

	Task<OperateResult> WriteTagAsync(string address, ushort typeCode, byte[] value, int length = 1);

	OperateResult<DateTime> ReadDate(string address);

	OperateResult WriteDate(string address, DateTime date);

	OperateResult WriteTimeAndDate(string address, DateTime date);

	OperateResult<TimeSpan> ReadTime(string address);

	OperateResult WriteTime(string address, TimeSpan time);

	OperateResult WriteTimeOfDate(string address, TimeSpan timeOfDate);

	Task<OperateResult<DateTime>> ReadDateAsync(string address);

	Task<OperateResult> WriteDateAsync(string address, DateTime date);

	Task<OperateResult> WriteTimeAndDateAsync(string address, DateTime date);

	Task<OperateResult<TimeSpan>> ReadTimeAsync(string address);

	Task<OperateResult> WriteTimeAsync(string address, TimeSpan time);

	Task<OperateResult> WriteTimeOfDateAsync(string address, TimeSpan timeOfDate);
}
