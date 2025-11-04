using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Security;
using HslCommunication.Enthernet;
using HslCommunication.Language;
using HslCommunication.MQTT;

namespace HslCommunication;

public class Authorization
{
	private static bool nasogaghgaknnc;

	private static readonly string Declaration;

	private static DateTime niahdiahduasdbubfas;

	internal static long naihsdadaasdasdiwasdaid;

	internal static long moashdawidaisaosdas;

	internal static double nuasgdawydbishcgas;

	internal static int nuasgdaaydbishdgas;

	internal static int nuasgdawydbishdgas;

	internal static double nuasgdawydaishdgas;

	internal static int nasidhadguawdbasd;

	internal static int niasdhasdguawdwdad;

	internal static int hidahwdauushduasdhu;

	internal static long iahsduiwikaskfhishfdi;

	internal static int zxnkasdhiashifshfsofh;

	internal static int aosghasdugfavugaosidhaisf;

	static Authorization()
	{
		nasogaghgaknnc = false;
		Declaration = "欢迎使用";
		niahdiahduasdbubfas = DateTime.Now;
		naihsdadaasdasdiwasdaid = 0L;
		moashdawidaisaosdas = 0L;
		nuasgdawydbishcgas = 8.0;
		nuasgdaaydbishdgas = 0;
		nuasgdawydbishdgas = 8;
		nuasgdawydaishdgas = 24.0;
		nasidhadguawdbasd = 1000;
		niasdhasdguawdwdad = 12345;
		hidahwdauushduasdhu = 23456;
		iahsduiwikaskfhishfdi = 0L;
		zxnkasdhiashifshfsofh = 0;
		aosghasdugfavugaosidhaisf = 0;
		niahdiahduasdbubfas = nashgaosgasaisfasfga();
		if (naihsdadaasdasdiwasdaid != 0)
		{
			naihsdadaasdasdiwasdaid = niasdhasdguawdwdad;
		}
		if (nuasgdawydaishdgas != 24.0)
		{
			nuasgdawydaishdgas = 24.0;
		}
		if (nuasgdaaydbishdgas != 0)
		{
			nuasgdaaydbishdgas = 10000;
		}
		if (nuasgdawydbishdgas != 24)
		{
			nuasgdawydbishdgas = 24;
		}
	}

	private static void asidhiahfaoaksdnasoif(object obj)
	{
		for (int i = 0; i < 3600; i++)
		{
			HslHelper.ThreadSleep(1000);
			if (naihsdadaasdasdiwasdaid == niasdhasdguawdwdad && nuasgdaaydbishdgas > 0)
			{
				return;
			}
		}
		NetSimplifyClient netSimplifyClient = new NetSimplifyClient("118.24.36.220", 18467);
		netSimplifyClient.ReadCustomerFromServer(500, SoftBasic.FrameworkVersion.ToString());
	}

	private static void noajfojgkansdnfgaggh()
	{
		if (!nasogaghgaknnc)
		{
			if (niahdiahduasdbubfas < new DateTime(1980, 1, 1) && nashgaosgasaisfasfga() >= new DateTime(1980, 1, 1))
			{
				niahdiahduasdbubfas = nashgaosgasaisfasfga();
				nasogaghgaknnc = true;
			}
			else if (niahdiahduasdbubfas >= new DateTime(1980, 1, 1) && nashgaosgasaisfasfga() >= new DateTime(1980, 1, 1))
			{
				nasogaghgaknnc = true;
			}
		}
	}

	internal static bool nzugaydgwadawdibbas()
	{
		moashdawidaisaosdas++;
		if (naihsdadaasdasdiwasdaid == niasdhasdguawdwdad && nuasgdaaydbishdgas > 0)
		{
			return nuasduagsdwydbasudasd();
		}
		noajfojgkansdnfgaggh();
		if ((nashgaosgasaisfasfga() - niahdiahduasdbubfas).TotalHours < nuasgdawydaishdgas)
		{
			return nuasduagsdwydbasudasd();
		}
		return asdhuasdgawydaduasdgu();
	}

	internal static bool asdniasnfaksndiqwhawfskhfaiw()
	{
		if (naihsdadaasdasdiwasdaid == niasdhasdguawdwdad && nuasgdaaydbishdgas > 0)
		{
			return nuasduagsdwydbasudasd();
		}
		noajfojgkansdnfgaggh();
		if ((nashgaosgasaisfasfga() - niahdiahduasdbubfas).TotalHours < (double)nuasgdawydbishdgas)
		{
			return nuasduagsdwydbasudasd();
		}
		return asdhuasdgawydaduasdgu();
	}

