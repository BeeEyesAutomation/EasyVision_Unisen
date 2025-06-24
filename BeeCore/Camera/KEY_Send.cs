using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KEY_Send_n
{
	internal class KEY_Send
	{
		public static int WM_SETTEXT = 12;

		public static int WM_GETTEXT = 13;

		public const byte VK_NUMLOCK = 144;

		public const byte VK_CAPITAL = 20;

		public const byte VK_KANA = 21;

		public const byte VK_SCROLL = 145;

		private const int KEYEVENTF_KEYDOWN = 0;

		private const int KEYEVENTF_EXTENDEDKEY = 1;

		private const int KEYEVENTF_KEYUP = 2;

		private static bool RecordCapsLock;

		private static bool RecordScrollLock;

		private static bool RecordNumLock;

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, string lParam);

		[DllImport("user32.dll ")]
		private static extern IntPtr GetDlgItem(IntPtr hParent, int nIDParentItem);

		[DllImport("user32.dll")]
		private static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		[DllImport("User32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("User32.dll")]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

		[DllImport("User32.dll")]
		private static extern IntPtr FindEx(IntPtr hwnd, IntPtr hwndChild, string lpClassName, string lpWindowName);

		[DllImport("User32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

		[DllImport("User32.dll")]
		public static extern short GetKeyState(int keyCode);

		public static void SendKeyToActiveForm(string str)
		{
			IntPtr activeWindow = GetActiveWindow();
			SendMessage(activeWindow, WM_SETTEXT, 0, str);
		}

		public static void PressDownKey(bool NumLock_en, bool CapsLock_en, bool ScrollLock_en)
		{
			bool flag = ((ushort)GetKeyState(20) & 0xFFFF) != 0;
			bool flag2 = ((ushort)GetKeyState(144) & 0xFFFF) != 0;
			bool flag3 = ((ushort)GetKeyState(145) & 0xFFFF) != 0;
			if (flag3 != ScrollLock_en)
			{
				keybd_event(145, 45, 1, 0);
			}
			if (flag != CapsLock_en)
			{
				keybd_event(20, 45, 1, 0);
			}
			if (flag2 != NumLock_en)
			{
				keybd_event(144, 45, 1, 0);
			}
			keybd_event(144, 45, 3, 0);
			keybd_event(20, 45, 3, 0);
			keybd_event(145, 45, 3, 0);
		}

		private static void Delay(int milliSecond)
		{
			int tickCount = Environment.TickCount;
			while (Math.Abs(Environment.TickCount - tickCount) < milliSecond)
			{
				Application.DoEvents();
			}
		}

		public static void SpecialKey(bool en)
		{
			if (en)
			{
				RecordCapsLock = ((ushort)GetKeyState(20) & 0xFFFF) != 0;
				RecordScrollLock = ((ushort)GetKeyState(145) & 0xFFFF) != 0;
				RecordNumLock = ((ushort)GetKeyState(144) & 0xFFFF) != 0;
				PressDownKey(false, false, false);
				Delay(200);
				PressDownKey(true, true, true);
				Delay(200);
				PressDownKey(true, false, true);
				Delay(200);
				PressDownKey(false, true, false);
				Delay(200);
				PressDownKey(true, false, true);
				Delay(200);
				PressDownKey(true, true, true);
				return;
			}
			bool flag = ((ushort)GetKeyState(20) & 0xFFFF) != 0;
			bool flag2 = ((ushort)GetKeyState(145) & 0xFFFF) != 0;
			bool flag3 = ((ushort)GetKeyState(144) & 0xFFFF) != 0;
			if (flag3 != RecordScrollLock)
			{
				keybd_event(145, 45, 1, 0);
			}
			if (flag != RecordCapsLock)
			{
				keybd_event(20, 45, 1, 0);
			}
			if (flag2 != RecordNumLock)
			{
				keybd_event(144, 45, 1, 0);
			}
			keybd_event(144, 45, 3, 0);
			keybd_event(20, 45, 3, 0);
			keybd_event(145, 45, 3, 0);
		}
	}
}
