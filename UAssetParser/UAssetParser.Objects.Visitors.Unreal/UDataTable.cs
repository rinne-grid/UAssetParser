using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[Description("DataTable")]
	[Category("Unreal")]
	public class UDataTable : Dictionary<FName, UObject>, IExportObject, IEnumerable
	{
		public static IExportObject Deserialize(BinaryReader reader, FObjectExport export, FPackageFileSummary summary)
		{
			UDataTable uDataTable = new UDataTable();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				FName fName = LSerializer.Deserialize<FName>(reader);
				fName.Ref(summary);
				UObject uObject = new UObject(reader, summary, pad: false);
				uObject.Name = fName;
				uDataTable.Add(fName, uObject);
			}
			return uDataTable;
		}

		public object Serialize()
		{
			return this.Select((KeyValuePair<FName, UObject> x) => new KeyValuePair<string, object>(x.Key.GetValue(), x.Value.ToDictionary())).ToArray();
		}

		public void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
			writer.Write(base.Count);
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FName, UObject> current = enumerator.Current;
					BSerializer.Serialize<FName>(writer, current.Key);
					current.Value.BSerialize(writer, summary);
				}
			}
		}

		public void UpdateFromJSON(object jdata, FPackageFileSummary summary)
		{
			KeyValuePair<string, object>[] array = JsonConvert.DeserializeObject<KeyValuePair<string, object>[]>(jdata.ToString());
			int num = 0;
			if (array.Length != base.Count && !UAsset.Options.AllowNewEntries)
			{
				throw new NotImplementedException("JSON data has different amount of keys!");
			}
			if (array.Length < base.Count)
			{
				throw new NotImplementedException("Removal of the entries is not possible, fam");
			}
			for (int i = base.Count; i < array.Length; i++)
			{
				FName fName = new FName();
				fName.UpdateName(array[i].Key, summary);
				UObject uObject = new UObject
				{
					Name = fName
				};
				foreach (FPropertyTag item in this.First().Value)
				{
					uObject.Add(DeepClone(item) as FPropertyTag);
				}
				Add(fName, uObject);
			}
			foreach (UObject value in base.Values)
			{
				value.UpdateFromJSON(array.ElementAt(num++).Value, summary);
			}
		}

		public void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FName, UObject> current = enumerator.Current;
					current.Key.UpdateIndex(names, summary);
					current.Value.UpdateIndex(names, summary);
				}
			}
		}

		private object DeepClone(object original)
		{
			if (original is List<object>)
			{
				return new List<object>();
			}
			UArrayProperty uArrayProperty = original as UArrayProperty;
			if (uArrayProperty != null && (string)uArrayProperty.ArrayType == "StructProperty")
			{
				throw new NotImplementedException("Arrays with StructProperties are not supported");
			}
			object obj = Activator.CreateInstance(original.GetType());
			PropertyInfo[] array = (from x in original.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				select (x) into x
				where x.CanWrite
				select x).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].PropertyType.IsValueType || array[i].PropertyType.IsPrimitive || array[i].PropertyType == typeof(string))
				{
					array[i].SetValue(obj, array[i].GetValue(original));
				}
				else
				{
					array[i].SetValue(obj, DeepClone(array[i].GetValue(original)));
				}
			}
			return obj;
		}
	}
}
