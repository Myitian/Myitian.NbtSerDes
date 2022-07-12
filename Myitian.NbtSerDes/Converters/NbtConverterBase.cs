using System;
using System.IO;

namespace Myitian.NbtSerDes
{
    public abstract class NbtConverterBase
    {
        public NbtConverter converter;
        public abstract byte ID { get; }

        public NbtConverterBase(NbtConverter converter)
        {
            this.converter = converter;
        }
        public abstract void Serialize(ref Stream stream, dynamic value);
        public abstract dynamic Deserialize(ref Stream stream, Type type);
    }
}
