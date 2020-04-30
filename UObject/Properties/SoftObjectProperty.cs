using System;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;

namespace UObject.Properties
{
    [PublicAPI]
    public class SoftObjectProperty : AbstractProperty
    {
        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public Name Package { get; set; } = new Name();

        public string Path { get; set; } = "None";

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag)
        {
            base.Deserialize(buffer, asset, ref cursor, ignoreTag);
            Guid.Deserialize(buffer, asset, ref cursor);
            Package.Deserialize(buffer, asset, ref cursor);
            Path = ObjectSerializer.DeserializeString(buffer, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
            Package.Serialize(ref buffer, asset, ref cursor);
            ObjectSerializer.SerializeString(ref buffer, Path, ref cursor);
        }
    }
}
