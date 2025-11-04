namespace HslCommunication.Core;

public class SessionContext : ISessionContext
{
	public string UserName { get; set; }

	public string ClientId { get; set; }

	public object Tag { get; set; }
}
