using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UAssetParser.Extensions;
using UAssetParser.Formats;
using UAssetParser.Objects;

namespace UAssetParser
{
	[DataContract]
	public class UAsset
	{
		public static UAParserOptions Options
		{
			get;
			set;
		}

		[DataMember]
		public FPackageFileSummary Summary
		{
			get;
			set;
		}

		public UAsset(string path)
			: this(File.OpenRead(path))
		{
		}

		public UAsset(Stream uassetStream, Stream uexpStream = null, Stream ubulkStream = null, bool keepOpen = false)
		{
			using (BinaryReader data = new BinaryReader(uassetStream, Encoding.UTF8, keepOpen))
			{
				Summary = LSerializer.Deserialize<FPackageFileSummary>(data);
				FObjectImport[] imports = Summary.Imports;
				for (int i = 0; i < imports.Length; i++)
				{
					imports[i].Ref(Summary);
				}
				FObjectExport[] exports = Summary.Exports;
				foreach (FObjectExport fObjectExport in exports)
				{
					fObjectExport.Ref(Summary);
					using (MemoryStream input = GetUEventStream(fObjectExport, uassetStream, uexpStream))
					{
						using (BinaryReader data2 = new BinaryReader(input, Encoding.UTF8, leaveOpen: false))
						{
							fObjectExport.Objects.Add(new UObject(data2, Summary, pad: true, fObjectExport));
						}
					}
				}
			}
		}

		private MemoryStream GetUEventStream(FObjectExport export, Stream uasset, Stream uexp)
		{
			Span<byte> buffer = new Span<byte>(new byte[export.SerialSize]);
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

		public void SerializeToBinary(string filepath)
		{
			byte[] array = SerializeExportData();
			Summary.UpdateMeta(array.Length);
			using (FileStream output = new FileStream(Path.ChangeExtension(filepath, "bin"), FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(output, Encoding.UTF8))
				{
					BSerializer.Serialize<FPackageFileSummary>(binaryWriter, Summary);
					binaryWriter.Write(0);
					binaryWriter.Write(array);
					binaryWriter.Write(Summary.Tag);
					binaryWriter.Flush();
					if (!Options.Verbose)
					{
						return;
					}
					Console.WriteLine($"Total new FNameEntries: {Summary.NewEntries.Count}");
					foreach (string newEntry in Summary.NewEntries)
					{
						Console.WriteLine(newEntry);
					}
					Console.WriteLine("Zero-references new FNames:");
					FNameEntry[] names = Summary.Names;
					foreach (FNameEntry fNameEntry in names)
					{
						if (fNameEntry.RefCount < 1 && fNameEntry.Name != "None")
						{
							Console.WriteLine(fNameEntry.Name);
						}
					}
					Console.WriteLine("Zero-references old FNames:");
					names = Summary.Names;
					foreach (FNameEntry fNameEntry2 in names)
					{
						if (fNameEntry2.RefCountOld < 1 && fNameEntry2.Name != "None")
						{
							Console.WriteLine(fNameEntry2.Name);
						}
					}
				}
			}
		}

		private byte[] SerializeExportData()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
				{
					FObjectExport[] exports = Summary.Exports;
					foreach (FObjectExport fObjectExport in exports)
					{
						foreach (UObject @object in fObjectExport.Objects)
						{
							@object.BSerialize(binaryWriter, Summary);
							if (fObjectExport.ClassIndex.Name == "StringTable")
							{
								binaryWriter.Write(0);
							}
						}
					}
					return memoryStream.ToArray();
				}
			}
		}

		public void UpdateFromJSON(string data)
		{
			Summary.UpdatedNames = Summary.Names.ToHashSet();
			FObjectExport[] exports = Summary.Exports;
			for (int i = 0; i < exports.Length; i++)
			{
				foreach (UObject @object in exports[i].Objects)
				{
					@object.UpdateFromJSON(data, Summary);
				}
			}
			List<FNameEntry> list = Summary.UpdatedNames.ToList();
			list.Sort(new FNameByString());
			Summary.Names = list.ToArray();
			string[] names = Summary.Names.Select((FNameEntry x) => x.Name).ToArray();
			FObjectImport[] imports = Summary.Imports;
			for (int i = 0; i < imports.Length; i++)
			{
				imports[i].UpdateIndex(names, Summary);
			}
			exports = Summary.Exports;
			foreach (FObjectExport obj in exports)
			{
				obj.ObjectName.UpdateIndex(names, Summary);
				foreach (UObject object2 in obj.Objects)
				{
					object2.UpdateIndex(names, Summary);
				}
			}
		}
	}
}
