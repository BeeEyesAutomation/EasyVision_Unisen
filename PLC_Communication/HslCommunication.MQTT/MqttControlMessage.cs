namespace HslCommunication.MQTT;

public class MqttControlMessage
{
	public const byte FAILED = 0;

	public const byte CONNECT = 1;

	public const byte CONNACK = 2;

	public const byte PUBLISH = 3;

	public const byte PUBACK = 4;

	public const byte PUBREC = 5;

	public const byte PUBREL = 6;

	public const byte PUBCOMP = 7;

	public const byte SUBSCRIBE = 8;

	public const byte SUBACK = 9;

	public const byte UNSUBSCRIBE = 10;

	public const byte UNSUBACK = 11;

	public const byte PINGREQ = 12;

	public const byte PINGRESP = 13;

	public const byte DISCONNECT = 14;

	public const byte REPORTPROGRESS = 15;

	public const byte FileNoSense = 100;

	public const byte FileDownload = 101;

	public const byte FileUpload = 102;

	public const byte FileDelete = 103;

	public const byte FileFolderDelete = 104;

	public const byte FileFolderFiles = 105;

	public const byte FileFolderPaths = 106;

	public const byte FileExists = 107;

	public const byte FileFolderInfo = 108;

	public const byte FileFolderInfos = 109;

	public const byte FileFolderClear = 110;

	public const byte FileFolderRename = 111;
}
