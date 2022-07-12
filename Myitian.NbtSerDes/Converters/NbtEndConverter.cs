using System;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtEndConverter : NbtConverterBase
    {
        public override byte ID => 0;

        public NbtEndConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {

        }
        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            return Activator.CreateInstance(type);
        }

    }
}
