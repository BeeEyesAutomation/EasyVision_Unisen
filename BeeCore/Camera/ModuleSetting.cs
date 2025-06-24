using System;

namespace ModuleSetting_n
{
	public class ModuleSetting
	{
		public const uint DEFALUT_EXP_TARGET = 112u;

		public const uint DEFALUT_EXP_TIME = 0u;

		public const uint DEFALUT_SENSOR_CONTRAST = 128u;

		public const uint Config_0x00 = 0u;

		public const uint TrigMode = 3u;

		public const uint TrigMode_Manual = 0u;

		public const uint TrigMode_CMD = 1u;

		public const uint TrigMode_Continuity = 2u;

		public const uint TrigMode_Sense = 3u;

		public const uint LightLEDMode = 12u;

		public const uint LightLEDMode_None = 0u;

		public const uint LightLEDMode_Normal = 4u;

		public const uint LightLEDMode_alway = 12u;

		public const uint FocuseLEDMode = 48u;

		public const uint FocuseLEDMode_None = 0u;

		public const uint FocuseLEDMode_Normal = 16u;

		public const uint FocuseLEDMode_Blink = 32u;

		public const uint FocuseLEDMode_alway = 48u;

		public const uint MuteMode = 64u;

		public const uint MuteMode_Enable = 0u;

		public const uint MuteMode_Disable = 64u;

		public const uint DecodeLED = 128u;

		public const uint DecodeLED_Disable = 0u;

		public const uint DecodeLED_Enable = 128u;

		public const uint Config_0x02 = 2u;

		public const uint MOTOR_TIPS = 640u;

		public const uint MOTOR_TIPS_Disable = 0u;

		public const uint MOTOR_TIPS_Enable = 128u;

		public const uint StateStrOut = 576u;

		public const uint StateStrOut_Disable = 0u;

		public const uint StateStrOut_Enable = 64u;

		public const uint USB_HID_Period = 543u;

		public const uint UnicodeEndianSet = 544u;

		public const uint UnicodeEndianBig = 0u;

		public const uint UnicodeEndianLittle = 32u;

		public const uint Config_0x03 = 3u;

		public const uint SettingCodeMode = 770u;

		public const uint SettingCodeMode_Disable = 2u;

		public const uint SettingCodeMode_Enable = 0u;

		public const uint SettingCodeOutput = 769u;

		public const uint SettingCodeOutput_Disable = 0u;

		public const uint SettingCodeOutput_Enable = 1u;

		public const uint MagicCodeOutput = 772u;

		public const uint MagicCodeOutput_Disable = 0u;

		public const uint MagicCodeOutput_Enable = 4u;

		public const uint InvoiceModeOtherWay = 776u;

		public const uint InvoiceModeOtherWay_Disable = 0u;

		public const uint InvoiceModeOtherWay_Enable = 8u;

		public const uint HexOnOneCodeOutput = 784u;

		public const uint HexOnOneCodeOutput_Disable = 0u;

		public const uint HexOnOneCodeOutput_Enable = 16u;

		public const uint DPM_Mode_USE = 864u;

		public const uint DPM_Mode_USE_A = 0u;

		public const uint DPM_Mode_USE_B = 32u;

		public const uint DPM_Mode_USE_C = 64u;

		public const uint DPM_Mode_USE_B_A = 96u;

		public const uint BlackAndWhiteAlt_Mode = 896u;

		public const uint BlackAndWhiteAlt_Enable = 128u;

		public const uint BlackAndWhiteAlt_Disable = 0u;

		public const uint Config_0x04 = 4u;

		public const uint LightLED_PWM_SET = 1279u;

		public const uint Config_0x05 = 5u;

		public const uint DecodeIntervalTime = 1535u;

		public const uint Config_0x06 = 6u;

		public const uint DecodeKeepTime = 1791u;

		public const uint Config_0x07 = 7u;

		public const uint AutoSleepMode = 1920u;

		public const uint AutoSleepMode_Enable = 128u;

		public const uint AutoSleepMode_Disable = 0u;

		public const uint AutoSleepTime_MSB = 1919u;

		public const uint Config_0x08 = 8u;

		public const uint AutoSleepTime_LSB = 2303u;

		public const uint Config_0x09 = 9u;

		public const uint MirrorImage = 2307u;

		public const uint MirrorImage_Enable = 1u;

		public const uint MirrorImage_Disable = 0u;

		public const uint OppositeImage = 2308u;

		public const uint OppositeImage_Enable = 4u;

		public const uint OppositeImage_Disable = 0u;

		public const uint ContrastEnhance_Mode = 2312u;

		public const uint ContrastEnhance_Enable = 8u;

		public const uint ContrastEnhance_Disable = 0u;

		public const uint ImgProc_Mode = 2320u;

		public const uint ImgProcOldMode = 16u;

		public const uint ImgProcNewMode = 0u;

		public const uint EqualizeHist_Mode = 2336u;

		public const uint EqualizeHist_Enable = 32u;

		public const uint EqualizeHist_Disable = 0u;

		public const uint GaussBlur_Mode = 2368u;

		public const uint GaussBlur_Enable = 64u;

		public const uint GaussBlur_Disable = 0u;

		public const uint MirrorAlt_Mode = 2432u;

		public const uint MirrorAlt_Enable = 128u;

		public const uint MirrorAlt_Disable = 0u;

		public const uint ImageOutputWay = 2496u;

		public const uint ImageOutputByUSB_COM = 0u;

		public const uint ImageOutputByUSB_HID = 64u;

		public const uint ImageOutputByUSB_CIM = 128u;

		public const uint ImageOutputByUART = 192u;

		public const uint Config_0x0A = 10u;

		public const uint BeepKeepFreq = 2815u;

		public const uint Config_0x0B = 11u;

		public const uint BeepKeepTime = 3071u;

		public const uint Config_0x0C = 12u;

		public const uint BeepPolarity = 3073u;

		public const uint BeepPolarity_Hight = 1u;

		public const uint BeepPolarity_Low = 0u;

		public const uint TipsLedPolarity = 3074u;

		public const uint TipsLedPolarity_Hight = 2u;

		public const uint TipsLedPolarity_Low = 0u;

		public const uint HID_SPEED_TYPE = 3076u;

		public const uint HID_SPEED_FAST = 4u;

		public const uint HID_SPEED_STD = 0u;

		public const uint Plat_LED_Adj = 3080u;

		public const uint Plat_LED_Adj_Enable = 8u;

		public const uint Plat_LED_Adj_Disable = 0u;

		public const uint HID_Host_TYPE = 3120u;

		public const uint HID_Host_Windows = 0u;

		public const uint HID_Host_Linux = 16u;

		public const uint HID_Host_Android = 32u;

		public const uint HID_Host_MacOs = 48u;

		public const uint ExtraLEDPolarity = 3136u;

		public const uint ExtraLEDPolarity_Hight = 64u;

		public const uint ExtraLEDPolarity_Low = 0u;

		public const uint ExtraTipsPolarity = 3200u;

		public const uint ExtraTipsPolarity_Hight = 128u;

		public const uint ExtraTipsPolarity_Low = 0u;

		public const uint Config_0x0D = 13u;

		public const uint OutputMode = 3331u;

		public const uint OutputMode_RS232 = 0u;

		public const uint OutputMode_USB_HID = 1u;

		public const uint OutputMode_Reserve = 2u;

		public const uint OutputMode_USB_COM = 3u;

		public const uint OutputDataEncode = 3340u;

		public const uint OutputDataEncode_GBK = 0u;

		public const uint OutputDataEncode_UNICODE = 4u;

		public const uint OutputDataEncode_AUTO = 8u;

		public const uint OutputDataEncode_UTF8 = 12u;

		public const uint InputDataEncode = 3376u;

		public const uint InputDataEncode_GBK = 0u;

		public const uint InputDataEncode_UNICODE = 16u;

		public const uint InputDataEncode_AUTO = 32u;

		public const uint InputDataEncode_UTF8 = 48u;

		public const uint InvoiceMode = 3456u;

		public const uint InvoiceModeEnable = 128u;

		public const uint InvoiceModeDisable = 0u;

		public const uint ExtAsciiMode = 3392u;

		public const uint ExtAsciiCJK = 64u;

		public const uint ExtAsciiOther = 0u;

		public const uint Config_0x0E = 14u;

		public const uint DecodeBeep = 3588u;

		public const uint DecodeBeep_Enable = 4u;

		public const uint DecodeBeep_Disable = 0u;

		public const uint ExtraLED = 3585u;

		public const uint ExtraLED_Enable = 1u;

		public const uint ExtraLED_Disable = 0u;

		public const uint GeneralTicketMode = 3586u;

		public const uint GeneralTicketEnable = 2u;

		public const uint GeneralTicketDisable = 0u;

		public const uint PlatIntervalMode = 3592u;

		public const uint PlatIntervalEnable = 8u;

		public const uint PlatIntervalDisable = 0u;

		public const uint PlatIrSenseMode = 3600u;

		public const uint PlatIrSenseEnable = 16u;

		public const uint PlatIrSenseDisable = 0u;

		public const uint LightSmartCtrlMode = 3680u;

		public const uint LightSmartCtrlNone = 0u;

		public const uint LightSmartCtrlFixAlt = 32u;

		public const uint LightSmartCtrlByGray = 64u;

		public const uint LightSmartCtrlByStatis = 96u;

		public const uint ScanMode = 3712u;

		public const uint ScanAccurateMode = 128u;

		public const uint ScanFasterMode = 0u;

		public const uint Config_0x0F = 15u;

		public const uint SensibilityAdj_Para1 = 4095u;

		public const uint Config_0x10 = 16u;

		public const uint SensibilityAdj_Para2 = 4351u;

		public const uint Config_0x11 = 17u;

		public const uint ExposureTimeSet = 4607u;

		public const uint Config_0x12 = 18u;

		public const uint ExposureTargetSet = 4863u;

		public const uint Config_0x13 = 19u;

		public const uint SameCodeDelay = 4992u;

		public const uint SameCodeDelay_Enable = 128u;

		public const uint SameCodeDelay_Disable = 0u;

		public const uint SameCodeDelayTime = 4991u;

		public const uint Config_0x14 = 20u;

		public const uint MessegeOutTime = 5375u;

		public const uint Config_0x15 = 21u;

		public const uint ExtraLED_PWM_SET = 5631u;

		public const uint Config_0x16 = 22u;

		public const uint HID_EncodeInputType = 5633u;

		public const uint HID_EncodeInputOEM = 0u;

		public const uint HID_EncodeInputWin = 1u;

		public const uint Config_0x18 = 24u;

		public const uint USB_HID_MODE = 6145u;

		public const uint USB_HID_POS = 1u;

		public const uint USB_HID_KYB = 0u;

		public const uint CAPS_CTRL_MODE = 6146u;

		public const uint CAPS_CTRL_Enable = 2u;

