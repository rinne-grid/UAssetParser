using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class IntProperty : AbstractGuidProperty
    {
        public int Value { get; set; }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            SpanHelper.WriteLittleInt(ref buffer, Value, ref cursor);
        }

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag)
        {
            base.Deserialize(buffer, asset, ref cursor, ignoreTag);
            Value = SpanHelper.ReadLittleInt(buffer, ref cursor);
        }
    }
}
