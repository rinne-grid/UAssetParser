using System;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Enum;

namespace UObject.Generics
{
    [PublicAPI]
    public interface IPropertyObject : ISerializableObject
    {
        void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode);
    }
}
