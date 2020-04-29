using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Properties
{
    [PublicAPI]
    public class ArrayProperty : AbstractProperty
    {
        [JsonIgnore]
        public Name ArrayType { get; set; }

        [JsonIgnore]
        public PropertyGuid Guid { get; set; }

        public List<object> Values { get; set; } = new List<object>();

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool ignore)
        {
            base.Deserialize(buffer, asset, ref cursor, ignore);
            ArrayType = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
            Guid = ObjectSerializer.DeserializeProperty<PropertyGuid>(buffer, asset, ref cursor);
            var count = SpanHelper.ReadLittleInt(buffer, ref cursor);
            for (var i = 0; i < count; ++i) Values.Add(ObjectSerializer.DeserializeProperty(buffer, asset, Tag, ArrayType, cursor, ref cursor, true));
        }
    }
}