		public const uint CAPS_CTRL_Disable = 0u;

		public const uint Config_0x19 = 25u;

		public const uint DecodeKeepImage = 6655u;

		public const uint Config_0x1A = 26u;

		public const uint TipsLedTimeSet = 6911u;

		public const ushort Config_0x1B = 27;

		public const ushort AccExpTimeLsb = 7167;

		public const ushort Config_0x1C = 28;

		public const ushort AccExpTimeMode = 7296;

		public const ushort AccExpTimeEnable = 128;

		public const ushort AccExpTimeDisable = 0;

		public const ushort AccExpTimeMsb = 7295;

		public const ushort Config_0x1E = 30;

		public const ushort DpmDecodeCtrlMode = 7687;

		public const ushort DpmDecodeCtrlModeA = 0;

		public const ushort DpmDecodeCtrlModeB = 1;

		public const ushort DpmDecodeCtrlModeC = 2;

		public const ushort DpmDecodeCtrlModeD = 3;

		public const ushort DpmDecodeCtrlModeE = 4;

		public const ushort DpmDecodeCtrlModeF = 5;

		public const ushort DpmDecodeCtrlModeG = 6;

		public const ushort DpmDecodeCtrlModeH = 7;

		public const uint Config_0x1f = 31u;

		public const uint DPM_ImageZoom = 7951u;

		public const uint DPM_ImageOnlyMode = 7952u;

		public const uint DPM_ImageOnlyModeDisable = 0u;

		public const uint DPM_ImageOnlyModeEnable = 16u;

		public const uint Config_0x2B = 43u;

		public const uint UART_BAUDRATE_MSB = 11263u;

		public const uint Config_0x2A = 42u;

		public const uint UART_BAUDRATE_LSB = 11007u;

		public const uint Config_0x2C = 44u;

		public const uint Barcode_360 = 11265u;

		public const uint Barcode_360_Enable = 1u;

		public const uint Barcode_360_Disable = 0u;

		public const uint Barcode_OPEN = 11270u;

		public const uint Barcode_OPEN_NONE = 0u;

		public const uint Barcode_OPEN_ALL = 2u;

		public const uint Barcode_OPEN_DEAFAULT = 4u;

		public const uint DecodeRangeCenter = 11272u;

		public const uint DecodeRangeCenter_Disable = 0u;

		public const uint DecodeRangeCenter_Enable = 8u;

		public const uint MuitBarCodeMode = 11280u;

		public const uint MuitBarCode_Disable = 0u;

		public const uint MuitBarCode_Enable = 16u;

		public const uint Config_0x2D = 45u;

		public const uint RangeCenterSet = 11775u;

		public const uint Config_0x2E = 46u;

		public const uint EAN13Code = 11777u;

		public const uint EAN13Code_Enable = 1u;

		public const uint EAN13Code_Disable = 0u;

		public const uint EANAddonCode = 11778u;

		public const uint EANAddonCode_Enable = 2u;

		public const uint EANAddonCode_Disable = 0u;

		public const uint EAN_Short_QZ_Mode = 11780u;

		public const uint EAN_Short_QZ_Enable = 4u;

		public const uint EAN_Short_QZ_Disable = 0u;

		public const uint EAN_Convert_ISBN = 11784u;

		public const uint EAN_Convert_ISBN_Enable = 8u;

		public const uint EAN_Convert_ISBN_Disable = 0u;

		public const uint EAN_DeleteStart_Mode = 11792u;

		public const uint EAN_DeleteStart_Enable = 16u;

		public const uint EAN_DeleteStart_Disable = 0u;

		public const uint EANAddonForce = 11808u;

		public const uint EANAddonForce_Enable = 32u;

		public const uint EANAddonForce_Disable = 0u;

		public const uint Config_0x2F = 47u;

		public const uint EAN8Code = 12033u;

		public const uint EAN8Code_Enable = 1u;

		public const uint EAN8Code_Disable = 0u;

		public const uint Config_0x30 = 48u;

		public const uint UPCACode = 12289u;

		public const uint UPCACode_Enable = 1u;

		public const uint UPCACode_Disable = 0u;

		public const uint UPCA_Expand_EAN = 12290u;

		public const uint UPCA_Expand_EAN_Enable = 2u;

		public const uint UPCA_Expand_EAN_Disable = 0u;

		public const uint Config_0x31 = 49u;

		public const uint UPCE0Code = 12545u;

		public const uint UPCE0Code_Enable = 1u;

		public const uint UPCE0Code_Disable = 0u;

		public const uint UPCE_Expand_EAN = 12546u;

		public const uint UPCE_Expand_EAN_Enable = 2u;

		public const uint UPCE_Expand_EAN_Disable = 0u;

		public const uint UPCE_Expand_UPCA = 12548u;

		public const uint UPCE_Expand_UPCA_Enable = 4u;

		public const uint UPCE_Expand_UPCA_Disable = 0u;

		public const uint UPCE_DeleteStart_Mode = 12560u;

		public const uint UPCE_DeleteStart_Enable = 16u;

		public const uint UPCE_DeleteStart_Disable = 0u;

		public const uint Config_0x32 = 50u;

		public const uint UPCE1Code = 12801u;

		public const uint UPCE1Code_Enable = 1u;

		public const uint UPCE1Code_Disable = 0u;

		public const uint Config_0x33 = 51u;

		public const uint Code128Code = 13057u;

		public const uint Code128Code_Enable = 1u;

		public const uint Code128Code_Disable = 0u;

		public const uint CODE128_Short_QZ_Mode = 13060u;

		public const uint CODE128_Short_QZ_Enable = 4u;

		public const uint CODE128_Short_QZ_Disable = 0u;

		public const uint CODE128_DataCrop = 13120u;

		public const uint CODE128_DataCrop_Disable = 0u;

		public const uint CODE128_DataCrop_Enable = 64u;

		public const uint CODE128_DataLenCheck = 13184u;

		public const uint CODE128_DataLenCheck_Disable = 0u;

		public const uint CODE128_DataLenCheck_Enable = 128u;

		public const uint Config_0x34 = 52u;

		public const uint Code128_MIN_LEN = 13567u;

		public const uint Config_0x35 = 53u;

		public const uint Code128_MAX_LEN = 13823u;

		public const uint Config_0x36 = 54u;

		public const uint Code39Code = 13825u;

		public const uint Code39Code_Enable = 1u;

		public const uint Code39Code_Disable = 0u;

		public const uint Code39_NoAsterisk = 13826u;

		public const uint Code39_NoAsteriskEnable = 2u;

		public const uint Code39_NoAsteriskDisable = 0u;

		public const uint CODE39_Short_QZ_Mode = 13828u;

		public const uint CODE39_Short_QZ_Enable = 4u;

		public const uint CODE39_Short_QZ_Disable = 0u;

		public const uint CODE39_FullAscii_Mode = 13832u;

		public const uint CODE39_FullAscii_Enable = 8u;

		public const uint CODE39_FullAscii_Disable = 0u;

		public const uint CODE39_DataCrop = 13888u;

		public const uint CODE39_DataCrop_Disable = 0u;

		public const uint CODE39_DataCrop_Enable = 64u;

		public const uint CODE39_DataLenCheck = 13952u;

		public const uint CODE39_DataLenCheck_Disable = 0u;

		public const uint CODE39_DataLenCheck_Enable = 128u;

		public const uint Config_0x37 = 55u;

		public const uint Code39_MIN_LEN = 14335u;

		public const uint Config_0x38 = 56u;

		public const uint Code39_MAX_LEN = 14591u;

		public const uint Config_0x39 = 57u;

		public const uint Code93Code = 14593u;

		public const uint Code93Code_Enable = 1u;

		public const uint Code93Code_Disable = 0u;

		public const uint CODE93_DataCrop_Disable = 0u;

		public const uint CODE93_DataCrop_Enable = 64u;

		public const uint CODE93_DataLenCheck = 14720u;

		public const uint CODE93_DataLenCheck_Disable = 0u;

		public const uint CODE93_DataLenCheck_Enable = 128u;

		public const uint Config_0x3A = 58u;

		public const uint Code93_MIN_LEN = 15103u;

		public const uint Config_0x3B = 59u;

		public const uint Code93_MAX_LEN = 15359u;

		public const uint Config_0x3C = 60u;

		public const uint CodaBarCode = 15361u;

		public const uint CodaBarCode_Enable = 1u;

		public const uint CodaBarCode_Disable = 0u;

		public const uint CodaBarStart = 15362u;

		public const uint CodaBarStaAndEnd_Enable = 2u;

		public const uint CodaBarStaAndEnd_Disable = 0u;

		public const uint CodaBar_DataCrop = 15424u;

		public const uint CodaBar_DataCrop_Disable = 0u;

		public const uint CodaBar_DataCrop_Enable = 64u;

		public const uint CodaBar_DataLenCheck = 15488u;

		public const uint CodaBar_DataLenCheck_Disable = 0u;

		public const uint CodaBar_DataLenCheck_Enable = 128u;

		public const uint Config_0x3D = 61u;

		public const uint CodaBar_MIN_LEN = 15871u;

		public const uint Config_0x3E = 62u;

		public const uint CodaBar_MAX_LEN = 16127u;

		public const uint Config_0x3F = 63u;

		public const uint QR_Code = 16129u;

		public const uint QR_Code_Enable = 1u;

		public const uint QR_Code_Disable = 0u;

		public const uint QR_DataCrop = 16192u;

		public const uint QR_DataCrop_Disable = 0u;

		public const uint QR_DataCrop_Enable = 64u;

		public const uint QR_DataLenCheck = 16256u;

		public const uint QR_DataLenCheck_Disable = 0u;

		public const uint QR_DataLenCheck_Enable = 128u;

		public const uint Config_0x40 = 64u;

		public const uint IL25_Code = 16385u;

		public const uint IL25_Code_Enable = 1u;

		public const uint IL25_Code_Disable = 0u;

		public const uint IL25_DataCrop = 16448u;

		public const uint IL25_DataCrop_Disable = 0u;

		public const uint IL25_DataCrop_Enable = 64u;

		public const uint IL25_DataLenCheck = 16512u;

		public const uint IL25_DataLenCheck_Disable = 0u;

		public const uint IL25_DataLenCheck_Enable = 128u;

		public const uint Config_0x41 = 65u;

		public const uint IL25_MIN_LEN = 16895u;

		public const uint Config_0x42 = 66u;

		public const uint IL25_MAX_LEN = 17151u;

		public const uint Config_0x43 = 67u;

		public const uint ID25_Code = 17153u;

		public const uint ID25_Code_Enable = 1u;

		public const uint ID25_Code_Disable = 0u;

		public const uint ID25_DataCrop = 17216u;

		public const uint ID25_DataCrop_Disable = 0u;

		public const uint ID25_DataCrop_Enable = 64u;

