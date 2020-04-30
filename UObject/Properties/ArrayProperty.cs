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
    public class ArrayProperty : AbstractProperty
    {
        [JsonIgnore]
        public Name ArrayType { get; set; } = new Name();

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public List<object?> Values { get; set; } = new List<object?>();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignore)
        {
            base.Deserialize(buffer, asset, ref cursor, ignore);
            ArrayType.Deserialize(buffer, asset, ref cursor);
            Guid.Deserialize(buffer, asset, ref cursor);
            var count = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (ArrayType == "StructProperty")
            {
                var structProperty = ObjectSerializer.DeserializeProperty(buffer, asset, Tag ?? new PropertyTag(), ArrayType, cursor, ref cursor, true) as StructProperty;
                for (var i = 0; i < count; ++i) Values.Add(ObjectSerializer.DeserializeStruct(buffer, asset, structProperty?.StructName ?? "None", ref cursor));
            }
            else
            {
                for (var i = 0; i < count; ++i) Values.Add(ObjectSerializer.DeserializeProperty(buffer, asset, Tag ?? new PropertyTag(), ArrayType, cursor, ref cursor, true));
            }
        }
    }
}
