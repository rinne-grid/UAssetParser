using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Package;
using UObject.Properties;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public class StringTable : Dictionary<string, object>, IObjectProperty
    {
        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
