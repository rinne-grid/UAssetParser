using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("Rotator"), Category("Unreal")]
    public class SRotator : IStructObject
    {
        [DataMember]
        public float Pitch { get; set; }

        [DataMember]
        public float Yaw { get; set; }

        [DataMember]
        public float Roll { get; set; }

        public object Serialize()
        {
            return this;
        }
    }
}
