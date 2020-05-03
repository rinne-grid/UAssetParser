using System;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public class StringTable : UnrealObject
    {
        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
