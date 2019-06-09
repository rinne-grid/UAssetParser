using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [Description("DataTable"), Category("Unreal")]
    public class UDataTable : Dictionary<FName, UObject>, IExportObject
    {
        public static IExportObject Deserialize(BinaryReader reader, FObjectExport export, FPackageFileSummary summary)
        {
            var instance = new UDataTable();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                var index = LSerializer.Deserialize<FName>(reader);
                index.Ref(summary);
                var uobj = new UObject(reader, summary, false);
                uobj.Name = index;
                instance.Add(index, uobj);
            }
            return instance;
        }

        public object Serialize()
        {
            return this.Select(x => new KeyValuePair<string, object>(x.Key.Name, x.Value.ToDictionary())).ToArray();
        }
    }
}
