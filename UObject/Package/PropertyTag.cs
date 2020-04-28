using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class PropertyTag : IObjectProperty
    {
        public Name Name { get; set; }
        public Name Type { get; set; }
        public int Size { get; set; }
        public int Index { get; set; }
        
        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
