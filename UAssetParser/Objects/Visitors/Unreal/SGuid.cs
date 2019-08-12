using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("Guid"), Category("Unreal")]
    public class SGuid : IStructObject
    {
        [DataMember]
        public Guid Fguid { get; set; }

        public object Serialize()
        {
            return this;
        }
    }
}
