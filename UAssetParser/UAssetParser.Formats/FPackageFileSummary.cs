using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Attributes;

namespace UAssetParser.Formats
{
	[DataContract]
	[DebuggerDisplay("[FPackageFileSummary {FolderName}]")]
	public class FPackageFileSummary
	{
		[DataMember]
		[TextDumpable]
		public int Tag
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int FileVersionUE4
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int FileVersionLicenseeUE4
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int FileVersionUE3
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int FileVersionLicenseeUE3
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public FCustomVersion[] CustomVersion
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int TotalHeaderSize
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public string FolderName
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public uint PackageFlags
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int NameCount
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int NameOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int GatherableNameCount
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int GatherableNameOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int ExportCount
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int ExportOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int ImportCount
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int ImportOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int DependsOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int SoftPackageReferencesCount
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int SoftPackageReferencesOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int SearchableNamesOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int ThumbnailTableOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public Guid Guid
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public FGenerationInfo[] Generations
		{
			get;
			set;
		}

		[DataMember]
		public FEngineVersion SavedByEngineVersion
		{
			get;
			set;
		}

		[DataMember]
		public FEngineVersion CompatibleWithEngineVersion
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public uint CompressionFlags
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public uint PackageSource
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public bool Unversioned
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int AssetRegistryDataOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public long BulkDataStartOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int WorldTileInfoDataOffset
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int[] ChunkIDs
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int PreloadDependencyCount
		{
			get;
			set;
		}

		[DataMember]
		[TextDumpable]
		public int PreloadDependencyOffset
		{
			get;
			set;
		}

		[DataMember]
		[Description("NameOffset")]
		[Category("NameCount")]
		public FNameEntry[] Names
		{
			get;
			set;
		}

		[DataMember]
		[Description("ImportOffset")]
		[Category("ImportCount")]
		public FObjectImport[] Imports
		{
			get;
			set;
		}

		[DataMember]
		[Description("ExportOffset")]
		[Category("ExportCount")]
		public FObjectExport[] Exports
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public HashSet<FNameEntry> UpdatedNames
		{
			get;
			set;
		} = new HashSet<FNameEntry>();


		[IgnoreDataMember]
		public HashSet<string> NewEntries
		{
			get;
			set;
		} = new HashSet<string>();


		public void UpdateMeta(int serialLength)
		{
			BulkDataStartOffset += serialLength - Exports.First().SerialSize;
			Exports.First().SerialSize = serialLength;
			NameCount = Names.Length;
			int num = 193;
			int num2 = 0;
			FNameEntry[] names = Names;
			foreach (FNameEntry fNameEntry in names)
			{
				num2 += fNameEntry.GetLength();
			}
			int num3 = ImportCount * 28;
			int num4 = ExportCount * 108;
			ImportOffset = num + num2;
			ExportOffset = num + num2 + num3;
			DependsOffset = num + num2 + num3 + num4 - 4;
			TotalHeaderSize = num + num2 + num3 + num4 + 4;
			AssetRegistryDataOffset = num + num2 + num3 + num4;
			PreloadDependencyOffset = TotalHeaderSize;
			Generations.First().NameCount = Names.Length;
			Exports.First().SerialOffset = TotalHeaderSize;
		}
	}
}
