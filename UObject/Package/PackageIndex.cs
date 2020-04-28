using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class PackageIndex : IObjectProperty
    {
        public int Index { get; set; }
        public object ObjectResource { get; set; }
        public bool IsImport => Index < 0;
        public bool IsExport => Index > 0;
        public bool IsNull => Index == 0;

        public string Name => ObjectResource switch
        {
            ObjectExport export => export.ObjectName,
            ObjectImport import => import.ObjectName,
            _ => null
        };

        public string FullName => ObjectResource switch
        {
            ObjectExport export => export.ObjectName,
            ObjectImport import => import.PackageRef.IsNull ? import.ObjectName : import.PackageRef.Name,
            _ => null
        };

        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
