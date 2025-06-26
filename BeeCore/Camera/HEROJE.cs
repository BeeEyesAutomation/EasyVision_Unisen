
//using Heroje_Debug_Tool.BaseClass;
//using Heroje_Debug_Tool.SubForm;
//using HJ_CRC32_n;
//using IniConfigFile_n;
//using KEY_Send_n;
//using ModuleSetting_n;
using BeeCore.Funtion;
using Heroje_Debug_Tool.SubForm;
using HJ_CRC32_n;
using KEY_Send_n;
using LibUsbDotNet.DeviceNotify;
using ModuleSetting_n;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XFeatures2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using static BeeCore.DeviceFindAndCom;
using DeviceType = BeeCore.DeviceFindAndCom.DeviceType;
using Point = System.Drawing.Point;
using Size = OpenCvSharp.Size;
using Timer = System.Timers.Timer;
//using System.Windows.Forms.DataVisualization.Charting;
namespace BeeCore
{
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

    public class HEROJE
    {
        private static byte[] DeviceParaRead = new byte[4096];

        private static  DevStateCB DevStateCallback;
        private static ModuleSetting DeviceCfgRead;
        private static Semaphore SemDataOk = new Semaphore(0, 2);
        private static bool is_no_next_data = false;
        private static byte[] DataHeaderFlag = new byte[4] { 128, 46, 46, 128 };
        private static byte[] DataEndFlag = new byte[4] { 46, 128, 128, 46 };
        private static byte[] DataReceiveBufP = new byte[37748736];
        private static byte[] DataReceiveBufN = new byte[37748736];
        private static  byte[] RegionData = new byte[3200];
        public static Polygon[] BarCodeRegion;
        private static uint ParaDataLen = 0u;
        private static uint DeviceTypeRecord = 0u;
        private static int BarcodeLen = 0;
        private static uint BarcodeType = 0u;

        public static bool is_get_rawimg = true;
        public static bool is_udp_brocast = true;
        private static bool SendConfigDataEn = true;
        public  static bool is_clear_before_connect = true;
        private static byte[] BarcodeData = new byte[4096];
        private static object[] invokeChartData = new object[2];

        private static object thisLock = new object();

        private static bool Is_Lock_Op = false;

        private static bool IsDataHeaderReceive, Is_NewLinuxPassword;
        private static int rec_frame_count = 0;

        private static int DataReceiveLenP = 0;

        private static  int DataReceiveLenN = 0;

        private static bool IsReadyDataP = false;

        private static bool IsReadyDataN = false;

        private static bool IsClearDataP_En = false;

        private static bool IsClearDataN_En = false;

        private static bool IsCopyToDataP = true;
        private static ProtocolHeaderStu ProtocolHeader = default(ProtocolHeaderStu);

        private static ProtocolExtraDataStu ProtocolExtraData = default(ProtocolExtraDataStu);

        private static byte[] ImageData = new byte[37748736];

        

        private static int ImageWidth = 0;

        private static int ImageHeight = 0;

        private static uint ImageSize = 0u;

        private static int ImageType = 0;
        public static double FrameTime = 0.0;

        private static long NowTime = 0L;

        private static long FrameCount = 0L;

        public static int UpdateOtherForm = 0;

        private  const int DATA_SWITCH_PARA = 1;

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

  

        public static Mutex mutex_mask_img = new Mutex();

        private static bool IsProfessonal = true;

        private string ExtriTrigStr = "None";

        private int IpIndexSel = 0;
        private Image ImageShow = new Bitmap(376, 240);
        private static bool IsNeedAutoConnect = true;
        public static DateTime dtOld= DateTime.Now;
        private static string LastDeviceIp;
        public static  List<String> ScanCard()
        {
            List < String > listIP= new List< String >();
            string hostName = Dns.GetHostName();
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
          
            IPAddress[] array = hostAddresses;
            foreach (IPAddress iPAddress in array)
            {
                if (!iPAddress.IsIPv6LinkLocal && iPAddress.ToString().Length < 20)
                {
                    listIP.Add(iPAddress.ToString());
                }
            }
          return listIP;
        }
        public void SetEnhance(bool Is)
        {
            if (!ToolCfg.UpdateAdjState)
            {
                if (Is)
                {
                    SetCfgCBFuncCB(2312u, 8u);
                }
                else
                {
                    SetCfgCBFuncCB(2312u, 0u);
                }
                SendCfgDataCBFuncCB(0u);
            }
        }
        public bool GetEnhance()
        {
         return   GetCfgCBFuncCB(2312u) == 8;
           
        }
        public bool GetMirror()
        {
            return GetCfgCBFuncCB(2307u) == 1;

        }
        public bool GetInverse()
        {
            return GetCfgCBFuncCB(2308u) == 4;

        }

        public bool GetEqualizeHist()
        {
            return GetCfgCBFuncCB(2336u) == 32;

        }
        private static void DeviceRestartCheck(uint action)
        {
            if ((action & 2) == 0)
            {
                return;
            }
            IsNeedAutoConnect = true;
            LastDeviceIp = devChoosed.IpAddrStr;
            DateTime now = DateTime.Now;
            while (true)
            {
                //Tsb_SearchDevice_Click(null, null);
                if (!IsNeedAutoConnect || DateTime.Now.Subtract(now).TotalSeconds > 10.0)
                {
                    break;
                }
                Thread.Sleep(100);
                Application.DoEvents();
            }
        }
        private static void DeviceDataReceive(ref DeviceFindAndCom.DeviceFound device, byte[] dat, int len)
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
            //if (dat[0] == 2 && dat[1] == 0 && dat[2] == 0 && dat[3] == 1 && ToolCfg.FormCMD_Tool != null)
            //{
            //   // ToolCfg.FormCMD_Tool.CMD_Ack_CallBack(dat, len);
            //}
            //if (ToolCfg.ConfigBarcodeCheckForm != null && ((dat[0] == 2 && dat[1] == 0 && dat[2] == 116 && dat[3] == 38) || !is_no_next_data))
            //{
            //    is_no_next_data = true;
            //    ToolCfg.ConfigBarcodeCheckForm.Data_Ack_CallBack(dat, len, out is_no_next_data);
            //}
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

