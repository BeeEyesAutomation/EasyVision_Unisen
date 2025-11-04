using System.Threading;

namespace HslCommunication.Core;

internal sealed class Singleton
{
	private static object m_lock = new object();

	private static Singleton SValue = null;

	public static Singleton GetSingleton()
	{
		if (SValue != null)
		{
			return SValue;
		}
		Monitor.Enter(m_lock);
		if (SValue == null)
		{
			Singleton value = new Singleton();
			Volatile.Write(ref SValue, value);
			SValue = new Singleton();
		}
		Monitor.Exit(m_lock);
		return SValue;
	}
}
