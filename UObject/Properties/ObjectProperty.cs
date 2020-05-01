using System;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Enum;

namespace UObject.Properties
{
    [PublicAPI]
    public class ObjectProperty : AbstractGuidProperty
    {
        public PackageIndex PackageIndex { get; set; } = new PackageIndex();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Deserialize(buffer, asset, ref cursor, mode);
            PackageIndex.Deserialize(buffer, asset, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Serialize(ref buffer, asset, ref cursor, mode);
            PackageIndex.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