        private static void DeviceStateChange(ref DeviceFindAndCom.DeviceFound dev, bool newstate)
        {
            DeviceFindAndCom.DeviceFound tmp = dev;
            if (tmp == null)
            {
                return;
            }
            //Invoke((MethodInvoker)delegate
           // {
                if (newstate)
                {
                    if (tmp.ConnectType == DeviceFindAndCom.DeviceType.USB_LIB)
                    {
                        ToolCfg.UpdateAdjState = true;
                        byte[] array = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
                       // tmp.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1UsbDevice", tmp.OtherInfoStr.GetHashCode().ToString(), "Usb Device(...)", 11, tmp);
                        tmp.SendData(tmp.DeviceHandle, array, array.Length);
                        tmp.USB_PID = 4660;
                        for (int i = 0; i < listDev.DeviceFoudList.Count; i++)
                        {
                            if (listDev.DeviceFoudList[i].ConnectType == DeviceFindAndCom.DeviceType.USB_HID)
                            {
                               // DeleteTreeNode(TrvDevicesList.Nodes, listDev.DeviceFoudList[i].OtherInfoStr.GetHashCode().ToString());
                            }
                            if (listDev.DeviceFoudList[i].ConnectType == DeviceFindAndCom.DeviceType.USB_COM)
                            {
                              //  DeleteTreeNode(TrvDevicesList.Nodes, listDev.DeviceFoudList[i].OtherInfoStr.GetHashCode().ToString());
                            }
                        }
                    }
                    else
                    {
                        int imgidx = (tmp.NetName.Contains("HM7") ? 7 : (tmp.NetName.Contains("HM6") ? 6 : (tmp.NetName.Contains("Tiny") ? 13 : (tmp.NetName.Contains("WL") ? 14 : (tmp.NetName.Contains("HR") ? 15 : (tmp.NetName.Contains("HM2") ? 16 : ((!tmp.NetName.Contains("H296")) ? 9 : 12)))))));
                      //  tmp.Node = AddTreeNode(TrvDevicesList.Nodes, "Tree1NetworkDevice", tmp.OtherInfoStr.GetHashCode().ToString(), tmp.NetName + "(" + tmp.IpAddrStr + "-" + tmp.MacStr.Substring(12, 5).Replace(":", "") + ")", imgidx, tmp);
                    }
                   // TrvDevicesList.ExpandAll();
                    //AutoConnectDevice();
                }
                else
            {
              
                for (int j = 0; j < listDev.DeviceFoudList.Count; j++)
                    {
                        if (listDev.DeviceFoudList[j].OtherInfoStr == tmp.OtherInfoStr)
                        {
                      
                          //  DeleteTreeNode(TrvDevicesList.Nodes, listDev.DeviceFoudList[j].OtherInfoStr.GetHashCode().ToString());
                            //tmp.Node = null;
                            if (listDev.DeviceFoudList[j] == ToolCfg.CurrentDevice)
                        {
                           // BeeCore.Camera.IsConnected = false;
                            //Invoke((MethodInvoker)delegate
                            //{
                        //    DevStateCallback(DevStateDef.DevDisConnnected);
                               // });
                            }
                        }
                    }
                }
          //  });
     //       dev.Node = tmp.Node;
        }
        private static Polygon[] ByteArrayToPolygonArray(byte[] dat, int len, int zoom)
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
        private static int num = 0;
        public static int CycleTime = 0;
        public static Mat ByteArrayToMat(byte[] byteArray, int width, int height,int Type)
        {
            Mat mat = new Mat(height, width, MatType.CV_8UC1); // 8-bit, 1 kênh (Grayscale)
            mat.SetArray( byteArray);
            return mat;
        }
     static   int numErr = 0;
        public static Mat  Read()
        {
          
            dtOld = DateTime.Now;
            if (Common.bmRaw != null)
                Common.bmRaw.Dispose();

            if (Common.listRaw.Count > 0)
            {
                numErr = 0;
                Mat raw= Common.listRaw[0].ToMat().Clone();// ByteToMat(ImageData, ImageWidth, ImageHeight, ImageType);
                Common.listRaw[0].Dispose();
                return raw;
                   
            }

            return new Mat();
            //uint num2 = 0u;
            //    byte[] array = null;
            //    SemDataOk.WaitOne();

            //    if (IsReadyDataP)
            //    {
            //        array = DataReceiveBufP;
            //        num = DataReceiveLenP;
            //    }
            //    if (IsReadyDataN)
            //    {
            //        array = DataReceiveBufN;
            //        num = DataReceiveLenN;
            //    }
            //    if (array != null)
            //    {
            //        ProtocolHeader.DataFormByteArray(array);
            //        BarcodeLen = ProtocolHeader.BarCodeLen;
            //        BarcodeType = ProtocolHeader.BarCodeType;
            //        ImageWidth = ProtocolHeader.ImageWidth;
            //        ImageHeight = ProtocolHeader.ImageHeight;
            //        ImageSize = ProtocolHeader.ImageSize;
            //        ImageType = ProtocolHeader.ImageType;
            //        uint num3 = HJ_CRC32.crc32_offset(array, 64, 8);
            //        uint paraCrc = ProtocolHeader.ParaCrc32;
            //        Is_NewLinuxPassword = (ProtocolHeader.ParaAction & 1) == 1;
            //        if (ProtocolHeader.Crc32 == num3)
            //        {
            //            ParaDataLen = ProtocolHeader.ParaDataLen;
            //            if (ParaDataLen != 256 && ParaDataLen != 4096)
            //            {
            //                ParaDataLen = 256u;
            //            }
            //            ToolCfg.ParaDataLen = (int)ParaDataLen;
            //            ToolCfg.SensorWidth = ProtocolHeader.SensorWidth;
            //            ToolCfg.SensorHeight = ProtocolHeader.SensorHeight;
            //            num2 += 64;
            //            if ((ProtocolHeader.DataSwitch & 1) == 1)
            //            {
            //                uint num4 = HJ_CRC32.crc32_offset(array, (int)(ParaDataLen + num2), (int)num2);
            //                if (num4 == paraCrc)
            //                {
            //                    SendConfigDataEn = true;
            //                    Array.Copy(array, num2, DeviceParaRead, 0L, ParaDataLen);
            //                }
            //                num2 += ParaDataLen;
            //            }
            //            int num5 = DeviceCfgRead.GET_CFG(7951u);
            //            if (num5 <= 0 || num5 > 32)
            //            {
            //                num5 = 3;
            //            }
            //            if ((ProtocolHeader.DataSwitch & 2) == 2)
            //            {
            //                Array.Copy(array, num2, BarcodeData, 0L, BarcodeLen);
            //                num2 += (uint)BarcodeLen;
            //                if (BarcodeLen == 0)
            //                {
            //                }
            //            }
            //            if ((ProtocolHeader.DataSwitch & 4) == 4)
            //            {
            //                Array.Copy(array, num2, ImageData, 0L, ImageSize);
            //                num2 += ImageSize;
            //            }
            //            if ((ProtocolHeader.DataSwitch & 8) == 8)
            //            {
            //                Array.Copy(array, num2, RegionData, 0L, ProtocolHeader.BarCodeNum * 4 * 8);
            //                num2 += (uint)(ProtocolHeader.BarCodeNum * 4 * 8);
            //                if (ProtocolHeader.BarCodeNum > 0)
            //                {
            //                    BarCodeRegion = ByteArrayToPolygonArray(RegionData, ProtocolHeader.BarCodeNum * 4 * 8, num5);
            //                }
            //            }
            //            if ((ProtocolHeader.DataSwitch & 0x10) == 16)
            //            {
            //                byte[] array2 = new byte[256];
            //                Array.Copy(array, num2, array2, 0L, array2.Length);
            //                num2 += (uint)array2.Length;
            //                ProtocolExtraData.DataFormByteArray(array2);
            //                //Invoke((MethodInvoker)delegate
            //                //{
            //                //	ReadingPagePara.ProtocolExtraData = ProtocolExtraData;

            //                //	ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateDecodeCount);
            //                //	if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK && DeviceCfgRead.READ_CFG(155651u, 2u))
            //                //	{
            //                //		AdvancedPage_UpdateCB(ProtocolExtraData);
            //                //	}
            //                //});
            //            }
            //            ToolCfg.DeviceTypeRecord = ProtocolHeader.DeviceType;
            //            if ((ProtocolHeader.ParaAction & 2) == 2)
            //            {
            //                //Invoke((MethodInvoker)delegate
            //                //{
            //                //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AF_OK);
            //                //});
            //            }
            //            if ((ProtocolHeader.ParaAction & 4) == 4)
            //            {
            //                //Invoke((MethodInvoker)delegate
            //                //{
            //                //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_START);
            //                //});
            //            }
            //            if ((ProtocolHeader.ParaAction & 8) == 8)
            //            {
            //                //Invoke((MethodInvoker)delegate
            //                //{

            //                //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_PROC);
            //                //});
            //            }
            //            if ((ProtocolHeader.ParaAction & 0x40) == 64)
            //            {
            //                //Invoke((MethodInvoker)delegate
            //                //{
            //                //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.ADC_PROC);
            //                //});
            //            }
            //            if ((ProtocolHeader.ParaAction & 0x10) == 16)
            //            {
            //                //Invoke((MethodInvoker)delegate
            //                //{
            //                //    //ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_OK_END);
            //                //});
            //                ToolCfg.UpdateAdjState = true;
            //            }
            //            if ((ProtocolHeader.ParaAction & 0x20) == 32)
            //            {
            //                //Invoke((MethodInvoker)delegate
            //                //{
            //                //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_NG_END);
            //                //});
            //                ToolCfg.UpdateAdjState = true;
            //            }
            //            //if (ToolCfg.UpdateAdjState)
            //            //{
            //            //    if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 100)
            //            //    {
            //            //        //Invoke((MethodInvoker)delegate
            //            //        //{
            //            //        if (ToolCfg.CurrentDevice.Node != null)
            //            //        {
            //            //            string text2;
            //            //            if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
            //            //            {
            //            //                text2 = ProtocolHeader.DeviceID.ToString("X8") + "-" + ToolCfg.CurrentDevice.MacStr.Substring(12, 5).Replace(":", "");
            //            //                //DevStateCallback(DevStateDef.DevConnected);
            //            //            }
            //            //            else
            //            //            {
            //            //                text2 = ProtocolHeader.DeviceID.ToString("X8");
            //            //                //DevStateCallback(DevStateDef.DevConnected);
            //            //            }
            //            //            ToolCfg.CurrentDevice.Node.Text = Encoding.Default.GetString(ProtocolHeader.DeviceName).TrimEnd(default(char)) + "(" + text2 + ")";
            //            //            if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 9)
            //            //            {
            //            //                ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType;
            //            //            }
            //            //            else
            //            //            {
            //            //                ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType + 3;
            //            //            }
            //            //            ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
            //            //        }
            //            //        // });
            //            //        //if (ProtocolHeader.DeviceType == 19 || ProtocolHeader.DeviceType < 6 || (ProtocolHeader.DeviceType >= 9 && ProtocolHeader.DeviceType <= 14))
            //            //        //{
            //            //        //    ToolCfg.ftp.InitFtpParam("root", "hj168", true);
            //            //        //}
            //            //        //else if (Is_NewLinuxPassword)
            //            //        //{
            //            //        //    ToolCfg.ftp.InitFtpParam("root", "lianghj");
            //            //        //}
            //            //        //else
            //            //        //{
            //            //        //    ToolCfg.ftp.InitFtpParam("root", "");
            //            //        //}
            //            //        if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
            //            //        {
            //            //            DateTime now = DateTime.Now;
            //            //            int num6 = (now.Year - 2000 << 26) | (now.Month << 22) | (now.Day << 17) | (now.Hour << 12) | (now.Minute << 6) | now.Second;
            //            //            byte[] obj = new byte[9] { 126, 0, 12, 84, 0, 0, 0, 0, 116 };
            //            //            obj[4] = (byte)((uint)num6 & 0xFFu);
            //            //            obj[5] = (byte)((num6 & 0xFF00) >> 8);
            //            //            obj[6] = (byte)((num6 & 0xFF0000) >> 16);
            //            //            obj[7] = (byte)((num6 & 0xFF000000u) >> 24);
            //            //            byte[] array3 = obj;
            //            //            ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, array3, array3.Length);
            //            //        }
            //            //        ToolCfg.width_offset = ProtocolHeader.SensorXStart;
            //            //        ToolCfg.height_offset = ProtocolHeader.SensorYStart;
            //            //    }
            //            //    else
            //            //    {
            //            //        //Invoke((MethodInvoker)delegate
            //            //        //{
            //            //        ToolCfg.CurrentDevice.Node.ImageIndex = 0;
            //            //        ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
            //            //        ToolCfg.CurrentDevice.Node.Text = "USB_Devices(" + ProtocolHeader.DeviceID.ToString("X8") + ")";
            //            //        // });
            //            //        if (DeviceCfgRead.GET_CFG(57599u) == 5)
            //            //        {
            //            //            ToolCfg.width_offset = 260;
            //            //            ToolCfg.height_offset = 80;
            //            //        }
            //            //        else if (DeviceCfgRead.GET_CFG(57599u) == 4)
            //            //        {
            //            //            if (DeviceCfgRead.READ_CFG(3074u, 0u))
            //            //            {
            //            //                ToolCfg.width_offset = 340;
            //            //                ToolCfg.height_offset = 80;
            //            //            }
            //            //            else
            //            //            {
            //            //                ToolCfg.width_offset = 120;
            //            //                ToolCfg.height_offset = 80;
            //            //            }
            //            //        }
            //            //        else if (DeviceCfgRead.GET_CFG(57599u) == 3)
            //            //        {
            //            //            if (DeviceCfgRead.GET_CFG(61183u) == 5)
            //            //            {
            //            //                ToolCfg.width_offset = 420;
            //            //                ToolCfg.height_offset = 100;
            //            //            }
            //            //            else
            //            //            {
            //            //                ToolCfg.width_offset = 380;
            //            //                ToolCfg.height_offset = 160;
            //            //            }
            //            //        }
            //            //        else
            //            //        {
            //            //            ToolCfg.width_offset = 380;
            //            //            ToolCfg.height_offset = 160;
            //            //        }
            //            //    }
            //            //}
            //            //if (IsNeedToUpdateTree)
            //            //{
            //            //    IsNeedToUpdateTree = false;
            //            //    //Invoke((MethodInvoker)delegate
            //            //    //{
            //            //    if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 100)
            //            //    {
            //            //        if (ToolCfg.CurrentDevice.Node != null)
            //            //        {
            //            //            string text;
            //            //            if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
            //            //            {
            //            //                text = ProtocolHeader.DeviceID.ToString("X8") + "-" + ToolCfg.CurrentDevice.MacStr.Substring(12, 5).Replace(":", "");
            //            //                //DevStateCallback(DevStateDef.DevConnected);
            //            //            }
            //            //            else
            //            //            {
            //            //                text = ProtocolHeader.DeviceID.ToString("X8");
            //            //            }
            //            //            ToolCfg.CurrentDevice.Node.Text = Encoding.Default.GetString(ProtocolHeader.DeviceName).TrimEnd(default(char)) + "(" + text + ")";
            //            //            if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 9)
            //            //            {
            //            //                ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType;
            //            //            }
            //            //            else
            //            //            {
            //            //                ToolCfg.CurrentDevice.Node.ImageIndex = ProtocolHeader.DeviceType + 3;
            //            //            }
            //            //            ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
            //            //        }
            //            //    }
            //            //    else if (ToolCfg.CurrentDevice.Node != null)
            //            //    {
            //            //        ToolCfg.CurrentDevice.Node.ImageIndex = 0;
            //            //        ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
            //            //        ToolCfg.CurrentDevice.Node.Text = "USB_Devices(" + ProtocolHeader.DeviceID.ToString("X8") + ")";
            //            //    }
            //            //    // });
            //            //}

            //            if ((FrameCount & 7) == 7)
            //            {
            //                long ticks = DateTime.Now.Ticks;
            //                if (NowTime != 0)
            //                {

            //                    FrameTime = 10000000.0 / (double)(ticks - NowTime) * 8.0;
            //                    //Invoke((MethodInvoker)delegate
            //                    //{
            //                    // ReadingPagePara.FrameRate = ((long)FrameTime).ToString();
            //                    BeeCore.Camera.FrameRate = Convert.ToInt32(FrameTime);
            //                    //});
            //                }

            //            NowTime = ticks;
            //            }
            //            //    TrigCount++;
            //            FrameCount++;

            //            //TrigCount++;
            //            //FrameCount++;
            //            //string str = "";
            //            //if (DeviceCfgRead.GET_CFG(57599u) == 5)
            //            //{
            //            //	str = "VL";
            //            //}
            //            //else if (DeviceCfgRead.GET_CFG(57599u) == 4)
            //            //{
            //            //	str = "VO";
            //            //}
            //            //else
            //            //{
            //            //	str = "VM";
            //            //}
            //            //if (!ToolCfg.is_stop)
            //            //{
            //            //	if (ProtocolHeader.SensorHeight > 960)
            //            //	{
            //            //		BarcodePen = new Pen(new SolidBrush(BarcodeColor), 5 - num5);
            //            //	}
            //            //	Invoke((MethodInvoker)delegate
            //            //	{
            //            //		ReadingPagePara.TrigCount = TrigCount;

            //            //		//ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateTrigCount);
            //            //		//UpdateParaAndDisplay(DeviceParaRead);
            //            //		TxbDeviceInfo.Text = str + DeviceCfgRead.GET_CFG(58111u);
            //            //	});
            //            //}
            //            if (!ToolCfg.is_stop)
            //            {

            //                //Invoke((MethodInvoker)delegate
            //                //{
            //                //  //  ReadingPagePara.TrigCount = TrigCount;
            //                //    //ReadingPagePara.TrigCount = TrigCount;

            //                //});
            //            }
            //            if (ImageWidth > 0 && ImageHeight > 0 && num == num2 + 4)
            //            {
            //                if ((ProtocolHeader.DataSwitch & 4) != 4)
            //                {
            //                    Array.Clear(ImageData, 0, ImageData.Length);
            //                }
            //           int a= ImageType;
            //            Common.matRaw = ByteToMat(ImageData, ImageWidth, ImageHeight, ImageType);

            //            //if (DeviceCfgRead.READ_CFG(11272u, 8u))
            //            //    {
            //            //        int num7 = (DeviceCfgRead.GET_CFG(50431u) * 8 - ToolCfg.width_offset) / num5;
            //            //        int num8 = DeviceCfgRead.GET_CFG(50943u) * 8 / num5;
            //            //        int num9 = (DeviceCfgRead.GET_CFG(50687u) * 8 - ToolCfg.height_offset) / num5;
            //            //        int num10 = DeviceCfgRead.GET_CFG(51199u) * 8 / num5;
            //            //        if (num8 == 0)
            //            //        {
            //            //            num8 = 1;
            //            //        }
            //            //        if (num10 == 0)
            //            //        {
            //            //            num10 = 1;
            //            //        }
            //            //        try
            //            //        {
            //            //            //db01

            //            //            //ImageShow = new Bitmap(ProtocolHeader.SensorWidth / num5, ProtocolHeader.SensorHeight / num5);
            //            //            //Image image = Byte2Bitmap(ImageData, num8, num10, PixelFormat.Format8bppIndexed, ImageType);
            //            //            //Graphics graphics = Graphics.FromImage(ImageShow);
            //            //            //graphics.Clear(Color.FromArgb(27, 27, 27));
            //            //            //graphics.DrawImage(image, num7, num9);
            //            //            //ToolCfg.TemplateImg = (Image)ImageShow.Clone();
            //            //            //if (ToolCfg.IsDispNetGrid)
            //            //            //{
            //            //            //	graphics.DrawLine(new Pen(Color.Blue), 0, num10 / 2, num8 - 1, num10 / 2);
            //            //            //	graphics.DrawLine(new Pen(Color.Blue), num8 / 2, 0, num8 / 2, num10 - 1);
            //            //            //	graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, num10 / 4, num8 - 1, num10 / 4);
            //            //            //	graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, num10 * 3 / 4, num8 - 1, num10 * 3 / 4);
            //            //            //	graphics.DrawLine(new Pen(Color.CornflowerBlue), num8 / 4, 0, num8 / 4, num10 - 1);
            //            //            //	graphics.DrawLine(new Pen(Color.CornflowerBlue), num8 * 3 / 4, 0, num8 * 3 / 4, num10 - 1);
            //            //            //}
            //            //            //if (BarcodeLen > 0 && BarCodeRegion != null)
            //            //            //{
            //            //            //	for (int i = 0; i < BarCodeRegion.Length; i++)
            //            //            //	{
            //            //            //		Point[] array4 = BarCodeRegion[i].ToPointArray();
            //            //            //		for (int j = 0; j < array4.Length; j++)
            //            //            //		{
            //            //            //			array4[j].X += num7;
            //            //            //			array4[j].Y += num9;
            //            //            //		}
            //            //            //		graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //            //            //		graphics.DrawLines(BarcodePen, array4);
            //            //            //	}
            //            //            //}
            //            //            if (!ToolCfg.is_stop)
            //            //        {


            //            //          //  Common.matRaw = Common.bmRaw.ToMat().Clone();
            //            //                //Invoke((MethodInvoker)delegate
            //            //                //{
            //            //                //    //ReadingPagePara.ImageShow = ImageShow;
            //            //                //    //BeeCore.Common.intPtrRaw = BeeCore.Common.ArrayToIntPtr(ImageData);
            //            //                //    //if (ReadingPagePara.ImageShow != null)
            //            //                //    //ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateImage);
            //            //                //});
            //            //            }
            //            //            //graphics.Dispose();
            //            //        }
            //            //        catch (Exception)
            //            //        {
            //            //        }
            //            //    }
            //            //    else
            //            //    {

            //            //        // ImageShow = Byte2Bitmap(ImageData, ImageWidth, ImageHeight, PixelFormat.Format8bppIndexed, ImageType);
            //            //        // ToolCfg.TemplateImg = (Image)ImageShow.Clone();
            //            //        // Graphics graphics2 = Graphics.FromImage(ImageShow);
            //            //        //                        //if (ToolCfg.IsDispNetGrid)
            //            //        //{
            //            //        //	int imageWidth = ImageWidth;
            //            //        //	int imageHeight = ImageHeight;
            //            //        //	graphics2.DrawLine(new Pen(Color.Blue), 0, imageHeight / 2, imageWidth - 1, imageHeight / 2);
            //            //        //	graphics2.DrawLine(new Pen(Color.Blue), imageWidth / 2, 0, imageWidth / 2, imageHeight - 1);
            //            //        //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), 0, imageHeight / 4, imageWidth - 1, imageHeight / 4);
            //            //        //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), 0, imageHeight * 3 / 4, imageWidth - 1, imageHeight * 3 / 4);
            //            //        //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), imageWidth / 4, 0, imageWidth / 4, imageHeight - 1);
            //            //        //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), imageWidth * 3 / 4, 0, imageWidth * 3 / 4, imageHeight - 1);
            //            //        //}
            //            //        //if (BarcodeLen > 0 && BarCodeRegion != null)
            //            //        //{
            //            //        //	graphics2.SmoothingMode = SmoothingMode.AntiAlias;
            //            //        //	try
            //            //        //	{
            //            //        //		for (int k = 0; k < BarCodeRegion.Length; k++)
            //            //        //		{
            //            //        //			graphics2.DrawLines(BarcodePen, BarCodeRegion[k].ToPointArray());
            //            //        //		}
            //            //        //	}
            //            //        //	catch
            //            //        //	{
            //            //        //	}
            //            //        //}
            //            //        if (!ToolCfg.is_stop)// base.IsHandleCreated &&
            //            //    {

            //            //        Common.bmRaw = Byte2Bitmap(ImageData, ImageWidth, ImageHeight, PixelFormat.Format8bppIndexed, ImageType);
            //            //     //   Common.matRaw = Common.bmRaw.ToMat().Clone();
            //            //        //Invoke((MethodInvoker)delegate
            //            //        //{
            //            //        //    //                              ReadingPagePara.ImageShow = ImageShow;
            //            //        //    //BeeCore.Common.intPtrRaw = BeeCore.Common.ArrayToIntPtr(ImageData);
            //            //        //    //if (ReadingPagePara.ImageShow != null)


            //            //        //    //ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateImage);
            //            //        //});
            //            //    }
            //            //        // graphics2.Dispose();
            //            //    }
            //            }
            //        }
            //    }
            //    if (IsReadyDataP)
            //    {
            //        IsReadyDataP = false;
            //    }
            //    if (IsReadyDataN)
            //    {
            //        IsReadyDataN = false;
            //    }
            TimeSpan sp = DateTime.Now - dtOld;
            CycleTime = Convert.ToInt32(sp.TotalMilliseconds);

     //  return new Size(ImageWidth, ImageHeight);
            // ThreadDataProc.DisableComObjectEagerCleanup();
        }
        public static bool IsNeedToUpdateTree=false;
        public static string BarcodeStr;
        private static void DataReceiveAndStateUpdate()
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
                            //Invoke((MethodInvoker)delegate
                            //{
                            //	ReadingPagePara.ProtocolExtraData = ProtocolExtraData;

