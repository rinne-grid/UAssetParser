using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Properties;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public class StringTable : Dictionary<string, object>, IObjectProperty
    {
        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
