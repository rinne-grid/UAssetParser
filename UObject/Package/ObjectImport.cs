using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class ObjectImport : IObjectProperty
    {
        public Name ClassPackage { get; set; }
        public Name ClassName { get; set; }
        public PackageIndex PackageRef { get; set; }
        public Name ObjectName { get; set; }
        
        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
