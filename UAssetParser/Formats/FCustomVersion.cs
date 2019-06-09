using System;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
    [DataContract]
    public class FCustomVersion
    {
        [DataMember]
        public Guid Key { get; set; }
    }
}
