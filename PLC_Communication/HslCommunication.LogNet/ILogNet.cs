using System;

namespace HslCommunication.LogNet;

public interface ILogNet : IDisposable
{
	LogSaveMode LogSaveMode { get; }

	LogStatistics LogNetStatistics { get; set; }

	bool ConsoleOutput { get; set; }

	bool LogThreadID { get; set; }

	bool LogStxAsciiCode { get; set; }

	int HourDeviation { get; set; }

	bool ConsoleColorEnable { get; set; }

	bool EncoderShouldEmitUTF8Identifier { get; set; }

	event EventHandler<HslEventArgs> BeforeSaveToFile;

	void RecordMessage(HslMessageDegree degree, string keyWord, string text);

	void WriteDescrition(string description);

	void WriteDebug(string text);

	void WriteDebug(string keyWord, string text);

	void WriteError(string text);

	void WriteError(string keyWord, string text);

	void WriteException(string keyWord, Exception ex);

	void WriteException(string keyWord, string text, Exception ex);

	void WriteFatal(string text);

	void WriteFatal(string keyWord, string text);

	void WriteInfo(string text);

	void WriteInfo(string keyWord, string text);

	void WriteNewLine();

	void WriteAnyString(string text);

	void WriteWarn(string text);

	void WriteWarn(string keyWord, string text);

	void SetMessageDegree(HslMessageDegree degree);

	string[] GetExistLogFileNames();

	void FiltrateKeyword(string keyword);

	void RemoveFiltrate(string keyword);
}
