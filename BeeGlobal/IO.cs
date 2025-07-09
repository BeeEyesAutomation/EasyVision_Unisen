
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class IO
    {
        public String StringConnect = "192.168.1:100";
      
        public bool IsBusy = false;
        public String[] nameInput = new String[16];
        public String[] nameOutput = new String[16];
        public int[] valueInput = new int[16];
        public int[] valueOutput = new int[16];
        public int[] AddressStarts;
        public int[] LenReads;
        public String Port = "COM8";
        public int Baurate = 115200;
        public byte SlaveID=1;
        public bool IsBypass=false;
        public IO() { }
        public bool IsConnected = false,IsWriting=false;
        public bool Connect( String IdPort )
        {
            if (!File.Exists("PLC.ini"))
                return false;
            String[] stringLine = File.ReadAllLines("PLC.ini");
            AddressStarts = Array.ConvertAll(stringLine[0].Split(','), int.Parse);
            LenReads = Array.ConvertAll(stringLine[1].Split(','), int.Parse);
            nameInput = stringLine[2].Split(',');
            nameOutput = stringLine[3].Split(',');
            if (IdPort.Trim() == "") return false;
            IsConnected= Modbus.ConnectPLC(Port, Baurate, SlaveID);
            if(IsConnected) valueOutput = Modbus.ReadBit(2);
            //  Modbus.ReadHolding(0, 10);
            return IsConnected;
        }
        public void Disconnect()
        {
             Modbus.DisconnectPLC();
            IsConnected = false;
        }
        public  bool Read()
        {
            if (!IsConnected) return false;
            if (IsWriting)
            {
               return false;
            }
             valueInput= Modbus.ReadBit(1);
         //   valueOutput = Modbus.ReadBit(2);
            //  valueInput = new int[dataBytes.Length / 2];

            // valueInput =  Modbus.ReadHolding(AddressStarts[0]);
            //  if(IsReadOut)
            //valueOutput = Modbus.ReadHolding(AddressStarts[1]);
            //if (valueInput.Count()==1||valueOutput.Count()==1) IsConnected = false;
            return IsConnected;
        }
        public IO_Processing IO_Processing = IO_Processing.None;
        public async void WriteIO(IO_Processing Processing,bool Is=false,int Delay=1)
        {   if (!IsConnected) return;
            IsWriting = true;
            switch (Processing )
            {
                case IO_Processing.Trigger:
                    SetOutPut(1, false);//Ready false
                    SetOutPut(2, true);//Busy
                    SetOutPut(7, true);//LIGHT
                    WriteOutPut();
                    break;
                case IO_Processing.Close:
                    SetOutPut(0, false); //T.Result
                    SetOutPut(1, false); //Ready
                    SetOutPut(3, false); //Busy
                    SetOutPut(4, false); //Busy
                    SetOutPut(5, false); //Busy
                    SetOutPut(6, false); //Busy
                    SetOutPut(7, false); //Busy
                    SetOutPut(2, true); //Busy
                  
                    WriteOutPut();
                    Disconnect();
                    break;
                case IO_Processing.Error:
                    Global.Comunication.IO.SetOutPut(3, true);//CCD Err
                    Global.Comunication.IO.WriteOutPut();
                    break;
                case IO_Processing.NoneErr:
                    Global.Comunication.IO.SetOutPut(3, false);//CCD Err
                    Global.Comunication.IO.WriteOutPut();
                    break;
                case IO_Processing.Result:
                    if (Is)
                    {
                        Global.Comunication.IO.SetOutPut(0, false); //OK
                        Global.Comunication.IO.SetOutPut(7, false); //Light
                        Global.Comunication.IO.SetOutPut(2, false); //Busy
                        Global.Comunication.IO.WriteOutPut();
                        await Task.Delay(Delay);
                        Global.Comunication.IO.SetOutPut(1, true);//Ready false
                        Global.Comunication.IO.SetOutPut(0, false); //OK
                        Global.Comunication.IO.WriteOutPut();
                    }
                    else
                    {

                        if (valueInput[3] == 1) //Bypass
                        {
                           SetOutPut(0, false); //OK
                           SetOutPut(7, false); //Light
                           SetOutPut(2, false); //Busy
                           WriteOutPut();
                           await Task.Delay(Delay);
                           Global.Comunication.IO.SetOutPut(1, true);//Ready false
                           Global.Comunication.IO.SetOutPut(0, false); //OK
                           Global.Comunication.IO.WriteOutPut();
                        }
                        else
                        {
                            Global.Comunication.IO.SetOutPut(0, true); //NG
                            Global.Comunication.IO.SetOutPut(7, false); //Light
                            Global.Comunication.IO.SetOutPut(2, false); //Busy
                            Global.Comunication.IO.WriteOutPut();
                            await Task.Delay(Delay);
                            Global.Comunication.IO.SetOutPut(1, true);//Ready false
                            Global.Comunication.IO.SetOutPut(0, false); //False
                            Global.Comunication.IO.WriteOutPut();
                        }
                    }
                    break;
                case IO_Processing.ChangeMode:
                    if(Is)
                   {
                        Global.Comunication.IO.SetOutPut(2, false); //Busy
                        Global.Comunication.IO.WriteOutPut();
                    }
                    else
                    {
                        Global.Comunication.IO.SetOutPut(2, true); //Not Busy
                        Global.Comunication.IO.WriteOutPut();
                    }
                        break;
                case IO_Processing.Light:
                        Global.Comunication.IO.SetOutPut(7, Is); //Busy
                        Global.Comunication.IO.WriteOutPut();
                    break;
                case IO_Processing.ChangeProg:
                    Global.Comunication.IO.SetOutPut(2, true); //Busy
                    Global.Comunication.IO.WriteOutPut();
                    break;
                case IO_Processing.Reset:
                    Global.Comunication.IO.SetOutPut(1, true); //Ready
                    Global.Comunication.IO.SetOutPut(2, false); //Busy
                    Global.Comunication.IO.SetOutPut(3, false); //Err
                    Global.Comunication.IO.WriteOutPut();
                    break;

            }
            IsWriting = false;
        }
        public bool CheckReady()
        {
        if (valueInput[0] == 1&& valueOutput[2] == 0)
            {
                return true;
            }
            else
            {
              
                return false;
            }
            return false;
        }
        public bool CheckErr(bool IsCameraConnected)
        {
            if (!IsCameraConnected)
            {
                if (Global.Comunication.IO.valueOutput[3] == 0)
                {
                    Global.Comunication.IO.SetOutPut(3, true);//CCD Err
                    Global.Comunication.IO.WriteOutPut();
                    return false;
                }
                return true;
            }
            else
            {
                if (Global.Comunication.IO.valueOutput[3] == 1)
                {
                    Global.Comunication.IO.IO_Processing = IO_Processing.Error;
                    Global.Comunication.IO.SetOutPut(3, false);//CCD Err
                    Global.Comunication.IO.WriteOutPut();
                    return true;
                }
                return true;
            }
            return false;
        }
        public int ReadPara(int Add )
        {
      
           return Modbus.ReadHolding(Add,1)[0];
        
        }
        public bool WritePara(int Add, int Value)
        {
            IsWriting = true;
            IsConnected = Modbus.WritePLC(Add, Value);

            IsWriting = false;
            return IsConnected;
        }
        public   bool WriteInPut(int Add,bool Value)
        {
            IsWriting = true;
            IsConnected = Modbus.WritePLC(AddressStarts[0]+Add,Convert.ToInt16(Value));

            IsWriting = false;
            return IsConnected;
        }
        public void SetOutPut(int Add, bool Value)
        {
            valueOutput[Add] = Convert.ToInt32(Value);
           
            // Mảng bit (16 bit: 0 hoặc 1), bit 15 là MSB, bit 0 là LSB
           
        }
        public bool WriteOutPut()
        {
            int[] bitArray = new int[16] {
                0, 0, 0, 0, 0, 0, 0, 0,   // Bit 15 đến 8
                0, 0, 0, 0, 0, 0, 0, 0   // Bit 7 đến 0
            };
            for (int i = 0; i < 16; i++)
            {
                bitArray[15 - i] = valueOutput[i]; // bit 15 là MSB

            }
            int Val = 0;

            for (int i = 0; i < 16; i++)
            {
                Val |= (bitArray[i] & 1) << (15 - i); // bit 15 là MSB

            }
            IsWriting = true;
            IsConnected = Modbus.WriteBit(Val);

            IsWriting = false;
            return IsConnected;
        }
    }
}
