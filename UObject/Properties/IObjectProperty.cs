using System;
using JetBrains.Annotations;
using UObject.Package;

namespace UObject.Properties
{
    [PublicAPI]
    public interface IObjectProperty
    {
        int Deserialize(Span<byte> buffer, AssetFile asset);
        int Serialize(Span<byte> buffer, AssetFile asset);
    }
}
