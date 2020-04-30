using System;
using System.Globalization;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;
using UObject.JSON;

namespace UObject.Properties
{
    [PublicAPI]
    public class FloatProperty : AbstractProperty, IValueType<float>
    {
        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public float Value { get; set; }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Deserialize(buffer, asset, ref cursor, isArray);
            Value = SpanHelper.ReadLittleSingle(buffer, ref cursor);
            if (!isArray) Guid.Deserialize(buffer, asset, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Serialize(ref buffer, asset, ref cursor, isArray);
            SpanHelper.WriteLittleSingle(ref buffer, Value, ref cursor);
            if (!isArray) Guid.Serialize(ref buffer, asset, ref cursor);
        }
    }
}
