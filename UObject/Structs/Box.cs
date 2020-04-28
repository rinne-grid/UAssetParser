using JetBrains.Annotations;

namespace UObject.Structs
{
    [PublicAPI]
    public struct Box
    {
        public Vector Min { get; set; }
        public Vector Max { get; set; }
        public bool IsValid { get; set; }
    }
}
