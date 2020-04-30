using System;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;

namespace UObject.Properties
{
    [PublicAPI]
    public class UInt32Property : AbstractProperty
    {
        public uint Value { get; set; }

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            SpanHelper.WriteLittleUInt(ref buffer, Value, ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
        }

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag)
        {
            base.Deserialize(buffer, asset, ref cursor, ignoreTag);
            Value = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            Guid.Deserialize(buffer, asset, ref cursor);
        }
    }
}
