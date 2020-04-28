using System;
using System.Buffers.Binary;
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

        public static Span<byte> SerializeString(string text)
        {
            if (text == null) return new Span<byte>(new byte[4]);
            if (text == string.Empty)
            {
                var empty = new Span<byte>(new byte[5]);
                BinaryPrimitives.WriteInt32LittleEndian(empty, 1);
                return empty;
            }

            var length = text.Length + 1;
            var utf16 = false;
            var bufferLength = Encoding.UTF8.GetByteCount(text);
            if (bufferLength + 1 != length)
            {
                utf16 = true;
                bufferLength = Encoding.Unicode.GetByteCount(text);
            }

            var span = new Span<char>(text.ToCharArray());
            var blob = new Span<byte>(new byte[4 + bufferLength + 1]);
            BinaryPrimitives.WriteInt32LittleEndian(blob, utf16 ? 0 - length : length);
            if (utf16) Encoding.Unicode.GetBytes(span, blob.Slice(4));
            else Encoding.UTF8.GetBytes(span, blob.Slice(4));
            return blob;
        }
    }
}
