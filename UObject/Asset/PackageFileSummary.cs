using System;
using System.Linq;
using DragonLib.IO;
using JetBrains.Annotations;

namespace UObject.Asset
{
    [PublicAPI]
    public class PackageFileSummary
    {
        public uint Tag { get; set; }
        public int LegacyFileVersion { get; set; }
        public int LegacyUE3Version { get; set; }
        public int FileVersionUE4 { get; set; }
        public int FileVersionLicenseeUE4 { get; set; }
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
        public int EngineChangeList { get; set; }
        public EngineVersion CompatibleWithEngineVersion { get; set; }
        public CompressedChunk[] CompressedChunks { get; set; }
        public uint PackageSource { get; set; }
        public string[] AdditionalPackagesToCook { get; set; }
        public int NumTextureAllocations { get; set; }
        public int AssetRegistryDataOffset { get; set; }
        public long BulkDataStartOffset { get; set; }
        public int WorldTileInfoDataOffset { get; set; }
        public int[] ChunkIDs { get; set; }
        public int PreloadDependencyCount { get; set; }
        public int PreloadDependencyOffset { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, int fileVersionOverride, ref int cursor)
        {
            Tag = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            Logger.Assert(Tag == 0x9E2A83C1, "Tag == 0x9E2A83C1", "Tag does not match expected asset magic tag", $"Got {Tag:X8} instead!");
            LegacyFileVersion = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (LegacyFileVersion != -4) LegacyUE3Version = SpanHelper.ReadLittleInt(buffer, ref cursor);
            FileVersionUE4 = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (FileVersionUE4 < 214) FileVersionUE4 = fileVersionOverride;
            FileVersionLicenseeUE4 = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (LegacyFileVersion <= -2)
            {
                var customVersionCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
                CustomVersion = new CustomVersion[customVersionCount];
                for (var i = 0; i < customVersionCount; ++i)
                {
                    CustomVersion[i] = new CustomVersion();
                    CustomVersion[i].Deserialize(buffer, asset, ref cursor);
                }
            }

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
            if (FileVersionUE4 >= 337)
                SavedByEngineVersion = SpanHelper.ReadStruct<EngineVersion>(buffer, ref cursor);
            else
                EngineChangeList = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (FileVersionUE4 >= 448)
                CompatibleWithEngineVersion = SpanHelper.ReadStruct<EngineVersion>(buffer, ref cursor);
            var compressedChunkCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            CompressedChunks = SpanHelper.ReadStructArray<CompressedChunk>(buffer, compressedChunkCount, ref cursor);
            PackageSource = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            var additionalPackagesToCookCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
            AdditionalPackagesToCook = new string[additionalPackagesToCookCount];
            for (var i = 0; i < additionalPackagesToCookCount; ++i) AdditionalPackagesToCook[i] = ObjectSerializer.DeserializeString(buffer, ref cursor);
            if (LegacyFileVersion > -7) NumTextureAllocations = SpanHelper.ReadLittleInt(buffer, ref cursor);
            AssetRegistryDataOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            BulkDataStartOffset = SpanHelper.ReadLittleLong(buffer, ref cursor);
            if (FileVersionUE4 >= 224)
                WorldTileInfoDataOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (FileVersionUE4 >= 327)
            {
                var chunkIdCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
                ChunkIDs = SpanHelper.ReadStructArray<int>(buffer, chunkIdCount, ref cursor);
            }
            else if (FileVersionUE4 >= 279)
            {
                ChunkIDs = new[] { SpanHelper.ReadLittleInt(buffer, ref cursor) };
            }

            if (FileVersionUE4 >= 511)
            {
                PreloadDependencyCount = SpanHelper.ReadLittleInt(buffer, ref cursor);
                PreloadDependencyOffset = SpanHelper.ReadLittleInt(buffer, ref cursor);
            }
        }

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            SpanHelper.WriteLittleUInt(buffer, Tag, ref cursor);
            SpanHelper.WriteLittleInt(buffer, LegacyFileVersion, ref cursor);
            if (LegacyFileVersion != -4) SpanHelper.WriteLittleInt(buffer, LegacyUE3Version, ref cursor);
            SpanHelper.WriteLittleInt(buffer, FileVersionUE4, ref cursor);
            SpanHelper.WriteLittleInt(buffer, FileVersionLicenseeUE4, ref cursor);
            if (LegacyFileVersion <= -2)
            {
                SpanHelper.WriteLittleInt(buffer, CustomVersion.Length, ref cursor);
                foreach (var customVersion in CustomVersion) customVersion.Serialize(buffer, asset, ref cursor);
            }

            SpanHelper.WriteLittleInt(buffer, TotalHeaderSize, ref cursor);
            ObjectSerializer.SerializeString(buffer, FolderName, ref cursor);
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
            if (FileVersionUE4 >= 337)
                SpanHelper.WriteStruct(buffer, SavedByEngineVersion, ref cursor);
            else
                SpanHelper.WriteLittleInt(buffer, EngineChangeList, ref cursor);
            if (FileVersionUE4 >= 448)
                SpanHelper.WriteStruct(buffer, CompatibleWithEngineVersion, ref cursor);
            SpanHelper.WriteLittleInt(buffer, CompressedChunks.Length, ref cursor);
            SpanHelper.WriteStructArray(buffer, CompressedChunks, ref cursor);
            SpanHelper.WriteLittleUInt(buffer, PackageSource, ref cursor);
            SpanHelper.WriteLittleInt(buffer, AdditionalPackagesToCook.Length, ref cursor);
            foreach (var additionalPackage in AdditionalPackagesToCook) ObjectSerializer.SerializeString(buffer, additionalPackage, ref cursor);
            if (LegacyFileVersion > -7) SpanHelper.WriteLittleInt(buffer, NumTextureAllocations, ref cursor);

            SpanHelper.WriteLittleInt(buffer, AssetRegistryDataOffset, ref cursor);
            SpanHelper.WriteLittleLong(buffer, BulkDataStartOffset, ref cursor);
            if (FileVersionUE4 >= 224)
                SpanHelper.WriteLittleInt(buffer, WorldTileInfoDataOffset, ref cursor);
            if (FileVersionUE4 >= 327)
            {
                SpanHelper.WriteLittleInt(buffer, ChunkIDs.Length, ref cursor);
                SpanHelper.WriteStructArray(buffer, ChunkIDs, ref cursor);
            }
            else if (FileVersionUE4 >= 279)
            {
                SpanHelper.WriteLittleInt(buffer, ChunkIDs.FirstOrDefault(), ref cursor);
            }

            if (FileVersionUE4 >= 511)
            {
                SpanHelper.WriteLittleInt(buffer, PreloadDependencyCount, ref cursor);
                SpanHelper.WriteLittleInt(buffer, PreloadDependencyOffset, ref cursor);
            }
        }
    }
}
