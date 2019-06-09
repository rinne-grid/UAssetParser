using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("Box2D"), Category("Unreal")]
    public class SBox2D : IStructObject
    {
        [DataMember]
        public SVector2D X { get; set; }

        [DataMember]
        public SVector2D Y { get; set; }

        [DataMember, Size(1)]
        public bool IsValid { get; set; }

        public object Serialize()
        {
            return this;
        }
    }
}