		public const uint ID25_DataLenCheck = 17280u;

		public const uint ID25_DataLenCheck_Disable = 0u;

		public const uint ID25_DataLenCheck_Enable = 128u;

		public const uint Config_0x44 = 68u;

		public const uint ID25_MIN_LEN = 17663u;

		public const uint Config_0x45 = 69u;

		public const uint ID25_MAX_LEN = 17919u;

		public const uint Config_0x46 = 70u;

		public const uint MX25_Code = 17921u;

		public const uint MX25_Code_Enable = 1u;

		public const uint MX25_Code_Disable = 0u;

		public const uint Config_0x47 = 71u;

		public const uint MX25_MIN_LEN = 18431u;

		public const uint Config_0x48 = 72u;

		public const uint MX25_MAX_LEN = 18687u;

		public const uint Config_0x49 = 73u;

		public const uint Code11_Code = 18689u;

		public const uint Code11_Code_Enable = 1u;

		public const uint Code11_Code_Disable = 0u;

		public const uint Code11_CheckMode = 18694u;

		public const uint Code11_Check_None = 0u;

		public const uint Code11_Check_OneBit = 2u;

		public const uint Code11_Check_TwoBit = 4u;

		public const uint Config_0x4A = 74u;

		public const uint Code11_MIN_LEN = 19199u;

		public const uint Config_0x4B = 75u;

		public const uint Code11_MAX_LEN = 19455u;

		public const uint Config_0x4C = 76u;

		public const uint MSI_Code = 19457u;

		public const uint MSI_Code_Enable = 1u;

		public const uint MSI_Code_Disable = 0u;

		public const uint MSI_CheckCodeOutput = 19458u;

		public const uint MSI_CheckCodeOutput_Enable = 0u;

		public const uint MSI_CheckCodeOutput_Disable = 2u;

		public const uint Config_0x4D = 77u;

		public const uint MSI_MIN_LEN = 19967u;

		public const uint Config_0x4E = 78u;

		public const uint MSI_MAX_LEN = 20223u;

		public const uint Config_0x4F = 79u;

		public const uint RSS_14_Code = 20225u;

		public const uint RSS_14_Enable = 1u;

		public const uint RSS_14_Disable = 0u;

		public const uint Config_0x50 = 80u;

		public const uint RSS_Finite_Code = 20481u;

		public const uint RSS_Finite_Enable = 1u;

		public const uint RSS_Finite_Disable = 0u;

		public const uint Config_0x51 = 81u;

		public const uint RSS_Exten_Code = 20737u;

		public const uint RSS_Exten_Enable = 1u;

		public const uint RSS_Exten_Disable = 0u;

		public const uint Config_0x52 = 82u;

		public const uint RSS_MIN_LEN = 21247u;

		public const uint Config_0x53 = 83u;

		public const uint RSS_MAX_LEN = 21503u;

		public const uint Config_0x54 = 84u;

		public const uint DataMatrix_Code = 21505u;

		public const uint DataMatrix_Enable = 1u;

		public const uint DataMatrix_Disable = 0u;

		public const uint DM_Timeout = 21506u;

		public const uint DM_Timeout_Enable = 2u;

		public const uint DM_Timeout_Disable = 0u;

		public const uint DM_Short_QZ_Mode = 21508u;

		public const uint DM_Short_QZ_Enable = 4u;

		public const uint DM_Short_QZ_Disable = 0u;

		public const uint DM_DataCrop = 21568u;

		public const uint DM_DataCrop_Disable = 0u;

		public const uint DM_DataCrop_Enable = 64u;

		public const uint DM_DataLenCheck = 21632u;

		public const uint DM_DataLenCheck_Disable = 0u;

		public const uint DM_DataLenCheck_Enable = 128u;

		public const uint Config_0x55 = 85u;

		public const uint PDF417_Code = 21761u;

		public const uint PDF417_Enable = 1u;

		public const uint PDF417_Disable = 0u;

		public const uint PDF417_DataCrop = 21824u;

		public const uint PDF417_DataCrop_Disable = 0u;

		public const uint PDF417_DataCrop_Enable = 64u;

		public const uint PDF417_DataLenCheck = 21888u;

		public const uint PDF417_DataLenCheck_Disable = 0u;

		public const uint PDF417_DataLenCheck_Enable = 128u;

		public const uint Config_0x56 = 86u;

		public const uint AZTEC_Code = 22017u;

		public const uint AZTEC_Enable = 1u;

		public const uint AZTEC_Disable = 0u;

		public const uint AZTEC_DataCrop = 22080u;

		public const uint AZTEC_DataCrop_Disable = 0u;

		public const uint AZTEC_DataCrop_Enable = 64u;

		public const uint AZTEC_DataLenCheck = 22144u;

		public const uint AZTEC_DataLenCheck_Disable = 0u;

		public const uint AZTEC_DataLenCheck_Enable = 128u;

		public const uint Config_0x57 = 87u;

		public const uint HAXIN_Code = 22273u;

		public const uint HAXIN_Enable = 1u;

		public const uint HAXIN_Disable = 0u;

		public const uint Config_0x58 = 88u;

		public const uint MICROPDF_Code = 22529u;

		public const uint MICROPDF_Enable = 1u;

		public const uint MICROPDF_Disable = 0u;

		public const uint Config_0x59 = 89u;

		public const uint TRIOPTIC_Code = 22785u;

		public const uint TRIOPTIC_Enable = 1u;

		public const uint TRIOPTIC_Disable = 0u;

		public const uint Config_0x5a = 90u;

		public const uint CODEBLOCK_F_Code = 23041u;

		public const uint CODEBLOCK_F_Enable = 1u;

		public const uint CODEBLOCK_F_Disable = 0u;

		public const uint Config_0x5b = 91u;

		public const uint STRAIGHT_Code = 23297u;

		public const uint STRAIGHT_Enable = 1u;

		public const uint STRAIGHT_Disable = 0u;

		public const uint Config_0x5c = 92u;

		public const uint TELEPEN_Code = 23553u;

		public const uint TELEPEN_Enable = 1u;

		public const uint TELEPEN_Disable = 0u;

		public const uint Config_0x5d = 93u;

		public const uint MAXICODE_Code = 23809u;

		public const uint MAXICODE_Enable = 1u;

		public const uint MAXICODE_Disable = 0u;

		public const uint Config_0x5e = 94u;

		public const uint CODE32_Code = 24065u;

		public const uint CODE32_Enable = 1u;

		public const uint CODE32_Disable = 0u;

		public const uint Config_0x5f = 95u;

		public const uint DATABAR_Code = 24321u;

		public const uint DATABAR_Code_Enable = 1u;

		public const uint DATABAR_Code_Disable = 0u;

		public const uint Config_0x60 = 96u;

		public const uint BarcodeEOF = 24577u;

		public const uint BarcodeEOF_Enable = 1u;

		public const uint BarcodeEOF_Disable = 0u;

		public const uint BarcodeSuffix = 24578u;

		public const uint BarcodeSuffix_Enable = 2u;

		public const uint BarcodeSuffix_Disable = 0u;

		public const uint BarcodeCodeID = 24580u;

		public const uint BarcodeCodeID_Enable = 4u;

		public const uint BarcodeCodeID_Disable = 0u;

		public const uint BarcodePrefix = 24584u;

		public const uint BarcodePrefix_Enable = 8u;

		public const uint BarcodePrefix_Disable = 0u;

		public const uint BarcodeRF_Info = 24592u;

		public const uint BarcodeRF_Info_Enable = 16u;

		public const uint BarcodeRF_Info_Disable = 0u;

		public const uint BarcodeEofType = 24672u;

		public const uint BarcodeEofType_CR = 0u;

		public const uint BarcodeEofType_CRLF = 32u;

		public const uint BarcodeEofType_TAB = 64u;

		public const uint BarcodeEofType_NONE = 96u;

		public const uint Config_0x61 = 97u;

		public const uint KeyBoardSet = 24847u;

		public const uint KeyBoardSet_US = 0u;

		public const uint KeyBoardSet_CZ = 1u;

		public const uint KeyBoardSet_FR = 2u;

		public const uint KeyBoardSet_GM = 3u;

		public const uint KeyBoardSet_HG = 4u;

		public const uint KeyBoardSet_IT = 5u;

		public const uint KeyBoardSet_JP = 6u;

		public const uint KeyBoardSet_SP = 7u;

		public const uint KeyBoardSet_RS = 8u;

		public const uint KeyBoardSet_BZ = 9u;

		public const uint KeyBoardSet_PT = 10u;

		public const uint KeyBoardSet_DM = 11u;

		public const uint KeyBoardSet_NW = 12u;

		public const uint KeyBoardSet_NL = 13u;

		public const uint KeyBoardSet_TK = 14u;

		public const uint KeyBoardSet_SW = 15u;

		public const uint KeyBoardUnAsciiMode = 25056u;

		public const uint KeyBoardUnAsciiNone = 0u;

		public const uint KeyBoardUnAsciiQuestion = 32u;

		public const uint KeyBoardUnAsciiSpace = 64u;

		public const uint KeyBoardUnAsciiHotKey1 = 96u;

		public const uint KeyBoardUnAsciiHotKey2 = 128u;

		public const uint KeyBoardUnAsciiHotKey3 = 160u;

		public const uint KeyBoardUnAsciiHotKey4 = 192u;

		public const uint KeyBoardUnAsciiCustom = 224u;

		public const uint Config_0x62 = 98u;

		public const uint BarcodeSufLen = 25103u;

		public const uint BarcodePreLen = 25328u;

		public const uint Config_0x63 = 99u;

		public const uint BarcodePreStr = 25599u;

		public const uint Config_0x72 = 114u;

		public const uint BarcodeSufStr = 29439u;

		public const uint Config_0x81 = 129u;

		public const uint Rf_Info_Len = 33039u;

		public const uint Config_0x82 = 130u;

		public const uint BarcodeRfStr = 33535u;

		public const uint Config_0x91 = 145u;

		public const uint BarcodeIdStr = 37375u;

		public const uint Config_0xB0 = 176u;

		public const uint DATA_StringCrop = 45059u;

		public const uint DATA_StringCrop_All = 0u;

		public const uint DATA_StringCrop_Front = 1u;

		public const uint DATA_StringCrop_After = 2u;

		public const uint DATA_StringCrop_Center = 3u;

		public const uint BarcodeDeleteMode = 45060u;

		public const uint BarcodeDeleteDisable = 0u;

		public const uint BarcodeDeleteEnable = 4u;

		public const uint Config_0xB1 = 177u;

		public const uint CropFront_LEN = 45567u;

		public const uint Config_0xB2 = 178u;

		public const uint CropAfter_LEN = 45823u;

		public const uint Config_0xB5 = 181u;

		public const uint Pharma_Code = 46337u;

		public const uint Pharma_Code_Enable = 1u;

		public const uint Pharma_Code_Disable = 0u;

