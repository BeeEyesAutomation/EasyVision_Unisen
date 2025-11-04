using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HslCommunication.BasicFramework;

[Serializable]
public sealed class Exception<TExceptionArgs> : Exception, ISerializable where TExceptionArgs : ExceptionArgs
{
	private const string c_args = "Args";

	private readonly TExceptionArgs m_args;

	public TExceptionArgs Args => m_args;

	public override string Message
	{
		get
		{
			string message = base.Message;
			return (m_args == null) ? message : (message + " (" + m_args.Message + ")");
		}
	}

	public Exception(string message = null, Exception innerException = null)
		: this((TExceptionArgs)null, message, innerException)
	{
	}

	public Exception(TExceptionArgs args, string message = null, Exception innerException = null)
		: base(message, innerException)
	{
		m_args = args;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	private Exception(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		m_args = (TExceptionArgs)info.GetValue("Args", typeof(TExceptionArgs));
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("Args", m_args);
		base.GetObjectData(info, context);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Exception<TExceptionArgs> ex))
		{
			return false;
		}
		return object.Equals(m_args, ex.m_args) && base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
