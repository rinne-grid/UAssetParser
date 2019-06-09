using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UAssetParser.Formats;
using UAssetParser.Objects.Visitors;

namespace UAssetParser.Objects
{
    public class UObject : List<FPropertyTag>
    {
        public IExportObject ObjectData { get; set; }

        public FName Name { get; set; }

#if DEBUG
        private List<long> CachedPos { get; set; } = new List<long>();
#endif

        public UObject(BinaryReader data, FPackageFileSummary summary, bool pad = true, FObjectExport export = null)
        {
            while (true)
            {
                var start = data.BaseStream.Position;
#if DEBUG
                CachedPos.Add(start);
#endif

                var name = LSerializer.Deserialize<FName>(data);
                name.Ref(summary);
                if (name == "None") break;
                data.BaseStream.Position = start;

                var tag = LSerializer.Deserialize<FPropertyTag>(data);
                data.BaseStream.Position = start;
                tag.Ref(summary);

                tag = VisitorFactory.Visit(data, tag, summary);
                tag.Ref(summary);
                Add(tag);
            }

            if (pad) data.BaseStream.Position += 4;
            if (export != null) ObjectData = VisitorFactory.VisitSubtype(data, export, summary);
        }

        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();
            foreach (var tag in this)
            {
                dict[tag.Name] = tag.GetValue();
            }
            if (ObjectData != null) dict["ExportData"] = ObjectData.Serialize();
            if (Name != null) dict["UObjectId"] = Name.Name;
            return dict;
        }
    }
}
