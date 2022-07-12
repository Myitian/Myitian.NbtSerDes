using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Myitian.NbtSerDes
{
    public class NbtByteArrayConverter : NbtConverterBase
    {
        public override byte ID => 7;

        public NbtByteArrayConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            if (value != null)
            {
                Type type = value.GetType();
                if (value is byte[] bytes)
                {
                    stream.Write(BitConv.GetBytes(bytes.Length), 0, 4);
                    stream.Write(bytes, 0, bytes.Length);
                    return;
                }
                else if (type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IEnumerable<byte>)).Length > 0)
                {
                    bytes = (value as IEnumerable<byte>).ToArray();
                    stream.Write(BitConv.GetBytes(bytes.Length), 0, 4);
                    stream.Write(bytes, 0, bytes.Length);
                    return;
                }
                throw new ArgumentException($"Unsupported Type: {type}");
            }
        }

        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            int read;
            byte[] buffer;
            if (type == typeof(byte[]))
            {
                buffer = new byte[4];
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    buffer = new byte[BitConv.ToInt32(buffer, 0)];
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        return buffer;
                    }
                }
                throw new EndOfStreamException();
            }
            else if (type == typeof(sbyte[]) || type == typeof(Array) || type == typeof(object))
            {
                buffer = new byte[4];
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    buffer = new byte[BitConv.ToInt32(buffer, 0)];
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        sbyte[] sbuffer = new sbyte[buffer.Length];
                        Buffer.BlockCopy(buffer, 0, sbuffer, 0, buffer.Length);
                        return sbuffer;
                    }
                }
                throw new EndOfStreamException();
            }
            else if (type == typeof(List<byte>))
            {
                buffer = new byte[4];
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    buffer = new byte[BitConv.ToInt32(buffer, 0)];
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        return buffer.ToList();
                    }
                }
                throw new EndOfStreamException();
            }
            else if (type == typeof(List<sbyte>))
            {
                buffer = new byte[4];
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    buffer = new byte[BitConv.ToInt32(buffer, 0)];
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        sbyte[] sbuffer = new sbyte[buffer.Length];
                        Buffer.BlockCopy(buffer, 0, sbuffer, 0, buffer.Length);
                        return sbuffer;
                    }
                }
                throw new EndOfStreamException();
            }
            throw new ArgumentException($"Unsupported Type: {type}");
        }
    }
}
