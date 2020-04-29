using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class ObjectImport : ISerializableObject
    {
        public Name ClassPackage { get; set; }
        public Name ClassName { get; set; }
        public PackageIndex PackageRef { get; set; }
        public Name ObjectName { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            ClassPackage = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
            ClassName = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
            PackageRef = ObjectSerializer.DeserializeProperty<PackageIndex>(buffer, asset, ref cursor);
            ObjectName = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
        }

        public void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            ClassPackage.Serialize(ref buffer, asset, ref cursor);
            ClassName.Serialize(ref buffer, asset, ref cursor);
            PackageRef.Serialize(ref buffer, asset, ref cursor);
            ObjectName.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
