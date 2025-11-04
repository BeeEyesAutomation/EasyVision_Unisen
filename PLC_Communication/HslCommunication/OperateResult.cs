using System;

namespace HslCommunication;

public class OperateResult
{
	public bool IsSuccess { get; set; }

	public string Message { get; set; } = StringResources.Language.UnknownError;

	public int ErrorCode { get; set; } = 10000;

	public OperateResult()
	{
	}

	public OperateResult(string msg)
	{
		Message = msg;
	}

	public OperateResult(int err, string msg)
	{
		ErrorCode = err;
		Message = msg;
	}

	public string ToMessageShowString()
	{
		return $"{StringResources.Language.ErrorCode}:{ErrorCode}{Environment.NewLine}{StringResources.Language.TextDescription}:{Message}";
	}

	public void CopyErrorFromOther<TResult>(TResult result) where TResult : OperateResult
	{
		if (result != null)
		{
			ErrorCode = result.ErrorCode;
			Message = result.Message;
		}
	}

	public OperateResult<T> Convert<T>(T content)
	{
		return IsSuccess ? CreateSuccessResult(content) : CreateFailedResult<T>(this);
	}

	public OperateResult<T> ConvertFailed<T>()
	{
		return CreateFailedResult<T>(this);
	}

	public OperateResult<T1, T2> Convert<T1, T2>(T1 content1, T2 content2)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2) : CreateFailedResult<T1, T2>(this);
	}

	public OperateResult<T1, T2> ConvertFailed<T1, T2>()
	{
		return CreateFailedResult<T1, T2>(this);
	}

	public OperateResult<T1, T2, T3> Convert<T1, T2, T3>(T1 content1, T2 content2, T3 content3)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3) : CreateFailedResult<T1, T2, T3>(this);
	}

	public OperateResult<T1, T2, T3> ConvertFailed<T1, T2, T3>()
	{
		return CreateFailedResult<T1, T2, T3>(this);
	}

	public OperateResult<T1, T2, T3, T4> Convert<T1, T2, T3, T4>(T1 content1, T2 content2, T3 content3, T4 content4)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3, content4) : CreateFailedResult<T1, T2, T3, T4>(this);
	}

	public OperateResult<T1, T2, T3, T4> ConvertFailed<T1, T2, T3, T4>()
	{
		return CreateFailedResult<T1, T2, T3, T4>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5> Convert<T1, T2, T3, T4, T5>(T1 content1, T2 content2, T3 content3, T4 content4, T5 content5)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3, content4, content5) : CreateFailedResult<T1, T2, T3, T4, T5>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5> ConvertFailed<T1, T2, T3, T4, T5>()
	{
		return CreateFailedResult<T1, T2, T3, T4, T5>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6> Convert<T1, T2, T3, T4, T5, T6>(T1 content1, T2 content2, T3 content3, T4 content4, T5 content5, T6 content6)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3, content4, content5, content6) : CreateFailedResult<T1, T2, T3, T4, T5, T6>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6> ConvertFailed<T1, T2, T3, T4, T5, T6>()
	{
		return CreateFailedResult<T1, T2, T3, T4, T5, T6>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7> Convert<T1, T2, T3, T4, T5, T6, T7>(T1 content1, T2 content2, T3 content3, T4 content4, T5 content5, T6 content6, T7 content7)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3, content4, content5, content6, content7) : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7> ConvertFailed<T1, T2, T3, T4, T5, T6, T7>()
	{
		return CreateFailedResult<T1, T2, T3, T4, T5, T6, T7>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> Convert<T1, T2, T3, T4, T5, T6, T7, T8>(T1 content1, T2 content2, T3 content3, T4 content4, T5 content5, T6 content6, T7 content7, T8 content8)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3, content4, content5, content6, content7, content8) : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> ConvertFailed<T1, T2, T3, T4, T5, T6, T7, T8>()
	{
		return CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> Convert<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 content1, T2 content2, T3 content3, T4 content4, T5 content5, T6 content6, T7 content7, T8 content8, T9 content9)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3, content4, content5, content6, content7, content8, content9) : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> ConvertFailed<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
	{
		return CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Convert<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 content1, T2 content2, T3 content3, T4 content4, T5 content5, T6 content6, T7 content7, T8 content8, T9 content9, T10 content10)
	{
		return IsSuccess ? CreateSuccessResult(content1, content2, content3, content4, content5, content6, content7, content8, content9, content10) : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ConvertFailed<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
	{
		return CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this);
	}

	public OperateResult Then(Func<OperateResult> func)
	{
		return IsSuccess ? func() : this;
	}

	public OperateResult<T> Then<T>(Func<OperateResult<T>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T>(this);
	}

	public OperateResult<T1, T2> Then<T1, T2>(Func<OperateResult<T1, T2>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2>(this);
	}

	public OperateResult<T1, T2, T3> Then<T1, T2, T3>(Func<OperateResult<T1, T2, T3>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3>(this);
	}

	public OperateResult<T1, T2, T3, T4> Then<T1, T2, T3, T4>(Func<OperateResult<T1, T2, T3, T4>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3, T4>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5> Then<T1, T2, T3, T4, T5>(Func<OperateResult<T1, T2, T3, T4, T5>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3, T4, T5>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6> Then<T1, T2, T3, T4, T5, T6>(Func<OperateResult<T1, T2, T3, T4, T5, T6>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3, T4, T5, T6>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7> Then<T1, T2, T3, T4, T5, T6, T7>(Func<OperateResult<T1, T2, T3, T4, T5, T6, T7>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> Then<T1, T2, T3, T4, T5, T6, T7, T8>(Func<OperateResult<T1, T2, T3, T4, T5, T6, T7, T8>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> func)
	{
		return IsSuccess ? func() : CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this);
	}

	public static OperateResult<T> CreateFailedResult<T>(OperateResult result)
	{
		return new OperateResult<T>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T> CreateFailedResult<T>(string msgHead, OperateResult result)
	{
		return new OperateResult<T>
		{
			ErrorCode = result.ErrorCode,
			Message = msgHead + " : " + result.Message
		};
	}

	public static OperateResult<T1, T2> CreateFailedResult<T1, T2>(OperateResult result)
	{
		return new OperateResult<T1, T2>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2> CreateFailedResult<T1, T2>(string msgHead, OperateResult result)
	{
		return new OperateResult<T1, T2>
		{
			ErrorCode = result.ErrorCode,
			Message = msgHead + " : " + result.Message
		};
	}

	public static OperateResult<T1, T2, T3> CreateFailedResult<T1, T2, T3>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2, T3> CreateFailedResult<T1, T2, T3>(string msgHead, OperateResult result)
	{
		return new OperateResult<T1, T2, T3>
		{
			ErrorCode = result.ErrorCode,
			Message = msgHead + " : " + result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4> CreateFailedResult<T1, T2, T3, T4>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4> CreateFailedResult<T1, T2, T3, T4>(string msgHead, OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4>
		{
			ErrorCode = result.ErrorCode,
			Message = msgHead + " : " + result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5> CreateFailedResult<T1, T2, T3, T4, T5>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4, T5>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6> CreateFailedResult<T1, T2, T3, T4, T5, T6>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(OperateResult result)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			ErrorCode = result.ErrorCode,
			Message = result.Message
		};
	}

	public static OperateResult CreateSuccessResult()
	{
		return new OperateResult
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText
		};
	}

	public static OperateResult<T> CreateSuccessResult<T>(T value)
	{
		return new OperateResult<T>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content = value
		};
	}

	public static OperateResult<T1, T2> CreateSuccessResult<T1, T2>(T1 value1, T2 value2)
	{
		return new OperateResult<T1, T2>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2
		};
	}

	public static OperateResult<T1, T2, T3> CreateSuccessResult<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
	{
		return new OperateResult<T1, T2, T3>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3
		};
	}

	public static OperateResult<T1, T2, T3, T4> CreateSuccessResult<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
	{
		return new OperateResult<T1, T2, T3, T4>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3,
			Content4 = value4
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5> CreateSuccessResult<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
	{
		return new OperateResult<T1, T2, T3, T4, T5>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3,
			Content4 = value4,
			Content5 = value5
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6> CreateSuccessResult<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3,
			Content4 = value4,
			Content5 = value5,
			Content6 = value6
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3,
			Content4 = value4,
			Content5 = value5,
			Content6 = value6,
			Content7 = value7
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3,
			Content4 = value4,
			Content5 = value5,
			Content6 = value6,
			Content7 = value7,
			Content8 = value8
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3,
			Content4 = value4,
			Content5 = value5,
			Content6 = value6,
			Content7 = value7,
			Content8 = value8,
			Content9 = value9
		};
	}

	public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)
	{
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			IsSuccess = true,
			ErrorCode = 0,
			Message = StringResources.Language.SuccessText,
			Content1 = value1,
			Content2 = value2,
			Content3 = value3,
			Content4 = value4,
			Content5 = value5,
			Content6 = value6,
			Content7 = value7,
			Content8 = value8,
			Content9 = value9,
			Content10 = value10
		};
	}
}
public class OperateResult<T> : OperateResult
{
	public T Content { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T> Check(Func<T, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content))
		{
			return this;
		}
		return new OperateResult<T>(message);
	}

	public OperateResult<T> Check(Func<T, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T, OperateResult> func)
	{
		return base.IsSuccess ? func(Content) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2> Check(Func<T1, T2, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2))
		{
			return this;
		}
		return new OperateResult<T1, T2>(message);
	}

	public OperateResult<T1, T2> Check(Func<T1, T2, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3> Check(Func<T1, T2, T3, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3>(message);
	}

	public OperateResult<T1, T2, T3> Check(Func<T1, T2, T3, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3, T4> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public T4 Content4 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3, T4> Check(Func<T1, T2, T3, T4, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3, Content4))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3, T4>(message);
	}

	public OperateResult<T1, T2, T3, T4> Check(Func<T1, T2, T3, T4, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3, Content4);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3, T4>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, T4, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, T4, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, T4, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3, T4, T5> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public T4 Content4 { get; set; }

	public T5 Content5 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3, T4, T5> Check(Func<T1, T2, T3, T4, T5, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3, Content4, Content5))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3, T4, T5>(message);
	}

	public OperateResult<T1, T2, T3, T4, T5> Check(Func<T1, T2, T3, T4, T5, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3, Content4, Content5);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3, T4, T5>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, T4, T5, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, T4, T5, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, T4, T5, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3, T4, T5, T6> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public T4 Content4 { get; set; }

	public T5 Content5 { get; set; }

	public T6 Content6 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3, T4, T5, T6> Check(Func<T1, T2, T3, T4, T5, T6, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3, Content4, Content5, Content6))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3, T4, T5, T6>(message);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6> Check(Func<T1, T2, T3, T4, T5, T6, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3, Content4, Content5, Content6);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3, T4, T5, T6>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, T4, T5, T6, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, T4, T5, T6, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3, T4, T5, T6, T7> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public T4 Content4 { get; set; }

	public T5 Content5 { get; set; }

	public T6 Content6 { get; set; }

	public T7 Content7 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7> Check(Func<T1, T2, T3, T4, T5, T6, T7, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3, Content4, Content5, Content6, Content7))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7>(message);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7> Check(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3, Content4, Content5, Content6, Content7);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3, T4, T5, T6, T7>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, T4, T5, T6, T7, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public T4 Content4 { get; set; }

	public T5 Content5 { get; set; }

	public T6 Content6 { get; set; }

	public T7 Content7 { get; set; }

	public T8 Content8 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> Check(Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8>(message);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> Check(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public T4 Content4 { get; set; }

	public T5 Content5 { get; set; }

	public T6 Content6 { get; set; }

	public T7 Content7 { get; set; }

	public T8 Content8 { get; set; }

	public T9 Content9 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> Check(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(message);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> Check(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
public class OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : OperateResult
{
	public T1 Content1 { get; set; }

	public T2 Content2 { get; set; }

	public T3 Content3 { get; set; }

	public T4 Content4 { get; set; }

	public T5 Content5 { get; set; }

	public T6 Content6 { get; set; }

	public T7 Content7 { get; set; }

	public T8 Content8 { get; set; }

	public T9 Content9 { get; set; }

	public T10 Content10 { get; set; }

	public OperateResult()
	{
	}

	public OperateResult(string msg)
		: base(msg)
	{
	}

	public OperateResult(int err, string msg)
		: base(err, msg)
	{
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Check(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> check, string message = "All content data check failed")
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		if (check(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10))
		{
			return this;
		}
		return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(message);
	}

	public OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Check(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult> check)
	{
		if (!base.IsSuccess)
		{
			return this;
		}
		OperateResult operateResult = check(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(operateResult);
		}
		return this;
	}

	public OperateResult Then(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : this;
	}

	public OperateResult<TResult> Then<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult>(this);
	}

	public OperateResult<TResult1, TResult2> Then<TResult1, TResult2>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3> Then<TResult1, TResult2, TResult3>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4> Then<TResult1, TResult2, TResult3, TResult4>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3, TResult4>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5> Then<TResult1, TResult2, TResult3, TResult4, TResult5>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this);
	}

	public OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10> Then<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, OperateResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>> func)
	{
		return base.IsSuccess ? func(Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8, Content9, Content10) : OperateResult.CreateFailedResult<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this);
	}
}
