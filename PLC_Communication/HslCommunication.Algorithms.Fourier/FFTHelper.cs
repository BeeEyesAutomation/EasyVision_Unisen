using System;
using System.Collections.Generic;
using System.Linq;

namespace HslCommunication.Algorithms.Fourier;

public class FFTHelper
{
	private static void bitrp(double[] xreal, double[] ximag, int n)
	{
		int num = 1;
		int num2 = 0;
		while (num < n)
		{
			num2++;
			num *= 2;
		}
		for (num = 0; num < n; num++)
		{
			int num3 = num;
			int num4 = 0;
			for (int i = 0; i < num2; i++)
			{
				num4 = num4 * 2 + num3 % 2;
				num3 /= 2;
			}
			if (num4 > num)
			{
				double num5 = xreal[num];
				xreal[num] = xreal[num4];
				xreal[num4] = num5;
				num5 = ximag[num];
				ximag[num] = ximag[num4];
				ximag[num4] = num5;
			}
		}
	}

	public static double[] FFT(double[] xreal)
	{
		return FFTValue(xreal, new double[xreal.Length]);
	}

	public static double[] FFTValue(double[] xreal, double[] ximag, bool isSqrtDouble = false)
	{
		int num;
		for (num = 2; num <= xreal.Length; num *= 2)
		{
		}
		num /= 2;
		double[] array = new double[num / 2];
		double[] array2 = new double[num / 2];
		bitrp(xreal, ximag, num);
		double num2 = Math.PI * -2.0 / (double)num;
		double num3 = Math.Cos(num2);
		double num4 = Math.Sin(num2);
		array[0] = 1.0;
		array2[0] = 0.0;
		for (int i = 1; i < num / 2; i++)
		{
			array[i] = array[i - 1] * num3 - array2[i - 1] * num4;
			array2[i] = array[i - 1] * num4 + array2[i - 1] * num3;
		}
		for (int num5 = 2; num5 <= num; num5 *= 2)
		{
			for (int j = 0; j < num; j += num5)
			{
				for (int k = 0; k < num5 / 2; k++)
				{
					int num6 = j + k;
					int num7 = num6 + num5 / 2;
					int num8 = num * k / num5;
					num3 = array[num8] * xreal[num7] - array2[num8] * ximag[num7];
					num4 = array[num8] * ximag[num7] + array2[num8] * xreal[num7];
					double num9 = xreal[num6];
					double num10 = ximag[num6];
					xreal[num6] = num9 + num3;
					ximag[num6] = num10 + num4;
					xreal[num7] = num9 - num3;
					ximag[num7] = num10 - num4;
				}
			}
		}
		double[] array3 = new double[num];
		for (int l = 0; l < array3.Length; l++)
		{
			array3[l] = Math.Sqrt(Math.Pow(xreal[l], 2.0) + Math.Pow(ximag[l], 2.0));
			if (isSqrtDouble)
			{
				array3[l] = Math.Sqrt(array3[l]);
			}
		}
		return array3;
	}

	public static int FFT(double[] xreal, double[] ximag)
	{
		return FFTValue(xreal, ximag).Length;
	}

	public static int FFT(float[] xreal, float[] ximag)
	{
		return FFT(((IEnumerable<float>)xreal).Select((Func<float, double>)((float m) => m)).ToArray(), ((IEnumerable<float>)ximag).Select((Func<float, double>)((float m) => m)).ToArray());
	}

	public static int IFFT(float[] xreal, float[] ximag)
	{
		return IFFT(((IEnumerable<float>)xreal).Select((Func<float, double>)((float m) => m)).ToArray(), ((IEnumerable<float>)ximag).Select((Func<float, double>)((float m) => m)).ToArray());
	}

	public static int IFFT(double[] xreal, double[] ximag)
	{
		int num;
		for (num = 2; num <= xreal.Length; num *= 2)
		{
		}
		num /= 2;
		double[] array = new double[num / 2];
		double[] array2 = new double[num / 2];
		bitrp(xreal, ximag, num);
		double num2 = Math.PI * 2.0 / (double)num;
		double num3 = Math.Cos(num2);
		double num4 = Math.Sin(num2);
		array[0] = 1.0;
		array2[0] = 0.0;
		for (int i = 1; i < num / 2; i++)
		{
			array[i] = array[i - 1] * num3 - array2[i - 1] * num4;
			array2[i] = array[i - 1] * num4 + array2[i - 1] * num3;
		}
		for (int num5 = 2; num5 <= num; num5 *= 2)
		{
			for (int j = 0; j < num; j += num5)
			{
				for (int k = 0; k < num5 / 2; k++)
				{
					int num6 = j + k;
					int num7 = num6 + num5 / 2;
					int num8 = num * k / num5;
					num3 = array[num8] * xreal[num7] - array2[num8] * ximag[num7];
					num4 = array[num8] * ximag[num7] + array2[num8] * xreal[num7];
					double num9 = xreal[num6];
					double num10 = ximag[num6];
					xreal[num6] = num9 + num3;
					ximag[num6] = num10 + num4;
					xreal[num7] = num9 - num3;
					ximag[num7] = num10 - num4;
				}
			}
		}
		for (int l = 0; l < num; l++)
		{
			xreal[l] /= num;
			ximag[l] /= num;
		}
		return num;
	}
}
