using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetParser.Formats;
using UAssetParser.Objects.Visitors;

namespace UAssetParser.Objects
{
	public class UObject : List<FPropertyTag>
	{
		public IExportObject ObjectData
		{
			get;
			set;
		}

		public FName Name
		{
			get;
			set;
		}

		public UObject()
		{
		}

		public UObject(BinaryReader data, FPackageFileSummary summary, bool pad = true, FObjectExport export = null)
		{
			while (true)
			{
				long position = data.BaseStream.Position;
				FName fName = LSerializer.Deserialize<FName>(data);
				fName.Ref(summary);
				if ((string)fName == "None")
				{
					break;
				}
				data.BaseStream.Position = position;
				FPropertyTag fPropertyTag = LSerializer.Deserialize<FPropertyTag>(data);
				data.BaseStream.Position = position;
				fPropertyTag.Ref(summary);
				fPropertyTag = VisitorFactory.Visit(data, fPropertyTag, summary);
				fPropertyTag.Ref(summary);
				Add(fPropertyTag);
			}
			if (pad)
			{
				data.BaseStream.Position += 4L;
			}
			if (export != null)
			{
				ObjectData = VisitorFactory.VisitSubtype(data, export, summary);
			}
		}

		public Dictionary<string, object> ToDictionary(bool inStruct = false)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FPropertyTag current = enumerator.Current;
					dictionary[current.Name.GetValue()] = current.GetValue();
				}
			}
			if (inStruct && base.Count > 0)
			{
				dictionary["UTTypes"] = string.Join(",", this.Select((FPropertyTag x) => x.Type.Name));
			}
			if (ObjectData != null)
			{
				dictionary["ExportData"] = ObjectData.Serialize();
			}
			return dictionary;
		}

		public int GetSize()
		{
			int num = 0;
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FPropertyTag current = enumerator.Current;
					num += current.GetSize();
				}
			}
			return num + 8;
		}

		public void UpdateFromJSON(object jdata, FPackageFileSummary summary)
		{
			if (ObjectData != null)
			{
				ObjectData.UpdateFromJSON(jdata, summary);
				return;
			}
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FPropertyTag current = enumerator.Current;
					JObject.Parse(jdata.ToString()).TryGetValue(current.Name, StringComparison.OrdinalIgnoreCase, out JToken value);
					current.UpdateFromJSON(value, summary);
				}
			}
		}

		public void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
			int index = summary.Names.Select((FNameEntry x, int i) => new
			{
				name = x.Name,
				index = i
			}).First(x => x.name.Equals("None")).index;
			FName instance = new FName
			{
				Index = index
			};
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FPropertyTag current = enumerator.Current;
					BSerializer.Serialize(writer, current.GetType(), current);
					current.BSerialize(writer, summary);
				}
			}
			BSerializer.Serialize(writer, typeof(FName), instance);
			if (ObjectData != null)
			{
				writer.BaseStream.Position += 4L;
				ObjectData.BSerialize(writer, summary);
			}
		}

		public void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.UpdateIndex(names, summary);
				}
			}
			if (ObjectData != null)
			{
				ObjectData.UpdateIndex(names, summary);
			}
		}
	}
}
