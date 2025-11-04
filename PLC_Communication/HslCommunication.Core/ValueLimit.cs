namespace HslCommunication.Core;

public struct ValueLimit
{
	public double MaxValue { get; set; }

	public double MinValue { get; set; }

	public double Average { get; set; }

	public double StartValue { get; set; }

	public double Current { get; set; }

	public int Count { get; set; }

	public ValueLimit SetNewValue(double value)
	{
		if (!double.IsNaN(value))
		{
			if (Count == 0)
			{
				MaxValue = value;
				MinValue = value;
				Count = 1;
				Current = value;
				Average = value;
				StartValue = value;
			}
			else
			{
				if (value < MinValue)
				{
					MinValue = value;
				}
				if (value > MaxValue)
				{
					MaxValue = value;
				}
				Current = value;
				Average = ((double)Count * Average + value) / (double)(Count + 1);
				Count++;
			}
		}
		return this;
	}

	public override string ToString()
	{
		return $"Avg[{Current}]";
	}

	public static bool operator ==(ValueLimit value1, ValueLimit value2)
	{
		if (value1.Count != value2.Count)
		{
			return false;
		}
		if (value1.MaxValue != value2.MaxValue)
		{
			return false;
		}
		if (value1.MinValue != value2.MinValue)
		{
			return false;
		}
		if (value1.Current != value2.Current)
		{
			return false;
		}
		if (value1.Average != value2.Average)
		{
			return false;
		}
		if (value1.StartValue != value2.StartValue)
		{
			return false;
		}
		return true;
	}

	public static bool operator !=(ValueLimit value1, ValueLimit value2)
	{
		return !(value1 == value2);
	}

	public override bool Equals(object obj)
	{
		if (obj is ValueLimit valueLimit)
		{
			return this == valueLimit;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
