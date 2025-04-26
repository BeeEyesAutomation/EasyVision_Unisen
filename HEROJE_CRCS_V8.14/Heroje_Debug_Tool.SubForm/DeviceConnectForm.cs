using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.Properties;
using HJ_CRC32_n;
using IniConfigFile_n;
using KEY_Send_n;
using ModuleSetting_n;
using 合杰图像算法调试工具;
using 图像算法调试工具;

namespace Heroje_Debug_Tool.SubForm
{
	public class DeviceConnectForm : Form
	{
		private struct DevConnectedTypeStu
		{
			public bool IsUSBConnect;

			public bool IsInternet;

			public bool IsCrossNetSegSerch;

			public DevConnectedTypeStu(int a)
			{
				IsUSBConnect = true;
				IsInternet = true;
				IsCrossNetSegSerch = true;
			}
		}

		private enum AllBarcodeType
		{
			EAN13,
			EAN8,
			UPCA,
			UPCE,
			UPCE1,
			CODE128,
			CODE39,
			CODE93,
			CODABAR,
			IL25,
			ID25,
			MT25,
			CODE11,
			MSI,
			DATABAR,
			DATABAR_LIM,
			DATABAR_EXP,
			QR,
			DM,
			PDF417,
			AZTEC,
			HAXIN,
			MPDF417,
			TRIOPTIC,
			CODEBLOCK_F,
			STRAIGHT25,
			TELEPEN,
			MAXICODE,
			PHARMA,
			CODE32
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Polygon
		{
			public Point p0;

			public Point p1;

			public Point p2;

			public Point p3;

			public Point[] ToPointArray()
			{
				return new Point[5] { p0, p1, p2, p3, p0 };
			}
		}

		private enum Image_Type
		{
			IMAGE_TYPE_JPG = 1,
			IMAGE_TYPE_BMP_8Bit,
			IMAGE_TYPE_RGB_16Bit,
			IMAGE_TYPE_BMP_24Bit,
			IMAGE_TYPE_YUV_16Bit,
			IMAGE_TYPE_GRAY_8Bit,
			IMAGE_TYPE_RAW_8Bit,
			IMAGE_TYPE_TIFF
		}

		public delegate void ChartDelegate(Chart _Chart, uint data);

		private DevConnectedTypeStu DevConnectedType = new DevConnectedTypeStu(0);

		private bool IsNeedAutoConnect = true;

		private string LastDeviceIp;

		private UiParaInfoStu UpdateUiParaInfo = default(UiParaInfoStu);

		private UpdateParaAndDisplayCB UpdateParaAndDisplayCBFunc;

		private ReadingPageParaStu ReadingPagePara = default(ReadingPageParaStu);

		private ReadingPage_UpdateDB ReadingPage_UpdateCB;

		private AdvancedPage_UpdateDB AdvancedPage_UpdateCB;

		public DevStateCB DevStateCallback;

		private const int back_ground = 169;

		private int TemplateColorIdx = 0;

		private Color[] TemplateColor = new Color[20]
		{
			Color.Blue,
			Color.Navy,
			Color.CornflowerBlue,
			Color.MediumSlateBlue,
			Color.Crimson,
			Color.DarkRed,
			Color.DarkSalmon,
			Color.Orange,
			Color.OrangeRed,
			Color.PaleGreen,
			Color.MediumSeaGreen,
			Color.CadetBlue,
			Color.DarkSlateGray,
			Color.DarkMagenta,
			Color.MediumVioletRed,
			Color.Brown,
			Color.Bisque,
			Color.Cyan,
			Color.Green,
			Color.LightGray
		};

		private ProtocolHeaderStu ProtocolHeader = default(ProtocolHeaderStu);

		private ProtocolExtraDataStu ProtocolExtraData = default(ProtocolExtraDataStu);

		private byte[] ImageData = new byte[37748736];

		private byte[] BarcodeData = new byte[4096];

		private string BarcodeStr;

		private byte[] RegionData = new byte[3200];

		private Polygon[] BarCodeRegion;

		private uint ParaDataLen = 0u;

		private uint DeviceTypeRecord = 0u;

		private int BarcodeLen = 0;

		private uint BarcodeType = 0u;

		private int ImageWidth = 0;

		private int ImageHeight = 0;

		private uint ImageSize = 0u;

		private int ImageType = 0;

		private Image ImageShow = new Bitmap(376, 240);

		private double FrameTime = 0.0;

		private long NowTime = 0L;

		private long FrameCount = 0L;

		public int UpdateOtherForm = 0;

		private const int DATA_SWITCH_PARA = 1;

		private const int DATA_SWITCH_BARCODE = 2;

		private const int DATA_SWITCH_IMAGE = 4;

		private const int DATA_SWITCH_REGION = 8;

		private const int DATA_SWITCH_EXTRA_DATA = 16;

		private const int PARA_ACTION_TO_PC_PASSWORD = 1;

		private const int PARA_ACTION_TO_PC_AF_OK = 2;

		private const int PARA_ACTION_TO_PC_AP_START = 4;

		private const int PARA_ACTION_TO_PC_AP_PROC = 8;

		private const int PARA_ACTION_TO_PC_AP_OK_END = 16;

		private const int PARA_ACTION_TO_PC_AP_NG_END = 32;

		private const int PARA_ACTION_TO_PC_ADC_PROC = 64;

		private bool SendConfigDataEn = false;

		public static Mutex mutex_mask_img = new Mutex();

		private static bool IsProfessonal = true;

		private string ExtriTrigStr = "None";

		private int IpIndexSel = 0;

		private string IPADDR_SEG0 = "192";

		private string IPADDR_SEG1 = "168";

		private string IPADDR_SEG2 = "1";

		private string IPADDR_SEG3 = "100";

		private uint MaxDecodeTime = 0u;

		private int SameCodeDlyTime;

		private Thread ThreadDataProc;

		private Semaphore SemDataOk = new Semaphore(0, 2);

		private DeviceFindAndCom DpmDevice;

		private ControlAndXML control_xml = new ControlAndXML();

		private byte[] DeviceParaRead = new byte[4096];

		private Bitmap MaskImg = new Bitmap(10, 10);

		private SolidBrush MaskBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));

		private Pen ROIPen = new Pen(new SolidBrush(Color.FromArgb(200, 0, 0, 200)), 2f);

		private Pen BarcodePen = new Pen(new SolidBrush(Color.FromArgb(255, 50, 255, 50)), 1f);

		private Color BarcodeColor = Color.FromArgb(255, 50, 255, 50);

		private bool Is_NewLinuxPassword = false;

		private bool IsLabShowStr = true;

		private bool IsSelectRoi = false;

		private bool IsSelectOK = false;

		private bool IsSelectChange = false;

		private bool IsNeedToUpdateTree = false;

		private bool IsRoiForTemplate = false;

		private Point mouseDownPoint = default(Point);

		private bool isSelected = false;

		private bool IsFocusROIPress = false;

		private int PosX0 = 0;

		private int PosY0 = 0;

		private int PosX1 = 1920;

		private int PosY1 = 1080;

		private int OffsetX = 0;

		private int OffsetY = 0;

		private double ImageRatio = 0.0;

		public ModuleSetting DeviceCfgRead;

		public bool is_get_rawimg = true;

		public bool is_udp_brocast = true;

		public bool is_clear_before_connect = true;

		private object[] invokeChartData = new object[2];

		private object thisLock = new object();

		private bool Is_Lock_Op = false;

		private bool IsDataHeaderReceive;

		private byte[] DataHeaderFlag = new byte[4] { 128, 46, 46, 128 };

		private byte[] DataEndFlag = new byte[4] { 46, 128, 128, 46 };

		private byte[] DataReceiveBufP = new byte[37748736];

		private byte[] DataReceiveBufN = new byte[37748736];

		private int rec_frame_count = 0;

		private int DataReceiveLenP = 0;

		private int DataReceiveLenN = 0;

		private bool IsReadyDataP = false;

		private bool IsReadyDataN = false;

		private bool IsClearDataP_En = false;

		private bool IsClearDataN_En = false;

		private bool IsCopyToDataP = true;

		private int TrigCount = 0;

		private int DecodeSuccessCount = 0;

		private int ImageTrigCount = 0;

		private double DecodeProportion = 1.0;

		private static bool is_no_next_data = false;

		private DeviceFindAndCom.DeviceFound SelectedDevice;

		public DriverInstall DriverInstallForm = new DriverInstall();

		private IContainer components = null;

		private ComboBox CobLocalIPSel;

		private Label LabLocalIPSel;

		private ContextMenuStrip Cms_RightDeviceClick;

		private ToolStripMenuItem Tsm_ConnectDevice;

		private ToolStripMenuItem Tsm_DisconnectDevice;

		private ToolStripMenuItem Tsm_RestartDevice;

		private ToolStripMenuItem Tsm_SaveDeviceCfg;

		private Panel PanDeviceInfo;

		private TextBox TxbDeviceInfo;

		private Label LabDeviceInfo;

		private TextBox TxbDeviceAddr;

		private GroupBox GrbDeviceOps;

		private Button BtnRecoverUserSetting;

		private Button BtnSaveUserSetting;

		private Button BtnCaptrueImageSave;

		private Button BtnRecoverFactory;

		private Label LabDeviceAddr;

		private GroupBox GrbDevicesList;

		private TreeView TrvDevicesList;

		private ToolStripMenuItem Tsm_EditDeviceIp;

		private ImageList DeviceImageList;

		public DeviceConnectForm()
		{
			InitializeComponent();
		}

		private void DevideConnectForm_Load(object sender, EventArgs e)
		{
		}