		public const uint Config_0xB6 = 182u;

		public const uint OCR_Code = 46593u;

		public const uint OCR_Code_Enable = 1u;

		public const uint OCR_Code_Disable = 0u;

		public const uint OCR_MuiltLine = 46594u;

		public const uint OCR_MuiltLine_Enable = 2u;

		public const uint OCR_MuiltLine_Disable = 0u;

		public const uint Config_0xB7 = 183u;

		public const uint OCR_Code_Len = 47103u;

		public const uint Config_0xB8 = 184u;

		public const uint OCR_MICR_Len = 47359u;

		public const uint Config_0xBB = 187u;

		public const uint BarcodeLenCheckMinLSB = 48127u;

		public const uint Config_0xBC = 188u;

		public const uint BarcodeLenCheckMinMSB = 48383u;

		public const uint Config_0xBD = 189u;

		public const uint BarcodeLenCheckMaxLSB = 48639u;

		public const uint Config_0xBE = 190u;

		public const uint BarcodeLenCheckMaxMSB = 48895u;

		public const uint Config_0xBF = 191u;

		public const uint BarcodeRegionOut = 49024u;

		public const uint BarcodeRegionOutEnable = 128u;

		public const uint BarcodeRegionOutDisable = 0u;

		public const uint MuiltCodeOutWay = 49008u;

		public const uint MuiltCodeOutBySystem = 0u;

		public const uint MuiltCodeOutByLength = 16u;

		public const uint MuiltCodeOutByShort = 32u;

		public const uint MuiltCodeOutByTop = 48u;

		public const uint MuiltCodeOutByButtom = 64u;

		public const uint MuiltCodeOutByLeft = 80u;

		public const uint MuiltCodeOutByRight = 96u;

		public const uint MuiltCode_Num = 48911u;

		public const uint Config_0xC0 = 192u;

		public const uint SensorContrast = 49407u;

		public const uint Config_0xC1 = 193u;

		public const uint ScanRecognizeTimeout = 49663u;

		public const uint Config_0xC2 = 194u;

		public const uint SenseTrigTimeout = 49919u;

		public const uint Config_0xC3 = 195u;

		public const uint SenseThreshold = 50175u;

		public const uint Config_0xC4 = 196u;

		public const uint ImageCrop_WStart = 50431u;

		public const uint Config_0xC5 = 197u;

		public const uint ImageCrop_HStart = 50687u;

		public const uint Config_0xC6 = 198u;

		public const uint ImageCrop_Width = 50943u;

		public const uint Config_0xC7 = 199u;

		public const uint ImageCrop_Height = 51199u;

		public const uint Config_0xC8 = 200u;

		public const uint OV9281GrobalGain = 51455u;

		public const uint Config_0xC9 = 201u;

		public const uint DPM_Decode_Level = 51471u;

		public const uint DPM_Decode_Size = 51504u;

		public const uint DPM_Decode_Normal = 0u;

		public const uint DPM_Decode_S = 16u;

		public const uint DPM_Decode_XS = 32u;

		public const uint BarcodeStoreMode = 51584u;

		public const uint BarcodeStoreModeEnable = 128u;

		public const uint BarcodeStoreModeDisable = 0u;

		public const uint Config_0xCA = 202u;

		public const uint OV9281_SCAN_Mode = 51715u;

		public const uint OV9281_SCAN_Full = 0u;

		public const uint OV9281_SCAN_OneD = 1u;

		public const uint OV9281_SCAN_CropL = 2u;

		public const uint OV9281_SCAN_CropS = 3u;

		public const uint ImageCrop_Mode = 51740u;

		public const uint ImageCrop_Normal = 0u;

		public const uint ImageCrop_QVGA = 4u;

		public const uint ImageCrop_HVGA = 8u;

		public const uint ImageCrop_AIM1 = 12u;

		public const uint ImageCrop_AIM2 = 16u;

		public const uint ImageCrop_AIM3 = 20u;

		public const uint ImageCrop_AIM4 = 24u;

		public const uint ImageCrop_AIM5 = 28u;

		public const uint ExtraTrigMode = 51744u;

		public const uint ExtraTrigModeEnable = 32u;

		public const uint ExtraTrigModeDisable = 0u;

		public const uint ExtraTipsMode = 51776u;

		public const uint ExtraTipsSucess = 64u;

		public const uint ExtraTipsFailed = 0u;

		public const uint LedIntervalOffMode = 51840u;

		public const uint LedIntervalOffEnable = 128u;

		public const uint LedIntervalOffDisable = 0u;

		public const uint Config_0xCB = 203u;

		public const uint Decode_FocusLine_Mode = 52160u;

		public const uint Decode_FocusLine_Disable = 0u;

		public const uint Decode_FocusLine_Mode1 = 64u;

		public const uint Decode_FocusLine_Mode2 = 128u;

		public const uint Decode_FocusLine_Mode3 = 192u;

		public const uint Decode_FocusLine_OFFSET = 52031u;

		public const uint Config_0xCC = 204u;

		public const uint DPM_Tool_Mode = 52225u;

		public const uint DPM_Tool_Mode_Enable = 1u;

		public const uint DPM_Tool_Mode_Disable = 0u;

		public const uint DPM_Tool_ImgType = 52238u;

		public const uint DPM_Tool_ImgType_PNG = 0u;

		public const uint DPM_Tool_ImgType_JPG = 2u;

		public const uint DPM_Tool_ImgType_BMP8 = 4u;

		public const uint DPM_Tool_ImgType_RGB16 = 6u;

		public const uint DPM_Tool_ImgType_TIFF = 8u;

		public const uint DPM_Tool_ImgType_YUV16 = 10u;

		public const uint DPM_Tool_ImgType_GRAY8 = 12u;

		public const uint DPM_Tool_ImgType_RAW8 = 14u;

		public const uint DPM_Tool_SendWay = 52272u;

		public const uint DPM_Tool_SendByUSB_COM = 0u;

		public const uint DPM_Tool_SendByUSB_HID = 16u;

		public const uint DPM_Tool_SendByRS232 = 32u;

		public const uint DPM_Tool_SendByNETWORK = 48u;

		public const uint DPM_Tool_SendImage = 52288u;

		public const uint DPM_Tool_SendImage_Enable = 64u;

		public const uint DPM_Tool_SendImage_Disable = 0u;

		public const uint Config_0xD0 = 208u;

		public const uint TrigAckMode = 53249u;

		public const uint TrigAckMode_Enable = 1u;

		public const uint TrigAckMode_Disable = 0u;

		public const uint SingleCmdTrigMode = 53250u;

		public const uint SingleCmdTrig_Enable = 2u;

		public const uint SingleCmdTrig_Disable = 0u;

		public const uint SameCodeErrTipsMode = 53252u;

		public const uint SameCodeErrTipsDisable = 0u;

		public const uint SameCodeErrTipsEnable = 4u;

		public const uint EditCmdTrigLen = 53360u;

		public const uint EditCmdTrigLen0 = 0u;

		public const uint EditCmdTrigLen1 = 16u;

		public const uint EditCmdTrigLen2 = 32u;

		public const uint EditCmdTrigLen3 = 48u;

		public const uint EditCmdTrigLen4 = 64u;

		public const uint EditCmdTrigLen5 = 80u;

		public const uint EditCmdTrigLen6 = 96u;

		public const uint EditCmdTrigLen7 = 112u;

		public const uint DM_LowContrastMode = 53376u;

		public const uint DM_LowContrastModeDisable = 0u;

		public const uint DM_LowContrastModeEnable = 128u;

		public const uint Config_0xD1 = 209u;

		public const uint SingleCmdTrigValue = 53759u;

		public const uint Config_0xD4 = 212u;

		public const uint ImageROI_WStart = 54527u;

		public const uint Config_0xD5 = 213u;

		public const uint ImageROI_HStart = 54783u;

		public const uint Config_0xD6 = 214u;

		public const uint ImageROI_Width = 55039u;

		public const uint Config_0xD7 = 215u;

		public const uint ImageROI_Height = 55295u;

		public const uint Config_0xD8 = 216u;

		public const uint ImageROI_Mode = 55297u;

		public const uint ImageROI_Mode_Enable = 1u;

		public const uint ImageROI_Mode_Disable = 0u;

		public const uint ExtraSingleOutMode = 55424u;

		public const uint ExtraSingleOutEnable = 128u;

		public const uint ExtraSingleOutDisable = 0u;

		public const uint ExtraSingleOutPol = 55360u;

		public const uint ExtraSingleOutPolHight = 64u;

		public const uint ExtraSingleOutPolLow = 0u;

		public const uint ExtraSingleOutType = 55328u;

		public const uint ExtraSingleOutSucess = 32u;

		public const uint ExtraSingleOutFailed = 0u;

		public const uint USB_DATA_NoOutMode = 55312u;

		public const uint USB_DATA_NoOutModeEnable = 16u;

		public const uint USB_DATA_NoOutModeDisable = 0u;

		public const uint Config_0xD9 = 217u;

		public const uint SetToScanner = 55807u;

		public const uint LoadFactoryMode = 80u;

		public const uint LoadUserSettingMode = 85u;

		public const uint SaveCurrenSetting = 86u;

		public const uint EnterMildSleepMode = 160u;

		public const uint EnterDeepSleepMode = 165u;

		public const uint ScannerWeakUpMode = 0u;

		public const uint ScannerSoftwareStateOut = 1u;

		public const uint ScannerHardwareStateOut = 2u;

		public const uint ScannerImageStateOut = 3u;

		public const uint Config_0xDA = 218u;

		public const uint SetAdjustValue = 56063u;

		public const uint SetAdd_LightLED = 1u;

		public const uint SetDec_LightLED = 2u;

		public const uint SetRst_LightLED = 3u;

		public const uint SetAdd_Exposure = 17u;

		public const uint SetDec_Exposure = 18u;

		public const uint SetRst_Exposure = 19u;

		public const uint SetAdd_AeTarget = 33u;

		public const uint SetDec_AeTarget = 34u;

		public const uint SetRst_AeTarget = 35u;

		public const uint SetAdd_SensorGain = 49u;

		public const uint SetDec_SensorGain = 50u;

		public const uint SetRst_SensorGain = 51u;

		public const uint SetAdd_Contrast = 65u;

		public const uint SetDec_Contrast = 66u;

		public const uint SetRst_Contrast = 67u;

		public const uint SetAdd_FocusLen = 81u;

		public const uint SetDec_FocusLen = 82u;

		public const uint SetRst_FocusLen = 83u;

		public const uint SetAdd_BeepFreq = 97u;

		public const uint SetDec_BeepFreq = 98u;

		public const uint SetRst_BeepFreq = 99u;

		public const uint Config_0xDB = 219u;

		public const uint ImgGaussBlurPara = 56319u;

		public const uint Config_0xDC = 220u;

