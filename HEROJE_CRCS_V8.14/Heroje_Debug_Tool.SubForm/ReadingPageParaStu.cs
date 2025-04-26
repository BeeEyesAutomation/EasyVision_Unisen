using System.Drawing;

namespace Heroje_Debug_Tool.SubForm
{
	public struct ReadingPageParaStu
	{
		public ProtocolExtraDataStu ProtocolExtraData;

		public uint DecodeTime;

		public int BarcodeLen;

		public string barcode_type;

		public string barcode_str;

		public string FrameRate;

		public int TrigCount;

		public Image ImageShow;

		public DeviceConnectForm.Polygon BarCodeRegion;
	}
}
