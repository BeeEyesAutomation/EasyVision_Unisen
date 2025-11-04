using System;
using System.Text;

namespace HslCommunication.Robot.EFORT;

public class EfortData
{
	public string PacketStart { get; set; }

	public ushort PacketOrders { get; set; }

	public ushort PacketHeartbeat { get; set; }

	public byte ErrorStatus { get; set; }

	public byte HstopStatus { get; set; }

	public byte AuthorityStatus { get; set; }

	public byte ServoStatus { get; set; }

	public byte AxisMoveStatus { get; set; }

	public byte ProgMoveStatus { get; set; }

	public byte ProgLoadStatus { get; set; }

	public byte ProgHoldStatus { get; set; }

	public ushort ModeStatus { get; set; }

	public ushort SpeedStatus { get; set; }

	public byte[] IoDOut { get; set; }

	public byte[] IoDIn { get; set; }

	public int[] IoIOut { get; set; }

	public int[] IoIIn { get; set; }

	public string ProjectName { get; set; }

	public string ProgramName { get; set; }

	public string ErrorText { get; set; }

	public float[] DbAxisPos { get; set; }

	public float[] DbCartPos { get; set; }

	public float[] DbAxisSpeed { get; set; }

	public float[] DbAxisAcc { get; set; }

	public float[] DbAxisAccAcc { get; set; }

	public float[] DbAxisTorque { get; set; }

	public int[] DbAxisDirCnt { get; set; }

	public int[] DbAxisTime { get; set; }

	public int DbDeviceTime { get; set; }

	public string PacketEnd { get; set; }

	public EfortData()
	{
		IoDOut = new byte[32];
		IoDIn = new byte[32];
		IoIOut = new int[32];
		IoIIn = new int[32];
		DbAxisPos = new float[7];
		DbCartPos = new float[6];
		DbAxisSpeed = new float[7];
		DbAxisAcc = new float[7];
		DbAxisAccAcc = new float[7];
		DbAxisTorque = new float[7];
		DbAxisDirCnt = new int[7];
		DbAxisTime = new int[7];
	}

	public static OperateResult<EfortData> PraseFromPrevious(byte[] data)
	{
		if (data.Length < 784)
		{
			return new OperateResult<EfortData>(string.Format(StringResources.Language.DataLengthIsNotEnough, 784, data.Length));
		}
		EfortData efortData = new EfortData();
		efortData.PacketStart = Encoding.ASCII.GetString(data, 0, 15).Trim();
		efortData.PacketOrders = BitConverter.ToUInt16(data, 17);
		efortData.PacketHeartbeat = BitConverter.ToUInt16(data, 19);
		efortData.ErrorStatus = data[21];
		efortData.HstopStatus = data[22];
		efortData.AuthorityStatus = data[23];
		efortData.ServoStatus = data[24];
		efortData.AxisMoveStatus = data[25];
		efortData.ProgMoveStatus = data[26];
		efortData.ProgLoadStatus = data[27];
		efortData.ProgHoldStatus = data[28];
		efortData.ModeStatus = BitConverter.ToUInt16(data, 29);
		efortData.SpeedStatus = BitConverter.ToUInt16(data, 31);
		for (int i = 0; i < 32; i++)
		{
			efortData.IoDOut[i] = data[33 + i];
		}
		for (int j = 0; j < 32; j++)
		{
			efortData.IoDIn[j] = data[65 + j];
		}
		for (int k = 0; k < 32; k++)
		{
			efortData.IoIOut[k] = BitConverter.ToInt32(data, 97 + 4 * k);
		}
		for (int l = 0; l < 32; l++)
		{
			efortData.IoIIn[l] = BitConverter.ToInt32(data, 225 + 4 * l);
		}
		efortData.ProjectName = Encoding.ASCII.GetString(data, 353, 32).Trim(default(char));
		efortData.ProgramName = Encoding.ASCII.GetString(data, 385, 32).Trim(default(char));
		efortData.ErrorText = Encoding.ASCII.GetString(data, 417, 128).Trim(default(char));
		for (int m = 0; m < 7; m++)
		{
			efortData.DbAxisPos[m] = BitConverter.ToSingle(data, 545 + 4 * m);
		}
		for (int n = 0; n < 6; n++)
		{
			efortData.DbCartPos[n] = BitConverter.ToSingle(data, 573 + 4 * n);
		}
		for (int num = 0; num < 7; num++)
		{
			efortData.DbAxisSpeed[num] = BitConverter.ToSingle(data, 597 + 4 * num);
		}
		for (int num2 = 0; num2 < 7; num2++)
		{
			efortData.DbAxisAcc[num2] = BitConverter.ToSingle(data, 625 + 4 * num2);
		}
		for (int num3 = 0; num3 < 7; num3++)
		{
			efortData.DbAxisAccAcc[num3] = BitConverter.ToSingle(data, 653 + 4 * num3);
		}
		for (int num4 = 0; num4 < 7; num4++)
		{
			efortData.DbAxisTorque[num4] = BitConverter.ToSingle(data, 681 + 4 * num4);
		}
		for (int num5 = 0; num5 < 7; num5++)
		{
			efortData.DbAxisDirCnt[num5] = BitConverter.ToInt32(data, 709 + 4 * num5);
		}
		for (int num6 = 0; num6 < 7; num6++)
		{
			efortData.DbAxisTime[num6] = BitConverter.ToInt32(data, 737 + 4 * num6);
		}
		efortData.DbDeviceTime = BitConverter.ToInt32(data, 765);
		efortData.PacketEnd = Encoding.ASCII.GetString(data, 769, 15).Trim();
		return OperateResult.CreateSuccessResult(efortData);
	}