		public const uint ImgContrastPointX0 = 56575u;

		public const uint Config_0xDD = 221u;

		public const uint ImgContrastPointY0 = 56831u;

		public const uint Config_0xDE = 222u;

		public const uint ImgContrastPointX1 = 57087u;

		public const uint Config_0xDF = 223u;

		public const uint ImgContrastPointY1 = 57343u;

		public const uint ReadSannerName = 57599u;

		public const uint ReadHardwareVer = 57855u;

		public const uint ReadSorfwareVer = 58111u;

		public const uint EditTrigCMD0 = 58367u;

		public const uint EditTrigCMD1 = 58623u;

		public const uint EditTrigCMD2 = 58879u;

		public const uint EditTrigCMD3 = 59135u;

		public const uint EditTrigCMD4 = 52735u;

		public const uint EditTrigCMD5 = 52991u;

		public const uint EditTrigCMD6 = 53247u;

		public const uint Config_0xE7 = 231u;

		public const uint BarcodeDeleteSet = 59391u;

		public const uint Config_0xE8 = 232u;

		public const uint KeyBoardUnAsciiSet = 59647u;

		public const uint Config_0xE9 = 233u;

		public const uint UART_PolarityMode = 59649u;

		public const uint UART_PolarityDisable = 0u;

		public const uint UART_PolarityEnable = 1u;

		public const uint UART_PolaritySet = 59654u;

		public const uint UART_PolarityEven = 0u;

		public const uint UART_PolarityODD = 2u;

		public const uint UART_PolarityMask = 4u;

		public const uint UART_PolaritySpace = 6u;

		public const uint UART_StopBitSet = 59672u;

		public const uint UART_StopBit1_0 = 0u;

		public const uint UART_StopBit1_5 = 8u;

		public const uint UART_StopBit2_0 = 16u;

		public const uint UART_DataBitLenSet = 59744u;

		public const uint UART_DataBitLen5 = 0u;

		public const uint UART_DataBitLen6 = 32u;

		public const uint UART_DataBitLen7 = 64u;

		public const uint UART_DataBitLen8 = 96u;

		public const uint Custom_VID_MSB = 60159u;

		public const uint Custom_VID_LSB = 60415u;

		public const uint Custom_PID_MSB = 60671u;

		public const uint Custom_PID_LSB = 60927u;

		public const uint Custom_REV_MSB = 61183u;

		public const uint Custom_REV_LSB = 61439u;

		public const uint Config_0xF0 = 240u;

		public const uint ScanControlMode = 61443u;

		public const uint ScanControlByDefault = 0u;

		public const uint ScanControlByCustom = 1u;

		public const uint ScanTemplateFloop = 61692u;

		public const uint UserSensorGain0 = 61951u;

		public const uint UserSensorGain1 = 62207u;

		public const uint UserSensorGain2 = 62463u;

		public const uint UserSensorExp0 = 62719u;

		public const uint UserSensorExp1 = 62975u;

		public const uint UserSensorExp2 = 63231u;

		public const uint UserLightLED0 = 63487u;

		public const uint UserLightLED1 = 63743u;

		public const uint UserLightLED2 = 63999u;

		public const uint UserExtraLED0 = 64255u;

		public const uint UserExtraLED1 = 64511u;

		public const uint UserExtraLED2 = 64767u;

		public const uint UserDpmModeSel0 = 65008u;

		public const uint UserDpmModeSel1 = 65264u;

		public const uint UserDpmModeSel2 = 65520u;

		public const uint Config_0x100 = 256u;

		public const uint CONST_VALUE_0X55 = 65791u;

		public const uint Config_0x101 = 257u;

		public const uint CONST_VALUE_0XAA = 66047u;

		public const uint Config_0x102 = 258u;

		public const uint EXTRA_CFG_FLAG_LSB = 66303u;

		public const uint Config_0x103 = 259u;

		public const uint EXTRA_CFG_FLAG_MSB = 66559u;

		public const uint Config_0x108 = 264u;

		public const uint H295_LightSel = 67585u;

		public const uint H295_LightSelNear = 0u;

		public const uint H295_LightSelFar = 1u;

		public const uint Config_0x10B = 267u;

		public const uint CMD_Extra_Mode = 68355u;

		public const uint CMD_Extra_Normal = 0u;

		public const uint CMD_Extra_TryAllGetBest = 1u;

		public const uint CMD_Extra_TryAllOutputAll = 2u;

		public const uint Config_0x110 = 272u;

		public const uint ImgProcMedianFilterNum = 69647u;

		public const uint ImgProcMedianFilterAgain = 69872u;

		public const uint Config_0x111 = 273u;

		public const uint ImgProcEqualizeHistNum = 69903u;

		public const uint ImgProcEqualizeHistAgain = 70128u;

		public const uint Config_0x112 = 274u;

		public const uint ImgProcGaussBlurNum = 70159u;

		public const uint ImgProcGaussBlurAgain = 70384u;

		public const uint Config_0x113 = 275u;

		public const uint ImgProcMorphDilateNum = 70415u;

		public const uint ImgProcMorphDilateAgain = 70640u;

		public const uint Config_0x114 = 276u;

		public const uint ImgProcMorphErodeNum = 70671u;

		public const uint ImgProcMorphErodeAgain = 70896u;

		public const uint Config_0x115 = 277u;

		public const uint ImgProcContrastEnhanceNum = 70927u;

		public const uint ImgProcContrastEnhanceAgain = 71152u;

		public const uint Config_0x120 = 288u;

		public const uint HightLightLED_Mode = 73731u;

		public const uint HightLightLED_AllLight = 0u;

		public const uint HightLightLED_OnlyLeft = 1u;

		public const uint HightLightLED_OnlyRight = 2u;

		public const uint HightLightLED_AutoAlt = 3u;

		public const uint Config_0x121 = 289u;

		public const uint SensorFoucus_Mode = 73987u;

		public const uint SensorFoucus_None = 0u;

		public const uint SensorFoucus_AutoByMTF1 = 1u;

		public const uint SensorFoucus_AutoByMTF2 = 2u;

		public const uint SensorFoucus_AutoByMTF3 = 3u;

		public const uint Config_0x122 = 290u;

		public const uint SensorFoucusAdjLsb = 74495u;

		public const uint Config_0x123 = 291u;

		public const uint SensorFoucusAdjMsb = 74751u;

		public const uint Config_0x124 = 292u;

		public const uint FarLeftTop_LED_Mode = 74753u;

		public const uint FarLeftTop_LED_Enable = 1u;

		public const uint FarLeftTop_LED_Disable = 0u;

		public const uint FarLeftButtom_LED_Mode = 74754u;

		public const uint FarLeftButtom_LED_Enable = 2u;

		public const uint FarLeftButtom_LED_Disable = 0u;

		public const uint FarRightButtom_LED_Mode = 74756u;

		public const uint FarRightButtom_LED_Enable = 4u;

		public const uint FarRightButtom_LED_Disable = 0u;

		public const uint FarRightTop_LED_Mode = 74760u;

		public const uint FarRightTop_LED_Enable = 8u;

		public const uint FarRightTop_LED_Disable = 0u;

		public const uint Config_0x125 = 293u;

		public const uint NearLeftTop_LED_Mode = 75009u;

		public const uint NearLeftTop_LED_Enable = 1u;

		public const uint NearLeftTop_LED_Disable = 0u;

		public const uint NearLeftButtom_LED_Mode = 75010u;

		public const uint NearLeftButtom_LED_Enable = 2u;

		public const uint NearLeftButtom_LED_Disable = 0u;

		public const uint NearRightButtom_LED_Mode = 75012u;

		public const uint NearRightButtom_LED_Enable = 4u;

		public const uint NearRightButtom_LED_Disable = 0u;

		public const uint NearRightTop_LED_Mode = 75016u;

		public const uint NearRightTop_LED_Enable = 8u;

		public const uint NearRightTop_LED_Disable = 0u;

		public const uint Config_0x127 = 295u;

		public const uint Sensor_HDR_Mode = 75523u;

		public const uint Sensor_HDR_ModeNone = 0u;

		public const uint Sensor_HDR_ModeUseDiffExp = 1u;

		public const uint Sensor_HDR_ModeUseDiffRatio = 2u;

		public const uint Config_0x128 = 296u;

		public const uint SensorHDR_LongTimeLsb = 76031u;

		public const uint Config_0x129 = 297u;

		public const uint SensorHDR_LongTimeMsb = 76287u;

		public const uint Config_0x12A = 298u;

		public const uint SensorHDR_ShortTimeLsb = 76543u;

		public const uint Config_0x12B = 299u;

		public const uint SensorHDR_ShortTimeMsb = 76799u;

		public const uint Config_0x12C = 300u;

		public const uint SensorHDR_LightExp = 77055u;

		public const uint Config_0x130 = 304u;

		public const uint RecordRuntimeLogMode = 77952u;

		public const uint RecordRuntimeLog_Enable = 128u;

		public const uint RecordRuntimeLog_Disable = 0u;

		public const uint RecordRuntimeLogWay = 77920u;

		public const uint RecordRuntimeLogToFiles = 0u;

		public const uint RecordRuntimeLogToConsole = 32u;

		public const uint RecordRuntimeLogLevelSel = 77831u;

		public const uint RecordRuntimeLogLevelAssert = 0u;

		public const uint RecordRuntimeLogLevelErrorH = 1u;

		public const uint RecordRuntimeLogLevelErrorM = 2u;

		public const uint RecordRuntimeLogLevelErrorL = 3u;

		public const uint RecordRuntimeLogLevelWerning = 4u;

		public const uint RecordRuntimeLogLevelTest = 5u;

		public const uint RecordRuntimeLogLevelInfo = 6u;

		public const uint RecordRuntimeLogLevelDebug = 7u;

		public const uint Config_0x131 = 305u;

		public const uint ToolDebugSendLastTrigMode = 78081u;

		public const uint ToolDebugSendLastTrigEnable = 1u;

		public const uint ToolDebugSendLastTrigDisable = 0u;

		public const uint Config_0x140 = 320u;

		public const uint ExtraTipsTimeSet = 82175u;

		public const uint Config_0x141 = 321u;

		public const uint ExtraSignalTimeSet = 82431u;

		public const uint Config_0x150 = 336u;

		public const uint ModBusModeSel = 86019u;

		public const uint ModBusModeNone = 0u;

		public const uint ModBusModeRtu = 1u;

		public const uint ModBusModeAscii = 2u;

		public const uint ModBusModeTcp = 3u;

		public const uint ModBusEndianSel = 86032u;

		public const uint ModBusEndianBig = 0u;

		public const uint ModBusEndianLittle = 16u;

		public const uint EtherNetIP_Mode = 86048u;

		public const uint EtherNetIP_Disable = 0u;

		public const uint EtherNetIP_Enable = 32u;

		public const uint ProfileNet_Mode = 86080u;

