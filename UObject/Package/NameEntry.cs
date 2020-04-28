using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class NameEntry : IObjectProperty
    {
        public string Name { get; set; }
        public ushort NonCasePreservingHash { get; set; }
        public ushort CasePreservingHash { get; set; }

        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
