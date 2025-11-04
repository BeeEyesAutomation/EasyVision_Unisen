using System;
using System.Text;

namespace HslCommunication.BasicFramework;

public sealed class VersionInfo
{
	public DateTime ReleaseDate { get; set; } = DateTime.Now;

	public StringBuilder UpdateDetails { get; set; } = new StringBuilder();

	public SystemVersion VersionNum { get; set; } = new SystemVersion(1, 0, 0);

	public override string ToString()
	{
		return VersionNum.ToString();
	}
}