		public const uint ProfileNet_Disable = 0u;

		public const uint ProfileNet_Enable = 64u;

		public const uint MelseC_SLMP_Mode = 86144u;

		public const uint MelseC_SLMP_Disable = 0u;

		public const uint MelseC_SLMP_Enable = 128u;

		public const uint Config_0x151 = 337u;

		public const uint ModBusAddrSet = 86527u;

		public const uint Config_0x152 = 338u;

		public const uint ModBusDiscreteRegOffsetLsb = 86783u;

		public const uint Config_0x153 = 339u;

		public const uint ModBusDiscreteRegOffsetMsb = 87039u;

		public const uint Config_0x154 = 340u;

		public const uint ModBusCoilRegOffsetLsb = 87295u;

		public const uint Config_0x155 = 341u;

		public const uint ModBusCoilRegOffsetMsb = 87551u;

		public const uint Config_0x156 = 342u;

		public const uint ModBusInputRegOffsetLsb = 87807u;

		public const uint Config_0x157 = 343u;

		public const uint ModBusInputRegOffsetMsb = 88063u;

		public const uint Config_0x158 = 344u;

		public const uint ModBusHoldingRegOffsetLsb = 88319u;

		public const uint Config_0x159 = 345u;

		public const uint ModBusHoldingRegOffsetMsb = 88575u;

		public const uint Config_0x15A = 346u;

		public const uint ModBusTcpPortLsb = 88831u;

		public const uint Config_0x15B = 347u;

		public const uint ModBusTcpPortMsb = 89087u;

		public const uint Config_0x15C = 348u;

		public const uint MC_TCP_Port_Lsb = 89343u;

		public const uint Config_0x15D = 349u;

		public const uint MC_TCP_Port_Msb = 89599u;

		public const uint Config_0x15E = 350u;

		public const uint MC_DataRangeToKeyenceMode = 89601u;

		public const uint MC_DataRangeToKeyenceEnable = 1u;

		public const uint MC_DataRangeToKeyenceDisable = 0u;

		public const uint Config_0x160 = 352u;

		public const uint DecoderROI_Mode = 90113u;

		public const uint DecoderROI_Enable = 1u;

		public const uint DecoderROI_Disable = 0u;

		public const uint SensorProcROI_Mode = 90114u;

		public const uint SensorProcROI_Enable = 2u;

		public const uint SensorProcROI_Disable = 0u;

		public const uint Config_0x161 = 353u;

		public const uint DecoderROI_X0 = 90623u;

		public const uint Config_0x162 = 354u;

		public const uint DecoderROI_Y0 = 90879u;

		public const uint Config_0x163 = 355u;

		public const uint DecoderROI_X1 = 91135u;

		public const uint Config_0x164 = 356u;

		public const uint DecoderROI_Y1 = 91391u;

		public const uint Config_0x165 = 357u;

		public const uint SensorProcROI_X0 = 91647u;

		public const uint Config_0x166 = 358u;

		public const uint SensorProcROI_Y0 = 91903u;

		public const uint Config_0x167 = 359u;

		public const uint SensorProcROI_X1 = 92159u;

		public const uint Config_0x168 = 360u;

		public const uint SensorProcROI_Y1 = 92415u;

		public const uint Config_0x200 = 512u;

		public const uint DeviceNetworkMode = 131079u;

		public const uint DeviceNetworkTcpClient = 0u;

		public const uint DeviceNetworkTcpServer = 1u;

		public const uint DeviceNetworkUdpClient = 2u;

		public const uint DeviceNetworkUdpServer = 3u;

		public const uint DeviceNetworkUdpHttpPost = 4u;

		public const uint DeviceMacRandom = 131080u;

		public const uint DeviceMacRandomDisable = 0u;

		public const uint DeviceMacRandomEnable = 8u;

		public const uint DeviceIP_DhcpMode = 131120u;

		public const uint DeviceIP_DhcpNone = 0u;

		public const uint DeviceIP_DhcpClient = 16u;

		public const uint DeviceIP_DhcpServer = 32u;

		public const uint DeviceNetLossCheckMode = 131200u;

		public const uint DeviceNetLossCheckEnable = 128u;

		public const uint DeviceNetLossCheckDisable = 0u;

		public const uint Config_0x201 = 513u;

		public const uint TcpUseMuiltClientMode = 131344u;

		public const uint TcpUseMuiltClientModeEnable = 16u;

		public const uint TcpUseMuiltClientModeDisable = 0u;

		public const uint TcpForceOneClientMode = 131360u;

		public const uint TcpForceOneClientModeEnable = 32u;

		public const uint TcpForceOneClientModeDisable = 0u;

		public const uint Config_0x202 = 514u;

		public const uint DeviceIP_SEG0_SET = 131839u;

		public const uint Config_0x203 = 515u;

		public const uint DeviceIP_SEG1_SET = 132095u;

		public const uint Config_0x204 = 516u;

		public const uint DeviceIP_SEG2_SET = 132351u;

		public const uint Config_0x205 = 517u;

		public const uint DeviceIP_SEG3_SET = 132607u;

		public const uint Config_0x206 = 518u;

		public const uint DeviceGW_SEG0_SET = 132863u;

		public const uint Config_0x207 = 519u;

		public const uint DeviceGW_SEG1_SET = 133119u;

		public const uint Config_0x208 = 520u;

		public const uint DeviceGW_SEG2_SET = 133375u;

		public const uint Config_0x209 = 521u;

		public const uint DeviceGW_SEG3_SET = 133631u;

		public const uint Config_0x20A = 522u;

		public const uint DeviceMASK_SEG0_SET = 133887u;

		public const uint Config_0x20B = 523u;

		public const uint DeviceMASK_SEG1_SET = 134143u;

		public const uint Config_0x20C = 524u;

		public const uint DeviceMASK_SEG2_SET = 134399u;

		public const uint Config_0x20D = 525u;

		public const uint DeviceMASK_SEG3_SET = 134655u;

		public const uint Config_0x210 = 528u;

		public const uint DeviceMAC_SEG0_SET = 135423u;

		public const uint Config_0x211 = 529u;

		public const uint DeviceMAC_SEG1_SET = 135679u;

		public const uint Config_0x212 = 530u;

		public const uint DeviceMAC_SEG2_SET = 135935u;

		public const uint Config_0x213 = 531u;

		public const uint DeviceMAC_SEG3_SET = 136191u;

		public const uint Config_0x214 = 532u;

		public const uint DeviceMAC_SEG4_SET = 136447u;

		public const uint Config_0x215 = 533u;

		public const uint DeviceMAC_SEG5_SET = 136703u;

		public const uint Config_0x216 = 534u;

		public const uint DeviceSIP_SEG0_SET = 136959u;

		public const uint Config_0x217 = 535u;

		public const uint DeviceSIP_SEG1_SET = 137215u;

		public const uint Config_0x218 = 536u;

		public const uint DeviceSIP_SEG2_SET = 137471u;

		public const uint Config_0x219 = 537u;

		public const uint DeviceSIP_SEG3_SET = 137727u;

		public const uint Config_0x21A = 538u;

		public const uint DeviceNetworkPortLSB = 137983u;

		public const uint Config_0x21B = 539u;

		public const uint DeviceNetworkPortMSB = 138239u;

		public const uint Config_0x21C = 540u;

		public const uint DeviceUdpServerPortLSB = 138495u;

		public const uint Config_0x21D = 541u;

		public const uint DeviceUdpServerPortMSB = 138751u;

		public const uint Config_0x21E = 542u;

		public const uint TcpReconnectServerTime = 139007u;

		public const uint Config_0x21F = 543u;

		public const uint NetWorkDebugMode = 139009u;

		public const uint NetWorkDebugEnable = 1u;

		public const uint NetWorkDebugDisable = 0u;

		public const uint Config_0x220 = 544u;

		public const uint TemplateMuiltCodeMode = 139271u;

		public const uint TemplateMuiltCodeNone = 0u;

		public const uint TemplateMuiltCodeNormal = 1u;

		public const uint TemplateMuiltCodeInteGral = 2u;

		public const uint TemplateMuiltCodeLabel = 3u;

		public const uint TemplateMuiltCodeAttempt = 4u;

		public const uint TemplateMuiltGroupMode = 139272u;

		public const uint TemplateMuiltGroupDisable = 0u;

		public const uint TemplateMuiltGroupEnable = 8u;

		public const uint Config_0x221 = 545u;

		public const uint ReadFailedSendImageMode = 139521u;

		public const uint ReadFailedSendImageDisable = 0u;

		public const uint ReadFailedSendImageEnable = 1u;

		public const uint ReadGoodSendImageMode = 139522u;

		public const uint ReadGoodSendImageDisable = 0u;

		public const uint ReadGoodSendImageEnable = 2u;

		public const uint ReadFailedSaveImageMode = 139524u;

		public const uint ReadFailedSaveImageDisable = 0u;

		public const uint ReadFailedSaveImageEnable = 4u;

		public const uint ReadGoodSaveImageMode = 139528u;

		public const uint ReadGoodSaveImageDisable = 0u;

		public const uint ReadGoodSaveImageEnable = 8u;

		public const uint ReadFailedSaveFtpImageMode = 139536u;

		public const uint ReadFailedSaveFtpImageDisable = 0u;

		public const uint ReadFailedSaveFtpImageEnable = 16u;

		public const uint ReadGoodSaveFtpImageMode = 139552u;

		public const uint ReadGoodSaveFtpImageDisable = 0u;

		public const uint ReadGoodSaveFtpImageEnable = 32u;

		public const uint Config_0x222 = 546u;

		public const uint MuiltCoreDecodeMode = 139783u;

		public const uint MuiltCoreDecodeNone = 0u;

		public const uint MuiltCoreDecodeMuiltImg = 1u;

		public const uint MuiltCoreDecodeSplitImg = 2u;

		public const uint MuiltCoreDecodeSigleType = 3u;

		public const uint MuiltCoreDecodeDiffLib = 4u;

		public const uint Config_0x223 = 547u;

		public const uint MuiltCodeSplitChar = 140287u;

		public const uint Config_0x224 = 548u;

		public const uint TemplateMuiltGroupSet = 140543u;

		public const uint Config_0x230 = 560u;

		public const uint BarcodeExpectSwitch = 143615u;

		public const uint Config_0x231 = 561u;

		public const uint BarcodeHateSwitch = 143871u;

		public const uint Config_0x232 = 562u;

		public const uint BarcodeTestOffsetLenLSB = 144127u;

		public const uint Config_0x233 = 563u;

		public const uint BarcodeTestOffsetLenMSB = 144383u;

		public const uint Config_0x234 = 564u;

		public const uint BarcodeTestTotalLenLSB = 144639u;

		public const uint Config_0x235 = 565u;

		public const uint BarcodeTestTotalLenMSB = 144895u;

		public const uint Config_0x23F = 575u;