                            //	ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateDecodeCount);
                            //	if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK && DeviceCfgRead.READ_CFG(155651u, 2u))
                            //	{
                            //		AdvancedPage_UpdateCB(ProtocolExtraData);
                            //	}
                            //});
                        }
                        ToolCfg.DeviceTypeRecord = ProtocolHeader.DeviceType;
                        if ((ProtocolHeader.ParaAction & 2) == 2)
                        {
                            //Invoke((MethodInvoker)delegate
                            //{
                            //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AF_OK);
                            //});
                        }
                        if ((ProtocolHeader.ParaAction & 4) == 4)
                        {
                            //Invoke((MethodInvoker)delegate
                            //{
                            //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_START);
                            //});
                        }
                        if ((ProtocolHeader.ParaAction & 8) == 8)
                        {
                            //Invoke((MethodInvoker)delegate
                            //{

                            //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_PROC);
                            //});
                        }
                        if ((ProtocolHeader.ParaAction & 0x40) == 64)
                        {
                            //Invoke((MethodInvoker)delegate
                            //{
                            //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.ADC_PROC);
                            //});
                        }
                        if ((ProtocolHeader.ParaAction & 0x10) == 16)
                        {
                            //Invoke((MethodInvoker)delegate
                            //{
                            //    //ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_OK_END);
                            //});
                            ToolCfg.UpdateAdjState = true;
                        }
                        if ((ProtocolHeader.ParaAction & 0x20) == 32)
                        {
                            //Invoke((MethodInvoker)delegate
                            //{
                            //    //  ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.AP_NG_END);
                            //});
                            ToolCfg.UpdateAdjState = true;
                        }
                        if (ToolCfg.UpdateAdjState)
                        {
                            if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 100)
                            {
                                //Invoke((MethodInvoker)delegate
                                //{
                                if (ToolCfg.CurrentDevice.Node != null)
                                {
                                    string text2;
                                    if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
                                    {
                                        text2 = ProtocolHeader.DeviceID.ToString("X8") + "-" + ToolCfg.CurrentDevice.MacStr.Substring(12, 5).Replace(":", "");
                                        //DevStateCallback(DevStateDef.DevConnected);
                                    }
                                    else
                                    {
                                        text2 = ProtocolHeader.DeviceID.ToString("X8");
                                        //DevStateCallback(DevStateDef.DevConnected);
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
                                // });
                                //if (ProtocolHeader.DeviceType == 19 || ProtocolHeader.DeviceType < 6 || (ProtocolHeader.DeviceType >= 9 && ProtocolHeader.DeviceType <= 14))
                                //{
                                //    ToolCfg.ftp.InitFtpParam("root", "hj168", true);
                                //}
                                //else if (Is_NewLinuxPassword)
                                //{
                                //    ToolCfg.ftp.InitFtpParam("root", "lianghj");
                                //}
                                //else
                                //{
                                //    ToolCfg.ftp.InitFtpParam("root", "");
                                //}
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
                                //Invoke((MethodInvoker)delegate
                                //{
                                ToolCfg.CurrentDevice.Node.ImageIndex = 0;
                                ToolCfg.CurrentDevice.Node.SelectedImageIndex = ToolCfg.CurrentDevice.Node.ImageIndex;
                                ToolCfg.CurrentDevice.Node.Text = "USB_Devices(" + ProtocolHeader.DeviceID.ToString("X8") + ")";
                                // });
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
                            //Invoke((MethodInvoker)delegate
                            //{
                            if (ProtocolHeader.DeviceType > 0 && ProtocolHeader.DeviceType < 100)
                            {
                                if (ToolCfg.CurrentDevice.Node != null)
                                {
                                    string text;
                                    if (ToolCfg.CurrentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
                                    {
                                        text = ProtocolHeader.DeviceID.ToString("X8") + "-" + ToolCfg.CurrentDevice.MacStr.Substring(12, 5).Replace(":", "");
                                        //DevStateCallback(DevStateDef.DevConnected);
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
                            // });
                        }
                        if (BarcodeLen > 0)
                        {
                            //DecodeSuccessCount++;
                            //Invoke((MethodInvoker)delegate
                            //{
                            //    ReadingPagePara.DecodeTime = ProtocolHeader.DecodeTime;
                            //    ReadingPagePara.BarcodeLen = BarcodeLen;
                            //    ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateDecodeTimeInfo);
                            //});
                            string barcode_type;
                            //if (BarcodeType < 5)
                            //{
                            //    barcode_type = "UPC/EAN";
                            //}
                            //else
                            //{
                            //    barcode_type = Enum.GetName(typeof(AllBarcodeType), BarcodeType);
                            //}
                            string barcode_str = Encoding.Default.GetString(BarcodeData, 0, BarcodeLen);
                            BarcodeStr = (string)barcode_str.Clone();
                          
                        }
                        else
                        {
                            BarcodeStr = "";
                           // ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateBarcodeStr);
                        }

                        if ((FrameCount & 7) == 7)
                        {
                            long ticks = DateTime.Now.Ticks;
                            if (NowTime != 0)
                            {

                                FrameTime = 10000000.0 / (double)(ticks - NowTime) * 8.0;
                                //Invoke((MethodInvoker)delegate
                                //{
                                // ReadingPagePara.FrameRate = ((long)FrameTime).ToString();
                                //BeeCore.Camera.FrameRate = Convert.ToInt32(FrameTime);

                                //});
                            }

                            NowTime = ticks;
                        }
                        //    TrigCount++;
                        FrameCount++;

                        //TrigCount++;
                        //FrameCount++;
                        //string str = "";
                        //if (DeviceCfgRead.GET_CFG(57599u) == 5)
                        //{
                        //	str = "VL";
                        //}
                        //else if (DeviceCfgRead.GET_CFG(57599u) == 4)
                        //{
                        //	str = "VO";
                        //}
                        //else
                        //{
                        //	str = "VM";
                        //}
                        //if (!ToolCfg.is_stop)
                        //{
                        //	if (ProtocolHeader.SensorHeight > 960)
                        //	{
                        //		BarcodePen = new Pen(new SolidBrush(BarcodeColor), 5 - num5);
                        //	}
                        //	Invoke((MethodInvoker)delegate
                        //	{
                        //		ReadingPagePara.TrigCount = TrigCount;

                        //		//ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateTrigCount);
                        //		//UpdateParaAndDisplay(DeviceParaRead);
                        //		TxbDeviceInfo.Text = str + DeviceCfgRead.GET_CFG(58111u);
                        //	});
                        //}
                        if (!ToolCfg.is_stop)
                        {

                            //Invoke((MethodInvoker)delegate
                            //{
                            //  //  ReadingPagePara.TrigCount = TrigCount;
                            //    //ReadingPagePara.TrigCount = TrigCount;

                            //});
                        }
                        if (ImageWidth > 0 && ImageHeight > 0 && num == num2 + 4)
                        {
                            if ((ProtocolHeader.DataSwitch & 4) != 4)
                            {
                                Array.Clear(ImageData, 0, ImageData.Length);
                            }
                           if (Common.listRaw.Count()>1)
                                Common.listRaw.RemoveAt(0);
                            Common.listRaw.Add(Byte2Bitmap(ImageData, ImageWidth, ImageHeight, PixelFormat.Format8bppIndexed, ImageType));

                            //if (DeviceCfgRead.READ_CFG(11272u, 8u))
                            //{
                            //    int num7 = (DeviceCfgRead.GET_CFG(50431u) * 8 - ToolCfg.width_offset) / num5;
                            //    int num8 = DeviceCfgRead.GET_CFG(50943u) * 8 / num5;
                            //    int num9 = (DeviceCfgRead.GET_CFG(50687u) * 8 - ToolCfg.height_offset) / num5;
                            //    int num10 = DeviceCfgRead.GET_CFG(51199u) * 8 / num5;
                            //    if (num8 == 0)
                            //    {
                            //        num8 = 1;
                            //    }
                            //    if (num10 == 0)
                            //    {
                            //        num10 = 1;
                            //    }
                            //    try
                            //    {
                            //        //db01

                            //        //ImageShow = new Bitmap(ProtocolHeader.SensorWidth / num5, ProtocolHeader.SensorHeight / num5);
                            //        //Image image = Byte2Bitmap(ImageData, num8, num10, PixelFormat.Format8bppIndexed, ImageType);
                            //        //Graphics graphics = Graphics.FromImage(ImageShow);
                            //        //graphics.Clear(Color.FromArgb(27, 27, 27));
                            //        //graphics.DrawImage(image, num7, num9);
                            //        //ToolCfg.TemplateImg = (Image)ImageShow.Clone();
                            //        //if (ToolCfg.IsDispNetGrid)
                            //        //{
                            //        //	graphics.DrawLine(new Pen(Color.Blue), 0, num10 / 2, num8 - 1, num10 / 2);
                            //        //	graphics.DrawLine(new Pen(Color.Blue), num8 / 2, 0, num8 / 2, num10 - 1);
                            //        //	graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, num10 / 4, num8 - 1, num10 / 4);
                            //        //	graphics.DrawLine(new Pen(Color.CornflowerBlue), 0, num10 * 3 / 4, num8 - 1, num10 * 3 / 4);
                            //        //	graphics.DrawLine(new Pen(Color.CornflowerBlue), num8 / 4, 0, num8 / 4, num10 - 1);
                            //        //	graphics.DrawLine(new Pen(Color.CornflowerBlue), num8 * 3 / 4, 0, num8 * 3 / 4, num10 - 1);
                            //        //}
                            //        //if (BarcodeLen > 0 && BarCodeRegion != null)
                            //        //{
                            //        //	for (int i = 0; i < BarCodeRegion.Length; i++)
                            //        //	{
                            //        //		Point[] array4 = BarCodeRegion[i].ToPointArray();
                            //        //		for (int j = 0; j < array4.Length; j++)
                            //        //		{
                            //        //			array4[j].X += num7;
                            //        //			array4[j].Y += num9;
                            //        //		}
                            //        //		graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            //        //		graphics.DrawLines(BarcodePen, array4);
                            //        //	}
                            //        //}
                            //        if (!ToolCfg.is_stop)
                            //        {
                            //            BeeCore.Common.GetImgeTinyCam(Byte2Bitmap(ImageData, ImageWidth, ImageHeight, PixelFormat.Format8bppIndexed, ImageType));

                            //            //Invoke((MethodInvoker)delegate
                            //            //{
                            //            //    //ReadingPagePara.ImageShow = ImageShow;
                            //            //    //BeeCore.Common.intPtrRaw = BeeCore.Common.ArrayToIntPtr(ImageData);
                            //            //    //if (ReadingPagePara.ImageShow != null)
                            //            //    //ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateImage);
                            //            //});
                            //        }
                            //        //graphics.Dispose();
                            //    }
                            //    catch (Exception)
                            //    {
                            //    }
                            //}
                            //else
                            //{

                            //    // ImageShow = Byte2Bitmap(ImageData, ImageWidth, ImageHeight, PixelFormat.Format8bppIndexed, ImageType);
                            //    // ToolCfg.TemplateImg = (Image)ImageShow.Clone();
                            //    // Graphics graphics2 = Graphics.FromImage(ImageShow);
                            //    //                        //if (ToolCfg.IsDispNetGrid)
                            //    //{
                            //    //	int imageWidth = ImageWidth;
                            //    //	int imageHeight = ImageHeight;
                            //    //	graphics2.DrawLine(new Pen(Color.Blue), 0, imageHeight / 2, imageWidth - 1, imageHeight / 2);
                            //    //	graphics2.DrawLine(new Pen(Color.Blue), imageWidth / 2, 0, imageWidth / 2, imageHeight - 1);
                            //    //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), 0, imageHeight / 4, imageWidth - 1, imageHeight / 4);
                            //    //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), 0, imageHeight * 3 / 4, imageWidth - 1, imageHeight * 3 / 4);
                            //    //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), imageWidth / 4, 0, imageWidth / 4, imageHeight - 1);
                            //    //	graphics2.DrawLine(new Pen(Color.CornflowerBlue), imageWidth * 3 / 4, 0, imageWidth * 3 / 4, imageHeight - 1);
                            //    //}
                            //    //if (BarcodeLen > 0 && BarCodeRegion != null)
                            //    //{
                            //    //	graphics2.SmoothingMode = SmoothingMode.AntiAlias;
                            //    //	try
                            //    //	{
                            //    //		for (int k = 0; k < BarCodeRegion.Length; k++)
                            //    //		{
                            //    //			graphics2.DrawLines(BarcodePen, BarCodeRegion[k].ToPointArray());
                            //    //		}
                            //    //	}
                            //    //	catch
                            //    //	{
                            //    //	}
                            //    //}
                            //    if ( !ToolCfg.is_stop)// base.IsHandleCreated &&
                            //    {
                            //        BeeCore.Common.GetImgeTinyCam(Byte2Bitmap(ImageData, ImageWidth, ImageHeight, PixelFormat.Format8bppIndexed, ImageType));
                            //        //Invoke((MethodInvoker)delegate
                            //        //{
                            //        //    //                              ReadingPagePara.ImageShow = ImageShow;
                            //        //    //BeeCore.Common.intPtrRaw = BeeCore.Common.ArrayToIntPtr(ImageData);
                            //        //    //if (ReadingPagePara.ImageShow != null)


                            //        //    //ReadingPage_UpdateCB(ReadingPagePara, ReadingPageActDef.UpdateImage);
                            //        //});
                            //    }
                            //    // graphics2.Dispose();
                            //}
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
        private static Mat ByteToMat(byte[] data, int w, int h,int Type)
        {
            Mat mat = new Mat();
            switch (Type)
            {
                case 1:
                    if (data[0] == byte.MaxValue && data[1] == 216)
                    {
                        MemoryStream stream = new MemoryStream(w * h * 2 + 4096);
                        MemoryStream stream2 = new MemoryStream(data);
                        Bitmap bitmap2 = new Bitmap(stream2);
                        bitmap2.Save(stream, ImageFormat.Png);
                        Bitmap bitmap3 = new Bitmap(stream);
                        // ToolCfg.SaveImg = (Image)bitmap3.Clone();
                        mat =  bitmap3.ToMat();
                    }
                    break; 
                case 2:
                    if (data[0] == 66 && data[1] == 77)
                    {
                        MemoryStream stream3 = new MemoryStream(w * h * 4 + 4096);
                        MemoryStream stream4 = new MemoryStream(data);
                        Bitmap bitmap6 = new Bitmap(stream4);
                        bitmap6.Save(stream3, ImageFormat.Png);
                        Bitmap bitmap7 = new Bitmap(stream3);
                        mat = bitmap7.ToMat();

                        //  ToolCfg.SaveImg = (Image)bitmap7.Clone();

                    }
                    break;
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
                        mat = bitmap4.ToMat();
                        // ToolCfg.SaveImg = (Image)bitmap4.Clone();

                    }
                    break;
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
                        mat = bitmap5.ToMat();
                    }
                    break;
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
                        mat = bitmap.ToMat();
                        // ToolCfg.SaveImg = (Image)bitmap.Clone();

                    }
                    break;
            }
            
            return mat;
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
                       // ToolCfg.SaveImg = (Image)bitmap3.Clone();
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
                       
                      //  ToolCfg.SaveImg = (Image)bitmap7.Clone();
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
                       // ToolCfg.SaveImg = (Image)bitmap4.Clone();
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
                       // ToolCfg.SaveImg = (Image)bitmap.Clone();
                        return bitmap;
                    }
            }
        }

        private static object BytesToStruct(byte[] bytes, Type strcutType)
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

        private static byte[] StructToBytes(object obj)
        {
            int num = Marshal.SizeOf(obj);
            byte[] array = new byte[num];
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.StructureToPtr(obj, intPtr, false);
            Marshal.Copy(intPtr, array, 0, num);
            Marshal.FreeHGlobal(intPtr);
            return array;
        }
        private static Thread ThreadDataProc;
        public static Thread threadReadCCD;
        public static  DeviceFindAndCom listDev;
        private static DeviceFindAndCom.DeviceFound devChoosed;
        private static void DevStateCB_Func(DevStateDef a)
        {
            switch (a)
            {
                //case DevStateDef.DevConnected:
                //	TSB_ConnectDevice.Enabled = false;
                //	TSB_DisconnectDevice.Enabled = true;
                //	break;
                //case DevStateDef.DevDisConnnected:
                //	TSB_ConnectDevice.Enabled = true;
                //	TSB_DisconnectDevice.Enabled = false;
                //	break;
                //case DevStateDef.BothDisenable:
                //	TSB_ConnectDevice.Enabled = false;
                //	TSB_DisconnectDevice.Enabled = false;
                //	break;
            }
        }
        public static String  Scan(int IPADDR_SEG0, int IPADDR_SEG1, int IPADDR_SEG2, int IPADDR_SEG3)
        {
            Inifunc();
            String listFind = "";
            listDev = new DeviceFindAndCom(true, false, true, DeviceDataReceive, DeviceStateChange, IPADDR_SEG0,IPADDR_SEG1,IPADDR_SEG2, IPADDR_SEG3, false);
            listDev.EnumAllDevice(true, true, true, false);
            foreach (DeviceFound dev in listDev.DeviceFoudList)
            {
              
                listFind +=(dev.ConnectType.ToString() + "_" + dev.NetName+"_" + dev.IpAddrStr + "_" + dev.MacStr) +"\n";

            }
            if (listFind == "")
                listFind = "No Device";
            DeviceCfgRead = new ModuleSetting(DeviceParaRead);
            DeviceCfgRead.FactorySetting();
            DevStateCallback = DevStateCB_Func;
          
            return listFind;
        }
        private static void AutoResetKey_Elapsed(object sender, ElapsedEventArgs e)
        {
            KEY_Send.SpecialKey(false);
            ((System.Timers.Timer)sender).Dispose();
        }
        public DeviceFindAndCom.DeviceType deviceType = DeviceType.NETWORK;
        public static bool DeviceConfigDataSend(uint action)
        {
            if (SendConfigDataEn && devChoosed != null && devChoosed.IsConnect)
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
                devChoosed.SendData(devChoosed.DeviceHandle, array, (int)(ParaDataLen + 64));
                UpdateOtherForm = 0;
                DeviceRestartCheck(action);
                return true;
            }
            return false;
        }
        public static bool Disconnect()
        {
            if (ThreadDataProc == null) return false;
            ToolCfg.is_stop = true;
            ThreadDataProc.Abort();
            if (devChoosed.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
            {
                if (devChoosed.DeviceHandle != null)
                {
                    byte[] array = new byte[8] { 46, 128, 128, 46, 83, 116, 111, 112 };
                    devChoosed.SendData(devChoosed.DeviceHandle, array, array.Length);
                    DevStateCallback(DevStateDef.DevDisConnnected);
                    devChoosed.IsConnect = false;
                   // Thread.Sleep(500);
                }
            }
            else
            {
               // if (ToolCfg.is_RdbTrigExtern_checked)
                //{
                    DeviceCfgRead.SET_CFG(3u, 0u);
               // }
               // if (ToolCfg.is_RdbOuputByCOM_checked)
               // {
                    DeviceCfgRead.SET_CFG(3331u, 3u);
               // }
                DeviceCfgRead.SET_CFG(52225u, 0u);
                DeviceConfigDataSend(1026u);
            }
            return false;
        }
        public static bool Connect(int index)
        {
            if(listDev.DeviceFoudList.Count==0||index>= listDev.DeviceFoudList.Count)
             return false;
              devChoosed = listDev.DeviceFoudList[index];

            if (devChoosed == null)
            {
                return false;
            }
            if (is_clear_before_connect && devChoosed.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
            {
                listDev.ClearAllConnected();
                DeviceCfgRead.ClearCfgData();
               // ToolCfg.UpdateAdjState = true;
            }
            if (devChoosed.ConnectType == DeviceFindAndCom.DeviceType.USB_HID)
            {
                KEY_Send.SpecialKey(true);
                System.Timers.Timer timer = new System.Timers.Timer(1000.0);
                timer.Elapsed += AutoResetKey_Elapsed;
                timer.Start();
                SendConfigDataEn = false;
            }
            else if (devChoosed.ConnectType == DeviceFindAndCom.DeviceType.USB_COM)
            {
                if (devChoosed.NetName != null && devChoosed.NetName.Contains("COM"))
                {
                    byte[] array = new byte[9] { 126, 0, 9, 49, 0, 204, 1, 171, 205 };
                    byte[] array2 = new byte[9] { 126, 0, 10, 162, 0, 0, 0, 65, 176 };
                    SerialPort serialPort = new SerialPort();
                    serialPort.PortName = devChoosed.NetName;
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
            else if (devChoosed.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
            {
                if (devChoosed.DeviceHandle == null)
                {
                    byte[] array3 = new byte[4];
                    bool flag = false;
                    IPAddress address;
                    if (IPAddress.TryParse(devChoosed.IpAddrStr, out address))
                    {
                        array3 = address.GetAddressBytes();
                        flag = true;
                    }
                    if (devChoosed.IsCommunicate)
                    {
                        listDev.StartConnectDeviceByTcp(devChoosed);
                      ThreadDataProc = new Thread(DataReceiveAndStateUpdate);
                       
                          ThreadDataProc.Start();
                        //return true;
                    }
                    if (flag && array3[0] == listDev.IpAddrSeg0 && array3[1] == listDev.IpAddrSeg1 && array3[2] == listDev.IpAddrSeg2)
                    {
                      
                        devChoosed.IsCommunicate = true;
                        listDev.StartConnectDeviceByTcp(devChoosed);
                        ThreadDataProc = new Thread(DataReceiveAndStateUpdate);
                       
                         ThreadDataProc.Start();
                       // return true;
                    }
                    string text;
                    string caption;
                  //  return false;
                       // text = "Current device ip:" + selectedDevice.IpAddrStr + ",is not the same segment with host ip:" + DpmDevice.IpAddrSeg0 + "." + DpmDevice.IpAddrSeg1 + "." + DpmDevice.IpAddrSeg2 + "." + DpmDevice.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
                       // caption = "Pay Attention";
                    
                    //if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
                    //{
                    //    ShowDeviceIpSettingForm();
                    //}
                }
                else
                {
                    byte[] array4 = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
                   // ToolCfg.UpdateAdjState = true;
                   // selectedDevice.SendData(selectedDevice.DeviceHandle, array4, array4.Length);
                }
            }
            else if (!devChoosed.IsConnect)
            {
                byte[] array5 = new byte[8] { 46, 128, 128, 46, 82, 101, 97, 100 };
               // ToolCfg.UpdateAdjState = true;
                devChoosed.SendData(devChoosed.DeviceHandle, array5, array5.Length);
            }
           
            return true;
        }
        private static SetCfgCB SetCfgCBFuncCB;

        private static GetCfgCB GetCfgCBFuncCB;
        private static SendCfgDataCB SendCfgDataCBFuncCB;
        public  static byte SetPara(uint paraName, uint paraVal)
        {
            if (!ToolCfg.UpdateAdjState)
            {
                DeviceCfgRead.SET_CFG(paraName, paraVal);
                return DeviceCfgRead.GET_CFG(paraName);
            }
            return 0;
        }
        public static byte GetPara(uint paraName)
        {
            return DeviceCfgRead.GET_CFG(paraName);
        }
    
        public static void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
        {
            SetCfgCBFuncCB = setCfgCB;
            GetCfgCBFuncCB = getCfgCB;
            SendCfgDataCBFuncCB = sendCfgDataCB;
        }

        private static void Inifunc()
        {
            SetCBFunc(SetPara, GetPara, DeviceConfigDataSend);
            tmSend.Elapsed += TmSend_Elapsed; tmSend.Interval = 100;
            tmDelayRead.Elapsed += TmDelayRead_Elapsed; ; tmDelayRead.Interval = 100;
        }
        public static void DisConnect()
        {
            if (listDev == null) return;
            listDev.CloseDevice();
            listDev.DestroyDevice();
          
        }
        private static void TmDelayRead_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmDelayRead.Enabled = false;
            Mat raw= Read();
            OpenCvSharp.Size sz = raw.Size();
           // Read();
          
          //  OpenCvSharp.Size sz = Read();
        
            G.ParaCam.SizeCCD=new System.Drawing.Size(sz.Width, sz.Height);
            Shows.Full(Shows.imgTemp, G.ParaCam.SizeCCD);
        }

        private static void TmSend_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendCfgDataCBFuncCB(128u);
            tmSend.Enabled = false;
        }

        private static bool IsEnSetValueEx = false;
        private static Timer tmSend = new Timer();
        private static Timer tmDelayRead = new Timer();
        private static void EnaSetEx()
        {
           
           
            ToolCfg.UpdateAdjState = false;
            SetCfgCBFuncCB(7296u, 128u);
            SendCfgDataCBFuncCB(128u);
            
            IsEnSetValueEx = true;
        }
        private static void DisAuto()
        {
            if (!ToolCfg.UpdateAdjState)
            {
                int ExpRatioVal = 0;
                  SetCfgCBFuncCB(4607u, (ushort)ExpRatioVal);
                SendCfgDataCBFuncCB(128u);
                Thread.Sleep(10);
                ExpRatioVal = 1;
                SetCfgCBFuncCB(4607u, (ushort)ExpRatioVal);
                SendCfgDataCBFuncCB(128u);
            }
        }
        public static string SetExposure(int value)
        {
            ushort result = 0;
            try
            {
               
                if (ushort.TryParse(value.ToString(), out result) && result < ushort.MaxValue)
                {
                    if (!IsEnSetValueEx)
                    {
                        EnaSetEx();
                        Thread.Sleep(100);
                    }




                     SetCfgCBFuncCB(7295u, (byte)((result & 0xFF00) >> 8));
                    SetCfgCBFuncCB(7167u, (byte)(result & 0xFFu));
                    SendCfgDataCBFuncCB(0u);
                    Thread.Sleep(5);
                    SendCfgDataCBFuncCB(128u);
                    Thread.Sleep(5);
                   // DisAuto();
                    //int valueNew = Convert.ToInt32(GetExposure());
                    //if (valueNew != value)
                    //{
                    //    num++;
                    //    if (num < 10)
                    //        goto X;
                    //}

                }
                else
                {
                    return Result.More.ToString();
                }
                //tmSend.Enabled = true;
               
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Result.Success.ToString();
        }
        public static string SetGain(int value)
        {
            ushort result = 0;
            try
            {

                if (ushort.TryParse(value.ToString(), out result) && result < ushort.MaxValue)
                {
                    if (!IsEnSetValueEx)
                    {
                        EnaSetEx();
                        Thread.Sleep(100);
                    }




                    SetCfgCBFuncCB(7295u, (byte)((result & 0xFF00) >> 8));
                    SetCfgCBFuncCB(7167u, (byte)(result & 0xFFu));
                    SendCfgDataCBFuncCB(0u);
                    Thread.Sleep(5);
                    SendCfgDataCBFuncCB(128u);
                    Thread.Sleep(5);
                    // DisAuto();
                    //int valueNew = Convert.ToInt32(GetExposure());
                    //if (valueNew != value)
                    //{
                    //    num++;
                    //    if (num < 10)
                    //        goto X;
                    //}

                }
                else
                {
                    return Result.More.ToString();
                }
                //tmSend.Enabled = true;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Result.Success.ToString();
        }

        public static string GetExposure()
        {
            try
            {
                return (GetCfgCBFuncCB(7295u) * 256 + GetCfgCBFuncCB(7167u)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Result.Failure.ToString();
        }
        public static void SetReSolution( int TypeRes=2)
        {
            if (!ToolCfg.UpdateAdjState)
            {
                SetCfgCBFuncCB(7951u, (ushort)(TypeRes));
                SendCfgDataCBFuncCB(0u);
                if (GetCfgCBFuncCB(11272u) == 8)
                {
                }
                ToolCfg.MaskRect.Width = 0;
            }
            tmDelayRead.Enabled = false; tmDelayRead.Enabled = true;
           
           
        }
        public static void Light(int Type,bool IsOn)
        {
            switch (Type)
            {
                case 1:
                    if (!ToolCfg.UpdateAdjState)
                    {
                        if (IsOn)
                        {
                            SetCfgCBFuncCB(75009u, 1u);
                            SetCfgCBFuncCB(75010u, 0u);
                        }
                        else
                        {
                            SetCfgCBFuncCB(75009u, 0u);
                            SetCfgCBFuncCB(75010u, 0u);
                        }
                        SendCfgDataCBFuncCB(512u);
                    }
                    break;
                case 2:
                    if (!ToolCfg.UpdateAdjState)
                    {
                        if (IsOn)
                        {
                            SetCfgCBFuncCB(75010u, 2u);
                            SetCfgCBFuncCB(75009u, 0u);
                        }
                        else
                        {
                            SetCfgCBFuncCB(75009u, 0u);
                            SetCfgCBFuncCB(75010u, 0u);
                        }
                        SendCfgDataCBFuncCB(512u);
                    }
                    break;
                case 3:
                    if (!ToolCfg.UpdateAdjState)
                    {
                        if (IsOn)
                        {
                            SetCfgCBFuncCB(75010u, 2u);
                            SetCfgCBFuncCB(75009u, 1u);
                        }
                        else
                        {
                            SetCfgCBFuncCB(75009u, 0u);
                            SetCfgCBFuncCB(75010u, 0u);
                        }
                        SendCfgDataCBFuncCB(512u);
                        
                    }
                    break;


            }
        }
    }
}
