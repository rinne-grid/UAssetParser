using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using UAssetParser.Formats;
using UAssetParser.Objects;

namespace UAssetParser
{
    [DataContract]
    public class UAsset
    {
        public UAsset(string path) : this(File.OpenRead(path)) { }

        [DataMember]
        public FPackageFileSummary Summary { get; set; }

        // TODO: Figure out what ubulk does.
        public UAsset(Stream uassetStream, Stream uexpStream = null, Stream ubulkStream = null, bool keepOpen = false)
        {
            using var reader = new BinaryReader(uassetStream, Encoding.UTF8, keepOpen);
            Summary = LSerializer.Deserialize<FPackageFileSummary>(reader);

            foreach (var import in Summary.Imports)
            {
                import.Ref(Summary);
            }

            foreach (var export in Summary.Exports)
            {
                export.Ref(Summary);
                using var ms = GetUEventStream(export, uassetStream, uexpStream);
                using var msr = new BinaryReader(ms, Encoding.UTF8, false);
                export.Objects.Add(new UObject(msr, Summary, true, export));
            }
        }

        private MemoryStream GetUEventStream(FObjectExport export, Stream uasset, Stream uexp)
        {
            var buffer = new Span<byte>(new byte[export.SerialSize]);
            if (uexp != null)
            {
                uexp.Position = export.SerialOffset - Summary.TotalHeaderSize;
                uexp.Read(buffer);
            }
            else
            {
                uasset.Position = export.SerialOffset;
                uasset.Read(buffer);
            }
            return new MemoryStream(buffer.ToArray());
        }
    }
}
