using System.Collections.Generic;

namespace Heroje_Debug_Tool.BaseClass
{
	internal class MultLanguageText
	{
		public enum TextDef
		{
			TextUpperLeft,
			TextLowerLeft,
			TextUpperRight,
			TextLowerRight,
			TextUpperPart,
			TextLowerPart,
			TextLeftSide,
			TextRightSide,
			TextRedFillLight,
			TextWhiteFillLight,
			TextRedFillLight1,
			TextRedFillLight2,
			TextRedFillLight3,
			TextRedFillLight4,
			TextRedshadeLight,
			TextBlueFillLight,
			TextTemplateNameNullTips,
			TextTemplateNameInvalidTips,
			DrawRioTips,
			DrawRoiOkTips,
			AiTrain,
			StopTrain,
			SelectComTips,
			NetRightIpTips,
			NetRightPortTips,
			NetRightRemoteIpTips,
			NetRightRemotePortTips,
			NetDisconnect,
			NetConnect,
			NetOpen,
			UdpCloseTips,
			StateListening,
			StateClientConnected,
			StateClientDisconnect,
			stateDisconnect,
			DisconnectTips,
			NetServerNotOpenTips,
			EnterRightHexTips,
			OpenUdpTips,
			StateDisconnectWithServer,
			EnterTextTips,
			StateConnectToServer,
			ClearReceiveDataTips,
			ClearReceiveCountTips,
			ClearSendDataTips,
			ClearSendCountTips,
			ConnectThenResendTips,
			StateUdpOpened,
			FirmWarePageText,
			SaveTips_1,
			SaveTips_2,
			UpOneLevelText,
			FolderText,
			CharConvertError,
			TempState,
			TempEnableRoi,
			TempEnable,
			TempGroupMode,
			TempEnable_Y,
			TempEnable_N,
			TempRowColErrorTips,
			TempRowColMaxTips,
			TempSaveRoiTips,
			TempRoiStartDraw,
			TempRoiEndDraw,
			TempCaptureTips,
			CmdXmlCfgErrorTips,
			CmdCodeTypeKTips,
			CmdConnectThenSendTips,
			CmdRemove7e00,
			CmdCalcResult,
			CmdSaveCfgTips,
			CmdSaveCfgOkTips,
			CmdSaveErrorTips,
			CmdSaveKeyErrorTips,
			CmdContinueModeTips,
			TypeTrigModeTips,
			DriverInstallExitTips
		}

		public struct StringPairStu
		{
			public string TitleText;

			public string ContentText;

			public StringPairStu(string title, string content)
			{
				TitleText = title;
				ContentText = content;
			}
		}

		private static Dictionary<TextDef, StringPairStu> CurLanguage = new Dictionary<TextDef, StringPairStu>();

		public static Dictionary<TextDef, StringPairStu> CurLanguageCfg
		{
			get
			{
				if (CurLanguage.Count == 0)
				{
					MultLanguageText_Init();
				}
				return CurLanguage;
			}
		}