		public const uint FTPD_TIMEOUT_SET = 147455u;

		public const uint Config_0x240 = 576u;

		public const uint FTPD_USER_NAME = 147711u;

		public const uint Config_0x250 = 592u;

		public const uint FTPD_USER_PASSWD = 151807u;

		public const uint Config_0x260 = 608u;

		public const uint DevicesNetworkingMode = 155651u;

		public const uint DevicesNetworkingDisable = 0u;

		public const uint DevicesNetworkingClient = 1u;

		public const uint DevicesNetworkingServer = 2u;

		public const uint HostNetworkingUploadMode = 155660u;

		public const uint HostNetworkingUploadNormal = 0u;

		public const uint HostNetworkingUploadSplit = 4u;

		public const uint HostNetworkingUploadOnlyOne = 8u;

		public const uint HostNetworkingTrigMode = 155696u;

		public const uint HostNetworkingTrigSameTime = 0u;

		public const uint HostNetworkingTrigHostFirst = 16u;

		public const uint HostNetworkingTrigClientFirst = 32u;

		public const uint HostNetworkingTrigNotTrig = 48u;

		public const uint Config_0x261 = 609u;

		public const uint DevicesNetworkingServerTimeOutLsb = 156159u;

		public const uint Config_0x262 = 610u;

		public const uint DevicesNetworkingServerTimeOutMsb = 156415u;

		public const uint Config_0x263 = 611u;

		public const uint HostSIP_SEG0_SET = 156671u;

		public const uint Config_0x264 = 612u;

		public const uint HostSIP_SEG1_SET = 156927u;

		public const uint Config_0x265 = 613u;

		public const uint HostSIP_SEG2_SET = 157183u;

		public const uint Config_0x266 = 614u;

		public const uint HostSIP_SEG3_SET = 157439u;

		public const uint Config_0x267 = 615u;

		public const uint DevicesNetworkingServerPortLsb = 157695u;

		public const uint Config_0x268 = 616u;

		public const uint DevicesNetworkingServerPortMsb = 157951u;

		public const uint Config_0x269 = 617u;

		public const uint NetworkingServerSplitCharSet = 158207u;

		public const uint Config_0x300 = 768u;

		public const uint DatamatrixSymbolColsMin = 196863u;

		public const uint Config_0x301 = 769u;

		public const uint DatamatrixSymbolColsMax = 197119u;

		public const uint Config_0x302 = 770u;

		public const uint DatamatrixSymbolRowsMin = 197375u;

		public const uint Config_0x303 = 771u;

		public const uint DatamatrixSymbolRowsMax = 197631u;

		public const uint Config_0x304 = 772u;

		public const uint DatamatrixModuleSizeMin = 197887u;

		public const uint Config_0x305 = 773u;

		public const uint DatamatrixModuleSizeMax = 198143u;

		public const uint Config_0x306 = 774u;

		public const uint DatamatrixSlantMax = 198399u;

		public const uint Config_0x307 = 775u;

		public const uint DatamatrixModuleGapAbout = 198655u;

		public const uint DatamatrixModuleGapMinSel = 198403u;

		public const uint DatamatrixModuleGapMinNone = 0u;

		public const uint DatamatrixModuleGapMinSmall = 1u;

		public const uint DatamatrixModuleGapMinBig = 2u;

		public const uint DatamatrixModuleGapMaxSel = 198412u;

		public const uint DatamatrixModuleGapMaxNone = 0u;

		public const uint DatamatrixModuleGapMaxSmall = 4u;

		public const uint DatamatrixModuleGapMaxBig = 8u;

		public const uint DatamatrixMirrorCodeMode = 198448u;

		public const uint DatamatrixMirrorCodeAllDecode = 0u;

		public const uint DatamatrixMirrorCodeOnlyNormal = 16u;

		public const uint DatamatrixMirrorCodeOnlyMirror = 32u;

		public const uint DatamatrixColorInvertMode = 198592u;

		public const uint DatamatrixColorInvertAllDecode = 0u;

		public const uint DatamatrixColorInvertOnlyNormal = 64u;

		public const uint DatamatrixColorInvertOnlyInvert = 128u;

		public const uint Config_0x308 = 776u;

		public const uint DatamatrixModuleShapeSel = 198659u;

		public const uint DatamatrixModuleShapeAll = 0u;

		public const uint DatamatrixModuleShapeSquare = 1u;

		public const uint DatamatrixModuleShapeRectangle = 2u;

		public const uint DatamatrixModuleGridEnhanceMode = 198660u;

		public const uint DatamatrixModuleGridEnhanceDisable = 0u;

		public const uint DatamatrixModuleGridEnhanceEnable = 4u;

		public const uint DatamatrixModuleFinderPatternEnhanceMode = 198664u;

		public const uint DatamatrixModuleFinderPatternEnhanceDisable = 0u;

		public const uint DatamatrixModuleFinderPatternEnhanceEnable = 8u;

		public const uint DatamatrixSmallerCodeEnchanceMode = 198672u;

		public const uint DatamatrixSmallerCodeEnchanceDisable = 0u;

		public const uint DatamatrixSmallerCodeEnchanceEnable = 16u;

		public const uint DatamatrixNonECC200Mode = 198688u;

		public const uint DatamatrixNonECC200ModeDisable = 0u;

		public const uint DatamatrixNonECC200ModeEnable = 32u;

		public const uint Config_0x309 = 777u;

		public const uint DatamatrixManualThValue = 199167u;

		public const uint Config_0x30F = 783u;

		public const uint DecoderC_UseTrainParaMode = 200449u;

		public const uint DecoderC_UseTrainParaDisable = 0u;

		public const uint DecoderC_UseTrainParaEnable = 1u;

		public const uint Config_0x310 = 784u;

		public const uint DataCodeQualityCalcMode = 200711u;

		public const uint DataCodeQualityCalcNone = 0u;

		public const uint DataCodeQualityCalcISO15415 = 1u;

		public const uint DataCodeQualityCalcISO29158 = 2u;

		public const uint DataCodeQualityCalcSEMI_T10 = 3u;

		public const uint DataCodeQualityCalcSAE_AS9132 = 4u;

		public const uint DataCodeQualityCalcOutputMode = 200728u;

		public const uint DataCodeQualityCalcOutputOnlyAll = 0u;

		public const uint DataCodeQualityCalcOutputOnlyData = 8u;

		public const uint DataCodeQualityCalcOutputDescAndData = 16u;

		public const uint DataCodeQualityCalcOutputJsonType = 24u;

		public const uint Config_0x311 = 785u;

		public const uint BarcodeDataCodeQualitySeparator = 201215u;

		public const uint Config_0x312 = 786u;

		public const uint DataCodeQualityItemSeparator = 201471u;

		public const uint Config_0x313 = 787u;

		public const uint DataCodeQualityTheLowerQualitySet = 201479u;

		public const uint DataCodeQualityTheLowerQualityNone = 0u;

		public const uint DataCodeQualityTheLowerQualityA = 1u;

		public const uint DataCodeQualityTheLowerQualityB = 2u;

		public const uint DataCodeQualityTheLowerQualityC = 3u;

		public const uint DataCodeQualityTheLowerQualityD = 4u;

		public const uint DataCodeQualityCauseNG_Mode = 201496u;

		public const uint DataCodeQualityCauseNG_None = 0u;

		public const uint DataCodeQualityCauseNG_OUT1 = 8u;

		public const uint DataCodeQualityCauseNG_OUT2 = 16u;

		public const uint DataCodeQualityCauseNG_TH_Set = 201696u;

		public const uint DataCodeQualityCauseNG_TH_SetNone = 0u;

		public const uint DataCodeQualityCauseNG_TH_SetA = 32u;

		public const uint DataCodeQualityCauseNG_TH_SetB = 64u;

		public const uint DataCodeQualityCauseNG_TH_SetC = 96u;

		public const uint DataCodeQualityCauseNG_TH_SetD = 128u;

		public const uint Config_0x320 = 800u;

		public const uint CoorOutputMode = 204803u;

		public const uint CoorOutputModeNone = 0u;

		public const uint CoorOutputOnlyFourPoint = 1u;

		public const uint CoorOutputOnlyCenterPoint = 2u;

		public const uint CoorOutputAllPoint = 3u;

		public const uint Config_0x314 = 788u;

		public const uint TemplateCodeIndexOutputMode = 201731u;

		public const uint TemplateCodeIndexOutputNoOut = 0u;

		public const uint TemplateCodeIndexOutputPrefix = 1u;

		public const uint TemplateCodeIndexOutputSuffix = 2u;

		public const uint Config_0x315 = 789u;

		public const uint TemplateCodeIndexOutputSeparator = 202239u;

		public const uint Config_0x316 = 790u;

		public const uint DataCodeQualityItemCloseLSB = 202495u;

		public const uint DataCodeQualityItem_SC_Switch = 202241u;

		public const uint DataCodeQualityItem_SC_OFF = 1u;

		public const uint DataCodeQualityItem_SC_ON = 0u;

		public const uint DataCodeQualityItem_MD_Switch = 202242u;

		public const uint DataCodeQualityItem_MD_OFF = 2u;

		public const uint DataCodeQualityItem_MD_ON = 0u;

		public const uint DataCodeQualityItem_FPD_Switch = 202244u;

		public const uint DataCodeQualityItem_FPD_OFF = 4u;

		public const uint DataCodeQualityItem_FPD_ON = 0u;

		public const uint DataCodeQualityItem_DEC_Switch = 202248u;

		public const uint DataCodeQualityItem_DEC_OFF = 8u;

		public const uint DataCodeQualityItem_DEC_ON = 0u;

		public const uint DataCodeQualityItem_AN_Switch = 202256u;

		public const uint DataCodeQualityItem_AN_OFF = 16u;

		public const uint DataCodeQualityItem_AN_ON = 0u;

		public const uint DataCodeQualityItem_GN_Switch = 202272u;

		public const uint DataCodeQualityItem_GN_OFF = 32u;

		public const uint DataCodeQualityItem_GN_ON = 0u;

		public const uint DataCodeQualityItem_UEC_Switch = 202304u;

		public const uint DataCodeQualityItem_UEC_OFF = 64u;

		public const uint DataCodeQualityItem_UEC_ON = 0u;

		public const uint DataCodeQualityItem_RM_Switch = 202368u;

		public const uint DataCodeQualityItem_RM_OFF = 128u;

		public const uint DataCodeQualityItem_RM_ON = 0u;

		public const uint Config_0x317 = 791u;

		public const uint DataCodeQualityItemCloseMSB = 202751u;

		public const uint DataCodeQualityItem_PG_Switch = 202497u;

		public const uint DataCodeQualityItem_PG_OFF = 1u;

		public const uint DataCodeQualityItem_PG_ON = 0u;

		public const uint DataCodeQualityItem_FID_Switch = 202498u;

		public const uint DataCodeQualityItem_FID_OFF = 2u;

