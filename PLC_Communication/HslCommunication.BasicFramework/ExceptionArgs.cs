using System;

namespace HslCommunication.BasicFramework;

[Serializable]
public abstract class ExceptionArgs
{
	public virtual string Message => string.Empty;
}
