using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;

namespace UObject.Properties
{
    [PublicAPI]
    public class MapProperty : AbstractProperty
    {
        public Name KeyType { get; set; } = new Name();
        public Name ValueType { get; set; } = new Name();

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public int Reserved { get; set; }
        public Dictionary<string, object?> Value { get; set; } = new Dictionary<string, object?>();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            Logger.Assert(isArray == false, "isArray == false");
            base.Deserialize(buffer, asset, ref cursor, isArray);
            KeyType.Deserialize(buffer, asset, ref cursor);
            ValueType.Deserialize(buffer, asset, ref cursor);
            Guid.Deserialize(buffer, asset, ref cursor);
            Reserved = SpanHelper.ReadLittleInt(buffer, ref cursor);
            Logger.Assert(Reserved == 0, "Reserved == 0");
            var count = SpanHelper.ReadLittleInt(buffer, ref cursor);
            for (var i = 0; i < count; ++i)
            {
                var key = ObjectSerializer.DeserializeProperty(buffer, asset, Tag ?? new PropertyTag(), KeyType, cursor, ref cursor, true);
                var value = ObjectSerializer.DeserializeProperty(buffer, asset, Tag ?? new PropertyTag(), ValueType, cursor, ref cursor, true);
                Value[key.ToString() ?? $"{cursor:X}"] = value;
            }
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            Logger.Assert(isArray == false, "isArray == false");
            base.Serialize(ref buffer, asset, ref cursor, isArray);
            KeyType.Serialize(ref buffer, asset, ref cursor);
            ValueType.Serialize(ref buffer, asset, ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
            SpanHelper.WriteLittleInt(ref buffer, Reserved, ref cursor);
        }
    }
}
