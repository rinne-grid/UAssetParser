using System;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Package
{
    [PublicAPI]
    public class PropertyGuid : IObjectProperty
    {
        public bool HasGuid { get; set; }

        public Guid Guid { get; set; } = Guid.Empty;

        public int Deserialize(Span<byte> buffer, AssetFile asset)
        {
            HasGuid = buffer[0] == 1;
            if (HasGuid) Guid = new Guid(buffer.Slice(1));
            else return 1;
            return 16 + 1;
        }

        public int Serialize(Span<byte> buffer, AssetFile asset)
        {
            buffer[0] = (byte) (Guid == Guid.Empty ? 0 : 1);
            if (Guid == Guid.Empty) return 1;
            Guid.TryWriteBytes(buffer.Slice(1));
            return 16 + 1;
        }
    }
}
