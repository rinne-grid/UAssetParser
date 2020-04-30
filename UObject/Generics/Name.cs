using System;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;

namespace UObject.Generics
{
    [PublicAPI]
    public class Name : ISerializableObject
    {
        [JsonIgnore]
        public int Index { get; set; }

        [JsonIgnore]
        public int ExIndex { get; set; }

        public string Value { get; set; } = "None";

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            Index = SpanHelper.ReadLittleInt(buffer, ref cursor);
            ExIndex = SpanHelper.ReadLittleInt(buffer, ref cursor);
            if (asset.Names.Length < Index || Index < 0) return;
            Value = asset.Names[Index].Name;
        }

        public void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            SpanHelper.WriteLittleInt(ref buffer, Index, ref cursor);
            SpanHelper.WriteLittleInt(ref buffer, ExIndex, ref cursor);
            // TODO: Recalculate Index.
        }

        public static implicit operator string(Name? name) => name?.Value ?? "None";
    }
}
