using System;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Generics
{
    [PublicAPI]
    public interface ISerializableObject
    {
        void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor);
        void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor);
    }

    [PublicAPI]
    public interface IPropertyObject : ISerializableObject
    {
        void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag);
    }
}
