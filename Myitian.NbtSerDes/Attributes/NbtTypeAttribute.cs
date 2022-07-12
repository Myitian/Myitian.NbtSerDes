using System;

namespace Myitian.NbtSerDes
{
    public class NbtTypeAttribute : Attribute
    {
        public NbtTypeAttribute(NbtType tagtype)
        {
            TagType = tagtype;
        }

        public NbtType TagType;
    }
}
