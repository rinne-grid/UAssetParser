using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Enum;
using UObject.Generics;
using UObject.JSON;

namespace UObject.Properties
{
    [PublicAPI]
    public class ArrayProperty : AbstractProperty, IArrayValueType<List<object?>>
    {
        public Name ArrayType { get; set; } = new Name();

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public List<object?> Value { get; set; } = new List<object?>();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            Logger.Assert(mode == SerializationMode.Normal, "mode == SerializationMode.Normal");
            base.Deserialize(buffer, asset, ref cursor, mode);
            ArrayType.Deserialize(buffer, asset, ref cursor);
            Guid.Deserialize(buffer, asset, ref cursor);
            var count = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (ArrayType == "StructProperty")
            {
                var structTag = new PropertyTag();
                structTag.Deserialize(buffer, asset, ref cursor);
                var structProperty = ObjectSerializer.DeserializeProperty(buffer, asset, structTag, ArrayType, cursor, ref cursor, SerializationMode.Array) as StructProperty;
                for (var i = 0; i < count; ++i) Value.Add(ObjectSerializer.DeserializeStruct(buffer, asset, structProperty?.StructName ?? "None", ref cursor));
            }
            else
            {
                var arrayMode = SerializationMode.Array;
                if (ArrayType == "ByteProperty" && Tag?.Size > 0 && Tag?.Size / count != 1) arrayMode &= SerializationMode.ByteAsEnum;
                for (var i = 0; i < count; ++i) Value.Add(ObjectSerializer.DeserializeProperty(buffer, asset, Tag ?? new PropertyTag(), ArrayType, cursor, ref cursor, arrayMode));
            }
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, SerializationMode mode)
        {
            Logger.Assert(mode == SerializationMode.Normal, "mode == SerializationMode.Normal");
            base.Serialize(ref buffer, asset, ref cursor, mode);
            ArrayType.Serialize(ref buffer, asset, ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
            SpanHelper.WriteLittleInt(ref buffer, Value.Count, ref cursor);
            // TODO: Serialize struct data
        }
    }
}
