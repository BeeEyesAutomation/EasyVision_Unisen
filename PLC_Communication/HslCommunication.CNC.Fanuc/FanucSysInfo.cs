using System.Text;

namespace HslCommunication.CNC.Fanuc;

public class FanucSysInfo
{
	public string TypeCode { get; set; }

	public string CncType { get; set; }

	public string MtType { get; set; }

	public string Series { get; set; }

	public string Version { get; set; }

	public int Axes { get; set; }

	public FanucSysInfo()
	{
	}

	public FanucSysInfo(byte[] buffer)
	{
		TypeCode = Encoding.ASCII.GetString(buffer, 32, 2);
		switch (TypeCode)
		{
		case "15":
			CncType = "Series 15/15i";
			break;
		case "16":
			CncType = "Series 16/16i";
			break;
		case "18":
			CncType = "Series 18/18i";
			break;
		case "21":
			CncType = "Series 21/210i";
			break;
		case "30":
			CncType = "Series 30i";
			break;
		case "31":
			CncType = "Series 31i";
			break;
		case "32":
			CncType = "Series 32i";
			break;
		case " 0":
			CncType = "Series 0i";
			break;
		case "PD":
			CncType = "Power Mate i-D";
			break;
		case "PH":
			CncType = "Power Mate i-H";
			break;
		}
		CncType += "-";
		switch (Encoding.ASCII.GetString(buffer, 34, 2))
		{
		case " M":
			MtType = "Machining center";
			break;
		case " T":
			MtType = "Lathe";
			break;
		case "MM":
			MtType = "M series with 2 path control";
			break;
		case "TT":
			MtType = "T series with 2/3 path control";
			break;
		case "MT":
			MtType = "T series with compound machining function";
			break;
		case " P":
			MtType = "Punch press";
			break;
		case " L":
			MtType = "Laser";
			break;
		case " W":
			MtType = "Wire cut";
			break;
		}
		CncType += Encoding.ASCII.GetString(buffer, 34, 2).Trim();
		switch (buffer[28])
		{
		case 1:
			CncType += "A";
			break;
		case 2:
			CncType += "B";
			break;
		case 3:
			CncType += "C";
			break;
		case 4:
			CncType += "D";
			break;
		case 6:
			CncType += "F";
			break;
		}
		Series = Encoding.ASCII.GetString(buffer, 36, 4);
		Version = Encoding.ASCII.GetString(buffer, 40, 4);
		Axes = int.Parse(Encoding.ASCII.GetString(buffer, 44, 2));
	}
}
