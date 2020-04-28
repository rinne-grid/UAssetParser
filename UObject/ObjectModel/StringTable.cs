using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public class StringTable : IObject
    {
        public int Deserialize(Span<byte> buffer) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer) => throw new NotImplementedException();

        public Dictionary<string, object> KeyValues { get; set; }
    }
}
