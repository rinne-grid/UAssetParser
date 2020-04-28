using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class PackageFileSummary : IObjectProperty
    {
        public uint Tag { get; set; }
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

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            // ReSharper disable once UselessBinaryOperation
            Tag = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            Logger.Assert(Tag == 0x9E2A83C1, "Tag == 0x9E2A83C1", "Tag does not match expected asset magic tag", $"Got {Tag:X8} instead!");
            FileVersionUE4 = SpanHelper.ReadLittleInt(buffer, ref cursor);
            FileVersionLicenseeUE4 = SpanHelper.ReadLittleInt(buffer, ref cursor);
            FileVersionUE3 = SpanHelper.ReadLittleInt(buffer, ref cursor);
            FileVersionLicenseeUE3 = SpanHelper.ReadLittleInt(buffer, ref cursor);
            var customVersionCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            CustomVersion = SpanHelper.ReadStructArray<CustomVersion>(buffer, customVersionCount, ref cursor);
            TotalHeaderSize = SpanHelper.ReadLittleInt(buffer, ref cursor);
            FolderName = ObjectSerializer.DeserializeString(buffer, ref cursor);
            PackageFlags = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            NameCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            NameOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            GatherableNameCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            GatherableNameOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ExportCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ExportOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ImportCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ImportOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            DependsOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            SoftPackageReferencesCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            SoftPackageReferencesOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            SearchableNamesOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ThumbnailTableOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            Guid = SpanHelper.ReadStruct<Guid>(buffer, ref cursor);
            var generationCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            Generations = SpanHelper.ReadStructArray<GenerationInfo>(buffer, generationCount, ref cursor);
            SavedByEngineVersion = SpanHelper.ReadStruct<EngineVersion>(buffer, ref cursor);
            CompatibleWithEngineVersion = SpanHelper.ReadStruct<EngineVersion>(buffer, ref cursor);
            CompressionFlags = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            PackageSource = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            Unversioned = SpanHelper.ReadLittleInt(buffer, ref cursor);
            AssetRegistryDataOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            BulkDataStartOffset = SpanHelper.ReadLittleLong(buffer, ref cursor);
            WorldTileInfoDataOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            var chunkIdCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ChunkIDs = SpanHelper.ReadStructArray<int>(buffer, chunkIdCount, ref cursor);
            PreloadDependencyCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            PreloadDependencyOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
        }

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            SpanHelper.WriteLittleUInt(buffer, Tag, ref cursor);
            SpanHelper.WriteLittleInt(buffer, FileVersionUE4, ref cursor);
            SpanHelper.WriteLittleInt(buffer, FileVersionLicenseeUE4, ref cursor);
            SpanHelper.WriteLittleInt(buffer, FileVersionUE3, ref cursor);
            SpanHelper.WriteLittleInt(buffer, FileVersionLicenseeUE3, ref cursor);
            SpanHelper.WriteLittleInt(buffer, CustomVersion.Length, ref cursor);
            SpanHelper.WriteStructArray(buffer, CustomVersion, ref cursor);
            SpanHelper.WriteLittleInt(buffer, TotalHeaderSize, ref cursor);
            var folderName = ObjectSerializer.SerializeString(FolderName);
            folderName.CopyTo(buffer.Slice(cursor));
            cursor += folderName.Length;
            SpanHelper.WriteLittleUInt(buffer, PackageFlags, ref cursor);
            SpanHelper.WriteLittleInt(buffer, NameCount, ref cursor);
            SpanHelper.WriteLittleInt(buffer, NameOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, GatherableNameCount, ref cursor);
            SpanHelper.WriteLittleInt(buffer, GatherableNameOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, ExportCount, ref cursor);
            SpanHelper.WriteLittleInt(buffer, ExportOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, ImportCount, ref cursor);
            SpanHelper.WriteLittleInt(buffer, ImportOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, DependsOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, SoftPackageReferencesCount, ref cursor);
            SpanHelper.WriteLittleInt(buffer, SoftPackageReferencesOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, SearchableNamesOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, ThumbnailTableOffset, ref cursor);
            SpanHelper.WriteStruct(buffer, Guid, ref cursor);
            SpanHelper.WriteLittleInt(buffer, Generations.Length, ref cursor);
            SpanHelper.WriteStructArray(buffer, Generations, ref cursor);
            SpanHelper.WriteStruct(buffer, SavedByEngineVersion, ref cursor);
            SpanHelper.WriteStruct(buffer, CompatibleWithEngineVersion, ref cursor);
            SpanHelper.WriteLittleUInt(buffer, CompressionFlags, ref cursor);
            SpanHelper.WriteLittleUInt(buffer, PackageSource, ref cursor);
            SpanHelper.WriteLittleInt(buffer, Unversioned, ref cursor);
            SpanHelper.WriteLittleInt(buffer, AssetRegistryDataOffset, ref cursor);
            SpanHelper.WriteLittleLong(buffer, BulkDataStartOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, WorldTileInfoDataOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, ChunkIDs.Length, ref cursor);
            SpanHelper.WriteStructArray(buffer, ChunkIDs, ref cursor);
            SpanHelper.WriteLittleInt(buffer, PreloadDependencyCount, ref cursor);
            SpanHelper.WriteLittleInt(buffer, PreloadDependencyOffset, ref cursor);
        }
    }
}
