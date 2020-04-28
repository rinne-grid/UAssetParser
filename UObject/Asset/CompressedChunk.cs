using JetBrains.Annotations;

namespace UObject.Asset
{
    [PublicAPI]
    public struct CompressedChunk
    {
        public int UncompressedOffset { get; set; }
        public int UncompressedSize { get; set; }
        public int CompressedOffset { get; set; }
        public int CompressedSize { get; set; }
    }
}
