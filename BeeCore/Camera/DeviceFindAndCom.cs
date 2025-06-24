using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

using LibUsbDotNet;
using LibUsbDotNet.DeviceNotify;
using LibUsbDotNet.Main;
using UsbRequestFuntion;

namespace BeeCore
{
	public class DeviceFindAndCom
	{
		public enum DeviceType
		{
			USB_HID,
			USB_COM,
			USB_LIB,
			NETWORK,
			RS232
		}

		public class DeviceFound
		{
			public TreeNode Node;

			public DeviceType ConnectType;

			public object DeviceHandle;

			public int SerialNum;

			public ushort USB_VID;

			public ushort USB_PID;

			public int CheckIsAliveCount;

			public bool IsConnect;

			public bool IsOldDevice;

			public bool IsCommunicate;

			public string OtherInfoStr;

			public string IpAddrStr;

			public string MacStr;

			public string NetName;

			public UserTcp user_tcp;

			public Device_DataSend SendData;

			public Device_Close CloseDevice;

			public DeviceFound(DeviceType type, bool is_old, string otherstr)
			{
				Node = null;
				ConnectType = type;
				SerialNum = 0;
				USB_VID = 0;
				USB_PID = 0;
				IsConnect = false;
				IsOldDevice = is_old;
				OtherInfoStr = otherstr;
				DeviceHandle = 0;
				SendData = null;
				CloseDevice = null;
				MacStr = "";
				IpAddrStr = "";
				NetName = "";
				user_tcp = null;
				IsCommunicate = false;
			}
		}

		public delegate void Device_Close();

		public delegate void Device_DataSend(object devhdl, byte[] dat, int len);

		public delegate void Device_DataReceived(ref DeviceFound device, byte[] dat, int len);

		public delegate void ConnectStateChange(ref DeviceFound device, bool state);

		private delegate void UsbErrorEventDelegate(object sender, UsbError e);

		public class UserTcp
		{
			public TcpClient TCPC;

			public string Name;

			public string MAC;

			public string Serial;

			public DeviceFound device;

			public UserTcp(TcpClient tcpc, string name, string mac)
			{
				TCPC = tcpc;
				Name = name;
				MAC = mac;
				Serial = null;
			}
		}

		public Semaphore SemConnect = new Semaphore(0, 4);

		public List<DeviceFound> DeviceFoudList = new List<DeviceFound>();

		private UsbDeviceFinder MyUsbFinder;

		private IDeviceNotifier devNotifier;

		private Thread ThreadConnectProc;

		private System.Timers.Timer TcpCheckConnectTimer;

		private Device_DataReceived UartDataReceive;

		private ConnectStateChange ConnectStateChangeProc;

		public int IpAddrSeg0;

		public int IpAddrSeg1;

		public int IpAddrSeg2;

		public int IpAddrSeg3;

		private int NET_PORT = 47823;

		private byte[] mRxBuf = new byte[10240];

		private bool is_stop = false;

		private bool is_find_usb = false;

		private bool DeviceLostTips = false;

		private DeviceFound USB_Lib_Device;

		private UsbDevice mUsbDevice;

		private UsbEndpointReader mEpReader;

		private UsbEndpointWriter mEpWriter;

		private const int EpReadNum = 1;

		private const int EpWriteNum = 1;

		private bool is_need_log = true;

		private IPAddress LastIpAddr = new IPAddress(new byte[4] { 192, 168, 1, 91 });

		private bool is_device_alive = false;

		private bool is_need_Serch = true;

		public const uint ACK_TYPE_FOUND_DEVICE_FORCE = 1u;

		public const uint ACK_TYPE_DEVICE_ALIVE = 2u;

		public const uint ACK_TYPE_FOUND_DEVICE = 3u;

		public const uint ACK_TYPE_CHANGE_IP_OK = 4u;

		public const uint ACK_TYPE_RECOVER_CFG = 5u;

		private byte[] CheckDeviceCMD_Default = new byte[16]
		{
			128, 46, 46, 128, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		};

		private byte[] CheckDeviceCMDBrocast = new byte[16]
		{
			128, 46, 46, 128, 3, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		};

