using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace UAssetParser
{
    public static class LSerializer
    {
        // A DescriptionAttribute is used to get Offset. Similarly a CategoryAttribute is used to get Count.
        // If it starts with 0x, it's considered a number. However any other value will be checked against it's own structure.
        private readonly static Dictionary<Type, (PropertyInfo, string, object, object, object)[]> PropertyMap = new Dictionary<Type, (PropertyInfo, string, object, object, object)[]>();

        public delegate object PrimitiveVisitor(BinaryReader reader);

        public static Dictionary<Type, PrimitiveVisitor> PrimitiveMap = new Dictionary<Type, PrimitiveVisitor>()
        {
            { typeof(sbyte), (r) => r.ReadSByte() },
            { typeof(byte),  (r) => r.ReadByte() },
            { typeof(ushort), (r) => r.ReadUInt16() },
            { typeof(short), (r) => r.ReadInt16() },
            { typeof(uint), (r) => r.ReadUInt32() },
            { typeof(int), (r) => r.ReadInt32() },
            { typeof(ulong), (r) => r.ReadUInt64() },
            { typeof(long), (r) => r.ReadInt64() },
            { typeof(bool), (r) => {
                    var value = r.ReadByte() != 0; // XD
                    r.BaseStream.Position += 3;
                    return value; } },
            { typeof(char), (r) => r.ReadChar() },
            { typeof(decimal), (r) => r.ReadDecimal() },
            { typeof(double), (r) => r.ReadDouble() },
            { typeof(float), (r) => r.ReadSingle() },
            { typeof(string), FString },
            { typeof(Guid), (r) => FGuid(r) },
        };

        public static T Deserialize<T>(Stream data, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(data, Encoding.UTF8, leaveOpen);
            return Deserialize<T>(reader);
        }

        public static T Deserialize<T>(BinaryReader data)
        {
            return (T)Deserialize(data, typeof(T));
        }

        public static object Deserialize(BinaryReader data, Type T, int? countRef = null)
        {
            if (T.IsEnum) T = T.GetEnumUnderlyingType();
            return PrimitiveMap.TryGetValue(T, out var visitor) ? visitor(data) : DeserializeComplex(data, T, countRef);
        }

        private static object DeserializeComplex(BinaryReader data, Type T, int? countRef = null)
        {

            if (T.IsArray) return countRef.HasValue ? FArray(data, (int)countRef, T.GetElementType()) : FArray(data, T.GetElementType());

            var map = GetProperties(T);
            var instance = Activator.CreateInstance(T);
            foreach (var (property, kind, offsetBlob, countBlob, conditionBlob) in map)
            {
                if (conditionBlob is PropertyInfo conditionProperty)
                {
                    var condition = (int)Convert.ChangeType(conditionProperty.GetValue(instance), typeof(int));
                    if (condition != 1) continue;
                }
                int? offset = offsetBlob as int?;
                int? count = countBlob as int?;
                if (!offset.HasValue && offsetBlob is PropertyInfo offsetProperty) offset = (int)Convert.ChangeType(offsetProperty.GetValue(instance), typeof(int));
                if (!count.HasValue && countBlob is PropertyInfo countProperty) count = (int)Convert.ChangeType(countProperty.GetValue(instance), typeof(int));

                var pos = data.BaseStream.Position;
                if (offset.HasValue)
                    data.BaseStream.Position = (int)offset;

                var actualSize = (property.GetCustomAttribute<SizeAttribute>()?.Size).GetValueOrDefault(0);
                property.SetValue(instance, Deserialize(data, property.PropertyType, count));

                if (actualSize > 0) data.BaseStream.Position = pos + actualSize;
                else if (offset.HasValue) data.BaseStream.Position = pos;
            }
            return instance;
        }

        private static (PropertyInfo Property, string Kind, object OffsetReference, object CountReference, object ConditionReference)[] GetProperties(Type T)
        {
            if (!PropertyMap.TryGetValue(T, out var tupleArray))
            {
                if (T.GetCustomAttribute<DataContractAttribute>() == null)
                {
                    throw new InvalidDataException($"{T.FullName} does not have DataContractAttribute, please add data contract attributes or add a custom serializer");
                }

                var temp = GetAllProperties(T)
                            .Where(x => x.GetCustomAttribute<DataMemberAttribute>() != null && x.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
                            .Select((property) =>
                            {
                                object descr = property.GetCustomAttribute<DescriptionAttribute>()?.Description;
                                if (descr != null && ((string)descr).StartsWith("0x") && int.TryParse((string)descr, NumberStyles.HexNumber, null, out var tempDescr))
                                {
                                    descr = tempDescr;
                                }
                                object cate = property.GetCustomAttribute<CategoryAttribute>()?.Category;
                                if (cate != null && ((string)cate).StartsWith("0x") && int.TryParse((string)cate, NumberStyles.HexNumber, null, out var tempCate))
                                {
                                    cate = tempCate;
                                }
                                return (property, property.Name, descr, cate, (object)property.GetCustomAttribute<ConditionalAttribute>()?.Condition);
                            });
                tupleArray = temp.Select((tuple) =>
                {
                    var (property, kind, offset, count, condition) = tuple;
                    if (offset is string offsetRef) offset = temp.FirstOrDefault(x => x.Name == offsetRef).property;
                    if (count is string countRef) count = temp.FirstOrDefault(x => x.Name == countRef).property;
                    if (condition is string conditionRef) condition = temp.FirstOrDefault(x => x.Name == conditionRef).property;
                    return (property, kind, offset, count, condition);
                }).ToArray();
                PropertyMap[T] = tupleArray;
            }
            return tupleArray;
        }

        private static List<PropertyInfo> GetAllProperties(Type T)
        {
            var properties = new List<PropertyInfo>();
            var queue = new Queue<Type>();
            var done = new HashSet<Type>();
            queue.Enqueue(T);
            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                if (!done.Add(t)) continue;
                if (t.BaseType.FullName != "System.Object") queue.Enqueue(t.BaseType);
                properties.InsertRange(0, t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            }
            return properties;
        }

        // If the length of bytes is zero, the string is null.
        // If the length is above zero (positive), the string is UTF8.
        // If the length is below zero (negative), the string is UTF16LE.
        public static string FString(BinaryReader reader)
        {
            var bytes = reader.ReadInt32();
            if (bytes == 0) return string.Empty;
            else if (bytes > 0) return Encoding.UTF8.GetString(reader.ReadBytes(bytes), 0, bytes - 1);
            else return Encoding.Unicode.GetString(reader.ReadBytes((0 - bytes) * 2), 0, (0 - bytes) * 2 - 2);
        }

        public static Array FArray(BinaryReader reader, Type T)
        {
            var count = reader.ReadInt32();
            if (count == 0) return Array.CreateInstance(T, 0);

            return FArray(reader, count, T);
        }

        private static Array FArray(BinaryReader reader, int count, Type T)
        {
            var array = Array.CreateInstance(T, count);
            for (int i = 0; i < count; ++i)
            {
                array.SetValue(Deserialize(reader, T), i);
            }
            return array;
        }
        public static Guid FGuid(BinaryReader r)
        {
            return new Guid(r.ReadBytes(16));
        }
    }
}
