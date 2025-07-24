
using EasyModbus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeeGlobal
{


    public class Modbus
    {
        private static bool IsConnectPLC;

        static public bool IsConnectPLC1 { get => IsConnectPLC; set => IsConnectPLC = value; }
        static string modbusIpAddress = "192.168.37.224"; // Replace with your Modbus device IP
        static int modbusPort = 502; // Default Modbus TCP port
        static Thread listenerThread;
        static ModbusClient modbusClient;

        bool IsServerStarted = false;

        public static bool ConnectPLC(string Port, int Baurate = 9600, byte SlaveId = 1)
        {
            string PortConnect = Port.ToString().Trim();
            try
            {
                try
                {
                    if (modbusClient != null)
                        if (modbusClient.Connected)
                            modbusClient?.Disconnect();
                  
                }
                catch (Exception ex)
                {
                    ///  MessageBox.Show($"Lỗi khi ngắt kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              //  modbusClient = new ModbusClient(modbusIpAddress, modbusPort);
                modbusClient = new ModbusClient(PortConnect);//modbusIpAddress, modbusPort
                modbusClient.Baudrate = Baurate;
                modbusClient.UnitIdentifier = SlaveId;
                modbusClient.StopBits = StopBits.One;
                modbusClient.Parity = Parity.None;

                modbusClient.Connect();
                if (modbusClient.Connected)
                {

                    //      modbusClient.WriteSingleRegister(40001, 10);
                    //modbusClient.WriteSingleCoil(12, true);


                    IsConnectPLC1 = true;
                    return true;
                }
                else
                {
                    IsConnectPLC1 = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                //     MessageBox.Show(ex.Message);
            }
            return false;
        }
        //public int ConnectPLC()
        //{
        //    try
        //    {
        //        string[] comPorts = SerialPort.GetPortNames();

        //        if (comPorts.Length == 0)
        //        {
        //            IsConnectPLC1 = false;
        //            return -1;
        //        }

        //        foreach (string portName in comPorts)
        //        {
        //            string description = GetPortDescription(portName);

        //            if (description.Contains("USB-SERIAL CH340"))
        //            {
        //                // Kết nối với cổng COM này
        //                modbusClient = new ModbusClient(portName);
        //                modbusClient.Baudrate = 9600;
        //                modbusClient.UnitIdentifier = 2;
        //                modbusClient.StopBits = StopBits.One;
        //                modbusClient.Parity = Parity.None;

        //                modbusClient.Connect();

        //                if (modbusClient.Connected)
        //                {
        //                    MessageBox.Show(portName);
        //                    IsConnectPLC1 = true;
        //                    return 0;  // Thành công
        //                }
        //                else
        //                {
        //                    IsConnectPLC1 = false;
        //                    return -1;  // Kết nối thất bại
        //                }
        //            }
        //        }

        //        // Nếu không tìm thấy cổng COM với mô tả "USB-SERIAL CH340"
        //        IsConnectPLC1 = false;
        //        return -1;  // Không tìm thấy cổng phù hợp
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Lỗi: {ex.Message}");
        //        IsConnectPLC1 = false;
        //        return -1;  // Lỗi trong quá trình kết nối
        //    }
        //}

        // Hàm để lấy mô tả của cổng COM từ Win32_PnPEntity
        private string GetPortDescription(string portName)
        {
            string description = "Description not found";

            // Query WMI để lấy thông tin mô tả của cổng COM qua Win32_PnPEntity
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                string deviceID = queryObj["DeviceID"]?.ToString();
                string devDescription = queryObj["Description"]?.ToString();
                string pnpDeviceID = queryObj["PNPDeviceID"]?.ToString();

                // Kiểm tra nếu cổng COM chứa trong DeviceID và mô tả chứa "USB-SERIAL CH340"
                if (deviceID != null && deviceID.Contains("COM") && devDescription != null && devDescription.Contains("USB-SERIAL CH340"))
                {
                    description = deviceID;
                    break;
                }

                // Kiểm tra thông qua PNPDeviceID để đảm bảo cổng COM là chính xác
                if (pnpDeviceID != null && pnpDeviceID.Contains("USB\\VID_1A86&PID_7523") && devDescription != null)
                {
                    description = deviceID;
                    break;
                }
            }

            return description;
        }
        public static void DisconnectPLC()
        {
            try
            {
                if (modbusClient != null)
                    if (modbusClient.Connected)
                        modbusClient?.Disconnect();

            }
            catch (Exception ex)
            {
                ///  MessageBox.Show($"Lỗi khi ngắt kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static bool ReadCoil(int startAddress)
        {
            try
            {
                bool[] value = modbusClient.ReadCoils(startAddress, 1);
                if (value.Count() > 0)
                {
                    return value[0];
                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static int ReadInput(int startAddress)
        {
            try
            {
                bool[] value = modbusClient.ReadDiscreteInputs(startAddress, 1);
                if (value.Count() > 0)
                {
                    return Convert.ToInt32(value[0]);
                }
                else
                {

                    return -1;
                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                return -1;
            }
        }
        public static  int[] ReadBit(int startAddress)
        {
            int[] values =new  int[16];

            try
            {
                int[] val = modbusClient.ReadHoldingRegisters(startAddress, 1);
                ushort registerValue = (ushort)val[0];
                // Lấy từng bit
                for (int i = 15; i >= 0; i--)  // bit 15 là MSB, bit 0 là LSB
                {
                    int val2 = (registerValue >> i) & 1; ;
                    values[i] = val2;

                }

            }
            catch (Exception ex)
            {

                // return i;
            }
            return values;
        }
        public static bool WriteBit(int value)
        {
            // int[] values = new int[16];
            try
            {
                modbusClient.WriteSingleRegister(2, value);


            }
            catch (Exception ex)
            {

                // return i;
            }
            return true;
        }
        public static int[] ReadHolding(int startAddress, int lennght = 16)
        {
            int[] values = new int[1];
            try
            {
                values = modbusClient.ReadHoldingRegisters(startAddress, lennght);
                //if (value.Count() > 0)
                //{
                //    return Convert.ToInt32(value[0]);
                //}
                //else
                //{

                ////G.IsPLC = false;
                //    return -1;
                //}

            }
            catch (Exception ex)
            {

                // return i;
            }
            return values;
        }
        public static int WriteSingleCoil(int startAddress, bool Value)
        {




            try
            {
                modbusClient.WriteSingleCoil(startAddress, Value);


                return 0;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static bool WritePLC(int startAddress, int Value)
        {
            try
            {

                modbusClient.WriteSingleRegister(startAddress, Value);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
