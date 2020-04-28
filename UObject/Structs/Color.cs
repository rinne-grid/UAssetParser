using JetBrains.Annotations;

namespace UObject.Structs
{
    [PublicAPI]
    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }
}
