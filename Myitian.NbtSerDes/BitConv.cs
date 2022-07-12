using System;
namespace Myitian.NbtSerDes
{
    public static class BitConv
    {
        public static byte[] GetBytes(short val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }
        public static byte[] GetBytes(ushort val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }
        public static byte[] GetBytes(int val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }
        public static byte[] GetBytes(uint val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }
        public static byte[] GetBytes(long val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }
        public static byte[] GetBytes(ulong val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }
        public static byte[] GetBytes(float val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }
        public static byte[] GetBytes(double val, bool isLittleEndian = false)
        {
            byte[] b = BitConverter.GetBytes(val);
            if (!IsSameEndian(isLittleEndian)) Array.Reverse(b);
            return b;
        }

        public static short ToInt16(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToInt16(bytes, startIndex)
                :
                BitConverter.ToInt16(new byte[] { bytes[startIndex + 1], bytes[startIndex] }, 0);
        }
        public static ushort ToUInt16(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToUInt16(bytes, startIndex)
                :
                BitConverter.ToUInt16(new byte[] { bytes[startIndex + 1], bytes[startIndex] }, 0);
        }
        public static int ToInt32(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToInt32(bytes, startIndex)
                :
                BitConverter.ToInt32(new byte[] { bytes[startIndex + 3], bytes[startIndex + 2], bytes[startIndex + 1], bytes[startIndex] }, 0);
        }
        public static uint ToUInt32(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToUInt32(bytes, startIndex)
                :
                BitConverter.ToUInt32(new byte[] { bytes[startIndex + 3], bytes[startIndex + 2], bytes[startIndex + 1], bytes[startIndex] }, 0);
        }
        public static long ToInt64(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToInt64(bytes, startIndex)
                :
                BitConverter.ToInt64(new byte[] { bytes[startIndex + 7], bytes[startIndex + 6], bytes[startIndex + 5], bytes[startIndex + 4], bytes[startIndex + 3], bytes[startIndex + 2], bytes[startIndex + 1], bytes[startIndex] }, 0);
        }
        public static ulong ToUInt64(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToUInt64(bytes, startIndex)
                :
                BitConverter.ToUInt64(new byte[] { bytes[startIndex + 7], bytes[startIndex + 6], bytes[startIndex + 5], bytes[startIndex + 4], bytes[startIndex + 3], bytes[startIndex + 2], bytes[startIndex + 1], bytes[startIndex] }, 0);
        }
        public static float ToSingle(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToSingle(bytes, startIndex)
                :
                BitConverter.ToUInt32(new byte[] { bytes[startIndex + 3], bytes[startIndex + 2], bytes[startIndex + 1], bytes[startIndex] }, 0);
        }
        public static double ToDouble(byte[] bytes, int startIndex, bool isLittleEndian = false)
        {
            return IsSameEndian(isLittleEndian) ?

                BitConverter.ToDouble(bytes, startIndex)
                :
                BitConverter.ToDouble(new byte[] { bytes[startIndex + 7], bytes[startIndex + 6], bytes[startIndex + 5], bytes[startIndex + 4], bytes[startIndex + 3], bytes[startIndex + 2], bytes[startIndex + 1], bytes[startIndex] }, 0);
        }

        public static bool IsSameEndian(bool isLittleEndian) => BitConverter.IsLittleEndian == isLittleEndian;
    }
}