		private static void MultLanguageText_Init()
		{
			CurLanguage.Add(TextDef.TextUpperLeft, new StringPairStu("", "左上"));
			CurLanguage.Add(TextDef.TextLowerLeft, new StringPairStu("", "左下"));
			CurLanguage.Add(TextDef.TextUpperRight, new StringPairStu("", "右上"));
			CurLanguage.Add(TextDef.TextLowerRight, new StringPairStu("", "右下"));
			CurLanguage.Add(TextDef.TextUpperPart, new StringPairStu("", "上部"));
			CurLanguage.Add(TextDef.TextLowerPart, new StringPairStu("", "下部"));
			CurLanguage.Add(TextDef.TextLeftSide, new StringPairStu("", "左侧"));
			CurLanguage.Add(TextDef.TextRightSide, new StringPairStu("", "右侧"));
			CurLanguage.Add(TextDef.TextRedFillLight, new StringPairStu("", "红色补光灯"));
			CurLanguage.Add(TextDef.TextWhiteFillLight, new StringPairStu("", "白色补光灯"));
			CurLanguage.Add(TextDef.TextRedFillLight1, new StringPairStu("", "红色补光灯1"));
			CurLanguage.Add(TextDef.TextRedFillLight2, new StringPairStu("", "红色补光灯2"));
			CurLanguage.Add(TextDef.TextRedFillLight3, new StringPairStu("", "红色补光灯3"));
			CurLanguage.Add(TextDef.TextRedFillLight4, new StringPairStu("", "红色补光灯4"));
			CurLanguage.Add(TextDef.TextRedshadeLight, new StringPairStu("", "红色灯罩灯"));
			CurLanguage.Add(TextDef.TextBlueFillLight, new StringPairStu("", "蓝色补光灯"));
			CurLanguage.Add(TextDef.TextTemplateNameNullTips, new StringPairStu("提示", "请输入模板名称"));
			CurLanguage.Add(TextDef.TextTemplateNameInvalidTips, new StringPairStu("错误", "模板名称含有非法字符,请重新输入"));
			CurLanguage.Add(TextDef.DrawRioTips, new StringPairStu("", "框选ROI"));
			CurLanguage.Add(TextDef.DrawRoiOkTips, new StringPairStu("", "框选完成"));
			CurLanguage.Add(TextDef.AiTrain, new StringPairStu("", "AI调参"));
			CurLanguage.Add(TextDef.StopTrain, new StringPairStu("", "停止调参"));
			CurLanguage.Add(TextDef.SelectComTips, new StringPairStu("提示", "请先选择串口，再进行其他操作"));
			CurLanguage.Add(TextDef.NetRightIpTips, new StringPairStu("提示", "请先输入正确IP"));
			CurLanguage.Add(TextDef.NetRightPortTips, new StringPairStu("提示", "请先输入正确端口号"));
			CurLanguage.Add(TextDef.NetRightRemoteIpTips, new StringPairStu("提示", "请先输入正确远端IP"));
			CurLanguage.Add(TextDef.NetRightRemotePortTips, new StringPairStu("提示", "请先输入正确远端端口号"));
			CurLanguage.Add(TextDef.NetDisconnect, new StringPairStu("", "断开"));
			CurLanguage.Add(TextDef.NetConnect, new StringPairStu("", "连接"));
			CurLanguage.Add(TextDef.NetOpen, new StringPairStu("", "打开"));
			CurLanguage.Add(TextDef.UdpCloseTips, new StringPairStu("", "UDP通信已关闭"));
			CurLanguage.Add(TextDef.StateListening, new StringPairStu("", "状态：监听中..."));
			CurLanguage.Add(TextDef.StateClientConnected, new StringPairStu("", "状态：客户端已连接."));
			CurLanguage.Add(TextDef.StateClientDisconnect, new StringPairStu("", "状态：客户端连接中断！"));
			CurLanguage.Add(TextDef.stateDisconnect, new StringPairStu("", "状态：连接中断"));
			CurLanguage.Add(TextDef.DisconnectTips, new StringPairStu("提示", "连接已断开."));
			CurLanguage.Add(TextDef.NetServerNotOpenTips, new StringPairStu("提示", "服务端并未启动!"));
			CurLanguage.Add(TextDef.EnterRightHexTips, new StringPairStu("提示", "请输入正确的十六位字符，如“0A”“0x0a”"));
			CurLanguage.Add(TextDef.OpenUdpTips, new StringPairStu("提示", "请先打开UDP通信"));
			CurLanguage.Add(TextDef.StateDisconnectWithServer, new StringPairStu("", "状态：已与服务器断开连接."));
			CurLanguage.Add(TextDef.EnterTextTips, new StringPairStu("提示", "请先输入要发送的数据！"));
			CurLanguage.Add(TextDef.StateConnectToServer, new StringPairStu("", "已连接到服务器."));
			CurLanguage.Add(TextDef.ClearReceiveDataTips, new StringPairStu("提示", "是否清除接收数据？"));
			CurLanguage.Add(TextDef.ClearReceiveCountTips, new StringPairStu("提示", "是否清零接收字节计数？"));
			CurLanguage.Add(TextDef.ClearSendDataTips, new StringPairStu("提示", "是否清除发送数据？"));
			CurLanguage.Add(TextDef.ClearSendCountTips, new StringPairStu("提示", "是否清零发送字节计数？"));
			CurLanguage.Add(TextDef.ConnectThenResendTips, new StringPairStu("提示", "请建立连接后再发送"));
			CurLanguage.Add(TextDef.StateUdpOpened, new StringPairStu("", "状态：UDP通信已打开"));
			CurLanguage.Add(TextDef.FirmWarePageText, new StringPairStu("设备固件信息", "解码设备固件版本："));
			CurLanguage.Add(TextDef.SaveTips_1, new StringPairStu("", "设备配置正在保存中..."));
			CurLanguage.Add(TextDef.SaveTips_2, new StringPairStu("", "请勿断电，以免丢失数据"));
			CurLanguage.Add(TextDef.UpOneLevelText, new StringPairStu("", "上一级"));
			CurLanguage.Add(TextDef.FolderText, new StringPairStu("", "文件夹"));
			CurLanguage.Add(TextDef.CharConvertError, new StringPairStu("提示", "转换成字符串失败"));
			CurLanguage.Add(TextDef.TempState, new StringPairStu("", "状态："));
			CurLanguage.Add(TextDef.TempEnableRoi, new StringPairStu("", "图像ROI是否开启"));
			CurLanguage.Add(TextDef.TempEnable, new StringPairStu("", "是否启用模板"));
			CurLanguage.Add(TextDef.TempGroupMode, new StringPairStu("", "分组模式"));
			CurLanguage.Add(TextDef.TempEnable_Y, new StringPairStu("", "是"));
			CurLanguage.Add(TextDef.TempEnable_N, new StringPairStu("", "否"));
			CurLanguage.Add(TextDef.TempRowColErrorTips, new StringPairStu("提示", "行个数或者列个数设置有误，请检查后重试"));
			CurLanguage.Add(TextDef.TempRowColMaxTips, new StringPairStu("提示", "ROI个数不能超过20"));
			CurLanguage.Add(TextDef.TempSaveRoiTips, new StringPairStu("提示", "当前ROI尚未保存，确定要退出吗？"));
			CurLanguage.Add(TextDef.TempRoiStartDraw, new StringPairStu("", "开始绘制ROI"));
			CurLanguage.Add(TextDef.TempRoiEndDraw, new StringPairStu("", "ROI绘制完成"));
			CurLanguage.Add(TextDef.TempCaptureTips, new StringPairStu("提示", "绘制多ROI需要先触发采图，请检查后重试"));
			CurLanguage.Add(TextDef.CmdXmlCfgErrorTips, new StringPairStu("错误", "程序运行需要的配置文件格式不正确，导致程序不能正常运行。"));
			CurLanguage.Add(TextDef.CmdCodeTypeKTips, new StringPairStu("提示", "此功能无法通过命令设置"));
			CurLanguage.Add(TextDef.CmdConnectThenSendTips, new StringPairStu("提示", "请先连接上设备再进行发送指令操作"));
			CurLanguage.Add(TextDef.CmdRemove7e00, new StringPairStu("", "去掉7e 00"));
			CurLanguage.Add(TextDef.CmdCalcResult, new StringPairStu("", "计算结果："));
			CurLanguage.Add(TextDef.CmdSaveCfgTips, new StringPairStu("警告", "确认要保存当前配置？此操作不可撤销"));
			CurLanguage.Add(TextDef.CmdSaveCfgOkTips, new StringPairStu("提示", "设置保存成功。"));
			CurLanguage.Add(TextDef.CmdSaveErrorTips, new StringPairStu("错误", "格式错误，数据不保存。"));
			CurLanguage.Add(TextDef.CmdSaveKeyErrorTips, new StringPairStu("错误", "指定键值已经存在，填写的数据不保存。"));
			CurLanguage.Add(TextDef.CmdContinueModeTips, new StringPairStu("提示", "当前属于连续触发，配置命令可能会提示失败，但是命令一般都会配置成功。如需确认发送成功提示准确，可以先切回外部触发或者获取原图暂停数据回传，再去配置命令。"));
			CurLanguage.Add(TextDef.TypeTrigModeTips, new StringPairStu("提示", "需要在外部触发模式下进行配置!"));
			CurLanguage.Add(TextDef.DriverInstallExitTips, new StringPairStu("提示", "安装未完成，现在退出则停止安装。点击“OK”则退出安装。"));
		}

		public static string Get_Title(TextDef type)
		{
			if (CurLanguage.Count == 0)
			{
				MultLanguageText_Init();
			}
			return CurLanguage[type].TitleText;
		}

		public static string Get_Content(TextDef type)
		{
			if (CurLanguage.Count == 0)
			{
				MultLanguageText_Init();
			}
			return CurLanguage[type].ContentText;
		}

		public static void SetCurLanguageText(Dictionary<TextDef, StringPairStu> dat)
		{
			if (dat != null)
			{
				if (CurLanguage.Count == 0)
				{
					MultLanguageText_Init();
				}
				if (dat.Count == CurLanguage.Count)
				{
					CurLanguage = dat;
				}
			}
		}
	}
}
