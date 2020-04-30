using System;
using System.Globalization;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;

namespace UObject.Properties
{
    [PublicAPI]
    public class BoolProperty : AbstractProperty
    {
        public bool Value { get; set; }

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Deserialize(buffer, asset, ref cursor, isArray);
            Value = SpanHelper.ReadByte(buffer, ref cursor) == 1;
            if (!isArray) Guid.Deserialize(buffer, asset, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Serialize(ref buffer, asset, ref cursor, isArray);
            SpanHelper.WriteByte(ref buffer, (byte) (Value ? 1 : 0), ref cursor);
            if (!isArray) Guid.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
