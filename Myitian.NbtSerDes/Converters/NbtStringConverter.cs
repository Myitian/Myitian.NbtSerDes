using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Myitian.NbtSerDes
{
    public class NbtStringConverter : NbtConverterBase
    {
        public override byte ID => 8;

        public NbtStringConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            if (value == null)
            {
                stream.WriteByte(0);
                stream.WriteByte(0);
            }
            else
            {
                switch (value)
                {
                    case string s:
                        NbtNameStringConverter.Serialize(ref stream, s);
                        break;
                    case char[] c:
                        NbtNameStringConverter.Serialize(ref stream, new string(c));
                        break;
                    default:
                        Type type = value.GetType();
                        if (type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IEnumerable<char>)).Length > 0)
                        {
                            NbtNameStringConverter.Serialize(ref stream, new string((value as IEnumerable<char>).ToArray()));
                        }
                        else
                        {
                            NbtNameStringConverter.Serialize(ref stream, value.ToString());
                        }
                        break;
                }
            }
        }
        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            string result = NbtNameStringConverter.Deserialize(ref stream);
            if (type == typeof(string) || type == typeof(object))
            {
                return result;
            }
            else if (type == typeof(char[]))
            {
                return result.ToCharArray();
            }
            else if (type == typeof(List<char>))
            {
                return result.ToCharArray().ToList();
            }
            throw new ArgumentException($"Unsupported Type: {type}");
        }

    }
}
