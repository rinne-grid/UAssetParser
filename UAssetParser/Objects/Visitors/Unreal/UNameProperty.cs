using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("NameProperty"), Category("Unreal")]
    public class UNameProperty : FPropertyTag
    {
        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public FName Value { get; set; }

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            Value.Ref(summary);
        }

        public override object GetValue()
        {
            return Value.Name;
        }
    }
}
