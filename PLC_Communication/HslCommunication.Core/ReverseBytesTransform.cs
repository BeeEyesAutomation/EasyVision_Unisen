namespace HslCommunication.Core;

public class ReverseBytesTransform : RegularByteTransform
{
	public ReverseBytesTransform()
	{
		base.DataFormat = DataFormat.ABCD;
	}

	public ReverseBytesTransform(DataFormat dataFormat)
		: base(dataFormat)
	{
	}

	public override IByteTransform CreateByDateFormat(DataFormat dataFormat)
	{
		return new ReverseBytesTransform(dataFormat)
		{
			IsStringReverseByteWord = base.IsStringReverseByteWord
		};
	}

	public override string ToString()
	{
		return $"ReverseBytesTransform[{base.DataFormat}]";
	}
}