		private void DefaultTreeNodes()
		{
			TrvDevicesList.Nodes.Clear();
			TreeNode treeNode = new TreeNode();
			TreeNode treeNode2 = new TreeNode();
			treeNode.ImageIndex = 11;
			treeNode.Name = "Tree1UsbDevice";
			treeNode.SelectedImageIndex = 11;
			treeNode2.ImageIndex = 10;
			treeNode2.Name = "Tree1NetworkDevice";
			treeNode2.SelectedImageIndex = 10;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				treeNode.Text = "USB设备";
				treeNode2.Text = "网络设备";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				treeNode.Text = "USB設備";
				treeNode2.Text = "網絡設備";
			}
			else
			{
				treeNode.Text = "USB Devices";
				treeNode2.Text = "Network Devices";
			}
			TrvDevicesList.Nodes.AddRange(new TreeNode[2] { treeNode, treeNode2 });
		}

		private TreeNode AddTreeNode(TreeNodeCollection tnodes, string parentName, string newName, string newText, int imgidx, object tags)
		{
			TreeNode result = null;
			foreach (TreeNode tnode in tnodes)
			{
				if (tnode.Name == parentName.Trim())
				{
					TreeNode treeNode2 = new TreeNode();
					treeNode2.Text = newText;
					treeNode2.Name = newName;
					treeNode2.ImageIndex = imgidx;
					treeNode2.SelectedImageIndex = imgidx;
					treeNode2.Tag = tags;
					tnode.Nodes.Add(treeNode2);
					return treeNode2;
				}
				result = AddTreeNode(tnode.Nodes, parentName, newName, newText, imgidx, tags);
			}
			return result;
		}

		private void DeleteTreeNode(TreeNodeCollection tnodes, string newName)
		{
			foreach (TreeNode tnode in tnodes)
			{
				if (tnode.Name == newName.Trim())
				{
					tnode.Remove();
					break;
				}
				DeleteTreeNode(tnode.Nodes, newName);
			}
		}

		private void BtnSearchDevice_Click(object sender, EventArgs e)
		{
			if (!Is_Lock_Op)
			{
				Is_Lock_Op = true;
				ToolCfg.UpdateAdjState = true;
				if (DevConnectedType.IsCrossNetSegSerch)
				{
					DpmDevice.EnumAllDevice(DevConnectedType.IsUSBConnect, true, DevConnectedType.IsInternet, is_udp_brocast);
				}
				else
				{
					DpmDevice.EnumAllDevice(DevConnectedType.IsUSBConnect, true, DevConnectedType.IsInternet, false);
				}
				Thread.Sleep(2000);
				DefaultTreeNodes();
				ExpandAllDeviceFound(DpmDevice.DeviceFoudList);
				Application.DoEvents();
				Is_Lock_Op = false;
				IsNeedToUpdateTree = true;
			}
		}

		private void ExpandAllDeviceFound(List<DeviceFindAndCom.DeviceFound> DeviceFoudList)
		{
			int num = 0;
			for (int i = 0; i < DeviceFoudList.Count; i++)
			{
				DeviceFindAndCom.DeviceFound deviceFound = DeviceFoudList[i];
				if (deviceFound.ConnectType == DeviceFindAndCom.DeviceType.USB_LIB)
				{
					ToolCfg.UpdateAdjState = true;
					byte[] array = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
					deviceFound.SendData(deviceFound.DeviceHandle, array, array.Length);
				}
				else if (deviceFound.USB_VID == 47823)
				{
					if ((deviceFound.USB_PID & 0x3F00) == 12800)
					{
						num = 2;
						deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "HM2设备", num, deviceFound);
					}
					else if ((deviceFound.USB_PID & 0x3F00) == 13056)
					{
						num = 3;
						deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "HM3设备", num, deviceFound);
					}
					else if ((deviceFound.USB_PID & 0x3F00) == 13312)
					{
						num = 4;
						deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "HM4设备", num, deviceFound);
					}
					else if ((deviceFound.USB_PID & 0x3F00) == 13568)
					{
						num = 5;
						deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "HM5设备", num, deviceFound);
					}
					else if ((deviceFound.USB_PID & 0x3F00) == 13824)
					{
						num = 5;
						deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "HM5设备", num, deviceFound);
					}
					else if ((deviceFound.USB_PID & 0x3F00) == 14080)
					{
						num = 12;
						deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "H296(未连接)", num, deviceFound);
					}
					else if ((deviceFound.USB_PID & 0x3F00) == 14336)
					{
						num = 7;
						deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "HM7设备(USB)", num, deviceFound);
					}
				}
				else if (deviceFound.ConnectType == DeviceFindAndCom.DeviceType.USB_HID)
				{
					num = 9;
					deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "USB HID Device(0x" + deviceFound.USB_VID.ToString("X4") + ",0x" + deviceFound.USB_PID.ToString("X4") + ")", num, deviceFound);
				}
				else if (deviceFound.ConnectType == DeviceFindAndCom.DeviceType.USB_COM)
				{
					num = 9;
					deviceFound.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", deviceFound.OtherInfoStr.GetHashCode().ToString(), "USB CDC Device(" + deviceFound.NetName + ")", num, deviceFound);
				}
				TrvDevicesList.ExpandAll();
			}
		}

		private void AutoConnectDevice()
		{
			if (!IsNeedAutoConnect)
			{
				return;
			}
			try
			{
				if (DpmDevice.DeviceFoudList.Count == 1)
				{
					if (DpmDevice.DeviceFoudList[0].ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
					{
						ToolCfg.CurrentDevice = DpmDevice.DeviceFoudList[0];
						BtnConnectDevice_Click(null, null);
						DevStateCallback(DevStateDef.DevConnected);
						IsNeedAutoConnect = false;
					}
					return;
				}
				for (int i = 0; i < DpmDevice.DeviceFoudList.Count; i++)
				{
					if (LastDeviceIp == DpmDevice.DeviceFoudList[i].IpAddrStr)
					{
						if (DpmDevice.DeviceFoudList[i].ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
						{
							ToolCfg.CurrentDevice = DpmDevice.DeviceFoudList[i];
							BtnConnectDevice_Click(null, null);
							DevStateCallback(DevStateDef.DevConnected);
							IsNeedAutoConnect = false;
						}
						break;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void BtnConnectDevice_Click(object sender, EventArgs e)
		{
			DeviceFindAndCom.DeviceFound selectedDevice = SelectedDevice;
			if (selectedDevice == null)
			{
				return;
			}
			if (is_clear_before_connect && selectedDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
			{
				DpmDevice.ClearAllConnected();
				DeviceCfgRead.ClearCfgData();
				ToolCfg.UpdateAdjState = true;
			}
			if (selectedDevice.ConnectType == DeviceFindAndCom.DeviceType.USB_HID)
			{
				KEY_Send.SpecialKey(true);
				System.Timers.Timer timer = new System.Timers.Timer(1000.0);
				timer.Elapsed += AutoResetKey_Elapsed;
				timer.Start();
				SendConfigDataEn = false;
			}
			else if (selectedDevice.ConnectType == DeviceFindAndCom.DeviceType.USB_COM)
			{
				if (selectedDevice.NetName != null && selectedDevice.NetName.Contains("COM"))
				{
					byte[] array = new byte[9] { 126, 0, 9, 49, 0, 204, 1, 171, 205 };
					byte[] array2 = new byte[9] { 126, 0, 10, 162, 0, 0, 0, 65, 176 };
					SerialPort serialPort = new SerialPort();
					serialPort.PortName = selectedDevice.NetName;
					serialPort.BaudRate = 9600;
					serialPort.Parity = Parity.None;
					serialPort.NewLine = "\r\n";
					try
					{
						serialPort.Open();
						serialPort.Write(array, 0, array.Length);
						Thread.Sleep(300);
						serialPort.Write(array2, 0, array2.Length);
						Thread.Sleep(100);
						serialPort.Close();
					}
					catch
					{
					}
				}
				SendConfigDataEn = false;
			}
			else if (selectedDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
			{
				if (selectedDevice.DeviceHandle == null)
				{
					byte[] array3 = new byte[4];
					bool flag = false;
					IPAddress address;
					if (IPAddress.TryParse(selectedDevice.IpAddrStr, out address))
					{
						array3 = address.GetAddressBytes();
						flag = true;
					}
					if (selectedDevice.IsCommunicate)
					{
						DpmDevice.StartConnectDeviceByTcp(selectedDevice);
						return;
					}
					if (flag && array3[0] == DpmDevice.IpAddrSeg0 && array3[1] == DpmDevice.IpAddrSeg1 && array3[2] == DpmDevice.IpAddrSeg2)
					{
						ToolCfg.CurrentDevice.IsCommunicate = true;
						selectedDevice.IsCommunicate = true;
						DpmDevice.StartConnectDeviceByTcp(selectedDevice);
						return;
					}
					string text;
					string caption;
					if (ToolCfg.ConfigPath.Contains("ChineseS"))
					{
						text = "当前设备的ip:" + selectedDevice.IpAddrStr + "，与选择的主机ip:" + DpmDevice.IpAddrSeg0 + "." + DpmDevice.IpAddrSeg1 + "." + DpmDevice.IpAddrSeg2 + "." + DpmDevice.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
						caption = "注意";
					}
					else if (ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						text = "當前設備的ip:" + selectedDevice.IpAddrStr + "，與選擇的主機ip:" + DpmDevice.IpAddrSeg0 + "." + DpmDevice.IpAddrSeg1 + "." + DpmDevice.IpAddrSeg2 + "." + DpmDevice.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
						caption = "注意";
					}
					else
					{
						text = "Current device ip:" + selectedDevice.IpAddrStr + ",is not the same segment with host ip:" + DpmDevice.IpAddrSeg0 + "." + DpmDevice.IpAddrSeg1 + "." + DpmDevice.IpAddrSeg2 + "." + DpmDevice.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
						caption = "Pay Attention";
					}
					if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
					{
						ShowDeviceIpSettingForm();
					}
				}
				else
				{
					byte[] array4 = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
					ToolCfg.UpdateAdjState = true;
					selectedDevice.SendData(selectedDevice.DeviceHandle, array4, array4.Length);
				}
			}
			else if (!selectedDevice.IsConnect)
			{
				byte[] array5 = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
				ToolCfg.UpdateAdjState = true;
				selectedDevice.SendData(selectedDevice.DeviceHandle, array5, array5.Length);
			}
		}

		private void ShowDeviceIpSettingForm()
		{
			ToolCfg.DeviceIPSettingPage.ShowDialog();
		}

		public void ChangeIpByBrocast(byte[] dat, int num)
		{
			DpmDevice.SendDataByUdpBrocast(dat);
			if (num == 3)
			{
				Thread.Sleep(300);
				IsNeedAutoConnect = false;
				BtnSearchDevice_Click(null, null);
			}
		}

		private void AutoResetKey_Elapsed(object sender, ElapsedEventArgs e)
		{
			KEY_Send.SpecialKey(false);
			((System.Timers.Timer)sender).Dispose();
		}

		private void BtnDisConnectDevice_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
			{
				if (ToolCfg.CurrentDevice.DeviceHandle != null)
				{
					byte[] array = new byte[8] { 46, 128, 128, 46, 83, 116, 111, 112 };
					ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, array, array.Length);
					DevStateCallback(DevStateDef.DevDisConnnected);
					ToolCfg.CurrentDevice.IsConnect = false;
					Thread.Sleep(500);
				}
			}
			else
			{
				if (ToolCfg.is_RdbTrigExtern_checked)
				{
					DeviceCfgRead.SET_CFG(3u, 0u);
				}
				if (ToolCfg.is_RdbOuputByCOM_checked)
				{
					DeviceCfgRead.SET_CFG(3331u, 3u);
				}
				DeviceCfgRead.SET_CFG(52225u, 0u);
				DeviceConfigDataSend(1026u);
			}
		}

		private void DeviceRestartCheck(uint action)
		{
			if ((action & 2) == 0)
			{
				return;
			}
			IsNeedAutoConnect = true;
			LastDeviceIp = ToolCfg.CurrentDevice.IpAddrStr;
			DateTime now = DateTime.Now;
			while (true)
			{
				Tsb_SearchDevice_Click(null, null);
				if (!IsNeedAutoConnect || DateTime.Now.Subtract(now).TotalSeconds > 10.0)
				{
					break;
				}
				Thread.Sleep(100);
				Application.DoEvents();
			}
		}

		public bool DeviceConfigDataSend(uint action)
		{
			if (SendConfigDataEn && ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
			{
				byte[] array = new byte[4160];
				ProtocolHeader.Header = 2150510208u;
				ProtocolHeader.DataSwitch = 1u;
				ProtocolHeader.ParaAction = action;
				ProtocolHeader.ParaCrc32 = HJ_CRC32.crc32(DeviceParaRead, (int)ParaDataLen);
				byte[] buf = StructToBytes(ProtocolHeader);
				ProtocolHeader.Crc32 = HJ_CRC32.crc32_offset(buf, Marshal.SizeOf(ProtocolHeader), 8);
				buf = StructToBytes(ProtocolHeader);
				buf.CopyTo(array, 0);
				DeviceParaRead.CopyTo(array, 64);
				ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, array, (int)(ParaDataLen + 64));
				UpdateOtherForm = 0;
				DeviceRestartCheck(action);
				return true;
			}
			return false;
		}

		private object BytesToStruct(byte[] bytes, Type strcutType)
		{
			int num = Marshal.SizeOf(strcutType);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.Copy(bytes, 0, intPtr, num);
				return Marshal.PtrToStructure(intPtr, strcutType);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		private byte[] StructToBytes(object obj)
		{
			int num = Marshal.SizeOf(obj);
			byte[] array = new byte[num];
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(obj, intPtr, false);
			Marshal.Copy(intPtr, array, 0, num);
			Marshal.FreeHGlobal(intPtr);
			return array;
		}

		private void BtnOpenCfg_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Application.StartupPath;
			openFileDialog.Filter = "DPM配置文件|*.cfg";
			openFileDialog.FileName = "default";
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			byte[] array = new byte[4];
			byte[] array2 = new byte[4096];
			Stream stream = openFileDialog.OpenFile();
			int num = (int)(stream.Length - 64);
			stream.Read(array, 0, 4);
			stream.Seek(64L, SeekOrigin.Begin);
			stream.Read(array2, 0, num);
			stream.Close();
			uint num2 = HJ_CRC32.crc32(array2, num);
			uint num3 = (uint)(array[0] + (array[1] << 8) + (array[2] << 16) + (array[3] << 24));
			if (num2 == num3)
			{
				if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
				{
					string text;
					string caption;
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						text = "当前设备已经连接，是否把配置导入到设备?\r\n\r\n注:导入成功设备将会重启.";
						caption = "注意";
					}
					else
					{
						text = "The current device is connected. \r\nDo you want to import the configuration to the device.\r\nNote: if the import is successful, the device will restart";
						caption = "Pay Attention";
					}
					if (MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
					{
						array2.CopyTo(DeviceParaRead, 0);
						DeviceConfigDataSend(1024u);
						DeviceConfigDataSend(2u);
						UpdateParaAndDisplay(DeviceParaRead);
					}
				}
				else
				{
					array2.CopyTo(DeviceParaRead, 0);
					UpdateParaAndDisplay(DeviceParaRead);
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("文件格式错误,请打开正确的DPM配置文件。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				MessageBox.Show("File format error, please open the correct DPM configuration file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void BtnSaveCfg_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Application.StartupPath;
			saveFileDialog.Filter = "DPM配置文件|*.cfg";
			saveFileDialog.FileName = "default";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				Stream stream = saveFileDialog.OpenFile();
				byte[] array = new byte[4];
				uint num = HJ_CRC32.crc32(DeviceParaRead, DeviceParaRead.Length);
				array[0] = (byte)(num & 0xFFu);
				array[1] = (byte)((num >> 8) & 0xFFu);
				array[2] = (byte)((num >> 16) & 0xFFu);
				array[3] = (byte)((num >> 24) & 0xFFu);
				stream.Write(array, 0, 4);
				stream.Seek(64L, SeekOrigin.Begin);
				stream.Write(DeviceParaRead, 0, DeviceParaRead.Length);
				stream.Flush();
				stream.Close();
			}
		}

		private void UpdateParaAndDisplay(byte[] para)
		{
			if (UpdateParaAndDisplayCBFunc != null)
			{
				UpdateUiParaInfo.UpdateAdjState = ToolCfg.UpdateAdjState;
				UpdateUiParaInfo.ParaDataLen = (int)ParaDataLen;
				UpdateUiParaInfo.DeviceTypeRecord = (int)ToolCfg.DeviceTypeRecord;
				UpdateUiParaInfo.SensorSize = new Size(ProtocolHeader.SensorWidth, ProtocolHeader.SensorHeight);
				UpdateUiParaInfo.ConnectType = ToolCfg.CurrentDevice.ConnectType;
				UpdateUiParaInfo.DeviceName = ToolCfg.CurrentDevice.NetName;
				UpdateParaAndDisplayCBFunc(UpdateUiParaInfo);
			}
			if (ToolCfg.UpdateAdjState)
			{
				ToolCfg.UpdateAdjState = false;
			}
		}

		public void SetCBFunc(UpdateParaAndDisplayCB DisplayCB, ReadingPage_UpdateDB ReadingPageCB, AdvancedPage_UpdateDB AdvancedPageCB)
		{
			UpdateParaAndDisplayCBFunc = DisplayCB;
			ReadingPage_UpdateCB = ReadingPageCB;
			AdvancedPage_UpdateCB = AdvancedPageCB;
		}

		public byte SetPara(uint paraName, uint paraVal)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				DeviceCfgRead.SET_CFG(paraName, paraVal);
				return DeviceCfgRead.GET_CFG(paraName);
			}
			return 0;
		}

		public byte GetPara(uint paraName)
		{
			return DeviceCfgRead.GET_CFG(paraName);
		}

		public void UpdateLanguageUI()
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				TrvDevicesList.Nodes[0].Text = "USB设备";
				TrvDevicesList.Nodes[1].Text = "网络设备";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				TrvDevicesList.Nodes[0].Text = "USB設備";
				TrvDevicesList.Nodes[1].Text = "網絡設備";
			}
			else
			{
				TrvDevicesList.Nodes[0].Text = "USB Devices";
				TrvDevicesList.Nodes[1].Text = "Network Devices";
			}
		}

		private Polygon[] ByteArrayToPolygonArray(byte[] dat, int len, int zoom)
		{
			int num = len / 4 / 8;
			Polygon[] array = new Polygon[num];
			for (int i = 0; i < num; i++)
			{
				array[i].p0.X = ((dat[32 * i + 3] << 24) | (dat[32 * i + 2] << 16) | (dat[32 * i + 1] << 8) | dat[32 * i]) / zoom;
				array[i].p0.Y = ((dat[32 * i + 4 + 3] << 24) | (dat[32 * i + 4 + 2] << 16) | (dat[32 * i + 4 + 1] << 8) | dat[32 * i + 4]) / zoom;
				array[i].p1.X = ((dat[32 * i + 8 + 3] << 24) | (dat[32 * i + 8 + 2] << 16) | (dat[32 * i + 8 + 1] << 8) | dat[32 * i + 8]) / zoom;
				array[i].p1.Y = ((dat[32 * i + 12 + 3] << 24) | (dat[32 * i + 12 + 2] << 16) | (dat[32 * i + 12 + 1] << 8) | dat[32 * i + 12]) / zoom;
				array[i].p2.X = ((dat[32 * i + 16 + 3] << 24) | (dat[32 * i + 16 + 2] << 16) | (dat[32 * i + 16 + 1] << 8) | dat[32 * i + 16]) / zoom;
				array[i].p2.Y = ((dat[32 * i + 20 + 3] << 24) | (dat[32 * i + 20 + 2] << 16) | (dat[32 * i + 20 + 1] << 8) | dat[32 * i + 20]) / zoom;
				array[i].p3.X = ((dat[32 * i + 24 + 3] << 24) | (dat[32 * i + 24 + 2] << 16) | (dat[32 * i + 24 + 1] << 8) | dat[32 * i + 24]) / zoom;
				array[i].p3.Y = ((dat[32 * i + 28 + 3] << 24) | (dat[32 * i + 28 + 2] << 16) | (dat[32 * i + 28 + 1] << 8) | dat[32 * i + 28]) / zoom;
			}
			return array;
		}

		public static byte[] Bitmap2Byte(Bitmap bitmap)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb))
				{
					Graphics.FromImage(bitmap2).DrawImage(bitmap, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
					bitmap2.Save(memoryStream, ImageFormat.Bmp);
					byte[] array = new byte[memoryStream.Length - 54];
					memoryStream.Seek(54L, SeekOrigin.Begin);
					memoryStream.Read(array, 0, Convert.ToInt32(memoryStream.Length - 54));
					byte[] array2 = new byte[bitmap.Width * bitmap.Height];
					int num = bitmap2.Height - 1;
					int num2 = 0;
					while (num > 0)
					{
						for (int i = 0; i < bitmap2.Width; i++)
						{
							array2[num2] = array[(num * bitmap2.Width + i) * 4];
							num2++;
						}
						num--;
					}
					return array2;
				}
			}
		}

		private static byte[] ConvertBayer8ToBGR(byte[] bayerImgDat, int width, int height)
		{
			byte[] array = new byte[width * height * 3];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < height / 2; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (num2 % 2 == 0)
					{
						num3 = ((i != 0) ? ((bayerImgDat[num2 + width] + bayerImgDat[num2 - width]) / 2) : bayerImgDat[num2 + width]);
						array[num] = (byte)num3;
						num++;
						array[num] = bayerImgDat[num2];
						num++;
						num4 = ((j != 0) ? ((bayerImgDat[num2 + 1] + bayerImgDat[num2 - 1]) / 2) : bayerImgDat[num2 + 1]);
						array[num] = (byte)num4;
						num++;
						num2++;
					}
					else
					{
						num3 = ((j != height / 2 - 1) ? ((bayerImgDat[num2 + 1 + width] + bayerImgDat[num2 - 1 + width]) / 2) : bayerImgDat[num2 - 1 + width]);
						array[num] = (byte)num3;
						num++;
						num5 = (bayerImgDat[num2 - 1] + bayerImgDat[num2 + width]) / 2;
						array[num] = (byte)num5;
						num++;
						array[num] = bayerImgDat[num2];
						num++;
						num2++;
					}
				}
				for (int k = 0; k < width; k++)
				{
					if (num2 % 2 == 0)
					{
						array[num] = bayerImgDat[num2];
						num++;
						num5 = (bayerImgDat[num2 + 1] + bayerImgDat[num2 - width]) / 2;
						array[num] = (byte)num5;
						num++;
						num4 = ((k != 0) ? ((bayerImgDat[num2 - 1 - width] + bayerImgDat[num2 + 1 - width]) / 2) : bayerImgDat[num2 + 1 - width]);
						array[num] = (byte)num4;
						num++;
						num2++;
					}
					else
					{
						num3 = ((k != width - 1) ? ((bayerImgDat[num2 + 1] + bayerImgDat[num2 - 1]) / 2) : bayerImgDat[num2 - 1]);
						array[num] = (byte)num3;
						num++;
						array[num] = bayerImgDat[num2];
						num++;
						num4 = ((i != height / 2 - 1) ? ((bayerImgDat[num2 + width] + bayerImgDat[num2 - width]) / 2) : bayerImgDat[num2 - width]);
						array[num] = (byte)num4;
						num++;
						num2++;
					}
				}
			}
			return array;
		}

		public static Bitmap Byte2Bitmap(byte[] data, int w, int h, PixelFormat pxl, int type)
		{
			switch (type)
			{
			case 1:
				if (data[0] == byte.MaxValue && data[1] == 216)
				{
					MemoryStream stream = new MemoryStream(w * h * 2 + 4096);
					MemoryStream stream2 = new MemoryStream(data);
					Bitmap bitmap2 = new Bitmap(stream2);
					bitmap2.Save(stream, ImageFormat.Jpeg);
					Bitmap bitmap3 = new Bitmap(stream);
					ToolCfg.SaveImg = (Image)bitmap3.Clone();
					return bitmap3;
				}
				return new Bitmap(w, h);
			case 2:
				if (data[0] == 66 && data[1] == 77)
				{
					MemoryStream stream3 = new MemoryStream(w * h * 4 + 4096);
					MemoryStream stream4 = new MemoryStream(data);
					Bitmap bitmap6 = new Bitmap(stream4);
					bitmap6.Save(stream3, ImageFormat.Jpeg);
					Bitmap bitmap7 = new Bitmap(stream3);
					ToolCfg.SaveImg = (Image)bitmap7.Clone();
					return bitmap7;
				}
				return new Bitmap(w, h);
			case 9:
			{
				Bitmap bitmap4 = new Bitmap(w, h, PixelFormat.Format24bppRgb);
				int num4 = 0;
				int num5 = 0;
				Rectangle rect2 = new Rectangle(0, 0, bitmap4.Width, bitmap4.Height);
				BitmapData bitmapData2 = bitmap4.LockBits(rect2, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
				IntPtr scan2 = bitmapData2.Scan0;
				Marshal.Copy(data, 0, scan2, w * h * 3);
				bitmap4.UnlockBits(bitmapData2);
				ToolCfg.SaveImg = (Image)bitmap4.Clone();
				return bitmap4;
			}
			case 7:
			{
				int num6 = 0;
				int num7 = 0;
				byte[] array = ConvertBayer8ToBGR(data, w, h);
				Bitmap bitmap5 = new Bitmap(w, h, PixelFormat.Format32bppArgb);
				BitmapData bitmapData3 = bitmap5.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				IntPtr scan3 = bitmapData3.Scan0;
				for (num6 = 0; num6 < h; num6++)
				{
					for (num7 = 0; num7 < w; num7++)
					{
						Marshal.WriteInt32(scan3 + num6 * bitmapData3.Stride + 4 * num7, -16777216 | (array[(num6 * w + num7) * 3] << 16) | (array[(num6 * w + num7) * 3 + 1] << 8) | array[(num6 * w + num7) * 3 + 2]);
					}
				}
				bitmap5.UnlockBits(bitmapData3);
				return bitmap5;
			}
			default:
			{
				Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
				int num = 0;
				int num2 = 0;
				Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
				BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				IntPtr scan = bitmapData.Scan0;
				foreach (int num3 in data)
				{
					Marshal.WriteInt32(scan + num2 * bitmapData.Stride + 4 * num, -16777216 | (num3 << 16) | (num3 << 8) | num3);
					num++;
					if (num >= bitmap.Width)
					{
						num = 0;
						num2++;
						if (num2 >= bitmap.Height)
						{
							break;
						}
					}
				}
				bitmap.UnlockBits(bitmapData);
				ToolCfg.SaveImg = (Image)bitmap.Clone();
				return bitmap;
			}
			}
		}

		public void Page_Init()
		{
			IniConfigFile iniConfigFile = new IniConfigFile();
			if (!File.Exists(Application.StartupPath + "\\config.ini"))
			{
				StreamWriter streamWriter = File.CreateText(Application.StartupPath + "\\config.ini");
				streamWriter.Close();
				iniConfigFile.Config("config.ini");
				iniConfigFile.set("Language", "ChineseS");
				iniConfigFile.set("RegInfo", "12345678");
				iniConfigFile.set("IpIndex", "0");
				iniConfigFile.set("IPSEG0", "192");
				iniConfigFile.set("IPSEG1", "168");
				iniConfigFile.set("IPSEG2", "1");
				iniConfigFile.set("IPSEG3", "100");
				iniConfigFile.set("ExitAndSave", "1");
				iniConfigFile.set("ManualTrig", "0");
				iniConfigFile.set("QuitDebug", "0");
				iniConfigFile.set("UdpBrocast", "1");
				iniConfigFile.set("BarcodeColor", "255,50,255,50");
				iniConfigFile.set("ClearBeforConnect", "1");
				iniConfigFile.save();
			}
			else
			{
				iniConfigFile.Config("config.ini");
				ToolCfg.ConfigPath = iniConfigFile.get("Language") + ".xml";
				IPADDR_SEG0 = iniConfigFile.get("IPSEG0");
				IPADDR_SEG1 = iniConfigFile.get("IPSEG1");
				IPADDR_SEG2 = iniConfigFile.get("IPSEG2");
				IPADDR_SEG2 = iniConfigFile.get("IPSEG3");
				ToolCfg.is_exit_and_save = iniConfigFile.get("ExitAndSave") != "0";
				ToolCfg.is_manual_trig = iniConfigFile.get("ManualTrig") != "0";
				ToolCfg.is_quit_debug = iniConfigFile.get("QuitDebug") != "0";
				is_udp_brocast = iniConfigFile.get("UdpBrocast") != "0";
				is_clear_before_connect = iniConfigFile.get("ClearBeforConnect") != "0";
				if (!int.TryParse(iniConfigFile.get("IpIndex"), out IpIndexSel))
				{
					IpIndexSel = -1;
					iniConfigFile.set("IpIndex", IpIndexSel.ToString());
					iniConfigFile.save();
				}
			}
			string hostName = Dns.GetHostName();
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobLocalIPSel.Items.Add("使用config.ini文件指定");
			}
			else
			{
				CobLocalIPSel.Items.Add("Use config.ini ip");
			}
			IPAddress[] array = hostAddresses;
			foreach (IPAddress iPAddress in array)
			{
				if (!iPAddress.IsIPv6LinkLocal && iPAddress.ToString().Length < 20)
				{
					CobLocalIPSel.Items.Add(iPAddress.ToString());
				}
			}
			if (IpIndexSel > 0 && IpIndexSel < CobLocalIPSel.Items.Count)
			{
				try
				{
					IPAddress iPAddress2 = IPAddress.Parse(CobLocalIPSel.Items[IpIndexSel].ToString());
					IPADDR_SEG0 = iPAddress2.GetAddressBytes()[0].ToString();
					IPADDR_SEG1 = iPAddress2.GetAddressBytes()[1].ToString();
					IPADDR_SEG2 = iPAddress2.GetAddressBytes()[2].ToString();
					IPADDR_SEG3 = iPAddress2.GetAddressBytes()[3].ToString();
				}
				catch (Exception ex)
				{
					LogRecord.WriteError(ex);
				}
			}
			else
			{
				try
				{
					IpIndexSel = CobLocalIPSel.Items.Count - 1;
					IPAddress iPAddress3 = IPAddress.Parse(CobLocalIPSel.Items[IpIndexSel].ToString());
					IPADDR_SEG0 = iPAddress3.GetAddressBytes()[0].ToString();
					IPADDR_SEG1 = iPAddress3.GetAddressBytes()[1].ToString();
					IPADDR_SEG2 = iPAddress3.GetAddressBytes()[2].ToString();
					IPADDR_SEG3 = iPAddress3.GetAddressBytes()[3].ToString();
				}
				catch (Exception ex2)
				{
					LogRecord.WriteError(ex2);
				}
			}
			CobLocalIPSel.SelectedIndex = IpIndexSel;
			DpmDevice = new DeviceFindAndCom(true, false, true, DeviceDataReceive, DeviceStateChange, int.Parse(IPADDR_SEG0), int.Parse(IPADDR_SEG1), int.Parse(IPADDR_SEG2), int.Parse(IPADDR_SEG3), is_udp_brocast);
			ExpandAllDeviceFound(DpmDevice.DeviceFoudList);
			DeviceCfgRead = new ModuleSetting(DeviceParaRead);
			DeviceCfgRead.FactorySetting();
			ThreadDataProc = new Thread(DataReceiveAndStateUpdate);
			ThreadDataProc.Start();
			IsSelectChange = true;
		}

		private void DeviceDataReceive(ref DeviceFindAndCom.DeviceFound device, byte[] dat, int len)
		{
			if (device == null)
			{
				return;
			}
			device.IsConnect = true;
			ToolCfg.CurrentDevice = device;
			bool flag = false;
			if (len < 4)
			{
				return;
			}
			if (dat[0] == 2 && dat[1] == 0 && dat[2] == 0 && dat[3] == 1 && ToolCfg.FormCMD_Tool != null)
			{
				ToolCfg.FormCMD_Tool.CMD_Ack_CallBack(dat, len);
			}
			if (ToolCfg.ConfigBarcodeCheckForm != null && ((dat[0] == 2 && dat[1] == 0 && dat[2] == 116 && dat[3] == 38) || !is_no_next_data))
			{
				is_no_next_data = true;
				ToolCfg.ConfigBarcodeCheckForm.Data_Ack_CallBack(dat, len, out is_no_next_data);
			}
			if (dat[0] == DataHeaderFlag[0] && dat[1] == DataHeaderFlag[1] && dat[2] == DataHeaderFlag[2] && dat[3] == DataHeaderFlag[3])
			{
				IsDataHeaderReceive = true;
				flag = true;
				if (IsCopyToDataP && !IsReadyDataP)
				{
					Array.Copy(dat, 0, DataReceiveBufP, 0, len);
					DataReceiveLenP = len;
				}
				else if (!IsReadyDataN)
				{
					Array.Copy(dat, 0, DataReceiveBufN, 0, len);
					DataReceiveLenN = len;
				}
			}
			if (dat[len - 4] == DataEndFlag[0] && dat[len - 3] == DataEndFlag[1] && dat[len - 2] == DataEndFlag[2] && dat[len - 1] == DataEndFlag[3])
			{
				bool flag2 = false;
				IsDataHeaderReceive = false;
				if (flag)
				{
					if (IsCopyToDataP)
					{
						IsReadyDataP = true;
					}
					else
					{
						IsReadyDataN = true;
					}
				}
				else if (IsCopyToDataP && DataReceiveLenP + len >= DataReceiveBufP.Length)
				{
					IsReadyDataP = true;
					flag2 = true;
				}
				else if (DataReceiveLenN + len >= DataReceiveBufN.Length)
				{
					IsReadyDataN = true;
					flag2 = true;
				}
				else if (IsCopyToDataP && !IsReadyDataP)
				{
					Array.Copy(dat, 0, DataReceiveBufP, DataReceiveLenP, len);
					DataReceiveLenP += len;
					IsReadyDataP = true;
				}
				else if (!IsReadyDataN)
				{
					Array.Copy(dat, 0, DataReceiveBufN, DataReceiveLenN, len);
					DataReceiveLenN += len;
					IsReadyDataN = true;
				}
				try
				{
					if (!flag2)
					{
						SemDataOk.Release();
					}
					else
					{
						flag2 = false;
					}
				}
				catch (Exception)
				{
				}
				IsCopyToDataP = !IsCopyToDataP;
			}
			else
			{
				if (!IsDataHeaderReceive || flag)
				{
					return;
				}
				try
				{
					if (IsCopyToDataP && DataReceiveLenP + len >= DataReceiveBufP.Length)
					{
						IsDataHeaderReceive = false;
						IsReadyDataP = true;
						SemDataOk.Release();
					}
					else if (DataReceiveLenN + len >= DataReceiveBufN.Length)
					{
						IsDataHeaderReceive = false;
						IsReadyDataN = true;
						SemDataOk.Release();
					}
					else if (IsCopyToDataP && !IsReadyDataP)
					{
						Array.Copy(dat, 0, DataReceiveBufP, DataReceiveLenP, len);
						DataReceiveLenP += len;
					}
					else if (!IsReadyDataN)
					{
						Array.Copy(dat, 0, DataReceiveBufN, DataReceiveLenN, len);
						DataReceiveLenN += len;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		private void DeviceStateChange(ref DeviceFindAndCom.DeviceFound dev, bool newstate)
		{
			DeviceFindAndCom.DeviceFound tmp = dev;
			if (tmp == null)
			{
				return;
			}
			Invoke((MethodInvoker)delegate
			{
				if (newstate)
				{
					if (tmp.ConnectType == DeviceFindAndCom.DeviceType.USB_LIB)
					{
						ToolCfg.UpdateAdjState = true;
						byte[] array = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
						tmp.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", tmp.OtherInfoStr.GetHashCode().ToString(), "Usb Device(...)", 11, tmp);
						tmp.SendData(tmp.DeviceHandle, array, array.Length);
						tmp.USB_PID = 4660;
						for (int i = 0; i < DpmDevice.DeviceFoudList.Count; i++)
						{
							if (DpmDevice.DeviceFoudList[i].ConnectType == DeviceFindAndCom.DeviceType.USB_HID)
							{
								DeleteTreeNode(TrvDevicesList.Nodes, DpmDevice.DeviceFoudList[i].OtherInfoStr.GetHashCode().ToString());
							}
							if (DpmDevice.DeviceFoudList[i].ConnectType == DeviceFindAndCom.DeviceType.USB_COM)
							{
								DeleteTreeNode(TrvDevicesList.Nodes, DpmDevice.DeviceFoudList[i].OtherInfoStr.GetHashCode().ToString());
							}
						}
					}
					else
					{
						int imgidx = (tmp.NetName.Contains("HM7") ? 7 : (tmp.NetName.Contains("HM6") ? 6 : (tmp.NetName.Contains("Tiny") ? 13 : (tmp.NetName.Contains("WL") ? 14 : (tmp.NetName.Contains("HR") ? 15 : (tmp.NetName.Contains("HM2") ? 16 : ((!tmp.NetName.Contains("H296")) ? 9 : 12)))))));
						tmp.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1NetworkDevice", tmp.OtherInfoStr.GetHashCode().ToString(), tmp.NetName + "(" + tmp.IpAddrStr + "-" + tmp.MacStr.Substring(12, 5).Replace(":", "") + ")", imgidx, tmp);
					}
					TrvDevicesList.ExpandAll();
					AutoConnectDevice();
				}
				else
				{
					for (int j = 0; j < DpmDevice.DeviceFoudList.Count; j++)
					{
						if (DpmDevice.DeviceFoudList[j].OtherInfoStr == tmp.OtherInfoStr)
						{
							DeleteTreeNode(TrvDevicesList.Nodes, DpmDevice.DeviceFoudList[j].OtherInfoStr.GetHashCode().ToString());
							tmp.Node = null;
							if (DpmDevice.DeviceFoudList[j] == ToolCfg.CurrentDevice)
							{
								Invoke((MethodInvoker)delegate
								{
									DevStateCallback(DevStateDef.DevDisConnnected);
								});
							}
						}
					}
				}
			});
			dev.Node = tmp.Node;
		}

		private void DataReceiveAndStateUpdate()
		{
			int num = 0;
			while (!ToolCfg.is_stop)
			{
				uint num2 = 0u;
				byte[] array = null;
				SemDataOk.WaitOne();
				if (ToolCfg.is_stop)
				{
					break;
				}
				if (IsReadyDataP)
				{
					array = DataReceiveBufP;
					num = DataReceiveLenP;
				}
				if (IsReadyDataN)
				{
					array = DataReceiveBufN;
					num = DataReceiveLenN;
				}
				if (array != null)
				{
					ProtocolHeader.DataFormByteArray(array);
					BarcodeLen = ProtocolHeader.BarCodeLen;
					BarcodeType = ProtocolHeader.BarCodeType;
					ImageWidth = ProtocolHeader.ImageWidth;
					ImageHeight = ProtocolHeader.ImageHeight;
					ImageSize = ProtocolHeader.ImageSize;
					ImageType = ProtocolHeader.ImageType;
					uint num3 = HJ_CRC32.crc32_offset(array, 64, 8);
					uint paraCrc = ProtocolHeader.ParaCrc32;
					Is_NewLinuxPassword = (ProtocolHeader.ParaAction & 1) == 1;
					if (ProtocolHeader.Crc32 == num3)
					{
						ParaDataLen = ProtocolHeader.ParaDataLen;
						if (ParaDataLen != 256 && ParaDataLen != 4096)
						{
							ParaDataLen = 256u;
						}
						ToolCfg.ParaDataLen = (int)ParaDataLen;
						ToolCfg.SensorWidth = ProtocolHeader.SensorWidth;
						ToolCfg.SensorHeight = ProtocolHeader.SensorHeight;
						num2 += 64;
						if ((ProtocolHeader.DataSwitch & 1) == 1)
						{
							uint num4 = HJ_CRC32.crc32_offset(array, (int)(ParaDataLen + num2), (int)num2);
							if (num4 == paraCrc)
							{
								SendConfigDataEn = true;
								Array.Copy(array, num2, DeviceParaRead, 0L, ParaDataLen);
							}
							num2 += ParaDataLen;
						}
						int num5 = DeviceCfgRead.GET_CFG(7951u);
						if (num5 <= 0 || num5 > 32)
						{
							num5 = 3;
						}
						if ((ProtocolHeader.DataSwitch & 2) == 2)
						{
							Array.Copy(array, num2, BarcodeData, 0L, BarcodeLen);
							num2 += (uint)BarcodeLen;
							if (BarcodeLen == 0)
							{
							}
						}
						if ((ProtocolHeader.DataSwitch & 4) == 4)
						{
							Array.Copy(array, num2, ImageData, 0L, ImageSize);
							num2 += ImageSize;
						}
						if ((ProtocolHeader.DataSwitch & 8) == 8)
						{
							Array.Copy(array, num2, RegionData, 0L, ProtocolHeader.BarCodeNum * 4 * 8);
							num2 += (uint)(ProtocolHeader.BarCodeNum * 4 * 8);
							if (ProtocolHeader.BarCodeNum > 0)
							{
								BarCodeRegion = ByteArrayToPolygonArray(RegionData, ProtocolHeader.BarCodeNum * 4 * 8, num5);
							}
						}
						if ((ProtocolHeader.DataSwitch & 0x10) == 16)
						{
							byte[] array2 = new byte[256];
							Array.Copy(array, num2, array2, 0L, array2.Length);
							num2 += (uint)array2.Length;
							ProtocolExtraData.DataFormByteArray(array2);
							Invoke((MethodInvoker)delegate
							{
								ReadingPagePara.ProtocolExtraData = ProtocolExtraData;
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateDecodeCount);
								if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK && DeviceCfgRead.READ_CFG(155651u, 2u))
								{
									AdvancedPage_UpdateCB(ProtocolExtraData);
								}
							});
						}
						ToolCfg.DeviceTypeRecord = ProtocolHeader.DeviceType;
						if ((ProtocolHeader.ParaAction & 2) == 2)
						{
							Invoke((MethodInvoker)delegate
							{
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AF_OK);
							});
						}
						if ((ProtocolHeader.ParaAction & 4) == 4)
						{
							Invoke((MethodInvoker)delegate
							{
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_START);
							});
						}
						if ((ProtocolHeader.ParaAction & 8) == 8)
						{
							Invoke((MethodInvoker)delegate
							{
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_PROC);
							});
						}
						if ((ProtocolHeader.ParaAction & 0x40) == 64)
						{
							Invoke((MethodInvoker)delegate
							{
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.ADC_PROC);
							});
						}
						if ((ProtocolHeader.ParaAction & 0x10) == 16)
						{
							Invoke((MethodInvoker)delegate
							{
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_OK_END);
							});
							ToolCfg.UpdateAdjState = true;
						}
						if ((ProtocolHeader.ParaAction & 0x20) == 32)
						{
							Invoke((MethodInvoker)delegate
							{
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_NG_END);
							});
							ToolCfg.UpdateAdjState = true;
						}
						if (ToolCfg.UpdateAdjState)
						{
							if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 100)
							{
								Invoke((MethodInvoker)delegate
								{
									if (ToolCfg.CurrentDevice.Node != null)
									{
										string text2;
										if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
										{
											text2 = ProtocolHeader.DeviceID.ToString("X8") + "-" + ToolCfg.CurrentDevice.MacStr.Substring(12, 5).Replace(":", "");
											DevStateCallback(DevStateDef.DevConnected);
										}
										else
										{
											text2 = ProtocolHeader.DeviceID.ToString("X8");
											DevStateCallback(DevStateDef.DevConnected);
										}
										ToolCfg.CurrentDevice.Node.Text = Encoding.Default.GetString(ProtocolHeader.DeviceName).TrimEnd(default(char)) + "(" + text2 + ")";
										if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 9)
										{
											ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType;
										}
										else
										{
											ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType + 3;
										}
										ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
									}
								});
								if (ProtocolHeader.DeviceType == 19 || ProtocolHeader.DeviceType < 6 || (ProtocolHeader.DeviceType >= 9 && ProtocolHeader.DeviceType <= 14))
								{
									ToolCfg.ftp.InitFtpParam("root", "hj168", true);
								}
								else if (Is_NewLinuxPassword)
								{
									ToolCfg.ftp.InitFtpParam("root", "lianghj");
								}
								else
								{
									ToolCfg.ftp.InitFtpParam("root", "");
								}
								if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
								{
									DateTime now = DateTime.Now;
									int num6 = (now.Year - 2000 << 26) | (now.Month << 22) | (now.Day << 17) | (now.Hour << 12) | (now.Minute << 6) | now.Second;
									byte[] obj = new byte[9] { 126, 0, 12, 84, 0, 0, 0, 0, 116 };
									obj[4] = (byte)((uint)num6 & 0xFFu);
									obj[5] = (byte)((num6 & 0xFF00) >> 8);
									obj[6] = (byte)((num6 & 0xFF0000) >> 16);
									obj[7] = (byte)((num6 & 0xFF000000u) >> 24);
									byte[] array3 = obj;
									ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, array3, array3.Length);
								}
								ToolCfg.width_offset = ProtocolHeader.SensorXStart;
								ToolCfg.height_offset = ProtocolHeader.SensorYStart;
							}
							else
							{
								Invoke((MethodInvoker)delegate
								{
									ToolCfg.CurrentDevice.Node.ImageIndex = 0;
									ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
									ToolCfg.CurrentDevice.Node.Text = "USB_Devices(" + ProtocolHeader.DeviceID.ToString("X8") + ")";
								});
								if (DeviceCfgRead.GET_CFG(57599u) == 5)
								{
									ToolCfg.width_offset = 260;
									ToolCfg.height_offset = 80;
								}
								else if (DeviceCfgRead.GET_CFG(57599u) == 4)
								{
									if (DeviceCfgRead.READ_CFG(3074u, 0u))
									{
										ToolCfg.width_offset = 340;
										ToolCfg.height_offset = 80;
									}
									else
									{
										ToolCfg.width_offset = 120;
										ToolCfg.height_offset = 80;
									}
								}
								else if (DeviceCfgRead.GET_CFG(57599u) == 3)
								{
									if (DeviceCfgRead.GET_CFG(61183u) == 5)
									{
										ToolCfg.width_offset = 420;
										ToolCfg.height_offset = 100;
									}
									else
									{
										ToolCfg.width_offset = 380;
										ToolCfg.height_offset = 160;
									}
								}
								else
								{
									ToolCfg.width_offset = 380;
									ToolCfg.height_offset = 160;
								}
							}
						}
						if (IsNeedToUpdateTree)
						{
							IsNeedToUpdateTree = false;
							Invoke((MethodInvoker)delegate
							{
								if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 100)
								{
									if (ToolCfg.CurrentDevice.Node != null)
									{
										string text;
										if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
										{
											text = ProtocolHeader.DeviceID.ToString("X8") + "-" + ToolCfg.CurrentDevice.MacStr.Substring(12, 5).Replace(":", "");
											DevStateCallback(DevStateDef.DevConnected);
										}
										else
										{
											text = ProtocolHeader.DeviceID.ToString("X8");
										}
										ToolCfg.CurrentDevice.Node.Text = Encoding.Default.GetString(ProtocolHeader.DeviceName).TrimEnd(default(char)) + "(" + text + ")";
										if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 9)
										{
											ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType;
										}
										else
										{
											ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType + 3;
										}
										ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
									}
								}
								else if (ToolCfg.CurrentDevice.Node != null)
								{
									ToolCfg.CurrentDevice.Node.ImageIndex = 0;
									ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
									ToolCfg.CurrentDevice.Node.Text = "USB_Devices(" + ProtocolHeader.DeviceID.ToString("X8") + ")";
								}
							});
						}
						if (BarcodeLen > 0)
						{
							DecodeSuccessCount++;
							Invoke((MethodInvoker)delegate
							{
								ReadingPagePara.DecodeTime = ProtocolHeader.DecodeTime;
								ReadingPagePara.BarcodeLen = BarcodeLen;
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateDecodeTimeInfo);
							});
							string barcode_type;
							if (BarcodeType < 5)
							{
								barcode_type = "UPC/EAN";
							}
							else
							{
								barcode_type = Enum.GetName(typeof(AllBarcodeType), BarcodeType);
							}
							string barcode_str = Encoding.Default.GetString(BarcodeData, 0, BarcodeLen);
							BarcodeStr = (string)barcode_str.Clone();
							if (!ToolCfg.is_stop)
							{
								Invoke((MethodInvoker)delegate
								{
									if (barcode_type != null)
									{
										ReadingPagePara.barcode_type = barcode_type;
									}
									else
									{
										ReadingPagePara.barcode_type = "UnKnow";
									}
									if (BarCodeRegion != null)
									{
										ReadingPagePara.BarCodeRegion = BarCodeRegion[0];
									}
									ReadingPagePara.barcode_str = barcode_str;
									ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateBarcodeStr);
								});
							}
						}
						else
						{
							ReadingPagePara.barcode_str = "";
							ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateBarcodeStr);
						}
						if ((FrameCount & 7) == 7)
						{
							long ticks = DateTime.Now.Ticks;
							if (NowTime != 0)
							{
								FrameTime = 10000000.0 / (double)(ticks - NowTime) * 8.0;
								Invoke((MethodInvoker)delegate
								{
									ReadingPagePara.FrameRate = ((long)FrameTime).ToString();
									ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateFrameRate);
								});
							}
							NowTime = ticks;
						}
						TrigCount++;
						FrameCount++;
						string str = "";
						if (DeviceCfgRead.GET_CFG(57599u) == 5)
						{
							str = "VL";
						}
						else if (DeviceCfgRead.GET_CFG(57599u) == 4)
						{
							str = "VO";
						}
						else
						{
							str = "VM";
						}
						if (!ToolCfg.is_stop)
						{
							if (ProtocolHeader.SensorHeight > 960)
							{
								BarcodePen = new Pen(new SolidBrush(BarcodeColor), 5 - num5);
							}
							Invoke((MethodInvoker)delegate
							{
								ReadingPagePara.TrigCount = TrigCount;
								ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateTrigCount);
								UpdateParaAndDisplay(DeviceParaRead);
								TxbDeviceInfo.Text = str + DeviceCfgRead.GET_CFG(58111u);
							});
						}
						if (ImageWidth > 0 && ImageHeight > 0 && num == num2 + 4)
						{
							if ((ProtocolHeader.DataSwitch & 4) != 4)
							{
								Array.Clear(ImageData, 0, ImageData.Length);
							}
							if (DeviceCfgRead.READ_CFG(11272u, 8u))
							{
								int num7 = (DeviceCfgRead.GET_CFG(50431u) * 8 - ToolCfg.width_offset) / num5;
								int num8 = DeviceCfgRead.GET_CFG(50943u) * 8 / num5;
								int num9 = (DeviceCfgRead.GET_CFG(50687u) * 8 - ToolCfg.height_offset) / num5;
								int num10 = DeviceCfgRead.GET_CFG(51199u) * 8 / num5;
								if (num8 == 0)
								{
									num8 = 1;
								}
								if (num10 == 0)
								{
									num10 = 1;
								}
								try
								{
									//Debug Get Imgae
									ImageShow = new Bitmap(ProtocolHeader.SensorWidth / num5, ProtocolHeader.SensorHeight / num5);
									Image image = Byte2Bitmap(ImageData, num8, num10, PixelFormat.Format32bppArgb, ImageType);
									Graphics graphics = Graphics.FromImage(ImageShow);
									graphics.Clear(Color.FromArgb(27, 27, 27));
									graphics.DrawImage(image, num7, num9);
									ToolCfg.TemplateImg = (Image)ImageShow.Clone();
									if (ToolCfg.IsDispNetGrid)
									{
										graphics.DrawLine(new Pen(Color.Blue), 0, num10 / 2, num8 - 1, num10 / 2);
										graphics.DrawLine(new Pen(Color.Blue), num8 / 2, 0, num8 / 2, num10 - 1);
										graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, num10 / 4, num8 - 1, num10 / 4);
										graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, num10 * 3 / 4, num8 - 1, num10 * 3 / 4);
										graphics.DrawLine(new Pen(Color.CornflowerBlue), num8 / 4, 0, num8 / 4, num10 - 1);
										graphics.DrawLine(new Pen(Color.CornflowerBlue), num8 * 3 / 4, 0, num8 * 3 / 4, num10 - 1);
									}
									if (BarcodeLen > 0 && BarCodeRegion != null)
									{
										for (int i = 0; i < BarCodeRegion.Length; i++)
										{
											Point[] array4 = BarCodeRegion[i].ToPointArray();
											for (int j = 0; j < array4.Length; j++)
											{
												array4[j].X += num7;
												array4[j].Y += num9;
											}
											graphics.SmoothingMode = SmoothingMode.AntiAlias;
											graphics.DrawLines(BarcodePen, array4);
										}
									}
									if (!ToolCfg.is_stop)
									{
										Invoke((MethodInvoker)delegate
										{
											ReadingPagePara.ImageShow = ImageShow;
											ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateImage);
										});
									}
									graphics.Dispose();
								}
								catch (Exception)
								{
								}
							}
							else
							{
								ImageShow = Byte2Bitmap(ImageData, ImageWidth, ImageHeight, PixelFormat.Format32bppArgb, ImageType);
								ToolCfg.TemplateImg = (Image)ImageShow.Clone();
								Graphics graphics2 = Graphics.FromImage(ImageShow);
								if (ToolCfg.IsDispNetGrid)
								{
									int imageWidth = ImageWidth;
									int imageHeight = ImageHeight;
									graphics2.DrawLine(new Pen(Color.Blue), 0, imageHeight / 2, imageWidth - 1, imageHeight / 2);
									graphics2.DrawLine(new Pen(Color.Blue), imageWidth / 2, 0, imageWidth / 2, imageHeight - 1);
									graphics2.DrawLine(new Pen(Color.CornflowerBlue), 0, imageHeight / 4, imageWidth - 1, imageHeight / 4);
									graphics2.DrawLine(new Pen(Color.CornflowerBlue), 0, imageHeight * 3 / 4, imageWidth - 1, imageHeight * 3 / 4);
									graphics2.DrawLine(new Pen(Color.CornflowerBlue), imageWidth / 4, 0, imageWidth / 4, imageHeight - 1);
									graphics2.DrawLine(new Pen(Color.CornflowerBlue), imageWidth * 3 / 4, 0, imageWidth * 3 / 4, imageHeight - 1);
								}
								if (BarcodeLen > 0 && BarCodeRegion != null)
								{
									graphics2.SmoothingMode = SmoothingMode.AntiAlias;
									try
									{
										for (int k = 0; k < BarCodeRegion.Length; k++)
										{
											graphics2.DrawLines(BarcodePen, BarCodeRegion[k].ToPointArray());
										}
									}
									catch
									{
									}
								}
								if (base.IsHandleCreated && !ToolCfg.is_stop)
								{
									Invoke((MethodInvoker)delegate
									{
										ReadingPagePara.ImageShow = ImageShow;
										ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateImage);
									});
								}
								graphics2.Dispose();
							}
						}
					}
				}
				if (IsReadyDataP)
				{
					IsReadyDataP = false;
				}
				if (IsReadyDataN)
				{
					IsReadyDataN = false;
				}
			}
			ThreadDataProc.DisableComObjectEagerCleanup();
		}

		public void Tsb_SearchDevice_Click(object sender, EventArgs e)
		{
			IsNeedAutoConnect = true;
			BtnSearchDevice_Click(null, null);
		}

		public void Tsb_ConnectDevice_Click(object sender, EventArgs e)
		{
			BtnConnectDevice_Click(null, null);
		}

		public void Tsb_DisconnectDevice_Click(object sender, EventArgs e)
		{
			BtnDisConnectDevice_Click(null, null);
		}

		public void Tsb_OpenCfg_Click(object sender, EventArgs e)
		{
			BtnOpenCfg_Click(null, null);
		}

		public void Tsb_SaveCfg_Click(object sender, EventArgs e)
		{
			BtnSaveCfg_Click(null, null);
		}

		private void CobLocalIPSel_DropDown(object sender, EventArgs e)
		{
			string hostName = Dns.GetHostName();
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			CobLocalIPSel.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobLocalIPSel.Items.Add("使用config.ini文件指定");
			}
			else
			{
				CobLocalIPSel.Items.Add("Use config.ini ip");
			}
			IPAddress[] array = hostAddresses;
			foreach (IPAddress iPAddress in array)
			{
				if (!iPAddress.IsIPv6LinkLocal && iPAddress.ToString().Length < 20)
				{
					CobLocalIPSel.Items.Add(iPAddress.ToString());
				}
			}
		}

		private void CobLocalIPSel_DropDownClosed(object sender, EventArgs e)
		{
			if (CobLocalIPSel.SelectedIndex < 0)
			{
				CobLocalIPSel.SelectedIndex = CobLocalIPSel.Items.Count - 1;
			}
		}

		private void CobLocalIPSel_MouseLeave(object sender, EventArgs e)
		{
		}

		public void BtnSaveCurrentCfg_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
			{
				if (ToolCfg.is_RdbTrigExtern_checked)
				{
					DeviceCfgRead.SET_CFG(3u, 0u);
				}
				if (ToolCfg.is_RdbOuputByCOM_checked)
				{
					DeviceCfgRead.SET_CFG(3331u, 3u);
				}
				DeviceConfigDataSend(1024u);
				FormSaveTips formSaveTips = new FormSaveTips();
				formSaveTips.ac_time = 800;
				formSaveTips.ShowDialog();
			}
		}

		private void Tsm_EditDeviceIp_Click(object sender, EventArgs e)
		{
			ShowDeviceIpSettingForm();
		}

		private void Tsm_ConnectDevice_Click(object sender, EventArgs e)
		{
			BtnConnectDevice_Click(null, null);
		}

		private void Tsm_DisconnectDevice_Click(object sender, EventArgs e)
		{
			BtnDisConnectDevice_Click(null, null);
		}

		public void Tsm_RestartDevice_Click(object sender, EventArgs e)
		{
			DeviceConfigDataSend(2u);
		}

		private void Tsm_SaveDeviceCfg_Click(object sender, EventArgs e)
		{
			BtnSaveCurrentCfg_Click(null, null);
		}

		private void CobLocalIPSel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!IsSelectChange)
			{
				return;
			}
			IniConfigFile iniConfigFile = new IniConfigFile();
			iniConfigFile.Config("config.ini");
			iniConfigFile.set("IpIndex", CobLocalIPSel.SelectedIndex.ToString());
			iniConfigFile.save();
			if (CobLocalIPSel.SelectedIndex > 0)
			{
				IPAddress address;
				if (IPAddress.TryParse(CobLocalIPSel.Items[CobLocalIPSel.SelectedIndex].ToString(), out address))
				{
					if (DpmDevice != null)
					{
						DpmDevice.IpAddrSeg0 = address.GetAddressBytes()[0];
						DpmDevice.IpAddrSeg1 = address.GetAddressBytes()[1];
						DpmDevice.IpAddrSeg2 = address.GetAddressBytes()[2];
						DpmDevice.IpAddrSeg3 = address.GetAddressBytes()[3];
						ToolCfg.IpAddrSeg0 = DpmDevice.IpAddrSeg0;
						ToolCfg.IpAddrSeg1 = DpmDevice.IpAddrSeg1;
						ToolCfg.IpAddrSeg2 = DpmDevice.IpAddrSeg2;
						ToolCfg.IpAddrSeg3 = DpmDevice.IpAddrSeg3;
					}
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("当前IP地址选择无效", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("The current IP address selection is invalid", "Tips", MessageBoxButtons.OK);
				}
				return;
			}
			try
			{
				DpmDevice.IpAddrSeg0 = int.Parse(iniConfigFile.get("IPSEG0"));
				DpmDevice.IpAddrSeg1 = int.Parse(iniConfigFile.get("IPSEG1"));
				DpmDevice.IpAddrSeg2 = int.Parse(iniConfigFile.get("IPSEG2"));
				DpmDevice.IpAddrSeg3 = int.Parse(iniConfigFile.get("IPSEG3"));
				ToolCfg.IpAddrSeg0 = DpmDevice.IpAddrSeg0;
				ToolCfg.IpAddrSeg1 = DpmDevice.IpAddrSeg1;
				ToolCfg.IpAddrSeg2 = DpmDevice.IpAddrSeg2;
				ToolCfg.IpAddrSeg3 = DpmDevice.IpAddrSeg3;
			}
			catch
			{
			}
		}

		private void TrvDevicesList_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selectedNode = TrvDevicesList.SelectedNode;
			if (selectedNode.Tag != null)
			{
				DeviceFindAndCom.DeviceFound deviceFound = (DeviceFindAndCom.DeviceFound)selectedNode.Tag;
				if (deviceFound.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
				{
					TxbDeviceAddr.Text = deviceFound.OtherInfoStr;
				}
				else
				{
					TxbDeviceAddr.Text = "";
				}
				if (deviceFound.IsConnect)
				{
					DevStateCallback(DevStateDef.DevConnected);
				}
				else
				{
					DevStateCallback(DevStateDef.DevDisConnnected);
				}
				SelectedDevice = deviceFound;
				ToolCfg.DeviceIPSettingPage.SelectedDevIp = SelectedDevice.IpAddrStr;
				ToolCfg.DeviceIPSettingPage.SelectedDevMac = SelectedDevice.MacStr;
			}
			else
			{
				DevStateCallback(DevStateDef.BothDisenable);
			}
		}

		private void TrvDevicesList_DoubleClick(object sender, EventArgs e)
		{
			BtnConnectDevice_Click(null, null);
		}

		private void Cms_RightDeviceClick_Opening(object sender, CancelEventArgs e)
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				Tsm_ConnectDevice.Text = "连接设备";
				Tsm_DisconnectDevice.Text = "断开设备";
				Tsm_RestartDevice.Text = "重启设备(不保存)";
				Tsm_SaveDeviceCfg.Text = "保存配置到设备";
				Tsm_EditDeviceIp.Text = "修改IP地址";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				Tsm_ConnectDevice.Text = "連接設備";
				Tsm_DisconnectDevice.Text = "斷開設備";
				Tsm_RestartDevice.Text = "重啟設備(不保存)";
				Tsm_SaveDeviceCfg.Text = "保存配置到設備";
				Tsm_EditDeviceIp.Text = "修改IP地址";
			}
			else
			{
				Tsm_ConnectDevice.Text = "Connect Device ";
				Tsm_DisconnectDevice.Text = "Disconnect Device ";
				Tsm_RestartDevice.Text = "Restart Device (not saved)";
				Tsm_SaveDeviceCfg.Text = "Save Configuration to Device ";
				Tsm_EditDeviceIp.Text = "Modify IP Address ";
			}
		}

		private void TrvDevicesList_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}
			Point pt = new Point(e.X, e.Y);
			TreeNode nodeAt = TrvDevicesList.GetNodeAt(pt);
			if (nodeAt != null)
			{
				if (nodeAt.Parent != null && nodeAt.Parent.Name == "Tree1NetworkDevice")
				{
					nodeAt.ContextMenuStrip = Cms_RightDeviceClick;
				}
				TrvDevicesList.SelectedNode = nodeAt;
			}
		}

		private void BtnCaptrueImageSave_Click(object sender, EventArgs e)
		{
			DeviceConfigDataSend(65536u);
		}

		public void BtnRecoverFactory_Click(object sender, EventArgs e)
		{
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "确定要恢复出厂设置?";
				caption = "提示";
			}
			else
			{
				text = "Is sure to recovery factory setting?";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				DeviceConfigDataSend(32u);
			}
		}

		private void BtnSaveUserSetting_Click(object sender, EventArgs e)
		{
			DeviceConfigDataSend(8u);
			Thread.Sleep(200);
			DeviceConfigDataSend(8u);
		}

		private void BtnRecoverUserSetting_Click(object sender, EventArgs e)
		{
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "确定要恢复用户设置";
				caption = "提示";
			}
			else
			{
				text = "Is sure to recovery user setting?";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				DeviceConfigDataSend(16u);
			}
		}

		public void FormClosingDo(object sender, FormClosingEventArgs e)
		{
			if (ToolCfg.is_exit_and_save && ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
			{
				string text;
				string caption;
				if (ToolCfg.ConfigPath.Contains("ChineseS"))
				{
					text = "退出前，是否要保存配置到设备?";
					caption = "注意";
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					text = "退出前，是否要保存配置到設備?";
					caption = "注意";
				}
				else
				{
					text = "Before exit ,Save config to device?";
					caption = "Pay Attention";
				}
				DialogResult dialogResult = MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel);
				if (dialogResult == DialogResult.Yes)
				{
					if (ToolCfg.is_RdbTrigExtern_checked)
					{
						DeviceCfgRead.SET_CFG(3u, 0u);
					}
					if (ToolCfg.is_manual_trig)
					{
						DeviceCfgRead.SET_CFG(3u, 0u);
					}
					if (ToolCfg.is_RdbOuputByCOM_checked)
					{
						DeviceCfgRead.SET_CFG(3331u, 3u);
					}
					if (ToolCfg.is_quit_debug && ToolCfg.CurrentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK)
					{
						DeviceCfgRead.SET_CFG(52225u, 0u);
						DeviceConfigDataSend(1026u);
					}
					else
					{
						DeviceConfigDataSend(1024u);
					}
					FormSaveTips formSaveTips = new FormSaveTips();
					formSaveTips.ac_time = 3000;
					formSaveTips.ShowDialog();
				}
				if (dialogResult == DialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}
			if (!e.Cancel)
			{
				ToolCfg.is_stop = true;
				Thread.Sleep(500);
				try
				{
					SemDataOk.Release();
				}
				catch (Exception)
				{
				}
				switch (e.CloseReason)
				{
				case CloseReason.ApplicationExitCall:
					e.Cancel = true;
					break;
				case CloseReason.FormOwnerClosing:
					e.Cancel = true;
					break;
				case CloseReason.MdiFormClosing:
					e.Cancel = true;
					break;
				case CloseReason.TaskManagerClosing:
					e.Cancel = true;
					break;
				case CloseReason.UserClosing:
					e.Cancel = true;
					break;
				case CloseReason.WindowsShutDown:
					e.Cancel = false;
					break;
				}
				LogRecord.SaveLogFile();
				DpmDevice.CloseDevice();
				DpmDevice.DestroyDevice();
				Thread.Sleep(500);
				Application.ExitThread();
			}
		}

		public void InstallUSBDrvTool(object sender, EventArgs e)
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
			if (windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
			{
				if (DriverInstallForm == null || DriverInstallForm.IsDisposed)
				{
					DriverInstallForm = new DriverInstall();
					DriverInstallForm.ShowDialog();
				}
				else
				{
					DriverInstallForm.ShowDialog();
				}
				return;
			}
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "当前运行用户不是管理员，稍后软件会重启，弹出管理员身份申请请点击ok.";
				caption = "注意";
			}
			else
			{
				text = "The current running user is not an administrator. The software will restart later in administrator. Please click OK";
				caption = "Pay Attention";
			}
			try
			{
				if (MessageBox.Show(text, caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo();
					processStartInfo.FileName = Application.ExecutablePath;
					processStartInfo.Arguments = "";
					processStartInfo.Verb = "runas";
					Process.Start(processStartInfo);
					try
					{
						SemDataOk.Release();
					}
					catch (Exception)
					{
					}
					ToolCfg.is_stop = true;
					LogRecord.SaveLogFile();
					DpmDevice.CloseDevice();
					DpmDevice.DestroyDevice();
					Application.ExitThread();
				}
			}
			catch (Exception)
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("USB设备", 11, 11);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("网络设备", 10, 10);
            this.CobLocalIPSel = new System.Windows.Forms.ComboBox();
            this.LabLocalIPSel = new System.Windows.Forms.Label();
            this.Cms_RightDeviceClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Tsm_ConnectDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.Tsm_DisconnectDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.Tsm_RestartDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.Tsm_SaveDeviceCfg = new System.Windows.Forms.ToolStripMenuItem();
            this.Tsm_EditDeviceIp = new System.Windows.Forms.ToolStripMenuItem();
            this.PanDeviceInfo = new System.Windows.Forms.Panel();
            this.TxbDeviceInfo = new System.Windows.Forms.TextBox();
            this.LabDeviceInfo = new System.Windows.Forms.Label();
            this.TxbDeviceAddr = new System.Windows.Forms.TextBox();
            this.GrbDeviceOps = new System.Windows.Forms.GroupBox();
            this.BtnRecoverUserSetting = new System.Windows.Forms.Button();
            this.BtnSaveUserSetting = new System.Windows.Forms.Button();
            this.BtnCaptrueImageSave = new System.Windows.Forms.Button();
            this.BtnRecoverFactory = new System.Windows.Forms.Button();
            this.LabDeviceAddr = new System.Windows.Forms.Label();
            this.GrbDevicesList = new System.Windows.Forms.GroupBox();
            this.TrvDevicesList = new System.Windows.Forms.TreeView();
            this.DeviceImageList = new System.Windows.Forms.ImageList(this.components);
            this.Cms_RightDeviceClick.SuspendLayout();
            this.PanDeviceInfo.SuspendLayout();
            this.GrbDeviceOps.SuspendLayout();
            this.GrbDevicesList.SuspendLayout();
            this.SuspendLayout();
            // 
            // CobLocalIPSel
            // 
            this.CobLocalIPSel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CobLocalIPSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CobLocalIPSel.FormattingEnabled = true;
            this.CobLocalIPSel.Location = new System.Drawing.Point(91, 12);
            this.CobLocalIPSel.Name = "CobLocalIPSel";
            this.CobLocalIPSel.Size = new System.Drawing.Size(234, 22);
            this.CobLocalIPSel.TabIndex = 20;
            this.CobLocalIPSel.DropDown += new System.EventHandler(this.CobLocalIPSel_DropDown);
            this.CobLocalIPSel.SelectedIndexChanged += new System.EventHandler(this.CobLocalIPSel_SelectedIndexChanged);
            this.CobLocalIPSel.DropDownClosed += new System.EventHandler(this.CobLocalIPSel_DropDownClosed);
            this.CobLocalIPSel.MouseLeave += new System.EventHandler(this.CobLocalIPSel_MouseLeave);
            // 
            // LabLocalIPSel
            // 
            this.LabLocalIPSel.AutoSize = true;
            this.LabLocalIPSel.Location = new System.Drawing.Point(5, 15);
            this.LabLocalIPSel.Name = "LabLocalIPSel";
            this.LabLocalIPSel.Size = new System.Drawing.Size(91, 14);
            this.LabLocalIPSel.TabIndex = 19;
            this.LabLocalIPSel.Text = "本地IP选择：";
            // 
            // Cms_RightDeviceClick
            // 
            this.Cms_RightDeviceClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tsm_ConnectDevice,
            this.Tsm_DisconnectDevice,
            this.Tsm_RestartDevice,
            this.Tsm_SaveDeviceCfg,
            this.Tsm_EditDeviceIp});
            this.Cms_RightDeviceClick.Name = "CMS_DevideRightKey";
            this.Cms_RightDeviceClick.Size = new System.Drawing.Size(169, 114);
            this.Cms_RightDeviceClick.Opening += new System.ComponentModel.CancelEventHandler(this.Cms_RightDeviceClick_Opening);
            // 
            // Tsm_ConnectDevice
            // 
            this.Tsm_ConnectDevice.Name = "Tsm_ConnectDevice";
            this.Tsm_ConnectDevice.Size = new System.Drawing.Size(168, 22);
            this.Tsm_ConnectDevice.Text = "连接设备";
            this.Tsm_ConnectDevice.Click += new System.EventHandler(this.Tsm_ConnectDevice_Click);
            // 
            // Tsm_DisconnectDevice
            // 
            this.Tsm_DisconnectDevice.Name = "Tsm_DisconnectDevice";
            this.Tsm_DisconnectDevice.Size = new System.Drawing.Size(168, 22);
            this.Tsm_DisconnectDevice.Text = "断开设备";
            this.Tsm_DisconnectDevice.Click += new System.EventHandler(this.Tsm_DisconnectDevice_Click);
            // 
            // Tsm_RestartDevice
            // 
            this.Tsm_RestartDevice.Name = "Tsm_RestartDevice";
            this.Tsm_RestartDevice.Size = new System.Drawing.Size(168, 22);
            this.Tsm_RestartDevice.Text = "重启设备(不保存)";
            this.Tsm_RestartDevice.Click += new System.EventHandler(this.Tsm_RestartDevice_Click);
            // 
            // Tsm_SaveDeviceCfg
            // 
            this.Tsm_SaveDeviceCfg.Name = "Tsm_SaveDeviceCfg";
            this.Tsm_SaveDeviceCfg.Size = new System.Drawing.Size(168, 22);
            this.Tsm_SaveDeviceCfg.Text = "保存配置到设备";
            this.Tsm_SaveDeviceCfg.Click += new System.EventHandler(this.Tsm_SaveDeviceCfg_Click);
            // 
            // Tsm_EditDeviceIp
            // 
            this.Tsm_EditDeviceIp.Name = "Tsm_EditDeviceIp";
            this.Tsm_EditDeviceIp.Size = new System.Drawing.Size(168, 22);
            this.Tsm_EditDeviceIp.Text = "修改IP地址";
            this.Tsm_EditDeviceIp.Click += new System.EventHandler(this.Tsm_EditDeviceIp_Click);
            // 
            // PanDeviceInfo
            // 
            this.PanDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanDeviceInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanDeviceInfo.Controls.Add(this.TxbDeviceInfo);
            this.PanDeviceInfo.Controls.Add(this.LabDeviceInfo);
            this.PanDeviceInfo.Controls.Add(this.TxbDeviceAddr);
            this.PanDeviceInfo.Controls.Add(this.GrbDeviceOps);
            this.PanDeviceInfo.Controls.Add(this.LabDeviceAddr);
            this.PanDeviceInfo.Location = new System.Drawing.Point(5, 458);
            this.PanDeviceInfo.Name = "PanDeviceInfo";
            this.PanDeviceInfo.Size = new System.Drawing.Size(326, 219);
            this.PanDeviceInfo.TabIndex = 22;
            // 
            // TxbDeviceInfo
            // 
            this.TxbDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxbDeviceInfo.Location = new System.Drawing.Point(9, 76);
            this.TxbDeviceInfo.Name = "TxbDeviceInfo";
            this.TxbDeviceInfo.ReadOnly = true;
            this.TxbDeviceInfo.Size = new System.Drawing.Size(307, 23);
            this.TxbDeviceInfo.TabIndex = 9;
            // 
            // LabDeviceInfo
            // 
            this.LabDeviceInfo.AutoSize = true;
            this.LabDeviceInfo.Location = new System.Drawing.Point(7, 59);
            this.LabDeviceInfo.Name = "LabDeviceInfo";
            this.LabDeviceInfo.Size = new System.Drawing.Size(63, 14);
            this.LabDeviceInfo.TabIndex = 8;
            this.LabDeviceInfo.Text = "设备信息";
            // 
            // TxbDeviceAddr
            // 
            this.TxbDeviceAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxbDeviceAddr.Location = new System.Drawing.Point(9, 33);
            this.TxbDeviceAddr.Name = "TxbDeviceAddr";
            this.TxbDeviceAddr.ReadOnly = true;
            this.TxbDeviceAddr.Size = new System.Drawing.Size(307, 23);
            this.TxbDeviceAddr.TabIndex = 7;
            // 
            // GrbDeviceOps
            // 
            this.GrbDeviceOps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbDeviceOps.Controls.Add(this.BtnRecoverUserSetting);
            this.GrbDeviceOps.Controls.Add(this.BtnSaveUserSetting);
            this.GrbDeviceOps.Controls.Add(this.BtnCaptrueImageSave);
            this.GrbDeviceOps.Controls.Add(this.BtnRecoverFactory);
            this.GrbDeviceOps.Location = new System.Drawing.Point(4, 105);
            this.GrbDeviceOps.Name = "GrbDeviceOps";
            this.GrbDeviceOps.Size = new System.Drawing.Size(312, 109);
            this.GrbDeviceOps.TabIndex = 6;
            this.GrbDeviceOps.TabStop = false;
            this.GrbDeviceOps.Text = "设备操作";
            // 
            // BtnRecoverUserSetting
            // 
            this.BtnRecoverUserSetting.Location = new System.Drawing.Point(161, 64);
            this.BtnRecoverUserSetting.Name = "BtnRecoverUserSetting";
            this.BtnRecoverUserSetting.Size = new System.Drawing.Size(120, 36);
            this.BtnRecoverUserSetting.TabIndex = 2;
            this.BtnRecoverUserSetting.Text = "恢复用户设置";
            this.BtnRecoverUserSetting.UseVisualStyleBackColor = true;
            this.BtnRecoverUserSetting.Click += new System.EventHandler(this.BtnRecoverUserSetting_Click);
            // 
            // BtnSaveUserSetting
            // 
            this.BtnSaveUserSetting.Location = new System.Drawing.Point(16, 64);
            this.BtnSaveUserSetting.Name = "BtnSaveUserSetting";
            this.BtnSaveUserSetting.Size = new System.Drawing.Size(120, 36);
            this.BtnSaveUserSetting.TabIndex = 1;
            this.BtnSaveUserSetting.Text = "保存用户设置";
            this.BtnSaveUserSetting.UseVisualStyleBackColor = true;
            this.BtnSaveUserSetting.Click += new System.EventHandler(this.BtnSaveUserSetting_Click);
            // 
            // BtnCaptrueImageSave
            // 
            this.BtnCaptrueImageSave.Location = new System.Drawing.Point(161, 28);
            this.BtnCaptrueImageSave.Name = "BtnCaptrueImageSave";
            this.BtnCaptrueImageSave.Size = new System.Drawing.Size(120, 36);
            this.BtnCaptrueImageSave.TabIndex = 9;
            this.BtnCaptrueImageSave.Text = "存一张图";
            this.BtnCaptrueImageSave.UseVisualStyleBackColor = true;
            this.BtnCaptrueImageSave.Click += new System.EventHandler(this.BtnCaptrueImageSave_Click);
            // 
            // BtnRecoverFactory
            // 
            this.BtnRecoverFactory.Location = new System.Drawing.Point(16, 28);
            this.BtnRecoverFactory.Name = "BtnRecoverFactory";
            this.BtnRecoverFactory.Size = new System.Drawing.Size(120, 36);
            this.BtnRecoverFactory.TabIndex = 0;
            this.BtnRecoverFactory.Text = "恢复出厂设置";
            this.BtnRecoverFactory.UseVisualStyleBackColor = true;
            this.BtnRecoverFactory.Click += new System.EventHandler(this.BtnRecoverFactory_Click);
            // 
            // LabDeviceAddr
            // 
            this.LabDeviceAddr.AutoSize = true;
            this.LabDeviceAddr.Location = new System.Drawing.Point(7, 12);
            this.LabDeviceAddr.Name = "LabDeviceAddr";
            this.LabDeviceAddr.Size = new System.Drawing.Size(63, 14);
            this.LabDeviceAddr.TabIndex = 5;
            this.LabDeviceAddr.Text = "设备位置";
            // 
            // GrbDevicesList
            // 
            this.GrbDevicesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbDevicesList.Controls.Add(this.TrvDevicesList);
            this.GrbDevicesList.Location = new System.Drawing.Point(8, 40);
            this.GrbDevicesList.Name = "GrbDevicesList";
            this.GrbDevicesList.Size = new System.Drawing.Size(323, 412);
            this.GrbDevicesList.TabIndex = 23;
            this.GrbDevicesList.TabStop = false;
            this.GrbDevicesList.Text = "设备列表";
            // 
            // TrvDevicesList
            // 
            this.TrvDevicesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrvDevicesList.ImageIndex = 0;
            this.TrvDevicesList.ImageList = this.DeviceImageList;
            this.TrvDevicesList.Location = new System.Drawing.Point(6, 17);
            this.TrvDevicesList.Name = "TrvDevicesList";
            treeNode1.ImageIndex = 11;
            treeNode1.Name = "Tree1UsbDevice";
            treeNode1.SelectedImageIndex = 11;
            treeNode1.Text = "USB设备";
            treeNode2.ImageIndex = 10;
            treeNode2.Name = "Tree1NetworkDevice";
            treeNode2.SelectedImageIndex = 10;
            treeNode2.Text = "网络设备";
            this.TrvDevicesList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.TrvDevicesList.SelectedImageIndex = 0;
            this.TrvDevicesList.Size = new System.Drawing.Size(311, 389);
            this.TrvDevicesList.TabIndex = 3;
            this.TrvDevicesList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TrvDevicesList_AfterSelect);
            this.TrvDevicesList.DoubleClick += new System.EventHandler(this.TrvDevicesList_DoubleClick);
            this.TrvDevicesList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrvDevicesList_MouseDown);
            // 
            // DeviceImageList
            // 
            this.DeviceImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.DeviceImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.DeviceImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // DeviceConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(334, 689);
            this.Controls.Add(this.GrbDevicesList);
            this.Controls.Add(this.PanDeviceInfo);
            this.Controls.Add(this.CobLocalIPSel);
            this.Controls.Add(this.LabLocalIPSel);
            this.Font = new System.Drawing.Font("SimSun", 10.5F);
            this.Name = "DeviceConnectForm";
            this.Text = "设备列表";
            this.Load += new System.EventHandler(this.DevideConnectForm_Load);
            this.Cms_RightDeviceClick.ResumeLayout(false);
            this.PanDeviceInfo.ResumeLayout(false);
            this.PanDeviceInfo.PerformLayout();
            this.GrbDeviceOps.ResumeLayout(false);
            this.GrbDevicesList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
