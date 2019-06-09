using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("ObjectProperty"), Category("Unreal")]
    public class UObjectProperty : FPropertyTag
    {
        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public FPackageIndex Package { get; set; }

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            Package.Ref(summary);
        }

        public override object GetValue()
        {
            return Package.FullName;
        }
    }
}
