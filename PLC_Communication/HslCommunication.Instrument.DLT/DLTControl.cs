namespace HslCommunication.Instrument.DLT;

public class DLTControl
{
	public const byte DLT2007_Retain = 0;

	public const byte DLT2007_Broadcast = 8;

	public const byte DLT2007_ReadData = 17;

	public const byte DLT2007_ReadFollowData = 18;

	public const byte DLT2007_ReadAddress = 19;

	public const byte DLT2007_WriteData = 20;

	public const byte DLT2007_WriteAddress = 21;

	public const byte DLT2007_FreezeCommand = 22;

	public const byte DLT2007_ChangeBaudRate = 23;

	public const byte DLT2007_ChangePassword = 24;

	public const byte DLT2007_ClearMaxQuantityDemanded = 25;

	public const byte DLT2007_ElectricityReset = 26;

	public const byte DLT2007_EventReset = 27;

	public const byte DLT2007_ClosingAlarmPowerpProtection = 28;

	public const byte DLT2007_MultiFunctionTerminalOutputControlCommand = 29;

	public const byte DLT2007_SecurityAuthenticationCommand = 3;

	public const byte DLT1997_Retain = 0;

	public const byte DLT1997_ReadData = 1;

	public const byte DLT1997_ReadFollowData = 2;

	public const byte DLT1997_ReadAgain = 3;

	public const byte DLT1997_WriteData = 4;

	public const byte DLT1997_Broadcast = 8;

	public const byte DLT1997_WriteAddress = 10;

	public const byte DLT1997_ChangeBaudRate = 12;

	public const byte DLT1997_ChangePassword = 15;

	public const byte DLT1997_ClearMaxQuantityDemanded = 16;
}
