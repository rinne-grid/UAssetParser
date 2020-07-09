using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("StructProperty")]
	[Category("Unreal")]
	public class UStructProperty : FPropertyTag
	{
		[DataMember]
		public FName StructName
		{
			get;
			set;
		}

		[DataMember]
		public Guid StructGuid
		{
			get;
			set;
		}

		[DataMember]
		public FPropertyGuid Guid
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public object Struct
		{
			get;
			set;
		}

		public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
		{
			UStructProperty uStructProperty = LSerializer.Deserialize<UStructProperty>(reader);
			uStructProperty.Ref(summary);
			uStructProperty.Struct = (((object)VisitorFactory.VisitStruct(reader, uStructProperty.StructName.Name, summary)) ?? ((object)new UObject(reader, summary, pad: false)));
			return uStructProperty;
		}

		public override object GetValue()
		{
			return UArrayProperty.Unwrap(Struct);
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			StructName.Ref(summary);
		}

		public override int GetSize()
		{
			if (Guid.HasGuid != 0)
			{
				return base.Size + 48 + 16;
			}
			return base.Size + 49;
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			if (!Struct.GetType().IsArray)
			{
				SingleStruct(data, summary);
				return;
			}
			object[] array = Struct as object[];
			object[] array2 = data as object[];
			object[] array3 = new object[array2.Length];
			if (UAsset.Options.Verbose && array.Length != array2.Length)
			{
				Console.WriteLine("Array size difference between base and JSON: " + base.Name?.Name + " -> " + StructName.Name);
			}
			VisitorFactory.StructVisitor value;
			bool flag = VisitorFactory.StructVisitors.TryGetValue(StructName.Name, out value);
			Type type = flag ? value.Target.GetType() : null;
			if (!flag && !UAsset.Options.ForceArrays)
			{
				return;
			}
			if (UAsset.Options.ForceArrays && UAsset.Options.Verbose)
			{
				Console.WriteLine("Forcing array " + base.Name?.Name + " -> " + StructName.Name + " with UObjects, integrity warning!");
			}
			base.Size = 0;
			for (int i = 0; i < array2.Length; i++)
			{
				if (flag)
				{
					object obj = JsonConvert.DeserializeObject(array2[i].ToString(), type);
					array3[i] = (obj as IStructObject);
					base.Size += (obj as IStructObject).GetSize();
					continue;
				}
				JObject jObject = JObject.Parse(array2[i].ToString());
				List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>(jObject.Count - 1);
				string[] array4 = new string[jObject.Count - 1];
				foreach (JProperty item in jObject.Properties())
				{
					if (item.Name != "UTTypes")
					{
						list.Add(new KeyValuePair<string, object>(item.Name, item.Value));
					}
					else
					{
						array4 = item.Value.ToString().Split(',').ToArray();
					}
				}
				UObject uObject = new UObject();
				for (int j = 0; j < list.Count; j++)
				{
					uObject.Add(JsonToProperty(list[j].Key, array4[j], list[j].Value, summary));
				}
				array3[i] = uObject;
				base.Size += uObject.GetSize();
			}
			Struct = array3;
		}

		private void SingleStruct(object data, FPackageFileSummary summary)
		{
			IStructObject structObject = Struct as IStructObject;
			if (structObject != null)
			{
				Struct = JsonConvert.DeserializeObject(data.ToString(), Struct.GetType());
				base.Size = structObject.GetSize();
				return;
			}
			UObject uObject = Struct as UObject;
			if (uObject == null || !UAsset.Options.ForceArrays)
			{
				return;
			}
			if (UAsset.Options.Verbose)
			{
				Console.WriteLine($"Forcing Struct {base.Name?.Name} -> {StructName} update as a single UObject, integrity warning!");
			}
			JObject jObject = JObject.Parse(data.ToString());
			List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>(jObject.Count);
			foreach (JProperty item in jObject.Properties())
			{
				list.Add(new KeyValuePair<string, object>(item.Name, item.Value));
			}
			for (int i = 0; i < uObject.Count; i++)
			{
				uObject[i].UpdateFromJSON(list[i].Value, summary);
			}
			base.Size += uObject.GetSize();
		}

		private FPropertyTag JsonToProperty(string name, string type, object value, FPackageFileSummary summary)
		{
			FPropertyTag fPropertyTag = null;
			switch (type)
			{
			case "FloatProperty":
				fPropertyTag = new UFloatProperty
				{
					Size = 4
				};
				break;
			case "UInt32Property":
				fPropertyTag = new UInt32Property
				{
					Size = 4
				};
				break;
			case "IntProperty":
				fPropertyTag = new UIntProperty
				{
					Size = 4
				};
				break;
			case "NameProperty":
				fPropertyTag = new UNameProperty
				{
					Size = 8
				};
				break;
			case "StrProperty":
				fPropertyTag = new UStrProperty();
				break;
			default:
				throw new NotImplementedException("Dunno how to process " + type);
			}
			fPropertyTag.GenerateNew(name, type, summary);
			fPropertyTag.UpdateFromJSON(value, summary);
			return fPropertyTag;
		}

		public override void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
			if (Struct.GetType().IsArray)
			{
				object[] array = Struct as object[];
				foreach (object obj in array)
				{
					IStructObject structObject = obj as IStructObject;
					if (structObject != null)
					{
						BSerializer.Serialize(writer, structObject.GetType(), structObject);
					}
					else
					{
						(obj as UObject)?.BSerialize(writer, summary);
					}
				}
			}
			else if (Struct is IStructObject)
			{
				BSerializer.Serialize(writer, Struct.GetType(), Struct);
			}
			else
			{
				(Struct as UObject)?.BSerialize(writer, summary);
			}
		}

		public override void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			base.UpdateIndex(names, summary);
			StructName.UpdateIndex(names, summary);
			if (Struct.GetType().IsArray)
			{
				object[] array = Struct as object[];
				if (array.Length >= 1)
				{
					object[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						(array2[i] as UObject)?.UpdateIndex(names, summary);
					}
				}
			}
			else
			{
				(Struct as UObject)?.UpdateIndex(names, summary);
			}
		}
	}
}
