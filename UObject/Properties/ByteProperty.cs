using System;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;

namespace UObject.Properties
{
    [PublicAPI]
    public class ByteProperty : AbstractProperty
    {
        public Name EnumName { get; set; } = new Name();

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public object Value { get; set; }

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignoreTag)
        {
            base.Deserialize(buffer, asset, ref cursor, ignoreTag);
            if (!ignoreTag)
            {
                EnumName.Deserialize(buffer, asset, ref cursor);
                Guid.Deserialize(buffer, asset, ref cursor);
                if(EnumName.Value == "None")
                {
                    Value = SpanHelper.ReadByte(buffer, ref cursor);
                }
                else
                {
                    Value = new Name();
                    (Value as Name).Deserialize(buffer, asset, ref cursor);
                }
            }
            else
            {
                throw new NotImplementedException($"No array support for {nameof(ByteProperty)}");
            }
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            EnumName.Serialize(ref buffer, asset, ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
            if (Value is Name fname)
                fname.Serialize(ref buffer, asset, ref cursor);
            else
                SpanHelper.WriteByte(ref buffer, (byte) Value, ref cursor);
        }
    }
}
