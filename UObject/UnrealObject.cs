using System;
using System.Collections.Generic;
using UObject.Properties;

namespace UObject
{
    public class UnrealObject : Dictionary<string, IObjectProperty>, IObjectProperty
    {
        public int Deserialize(Span<byte> buffer) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer) => throw new NotImplementedException();
    }
}
