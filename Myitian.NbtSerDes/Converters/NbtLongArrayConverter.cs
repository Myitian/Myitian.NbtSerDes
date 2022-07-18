using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Myitian.NbtSerDes
{
    public class NbtLongArrayConverter : NbtConverterBase
    {
        public override byte ID => 12;

        public NbtLongArrayConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            if (value != null)
            {
                byte[] buffer;
                Type type = value.GetType();
                if (value is long[] longs)
                {
                    stream.Write(BitConv.GetBytes(longs.Length), 0, 4);
                    buffer = new byte[longs.Length << 3];
                    if (BitConv.IsSameEndian(false))
                    {
                        Buffer.BlockCopy(longs, 0, buffer, 0, buffer.Length);
                    }
                    else
                    {
                        for (int i = 0; i < longs.Length; i++)
                        {
                            BitConv.GetBytes(longs[i]).CopyTo(buffer, i << 3);
                        }
                    }
                }
                else if (type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IEnumerable<long>)).Length > 0)
                {
                    longs = (value as IEnumerable<long>).ToArray();
                    stream.Write(BitConv.GetBytes(longs.Length), 0, 4);
                    buffer = new byte[longs.Length << 3];
                    if (BitConv.IsSameEndian(false))
                    {
                        Buffer.BlockCopy(longs, 0, buffer, 0, buffer.Length);
                    }
                    else
                    {
                        for (int i = 0; i < longs.Length; i++)
                        {
                            BitConv.GetBytes(longs[i]).CopyTo(buffer, i << 3);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException($"Unsupported Type: {type}");
                }
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            int read, len;
            byte[] buffer = new byte[4];
            if (type == typeof(long[]) || type == typeof(Array) || type == typeof(object))
            {
                long[] longs;
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    len = BitConv.ToInt32(buffer, 0);
                    buffer = new byte[len << 3];
                    if (buffer.Length == 0)
                    {
                        return new long[0];
                    }
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        longs = new long[len];
                        if (BitConv.IsSameEndian(false))
                        {
                            Buffer.BlockCopy(buffer, 0, longs, 0, buffer.Length);
                        }
                        else
                        {
                            for (int i = 0; i < len; i++)
                            {
                                longs[i] = BitConv.ToInt64(buffer, i << 3);
                            }
                        }
                        return longs;
                    }
                }
            }
            if (type == typeof(ulong[]))
            {
                ulong[] longs;
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    len = BitConv.ToInt32(buffer, 0);
                    buffer = new byte[len << 3];
                    if (buffer.Length == 0)
                    {
                        return new ulong[0];
                    }
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        longs = new ulong[len];
                        if (BitConv.IsSameEndian(false))
                        {
                            Buffer.BlockCopy(buffer, 0, longs, 0, buffer.Length);
                        }
                        else
                        {
                            for (int i = 0; i < len; i++)
                            {
                                longs[i] = BitConv.ToUInt64(buffer, i << 3);
                            }
                        }
                        return longs;
                    }
                }
            }
            else if (type == typeof(List<long>))
            {
                long[] longs;
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    len = BitConv.ToInt32(buffer, 0);
                    buffer = new byte[len << 3];
                    if (buffer.Length == 0)
                    {
                        return new List<long>();
                    }
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        longs = new long[len];
                        if (BitConv.IsSameEndian(false))
                        {
                            Buffer.BlockCopy(buffer, 0, longs, 0, buffer.Length);
                        }
                        else
                        {
                            for (int i = 0; i < longs.Length; i++)
                            {
                                longs[i] = BitConv.ToInt64(buffer, i << 3);
                            }
                        }
                        return longs.ToList();
                    }
                }
            }
            else if (type == typeof(List<ulong>))
            {
                ulong[] longs;
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    len = BitConv.ToInt32(buffer, 0);
                    buffer = new byte[len << 3];
                    if (buffer.Length == 0)
                    {
                        return new List<ulong>();
                    }
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        longs = new ulong[len];
                        if (BitConv.IsSameEndian(false))
                        {
                            Buffer.BlockCopy(buffer, 0, longs, 0, buffer.Length);
                        }
                        else
                        {
                            for (int i = 0; i < longs.Length; i++)
                            {
                                longs[i] = BitConv.ToUInt64(buffer, i << 3);
                            }
                        }
                        return longs.ToList();
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Unsupported Type: {type}");
            }
            throw new EndOfStreamException();
        }
    }
}
