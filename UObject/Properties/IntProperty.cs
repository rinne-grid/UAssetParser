using System;
using System.Globalization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class IntProperty : AbstractGuidProperty
    {
        public int Value { get; set; }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Deserialize(buffer, asset, ref cursor, isArray);
            Value = SpanHelper.ReadLittleInt(buffer, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Serialize(ref buffer, asset, ref cursor, isArray);
            SpanHelper.WriteLittleInt(ref buffer, Value, ref cursor);
        }
    }
}
