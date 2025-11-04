using System.Runtime.InteropServices;

namespace HslCommunication.Core.Pipe;

internal class PCommHelper
{
	public const int B50 = 0;

	public const int B75 = 1;

	public const int B110 = 2;

	public const int B134 = 3;

	public const int B150 = 4;

	public const int B300 = 5;

	public const int B600 = 6;

	public const int B1200 = 7;

	public const int B1800 = 8;

	public const int B2400 = 9;

	public const int B4800 = 10;

	public const int B7200 = 11;

	public const int B9600 = 12;

	public const int B19200 = 13;

	public const int B38400 = 14;

	public const int B57600 = 15;

	public const int B115200 = 16;

	public const int B230400 = 17;

	public const int B460800 = 18;

	public const int B921600 = 19;

	public const int BIT_5 = 0;

	public const int BIT_6 = 1;

	public const int BIT_7 = 2;

	public const int BIT_8 = 3;

	public const int STOP_1 = 0;

	public const int STOP_2 = 4;

	public const int P_NONE = 0;

	public const int P_EVEN = 24;

	public const int P_ODD = 8;

	public const int P_MRK = 40;

	public const int P_SPC = 56;

	public const int SIO_OK = 0;

	public const int SIO_BADPORT = -1;

	public const int SIO_OUTCONTROL = -2;

	public const int SIO_NODATA = -4;

	public const int SIO_OPENFAIL = -5;

	public const int SIO_RTS_BY_HW = -6;

	public const int SIO_BADPARM = -7;

	public const int SIO_WIN32FAIL = -8;

	public const int SIO_BOARDNOTSUPPORT = -9;

	public const int SIO_ABORT_WRITE = -11;

	public const int SIO_WRITETIMEOUT = -12;

	[DllImport("PComm.dll")]
	public static extern int sio_open(int port);

	[DllImport("PComm.dll")]
	public static extern int sio_ioctl(int port, int baud, int mode);

	[DllImport("PComm.dll")]
	public static extern int sio_flowctrl(int port, int mode);

	[DllImport("PComm.dll")]
	public static extern int sio_lctrl(int port, int mode);

	[DllImport("PComm.dll")]
	public static extern int sio_iqueue(int port);

	[DllImport("PComm.dll")]
	public static extern int sio_DTR(int port, int mode);

	[DllImport("PComm.dll")]
	public static extern int sio_RTS(int port, int mode);

	[DllImport("PComm.dll")]
	public static extern int sio_flush(int port, int func);

	[DllImport("PComm.dll")]
	public static extern int sio_close(int port);

	[DllImport("PComm.dll")]
	public static extern int sio_read(int port, ref byte buf, int length);

	[DllImport("PComm.dll")]
	public static extern int sio_SetReadTimeouts(int port, int TotalTimeouts, int IntervalTimeouts);

	[DllImport("PComm.dll")]
	public static extern int sio_GetReadTimeouts(int port, out int TotalTimeouts, out int IntervalTimeouts);

	[DllImport("PComm.dll")]
	public static extern int sio_write(int port, byte[] buf, int length);

	[DllImport("PComm.dll")]
	public static extern int sio_putb_x_ex(int port, byte[] buf, int length, int timer);

	[DllImport("PComm.dll")]
	public static extern int sio_putch(int port, int term);

	[DllImport("PComm.dll")]
	public static extern int sio_getch(int port);
}
