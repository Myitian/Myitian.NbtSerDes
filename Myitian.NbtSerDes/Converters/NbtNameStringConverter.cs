using System;
using System.IO;
using System.Text;

namespace Myitian.NbtSerDes
{
    public class NbtNameStringConverter
    {
        public static void Serialize(ref Stream stream, string val)
        {
            byte[] b = Encoding.UTF8.GetBytes(val);
            if (b.Length > short.MaxValue)
            {
                throw new ArgumentException("The string is too long. Max length is 65535 bytes.");
            }
            stream.Write(BitConv.GetBytes((ushort)b.Length), 0, 2);
            stream.Write(b, 0, b.Length);
        }
        public static string Deserialize(ref Stream stream)
        {
            byte[] buffer = new byte[2];
            int read = stream.Read(buffer, 0, 2);
            if (read > 0)
            {
                buffer = new byte[BitConv.ToUInt16(buffer, 0)];
                read = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
            throw new EndOfStreamException();
        }
    }
}
