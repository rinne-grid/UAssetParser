using System;
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

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            SpanHelper.WriteByte(ref buffer, (byte) (Value ? 1 : 0), ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
        }

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag)
        {
            base.Deserialize(buffer, asset, ref cursor, ignoreTag);
            Value = SpanHelper.ReadByte(buffer, ref cursor) == 1;
            Guid.Deserialize(buffer, asset, ref cursor);
        }
    }
}
