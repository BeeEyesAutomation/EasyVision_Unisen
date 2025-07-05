// VisionAdapter: EtherNet/IP Server (Adapter)
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class EIPAdapter
{
    private TcpListener tcpListener;
    private UdpClient udpClient;
    private Thread tcpThread, udpThread;
    private volatile bool isRunning = false;

    public byte[] InputData = new byte[64];   // Input Assembly (Instance 100)
    public byte[] OutputData = new byte[32];  // Output Assembly (Instance 150)

    public string OCRResult = "ABC123";
    public string QRCodeResult = "QRCODE-XYZ";
    public short PositionX = 123;
    public short PositionY = 456;
    private const int TcpPort = 44818;
    private const int UdpPort = 2222;
    private Dictionary<byte, string> stringParams = new Dictionary<byte, string>();
    private Dictionary<byte, short> intParams = new Dictionary<byte, short>();

    public void Start()
    {
        if (isRunning) return;
        isRunning = true;

        stringParams[4] = OCRResult;
        stringParams[5] = QRCodeResult;
        intParams[6] = PositionX;
        intParams[7] = PositionY;

        tcpThread = new Thread(() =>
        {
            tcpListener = new TcpListener(IPAddress.Any, TcpPort);
            tcpListener.Start();
            Console.WriteLine("EIP TCP Listener started on port 44818");
            while (isRunning)
            {
                try
                {
                    var client = tcpListener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(_ => HandleTcpClient(client));
                }
                catch { }
            }
        });
        tcpThread.IsBackground = true;
        tcpThread.Start();

        udpThread = new Thread(() =>
        {
            udpClient = new UdpClient(UdpPort);
            Console.WriteLine("EIP UDP Listener started on port 2222");
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (isRunning)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEP);
                    byte[] reply = new byte[data.Length];
                    Buffer.BlockCopy(data, 0, reply, 0, data.Length);
                    Buffer.BlockCopy(InputData, 0, reply, 44, InputData.Length);
                    udpClient.Send(reply, reply.Length, remoteEP);
                }
                catch { }
            }
        });
        udpThread.IsBackground = true;
        udpThread.Start();

        Console.WriteLine("EIP Adapter running.");
    }

    private void HandleTcpClient(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        {
            var buffer = new byte[2048];
            while (client.Connected && isRunning)
            {
                try
                {
                    int len = stream.Read(buffer, 0, buffer.Length);
                    if (len == 0) break;

                    byte[] request = new byte[len];
                    Array.Copy(buffer, request, len);
                    byte[] response = HandleCipTcp(request);

                    if (response != null)
                        stream.Write(response, 0, response.Length);
                }
                catch { break; }
            }
        }
    }

    private byte[] HandleCipTcp(byte[] request)
    {
        ushort command = BitConverter.ToUInt16(request, 0);
        switch (command)
        {
            case 0x0065:
                return RegisterSessionResponse(request);
            case 0x006F:
                return HandleRRData(request);
            default:
                return null;
        }
       
    }

    private byte[] RegisterSessionResponse(byte[] request)
    {
        byte[] reply = new byte[28];
        Buffer.BlockCopy(request, 0, reply, 0, 4);
        reply[4] = 0x0C; reply[5] = 0x00;
        reply[24] = 0x01; reply[25] = 0x00;
        reply[26] = 0x01; reply[27] = 0x00;
        return reply;
    }

    private byte[] HandleRRData(byte[] request)
    {
        byte service = request[44];
        byte classId = request[46];
        byte instance = request[48];

        if (classId == 0x04)
        {
            if (service == 0x4C && instance == 100)
            {
                byte[] reply = new byte[44 + InputData.Length + 2];
                Buffer.BlockCopy(request, 0, reply, 0, 44);
                reply[42] = 0xCC;
                reply[44] = (byte)(InputData.Length + 2);
                Buffer.BlockCopy(InputData, 0, reply, 46, InputData.Length);
                return reply;
            }
            else if (service == 0x4D && instance == 150)
            {
                Buffer.BlockCopy(request, 50, OutputData, 0, OutputData.Length);
                return SimpleReply(request);
            }
        }
        else if (classId == 0x0F)
        {
            if (service == 0x0E) return HandleGetAttribute(request, instance);
            if (service == 0x10) return HandleSetAttribute(request, instance);
        }
        return null;
    }

    private byte[] HandleGetAttribute(byte[] request, byte instance)
    {
        byte[] data = null;

        switch (instance)
        {
            case 4:
            case 5:
                data = Encoding.ASCII.GetBytes(stringParams.ContainsKey(instance) ? stringParams[instance] : "");
                break;
            case 6:
            case 7:
                short val = intParams.ContainsKey(instance) ? intParams[instance] : (short)0;
                data = BitConverter.GetBytes(val);
                break;
            default:
                data = null;
                break;
        }


        if (data == null) return null;

        byte[] reply = new byte[44 + 2 + data.Length];
        Buffer.BlockCopy(request, 0, reply, 0, 44);
        reply[42] = 0x8E; // Service code reply
        reply[44] = (byte)(data.Length + 2);
        reply[45] = 0x00;
        Buffer.BlockCopy(data, 0, reply, 46, data.Length);
        return reply;
    }

    private byte[] HandleSetAttribute(byte[] request, byte instance)
    {
        int dataOffset = 50;
        if (instance == 4 || instance == 5)
        {
            string value = Encoding.ASCII.GetString(request, dataOffset, request.Length - dataOffset);
            stringParams[instance] = value.Trim('\0', ' ');
        }
        else if (instance == 6 || instance == 7)
        {
            if (request.Length >= dataOffset + 2)
            {
                short val = BitConverter.ToInt16(request, dataOffset);
                intParams[instance] = val;
            }
        }
        return SimpleReply(request);
    }

    private byte[] SimpleReply(byte[] request)
    {
        byte[] reply = new byte[46];
        Buffer.BlockCopy(request, 0, reply, 0, 44);
        reply[42] = 0xCD;
        return reply;
    }
}

