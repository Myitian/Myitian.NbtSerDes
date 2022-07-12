using System;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtByteConverter : NbtConverterBase
    {
        public override byte ID => 1;

        public NbtByteConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            switch (value)
            {
                case byte i:
                    stream.WriteByte(i);
                    break;
                case sbyte i:
                    stream.WriteByte((byte)i);
                    break;
                case short i:
                    stream.WriteByte((byte)i);
                    break;
                case ushort i:
                    stream.WriteByte((byte)i);
                    break;
                case char i:
                    stream.WriteByte((byte)i);
                    break;
                case int i:
                    stream.WriteByte((byte)i);
                    break;
                case uint i:
                    stream.WriteByte((byte)i);
                    break;
                case long i:
                    stream.WriteByte((byte)i);
                    break;
                case ulong i:
                    stream.WriteByte((byte)i);
                    break;
                case float i:
                    stream.WriteByte((byte)i);
                    break;
                case double i:
                    stream.WriteByte((byte)i);
                    break;
                case decimal i:
                    stream.WriteByte((byte)i);
                    break;
                default:
                    if (value == null)
                    {
                        stream.WriteByte(0);
                    }
                    else
                    {
                        stream.WriteByte((byte)value.GetHashCode());
                    }
                    break;
            }
        }

        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            int read;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
            }
            if (type == typeof(sbyte) || type == typeof(object))
            {
                read = stream.ReadByte();
                if (read >= 0)
                {
                    return (sbyte)read;
                }
            }
            else if (type == typeof(short) || type == typeof(int) || type == typeof(long) ||
                type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                read = stream.ReadByte();
                if (read >= 0)
                {
                    return Convert.ChangeType((sbyte)read, type);
                }
            }
            else if (type == typeof(byte) || type == typeof(ushort) || type == typeof(char) || type == typeof(uint) || type == typeof(ulong))
            {
                read = stream.ReadByte();
                if (read >= 0)
                {
                    return Convert.ChangeType((byte)read, type);
                }
            }
            else if (type == typeof(bool))
            {
                read = stream.ReadByte();
                if (read >= 0)
                {
                    return read != 0;
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