		private byte[] CheckLiveCMD = new byte[16]
		{
			128, 46, 46, 128, 2, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		};

		private UdpClient UdpClinetCk;

		private bool is_use_brocast = true;

		public DeviceFindAndCom(bool is_allow_usb, bool is_allow_uart, bool is_allow_tcp, Device_DataReceived receive_proc, ConnectStateChange conect_state, int ipaddr_seg0 = 192, int ipaddr_seg1 = 168, int ipaddr_seg2 = 31, int ipaddr_seg3 = 100, bool is_brocast = true)
		{
			UartDataReceive = receive_proc;
			ConnectStateChangeProc = conect_state;
			IpAddrSeg0 = ipaddr_seg0;
			IpAddrSeg1 = ipaddr_seg1;
			IpAddrSeg2 = ipaddr_seg2;
			IpAddrSeg3 = ipaddr_seg3;
			//ToolCfg.IpAddrSeg0 = IpAddrSeg0;
			//ToolCfg.IpAddrSeg1 = IpAddrSeg1;
			//ToolCfg.IpAddrSeg2 = IpAddrSeg2;
			//ToolCfg.IpAddrSeg3 = IpAddrSeg3;
			DeviceFoudList.Clear();
			ThreadConnectProc = new Thread(StratUsbDeviceEnum);
			ThreadConnectProc.Start();
			TcpCheckConnectTimer = new System.Timers.Timer(5000.0);
			TcpCheckConnectTimer.Elapsed += TcpCheckConnectTimer_Elapsed;
			TcpCheckConnectTimer.Start();
			devNotifier = DeviceNotifier.OpenDeviceNotifier();
			devNotifier.OnDeviceNotify += onDevNotify;
			MyUsbFinder = new UsbDeviceFinder(47823, 55287);
			UsbDevice.UsbErrorEvent += UsbGlobals_UsbErrorEvent;
			is_find_usb = is_allow_usb;
			if (is_allow_usb)
			{
				PnPEntityInfo[] enumAllUsbHidDevices = USB.EnumAllUsbHidDevices;
				if (enumAllUsbHidDevices != null)
				{
					PnPEntityInfo[] array = enumAllUsbHidDevices;
					for (int i = 0; i < array.Length; i++)
					{
						PnPEntityInfo pnPEntityInfo = array[i];
						if (pnPEntityInfo.ProductID == 22337 && pnPEntityInfo.VendorID == 1155 && pnPEntityInfo.Service != null)
						{
							DeviceFound item = new DeviceFound(DeviceType.USB_HID, true, pnPEntityInfo.PNPDeviceID)
							{
								DeviceHandle = null,
								USB_VID = 1155,
								USB_PID = 22337
							};
							DeviceFoudList.Add(item);
						}
						if (pnPEntityInfo.VendorID == 47823 && pnPEntityInfo.Service != null)
						{
							DeviceFound item = new DeviceFound(DeviceType.USB_HID, false, pnPEntityInfo.PNPDeviceID)
							{
								USB_VID = 47823,
								USB_PID = pnPEntityInfo.ProductID,
								DeviceHandle = null
							};
							DeviceFoudList.Add(item);
						}
					}
				}
				SemConnect.Release();
			}
			PnPEntityInfo[] enumAllUsbComDevices = USB.EnumAllUsbComDevices;
			if (enumAllUsbComDevices != null)
			{
				PnPEntityInfo[] array2 = enumAllUsbComDevices;
				for (int j = 0; j < array2.Length; j++)
				{
					PnPEntityInfo pnPEntityInfo2 = array2[j];
					int num = pnPEntityInfo2.Name.IndexOf("(COM");
					string text = ((num + 1 + 6 > pnPEntityInfo2.Name.Length) ? pnPEntityInfo2.Name.Substring(num + 1, 5) : pnPEntityInfo2.Name.Substring(num + 1, 6));
					if (text[4] == ')')
					{
						text = text.Substring(0, 4);
					}
					else if (text[5] == ')')
					{
						text = text.Substring(0, 5);
					}
					if (pnPEntityInfo2.ProductID == 22336 && pnPEntityInfo2.VendorID == 1155 && pnPEntityInfo2.Service != null)
					{
						DeviceFound item2 = new DeviceFound(DeviceType.USB_COM, true, pnPEntityInfo2.PNPDeviceID)
						{
							DeviceHandle = null,
							USB_VID = 1155,
							USB_PID = 22337,
							NetName = text
						};
						DeviceFoudList.Add(item2);
					}
					if (pnPEntityInfo2.VendorID == 47823 && pnPEntityInfo2.Service != null)
					{
						DeviceFound item2 = new DeviceFound(DeviceType.USB_COM, false, pnPEntityInfo2.PNPDeviceID)
						{
							USB_VID = 47823,
							USB_PID = pnPEntityInfo2.ProductID,
							DeviceHandle = null,
							NetName = text
						};
						DeviceFoudList.Add(item2);
					}
					if (pnPEntityInfo2.VendorID == 4292 && pnPEntityInfo2.ProductID == 60000 && pnPEntityInfo2.Service != null)
					{
						DeviceFound item2 = new DeviceFound(DeviceType.USB_COM, false, pnPEntityInfo2.PNPDeviceID)
						{
							USB_VID = 4292,
							USB_PID = 60000,
							DeviceHandle = null,
							NetName = text
						};
						DeviceFoudList.Add(item2);
					}
					if (pnPEntityInfo2.VendorID == 6790 && pnPEntityInfo2.ProductID == 29987 && pnPEntityInfo2.Service != null)
					{
						DeviceFound item2 = new DeviceFound(DeviceType.USB_COM, false, pnPEntityInfo2.PNPDeviceID)
						{
							USB_VID = 6790,
							USB_PID = 29987,
							DeviceHandle = null,
							NetName = text
						};
						DeviceFoudList.Add(item2);
					}
				}
				SemConnect.Release();
			}
			if (is_allow_tcp)
			{
				FoundNetworkDevice(is_brocast);
			}
		}