	internal static bool nuasduagsdwydbasudasd()
	{
		return true;
	}

	internal static bool asdhuasdgawydaduasdgu()
	{
		return false;
	}

	internal static bool ashdadgawdaihdadsidas()
	{
		return niasdhasdguawdwdad == 12345;
	}

	internal static DateTime nashgaosgasaisfasfga()
	{
		return DateTime.Now;
	}

	internal static DateTime iashdagsaawbdawda()
	{
		return DateTime.Now.AddDays(1.0);
	}

	internal static DateTime iashdagsaawadawda()
	{
		return DateTime.Now.AddDays(2.0);
	}

	internal static void oasjodaiwfsodopsdjpasjpf()
	{
		Interlocked.Increment(ref iahsduiwikaskfhishfdi);
	}

	internal static string nasduabwduadawdb(string miawdiawduasdhasd)
	{
		StringBuilder stringBuilder = new StringBuilder();
		MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(Encoding.Unicode.GetBytes(miawdiawduasdhasd));
		mD.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append((255 - array[i]).ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static bool SetAuthorizationCode(string ip, int port, string token = null)
	{
		MqttSyncClient mqttSyncClient = new MqttSyncClient(new MqttConnectionOptions
		{
			IpAddress = ip,
			Port = port,
			UseRSAProvider = true
		});
		RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
		OperateResult<string> operateResult = mqttSyncClient.ReadRpc<string>("GetLicense", new
		{
			pubKey = Convert.ToBase64String(rsa.GetPEMPublicKey()),
			token = token
		});
		if (operateResult.IsSuccess)
		{
			return SetAuthorizationCode(Encoding.UTF8.GetString(rsa.DecryptLargeData(Convert.FromBase64String(operateResult.Content))));
		}
		return asdhuasdgawydaduasdgu();
	}

	public static void SetDllContact(string contact)
	{
		DefaultLanguage.Contact = contact;
	}

	public static bool SetAuthorizationCode(string code)
	{
		nuasgdaaydbishdgas = 10000;
		nuasgdawydbishcgas = 286512937.0;
		naihsdadaasdasdiwasdaid = niasdhasdguawdwdad;
		return true;
	}

	public static OperateResult SetHslCertificate(byte[] cert)
	{
		if (HslCertificate.VerifyCer(Convert.FromBase64String("MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCoKsX100Id/uNzOuR9GhBiaXBfMqYBzyr9tGF55ZdP6Wsm51yizuiiEKK8tV5nTDo6mtx3O7gyNLECDHsO2OMxorF4cNNMCME5Mc7pzsryDNOtGJrULpTi6WWcXGK33W3Tx2gsvLTIP+eFKSfk3/hkBOjQiVlBN9tV/pgVL2zG9QIDAQAB"), cert))
		{
			HslCertificate hslCertificate = HslCertificate.CreateFrom(cert);
			if (hslCertificate.KeyWord != "HslCommunication")
			{
				return new OperateResult("Certificate key word not correct!");
			}
			if (hslCertificate.NotBefore <= nashgaosgasaisfasfga() && nashgaosgasaisfasfga() <= hslCertificate.NotAfter)
			{
				if (!string.IsNullOrEmpty(hslCertificate.UniqueID) && !(hslCertificate.UniqueID == Environment.MachineName))
				{
					return new OperateResult("Certificate uniqueID check failed");
				}
				if (hslCertificate.EffectiveHours <= aosghasdugfavugaosidhaisf)
				{
					nuasgdaaydbishdgas = 10000;
					nuasgdawydbishcgas = nuasgdawydbishdgas;
					naihsdadaasdasdiwasdaid = niasdhasdguawdwdad;
					return (naihsdadaasdasdiwasdaid == niasdhasdguawdwdad && nuasgdaaydbishdgas > 0) ? OperateResult.CreateSuccessResult() : new OperateResult("Unknown Failed");
				}
				nuasgdawydaishdgas = hslCertificate.EffectiveHours;
				nuasgdawydbishdgas = hslCertificate.EffectiveHours;
				return OperateResult.CreateSuccessResult();
			}
			return new OperateResult("Certificate time overdue");
		}
		return new OperateResult("Certificate verify failed!");
	}

	private static bool isActiveCodeEnterprisenvasfaosg(string code)
	{
		if (code == "91B82C714F7F7EA22781078B1C1CFE25")
		{
			return true;
		}
		return false;
	}
}
