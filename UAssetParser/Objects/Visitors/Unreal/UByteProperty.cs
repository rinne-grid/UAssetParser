using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("ByteProperty"), Category("Unreal")]
    public class UByteProperty : FPropertyTag
    {
        [DataMember]
        public FName EnumName { get; set; }

        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [IgnoreDataMember]
        public object Value { get; set; }


        public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
        {
            var instance = LSerializer.Deserialize<UByteProperty>(reader);
            instance.Ref(summary);
            if(instance.EnumName == "None")
            {
                instance.Value = reader.ReadByte();
            }
            else
            {
                instance.Value = LSerializer.Deserialize<FName>(reader);
                instance.Ref(summary);
            }
            return instance;
        }

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            EnumName.Ref(summary);
            if (Value is FName fn) fn.Ref(summary);
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}
