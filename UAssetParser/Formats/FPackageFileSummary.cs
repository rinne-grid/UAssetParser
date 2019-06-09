using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
    [DataContract]
    [DebuggerDisplay("[FPackageFileSummary {FolderName}]")]
    public class FPackageFileSummary
    {
        [DataMember]
        public int Tag { get; set; }

        [DataMember]
        public int FileVersionUE4 { get; set; }

        [DataMember]
        public int FileVersionLicenseeUE4 { get; set; }

        [DataMember]
        public int FileVersionUE3 { get; set; }

        [DataMember]
        public int FileVersionLicenseeUE3 { get; set; }

        [DataMember]
        public FCustomVersion[] CustomVersion { get; set; }

        [DataMember]
        public int TotalHeaderSize { get; set; }

        [DataMember]
        public string FolderName { get; set; }

        // this is read AFTER FolderName
        [DataMember]
        public uint PackageFlags { get; set; }

        [DataMember]
        public int NameCount { get; set; }

        [DataMember]
        public int NameOffset { get; set; }

        // this isn't actually read
        // [DataMember]
        // public string LocalizationId { get; set; }

        [DataMember]
        public int GatherableNameCount { get; set; }

        [DataMember]
        public int GatherableNameOffset { get; set; }

        [DataMember]
        public int ExportCount { get; set; }

        [DataMember]
        public int ExportOffset { get; set; }

        [DataMember]
        public int ImportCount { get; set; }

        [DataMember]
        public int ImportOffset { get; set; }

        [DataMember]
        public int DependsOffset { get; set; }

        [DataMember]
        public int SoftPackageReferencesCount { get; set; }

        [DataMember]
        public int SoftPackageReferencesOffset { get; set; }

        [DataMember]
        public int SearchableNamesOffset { get; set; }

        [DataMember]
        public int ThumbnailTableOffset { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public FGenerationInfo[] Generations { get; set; }

        [DataMember]
        public FEngineVersion SavedByEngineVersion { get; set; }

        [DataMember]
        public FEngineVersion CompatibleWithEngineVersion { get; set; }

        [DataMember]
        public uint CompressionFlags { get; set; }

        [DataMember]
        public uint PackageSource { get; set; }

        [DataMember]
        public bool Unversioned { get; set; } // actually an int, whatever.

        [DataMember]
        public int AssetRegistryDataOffset { get; set; }

        [DataMember]
        public long BulkDataStartOffset { get; set; }

        [DataMember]
        public int WorldTileInfoDataOffset { get; set; }

        [DataMember]
        public int[] ChunkIDs { get; set; }

        [DataMember]
        public int PreloadDependencyCount { get; set; }

        [DataMember]
        public int PreloadDependencyOffset { get; set; }

        // Beyond this point is dynamic.

        [DataMember, Description(nameof(NameOffset)), Category(nameof(NameCount))]
        public FNameEntry[] Names { get; set; }

        [DataMember, Description(nameof(ImportOffset)), Category(nameof(ImportCount))]
        public FObjectImport[] Imports { get; set; }

        [DataMember, Description(nameof(ExportOffset)), Category(nameof(ExportCount))]
        public FObjectExport[] Exports { get; set; }
    }
}
