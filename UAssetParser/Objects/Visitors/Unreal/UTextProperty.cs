using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("TextProperty"), Category("Unreal")]
    public class UTextProperty : FPropertyTag
    {
        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public long Unknown { get; set; }

        [DataMember]
        public FPropertyGuid Key { get; set; }

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public string Value { get; set; }

        public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
        {
            if (baseTag.Size < 8)
            {
                reader.BaseStream.Position += 0x19 + baseTag.Size;
                return baseTag;
            }
            var instance = LSerializer.Deserialize<UTextProperty>(reader);
            instance.Ref(summary);
            return instance;
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}
