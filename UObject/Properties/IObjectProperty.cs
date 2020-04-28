using System;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public interface IObjectProperty
    {
        void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor);
        void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor);
    }
}
