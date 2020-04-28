using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class AssetFile
    {
        public AssetFile(Span<byte> uasset, Span<byte> uexp, int unrealVersion = 524)
        {
            var cursor = 0;
            Summary = new PackageFileSummary();
            Summary.Deserialize(uasset, this, unrealVersion, ref cursor);
        }

        public AssetFile() { }

        public PackageFileSummary Summary { get; set; }
        public NameEntry[] Names { get; set; }
        public ObjectImport[] Imports { get; set; }
        public ObjectExport[] Exports { get; set; }
        public PreloadDependencyIndex[] PreloadDependencies { get; set; }

        public Dictionary<string, IObjectProperty> ExportObjects { get; set; } = new Dictionary<string, IObjectProperty>();
    }
}
