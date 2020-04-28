using JetBrains.Annotations;

namespace UObject.Structs
{
    [PublicAPI]
    public struct Rotator
    {
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }
    }
}
