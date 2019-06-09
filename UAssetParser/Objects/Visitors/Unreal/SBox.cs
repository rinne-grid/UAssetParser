using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("Box"), Category("Unreal")]
    public class SBox : IStructObject
    {
        [DataMember]
        public SVector Min { get; set; }

        [DataMember]
        public SVector Max { get; set; }

        [DataMember, Size(1)]
        public bool IsValid { get; set; }

        public object Serialize()
        {
            return this;
        }
    }
}
