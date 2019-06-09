using System.Diagnostics;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
    [DataContract]
    [DebuggerDisplay("{Name}")]
    public class FNameEntry
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ushort NonCasePreservingHash { get; set; }

        [DataMember]
        public ushort CasePreservingHash { get; set; }
    }
}
