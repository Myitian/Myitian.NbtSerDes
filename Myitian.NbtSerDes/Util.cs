using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Myitian.NbtSerDes
{
    public static class Util
    {
        public static byte[] CopyStream(Stream input)
        {
            List<byte> output_list = new List<byte>();
            byte[] buffer = new byte[65536];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0) break;
                output_list.AddRange(buffer.Take(read));
            }
            return output_list.ToArray();
        }
    }
}
