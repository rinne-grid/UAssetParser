using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
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

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
