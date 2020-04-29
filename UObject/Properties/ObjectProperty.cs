using System;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class ObjectProperty : AbstractProperty
    {
        public PackageIndex PackageIndex { get; set; }

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Deserialize(buffer, asset, ref cursor);
            PackageIndex = ObjectSerializer.DeserializeProperty<PackageIndex>(buffer, asset, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            PackageIndex.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
