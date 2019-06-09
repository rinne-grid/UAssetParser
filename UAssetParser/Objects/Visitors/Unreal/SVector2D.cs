using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("Vector2D"), Category("Unreal")]
    public class SVector2D : IStructObject
    {
        [DataMember]
        public float X { get; set; }

        [DataMember]
        public float Y { get; set; }

        public object Serialize()
        {
            return this;
        }
    }
}
