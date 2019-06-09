using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UAssetParser.Enums;
using UAssetParser.Objects;

namespace UAssetParser.Formats
{
    [DataContract]
    [DebuggerDisplay("[FObjectExport {ObjectName}]")]
    public class FObjectExport
    {
        [DataMember]
        public FPackageIndex ClassIndex { get; set; }

        [DataMember]
        public FPackageIndex SuperIndex { get; set; }

        [DataMember]
        public FPackageIndex TemplateIndex { get; set; }

        [DataMember]
        public FPackageIndex OuterIndex { get; set; }

        [DataMember]
        public FName ObjectName { get; set; }

        [DataMember]
        public EObjectFlags ObjectFlags { get; set; }

        [DataMember]
        public long SerialSize { get; set; }

        [DataMember]
        public long SerialOffset { get; set; }

        [DataMember]
        public bool ForcedExport { get; set; }

        [DataMember]
        public bool NotForClient { get; set; }

        [DataMember]
        public bool NotForServer { get; set; }

        [DataMember]
        public Guid PackageGuid { get; set; }

        [DataMember]
        public uint PackageFlags { get; set; }

        [DataMember]
        public bool NotAlwaysLoadedForEditorGame { get; set; }

        [DataMember]
        public bool IsAsset { get; set; }

        [DataMember]
        public int FirstExportDependency { get; set; }

        [DataMember]
        public bool SerializationBeforeSerializationDependencies { get; set; }

        [DataMember]
        public bool CreateBeforeSerializationDependencies { get; set; }

        [DataMember]
        public bool SerializationBeforeCreateDependencies { get; set; }

        [DataMember]
        public bool CreateBeforeCreateDependencies { get; set; }

        [DataMember]
        public EDynamicType DynamicType { get; set; }

        [IgnoreDataMember]
        public List<UObject> Objects { get; set; } = new List<UObject>();

        public void Ref(FPackageFileSummary summary)
        {
            ClassIndex.Ref(summary);
            TemplateIndex.Ref(summary);
            OuterIndex.Ref(summary);
            SuperIndex.Ref(summary);
            ObjectName.Ref(summary);
        }
    }
}
