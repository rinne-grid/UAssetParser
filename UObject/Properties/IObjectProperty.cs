using System;
using JetBrains.Annotations;

namespace UObject.Properties
{
    [PublicAPI]
    public interface IObjectProperty
    {
        int Deserialize(Span<byte> buffer);
        int Serialize(Span<byte> buffer);
    }
}
