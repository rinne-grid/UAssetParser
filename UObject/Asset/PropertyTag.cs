using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class PropertyTag : IObjectProperty
    {
        public Name Name { get; set; }
        public Name Type { get; set; }
        public int Size { get; set; }
        public int Index { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
