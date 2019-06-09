using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [DataContract, Description("ArrayProperty"), Category("Unreal")]
    public class UArrayProperty : FPropertyTag
    {
        [DataMember]
        public FName ArrayType { get; set; }

        [DataMember]
        public FPropertyGuid Guid { get; set; }

        [DataMember]
        public int ArraySize { get; set; }

        [IgnoreDataMember]
        public List<object> Entries { get; set; } = new List<object>();

        public override void Ref(FPackageFileSummary summary)
        {
            base.Ref(summary);
            ArrayType.Ref(summary);
        }

        public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
        {
            var instance = LSerializer.Deserialize<UArrayProperty>(reader);
            instance.Ref(summary);

            var @base = reader.BaseStream.Position;
            instance.Entries = GetEntries(instance.ArraySize, instance.ArrayType, reader, summary);
            reader.BaseStream.Position = @base + instance.Size - 4;
            return instance;
        }

        public static List<object> GetEntries(int arraySize, FName arrayType, BinaryReader reader, FPackageFileSummary summary)
        {
            var entries = new List<object>();
            if (arrayType == "StructProperty")
            {
                entries.Add(VisitorFactory.VisitEnumerable(reader, arrayType, summary, arraySize));
            }
            else
            {
                for (int i = 0; i < arraySize; ++i)
                {
                    entries.Add(VisitorFactory.VisitEnumerable(reader, arrayType, summary, 1));
                }
            }
            return entries;
        }

        public static object Unwrap(object x)
        {
            if (x is object[] array) return array.Select(Unwrap).ToArray();
            else if (x is UObject @object) return @object.ToDictionary();
            else if (x is IStructObject @struct) return @struct.Serialize();
            else if (x is FName name) return name.Name;
            else if (x is FPropertyTag tag) return tag.GetValue();
            else if (x is Tuple<FPropertyTag, FPropertyTag> tuple) return new object[] { tuple.Item1.GetValue(), tuple.Item2.GetValue() };
            return x;
        }

        public override object GetValue()
        {
            return Entries.Select(Unwrap).ToArray();
        }
    }
}
