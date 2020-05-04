using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;
using UObject.Properties;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public class UnrealObject : ISerializableObject
    {
        public Dictionary<string, AbstractProperty> Value { get; set; } = new Dictionary<string, AbstractProperty>();

        public virtual void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            while (true)
            {
                var tmp = cursor;
                var key = new Name();
                key.Deserialize(buffer, asset, ref cursor);
                if (key == "None") break;
                // rollback
                cursor = tmp;
                var property = ObjectSerializer.DeserializeProperty(buffer, asset, ref cursor);
                Value[property.Tag?.Name.Value ?? $"{cursor:X}"] = property;
            }
        }

        public virtual void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
