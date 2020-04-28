using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class Name : IObjectProperty
    {
        public int Index { get; set; }
        public int ExIndex { get; set; }
        public string Value { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public static implicit operator string(Name name) => name.Value;
    }
}
