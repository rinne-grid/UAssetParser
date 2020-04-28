using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class PackageFileSummary : IObjectProperty
    {
        public int Tag { get; set; }
        public int FileVersionUE4 { get; set; }
        public int FileVersionLicenseeUE4 { get; set; }
        public int FileVersionUE3 { get; set; }
        public int FileVersionLicenseeUE3 { get; set; }
        public CustomVersion[] CustomVersion { get; set; }
        public int TotalHeaderSize { get; set; }
        public string FolderName { get; set; }
        public uint PackageFlags { get; set; }
        public int NameCount { get; set; }
        public int NameOffset { get; set; }
        public int GatherableNameCount { get; set; }
        public int GatherableNameOffset { get; set; }
        public int ExportCount { get; set; }
        public int ExportOffset { get; set; }
        public int ImportCount { get; set; }
        public int ImportOffset { get; set; }
        public int DependsOffset { get; set; }
        public int SoftPackageReferencesCount { get; set; }
        public int SoftPackageReferencesOffset { get; set; }
        public int SearchableNamesOffset { get; set; }
        public int ThumbnailTableOffset { get; set; }
        public Guid Guid { get; set; }
        public GenerationInfo[] Generations { get; set; }
        public EngineVersion SavedByEngineVersion { get; set; }
        public EngineVersion CompatibleWithEngineVersion { get; set; }
        public uint CompressionFlags { get; set; }
        public uint PackageSource { get; set; }
        public int Unversioned { get; set; }
        public int AssetRegistryDataOffset { get; set; }
        public long BulkDataStartOffset { get; set; }
        public int WorldTileInfoDataOffset { get; set; }
        public int[] ChunkIDs { get; set; }
        public int PreloadDependencyCount { get; set; }
        public int PreloadDependencyOffset { get; set; }
        
        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
