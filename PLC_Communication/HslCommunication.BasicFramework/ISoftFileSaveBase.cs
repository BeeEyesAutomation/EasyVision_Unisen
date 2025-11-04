namespace HslCommunication.BasicFramework;

public interface ISoftFileSaveBase
{
	string FileSavePath { get; set; }

	string ToSaveString();

	void LoadByString(string content);

	void LoadByFile();

	void SaveToFile();
}
