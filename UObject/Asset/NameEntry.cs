using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class NameEntry : IObjectProperty
    {
        public string Name { get; set; }
        public ushort NonCasePreservingHash { get; set; }
        public ushort CasePreservingHash { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
