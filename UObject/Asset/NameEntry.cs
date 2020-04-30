using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Generics;

namespace UObject.Asset
{
    [PublicAPI]
    public class NameEntry : ISerializableObject
    {
        public string Name { get; set; } = "None";
        public ushort NonCasePreservingHash { get; set; }
        public ushort CasePreservingHash { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            Name = ObjectSerializer.DeserializeString(buffer, ref cursor);
            NonCasePreservingHash = SpanHelper.ReadLittleUShort(buffer, ref cursor);
            CasePreservingHash = SpanHelper.ReadLittleUShort(buffer, ref cursor);
        }

        public void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor)
        {
            ObjectSerializer.SerializeString(ref buffer, Name, ref cursor);
            SpanHelper.WriteLittleUShort(ref buffer, NonCasePreservingHash, ref cursor);
            SpanHelper.WriteLittleUShort(ref buffer, CasePreservingHash, ref cursor);
        }
    }
}
