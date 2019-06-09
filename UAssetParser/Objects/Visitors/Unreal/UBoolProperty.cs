using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("BoolProperty"), Category("Unreal")]
    public class UBoolProperty : FPropertyTag
    {
        [DataMember, Size(1)]
        public bool Value { get; set; }

        [DataMember]
        public FPropertyGuid Guid { get; set; }

        public override object GetValue()
        {
            return Value;
        }
    }
}
