using System;

namespace HslCommunication.Algorithms.ConnectPool;

public interface IConnector
{
	bool IsConnectUsing { get; set; }

	string GuidToken { get; set; }

	DateTime LastUseTime { get; set; }

	void Open();

	void Close();
}
