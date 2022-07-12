using System;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtShortConverter : NbtConverterBase
    {
        public override byte ID => 2;

        public NbtShortConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            switch (value)
            {
                case bool i:
                    stream.Write(BitConv.GetBytes((short)(i ? 1 : 0)), 0, 2);
                    break;
                case byte i:
                    stream.Write(BitConv.GetBytes(i), 0, 2);
                    break;
                case sbyte i:
                    stream.Write(BitConv.GetBytes(i), 0, 2);
                    break;
                case short i:
                    stream.Write(BitConv.GetBytes(i), 0, 2);
                    break;
                case ushort i:
                    stream.Write(BitConv.GetBytes(i), 0, 2);
                    break;
                case char i:
                    stream.Write(BitConv.GetBytes(i), 0, 2);
                    break;
                case int i:
                    stream.Write(BitConv.GetBytes((short)i), 0, 2);
                    break;
                case uint i:
                    stream.Write(BitConv.GetBytes((short)i), 0, 2);
                    break;
                case long i:
                    stream.Write(BitConv.GetBytes((short)i), 0, 2);
                    break;
                case ulong i:
                    stream.Write(BitConv.GetBytes((short)i), 0, 2);
                    break;
                case float i:
                    stream.Write(BitConv.GetBytes((short)i), 0, 4);
                    break;
                case double i:
                    stream.Write(BitConv.GetBytes((short)i), 0, 4);
                    break;
                case decimal i:
                    stream.Write(BitConv.GetBytes((short)i), 0, 4);
                    break;
                default:
                    if (value == null)
                    {
                        stream.Write(NbtConverter.Z2, 0, 2);
                    }
                    else
                    {
                        stream.Write(BitConv.GetBytes((short)value.GetHashCode()), 0, 2);
                    }
                    break;
            }
        }

        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            byte[] buffer = new byte[2];
            int read;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
            }
            if (type == typeof(short) || type == typeof(object))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return BitConv.ToInt16(buffer, 0);
                }
            }
            //
            else if (type == typeof(sbyte))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (sbyte)BitConv.ToInt16(buffer, 0);
                }
            }
            else if (type == typeof(int))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (short)BitConv.ToInt16(buffer, 0);
                }
            }
            else if (type == typeof(long))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (long)BitConv.ToInt16(buffer, 0);
                }
            }
            else if (type == typeof(float))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (float)BitConv.ToInt16(buffer, 0);
                }
            }
            else if (type == typeof(double))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (double)BitConv.ToInt16(buffer, 0);
                }
            }
            else if (type == typeof(decimal))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (decimal)BitConv.ToInt16(buffer, 0);
                }
            }
            //
            else if (type == typeof(byte))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (byte)BitConv.ToUInt16(buffer, 0);
                }
            }
            else if (type == typeof(ushort))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return BitConv.ToUInt16(buffer, 0);
                }
            }
            else if (type == typeof(char))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (char)BitConv.ToUInt16(buffer, 0);
                }
            }
            else if (type == typeof(uint))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (uint)BitConv.ToUInt16(buffer, 0);
                }
            }
            else if (type == typeof(ulong))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return (ulong)BitConv.ToUInt16(buffer, 0);
                }
            }
            //
            else if (type == typeof(bool))
            {
                read = stream.Read(buffer, 0, 2);
                if (read > 0)
                {
                    return BitConv.ToInt16(buffer, 0) != 0;
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
