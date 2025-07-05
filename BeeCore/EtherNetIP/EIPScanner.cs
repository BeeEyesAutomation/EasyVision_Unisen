// EIPScanner: Đóng vai PLC giả (EtherNet/IP Scanner)
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class EIPScanner
{
    private UdpClient udpClient;
    private IPEndPoint remoteEP;
    private Thread sendThread;

    public byte[] OutputData = new byte[8]; // Instance 150 (Scanner → Adapter)
    public byte[] InputData = new byte[16]; // Instance 100 (Adapter → Scanner)

    public void ConnectTo(string ip,int port= 2222)
    {
        remoteEP = new IPEndPoint(IPAddress.Parse(ip),port);
        udpClient = new UdpClient();

        sendThread = new Thread(CyclicSendReceive);
        sendThread.Start();

        Console.WriteLine($"EIPScanner started to {ip}:{port}");
    }

    private void CyclicSendReceive()
    {
        while (true)
        {
            byte[] packet = BuildOutputPacket();
            udpClient.Send(packet, packet.Length, remoteEP);

            var recvEP = new IPEndPoint(IPAddress.Any, 0);
            if (udpClient.Available > 0)
            {
                byte[] data = udpClient.Receive(ref recvEP);
                Buffer.BlockCopy(data, 44, InputData, 0, Math.Min(InputData.Length, data.Length - 44));
                PrintInput();
            }

            Thread.Sleep(500); // Cyclic 500ms
        }
    }

    private byte[] BuildOutputPacket()
    {
        byte[] packet = new byte[60]; // 44 bytes header giả lập + 8 byte Output + dự phòng
        // Header EIP giả lập
        packet[0] = 0x70; // SendUnitData
        // Các field khác có thể điền nếu cần (encapsulation header)

        // Gắn Output data vào offset 44
        Buffer.BlockCopy(OutputData, 0, packet, 44, OutputData.Length);

        return packet;
    }

    private void PrintInput()
    {
        Console.WriteLine("[Scanner] Nhận Input từ Adapter:");
        Console.WriteLine($"Kết quả: {(InputData[0] == 1 ? "OK" : "NG")}, Mã: {InputData[1]}");
        ushort val1 = (ushort)(InputData[2] | (InputData[3] << 8));
        ushort val2 = (ushort)(InputData[4] | (InputData[5] << 8));
        Console.WriteLine($"Đo 1: {val1}, Đo 2: {val2}");
        Console.WriteLine($"Job ID: {InputData[6]}, Light: {InputData[7]}");
    }

    public void SetTrigger(byte jobId = 1, byte light = 50)
    {
        OutputData[0] = 1; // Trigger
        OutputData[1] = jobId;
        OutputData[2] = light;
    }

    public void ClearTrigger()
    {
        OutputData[0] = 0;
    }
}
