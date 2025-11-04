namespace HslCommunication.Core;

public class ReverseWordTransform : RegularByteTransform
{
	public ReverseWordTransform()
	{
		base.DataFormat = DataFormat.CDAB;
	}

	public ReverseWordTransform(DataFormat dataFormat)
		: base(dataFormat)
	{
	}

	public override IByteTransform CreateByDateFormat(DataFormat dataFormat)
	{
		return new ReverseWordTransform(dataFormat)
		{
			IsStringReverseByteWord = base.IsStringReverseByteWord
		};
	}

	public override string ToString()
	{
		return $"ReverseWordTransform[{base.DataFormat}]";
	}
}
