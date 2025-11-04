using System;
using System.IO;
using System.Text;
using System.Threading;
using HslCommunication.Core;

namespace HslCommunication.BasicFramework;

public sealed class SoftNumericalOrder : SoftFileSaveBase
{
	private long CurrentIndex = 0L;

	private string TextHead = string.Empty;

	private string TimeFormate = "yyyyMMdd";

	private int NumberLength = 5;

	private HslAsyncCoordinator AsyncCoordinator = null;

	public SoftNumericalOrder(string textHead, string timeFormate, int numberLength, string fileSavePath)
	{
		base.LogHeaderText = "SoftNumericalOrder";
		TextHead = textHead;
		TimeFormate = timeFormate;
		NumberLength = numberLength;
		base.FileSavePath = fileSavePath;
		LoadByFile();
		AsyncCoordinator = new HslAsyncCoordinator(delegate
		{
			if (!string.IsNullOrEmpty(base.FileSavePath))
			{
				using (StreamWriter streamWriter = new StreamWriter(base.FileSavePath, append: false, Encoding.Default))
				{
					streamWriter.Write(CurrentIndex);
				}
			}
		});
	}

	public override string ToSaveString()
	{
		return CurrentIndex.ToString();
	}

	public override void LoadByString(string content)
	{
		CurrentIndex = Convert.ToInt64(content);
	}

	public void ClearNumericalOrder()
	{
		Interlocked.Exchange(ref CurrentIndex, 0L);
		AsyncCoordinator.StartOperaterInfomation();
	}

	public string GetNumericalOrder()
	{
		long num = Interlocked.Increment(ref CurrentIndex);
		AsyncCoordinator.StartOperaterInfomation();
		if (string.IsNullOrEmpty(TimeFormate))
		{
			return TextHead + num.ToString().PadLeft(NumberLength, '0');
		}
		return TextHead + DateTime.Now.ToString(TimeFormate) + num.ToString().PadLeft(NumberLength, '0');
	}

	public string GetNumericalOrder(string textHead)
	{
		long num = Interlocked.Increment(ref CurrentIndex);
		AsyncCoordinator.StartOperaterInfomation();
		if (string.IsNullOrEmpty(TimeFormate))
		{
			return textHead + num.ToString().PadLeft(NumberLength, '0');
		}
		return textHead + DateTime.Now.ToString(TimeFormate) + num.ToString().PadLeft(NumberLength, '0');
	}

	public long GetLongOrder()
	{
		long result = Interlocked.Increment(ref CurrentIndex);
		AsyncCoordinator.StartOperaterInfomation();
		return result;
	}
}
