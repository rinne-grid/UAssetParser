using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class Name : IObjectProperty
    {
        public int Index { get; set; }
        public int ExIndex { get; set; }
        public string Value { get; set; }

        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public static implicit operator string(Name name) => name.Value;
    }
}
