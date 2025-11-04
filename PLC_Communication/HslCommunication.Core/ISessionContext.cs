namespace HslCommunication.Core;

public interface ISessionContext
{
	string UserName { get; set; }

	string ClientId { get; set; }

	object Tag { get; set; }
}
