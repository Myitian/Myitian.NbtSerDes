using System;

namespace Myitian.NbtSerDes
{
    public class NbtPropertyAttribute : Attribute
    {
        public NbtPropertyAttribute(string name)
        {
            PropertyName = name;
        }

        public string PropertyName;
    }
}
