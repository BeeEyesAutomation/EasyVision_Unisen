using System;

namespace HslCommunication.Secs.Types;

public class VariableName
{
	public long ID { get; set; }

	public string Name { get; set; }

	public string Units { get; set; }

	public override string ToString()
	{
		return Name;
	}

	public static implicit operator VariableName(SecsValue value)
	{
		TypeHelper.TypeListCheck(value);
		if (value.Value is SecsValue[] array)
		{
			VariableName variableName = new VariableName();
			variableName.ID = Convert.ToInt64(array[0].Value);
			variableName.Name = array[1].Value as string;
			variableName.Units = array[2].Value as string;
			return variableName;
		}
		return null;
	}

	public static implicit operator SecsValue(VariableName value)
	{
		return new SecsValue(new object[3] { value.ID, value.Name, value.Units });
	}
}
