using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("FloatProperty"), Category("Unreal")]
    public class UFloatProperty : FPropertyTag
    {
        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public float Value { get; set; }

        public override object GetValue()
        {
            return Value;
        }
    }
}
