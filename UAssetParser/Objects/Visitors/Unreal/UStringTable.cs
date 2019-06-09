using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
    [Description("StringTable"), Category("Unreal")]
    public class StringTable : Dictionary<string, string>, IExportObject
    {
        public string Name { get; set; }

        public static IExportObject Deserialize(BinaryReader reader, FObjectExport export, FPackageFileSummary summary)
        {
            var instance = new StringTable();
            instance.Name = LSerializer.FString(reader);
            var count = reader.ReadInt32();
            for(int i = 0; i < count; ++i)
            {
                instance.Add(LSerializer.FString(reader), LSerializer.FString(reader));
            }
            return instance;
        }

        public object Serialize()
        {
            return new { Name, Table = this };
        }
    }
}
