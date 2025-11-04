using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Core;

namespace HslCommunication.Instrument.RKC.Helper;

public class TemperatureControllerHelper
{
	public static OperateResult<byte[]> BuildReadCommand(byte station, string address)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		if (station >= 100)
		{
			return new OperateResult<byte[]>("Station must less than 100");
		}
		try
		{
			byte[] array = new byte[4 + address.Length];
			array[0] = 4;
			Encoding.ASCII.GetBytes(station.ToString("D2")).CopyTo(array, 1);
			Encoding.ASCII.GetBytes(address).CopyTo(array, 3);
			array[array.Length - 1] = 5;
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteCommand(byte station, string address, double value)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		if (station >= 100)
		{
			return new OperateResult<byte[]>("Station must less than 100");
		}
		if (value.ToString().Length > 6)
		{
			return new OperateResult<byte[]>("The data consists of up to 6 characters");
		}
		try
		{
			List<byte> list = new List<byte>(20);
			list.Add(4);
			list.AddRange(Encoding.ASCII.GetBytes(station.ToString("D2")));
			list.Add(2);
			list.AddRange(Encoding.ASCII.GetBytes(address));
			list.AddRange(Encoding.ASCII.GetBytes(value.ToString()));
			list.Add(3);
			int num = list[4];
			for (int i = 5; i < list.Count; i++)
			{
				num ^= list[i];
			}
			list.Add((byte)num);
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<double> ReadDouble(IReadWriteDevice device, byte station, string address)
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(station, address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = device.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(operateResult2);
		}
		if (operateResult2.Content[0] != 2)
		{
			return new OperateResult<double>("STX check failed: " + operateResult2.Content.ToHexString(' '));
		}
		try
		{
			return OperateResult.CreateSuccessResult(double.Parse(Encoding.ASCII.GetString(operateResult2.Content, 3, operateResult2.Content.Length - 5)));
		}
		catch (Exception ex)
		{
			return new OperateResult<double>(ex.Message + Environment.NewLine + "Source: " + operateResult2.Content.ToHexString(' '));
		}
	}

	public static OperateResult Write(IReadWriteDevice device, byte station, string address, double value)
	{
		OperateResult<byte[]> operateResult = BuildWriteCommand(station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = device.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult2.Content[0] != 6)
		{
			return new OperateResult<double>("ACK check failed: " + operateResult2.Content.ToHexString(' '));
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<double>> ReadDoubleAsync(IReadWriteDevice device, byte station, string address)
	{
		OperateResult<byte[]> build = BuildReadCommand(station, address);
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(build);
		}
		OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(read);
		}
		if (read.Content[0] != 2)
		{
			return new OperateResult<double>("STX check failed: " + read.Content.ToHexString(' '));
		}
		try
		{
			return OperateResult.CreateSuccessResult(double.Parse(Encoding.ASCII.GetString(read.Content, 3, read.Content.Length - 5)));
		}
		catch (Exception ex)
		{
			return new OperateResult<double>(ex.Message + Environment.NewLine + "Source: " + read.Content.ToHexString(' '));
		}
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice device, byte station, string address, double value)
	{
		OperateResult<byte[]> build = BuildWriteCommand(station, address, value);
		if (!build.IsSuccess)
		{
			return build;
		}
		OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content[0] != 6)
		{
			return new OperateResult<double>("ACK check failed: " + read.Content.ToHexString(' '));
		}
		return OperateResult.CreateSuccessResult();
	}
}
