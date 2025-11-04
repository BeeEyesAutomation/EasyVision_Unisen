using System;
using System.Text;

namespace HslCommunication.LogNet;

public class LogNetManagment
{
	public static string LogFileHeadString = "Logs_";

	public static ILogNet LogNet { get; set; }

	internal static string GetDegreeDescription(HslMessageDegree degree)
	{
		if (1 == 0)
		{
		}
		string result = degree switch
		{
			HslMessageDegree.DEBUG => StringResources.Language.LogNetDebug, 
			HslMessageDegree.INFO => StringResources.Language.LogNetInfo, 
			HslMessageDegree.WARN => StringResources.Language.LogNetWarn, 
			HslMessageDegree.ERROR => StringResources.Language.LogNetError, 
			HslMessageDegree.FATAL => StringResources.Language.LogNetFatal, 
			HslMessageDegree.None => StringResources.Language.LogNetAbandon, 
			_ => StringResources.Language.LogNetAbandon, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static string GetSaveStringFromException(string text, Exception ex)
	{
		StringBuilder stringBuilder = new StringBuilder(text);
		if (ex != null)
		{
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.Append(" : ");
			}
			try
			{
				stringBuilder.Append(StringResources.Language.ExceptionMessage);
				stringBuilder.Append(ex.Message);
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(StringResources.Language.ExceptionSource);
				stringBuilder.Append(ex.Source);
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(StringResources.Language.ExceptionStackTrace);
				stringBuilder.Append(ex.StackTrace);
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(StringResources.Language.ExceptionType);
				stringBuilder.Append(ex.GetType().ToString());
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(StringResources.Language.ExceptionTargetSite);
				stringBuilder.Append(ex.TargetSite?.ToString());
			}
			catch
			{
			}
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("\u0002/=================================================[    Exception    ]================================================/");
		}
		try
		{
			return stringBuilder.ToString();
		}
		catch
		{
			return string.IsNullOrEmpty(text) ? ex.Message : (text + ":" + ex.Message);
		}
	}
}
