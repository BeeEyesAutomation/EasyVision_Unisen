using System;
using System.Xml.Linq;

namespace HslCommunication.Profinet.AllenBradley;

public class AllenBradleyItemValue
{
	public string Name { get; set; }

	public byte[] Buffer { get; set; }

	public bool IsArray { get; set; }

	public int TypeLength { get; set; } = 1;

	public ushort TypeCode { get; set; } = 193;

	public AllenBradleyItemValue()
	{
	}

	public AllenBradleyItemValue(XElement element)
	{
		LoadByXml(element);
	}

	public XElement ToXml()
	{
		XElement xElement = new XElement("AllenBradleyItemValue");
		xElement.SetAttributeValue("Name", Name);
		xElement.SetAttributeValue("TypeCode", TypeCode);
		xElement.SetAttributeValue("IsArray", IsArray);
		xElement.SetAttributeValue("TypeLength", TypeLength);
		if (Buffer != null)
		{
			xElement.SetAttributeValue("Buffer", Buffer.ToHexString());
		}
		return xElement;
	}

	private T GetXmlValue<T>(XElement element, string name, T defaultValue, Func<string, T> trans)
	{
		XAttribute xAttribute = element.Attribute(name);
		if (xAttribute == null)
		{
			return defaultValue;
		}
		try
		{
			return trans(xAttribute.Value);
		}
		catch
		{
			return defaultValue;
		}
	}

	public void LoadByXml(XElement element)
	{
		if (element.Name == "AllenBradleyItemValue")
		{
			Name = GetXmlValue(element, "Name", Name, (string m) => m);
			TypeCode = GetXmlValue(element, "TypeCode", TypeCode, ushort.Parse);
			IsArray = GetXmlValue(element, "IsArray", IsArray, bool.Parse);
			TypeLength = GetXmlValue(element, "TypeLength", TypeLength, int.Parse);
			Buffer = GetXmlValue(element, "Buffer", "", (string m) => m).ToHexBytes();
		}
	}
}
