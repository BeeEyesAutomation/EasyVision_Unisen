namespace CoordinateConv_N
{
	internal class CoordinateConv
	{
		public static void ImgToPtb(int imageWidth, int imageHeight, int PtbWidth, int PtbHeight, int x0, int y0, ref int x, ref int y)
		{
			if ((double)PtbWidth / (double)PtbHeight >= (double)imageWidth / (double)imageHeight)
			{
				x = (int)(((double)PtbWidth - (double)imageWidth * (double)PtbHeight / (double)imageHeight) / 2.0 + (double)x0 * (double)PtbHeight / (double)imageHeight);
				y = (int)((double)y0 * (double)PtbHeight / (double)imageHeight);
			}
			else
			{
				x = (int)((double)x0 * (double)PtbWidth / (double)imageWidth);
				y = (int)(((double)PtbHeight - (double)imageHeight * (double)PtbWidth / (double)imageWidth) / 2.0 + (double)y0 * (double)PtbWidth / (double)imageWidth);
			}
		}

		public static void PtbToImg(int imageWidth, int imageHeight, int PtbWidth, int PtbHeight, int x0, int y0, ref int x, ref int y)
		{
			if ((float)PtbWidth / (float)PtbHeight >= (float)imageWidth / (float)imageHeight)
			{
				x = (int)((float)imageHeight / (float)PtbHeight * ((float)x0 - ((float)PtbWidth - (float)imageWidth * (float)PtbHeight / (float)imageHeight) / 2f));
				y = (int)((float)y0 * (float)imageHeight / (float)PtbHeight);
			}
			else
			{
				x = (int)((float)x0 / (float)PtbWidth * (float)imageWidth);
				y = (int)((float)imageWidth / (float)PtbWidth * ((float)y0 - ((float)PtbHeight - (float)imageHeight * (float)PtbWidth / (float)imageWidth) / 2f));
			}
			x = ((x >= 0) ? x : 0);
			y = ((y >= 0) ? y : 0);
			x = ((x > imageWidth) ? imageWidth : x);
			y = ((y > imageHeight) ? imageHeight : y);
		}
	}
}
