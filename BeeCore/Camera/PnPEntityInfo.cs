using System;

namespace UsbRequestFuntion
{
	public struct PnPEntityInfo
	{
		public string PNPDeviceID;

		public string Name;

		public string Description;

		public string Service;

		public string Status;

		public ushort VendorID;

		public ushort ProductID;

		public Guid ClassGuid;
	}
}
