using Newtonsoft.Json;
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
	[Description("ArrayProperty")]
	[Category("Unreal")]
	public class UArrayProperty : FPropertyTag
	{
		[DataMember]
		public FName ArrayType
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

		[DataMember]
		public int ArraySize
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public List<object> Entries
		{
			get;
			set;
		} = new List<object>();


		[IgnoreDataMember]
		public bool isFNameEntries
		{
			get
			{
				if (!((string)ArrayType == "ByteProperty") && !((string)ArrayType == "NameProperty"))
				{
					return (string)ArrayType == "EnumProperty";
				}
				return true;
			}
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			ArrayType.Ref(summary);
		}

		public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
		{
			UArrayProperty uArrayProperty = LSerializer.Deserialize<UArrayProperty>(reader);
			uArrayProperty.Ref(summary);
			long position = reader.BaseStream.Position;
			uArrayProperty.Entries = GetEntries(uArrayProperty.ArraySize, uArrayProperty.ArrayType, reader, summary);
			long position2 = reader.BaseStream.Position;
			if (position + uArrayProperty.Size - 4 != position2)
			{
				Console.WriteLine("WARNING: ARRAY SIZE OFFSHOOT!");
			}
			reader.BaseStream.Position = position + uArrayProperty.Size - 4;
			return uArrayProperty;
		}

		public static List<object> GetEntries(int arraySize, FName arrayType, BinaryReader reader, FPackageFileSummary summary)
		{
			List<object> list = new List<object>();
			if ((string)arrayType == "StructProperty")
			{
				list.Add(VisitorFactory.VisitEnumerable(reader, arrayType, summary, arraySize));
			}
			else
			{
				for (int i = 0; i < arraySize; i++)
				{
					list.Add(VisitorFactory.VisitEnumerable(reader, arrayType, summary, 1));
				}
			}
			return list;
		}

		public static object Unwrap(object x)
		{
			object[] array = x as object[];
			if (array != null)
			{
				return array.Select(Unwrap).ToArray();
			}
			UObject uObject = x as UObject;
			if (uObject != null)
			{
				return uObject.ToDictionary(inStruct: true);
			}
			IStructObject structObject = x as IStructObject;
			if (structObject != null)
			{
				return structObject.Serialize();
			}
			FName fName = x as FName;
			if (fName != null)
			{
				return fName.GetValue();
			}
			FPropertyTag fPropertyTag = x as FPropertyTag;
			if (fPropertyTag != null)
			{
				return fPropertyTag.GetValue();
			}
			Tuple<FPropertyTag, FPropertyTag> tuple = x as Tuple<FPropertyTag, FPropertyTag>;
			if (tuple != null)
			{
				return new object[2]
				{
					tuple.Item1.GetValue(),
					tuple.Item2.GetValue()
				};
			}
			return x;
		}

		public override object GetValue()
		{
			return Entries.Select(Unwrap).ToArray();
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			if ((string)ArrayType == "StructProperty")
			{
				object[] data2 = JsonConvert.DeserializeObject<object[][]>(data.ToString()).FirstOrDefault();
				object obj = Entries.First();
				UStructProperty uStructProperty = obj as UStructProperty;
				if (uStructProperty != null)
				{
					uStructProperty.UpdateFromJSON(data2, summary);
					ArraySize = ((!uStructProperty.Struct.GetType().IsArray) ? 1 : (uStructProperty.Struct as object[]).Length);
					base.Size = uStructProperty.GetSize() + 4;
				}
				else if (obj is UObject)
				{
					throw new NotImplementedException("Uobject as Array entry for StructProperty array not supported");
				}
				return;
			}
			object[] array = JsonConvert.DeserializeObject<object[]>(data.ToString());
			if (isFNameEntries && !UAsset.Options.ForceArrays)
			{
				return;
			}
			if (UAsset.Options.ForceArrays && UAsset.Options.ForceArrays)
			{
				Console.WriteLine("Forcefully updating array " + base.Name?.Name + " with name entries, integrity may suffer");
			}
			List<object> list = new List<object>();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				object obj2 = null;
				switch ((string)ArrayType)
				{
				case "ByteProperty":
				case "NameProperty":
				case "EnumProperty":
					obj2 = new FName();
					(obj2 as FName).UpdateName(array[i].ToString(), summary);
					num = 8;
					break;
				case "IntProperty":
					obj2 = Convert.ToInt32(array[i].ToString());
					num = 4;
					break;
				case "UInt32Property":
					obj2 = Convert.ToUInt32(array[i].ToString());
					num = 4;
					break;
				case "FloatProperty":
					obj2 = Convert.ToSingle(array[i].ToString());
					num = 4;
					break;
				default:
					throw new NotImplementedException($"No converter for {ArrayType}");
				}
				if (obj2 != null)
				{
					list.Add(obj2);
				}
			}
			ArraySize = list.Count;
			base.Size = ArraySize * num + 4;
			Entries = list;
		}

		public override void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
			if ((string)ArrayType == "StructProperty")
			{
				VisitorFactory.WriteEnumerable(writer, ArrayType, summary, Entries.First(), ArraySize);
			}
			else
			{
				if (Entries.Count <= 0)
				{
					return;
				}
				foreach (object entry in Entries)
				{
					VisitorFactory.WriteEnumerable(writer, ArrayType, summary, entry, ArraySize);
				}
			}
		}

		public override void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			base.UpdateIndex(names, summary);
			ArrayType.UpdateIndex(names, summary);
			if ((string)ArrayType == "StructProperty")
			{
				(Entries.First() as UStructProperty).UpdateIndex(names, summary);
			}
			else
			{
				if (Entries.Count <= 0)
				{
					return;
				}
				foreach (object entry in Entries)
				{
					switch ((string)ArrayType)
					{
					case "ByteProperty":
					case "NameProperty":
					case "EnumProperty":
						(entry as FName).UpdateIndex(names, summary);
						break;
					case "SoftObjectProperty":
					case "ObjectProperty":
					case "TextProperty":
						throw new NotImplementedException($"No indexer logic for {ArrayType}");
					}
				}
			}
		}
	}
}
