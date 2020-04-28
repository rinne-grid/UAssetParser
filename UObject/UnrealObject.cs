using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Properties;

namespace UObject
{
    [PublicAPI]
    public class UnrealObject : Dictionary<string, IObjectProperty>, IObjectProperty
    {
        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
