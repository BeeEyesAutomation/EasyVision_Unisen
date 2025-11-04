using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HslCommunication.BasicFramework;

public class SoftMail
{
	public static SoftMail MailSystem163 = new SoftMail(delegate(SmtpClient mail)
	{
		mail.Host = "smtp.163.com";
		mail.UseDefaultCredentials = true;
		mail.EnableSsl = true;
		mail.Port = 25;
		mail.DeliveryMethod = SmtpDeliveryMethod.Network;
		mail.Credentials = new NetworkCredential("softmailsendcenter", "zxcvbnm6789");
	}, "softmailsendcenter@163.com", "hsl200909@163.com");

	public static SoftMail MailSystemQQ = new SoftMail(delegate(SmtpClient mail)
	{
		mail.Host = "smtp.qq.com";
		mail.UseDefaultCredentials = true;
		mail.Port = 587;
		mail.EnableSsl = true;
		mail.DeliveryMethod = SmtpDeliveryMethod.Network;
		mail.Credentials = new NetworkCredential("974856779", "tvnlczxdumutbbic");
	}, "974856779@qq.com", "hsl200909@163.com");

	private static long SoftMailSendFailedCount { get; set; } = 0L;

	private SmtpClient smtpClient { get; set; }

	private string MailFromAddress { get; set; } = "";

	public string MailSendAddress { get; set; } = "";

	public SoftMail(Action<SmtpClient> mailIni, string addr_From = "", string addr_to = "")
	{
		smtpClient = new SmtpClient();
		mailIni(smtpClient);
		MailFromAddress = addr_From;
		MailSendAddress = addr_to;
	}

	private string GetExceptionMail(Exception ex)
	{
		return StringResources.Language.Time + DateTime.Now.ToString() + Environment.NewLine + StringResources.Language.SoftWare + ex.Source + Environment.NewLine + StringResources.Language.ExceptionMessage + ex.Message + Environment.NewLine + StringResources.Language.ExceptionType + ex.GetType().ToString() + Environment.NewLine + StringResources.Language.ExceptionStackTrace + ex.StackTrace + Environment.NewLine + StringResources.Language.ExceptionTargetSite + ex.TargetSite.Name;
	}

	public bool SendMail(Exception ex)
	{
		return SendMail(ex, "");
	}

	public bool SendMail(string subject, string body)
	{
		return SendMail(MailSendAddress, subject, body);
	}

	public bool SendMail(string subject, string body, bool isHtml)
	{
		return SendMail(MailSendAddress, subject, body, isHtml);
	}

	public bool SendMail(Exception ex, string addtion)
	{
		if (string.IsNullOrEmpty(MailSendAddress))
		{
			return false;
		}
		return SendMail(MailSendAddress, StringResources.Language.BugSubmit, string.IsNullOrEmpty(addtion) ? GetExceptionMail(ex) : ("User：" + addtion + Environment.NewLine + GetExceptionMail(ex)));
	}

	public bool SendMail(string addr_to, string subject, string body)
	{
		return SendMail(addr_to, subject, body, isHtml: false);
	}

	public bool SendMail(string addr_to, string subject, string body, bool isHtml)
	{
		return SendMail(MailFromAddress, StringResources.Language.MailServerCenter, new string[1] { addr_to }, subject, body, MailPriority.Normal, isHtml);
	}

	public bool SendMail(string addr_from, string name, string[] addr_to, string subject, string body, MailPriority priority, bool isHtml)
	{
		if (SoftMailSendFailedCount > 10)
		{
			SoftMailSendFailedCount++;
			return true;
		}
		using MailMessage mailMessage = new MailMessage();
		try
		{
			mailMessage.From = new MailAddress(addr_from, name, Encoding.UTF8);
			foreach (string addresses in addr_to)
			{
				mailMessage.To.Add(addresses);
			}
			mailMessage.Subject = subject;
			mailMessage.Body = body;
			MailMessage mailMessage2 = mailMessage;
			mailMessage2.Body = mailMessage2.Body + Environment.NewLine + Environment.NewLine + Environment.NewLine + StringResources.Language.MailSendTail;
			mailMessage.SubjectEncoding = Encoding.UTF8;
			mailMessage.BodyEncoding = Encoding.UTF8;
			mailMessage.Priority = priority;
			mailMessage.IsBodyHtml = isHtml;
			smtpClient.Send(mailMessage);
			SoftMailSendFailedCount = 0L;
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(SoftBasic.GetExceptionMessage(ex));
			SoftMailSendFailedCount++;
			return false;
		}
	}
}
