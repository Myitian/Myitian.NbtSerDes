using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Myitian.NbtSerDes
{
    public class NbtConverter
    {
        internal static readonly byte[] Z1 = new byte[1];
        internal static readonly byte[] Z2 = new byte[2];
        internal static readonly byte[] Z4 = new byte[4];
        internal static readonly byte[] Z8 = new byte[8];

        private readonly NbtConverterBase[] converters;
        public readonly Type[] TagToType =
        {
            null,
            typeof(sbyte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(byte[]),
            typeof(string),
            typeof(object[]),
            typeof(Dictionary<string, object>),
            typeof(int[]),
            typeof(long[]),
        };

        public readonly HashSet<Type>[] CompatibleTypes =
        {
            null,
            new HashSet<Type>()
            {
                typeof(bool),
                typeof(bool?),
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(short),
                typeof(short?),
                typeof(ushort),
                typeof(ushort?),
                typeof(char),
                typeof(char?),
                typeof(int),
                typeof(int?),
                typeof(uint),
                typeof(uint?),
                typeof(long),
                typeof(long?),
                typeof(ulong),
                typeof(ulong?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?),
                typeof(Enum)
            },
            new HashSet<Type>()
            {
                typeof(bool),
                typeof(bool?),
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(short),
                typeof(short?),
                typeof(ushort),
                typeof(ushort?),
                typeof(char),
                typeof(char?),
                typeof(int),
                typeof(int?),
                typeof(uint),
                typeof(uint?),
                typeof(long),
                typeof(long?),
                typeof(ulong),
                typeof(ulong?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            },
            new HashSet<Type>()
            {
                typeof(bool),
                typeof(bool?),
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(short),
                typeof(short?),
                typeof(ushort),
                typeof(ushort?),
                typeof(char),
                typeof(char?),
                typeof(int),
                typeof(int?),
                typeof(uint),
                typeof(uint?),
                typeof(long),
                typeof(long?),
                typeof(ulong),
                typeof(ulong?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            },
            new HashSet<Type>()
            {
                typeof(bool),
                typeof(bool?),
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(short),
                typeof(short?),
                typeof(ushort),
                typeof(ushort?),
                typeof(char),
                typeof(char?),
                typeof(int),
                typeof(int?),
                typeof(uint),
                typeof(uint?),
                typeof(long),
                typeof(long?),
                typeof(ulong),
                typeof(ulong?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            },
            new HashSet<Type>()
            {
                typeof(bool),
                typeof(bool?),
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(short),
                typeof(short?),
                typeof(ushort),
                typeof(ushort?),
                typeof(char),
                typeof(char?),
                typeof(int),
                typeof(int?),
                typeof(uint),
                typeof(uint?),
                typeof(long),
                typeof(long?),
                typeof(ulong),
                typeof(ulong?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            },
            new HashSet<Type>()
            {
                typeof(bool),
                typeof(bool?),
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(short),
                typeof(short?),
                typeof(ushort),
                typeof(ushort?),
                typeof(char),
                typeof(char?),
                typeof(int),
                typeof(int?),
                typeof(uint),
                typeof(uint?),
                typeof(long),
                typeof(long?),
                typeof(ulong),
                typeof(ulong?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            },
            new HashSet<Type>()
            {
                typeof(byte[]),
                typeof(sbyte[]),
                typeof(List<byte>),
                typeof(List<sbyte>)
            },
            new HashSet<Type>()
            {
                typeof(string),
                typeof(char[]),
                typeof(List<char>)
            },
            new HashSet<Type>()
            {
                typeof(Array),
                typeof(List<>),
                typeof(IEnumerable<>),
                typeof(IEnumerable)
            },
            new HashSet<Type>()
            {
                typeof(KeyValuePair<,>),
                typeof(Dictionary<,>),
                typeof(IDictionary<,>),
                typeof(IDictionary)
            },
            new HashSet<Type>()
            {
                typeof(int[]),
                typeof(uint[]),
                typeof(List<int>),
                typeof(List<uint>)
            },
            new HashSet<Type>()
            {
                typeof(long[]),
                typeof(ulong[]),
                typeof(List<long>),
                typeof(List<ulong>)
            },
        };
        public Dictionary<Type, NbtConverterBase> RegisteredConverter;
        public List<KeyValuePair<Type, NbtConverterBase>> RegisteredKnownTypeGenericInterfaceConverter;
        public List<KeyValuePair<Type, NbtConverterBase>> RegisteredGenericInterfaceConverter;
        public List<KeyValuePair<Type, NbtConverterBase>> RegisteredInterfaceConverter;
        public NbtConverterBase RegisteredArrayConverter;
        public NbtConverterBase RegisteredCompoundConverter;

        public NbtConverter()
        {
            converters = new NbtConverterBase[]
            {
                new NbtEndConverter(this),
                new NbtByteConverter(this),
                new NbtShortConverter(this),
                new NbtIntConverter(this),
                new NbtLongConverter(this),
                new NbtFloatConverter(this),
                new NbtDoubleConverter(this),
                new NbtByteArrayConverter(this),
                new NbtStringConverter(this),
                new NbtListConverter(this),
                new NbtCompoundConverter(this),
                new NbtIntArrayConverter(this),
                new NbtLongArrayConverter(this)
            };
            RegisteredConverter = new Dictionary<Type, NbtConverterBase>()
            {
                { typeof(bool), converters[1] },
                { typeof(bool?), converters[1] },
                { typeof(byte), converters[1] },
                { typeof(byte?), converters[1] },
                { typeof(sbyte), converters[1] },
                { typeof(sbyte?), converters[1] },
                { typeof(short), converters[2] },
                { typeof(short?), converters[2] },
                { typeof(ushort), converters[2] },
                { typeof(ushort?), converters[2] },
                { typeof(char), converters[2] },
                { typeof(char?), converters[2] },
                { typeof(int), converters[3] },
                { typeof(int?), converters[3] },
                { typeof(uint), converters[3] },
                { typeof(uint?), converters[3] },
                { typeof(long), converters[4] },
                { typeof(long?), converters[4] },
                { typeof(ulong), converters[4] },
                { typeof(ulong?), converters[4] },
                { typeof(float), converters[5] },
                { typeof(float?), converters[5] },
                { typeof(double), converters[6] },
                { typeof(double?), converters[6] },
                { typeof(decimal), converters[6] },
                { typeof(decimal?), converters[6] },
                { typeof(byte[]), converters[7] },
                { typeof(sbyte[]), converters[7] },
                { typeof(string), converters[8] },
                { typeof(char[]), converters[8] },
                { typeof(int[]), converters[11] },
                { typeof(uint[]), converters[11] },
                { typeof(long[]), converters[12] },
                { typeof(ulong[]), converters[12] }
            };
            RegisteredKnownTypeGenericInterfaceConverter = new List<KeyValuePair<Type, NbtConverterBase>>()
            {
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<byte>), converters[7]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<byte>), converters[7]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<sbyte>), converters[7]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<sbyte>), converters[7]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<char>), converters[8]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<char>), converters[8]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<int>), converters[11]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<int>), converters[11]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<uint>), converters[11]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<uint>), converters[11]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<long>), converters[12]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<long>), converters[12]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<ulong>), converters[12]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<ulong>), converters[12])
            };
            RegisteredGenericInterfaceConverter = new List<KeyValuePair<Type, NbtConverterBase>>()
            {
                new KeyValuePair<Type, NbtConverterBase>(typeof(IDictionary<,>), converters[10]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(ICollection<>), converters[9]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable<>), converters[9])
            };
            RegisteredInterfaceConverter = new List<KeyValuePair<Type, NbtConverterBase>>()
            {
                new KeyValuePair<Type, NbtConverterBase>(typeof(IDictionary), converters[10]),
                new KeyValuePair<Type, NbtConverterBase>(typeof(IEnumerable), converters[9])
            };
            RegisteredArrayConverter = converters[9];
            RegisteredCompoundConverter = converters[10];
        }

        public bool IsCompatible(byte tag, Type type)
        {
            foreach (Type t in CompatibleTypes[tag])
            {
                if ((type == t) ||
                    (type.BaseType == t) ||
                    (type.IsGenericType && type.GetGenericTypeDefinition() == t) ||
                    (type.FindInterfaces(HasImplementedRawGeneric, t).Length > 0))
                {
                    return true;
                }
            }
            return false;
        }
        public byte FindTagByType(Type type)
        {
            if (type == null)
            {
                return RegisteredCompoundConverter.ID;
            }
            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredConverter)
            {
                if (type == kvp.Key)
                {
                    return kvp.Value.ID;
                }
            }
            if (type.IsArray)
            {
                return RegisteredArrayConverter.ID;
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredKnownTypeGenericInterfaceConverter)
            {
                if (type.FindInterfaces(HasImplementedRawGeneric, kvp.Key).Length > 0)
                {
                    return kvp.Value.ID;
                }
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredGenericInterfaceConverter)
            {
                if (type.FindInterfaces(HasImplementedRawGeneric, kvp.Key).Length > 0)
                {
                    return kvp.Value.ID;
                }
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredInterfaceConverter)
            {
                if (type.FindInterfaces(HasImplementedRawGeneric, kvp.Key).Length > 0)
                {
                    return kvp.Value.ID;
                }
            }
            if ((!type.IsPrimitive && !type.IsEnum && type.IsValueType) || type.IsClass)
            {
                return RegisteredCompoundConverter.ID;
            }
            throw new ArgumentException($"Unsupported Type: {type}");
        }
        public Type FindTypeByTag(byte tag)
        {
            if (tag >= 0 && tag < TagToType.Length)
            {
                return TagToType[tag];
            }
            throw new ArgumentException($"Unsupported tag: {tag}");
        }
        public NbtConverterBase FindConverterByType(Type type)
        {
            if (type == null)
            {
                return RegisteredCompoundConverter;
            }
            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredConverter)
            {
                if (type == kvp.Key)
                {
                    return kvp.Value;
                }
            }
            if (type.IsArray)
            {
                return RegisteredArrayConverter;
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredKnownTypeGenericInterfaceConverter)
            {
                if (type.FindInterfaces(HasImplementedRawGeneric, kvp.Key).Length > 0)
                {
                    return kvp.Value;
                }
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredGenericInterfaceConverter)
            {
                if (type.FindInterfaces(HasImplementedRawGeneric, kvp.Key).Length > 0)
                {
                    return kvp.Value;
                }
            }
            foreach (KeyValuePair<Type, NbtConverterBase> kvp in RegisteredInterfaceConverter)
            {
                if (type.FindInterfaces(HasImplementedRawGeneric, kvp.Key).Length > 0)
                {
                    return kvp.Value;
                }
            }
            if ((!type.IsPrimitive && !type.IsEnum && type.IsValueType) || type.IsClass)
            {
                return RegisteredCompoundConverter;
            }
            throw new ArgumentException($"Unsupported Type: {type}");
        }
        public NbtConverterBase FindConverterByTag(byte tag)
        {
            if (tag >= 0 && tag < converters.Length)
            {
                return converters[tag];
            }
            throw new ArgumentException($"Unsupported tag: {tag}");
        }
        public T DeserializeObject<T>(Stream nbt)
        {
            DeserializeObject(ref nbt, typeof(T), out dynamic payload);
            return (T)payload;
        }
        public void SerializeObject(ref Stream stream, string name, dynamic value, NbtConverterBase converter = null)
        {
            if (value is null)
            {
                stream.WriteByte(0);
            }
            else
            {
                if (converter is null)
                {
                    Type type = value.GetType();
                    stream.WriteByte(FindTagByType(type));
                }
                else
                {
                    stream.WriteByte(converter.ID);
                }
                NbtNameStringConverter.Serialize(ref stream, name);
                SerializeObjectPayload(ref stream, value, converter);
            }
        }
        public string DeserializeObject(ref Stream stream, Type tagretType, out dynamic payload, NbtConverterBase converter = null)
        {
            int read = stream.ReadByte();
            if (read >= 0)
            {
                if (read == 0)
                {
                    payload = null;
                    return null;
                }
                else
                {
                    string name = NbtNameStringConverter.Deserialize(ref stream);
                    payload = DeserializeObjectPayload(ref stream, (byte)read, tagretType, converter);
                    return name;
                }
            }
            throw new EndOfStreamException();
        }
        public void SerializeObjectPayload(ref Stream stream, dynamic value, NbtConverterBase converter = null)
        {
            if (converter == null)
            {
                FindConverterByType(value.GetType()).Serialize(ref stream, value);
            }
            else
            {
                converter.Serialize(ref stream, value);
            }
        }
        public dynamic DeserializeObjectPayload(ref Stream stream, byte tag, Type tagretType, NbtConverterBase converter = null)
        {
            if (converter == null)
            {
                return FindConverterByTag(tag).Deserialize(ref stream, tagretType);
            }
            else
            {
                return converter.Deserialize(ref stream, tagretType);
            }
        }

        public static NbtConverter DefaultConverter { get; set; } = new NbtConverter();
        public static byte[] SerializeObject(dynamic value, string name = "")
        {
            Stream ms = new MemoryStream();
            DefaultConverter.SerializeObject(ref ms, name, value);
            ms.Position = 0;
            byte[] bytes = Util.CopyStream(ms);
            ms.Close();
            return bytes;
        }
        public static T DeserializeObject<T>(byte[] nbt)
        {
            Stream ms = new MemoryStream(nbt);
            DefaultConverter.DeserializeObject(ref ms, typeof(T), out dynamic payload);
            T result = (T)payload;
            ms.Close();
            return result;
        }
        public static bool HasImplementedRawGeneric(Type m, object filterCriteria) => (m.IsGenericType ? m.GetGenericTypeDefinition() : m) == filterCriteria as Type;
    }
}
