using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[Description("StringTable")]
	[Category("Unreal")]
	public class StringTable : Dictionary<string, string>, IExportObject, IEnumerable
	{
		public string Name
		{
			get;
			set;
		}

		public static IExportObject Deserialize(BinaryReader reader, FObjectExport export, FPackageFileSummary summary)
		{
			StringTable stringTable = new StringTable();
			stringTable.Name = LSerializer.FString(reader);
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				stringTable.Add(LSerializer.FString(reader), LSerializer.FString(reader));
			}
			return stringTable;
		}

		public object Serialize()
		{
			return new
			{
				Name = Name,
				Table = this
			};
		}

		public void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
			BSerializer.WriteFString(writer, Name);
			writer.Write(base.Count);
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.Current;
					BSerializer.WriteFString(writer, current.Key);
					BSerializer.WriteFString(writer, current.Value);
				}
			}
		}

		public void UpdateFromJSON(object jdata, FPackageFileSummary summary)
		{
			var anonymousTypeObject = new
			{
				Name = string.Empty,
				Table = new Dictionary<string, string>()
			};
			var anon = JsonConvert.DeserializeAnonymousType(jdata.ToString(), anonymousTypeObject);
			Name = anon.Name;
			if (anon.Table.Count != base.Count)
			{
				throw new NotImplementedException("JSON data has different amount of keys!");
			}
			foreach (KeyValuePair<string, string> item in anon.Table)
			{
				base[item.Key] = item.Value;
			}
		}

		public void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
		}
	}
}
