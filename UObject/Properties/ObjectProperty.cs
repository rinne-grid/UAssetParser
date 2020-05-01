using System;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Enum;
using UObject.JSON;

namespace UObject.Properties
{
    [PublicAPI]
    public class ObjectProperty : AbstractGuidProperty, IValueType<PackageIndex>
    {
        public PackageIndex Value { get; set; } = new PackageIndex();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Deserialize(buffer, asset, ref cursor, mode);
            Value.Deserialize(buffer, asset, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Serialize(ref buffer, asset, ref cursor, mode);
            Value.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
