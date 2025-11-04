using System;

namespace HslCommunication.Profinet.AllenBradley;

public class AbStructHandle
{
	public ushort ReturnCount { get; set; }

	public uint TemplateObjectDefinitionSize { get; set; }

	public uint TemplateStructureSize { get; set; }

	public ushort MemberCount { get; set; }

	public ushort StructureHandle { get; set; }

	public AbStructHandle()
	{
	}

	public AbStructHandle(byte[] source, int index)
	{
		ReturnCount = BitConverter.ToUInt16(source, index);
		TemplateObjectDefinitionSize = BitConverter.ToUInt32(source, index + 6);
		TemplateStructureSize = BitConverter.ToUInt32(source, index + 14);
		MemberCount = BitConverter.ToUInt16(source, index + 22);
		StructureHandle = BitConverter.ToUInt16(source, index + 28);
	}
}
