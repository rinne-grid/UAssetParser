using System;
using JetBrains.Annotations;
using UObject.Package;

namespace UObject.Properties
{
    [PublicAPI]
    public class StructProperty : IObjectProperty
    {
        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