		public const uint DataCodeQualityItem_FID_ON = 0u;

		public const uint DataCodeQualityItem_VID_Switch = 202500u;

		public const uint DataCodeQualityItem_VID_OFF = 4u;

		public const uint DataCodeQualityItem_VID_ON = 0u;

		public byte[] SettingCfgData;

		public ModuleSetting(byte[] para)
		{
			SettingCfgData = para;
		}

		public void ClearCfgData()
		{
			Array.Clear(SettingCfgData, 0, SettingCfgData.Length);
		}

		public bool READ_CFG(uint a, uint b)
		{
			return (SettingCfgData[(a & 0xFFF00) >> 8] & (a & 0xFF)) == (byte)b;
		}

		public void SET_CFG(uint a, uint b)
		{
			SettingCfgData[(a & 0xFFF00) >> 8] &= (byte)(~(a & 0xFF));
			SettingCfgData[(a & 0xFFF00) >> 8] |= (byte)b;
		}

		public byte GET_CFG(uint a)
		{
			return (byte)(SettingCfgData[(a & 0xFFF00) >> 8] & (a & 0xFFu));
		}

		public void FactorySetting()
		{
			SET_CFG(3u, 2u);
			SET_CFG(12u, 4u);
			SET_CFG(48u, 32u);
			SET_CFG(3585u, 1u);
			SET_CFG(3600u, 0u);
			SET_CFG(49663u, 12u);
			SET_CFG(64u, 64u);
			SET_CFG(128u, 128u);
			SET_CFG(640u, 128u);
			SET_CFG(576u, 0u);
			SET_CFG(544u, 32u);
			SET_CFG(543u, 5u);
			SET_CFG(770u, 0u);
			SET_CFG(769u, 0u);
			SET_CFG(772u, 0u);
			SET_CFG(776u, 0u);
			SET_CFG(784u, 0u);
			SET_CFG(864u, 0u);
			SET_CFG(896u, 0u);
			SET_CFG(1279u, 255u);
			SET_CFG(1535u, 0u);
			SET_CFG(1791u, 50u);
			SET_CFG(1920u, 128u);
			SET_CFG(1919u, 35u);
			SET_CFG(2303u, 40u);
			SET_CFG(2308u, 0u);
			SET_CFG(2307u, 0u);
			SET_CFG(2312u, 0u);
			SET_CFG(2336u, 0u);
			SET_CFG(2368u, 0u);
			SET_CFG(2432u, 0u);
			SET_CFG(2496u, 0u);
			SET_CFG(3071u, 100u);
			SET_CFG(2815u, 166u);
			SET_CFG(6911u, 10u);
			SET_CFG(3073u, 1u);
			SET_CFG(3074u, 0u);
			SET_CFG(3136u, 64u);
			SET_CFG(3200u, 128u);
			SET_CFG(3076u, 4u);
			SET_CFG(3080u, 0u);
			SET_CFG(3120u, 0u);
			SET_CFG(3331u, 1u);
			SET_CFG(3340u, 0u);
			SET_CFG(3376u, 0u);
			SET_CFG(3392u, 64u);
			SET_CFG(5633u, 0u);
			SET_CFG(3456u, 0u);
			SET_CFG(3586u, 0u);
			SET_CFG(3680u, 64u);
			SET_CFG(3712u, 0u);
			SET_CFG(3592u, 0u);
			SET_CFG(3588u, 4u);
			SET_CFG(4095u, 50u);
			SET_CFG(4351u, 10u);
			SET_CFG(4607u, 0u);
			SET_CFG(7296u, 128u);
			SET_CFG(7295u, 0u);
			SET_CFG(7167u, 29u);
			SET_CFG(7687u, 0u);
			SET_CFG(7951u, 1u);
			SET_CFG(4863u, 112u);
			SET_CFG(49407u, 128u);
			SET_CFG(4992u, 0u);
			SET_CFG(4991u, 7u);
			SET_CFG(6145u, 0u);
			SET_CFG(6655u, 0u);
			SET_CFG(6146u, 0u);
			SET_CFG(5375u, 10u);
			SET_CFG(5631u, 0u);
			SET_CFG(11263u, 0u);
			SET_CFG(11007u, 96u);
			SET_CFG(11265u, 0u);
			SET_CFG(11270u, 4u);
			SET_CFG(11778u, 0u);
			SET_CFG(11808u, 0u);
			SET_CFG(11780u, 4u);
			SET_CFG(12560u, 0u);
			SET_CFG(11784u, 0u);
			SET_CFG(11792u, 0u);
			SET_CFG(13060u, 4u);
			SET_CFG(13828u, 4u);
			SET_CFG(13826u, 2u);
			SET_CFG(13832u, 0u);
			SET_CFG(12290u, 0u);
			SET_CFG(12546u, 0u);
			SET_CFG(12548u, 0u);
			SET_CFG(19458u, 2u);
			SET_CFG(18694u, 2u);
			SET_CFG(15362u, 2u);
			SET_CFG(21506u, 2u);
			SET_CFG(11272u, 0u);
			SET_CFG(11280u, 0u);
			SET_CFG(47103u, 18u);
			SET_CFG(47359u, 0u);
			SET_CFG(11775u, 100u);
			SET_CFG(24577u, 1u);
			SET_CFG(24578u, 0u);
			SET_CFG(29439u, 0u);
			SET_CFG(24580u, 0u);
			SET_CFG(24584u, 0u);
			SET_CFG(25599u, 0u);
			SET_CFG(24592u, 16u);
			SET_CFG(24672u, 96u);
			SET_CFG(24672u, 0u);
			SET_CFG(24847u, 0u);
			SET_CFG(25056u, 96u);
			SET_CFG(45059u, 0u);
			SET_CFG(59647u, 64u);
			SET_CFG(45060u, 0u);
			SET_CFG(59391u, 96u);
			SET_CFG(45567u, 0u);
			SET_CFG(45823u, 0u);
			SET_CFG(48127u, 0u);
			SET_CFG(48383u, 0u);
			SET_CFG(48639u, 255u);
			SET_CFG(48895u, 255u);
			SET_CFG(49024u, 0u);
			SET_CFG(49008u, 48u);
			SET_CFG(48911u, 1u);
			SET_CFG(52160u, 0u);
			SET_CFG(52031u, 31u);
			SET_CFG(52225u, 0u);
			SET_CFG(52238u, 2u);
			SET_CFG(52272u, 16u);
			SET_CFG(52288u, 64u);
			SET_CFG(51744u, 0u);
			SET_CFG(51776u, 0u);
			SET_CFG(51840u, 128u);
			SET_CFG(51715u, 0u);
			SET_CFG(51455u, 64u);
			SET_CFG(51471u, 0u);
			SET_CFG(51504u, 32u);
			SET_CFG(51584u, 0u);
			SET_CFG(53376u, 128u);
			SET_CFG(53249u, 0u);
			SET_CFG(49919u, 50u);
			SET_CFG(50175u, 32u);
			SET_CFG(50431u, 0u);
			SET_CFG(50687u, 0u);
			SET_CFG(50943u, 80u);
			SET_CFG(51199u, 60u);
			SET_CFG(53250u, 0u);
			SET_CFG(53252u, 0u);
			SET_CFG(56319u, 5u);
			SET_CFG(56575u, 70u);
			SET_CFG(56831u, 40u);
			SET_CFG(57087u, 180u);
			SET_CFG(57343u, 240u);
			SET_CFG(54527u, 0u);
			SET_CFG(54783u, 0u);
			SET_CFG(55039u, 80u);
			SET_CFG(55295u, 60u);
			SET_CFG(55297u, 0u);
			SET_CFG(57599u, 4u);
			SET_CFG(57855u, 3u);
			SET_CFG(58111u, 80u);
			SET_CFG(53759u, 65u);
			SET_CFG(53360u, 0u);
			SET_CFG(58367u, 43u);
			SET_CFG(58623u, 13u);
			SET_CFG(58879u, 10u);
			SET_CFG(59135u, 0u);
			SET_CFG(52735u, 0u);
			SET_CFG(52991u, 0u);
			SET_CFG(53247u, 0u);
			SET_CFG(60159u, 4u);
			SET_CFG(60415u, 131u);
			SET_CFG(60671u, 87u);
			SET_CFG(60927u, 65u);
			SET_CFG(61183u, 5u);
			SET_CFG(61439u, 16u);
			SET_CFG(59649u, 0u);
			SET_CFG(59654u, 0u);
			SET_CFG(59672u, 0u);
			SET_CFG(59744u, 96u);
			SET_CFG(61443u, 0u);
			SET_CFG(61692u, 12u);
			SET_CFG(61951u, 20u);
			SET_CFG(62207u, 13u);
			SET_CFG(62463u, 38u);
			SET_CFG(62719u, 42u);
			SET_CFG(62975u, 13u);
			SET_CFG(63231u, 13u);
			SET_CFG(65008u, 0u);
			SET_CFG(65264u, 0u);
			SET_CFG(65520u, 0u);
			SET_CFG(66559u, 116u);
			SET_CFG(66303u, 38u);
			SET_CFG(67585u, 0u);
			SET_CFG(2320u, 0u);
			SET_CFG(69647u, 0u);
			SET_CFG(69872u, 0u);
			SET_CFG(69903u, 0u);
			SET_CFG(70128u, 0u);
			SET_CFG(70159u, 0u);
			SET_CFG(70384u, 0u);
			SET_CFG(70415u, 0u);
			SET_CFG(70640u, 0u);
			SET_CFG(70671u, 0u);
			SET_CFG(70896u, 0u);
			SET_CFG(70927u, 0u);
			SET_CFG(71152u, 0u);
			SET_CFG(73731u, 0u);
			SET_CFG(131079u, 1u);
			SET_CFG(131080u, 8u);
			SET_CFG(131120u, 0u);
			SET_CFG(131839u, 192u);
			SET_CFG(132095u, 168u);
			SET_CFG(132351u, 0u);
			SET_CFG(132607u, 91u);
			SET_CFG(132863u, 192u);
			SET_CFG(133119u, 168u);
			SET_CFG(133375u, 0u);
			SET_CFG(133631u, 1u);
			SET_CFG(133887u, 255u);
			SET_CFG(134143u, 255u);
			SET_CFG(134399u, 255u);
			SET_CFG(134655u, 0u);
			SET_CFG(135423u, 188u);
			SET_CFG(135679u, 173u);
			SET_CFG(135935u, 189u);
			SET_CFG(136191u, 220u);
			SET_CFG(136447u, 116u);
			SET_CFG(136703u, 38u);
			SET_CFG(138239u, 5u);
			SET_CFG(137983u, 156u);
			SET_CFG(136959u, 192u);
			SET_CFG(137215u, 168u);
			SET_CFG(137471u, 0u);
			SET_CFG(137727u, 135u);
		}
	}
}
