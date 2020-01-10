using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace UAssetParser.Formats.FF7R
{
    [DataContract]
    public class FF7TxtRes
    {
        [DataMember]
        public string Str { get; set; }

        [DataMember]
        public int ArrayLength { get; set; }

        [IgnoreDataMember]
        public List<(FName,string)> Entries { get; set; }

        public void GetEntries(BinaryReader reader, FPackageFileSummary summary)
        {
            Entries = new List<(FName, string)>(ArrayLength);
            for (int i = 0; i < ArrayLength; i++)
            {
                var fname = LSerializer.Deserialize<FName>(reader);
                fname.Ref(summary);
                string data = LSerializer.FString(reader);
                Entries.Add((fname,data));
            }
        }

        public object GetValue()
        {
            var q = new Dictionary<string, object>(2);
            q.Add(nameof(Str), Str);
            foreach (var item in Entries)
                q.Add(item.Item1.Name, item.Item2);
            return q;
        }

       
    }
}
