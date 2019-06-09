using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("Vector"), Category("Unreal")]
    public class SVector : IStructObject
    {
        [DataMember]
        public float X { get; set; }

        [DataMember]
        public float Y { get; set; }

        [DataMember]
        public float Z { get; set; }

        public object Serialize()
        {
            return this;
        }
    }
}
