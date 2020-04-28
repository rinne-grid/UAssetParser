using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Package;
using UObject.Properties;

namespace UObject
{
    [PublicAPI]
    public class UnrealObject : Dictionary<string, IObjectProperty>, IObjectProperty
    {
        public int Deserialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer, AssetFile asset) => throw new NotImplementedException();
    }
}
