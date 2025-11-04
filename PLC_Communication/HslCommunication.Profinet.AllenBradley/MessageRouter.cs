using System;
using System.Text;
using HslCommunication.BasicFramework;

namespace HslCommunication.Profinet.AllenBradley;

public class MessageRouter
{
	private byte[] _router = new byte[6];

	public byte Backplane
	{
		get
		{
			return _router[0];
		}
		set
		{
			_router[0] = value;
		}
	}

	public byte Slot
	{
		get
		{
			return _router[5];
		}
		set
		{
			_router[5] = value;
		}
	}

	public MessageRouter()
	{
		_router[0] = 1;
		new byte[4] { 15, 2, 18, 1 }.CopyTo(_router, 1);
		_router[5] = 12;
	}

	public MessageRouter(string router)
	{
		string[] array = router.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length <= 6)
		{
			if (array.Length != 0)
			{
				_router[0] = byte.Parse(array[0]);
			}
			if (array.Length > 1)
			{
				_router[1] = byte.Parse(array[1]);
			}
			if (array.Length > 2)
			{
				_router[2] = byte.Parse(array[2]);
			}
			if (array.Length > 3)
			{
				_router[3] = byte.Parse(array[3]);
			}
			if (array.Length > 4)
			{
				_router[4] = byte.Parse(array[4]);
			}
			if (array.Length > 5)
			{
				_router[5] = byte.Parse(array[5]);
			}
		}
		else if (array.Length == 9)
		{
			string text = array[3] + "." + array[4] + "." + array[5] + "." + array[6];
			_router = new byte[6 + text.Length];
			_router[0] = byte.Parse(array[0]);
			_router[1] = byte.Parse(array[1]);
			_router[2] = (byte)(16 + byte.Parse(array[2]));
			_router[3] = (byte)text.Length;
			Encoding.ASCII.GetBytes(text).CopyTo(_router, 4);
			_router[_router.Length - 2] = byte.Parse(array[7]);
			_router[_router.Length - 1] = byte.Parse(array[8]);
		}
	}

	public MessageRouter(byte[] router)
	{
		_router = router;
	}

	public byte[] GetRouter()
	{
		return _router;
	}

	public byte[] GetRouterCIP()
	{
		byte[] array = GetRouter();
		if (array.Length % 2 == 1)
		{
			array = SoftBasic.SpliceArray<byte>(array, new byte[1]);
		}
		byte[] array2 = new byte[46 + array.Length];
		"54022006240105f70200 00800100fe8002001b05 28a7fd03020000008084 1e00f44380841e00f443 a305".ToHexBytes().CopyTo(array2, 0);
		array.CopyTo(array2, 42);
		"20022401".ToHexBytes().CopyTo(array2, 42 + array.Length);
		array2[41] = (byte)(array.Length / 2);
		return array2;
	}
}
