using System;
using JetBrains.Annotations;

namespace UObject.Enum
{
    [PublicAPI]
    [Flags]
    public enum SerializationMode
    {
        Normal = 0,
        Array = 1 << 0,
        Map = 1 << 1,
        ByteAsEnum = 1 << 2
    }
}
