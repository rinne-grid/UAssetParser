using System;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.Asset
{
    [PublicAPI]
    public class CustomVersion : IObjectProperty
    {
        public Guid Key { get; set; }
        public int Version { get; set; }

        public void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            if (asset.Summary.LegacyFileVersion < -2 && asset.Summary.LegacyFileVersion >= -5) Key = SpanHelper.ReadStruct<Guid>(buffer, ref cursor);

            Version = SpanHelper.ReadLittleInt(buffer, ref cursor);
        }

        public void Serialize(Span<byte> buffer, AssetFile asset, ref int cursor)
        {
            if (asset.Summary.LegacyFileVersion < -2 && asset.Summary.LegacyFileVersion >= -5) SpanHelper.WriteStruct(buffer, Key, ref cursor);

            SpanHelper.WriteLittleInt(buffer, Version, ref cursor);
        }
    }
}
