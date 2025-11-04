using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HslCommunication.Profinet.AllenBradley;

public class AbTagItem
{
	public uint InstanceID { get; set; }

	public string Name { get; set; }

	public ushort SymbolType { get; set; }

	public int ArrayDimension { get; set; }

	public bool IsStruct { get; set; }

	public int[] ArrayLength { get; set; }

	[JsonIgnore]
	public AbTagItem[] Members { get; set; }

	[JsonIgnore]
	public object Tag { get; set; }

	public int ByteOffset { get; set; }

	public AbTagItem()
	{
		ArrayLength = new int[3] { -1, -1, -1 };
	}

	public string GetTypeText()
	{
		string text = string.Empty;
		if (ArrayDimension == 1)
		{
			text = ((ArrayLength[0] >= 0) ? $"[{ArrayLength[0]}]" : "[]");
		}
		else if (ArrayDimension == 2)
		{
			text = $"[{ArrayLength[0]},{ArrayLength[1]}]";
		}
		else if (ArrayDimension == 3)
		{
			text = $"[{ArrayLength[0]},{ArrayLength[1]},{ArrayLength[2]}]";
		}
		if (IsStruct)
		{
			return "struct" + text;
		}
		if (SymbolType == 8)
		{
			return "date" + text;
		}
		if (SymbolType == 9)
		{
			return "time" + text;
		}
		if (SymbolType == 10)
		{
			return "timeAndDate" + text;
		}
		if (SymbolType == 11)
		{
			return "timeOfDate" + text;
		}
		if (SymbolType == 193)
		{
			return "bool" + text;
		}
		if (SymbolType == 194)
		{
			return "sbyte" + text;
		}
		if (SymbolType == 195)
		{
			return "short" + text;
		}
		if (SymbolType == 196)
		{
			return "int" + text;
		}
		if (SymbolType == 197)
		{
			return "long" + text;
		}
		if (SymbolType == 198)
		{
			return "byte" + text;
		}
		if (SymbolType == 199)
		{
			return "ushort" + text;
		}
		if (SymbolType == 200)
		{
			return "uint" + text;
		}
		if (SymbolType == 201)
		{
			return "ulong" + text;
		}
		if (SymbolType == 202)
		{
			return "float" + text;
		}
		if (SymbolType == 203)
		{
			return "double" + text;
		}
		if (SymbolType == 204)
		{
			return "struct";
		}
		if (SymbolType == 208)
		{
			return "string";
		}
		if (SymbolType == 209)
		{
			return "byte-str";
		}
		if (SymbolType == 210)
		{
			return "word-str";
		}
		if (SymbolType == 211)
		{
			if (ArrayDimension == 0)
			{
				return "bool[32]";
			}
			if (ArrayDimension == 1)
			{
				return "bool" + $"[{ArrayLength[0] * 32}]";
			}
			return "bool-str" + text;
		}
		if ((SymbolType | 0xF00) == 4033)
		{
			return "bool";
		}
		return "";
	}

	public override string ToString()
	{
		return Name;
	}

	private void SetSymbolType(ushort value)
	{
		ArrayDimension = (((value & 0x4000) == 16384) ? 2 : (((value & 0x2000) == 8192) ? 1 : 0));
		IsStruct = (value & 0x8000) == 32768;
		SymbolType = (ushort)(value & 0xFFF);
	}

	public static AbTagItem CloneBy(AbTagItem abTagItem)
	{
		if (abTagItem == null)
		{
			return null;
		}
		AbTagItem abTagItem2 = new AbTagItem();
		abTagItem2.InstanceID = abTagItem.InstanceID;
		abTagItem2.Name = abTagItem.Name;
		abTagItem2.ByteOffset = abTagItem.ByteOffset;
		abTagItem2.SymbolType = abTagItem.SymbolType;
		abTagItem2.ArrayDimension = abTagItem.ArrayDimension;
		abTagItem2.ArrayLength[0] = abTagItem.ArrayLength[0];
		abTagItem2.ArrayLength[1] = abTagItem.ArrayLength[1];
		abTagItem2.ArrayLength[2] = abTagItem.ArrayLength[2];
		abTagItem2.IsStruct = abTagItem.IsStruct;
		return abTagItem2;
	}

	public static AbTagItem[] CloneBy(AbTagItem[] abTagItems)
	{
		AbTagItem[] array = new AbTagItem[abTagItems.Length];
		for (int i = 0; i < abTagItems.Length; i++)
		{
			array[i] = CloneBy(abTagItems[i]);
		}
		return array;
	}

	public static AbTagItem PraseAbTagItem(byte[] source, ref int index)
	{
		AbTagItem abTagItem = new AbTagItem();
		abTagItem.InstanceID = BitConverter.ToUInt32(source, index);
		index += 4;
		ushort num = BitConverter.ToUInt16(source, index);
		index += 2;
		abTagItem.Name = Encoding.ASCII.GetString(source, index, num);
		index += num;
		abTagItem.SetSymbolType(BitConverter.ToUInt16(source, index));
		index += 2;
		abTagItem.ArrayLength[0] = BitConverter.ToInt32(source, index);
		index += 4;
		abTagItem.ArrayLength[1] = BitConverter.ToInt32(source, index);
		index += 4;
		abTagItem.ArrayLength[2] = BitConverter.ToInt32(source, index);
		index += 4;
		return abTagItem;
	}

	public static List<AbTagItem> PraseAbTagItems(byte[] source, int index, bool isGlobalVariable, out uint instance)
	{
		List<AbTagItem> list = new List<AbTagItem>();
		instance = 0u;
		while (index < source.Length)
		{
			AbTagItem abTagItem = PraseAbTagItem(source, ref index);
			instance = abTagItem.InstanceID;
			if ((abTagItem.SymbolType & 0x1000) != 4096 && !abTagItem.Name.StartsWith("__") && !abTagItem.Name.Contains(":"))
			{
				if (!isGlobalVariable)
				{
					abTagItem.Name = "Program:MainProgram." + abTagItem.Name;
				}
				list.Add(abTagItem);
			}
		}
		return list;
	}

	private static int CalculatesSpecifiedCharacterLength(byte[] source, int index, byte value)
	{
		for (int i = index; i < source.Length; i++)
		{
			if (source[i] == value)
			{
				return i - index;
			}
		}
		return -1;
	}

	private static string CalculatesString(byte[] source, ref int index, byte value)
	{
		if (index >= source.Length)
		{
			return string.Empty;
		}
		int num = CalculatesSpecifiedCharacterLength(source, index, value);
		if (num < 0)
		{
			index = source.Length;
			return string.Empty;
		}
		string result = Encoding.ASCII.GetString(source, index, num);
		index += num + 1;
		return result;
	}

	public static List<AbTagItem> PraseAbTagItemsFromStruct(byte[] source, int index, AbStructHandle structHandle)
	{
		List<AbTagItem> list = new List<AbTagItem>();
		int index2 = structHandle.MemberCount * 8 + index;
		string text = CalculatesString(source, ref index2, 0);
		for (int i = 0; i < structHandle.MemberCount; i++)
		{
			AbTagItem abTagItem = new AbTagItem();
			abTagItem.ArrayLength[0] = BitConverter.ToUInt16(source, 8 * i + index);
			abTagItem.SetSymbolType(BitConverter.ToUInt16(source, 8 * i + index + 2));
			abTagItem.ByteOffset = BitConverter.ToInt32(source, 8 * i + index + 4) + 2;
			abTagItem.Name = CalculatesString(source, ref index2, 0);
			list.Add(abTagItem);
		}
		return list;
	}
}
