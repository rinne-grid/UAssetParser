using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public class DataTable : Dictionary<string, object>, IObjectProperty
    {
        public int Deserialize(Span<byte> buffer) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer) => throw new NotImplementedException();
    }
}
