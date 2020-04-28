using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class ObjectImport : IObjectProperty
    {
        public Name ClassPackage { get; set; }
        public Name ClassName { get; set; }
        public PackageIndex PackageRef { get; set; }
        public Name ObjectName { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
