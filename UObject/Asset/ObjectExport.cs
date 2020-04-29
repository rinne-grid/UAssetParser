using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Enum;
using UObject.Properties;

namespace UObject.Asset
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

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            ClassIndex = ObjectSerializer.DeserializeProperty<PackageIndex>(buffer, asset, ref cursor);
            SuperIndex = ObjectSerializer.DeserializeProperty<PackageIndex>(buffer, asset, ref cursor);
            TemplateIndex = ObjectSerializer.DeserializeProperty<PackageIndex>(buffer, asset, ref cursor);
            OuterIndex = ObjectSerializer.DeserializeProperty<PackageIndex>(buffer, asset, ref cursor);
            ObjectName = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
            ObjectFlags = (ObjectFlags) SpanHelper.ReadLittleUInt(buffer, ref cursor);
            SerialSize = SpanHelper.ReadLittleLong(buffer, ref cursor);
            SerialOffset = SpanHelper.ReadLittleLong(buffer, ref cursor);
            ForcedExport = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            NotForClient = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            NotForServer = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            PackageGuid = SpanHelper.ReadStruct<Guid>(buffer, ref cursor);
            PackageFlags = SpanHelper.ReadLittleUInt(buffer, ref cursor);
            NotAlwaysLoadedForEditorGame = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            IsAsset = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            FirstExportDependency = SpanHelper.ReadLittleInt(buffer, ref cursor);
            SerializationBeforeSerializationDependencies = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            CreateBeforeSerializationDependencies = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            SerializationBeforeCreateDependencies = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            CreateBeforeCreateDependencies = SpanHelper.ReadLittleInt(buffer, ref cursor) == 1;
            DynamicType = (DynamicType) SpanHelper.ReadLittleUInt(buffer, ref cursor);
        }

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            ClassIndex.Serialize(buffer, asset, ref cursor);
            SuperIndex.Serialize(buffer, asset, ref cursor);
            TemplateIndex.Serialize(buffer, asset, ref cursor);
            OuterIndex.Serialize(buffer, asset, ref cursor);
            ObjectName.Serialize(buffer, asset, ref cursor);
            SpanHelper.WriteLittleUInt(buffer, (uint) ObjectFlags, ref cursor);
            SpanHelper.WriteLittleLong(buffer, SerialSize, ref cursor);
            SpanHelper.WriteLittleLong(buffer, SerialOffset, ref cursor);
            SpanHelper.WriteLittleInt(buffer, ForcedExport ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleInt(buffer, NotForClient ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleInt(buffer, NotForServer ? 1 : 0, ref cursor);
            SpanHelper.WriteStruct(buffer, PackageGuid, ref cursor);
            SpanHelper.WriteLittleUInt(buffer, PackageFlags, ref cursor);
            SpanHelper.WriteLittleInt(buffer, NotAlwaysLoadedForEditorGame ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleInt(buffer, IsAsset ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleInt(buffer, FirstExportDependency, ref cursor);
            SpanHelper.WriteLittleInt(buffer, SerializationBeforeSerializationDependencies ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleInt(buffer, CreateBeforeSerializationDependencies ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleInt(buffer, SerializationBeforeCreateDependencies ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleInt(buffer, CreateBeforeCreateDependencies ? 1 : 0, ref cursor);
            SpanHelper.WriteLittleUInt(buffer, (uint) DynamicType, ref cursor);
        }
    }
}
