using System;
using System.Globalization;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Enum;
using UObject.Generics;
using UObject.JSON;

namespace UObject.Properties
{
    [PublicAPI]
    public class UInt32Property : AbstractProperty, IValueType<uint>
    {
        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public uint Value { get; set; }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Deserialize(buffer, asset, ref cursor, mode);
            Value = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            if (mode == SerializationMode.Normal) Guid.Deserialize(buffer, asset, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Serialize(ref buffer, asset, ref cursor, mode);
            SpanHelper.WriteLittleUInt(ref buffer, Value, ref cursor);
            if (mode == SerializationMode.Normal) Guid.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