		private void onDevNotify(object sender, DeviceNotifyEventArgs e)
		{
			try
			{
				if (is_find_usb)
				{
					SemConnect.Release();
				}
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		private void UsbGlobals_UsbErrorEvent(object sender, UsbError e)
		{
			if (mUsbDevice != null)
			{
				mUsbDevice = null;
			}
			if (ConnectStateChangeProc != null && DeviceLostTips)
			{
				DeviceLostTips = false;
				ConnectStateChangeProc(ref USB_Lib_Device, false);
			}
		}

		private void mEp_DataReceived(object sender, EndpointDataEventArgs e)
		{
			UartDataReceive(ref USB_Lib_Device, e.Buffer, e.Count);
		}

		private void StratUsbDeviceEnum()
		{
			while (!is_stop)
			{
				SemConnect.WaitOne();
				if (is_stop)
				{
					break;
				}
				if (!is_find_usb)
				{
					continue;
				}
				try
				{
					if (mUsbDevice != null && mUsbDevice.IsOpen)
					{
						continue;
					}
					mUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);
					if (mUsbDevice != null)
					{
						IUsbDevice usbDevice = mUsbDevice as IUsbDevice;
						if (usbDevice != null)
						{
							usbDevice.SetConfiguration(1);
							usbDevice.ClaimInterface(0);
						}
						mEpReader = mUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
						if (mUsbDevice.Info.SerialString.Contains("0xfadaca1"))
						{
							mEpWriter = mUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep02);
						}
						else
						{
							mEpWriter = mUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
						}
						mEpReader.DataReceived += mEp_DataReceived;
						mEpReader.Flush();
						mEpReader.DataReceivedEnabled = true;
						DeviceFound deviceFound = new DeviceFound(DeviceType.USB_LIB, true, mUsbDevice.UsbRegistryInfo.SymbolicName);
						deviceFound.DeviceHandle = mEpWriter;
						deviceFound.SendData = UsbLibDeviceSendData;
						deviceFound.USB_VID = 47823;
						deviceFound.USB_PID = 55287;
						DeviceFoudList.Add(deviceFound);
						USB_Lib_Device = DeviceFoudList.Last();
						if (ConnectStateChangeProc != null)
						{
							DeviceLostTips = true;
							ConnectStateChangeProc(ref USB_Lib_Device, true);
						}
					}
				}
				catch (Exception)
				{
					if (mUsbDevice != null)
					{
						if (mUsbDevice.IsOpen)
						{
							mUsbDevice.Close();
						}
						mUsbDevice = null;
					}
					if (ConnectStateChangeProc != null && DeviceLostTips)
					{
						DeviceLostTips = false;
						ConnectStateChangeProc(ref USB_Lib_Device, false);
					}
				}
			}
		}

