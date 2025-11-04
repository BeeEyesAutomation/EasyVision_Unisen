using System;

namespace HslCommunication.Reflection;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class HslMqttPermissionAttribute : Attribute
{
	public string ClientID { get; set; }

	public string UserName { get; set; }

	public virtual bool CheckClientID(string clientID)
	{
		if (string.IsNullOrEmpty(ClientID))
		{
			return true;
		}
		return ClientID == clientID;
	}

	public virtual bool CheckUserName(string name)
	{
		if (string.IsNullOrEmpty(UserName))
		{
			return true;
		}
		return UserName == name;
	}
}
