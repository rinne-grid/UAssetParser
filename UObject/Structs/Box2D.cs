using JetBrains.Annotations;

namespace UObject.Structs
{
    [PublicAPI]
    public struct Box2D
    {
        public Vector2D Min { get; set; }
        public Vector2D Max { get; set; }
        public bool IsValid { get; set; }
    }
}
