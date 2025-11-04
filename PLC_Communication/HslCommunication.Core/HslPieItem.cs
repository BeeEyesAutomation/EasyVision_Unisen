using System.Drawing;

namespace HslCommunication.Core;

public class HslPieItem
{
	public string Name { get; set; }

	public int Value { get; set; }

	public Color Back { get; set; }

	public HslPieItem()
	{
		Back = Color.DodgerBlue;
	}
}
