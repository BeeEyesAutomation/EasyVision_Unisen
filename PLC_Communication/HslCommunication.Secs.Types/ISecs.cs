namespace HslCommunication.Secs.Types;

public interface ISecs
{
	OperateResult<uint> SendByCommand(byte stream, byte function, byte[] data, bool back);

	OperateResult<uint> SendByCommand(byte stream, byte function, SecsValue data, bool back);

	OperateResult<SecsMessage> ReadSecsMessage(byte stream, byte function, byte[] data, bool back);

	OperateResult<SecsMessage> ReadSecsMessage(byte stream, byte function, SecsValue data, bool back);
}
