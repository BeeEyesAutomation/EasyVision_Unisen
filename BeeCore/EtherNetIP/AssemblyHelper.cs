using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.EtherNetIP
{
    public static class AssemblyHelper
    {
        // Đọc UINT16 từ buffer (Little Endian)
        public static ushort GetUInt16(byte[] data, int offset)
        {
            return (ushort)(data[offset] | (data[offset + 1] << 8));
        }

        // Ghi UINT16 vào buffer (Little Endian)
        public static void SetUInt16(byte[] data, int offset, ushort value)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);
        }

        // Đọc giá trị kiểu BYTE
        public static byte GetByte(byte[] data, int offset)
        {
            return data[offset];
        }

        // Ghi giá trị kiểu BYTE
        public static void SetByte(byte[] data, int offset, byte value)
        {
            data[offset] = value;
        }
    }

}
