using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtListConverter : NbtConverterBase
    {
        public override byte ID => 9;

        public NbtListConverter(NbtConverter converter) : base(converter) { }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            if (value != null)
            {
                Type type = value.GetType();
                if (type.IsArray)
                {
                    Type itype = type.GetElementType();
                    stream.WriteByte(converter.FindTagByType(itype));
                    stream.Write(BitConv.GetBytes(value.Length), 0, 4);
                    foreach (object item in value)
                    {
                        converter.SerializeObjectPayload(ref stream, item);
                    }
                }
                else if (type.IsGenericType)
                {
                    if (type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(ICollection<>)).Length > 0)
                    {
                        Type itype = type.GenericTypeArguments[0];
                        stream.WriteByte(converter.FindTagByType(itype));
                        stream.Write(BitConv.GetBytes(value.Count), 0, 4);
                        foreach (object item in value)
                        {
                            converter.SerializeObjectPayload(ref stream, item);
                        }
                    }
                    else if (type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IEnumerable<>)).Length > 0)
                    {
                        Type itype = type.GenericTypeArguments[0];
                        stream.WriteByte(converter.FindTagByType(itype));
                        Type lstype = typeof(List<>).MakeGenericType(itype);
                        dynamic li = Activator.CreateInstance(lstype, value);
                        stream.Write(BitConv.GetBytes(li.Count), 0, 4);
                        foreach (object item in li)
                        {
                            converter.SerializeObjectPayload(ref stream, item);
                        }
                    }
                }
                else if (type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IEnumerable)).Length > 0)
                {
                    List<object> li = new List<object>();
                    foreach (object item in value)
                    {
                        li.Add(item);
                    }
                    if (li.Count > 0)
                    {
                        Type itype = li[0].GetType();
                        byte tag = converter.FindTagByType(itype);
                        stream.WriteByte(tag);
                        stream.Write(BitConv.GetBytes(li.Count), 0, 4);
                        foreach (object item in li)
                        {
                            if (item.GetType() != itype)
                            {
                                converter.SerializeObjectPayload(ref stream, item);
                            }
                        }
                    }
                    else
                    {
                        stream.WriteByte(converter.FindTagByType(null));
                        stream.Write(BitConv.GetBytes(0), 0, 4);
                    }
                }
                else
                {
                    throw new ArgumentException($"Unsupported Type: {type}");
                }
            }
        }

        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            int read, len;
            if (type == typeof(Array) || type == typeof(object))
            {
                read = stream.ReadByte();
                if (read >= 0)
                {
                    byte tag = (byte)read;
                    Type itype = converter.FindTypeByTag(tag);
                    byte[] buffer = new byte[4];
                    read = stream.Read(buffer, 0, 4);
                    if (read > 0)
                    {
                        len = BitConv.ToInt32(buffer, 0);
                        dynamic li = Array.CreateInstance(itype, len);
                        for (int i = 0; i < len; i++)
                        {
                            li[i] = converter.DeserializeObjectPayload(ref stream, tag, itype);
                        }
                        return li;
                    }
                }
            }
            if (type.IsArray)
            {
                read = stream.ReadByte();
                if (read >= 0)
                {
                    byte tag = (byte)read;
                    Type itype = type.GetElementType();
                    if (converter.CompatibleTypes[tag].Contains(itype) || tag == 0 || tag == 9 || tag == 10) //TODO：0，9，10之类判断之后再完善
                    {
                        byte[] buffer = new byte[4];
                        read = stream.Read(buffer, 0, 4);
                        if (read > 0)
                        {
                            len = BitConv.ToInt32(buffer, 0);
                            dynamic li = Array.CreateInstance(itype, len);
                            for (int i = 0; i < len; i++)
                            {
                                li[i] = converter.DeserializeObjectPayload(ref stream, tag, itype);
                            }
                            return li;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("ElementTypeMisMatch");
                    }
                }
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                read = stream.ReadByte();
                if (read >= 0)
                {
                    byte tag = (byte)read;
                    Type itype = converter.FindTypeByTag(tag);
                    if (itype is null || converter.IsCompatible(tag, type.GenericTypeArguments[0]))
                    {
                        itype = type.GenericTypeArguments[0];
                        byte[] buffer = new byte[4];
                        read = stream.Read(buffer, 0, 4);
                        if (read > 0)
                        {
                            len = BitConv.ToInt32(buffer, 0);
                            dynamic li = Activator.CreateInstance(typeof(List<>).MakeGenericType(itype));
                            for (int i = 0; i < len; i++)
                            {
                                li.Add(converter.DeserializeObjectPayload(ref stream, tag, itype));
                            }
                            return li;
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
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