public static class AssemblyHelper
{
    public static ushort GetUInt16(byte[] data, int offset)
    {
        return (ushort)(data[offset] | (data[offset + 1] << 8));
    }

    public static void SetUInt16(byte[] data, int offset, ushort value)
    {
        data[offset] = (byte)(value & 0xFF);
        data[offset + 1] = (byte)((value >> 8) & 0xFF);
    }

    public static byte GetByte(byte[] data, int offset)
    {
        return data[offset];
    }

    public static void SetByte(byte[] data, int offset, byte value)
    {
        data[offset] = value;
    }

    public static void PrintInputData(byte[] input)
    {
        Console.WriteLine("=== Vision Input ===");
        Console.WriteLine($"Result: {input[0]} ({(input[0] == 1 ? "OK" : "NG")})");
        Console.WriteLine($"Code: {input[1]}");
        Console.WriteLine($"Measure1: {GetUInt16(input, 2)}");
        Console.WriteLine($"Measure2: {GetUInt16(input, 4)}");
        Console.WriteLine($"Job: {input[6]}, Light: {input[7]}");
    }
}

public static class VisionJobProcessor
{
    public static void RunJob(byte[] outputData, byte[] inputData)
    {
        byte trigger = AssemblyHelper.GetByte(outputData, 0);
        byte jobId = AssemblyHelper.GetByte(outputData, 1);
        byte light = AssemblyHelper.GetByte(outputData, 2);

        if (trigger == 1)
        {
            // Giả lập xử lý ảnh (bạn có thể gọi hàm xử lý thật ở đây)
            bool resultOK = true;

            AssemblyHelper.SetByte(inputData, 0, (byte)(resultOK ? 1 : 0)); // OK/NG
            AssemblyHelper.SetByte(inputData, 1, (byte)(resultOK ? 0 : 101)); // Mã lỗi
            AssemblyHelper.SetUInt16(inputData, 2, 4660); // Đo 1
            AssemblyHelper.SetUInt16(inputData, 4, 880);  // Đo 2
            AssemblyHelper.SetByte(inputData, 6, jobId);
            AssemblyHelper.SetByte(inputData, 7, light);

            AssemblyHelper.PrintInputData(inputData);
        }
    }
}