		private void TcpCheckConnectTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			TcpCheckConnectTimer.Stop();
			for (int i = 0; i < DeviceFoudList.Count; i++)
			{
				DeviceFound deviceFound = DeviceFoudList[i];
				if (deviceFound.ConnectType != DeviceType.NETWORK || deviceFound.DeviceHandle != null)
				{
				}
			}
			TcpCheckConnectTimer.Start();
		}

		public string get_ipaddr(object devicehdl)
		{
			if (devicehdl == null)
			{
				return "0.0.0.0";
			}
			LastIpAddr = ((IPEndPoint)((TcpClient)devicehdl).Client.RemoteEndPoint).Address;
			return ((TcpClient)devicehdl).Client.RemoteEndPoint.ToString();
		}

		private void onCompleteReadFromServerStream(IAsyncResult iar)
		{
			int num = 0;
			DeviceFound device = (DeviceFound)iar.AsyncState;
			try
			{
				TcpClient tcpClient = (TcpClient)device.DeviceHandle;
				if (tcpClient == null) return;
				if (tcpClient.Connected)
				{
					num = tcpClient.GetStream().EndRead(iar);
				}
				if (num == 0)
				{
					device.DeviceHandle = null;
					DeviceLostTips = false;
					ConnectStateChangeProc(ref device, false);
					return;
				}
				UartDataReceive(ref device, mRxBuf, num);
				mRxBuf.Initialize();
				if (tcpClient.Connected)
				{
					tcpClient.GetStream().BeginRead(mRxBuf, 0, mRxBuf.Length, onCompleteReadFromServerStream, device);
				}
			}
			catch (Exception ex)
			{
				device.DeviceHandle = null;
				device.IsConnect = false;
				DeviceLostTips = false;
				ConnectStateChangeProc(ref device, false);
				if (is_need_log)
				{
					LogRecord.WriteError(ex);
				}
			}
		}

