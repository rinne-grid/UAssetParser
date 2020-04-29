using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class NameEntry : IObjectProperty
    {
        public string Name { get; set; }
        public ushort NonCasePreservingHash { get; set; }
        public ushort CasePreservingHash { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            Name = ObjectSerializer.DeserializeString(buffer, ref cursor);
            NonCasePreservingHash = SpanHelper.ReadLittleUShort(buffer, ref cursor);
            CasePreservingHash = SpanHelper.ReadLittleUShort(buffer, ref cursor);
        }

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            ObjectSerializer.SerializeString(buffer, Name, ref cursor);
            SpanHelper.WriteLittleUShort(buffer, NonCasePreservingHash, ref cursor);
            SpanHelper.WriteLittleUShort(buffer, CasePreservingHash, ref cursor);
        }
    }
}
