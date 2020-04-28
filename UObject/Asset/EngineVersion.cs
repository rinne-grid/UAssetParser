using JetBrains.Annotations;

namespace UObject.Asset
{
    [PublicAPI]
    public struct EngineVersion
    {
        public uint Major { get; set; }
        public uint Minor { get; set; }
        public uint Patch { get; set; }
        public uint Changeset { get; set; }
    }
}
