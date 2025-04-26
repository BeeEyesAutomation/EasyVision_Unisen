namespace FTPManager_N
{
	public class FtpFileInfo
	{
		public string UnixFileType { get; set; }

		public string Permission { get; set; }

		public string NumberOfHardLinks { get; set; }

		public string Owner { get; set; }

		public string Group { get; set; }

		public string Size { get; set; }

		public string LastModifiedDate { get; set; }

		public string FileName { get; set; }

		public string FileDetail { get; set; }

		public FtpFileInfo(string fileDetail)
		{
			FileDetail = fileDetail;
			int num = 1;
			string[] array = fileDetail.Split(' ');
			string[] array2 = array;
			foreach (string text in array2)
			{
				switch (num)
				{
				case 1:
					if (!string.IsNullOrWhiteSpace(text))
					{
						if (text.Length == 10)
						{
							UnixFileType = text[0].ToString();
							Permission = text.Substring(1);
						}
						num++;
					}
					break;
				case 2:
					if (!string.IsNullOrWhiteSpace(text))
					{
						NumberOfHardLinks = text;
						num++;
					}
					break;
				case 3:
					if (!string.IsNullOrWhiteSpace(text))
					{
						Owner = text;
						num++;
					}
					break;
				case 4:
					if (!string.IsNullOrWhiteSpace(text))
					{
						Group = text;
						num++;
					}
					break;
				case 5:
					if (!string.IsNullOrWhiteSpace(text))
					{
						Size = text;
						num++;
					}
					break;
				case 6:
				case 7:
				case 8:
					if (!string.IsNullOrWhiteSpace(text))
					{
						LastModifiedDate = LastModifiedDate + text + " ";
						num++;
					}
					break;
				case 9:
					if (string.IsNullOrWhiteSpace(text))
					{
						FileName += " ";
					}
					else
					{
						FileName += text;
					}
					break;
				}
			}
			LastModifiedDate = LastModifiedDate.Trim();
			FileName = FileName.Trim();
		}
	}
}
