using System;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class AbstractGuidProperty : AbstractProperty
    {
        [JsonIgnore]
        public PropertyGuid Guid => ((PropertyGuidTag) Tag)?.Guid ?? new PropertyGuid();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => Tag = ObjectSerializer.DeserializeProperty<PropertyGuidTag>(buffer, asset, ref cursor);

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => Tag.Serialize(ref buffer, asset, ref cursor);
    }
}
