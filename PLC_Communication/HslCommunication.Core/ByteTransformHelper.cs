using System;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core;

public static class ByteTransformHelper
{
	public static OperateResult<TResult> GetResultFromBytes<TResult>(OperateResult<byte[]> result, Func<byte[], TResult> translator)
	{
		try
		{
			if (result.IsSuccess)
			{
				return OperateResult.CreateSuccessResult(translator(result.Content));
			}
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		catch (Exception ex)
		{
			OperateResult<TResult> operateResult = new OperateResult<TResult>();
			operateResult.Message = $"{StringResources.Language.DataTransformError} {SoftBasic.ByteToHexString(result.Content)} : Length({result.Content.Length}) {ex.Message}";
			return operateResult;
		}
	}

	public static OperateResult<TResult> GetResultFromArray<TResult>(OperateResult<TResult[]> result)
	{
		return GetSuccessResultFromOther(result, (TResult[] m) => m[0]);
	}

	public static OperateResult<TResult> GetSuccessResultFromOther<TResult, TIn>(OperateResult<TIn> result, Func<TIn, TResult> trans)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		try
		{
			TResult value = trans(result.Content);
			return OperateResult.CreateSuccessResult(value);
		}
		catch (Exception ex)
		{
			return new OperateResult<TResult>(StringResources.Language.DataTransformError + " " + ex.Message);
		}
	}

	public static OperateResult GetResultFromOther<TIn>(OperateResult<TIn> result, Func<TIn, OperateResult> trans)
	{
		if (!result.IsSuccess)
		{
			return result;
		}
		return trans(result.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn>(OperateResult<TIn> result, Func<TIn, OperateResult<TResult>> trans)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		return trans(result.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TResult>> trans2)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		return trans2(operateResult.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TResult>> trans3)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		return trans3(operateResult2.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3, TIn4>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TIn4>> trans3, Func<TIn4, OperateResult<TResult>> trans4)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		OperateResult<TIn4> operateResult3 = trans3(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult3);
		}
		return trans4(operateResult3.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3, TIn4, TIn5>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TIn4>> trans3, Func<TIn4, OperateResult<TIn5>> trans4, Func<TIn5, OperateResult<TResult>> trans5)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		OperateResult<TIn4> operateResult3 = trans3(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult3);
		}
		OperateResult<TIn5> operateResult4 = trans4(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult4);
		}
		return trans5(operateResult4.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TIn4>> trans3, Func<TIn4, OperateResult<TIn5>> trans4, Func<TIn5, OperateResult<TIn6>> trans5, Func<TIn6, OperateResult<TResult>> trans6)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		OperateResult<TIn4> operateResult3 = trans3(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult3);
		}
		OperateResult<TIn5> operateResult4 = trans4(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult4);
		}
		OperateResult<TIn6> operateResult5 = trans5(operateResult4.Content);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult5);
		}
		return trans6(operateResult5.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TIn4>> trans3, Func<TIn4, OperateResult<TIn5>> trans4, Func<TIn5, OperateResult<TIn6>> trans5, Func<TIn6, OperateResult<TIn7>> trans6, Func<TIn7, OperateResult<TResult>> trans7)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		OperateResult<TIn4> operateResult3 = trans3(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult3);
		}
		OperateResult<TIn5> operateResult4 = trans4(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult4);
		}
		OperateResult<TIn6> operateResult5 = trans5(operateResult4.Content);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult5);
		}
		OperateResult<TIn7> operateResult6 = trans6(operateResult5.Content);
		if (!operateResult6.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult6);
		}
		return trans7(operateResult6.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TIn4>> trans3, Func<TIn4, OperateResult<TIn5>> trans4, Func<TIn5, OperateResult<TIn6>> trans5, Func<TIn6, OperateResult<TIn7>> trans6, Func<TIn7, OperateResult<TIn8>> trans7, Func<TIn8, OperateResult<TResult>> trans8)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		OperateResult<TIn4> operateResult3 = trans3(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult3);
		}
		OperateResult<TIn5> operateResult4 = trans4(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult4);
		}
		OperateResult<TIn6> operateResult5 = trans5(operateResult4.Content);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult5);
		}
		OperateResult<TIn7> operateResult6 = trans6(operateResult5.Content);
		if (!operateResult6.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult6);
		}
		OperateResult<TIn8> operateResult7 = trans7(operateResult6.Content);
		if (!operateResult7.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult7);
		}
		return trans8(operateResult7.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TIn4>> trans3, Func<TIn4, OperateResult<TIn5>> trans4, Func<TIn5, OperateResult<TIn6>> trans5, Func<TIn6, OperateResult<TIn7>> trans6, Func<TIn7, OperateResult<TIn8>> trans7, Func<TIn8, OperateResult<TIn9>> trans8, Func<TIn9, OperateResult<TResult>> trans9)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		OperateResult<TIn4> operateResult3 = trans3(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult3);
		}
		OperateResult<TIn5> operateResult4 = trans4(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult4);
		}
		OperateResult<TIn6> operateResult5 = trans5(operateResult4.Content);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult5);
		}
		OperateResult<TIn7> operateResult6 = trans6(operateResult5.Content);
		if (!operateResult6.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult6);
		}
		OperateResult<TIn8> operateResult7 = trans7(operateResult6.Content);
		if (!operateResult7.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult7);
		}
		OperateResult<TIn9> operateResult8 = trans8(operateResult7.Content);
		if (!operateResult8.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult8);
		}
		return trans9(operateResult8.Content);
	}

	public static OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10>(OperateResult<TIn1> result, Func<TIn1, OperateResult<TIn2>> trans1, Func<TIn2, OperateResult<TIn3>> trans2, Func<TIn3, OperateResult<TIn4>> trans3, Func<TIn4, OperateResult<TIn5>> trans4, Func<TIn5, OperateResult<TIn6>> trans5, Func<TIn6, OperateResult<TIn7>> trans6, Func<TIn7, OperateResult<TIn8>> trans7, Func<TIn8, OperateResult<TIn9>> trans8, Func<TIn9, OperateResult<TIn10>> trans9, Func<TIn10, OperateResult<TResult>> trans10)
	{
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(result);
		}
		OperateResult<TIn2> operateResult = trans1(result.Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult);
		}
		OperateResult<TIn3> operateResult2 = trans2(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult2);
		}
		OperateResult<TIn4> operateResult3 = trans3(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult3);
		}
		OperateResult<TIn5> operateResult4 = trans4(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult4);
		}
		OperateResult<TIn6> operateResult5 = trans5(operateResult4.Content);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult5);
		}
		OperateResult<TIn7> operateResult6 = trans6(operateResult5.Content);
		if (!operateResult6.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult6);
		}
		OperateResult<TIn8> operateResult7 = trans7(operateResult6.Content);
		if (!operateResult7.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult7);
		}
		OperateResult<TIn9> operateResult8 = trans8(operateResult7.Content);
		if (!operateResult8.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult8);
		}
		OperateResult<TIn10> operateResult9 = trans9(operateResult8.Content);
		if (!result.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TResult>(operateResult9);
		}
		return trans10(operateResult9.Content);
	}
}
