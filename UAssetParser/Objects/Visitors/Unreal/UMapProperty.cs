using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("MapProperty"), Category("Unreal")]
    public class UMapProperty : FPropertyTag
    {
        [DataMember]
        public FName KeyType { get; set; }

        [DataMember]
        public FName ValueType { get; set; }

        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public int Zero { get; set; }

        [DataMember]
        public int Count { get; set; }

        [IgnoreDataMember]
        public List<(object, object)> Entries { get; set; } = new List<(object, object)>();

        public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
        {
            var instance = LSerializer.Deserialize<UMapProperty>(reader);
            instance.Ref(summary);
            for (int i = 0; i < instance.Count; ++i)
            {
                instance.Entries.Add((VisitorFactory.VisitEnumerable(reader, instance.KeyType, summary, 1), VisitorFactory.VisitEnumerable(reader, instance.ValueType, summary, 1)));
            }
            return instance;
        }

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            KeyType.Ref(summary);
            ValueType.Ref(summary);
        }

        public override object GetValue()
        {
            return Entries.Select(x => new KeyValuePair<object, object>(UArrayProperty.Unwrap(x.Item1), UArrayProperty.Unwrap(x.Item2)));
        }
    }
}
