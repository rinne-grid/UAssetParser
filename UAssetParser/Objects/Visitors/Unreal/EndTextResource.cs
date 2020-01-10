using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UAssetParser.Formats;
using UAssetParser.Formats.FF7R;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [Description("EndTextResource"), Category("Unreal")]
    public class EndTextResource : Dictionary<string, FF7TxtRes>, IExportObject
    {
        public static IExportObject Deserialize(BinaryReader reader, FObjectExport export, FPackageFileSummary summary)
        {
            var instance = new EndTextResource();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                var key = LSerializer.FString(reader);
                if (string.IsNullOrEmpty(key) || key[0] != '$')
                    throw new InvalidDataException("The key does not start with magic symbol");
                var obj = LSerializer.Deserialize<FF7TxtRes>(reader);
                obj.GetEntries(reader, summary);
                instance.Add(key, obj);
            }
            return instance;
        }

        public object Serialize()
        {
            return this.Select(x => new KeyValuePair<string, object>(x.Key, x.Value.GetValue()));
        }
    }
}
