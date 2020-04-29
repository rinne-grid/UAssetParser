using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.ObjectModel;
using UObject.Properties;
using UObject.Structs;

namespace UObject
{
    [PublicAPI]
    public static class ObjectSerializer
    {
        public static Dictionary<string, Type> PropertyTypes { get; } = new Dictionary<string, Type>
        {
            { nameof(ObjectProperty), typeof(ObjectProperty) },
            { nameof(StructProperty), typeof(StructProperty) }
        };

        public static Dictionary<string, Type> StructTypes { get; } = new Dictionary<string, Type>
        {
            { nameof(Box), typeof(Box) },
            { nameof(Box2D), typeof(Box2D) },
            { nameof(Color), typeof(Color) },
            { nameof(IntPoint), typeof(IntPoint) },
            { nameof(LinearColor), typeof(LinearColor) },
            { nameof(Rotator), typeof(Rotator) },
            { nameof(Vector), typeof(Vector) },
            { nameof(Vector2D), typeof(Vector2D) }
        };

        public static Dictionary<string, Type> ClassTypes { get; } = new Dictionary<string, Type>
        {
            { nameof(DataTable), typeof(DataTable) },
            { nameof(StringTable), typeof(StringTable) }
        };

        public static AssetFile Deserialize(Span<byte> uasset, Span<byte> uexp) => new AssetFile(uasset, uexp);

        public static Span<byte> SerializeExports(ref PackageFileSummary summary, List<UnrealObject> uexp) => throw new NotImplementedException();

        public static Span<byte> SerializeSummary(PackageFileSummary summary, List<UnrealObject> uasset) => throw new NotImplementedException();

        public static string DeserializeString(Span<byte> buffer, ref int cursor)
        {
            var count = SpanHelper.ReadLittleInt(buffer, ref cursor);
            var str = default(string);
            if (count > 0)
            {
                str = count == 1 ? string.Empty : Encoding.UTF8.GetString(buffer.Slice(cursor, count - 1));
                cursor += count;
            }
            else if (count < 0)
            {
                str = count == -1 ? string.Empty : Encoding.Unicode.GetString(buffer.Slice(cursor, (0 - count) * 2 - 2));
                cursor += count * 2;
            }

            return str;
        }

        public static T DeserializeProperty<T>(Span<byte> buffer, AssetFile asset, ref int cursor) where T : ISerializableObject, new()
        {
            var instance = new T();
            instance.Deserialize(buffer, asset, ref cursor);
            return instance;
        }

        public static AbstractProperty DeserializeProperty(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            var start = cursor;
            var tag = DeserializeProperty<PropertyTag>(buffer, asset, ref cursor);
            var tmp = cursor;
            try
            {
                cursor = start;
                AbstractProperty instance;
                if (!PropertyTypes.TryGetValue(tag.Type, out var propertyType))
                {
                    Logger.Warn("UObject", $"No Handler for property {tag.Name.Value} which has the type {tag.Type.Value}");
                    instance = new AbstractProperty();
                }
                else
                {
                    if (!(Activator.CreateInstance(propertyType) is AbstractProperty instanceWrap)) throw new InvalidDataException();
                    instance = instanceWrap;
                }

                instance.Deserialize(buffer, asset, ref cursor);
                return instance;
            }
            finally
            {
                cursor = tmp + tag.Size;
            }
        }

        public static T[] DeserializeProperties<T>(Span<byte> buffer, AssetFile asset, int count, ref int cursor) where T : ISerializableObject, new()
        {
            var instances = AllocateProperties<T>(count);
            DeserializeProperties(buffer, asset, instances, ref cursor);
            return instances;
        }

        public static T[] AllocateProperties<T>(int count) where T : ISerializableObject, new()
        {
            var instances = new T[count];
            for (var i = 0; i < count; ++i) instances[i] = new T();
            return instances;
        }

        public static void DeserializeProperties<T>(Span<byte> buffer, AssetFile asset, T[] instances, ref int cursor) where T : ISerializableObject, new()
        {
            foreach (var instance in instances) instance.Deserialize(buffer, asset, ref cursor);
        }

        public static void SerializeProperty<T>(ref Memory<byte> buffer, AssetFile asset, T instance, ref int cursor) where T : ISerializableObject, new() => instance.Serialize(ref buffer, asset, ref cursor);

        public static void SerializeProperties<T>(ref Memory<byte> buffer, AssetFile asset, T[] instances, ref int cursor) where T : ISerializableObject, new()
        {
            foreach (var instance in instances) instance.Serialize(ref buffer, asset, ref cursor);
        }

        public static void SerializeString(ref Memory<byte> buffer, string text, ref int cursor)
        {
            if (text == null) return;
            if (text == string.Empty)
            {
                SpanHelper.WriteLittleInt(ref buffer, 1, ref cursor);
                cursor += 1;
            }

            var length = text.Length + 1;
            var utf16 = false;
            var bufferLength = Encoding.UTF8.GetByteCount(text) + 1;
            if (bufferLength != length)
            {
                utf16 = true;
                bufferLength = Encoding.Unicode.GetByteCount(text) + 2;
            }

            var span = new Span<char>(text.ToCharArray());
            SpanHelper.WriteLittleInt(ref buffer, utf16 ? 0 - length : length, ref cursor);
            SpanHelper.EnsureSpace(ref buffer, cursor + bufferLength);
            if (utf16) Encoding.Unicode.GetBytes(span, buffer.Slice(cursor).Span);
            else Encoding.UTF8.GetBytes(span, buffer.Slice(cursor).Span);
            cursor += bufferLength;
        }

        public static ISerializableObject DeserializeObject(AssetFile asset, ObjectExport export, Span<byte> uasset, Span<byte> uexp)
        {
            var blob = uexp.Length > 0 ? uexp.Slice((int) (export.SerialOffset - asset.Summary.TotalHeaderSize), (int) export.SerialSize) : uasset.Slice((int) export.SerialOffset, (int) export.SerialSize);

            if (!ClassTypes.TryGetValue(export.ClassIndex.Name, out var classType)) throw new NotImplementedException(export.ClassIndex.Name);

            if (!(Activator.CreateInstance(classType) is ISerializableObject instance)) throw new NotImplementedException(export.ClassIndex.Name);

            var cursor = 0;
            instance.Deserialize(blob, asset, ref cursor);
            return instance;
        }
    }
}
