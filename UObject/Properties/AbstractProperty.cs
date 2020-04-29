using System;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class AbstractProperty : IPropertyObject
    {
        [JsonIgnore]
        public virtual PropertyTag Tag { get; set; }

        public virtual void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => Tag = ObjectSerializer.DeserializeProperty<PropertyTag>(buffer, asset, ref cursor);

        public virtual void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => Tag.Serialize(ref buffer, asset, ref cursor);

        public virtual void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag)
        {
            if (ignoreTag) return;
            Deserialize(buffer, asset, ref cursor);
        }
    }
}
