using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Properties;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public class UnrealObject : ISerializableObject
    {
        public Dictionary<string, object> Entries { get; set; } = new Dictionary<string, object>();

        public virtual void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            while (true)
            {
                var tmp = cursor;
                var key = ObjectSerializer.DeserializeProperty<Name>(buffer, asset, ref cursor);
                if (key == "None") break;
                // rollback
                cursor = tmp;
                var property = ObjectSerializer.DeserializeProperty(buffer, asset, ref cursor);
                Entries[property.Tag.Name] = property;
            }
        }

        public virtual void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor) => throw new NotImplementedException();
    }
}
