using System;
using JetBrains.Annotations;

namespace UObject.Package
{
    [PublicAPI]
    public class AssetFile
    {
        public AssetFile(Span<byte> uasset, Span<byte> uexp)
        {
        }

        public PackageFileSummary Summary { get; set; }
        public NameEntry[] Names { get; set; }
        public ObjectImport[] Imports { get; set; }
        public ObjectExport[] Exports { get; set; }
        public PreloadDependencyIndex[] PreloadDependencies { get; set; }
    }
}