		private void onCompleteConnect(IAsyncResult iar)
		{
			try
			{
				UserTcp userTcp = (UserTcp)iar.AsyncState;
				TcpClient tCPC = userTcp.TCPC;
				if (tCPC.Connected)
				{
					LogRecord.WriteInfo("Tcp Connect Ok , Add Device To Trees");
					tCPC.EndConnect(iar);
					DeviceFound device = userTcp.device;
					device.DeviceHandle = tCPC;
					byte[] array = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
					tCPC.GetStream().Write(array, 0, array.Length);
					tCPC.GetStream().BeginRead(mRxBuf, 0, mRxBuf.Length, onCompleteReadFromServerStream, device);
				}
				else
				{
					LogRecord.WriteInfo("Tcp Connect Filed!RemoteAddress:" + tCPC.Client.RemoteEndPoint.ToString());
				}
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		private void TestPing_PingCompleted(object sender, PingCompletedEventArgs e)
		{
			DeviceFound deviceFound = (DeviceFound)e.UserState;
		}

		private void CheckAliveUdpAckCallback(IAsyncResult iar)
		{
			DeviceFound deviceFound = (DeviceFound)iar.AsyncState;
			if (deviceFound.DeviceHandle != null)
			{
				IPAddress address = ((IPEndPoint)((TcpClient)deviceFound.DeviceHandle).Client.RemoteEndPoint).Address;
				IPEndPoint remoteEP = new IPEndPoint(address, 48604);
				byte[] array = UdpClinetCk.EndReceive(iar, ref remoteEP);
			}
			deviceFound.CheckIsAliveCount = 0;
		}

		public void TcpTrigFind()
		{
			is_need_Serch = true;
		}

		private void RecvUdpAckCallback(IAsyncResult iar)
		{
			UdpClient udpClient = (UdpClient)iar.AsyncState;
			IPAddress iPAddress = new IPAddress(new byte[4]
			{
				(byte)IpAddrSeg0,
				(byte)IpAddrSeg1,
				(byte)IpAddrSeg2,
				(byte)IpAddrSeg3
			});
			IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
			byte[] array = udpClient.EndReceive(iar, ref remoteEP);
			UdpClinetCk.BeginReceive(RecvUdpAckCallback, UdpClinetCk);
			LogRecord.WriteInfo("Udp Rx:" + array.Length + " Bytes , " + remoteEP.Address.ToString() + "-" + remoteEP.Port);
			Console.WriteLine(remoteEP.ToString() + "found\r\n");
			if (array.Length == 32 && remoteEP.Port == 48604 && array[4] != 2 && array[4] != 4)
			{
				byte[] addressBytes = remoteEP.Address.GetAddressBytes();
				TcpClient tcpc = new TcpClient();
				string name = Encoding.Default.GetString(array, 12, 20).Replace("\0", "");
				string mac = array[6].ToString("X2") + ":" + array[7].ToString("X2") + ":" + array[8].ToString("X2") + ":" + array[9].ToString("X2") + ":" + array[10].ToString("X2") + ":" + array[11].ToString("X2");
				UserTcp userTcp = new UserTcp(tcpc, name, mac);
				DeviceFound device = new DeviceFound(DeviceType.NETWORK, true, remoteEP.Address.ToString() + "(" + userTcp.MAC + ")");
				device.IpAddrStr = remoteEP.Address.ToString();
				device.SendData = TcpDeviceSendData;
				device.MacStr = userTcp.MAC;
				device.NetName = userTcp.Name;
				device.user_tcp = userTcp;
				device.DeviceHandle = null;
				userTcp.device = device;
				if (addressBytes[0] == IpAddrSeg0 && addressBytes[1] == IpAddrSeg1 && addressBytes[2] == IpAddrSeg2)
				{
					device.IsCommunicate = true;
				}
				else
				{
					device.IsCommunicate = false;
				}
				DeviceFoudList.Add(device);
				if (ConnectStateChangeProc != null)
				{
					DeviceLostTips = true;
					ConnectStateChangeProc(ref device, true);
				}
			}
		}

		public void StartConnectDeviceByTcp(DeviceFound dev)
		{
			LogRecord.WriteInfo("StartTcpConnect...");
			try
			{
				dev.user_tcp.TCPC.BeginConnect(dev.IpAddrStr, NET_PORT, onCompleteConnect, dev.user_tcp);
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		public void StopConnectDeviceByTcp(DeviceFound dev)
		{
			LogRecord.WriteInfo("StopTcpConnect...");
			dev.DeviceHandle = null;
			dev.IsConnect = false;
			dev.user_tcp.TCPC.Close();
			Thread.Sleep(300);
			Application.DoEvents();
			FoundNetworkDevice(is_use_brocast);
		}

		public void SendDataByUdpBrocast(byte[] dat)
		{
			if (UdpClinetCk != null)
			{
				UdpClinetCk.Send(dat, dat.Length, new IPEndPoint(IPAddress.Broadcast, 48604));
			}
		}

		private void FoundNetworkDevice(bool is_brocast)
		{
			is_use_brocast = is_brocast;
			IPAddress address = new IPAddress(new byte[4]
			{
				(byte)IpAddrSeg0,
				(byte)IpAddrSeg1,
				(byte)IpAddrSeg2,
				(byte)IpAddrSeg3
			});
			try
			{
				UdpClinetCk = new UdpClient(new IPEndPoint(address, 0));
				UdpClinetCk.EnableBroadcast = true;
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
				return;
			}
			byte[] address2 = new byte[4]
			{
				(byte)IpAddrSeg0,
				(byte)IpAddrSeg1,
				(byte)IpAddrSeg2,
				255
			};
			IPAddress iPAddress = new IPAddress(address2);
			is_use_brocast = is_brocast;
			IPEndPoint endPoint = ((!is_brocast) ? new IPEndPoint(new IPAddress(address2), 48604) : new IPEndPoint(IPAddress.Broadcast, 48604));
			try
			{
				LogRecord.WriteInfo("Udp broadcast:" + iPAddress.ToString());
				if (is_brocast)
				{
					UdpClinetCk.Send(CheckDeviceCMDBrocast, CheckDeviceCMDBrocast.Length, endPoint);
				}
				else
				{
					UdpClinetCk.Send(CheckDeviceCMD_Default, CheckDeviceCMD_Default.Length, endPoint);
				}
				UdpClinetCk.BeginReceive(RecvUdpAckCallback, UdpClinetCk);
			}
			catch (Exception ex2)
			{
				LogRecord.WriteError(ex2);
			}
		}

		private void TcpDeviceSendData(object devhdl, byte[] dat, int len)
		{
			try
			{
				if (devhdl != null)
				{
					((TcpClient)devhdl).GetStream().Write(dat, 0, dat.Length);
				}
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		private void UsbLibDeviceSendData(object devhdl, byte[] dat, int len)
		{
			try
			{
				int transferLength = 0;
				ErrorCode errorCode = ((UsbEndpointWriter)devhdl).Write(dat, 0, len, 500, out transferLength);
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		public void ClearAllConnected()
		{
			foreach (DeviceFound deviceFoud in DeviceFoudList)
			{
				try
				{
					if (deviceFoud.DeviceHandle != null && deviceFoud.ConnectType == DeviceType.NETWORK && deviceFoud.IsConnect)
					{
						((TcpClient)deviceFoud.DeviceHandle).Close();
					}
				}
				catch
				{
				}
			}
		}

		public void EnumAllDevice(bool is_allow_usb, bool is_allow_uart, bool is_allow_tcp, bool is_brocast)
		{
			is_need_log = false;
			foreach (DeviceFound deviceFoud in DeviceFoudList)
			{
				try
				{
					if (deviceFoud.DeviceHandle != null && deviceFoud.ConnectType == DeviceType.NETWORK)
					{
						((TcpClient)deviceFoud.DeviceHandle).Close();
					}
				}
				catch
				{
				}
			}
			is_need_log = true;
			DeviceFoudList.Clear();
			is_find_usb = is_allow_usb;
			if (mUsbDevice != null)
			{
				mUsbDevice.Close();
			}
			mUsbDevice = null;
			if (is_allow_usb)
			{
				PnPEntityInfo[] enumAllUsbHidDevices = USB.EnumAllUsbHidDevices;
				if (enumAllUsbHidDevices != null)
				{
					PnPEntityInfo[] array = enumAllUsbHidDevices;
					for (int i = 0; i < array.Length; i++)
					{
						PnPEntityInfo pnPEntityInfo = array[i];
						if (pnPEntityInfo.ProductID == 22337 && pnPEntityInfo.VendorID == 1155 && pnPEntityInfo.Service != null)
						{
							DeviceFound deviceFound = new DeviceFound(DeviceType.USB_HID, true, pnPEntityInfo.PNPDeviceID);
							deviceFound.DeviceHandle = null;
							deviceFound.USB_VID = 1155;
							deviceFound.USB_PID = 22337;
							DeviceFoudList.Add(deviceFound);
						}
						if (pnPEntityInfo.VendorID == 47823 && pnPEntityInfo.Service != null)
						{
							DeviceFound deviceFound = new DeviceFound(DeviceType.USB_HID, false, pnPEntityInfo.PNPDeviceID);
							deviceFound.USB_VID = 47823;
							deviceFound.USB_PID = pnPEntityInfo.ProductID;
							deviceFound.DeviceHandle = null;
							DeviceFoudList.Add(deviceFound);
						}
					}
					SemConnect.Release();
				}
				PnPEntityInfo[] enumAllUsbComDevices = USB.EnumAllUsbComDevices;
				if (enumAllUsbComDevices != null)
				{
					PnPEntityInfo[] array2 = enumAllUsbComDevices;
					for (int j = 0; j < array2.Length; j++)
					{
						PnPEntityInfo pnPEntityInfo2 = array2[j];
						int num = pnPEntityInfo2.Name.IndexOf("(COM");
						string text = ((num + 1 + 6 > pnPEntityInfo2.Name.Length) ? pnPEntityInfo2.Name.Substring(num + 1, 5) : pnPEntityInfo2.Name.Substring(num + 1, 6));
						if (text[4] == ')')
						{
							text = text.Substring(0, 4);
						}
						else if (text[5] == ')')
						{
							text = text.Substring(0, 5);
						}
						if (pnPEntityInfo2.ProductID == 22336 && pnPEntityInfo2.VendorID == 1155 && pnPEntityInfo2.Service != null)
						{
							DeviceFound deviceFound2 = new DeviceFound(DeviceType.USB_COM, true, pnPEntityInfo2.PNPDeviceID);
							deviceFound2.DeviceHandle = null;
							deviceFound2.USB_VID = 1155;
							deviceFound2.USB_PID = 22337;
							deviceFound2.NetName = text;
							DeviceFoudList.Add(deviceFound2);
						}
						if (pnPEntityInfo2.VendorID == 47823 && pnPEntityInfo2.Service != null)
						{
							DeviceFound deviceFound2 = new DeviceFound(DeviceType.USB_COM, false, pnPEntityInfo2.PNPDeviceID);
							deviceFound2.USB_VID = 47823;
							deviceFound2.USB_PID = pnPEntityInfo2.ProductID;
							deviceFound2.DeviceHandle = null;
							deviceFound2.NetName = text;
							DeviceFoudList.Add(deviceFound2);
						}
						if (pnPEntityInfo2.VendorID == 4292 && pnPEntityInfo2.ProductID == 60000 && pnPEntityInfo2.Service != null)
						{
							DeviceFound deviceFound2 = new DeviceFound(DeviceType.USB_COM, false, pnPEntityInfo2.PNPDeviceID);
							deviceFound2.USB_VID = 4292;
							deviceFound2.USB_PID = 60000;
							deviceFound2.DeviceHandle = null;
							deviceFound2.NetName = text;
							DeviceFoudList.Add(deviceFound2);
						}
						if (pnPEntityInfo2.VendorID == 6790 && pnPEntityInfo2.ProductID == 29987 && pnPEntityInfo2.Service != null)
						{
							DeviceFound deviceFound2 = new DeviceFound(DeviceType.USB_COM, false, pnPEntityInfo2.PNPDeviceID);
							deviceFound2.USB_VID = 6790;
							deviceFound2.USB_PID = 29987;
							deviceFound2.DeviceHandle = null;
							deviceFound2.NetName = text;
							DeviceFoudList.Add(deviceFound2);
						}
					}
					SemConnect.Release();
				}
			}
			if (is_allow_tcp)
			{
				FoundNetworkDevice(is_brocast);
			}
		}

		public void CloseDevice()
		{
			try
			{
				is_need_log = false;
				if (mUsbDevice != null)
				{
					if (mEpReader != null)
					{
						mEpReader.DataReceivedEnabled = false;
						mEpReader.DataReceived -= mEp_DataReceived;
						mEpReader.Dispose();
						mEpReader = null;
					}
					if (mEpWriter != null)
					{
						mEpWriter.Abort();
						mEpWriter.Dispose();
						mEpWriter = null;
					}
					IUsbDevice usbDevice = mUsbDevice as IUsbDevice;
					if (usbDevice != null)
					{
						usbDevice.ReleaseInterface(0);
					}
					mUsbDevice.Close();
				}
				for (int i = 0; i < DeviceFoudList.Count; i++)
				{
					DeviceFound deviceFound = DeviceFoudList[i];
					if (deviceFound.ConnectType == DeviceType.NETWORK && deviceFound.DeviceHandle != null)
					{
						((TcpClient)deviceFound.DeviceHandle).Close();
						deviceFound.DeviceHandle = null;
					}
				}
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		public void DestroyDevice()
		{
			try
			{
				is_stop = true;
				SemConnect.Release();
				ThreadConnectProc.Abort();
				TcpCheckConnectTimer.Stop();
				TcpCheckConnectTimer.Dispose();
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}
	}
}
