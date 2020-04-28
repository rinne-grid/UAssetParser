using JetBrains.Annotations;

namespace UObject.Asset
{
    [PublicAPI]
    public struct GenerationInfo
    {
        public int ExportCount { get; set; }
        public int NameCount { get; set; }
    }
}
