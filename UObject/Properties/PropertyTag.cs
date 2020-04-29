using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class PropertyTag : ISerializableObject
    {
        public Name Name { get; set; }
        public Name Type { get; set; }
        public int Size { get; set; }
        public int Index { get; set; }

        public virtual void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            Name = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
            Type = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
            Size = SpanHelper.ReadLittleInt(buffer, ref cursor);
            Index = SpanHelper.ReadLittleInt(buffer, ref cursor);
        }

        public virtual void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
