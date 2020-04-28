using System;
using JetBrains.Annotations;

namespace UObject.Properties
{
    [PublicAPI]
    public class StructProperty : IObjectProperty
    {
        public int Deserialize(Span<byte> buffer) => throw new NotImplementedException();

        public int Serialize(Span<byte> buffer) => throw new NotImplementedException();
    }
}
