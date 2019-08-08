using System.Diagnostics;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
    [DataContract]
    [DebuggerDisplay("[FName {Name} ({Index})]")]
    public class FName
    {
        [DataMember]
        public int Index { get; set; }
        
        [DataMember]
        public int ExIndex { get; set; }

        [IgnoreDataMember]
        public string Name { get; set; }

        public string Ref(FNameEntry[] names)
        {
            if (Name != null) return Name;
            if (names.Length < Index || Index < 0) return null;
            Name = names[Index].Name;
            return Name;
        }

        public static implicit operator string(FName @this)
        {
            return @this.Name;
        }

        public void Ref(FPackageFileSummary summary)
        {
            Ref(summary.Names);
        }
    }
}
