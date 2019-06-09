using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("SoftObjectProperty"), Category("Unreal")]
    public class USoftObjectProperty : FPropertyTag
    {
        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public FName Package { get; set; }

        [DataMember]
        public string Path { get; set; }

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            Package.Ref(summary);
        }

        public override object GetValue()
        {
            return (new string[] { Package.Name, Path }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}
