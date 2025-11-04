using System;

namespace HslCommunication.LogNet;

public class HslEventArgs : EventArgs
{
	public HslMessageItem HslMessage { get; set; }
}
