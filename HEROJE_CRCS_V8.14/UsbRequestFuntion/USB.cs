using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;

namespace UsbRequestFuntion
{
	public class USB
	{
		public static PnPEntityInfo[] AllUsbDevices
		{
			get
			{
				return WhoUsbDevice(0, 0, Guid.Empty);
			}
		}

		public static PnPEntityInfo[] AllPnPEntities
		{
			get
			{
				return WhoPnPEntity(0, 0, Guid.Empty);
			}
		}

		public static PnPEntityInfo[] EnumAllUsbHidDevices
		{
			get
			{
				return WhoPnPEntity(0, 0, new Guid("{745a17a0-74d3-11d0-b6fe-00a0c90f57da}"));
			}
		}

		public static PnPEntityInfo[] EnumAllUsbComDevices
		{
			get
			{
				return WhoPnPEntity(0, 0, new Guid("{4d36e978-e325-11ce-bfc1-08002be10318}"));
			}
		}

		public static string[] MulGetHardwareInfo(HardwareEnum hardType, string propKey)
		{
			List<string> list = new List<string>();
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"");
				ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
				foreach (ManagementBaseObject item in managementObjectCollection)
				{
					if (item.Properties[propKey].Value.ToString().Contains("COM"))
					{
						list.Add(item.Properties[propKey].Value.ToString());
					}
				}
				managementObjectSearcher.Dispose();
				return list.ToArray();
			}
			catch
			{
				return null;
			}
			finally
			{
				list = null;
			}
		}

		public static PnPEntityInfo[] WhoUsbDevice(ushort VendorID, ushort ProductID, Guid ClassGuid)
		{
			List<PnPEntityInfo> list = new List<PnPEntityInfo>();
			ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get();
			if (managementObjectCollection != null)
			{
				PnPEntityInfo item = default(PnPEntityInfo);
				foreach (ManagementObject item2 in managementObjectCollection)
				{
					string text = (item2["Dependent"] as string).Split('=')[1];
					Match match = Regex.Match(text, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
					if (!match.Success)
					{
						continue;
					}
					ushort num = Convert.ToUInt16(match.Value.Substring(4, 4), 16);
					if (VendorID != 0 && VendorID != num)
					{
						continue;
					}
					ushort num2 = Convert.ToUInt16(match.Value.Substring(13, 4), 16);
					if (ProductID != 0 && ProductID != num2)
					{
						continue;
					}
					ManagementObjectCollection managementObjectCollection2 = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + text).Get();
					if (managementObjectCollection2 == null)
					{
						continue;
					}
					foreach (ManagementObject item3 in managementObjectCollection2)
					{
						Guid guid = new Guid(item3["ClassGuid"] as string);
						if (!(ClassGuid != Guid.Empty) || !(ClassGuid != guid))
						{
							item.PNPDeviceID = item3["PNPDeviceID"] as string;
							item.Name = item3["Name"] as string;
							item.Description = item3["Description"] as string;
							item.Service = item3["Service"] as string;
							item.Status = item3["Status"] as string;
							item.VendorID = num;
							item.ProductID = num2;
							item.ClassGuid = guid;
							list.Add(item);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.ToArray();
		}

		public static PnPEntityInfo[] WhoUsbDevice(ushort VendorID, ushort ProductID)
		{
			return WhoUsbDevice(VendorID, ProductID, Guid.Empty);
		}

		public static PnPEntityInfo[] WhoUsbDevice(Guid ClassGuid)
		{
			return WhoUsbDevice(0, 0, ClassGuid);
		}

		public static PnPEntityInfo[] WhoUsbDevice(string PNPDeviceID)
		{
			List<PnPEntityInfo> list = new List<PnPEntityInfo>();
			ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get();
			if (managementObjectCollection != null)
			{
				PnPEntityInfo item = default(PnPEntityInfo);
				foreach (ManagementObject item2 in managementObjectCollection)
				{
					string text = (item2["Dependent"] as string).Split('=')[1];
					if (!string.IsNullOrEmpty(PNPDeviceID) && text.IndexOf(PNPDeviceID, 1, PNPDeviceID.Length - 2, StringComparison.OrdinalIgnoreCase) == -1)
					{
						continue;
					}
					Match match = Regex.Match(text, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
					if (!match.Success)
					{
						continue;
					}
					ManagementObjectCollection managementObjectCollection2 = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + text).Get();
					if (managementObjectCollection2 == null)
					{
						continue;
					}
					foreach (ManagementObject item3 in managementObjectCollection2)
					{
						item.PNPDeviceID = item3["PNPDeviceID"] as string;
						item.Name = item3["Name"] as string;
						item.Description = item3["Description"] as string;
						item.Service = item3["Service"] as string;
						item.Status = item3["Status"] as string;
						item.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);
						item.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16);
						item.ClassGuid = new Guid(item3["ClassGuid"] as string);
						list.Add(item);
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.ToArray();
		}

		public static PnPEntityInfo[] WhoUsbDevice(string[] ServiceCollection)
		{
			if (ServiceCollection == null || ServiceCollection.Length == 0)
			{
				return WhoUsbDevice(0, 0, Guid.Empty);
			}
			List<PnPEntityInfo> list = new List<PnPEntityInfo>();
			ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get();
			if (managementObjectCollection != null)
			{
				PnPEntityInfo item = default(PnPEntityInfo);
				foreach (ManagementObject item2 in managementObjectCollection)
				{
					string text = (item2["Dependent"] as string).Split('=')[1];
					Match match = Regex.Match(text, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
					if (!match.Success)
					{
						continue;
					}
					ManagementObjectCollection managementObjectCollection2 = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + text).Get();
					if (managementObjectCollection2 == null)
					{
						continue;
					}
					foreach (ManagementObject item3 in managementObjectCollection2)
					{
						string text2 = item3["Service"] as string;
						if (string.IsNullOrEmpty(text2))
						{
							continue;
						}
						foreach (string strB in ServiceCollection)
						{
							if (string.Compare(text2, strB, true) == 0)
							{
								item.PNPDeviceID = item3["PNPDeviceID"] as string;
								item.Name = item3["Name"] as string;
								item.Description = item3["Description"] as string;
								item.Service = text2;
								item.Status = item3["Status"] as string;
								item.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);
								item.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16);
								item.ClassGuid = new Guid(item3["ClassGuid"] as string);
								list.Add(item);
								break;
							}
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.ToArray();
		}

		public static PnPEntityInfo[] WhoPnPEntity(ushort VendorID, ushort ProductID, Guid ClassGuid)
		{
			List<PnPEntityInfo> list = new List<PnPEntityInfo>();
			string text = ((VendorID == 0) ? ((ProductID != 0) ? ("'%VID[_]____&PID[_]" + ProductID.ToString("X4") + "%'") : "'%VID[_]____&PID[_]____%'") : ((ProductID != 0) ? ("'%VID[_]" + VendorID.ToString("X4") + "&PID[_]" + ProductID.ToString("X4") + "%'") : ("'%VID[_]" + VendorID.ToString("X4") + "&PID[_]____%'")));
			string queryString = ((!(ClassGuid == Guid.Empty)) ? ("SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE" + text + " AND ClassGuid='" + ClassGuid.ToString("B") + "'") : ("SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE" + text));
			ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(queryString).Get();
			if (managementObjectCollection != null)
			{
				PnPEntityInfo item = default(PnPEntityInfo);
				foreach (ManagementObject item2 in managementObjectCollection)
				{
					string text2 = item2["PNPDeviceID"] as string;
					Match match = Regex.Match(text2, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
					string text3 = item2["Caption"] as string;
					string text4 = item2["CreationClassName"] as string;
					string text5 = item2["Description"] as string;
					string text6 = item2["Manufacturer"] as string;
					string text7 = item2["SystemName"] as string;
					string text8 = item2["SystemCreationClassName"] as string;
					if (match.Success)
					{
						item.PNPDeviceID = text2;
						item.Name = item2["Name"] as string;
						item.Description = item2["Description"] as string;
						item.Service = item2["Service"] as string;
						item.Status = item2["Status"] as string;
						item.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);
						item.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16);
						item.ClassGuid = new Guid(item2["ClassGuid"] as string);
						list.Add(item);
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.ToArray();
		}

		public static PnPEntityInfo[] WhoPnPEntity(ushort VendorID, ushort ProductID)
		{
			return WhoPnPEntity(VendorID, ProductID, Guid.Empty);
		}

		public static PnPEntityInfo[] WhoPnPEntity(Guid ClassGuid)
		{
			return WhoPnPEntity(0, 0, ClassGuid);
		}

		public static PnPEntityInfo[] WhoPnPEntity(string PNPDeviceID)
		{
			List<PnPEntityInfo> list = new List<PnPEntityInfo>();
			string queryString = ((!string.IsNullOrEmpty(PNPDeviceID)) ? ("SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%" + PNPDeviceID.Replace('\\', '_') + "%'") : "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%VID[_]____&PID[_]____%'");
			ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(queryString).Get();
			if (managementObjectCollection != null)
			{
				PnPEntityInfo item = default(PnPEntityInfo);
				foreach (ManagementObject item2 in managementObjectCollection)
				{
					string text = item2["PNPDeviceID"] as string;
					Match match = Regex.Match(text, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
					if (match.Success)
					{
						item.PNPDeviceID = text;
						item.Name = item2["Name"] as string;
						item.Description = item2["Description"] as string;
						item.Service = item2["Service"] as string;
						item.Status = item2["Status"] as string;
						item.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);
						item.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16);
						item.ClassGuid = new Guid(item2["ClassGuid"] as string);
						list.Add(item);
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.ToArray();
		}

		public static PnPEntityInfo[] WhoPnPEntity(string[] ServiceCollection)
		{
			if (ServiceCollection == null || ServiceCollection.Length == 0)
			{
				return WhoPnPEntity(0, 0, Guid.Empty);
			}
			List<PnPEntityInfo> list = new List<PnPEntityInfo>();
			string queryString = "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%VID[_]____&PID[_]____%'";
			ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(queryString).Get();
			if (managementObjectCollection != null)
			{
				PnPEntityInfo item = default(PnPEntityInfo);
				foreach (ManagementObject item2 in managementObjectCollection)
				{
					string text = item2["PNPDeviceID"] as string;
					Match match = Regex.Match(text, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
					if (!match.Success)
					{
						continue;
					}
					string text2 = item2["Service"] as string;
					if (string.IsNullOrEmpty(text2))
					{
						continue;
					}
					foreach (string strB in ServiceCollection)
					{
						if (string.Compare(text2, strB, true) == 0)
						{
							item.PNPDeviceID = text;
							item.Name = item2["Name"] as string;
							item.Description = item2["Description"] as string;
							item.Service = text2;
							item.Status = item2["Status"] as string;
							item.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);
							item.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16);
							item.ClassGuid = new Guid(item2["ClassGuid"] as string);
							list.Add(item);
							break;
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.ToArray();
		}
	}
}
