using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Enum;
using UObject.Generics;
using UObject.JSON;

namespace UObject.Properties
{
    [PublicAPI]
    public class TextProperty : AbstractGuidProperty, IValueType<string>
    {
        public int ReservedHashInt { get; set; }
        public PropertyGuid HashGuid { get; set; } = new PropertyGuid();
        public int ReservedValueInt { get; set; }
        public PropertyGuid ValueGuid { get; set; } = new PropertyGuid();
        public string Hash { get; set; } = "None";
        public string Value { get; set; } = "None";

        public override string ToString() => Value;

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Deserialize(buffer, asset, ref cursor, mode);
            ReservedHashInt = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ReservedValueInt = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (ReservedHashInt != 0) HashGuid.Deserialize(buffer, asset, ref cursor);

            ValueGuid.Deserialize(buffer, asset, ref cursor);

            if (ReservedValueInt != 0xFF)
            {
                Hash = ObjectSerializer.DeserializeString(buffer, ref cursor);
                Value = ObjectSerializer.DeserializeString(buffer, ref cursor);
            }
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            base.Serialize(ref buffer, asset, ref cursor, mode);
            SpanHelper.WriteLittleInt(ref buffer, ReservedHashInt, ref cursor);
            SpanHelper.WriteLittleInt(ref buffer, ReservedValueInt, ref cursor);
            if (ReservedHashInt != 0) HashGuid.Serialize(ref buffer, asset, ref cursor);
            ValueGuid.Serialize(ref buffer, asset, ref cursor);

            if (ReservedValueInt != 0xFF)
            {
                ObjectSerializer.SerializeString(ref buffer, Hash, ref cursor);
                ObjectSerializer.SerializeString(ref buffer, Value, ref cursor);
            }
        }
    }
}
