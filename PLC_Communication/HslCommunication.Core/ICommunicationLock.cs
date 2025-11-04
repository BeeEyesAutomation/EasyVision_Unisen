using System;

namespace HslCommunication.Core;

public interface ICommunicationLock : IDisposable
{
	OperateResult EnterLock(int timeout);

	void LeaveLock();
}
