using System;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.JSON;

namespace UObject.Properties
{
    [PublicAPI]
    public class StrProperty : AbstractGuidProperty, IValueType<string>
    {
        public string Value { get; set; } = "None";

        public override string ToString() => Value;

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Deserialize(buffer, asset, ref cursor, isArray);
            Value = ObjectSerializer.DeserializeString(buffer, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Serialize(ref buffer, asset, ref cursor, isArray);
            ObjectSerializer.SerializeString(ref buffer, Value, ref cursor);
        }
    }
}
