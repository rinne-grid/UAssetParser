using System;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;

namespace UObject.Properties
{
    [PublicAPI]
    public class EnumProperty : AbstractProperty
    {
        public Name EnumName { get; set; } = new Name();

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public Name Value { get; set; } = new Name();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag)
        {
            base.Deserialize(buffer, asset, ref cursor, ignoreTag);
            if (!ignoreTag)
            {
                EnumName.Deserialize(buffer, asset, ref cursor);
                Guid.Deserialize(buffer, asset, ref cursor);
                Value.Deserialize(buffer, asset, ref cursor);
            }
            else
            {
                Value.Deserialize(buffer, asset, ref cursor);
            }
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            EnumName.Serialize(ref buffer, asset, ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
            Value.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
