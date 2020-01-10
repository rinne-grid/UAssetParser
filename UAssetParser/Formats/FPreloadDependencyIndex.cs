using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
    [DataContract]
    public class FPreloadDependencyIndex
    {
        [DataMember]
        public int Index { get; set; }
    }
}