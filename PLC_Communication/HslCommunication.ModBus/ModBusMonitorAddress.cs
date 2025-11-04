using System;

namespace HslCommunication.ModBus;

public class ModBusMonitorAddress
{
	public ushort Address { get; set; }

	public event Action<ModBusMonitorAddress, short> OnWrite;

	public event Action<ModBusMonitorAddress, short, short> OnChange;

	public void SetValue(short value)
	{
		this.OnWrite?.Invoke(this, value);
	}

	public void SetChangeValue(short before, short after)
	{
		if (before != after)
		{
			this.OnChange?.Invoke(this, before, after);
		}
	}
}
