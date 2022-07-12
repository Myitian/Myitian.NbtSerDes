using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Myitian.NbtSerDes
{
    public class NbtIntArrayConverter : NbtConverterBase
    {
        public override byte ID => 11;

        public NbtIntArrayConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            if (value != null)
            {
                byte[] buffer;
                Type type = value.GetType();
                if (value is int[] ints)
                {
                    stream.Write(BitConv.GetBytes(ints.Length), 0, 4);
                    buffer = new byte[ints.Length << 2];
                    if (BitConv.IsSameEndian(false))
                    {
                        Buffer.BlockCopy(ints, 0, buffer, 0, buffer.Length);
                    }
                    else
                    {
                        for (int i = 0; i < ints.Length; i++)
                        {
                            BitConv.GetBytes(ints[i]).CopyTo(buffer, i << 2);
                        }
                    }
                }
                else if (type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IEnumerable<int>)).Length > 0)
                {
                    ints = (value as IEnumerable<int>).ToArray();
                    stream.Write(BitConv.GetBytes(ints.Length), 0, 4);
                    buffer = new byte[ints.Length << 2];
                    if (BitConv.IsSameEndian(false))
                    {
                        Buffer.BlockCopy(ints, 0, buffer, 0, buffer.Length);
                    }
                    else
                    {
                        for (int i = 0; i < ints.Length; i++)
                        {
                            BitConv.GetBytes(ints[i]).CopyTo(buffer, i << 2);
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
            byte[] buffer;
            int[] ints;
            if (type == typeof(int[]) || type == typeof(Array) || type == typeof(object))
            {
                buffer = new byte[4];
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    len = BitConv.ToInt32(buffer, 0);
                    buffer = new byte[len << 2];
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        ints = new int[len];
                        if (BitConv.IsSameEndian(false))
                        {
                            Buffer.BlockCopy(buffer, 0, ints, 0, buffer.Length);
                        }
                        else
                        {
                            for (int i = 0; i < len; i++)
                            {
                                ints[i] = BitConv.ToInt32(buffer, i << 2);
                            }
                        }
                        return ints;
                    }
                }
            }
            else if (type == typeof(List<int>))
            {
                buffer = new byte[4];
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    len = BitConv.ToInt32(buffer, 0);
                    buffer = new byte[len << 2];
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        ints = new int[len];
                        if (BitConv.IsSameEndian(false))
                        {
                            Buffer.BlockCopy(buffer, 0, ints, 0, buffer.Length);
                        }
                        else
                        {
                            for (int i = 0; i < len; i++)
                            {
                                ints[i] = BitConv.ToInt32(buffer, i << 2);
                            }
                        }
                        return ints.ToList();
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
