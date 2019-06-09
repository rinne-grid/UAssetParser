using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("StructProperty"), Category("Unreal")]
    public class UStructProperty : FPropertyTag
    {
        [DataMember]
        public FName StructName { get; set; }

        [DataMember]
        public Guid StructGuid { get; set; }

        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [IgnoreDataMember]
        public object Struct { get; set; }

        public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
        {
            var instance = LSerializer.Deserialize<UStructProperty>(reader);
            instance.Ref(summary);
            instance.Struct = (object)VisitorFactory.VisitStruct(reader, instance.StructName, summary) ?? new UObject(reader, summary, false, null);
            return instance;
        }
        public override object GetValue()
        {
            return UArrayProperty.Unwrap(Struct);
        }

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            StructName.Ref(summary);
        }
    }
}
