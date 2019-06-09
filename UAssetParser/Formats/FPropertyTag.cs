using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
    [DataContract]
    [DebuggerDisplay("[FPropertyTag {Name} ({Type})]")]
    public class FPropertyTag
    {
        [DataMember]
        public FName Name { get; set; }

        [DataMember]
        public FName Type { get; set; }

        [DataMember]
        public int Size { get; set; }

        [DataMember]
        public int Index { get; set; }

        public virtual void Ref(FPackageFileSummary summary)
        {
            Name.Ref(summary);
            Type.Ref(summary);
        }

        public virtual object GetValue()
        {
            return null;
        }
    }
}
