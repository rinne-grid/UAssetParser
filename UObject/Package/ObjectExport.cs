using System;
using JetBrains.Annotations;
using UObject.Enum;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class ObjectExport : IObjectProperty
    {
        public PackageIndex ClassIndex { get; set; }
        public PackageIndex SuperIndex { get; set; }
        public PackageIndex TemplateIndex { get; set; }
        public PackageIndex OuterIndex { get; set; }
        public Name ObjectName { get; set; }
        public ObjectFlags ObjectFlags { get; set; }
        public long SerialSize { get; set; }
        public long SerialOffset { get; set; }
        public bool ForcedExport { get; set; }
        public bool NotForClient { get; set; }
        public bool NotForServer { get; set; }
        public Guid PackageGuid { get; set; }
        public uint PackageFlags { get; set; }
        public bool NotAlwaysLoadedForEditorGame { get; set; }
        public bool IsAsset { get; set; }
        public int FirstExportDependency { get; set; }
        public bool SerializationBeforeSerializationDependencies { get; set; }
        public bool CreateBeforeSerializationDependencies { get; set; }
        public bool SerializationBeforeCreateDependencies { get; set; }
        public bool CreateBeforeCreateDependencies { get; set; }
        public DynamicType DynamicType { get; set; }
        
        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
