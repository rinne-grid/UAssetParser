using System;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;

namespace UObject.Properties
{
    [PublicAPI]
    public class SoftObjectProperty : AbstractGuidProperty
    {
        public Name Package { get; set; } = new Name();

        public string Path { get; set; } = "None";

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Deserialize(buffer, asset, ref cursor, isArray);
            Package.Deserialize(buffer, asset, ref cursor);
            Path = ObjectSerializer.DeserializeString(buffer, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Serialize(ref buffer, asset, ref cursor, isArray);
            Package.Serialize(ref buffer, asset, ref cursor);
            ObjectSerializer.SerializeString(ref buffer, Path, ref cursor);
        }
    }
}
