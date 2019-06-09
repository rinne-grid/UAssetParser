using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("StrProperty"), Category("Unreal")]
    public class UStrProperty : FPropertyTag
    {
        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public string Value { get; set; }

        public override object GetValue()
        {
            return Value;
        }
    }
}
