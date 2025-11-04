using System;
using System.Text;

namespace HslCommunication.Profinet.IDCard;

public class IdentityCard
{
	public string Name { get; set; }

	public string Sex { get; set; }

	public string Id { get; set; }

	public string Nation { get; set; }

	public DateTime Birthday { get; set; }

	public string Address { get; set; }

	public string Organ { get; set; }

	public DateTime ValidityStartDate { get; set; }

	public DateTime ValidityEndDate { get; set; }

	public byte[] Portrait { get; set; }

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("姓名：" + Name);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append("性别：" + Sex);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append("民族：" + Nation);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append("身份证号：" + Id);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append($"出身日期：{Birthday.Year}年{Birthday.Month}月{Birthday.Day}日");
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append("地址：" + Address);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append("发证机关：" + Organ);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append($"有效日期：{ValidityStartDate.Year}年{ValidityStartDate.Month}月{ValidityStartDate.Day}日 - {ValidityEndDate.Year}年{ValidityEndDate.Month}月{ValidityEndDate.Day}日");
		stringBuilder.Append(Environment.NewLine);
		return stringBuilder.ToString();
	}
}
