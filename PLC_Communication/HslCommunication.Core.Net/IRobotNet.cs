using System.Threading.Tasks;
using HslCommunication.LogNet;

namespace HslCommunication.Core.Net;

public interface IRobotNet
{
	ILogNet LogNet { get; set; }

	OperateResult<byte[]> Read(string address);

	OperateResult<string> ReadString(string address);

	OperateResult Write(string address, byte[] value);

	OperateResult Write(string address, string value);

	Task<OperateResult<byte[]>> ReadAsync(string address);

	Task<OperateResult<string>> ReadStringAsync(string address);

	Task<OperateResult> WriteAsync(string address, byte[] value);

	Task<OperateResult> WriteAsync(string address, string value);
}
