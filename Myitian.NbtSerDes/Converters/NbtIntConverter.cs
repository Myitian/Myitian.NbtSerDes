using System;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtIntConverter : NbtConverterBase
    {
        public override byte ID => 3;

        public NbtIntConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            switch (value)
            {
                case bool i:
                    stream.Write(BitConv.GetBytes(i ? 1 : 0), 0, 2);
                    break;
                case byte i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case sbyte i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case short i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case ushort i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case char i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case int i:
                    stream.Write(BitConv.GetBytes(i), 0, 4);
                    break;
                case uint i:
                    stream.Write(BitConv.GetBytes(i), 0, 4);
                    break;
                case long i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case ulong i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case float i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case double i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                case decimal i:
                    stream.Write(BitConv.GetBytes((int)i), 0, 4);
                    break;
                default:
                    if (value == null)
                    {
                        stream.Write(NbtConverter.Z4, 0, 4);
                    }
                    else
                    {
                        stream.Write(BitConv.GetBytes(value.GetHashCode()), 0, 4);
                    }
                    break;
            }
        }

        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            byte[] buffer = new byte[4];
            int read;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
            }
            if (type == typeof(int) || type == typeof(object))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return BitConv.ToInt32(buffer, 0);
                }
            }
            //
            else if (type == typeof(sbyte))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (sbyte)BitConv.ToInt32(buffer, 0);
                }
            }
            else if (type == typeof(short))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (short)BitConv.ToInt32(buffer, 0);
                }
            }
            else if (type == typeof(long))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (long)BitConv.ToInt32(buffer, 0);
                }
            }
            else if (type == typeof(float))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (float)BitConv.ToInt32(buffer, 0);
                }
            }
            else if (type == typeof(double))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (double)BitConv.ToInt32(buffer, 0);
                }
            }
            else if (type == typeof(decimal))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (decimal)BitConv.ToInt32(buffer, 0);
                }
            }
            //
            else if (type == typeof(byte))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (byte)BitConv.ToUInt32(buffer, 0);
                }
            }
            else if (type == typeof(ushort))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (ushort)BitConv.ToUInt32(buffer, 0);
                }
            }
            else if (type == typeof(char))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (char)BitConv.ToUInt32(buffer, 0);
                }
            }
            else if (type == typeof(uint))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return BitConv.ToUInt32(buffer, 0);
                }
            }
            else if (type == typeof(ulong))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return (ulong)BitConv.ToUInt32(buffer, 0);
                }
            }
            //
            else if (type == typeof(bool))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return BitConv.ToInt32(buffer, 0) != 0;
                }
            }
            else
            {
                throw new ArgumentException($"Unsupported T: {type}");
            }
            throw new EndOfStreamException();
        }
    }
}
