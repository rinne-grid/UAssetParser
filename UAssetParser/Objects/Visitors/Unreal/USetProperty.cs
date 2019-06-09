using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("SetProperty"), Category("Unreal")]
    public class USetProperty : FPropertyTag
    {
        [DataMember]
        public FName ArrayType { get; set; }

        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public int Zero { get; set; }

        [DataMember]
        public int Count { get; set; }

        [IgnoreDataMember]
        public List<object> Entries { get; set; }
        public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
        {
            var instance = LSerializer.Deserialize<USetProperty>(reader);
            instance.Ref(summary);
            var @base = reader.BaseStream.Position;
            instance.Entries = UArrayProperty.GetEntries(instance.Count, instance.ArrayType, reader, summary);
            reader.BaseStream.Position = @base + instance.Size - 8;
            return instance;
        }

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            ArrayType.Ref(summary);
        }

        public override object GetValue()
        {
            return Entries.Select(UArrayProperty.Unwrap).ToArray();
        }
    }
}
