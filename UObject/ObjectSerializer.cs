using System;
using System.Collections.Generic;
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

        public static Dictionary<string, Type> ExportTypes { get; } = new Dictionary<string, Type>
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

        public static T DeserializeProperty<T>(Span<byte> buffer, AssetFile asset, ref int cursor) where T : IObjectProperty, new()
        {
            var instance = new T();
            instance.Deserialize(buffer, asset, ref cursor);
            return instance;
        }

        public static T[] DeserializeProperties<T>(Span<byte> buffer, AssetFile asset, int count, ref int cursor) where T : IObjectProperty, new()
        {
            var instances = AllocateProperties<T>(count);
            DeserializeProperties(buffer, asset, instances, ref cursor);
            return instances;
        }

        public static T[] AllocateProperties<T>(int count) where T : IObjectProperty, new()
        {
            var instances = new T[count];
            for (var i = 0; i < count; ++i)
            {
                instances[i] = new T();
            }
            return instances;
        }

        public static void DeserializeProperties<T>(Span<byte> buffer, AssetFile asset, T[] instances, ref int cursor) where T : IObjectProperty, new()
        {
            foreach (var instance in instances)
            {
                instance.Deserialize(buffer, asset, ref cursor);
            }
        }

        public static void SerializeProperty<T>(Span<byte> buffer, AssetFile asset, T instance, ref int cursor) where T : IObjectProperty, new()
        {
            instance.Serialize(buffer, asset, ref cursor);
        }

        public static void SerializeProperties<T>(Span<byte> buffer, AssetFile asset, T[] instances, ref int cursor) where T : IObjectProperty, new()
        {
            foreach (var instance in instances) instance.Serialize(buffer, asset, ref cursor);
        }

        public static void SerializeString(Span<byte> buffer, string text, ref int cursor)
        {
            if (text == null) return;
            if (text == string.Empty)
            {
                SpanHelper.WriteLittleInt(buffer, 1, ref cursor);
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
            SpanHelper.WriteLittleInt(buffer, utf16 ? 0 - length : length, ref cursor);
            // TODO: SpanHelper.EnsureSpace(buffer, cursor + bufferLength);
            if (utf16) Encoding.Unicode.GetBytes(span, buffer.Slice(cursor));
            else Encoding.UTF8.GetBytes(span, buffer.Slice(cursor));
            cursor += bufferLength;
        }
    }
}
