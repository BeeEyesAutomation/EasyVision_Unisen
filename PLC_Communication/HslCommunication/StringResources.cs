using System.Globalization;
using HslCommunication.Language;

namespace HslCommunication;

public static class StringResources
{
	public static DefaultLanguage Language;

	static StringResources()
	{
		Language = new DefaultLanguage();
		if (CultureInfo.CurrentCulture.ToString().StartsWith("zh"))
		{
			SetLanguageChinese();
		}
		else
		{
			SeteLanguageEnglish();
		}
	}

	public static void SetLanguageChinese()
	{
		Language = new DefaultLanguage();
	}

	public static void SeteLanguageEnglish()
	{
		Language = new English();
	}
}
