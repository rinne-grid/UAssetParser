using System;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class StructProperty : ISerializableObject
    {
        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
