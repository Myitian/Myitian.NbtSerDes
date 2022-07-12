using System;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtLongConverter : NbtConverterBase
    {
        public override byte ID => 4;

        public NbtLongConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            switch (value)
            {
                case byte i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 8);
                    break;
                case sbyte i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 8);
                    break;
                case short i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 8);
                    break;
                case ushort i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 8);
                    break;
                case char i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 8);
                    break;
                case int i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 8);
                    break;
                case uint i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 8);
                    break;
                case long i:
                    stream.Write(BitConv.GetBytes(i), 0, 8);
                    break;
                case ulong i:
                    stream.Write(BitConv.GetBytes(i), 0, 8);
                    break;
                case TimeSpan i:
                    stream.Write(BitConv.GetBytes(i.TotalMilliseconds), 0, 8);
                    break;
                case DateTime i:
                    stream.Write(BitConv.GetBytes(i.Ticks / 10000), 0, 8);
                    break;
                case float i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 4);
                    break;
                case double i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 4);
                    break;
                case decimal i:
                    stream.Write(BitConv.GetBytes((long)i), 0, 4);
                    break;
                default:
                    if (value == null)
                    {
                        stream.Write(NbtConverter.Z8, 0, 8);
                    }
                    else
                    {
                        stream.Write(BitConv.GetBytes((long)value.GetHashCode()), 0, 8);
                    }
                    break;
            }
        }

        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            byte[] buffer = new byte[8];
            int read;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
            }
            if (type == typeof(long) || type == typeof(object))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return BitConv.ToInt64(buffer, 0);
                }
            }
            else if (type == typeof(sbyte))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (sbyte)BitConv.ToInt64(buffer, 0);
                }
            }
            else if (type == typeof(short))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (short)BitConv.ToInt64(buffer, 0);
                }
            }
            else if (type == typeof(int))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (int)BitConv.ToInt64(buffer, 0);
                }
            }
            else if (type == typeof(float))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (float)BitConv.ToInt64(buffer, 0);
                }
            }
            else if (type == typeof(double))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (double)BitConv.ToInt64(buffer, 0);
                }
            }
            else if (type == typeof(decimal))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (decimal)BitConv.ToInt64(buffer, 0);
                }
            }
            else if (type == typeof(TimeSpan))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return new TimeSpan(BitConv.ToInt64(buffer, 0) * 10000);
                }
            }
            else if (type == typeof(DateTime))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return new DateTime(BitConv.ToInt64(buffer, 0) * 10000);
                }
            }
            //
            else if (type == typeof(byte))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (byte)BitConv.ToUInt64(buffer, 0);
                }
            }
            else if (type == typeof(ushort))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (ushort)BitConv.ToUInt64(buffer, 0);
                }
            }
            else if (type == typeof(char))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (char)BitConv.ToUInt64(buffer, 0);
                }
            }
            else if (type == typeof(uint))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return (uint)BitConv.ToUInt64(buffer, 0);
                }
            }
            else if (type == typeof(ulong))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return BitConv.ToUInt64(buffer, 0);
                }
            }
            //
            else if (type == typeof(bool))
            {
                read = stream.Read(buffer, 0, 8);
                if (read > 0)
                {
                    return BitConv.ToInt64(buffer, 0) != 0;
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
