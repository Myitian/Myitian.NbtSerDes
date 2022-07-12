using System;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtFloatConverter : NbtConverterBase
    {
        public override byte ID => 5;

        public NbtFloatConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            switch (value)
            {
                case byte i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case sbyte i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case short i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case ushort i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case char i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case int i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case uint i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case long i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case ulong i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case float i:
                    stream.Write(BitConv.GetBytes(i), 0, 4);
                    break;
                case double i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                case decimal i:
                    stream.Write(BitConv.GetBytes((float)i), 0, 4);
                    break;
                default:
                    if (value == null)
                    {
                        stream.WriteByte(0);
                    }
                    else
                    {
                        stream.Write(BitConv.GetBytes((byte)value.GetHashCode()), 0, 1);
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
            if (type == typeof(float) || type == typeof(object))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return BitConv.ToSingle(buffer, 0);
                }
            }
            else if (type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) ||
                type == typeof(ushort) || type == typeof(char) || type == typeof(int) ||
                type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
                type == typeof(double) || type == typeof(decimal))
            {
                read = stream.Read(buffer, 0, 4);
                if (read > 0)
                {
                    return Convert.ChangeType(BitConv.ToSingle(buffer, 0), type);
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
