using BeeUi.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class PLC
    {
        public String StringConnect = "192.168.1:100";
      
        public bool IsBusy = false;
        public String[] nameInput = new String[16];
        public String[] nameOutput = new String[16];
        public int[] valueInput=new int[16];
        public int[] valueOutput= new int[16];
        public int[] AddressStarts;
        public int[] LenReads;
        public String Port = "COM4";
        public int Baurate = 9600;
        public byte SlaveID=2;
        public PLC() { }
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
            IsConnected= Modbus.ConnectPLC(IdPort, Baurate, SlaveID);
          //  Modbus.ReadHolding(0, 10);
            return IsConnected;
        }
        public  bool Read(bool IsReadOut = false)
        {
            if(IsWriting)
            {
               return false;
            }
            valueInput= Modbus.ReadBit(1);
            valueOutput = Modbus.ReadBit(2);
            //  valueInput = new int[dataBytes.Length / 2];

            // valueInput =  Modbus.ReadHolding(AddressStarts[0]);
            //  if(IsReadOut)
            //valueOutput = Modbus.ReadHolding(AddressStarts[1]);
            //if (valueInput.Count()==1||valueOutput.Count()==1) IsConnected = false;
            return IsConnected;
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
