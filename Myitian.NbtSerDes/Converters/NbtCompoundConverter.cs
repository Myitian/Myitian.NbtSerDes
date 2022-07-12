using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Myitian.NbtSerDes
{
    public class NbtCompoundConverter : NbtConverterBase
    {
        public override byte ID => 10;

        public NbtCompoundConverter(NbtConverter converter) : base(converter) { }

        private struct MemberInfoX
        {
            public string Name;
            public dynamic Value;
            public NbtConverterBase Converter;

            public MemberInfoX(string name, dynamic value, NbtConverterBase converter)
            {
                Name = name;
                Value = value;
                Converter = converter;
            }
        }
        private struct FieldInfoX
        {
            public string Name;
            public FieldInfo Value;
            public Type Type;
            public NbtConverterBase Converter;

            public FieldInfoX(string name, FieldInfo value, Type type, NbtConverterBase converter)
            {
                Name = name;
                Value = value;
                Type = type;
                Converter = converter;
            }
        }
        private struct PropertyInfoX
        {
            public string Name;
            public PropertyInfo Value;
            public Type Type;
            public NbtConverterBase Converter;

            public PropertyInfoX(string name, PropertyInfo value, Type type, NbtConverterBase converter)
            {
                Name = name;
                Value = value;
                Type = type;
                Converter = converter;
            }
        }

        public override void Serialize(ref Stream stream, dynamic value)
        {
            Type type = value.GetType();
            if (type.IsGenericType)
            {
                Type[] types;
                if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>) && value.Key is string && value.Value != null)
                {
                    converter.SerializeObject(ref stream, value.Key, value.Value);
                    converter.SerializeObject(ref stream, null, null);
                    return;
                }
                else if ((types = type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IDictionary<,>))).Length > 0)
                {
                    Type[] kvtypes = types[0].GetGenericArguments();
                    if (kvtypes[0] == typeof(string))
                    {
                        dynamic dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(kvtypes), value);
                        foreach (dynamic kvp in dict)
                        {
                            if (kvp.Value != null)
                            {
                                converter.SerializeObject(ref stream, kvp.Key, kvp.Value);
                            }
                        }
                        converter.SerializeObject(ref stream, null, null);
                        return;
                    }
                }
                else if ((types = type.FindInterfaces(NbtConverter.HasImplementedRawGeneric, typeof(IDictionary))).Length > 0)
                {
                    foreach (DictionaryEntry kvp in value as IDictionary)
                    {
                        Type ktype = kvp.Key.GetType();
                        if (ktype != typeof(string))
                        {
                            throw new ArgumentException($"Unsupported Type: {type} contains {ktype} key");
                        }
                        if (kvp.Value != null)
                        {
                            converter.SerializeObject(ref stream, kvp.Key.ToString(), kvp.Value);
                        }
                    }
                    converter.SerializeObject(ref stream, null, null);
                    return;
                }
            }
            if ((!type.IsPrimitive && !type.IsEnum && type.IsValueType) || type.IsClass) // Struct or Class
            {
                FieldInfo[] fields = type.GetFields();
                PropertyInfo[] properties = type.GetProperties();
                List<MemberInfoX> members = new List<MemberInfoX>();
                NbtConverterBase usingconverter;
                string name;
                Attribute attr;
                foreach (FieldInfo field in fields)
                {
                    usingconverter = null;
                    name = field.Name;
                    if ((attr = Attribute.GetCustomAttribute(field, typeof(NbtIgnoreAttribute))) != null)
                    {
                        continue;
                    }
                    if ((attr = field.GetCustomAttribute(typeof(NbtRequiredAttribute))) != null)
                    {
                        if (field.GetValue(value) is null)
                        {
                            throw new ArgumentNullException(field.ToString(), value.ToString());
                        }
                    }
                    if ((attr = field.GetCustomAttribute(typeof(NbtTypeAttribute))) != null)
                    {
                        usingconverter = converter.FindConverterByTag((byte)(attr as NbtTypeAttribute).TagType);
                    }
                    if ((attr = field.GetCustomAttribute(typeof(NbtPropertyAttribute))) != null)
                    {
                        name = (attr as NbtPropertyAttribute).PropertyName;
                    }
                    members.Add(new MemberInfoX(name, field.GetValue(value), usingconverter));
                }
                foreach (PropertyInfo property in properties)
                {
                    if (property.CanRead)
                    {
                        usingconverter = null;
                        name = property.Name;
                        if ((attr = property.GetCustomAttribute(typeof(NbtIgnoreAttribute))) != null)
                        {
                            continue;
                        }
                        if ((attr = property.GetCustomAttribute(typeof(NbtRequiredAttribute))) != null)
                        {
                            if (property.GetValue(value) is null)
                            {
                                throw new ArgumentNullException(property.ToString(), value.ToString());
                            }
                        }
                        if ((attr = property.GetCustomAttribute(typeof(NbtTypeAttribute))) != null)
                        {
                            usingconverter = converter.FindConverterByTag((byte)(attr as NbtTypeAttribute).TagType);
                        }
                        if ((attr = property.GetCustomAttribute(typeof(NbtPropertyAttribute))) != null)
                        {
                            name = (attr as NbtPropertyAttribute).PropertyName;
                        }
                        members.Add(new MemberInfoX(name, property.GetValue(value), usingconverter));
                    }
                }
                foreach (MemberInfoX member in members)
                {
                    if (!(member.Value is null))
                    {
                        converter.SerializeObject(ref stream, member.Name, member.Value, member.Converter);
                    }
                }
                converter.SerializeObject(ref stream, null, null);
                return;
            }
            throw new ArgumentException($"Unsupported Type: {type}");
        }
        public override dynamic Deserialize(ref Stream stream, Type type)
        {
            if (type == typeof(object))
            {
                dynamic dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(object)));
                string name = converter.DeserializeObject(ref stream, typeof(object), out dynamic payload);
                while (name != null && payload != null)
                {
                    dict.Add(name, payload);
                    name = converter.DeserializeObject(ref stream, typeof(object), out payload);
                }
                return dict;
            }
            if (type.IsGenericType)
            {
                Type[] types;
                if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    types = type.GetGenericArguments();
                    if (types[0] == typeof(string))
                    {
                        string name = converter.DeserializeObject(ref stream, types[1], out dynamic payload);
                        return Activator.CreateInstance(type, name, payload);
                    }
                    converter.DeserializeObject(ref stream, null, out _);
                }
                else if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    types = type.GetGenericArguments();
                    if (types[0] == typeof(string))
                    {
                        dynamic dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(types));
                        string name = converter.DeserializeObject(ref stream, types[1], out dynamic payload);
                        while (name != null && payload != null)
                        {
                            dict.Add(name, payload);
                            name = converter.DeserializeObject(ref stream, types[1], out payload);
                        }
                        return dict;
                    }
                }
            }
            if ((!type.IsPrimitive && !type.IsEnum && type.IsValueType) || type.IsClass) // Struct or Class
            {
                FieldInfo[] fields = type.GetFields();
                PropertyInfo[] properties = type.GetProperties();
                List<FieldInfoX> availableFields = new List<FieldInfoX>();
                List<PropertyInfoX> availableProperties = new List<PropertyInfoX>();
                NbtConverterBase usingconverter;
                string name;
                Attribute attr;
                foreach (FieldInfo field in fields)
                {
                    usingconverter = null;
                    name = field.Name;
                    if ((attr = Attribute.GetCustomAttribute(field, typeof(NbtIgnoreAttribute))) != null)
                    {
                        continue;
                    }
                    if ((attr = field.GetCustomAttribute(typeof(NbtTypeAttribute))) != null)
                    {
                        usingconverter = converter.FindConverterByTag((byte)(attr as NbtTypeAttribute).TagType);
                    }
                    if ((attr = field.GetCustomAttribute(typeof(NbtPropertyAttribute))) != null)
                    {
                        name = (attr as NbtPropertyAttribute).PropertyName;
                    }
                    availableFields.Add(new FieldInfoX(name, field, field.FieldType, usingconverter));
                }
                foreach (PropertyInfo property in properties)
                {
                    if (property.CanRead)
                    {
                        usingconverter = null;
                        name = property.Name;
                        if ((attr = property.GetCustomAttribute(typeof(NbtIgnoreAttribute))) != null)
                        {
                            continue;
                        }
                        if ((attr = property.GetCustomAttribute(typeof(NbtTypeAttribute))) != null)
                        {
                            usingconverter = converter.FindConverterByTag((byte)(attr as NbtTypeAttribute).TagType);
                        }
                        if ((attr = property.GetCustomAttribute(typeof(NbtPropertyAttribute))) != null)
                        {
                            name = (attr as NbtPropertyAttribute).PropertyName;
                        }
                        availableProperties.Add(new PropertyInfoX(name, property, property.PropertyType, usingconverter));
                    }
                }
                dynamic parent = Activator.CreateInstance(type);
                string kvname;
                int i;
                int read = stream.ReadByte();
                while (read > 0)
                {
                    kvname = NbtNameStringConverter.Deserialize(ref stream);
                    if ((i = availableFields.FindIndex(m => m.Name == kvname)) >= 0)
                    {
                        availableFields[i].Value.SetValue(parent, converter.DeserializeObjectPayload(ref stream, (byte)read, availableFields[i].Type, availableFields[i].Converter));
                    }
                    else if ((i = availableProperties.FindIndex(m => m.Name == kvname)) >= 0)
                    {
                        availableProperties[i].Value.SetValue(parent, converter.DeserializeObjectPayload(ref stream, (byte)read, availableProperties[i].Type, availableProperties[i].Converter));
                    }
                    read = stream.ReadByte();
                }
                return parent;
            }
            throw new ArgumentException($"Unsupported Type: {type}");
        }

    }
}
