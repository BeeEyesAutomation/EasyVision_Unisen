using System.Threading.Tasks;

namespace HslCommunication.Core.Net;

internal class StateObjectAsync<T> : StateObject
{
	public TaskCompletionSource<T> Tcs { get; set; }

	public StateObjectAsync()
	{
	}

	public StateObjectAsync(int length)
		: base(length)
	{
	}
}