	public static OperateResult<EfortData> PraseFrom(byte[] data)
	{
		if (data.Length < 788)
		{
			return new OperateResult<EfortData>(string.Format(StringResources.Language.DataLengthIsNotEnough, 788, data.Length));
		}
		EfortData efortData = new EfortData();
		efortData.PacketStart = Encoding.ASCII.GetString(data, 0, 16).Trim();
		efortData.PacketOrders = BitConverter.ToUInt16(data, 18);
		efortData.PacketHeartbeat = BitConverter.ToUInt16(data, 20);
		efortData.ErrorStatus = data[22];
		efortData.HstopStatus = data[23];
		efortData.AuthorityStatus = data[24];
		efortData.ServoStatus = data[25];
		efortData.AxisMoveStatus = data[26];
		efortData.ProgMoveStatus = data[27];
		efortData.ProgLoadStatus = data[28];
		efortData.ProgHoldStatus = data[29];
		efortData.ModeStatus = BitConverter.ToUInt16(data, 30);
		efortData.SpeedStatus = BitConverter.ToUInt16(data, 32);
		for (int i = 0; i < 32; i++)
		{
			efortData.IoDOut[i] = data[34 + i];
		}
		for (int j = 0; j < 32; j++)
		{
			efortData.IoDIn[j] = data[66 + j];
		}
		for (int k = 0; k < 32; k++)
		{
			efortData.IoIOut[k] = BitConverter.ToInt32(data, 100 + 4 * k);
		}
		for (int l = 0; l < 32; l++)
		{
			efortData.IoIIn[l] = BitConverter.ToInt32(data, 228 + 4 * l);
		}
		efortData.ProjectName = Encoding.ASCII.GetString(data, 356, 32).Trim(default(char));
		efortData.ProgramName = Encoding.ASCII.GetString(data, 388, 32).Trim(default(char));
		efortData.ErrorText = Encoding.ASCII.GetString(data, 420, 128).Trim(default(char));
		for (int m = 0; m < 7; m++)
		{
			efortData.DbAxisPos[m] = BitConverter.ToSingle(data, 548 + 4 * m);
		}
		for (int n = 0; n < 6; n++)
		{
			efortData.DbCartPos[n] = BitConverter.ToSingle(data, 576 + 4 * n);
		}
		for (int num = 0; num < 7; num++)
		{
			efortData.DbAxisSpeed[num] = BitConverter.ToSingle(data, 600 + 4 * num);
		}
		for (int num2 = 0; num2 < 7; num2++)
		{
			efortData.DbAxisAcc[num2] = BitConverter.ToSingle(data, 628 + 4 * num2);
		}
		for (int num3 = 0; num3 < 7; num3++)
		{
			efortData.DbAxisAccAcc[num3] = BitConverter.ToSingle(data, 656 + 4 * num3);
		}
		for (int num4 = 0; num4 < 7; num4++)
		{
			efortData.DbAxisTorque[num4] = BitConverter.ToSingle(data, 684 + 4 * num4);
		}
		for (int num5 = 0; num5 < 7; num5++)
		{
			efortData.DbAxisDirCnt[num5] = BitConverter.ToInt32(data, 712 + 4 * num5);
		}
		for (int num6 = 0; num6 < 7; num6++)
		{
			efortData.DbAxisTime[num6] = BitConverter.ToInt32(data, 740 + 4 * num6);
		}
		efortData.DbDeviceTime = BitConverter.ToInt32(data, 768);
		efortData.PacketEnd = Encoding.ASCII.GetString(data, 772, 16).Trim();
		return OperateResult.CreateSuccessResult(efortData);
	}
}
