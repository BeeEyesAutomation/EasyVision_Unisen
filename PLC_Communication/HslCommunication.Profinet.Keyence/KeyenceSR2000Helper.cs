using System;
using System.Linq;
using System.Text;

namespace HslCommunication.Profinet.Keyence;

internal class KeyenceSR2000Helper
{
	public static OperateResult<string> ReadBarcode(Func<byte[], OperateResult<byte[]>> readCore)
	{
		OperateResult<string> operateResult = ReadCustomer("LON", readCore);
		if (!operateResult.IsSuccess && operateResult.ErrorCode < 0)
		{
			ReadCustomer("LOFF", readCore);
		}
		return operateResult;
	}

	public static OperateResult Reset(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("RESET", readCore);
	}

	public static OperateResult OpenIndicator(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("AMON", readCore);
	}

	public static OperateResult CloseIndicator(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("AMOFF", readCore);
	}

	public static OperateResult<string> ReadVersion(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("KEYENCE", readCore);
	}

	public static OperateResult<string> ReadCommandState(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("CMDSTAT", readCore);
	}

	public static OperateResult<string> ReadErrorState(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("ERRSTAT", readCore);
	}

	public static OperateResult<bool> CheckInput(int number, Func<byte[], OperateResult<byte[]>> readCore)
	{
		OperateResult<string> operateResult = ReadCustomer("INCHK," + number, readCore);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult);
		}
		if (operateResult.Content == "ON")
		{
			return OperateResult.CreateSuccessResult(value: true);
		}
		if (operateResult.Content == "OFF")
		{
			return OperateResult.CreateSuccessResult(value: false);
		}
		return new OperateResult<bool>(operateResult.Content);
	}

	public static OperateResult SetOutput(int number, bool value, Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer((value ? "OUTON," : "OUTOFF,") + number, readCore);
	}

	public static OperateResult<int[]> ReadRecord(Func<byte[], OperateResult<byte[]>> readCore)
	{
		OperateResult<string> operateResult = ReadCustomer("NUM", readCore);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult((from n in operateResult.Content.Split(',')
			select int.Parse(n)).ToArray());
	}

	public static OperateResult Lock(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("LOCK", readCore);
	}

	public static OperateResult UnLock(Func<byte[], OperateResult<byte[]>> readCore)
	{
		return ReadCustomer("UNLOCK", readCore);
	}

	public static OperateResult<string> ReadCustomer(string command, Func<byte[], OperateResult<byte[]>> readCore)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(command + "\r");
		string text = command;
		if (command.IndexOf(',') > 0)
		{
			text = command.Substring(0, command.IndexOf(','));
		}
		OperateResult<byte[]> operateResult = readCore(bytes);
		if (!operateResult.IsSuccess)
		{
			return operateResult.Convert<string>(null);
		}
		string text2 = Encoding.ASCII.GetString(operateResult.Content).Trim('\r');
		if (text2.StartsWith("ER," + text + ","))
		{
			return new OperateResult<string>(GetErrorDescription(text2.Substring(4 + text.Length)));
		}
		if (text2.StartsWith("OK," + text) && text2.Length > 4 + text.Length)
		{
			return OperateResult.CreateSuccessResult(text2.Substring(4 + text.Length));
		}
		return OperateResult.CreateSuccessResult(text2);
	}

	public static string GetErrorDescription(string error)
	{
		if (1 == 0)
		{
		}
		string result = error switch
		{
			"00" => StringResources.Language.KeyenceSR2000Error00, 
			"01" => StringResources.Language.KeyenceSR2000Error01, 
			"02" => StringResources.Language.KeyenceSR2000Error02, 
			"03" => StringResources.Language.KeyenceSR2000Error03, 
			"04" => StringResources.Language.KeyenceSR2000Error04, 
			"05" => StringResources.Language.KeyenceSR2000Error05, 
			"10" => StringResources.Language.KeyenceSR2000Error10, 
			"11" => StringResources.Language.KeyenceSR2000Error11, 
			"12" => StringResources.Language.KeyenceSR2000Error12, 
			"13" => StringResources.Language.KeyenceSR2000Error13, 
			"14" => StringResources.Language.KeyenceSR2000Error14, 
			"20" => StringResources.Language.KeyenceSR2000Error20, 
			"21" => StringResources.Language.KeyenceSR2000Error21, 
			"22" => StringResources.Language.KeyenceSR2000Error22, 
			"23" => StringResources.Language.KeyenceSR2000Error23, 
			"99" => StringResources.Language.KeyenceSR2000Error99, 
			_ => StringResources.Language.UnknownError + " :" + error, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
