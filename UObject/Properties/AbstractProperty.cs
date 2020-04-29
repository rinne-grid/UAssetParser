using System;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class AbstractProperty : ISerializableObject
    {
        public PropertyTag Tag { get; set; }

        public virtual void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => Tag = ObjectSerializer.DeserializeProperty<PropertyTag>(buffer, asset, ref cursor);

        public virtual void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => Tag.Serialize(ref buffer, asset, ref cursor);
    }
}
