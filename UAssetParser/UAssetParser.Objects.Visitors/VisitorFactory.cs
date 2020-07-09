using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UAssetParser.Formats;
using UAssetParser.Objects.Visitors.Unreal;

namespace UAssetParser.Objects.Visitors
{
	public static class VisitorFactory
	{
		public delegate FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary);

		public delegate IExportObject ExportVisitor(BinaryReader reader, FObjectExport export, FPackageFileSummary summary);

		public delegate IStructObject StructVisitor(BinaryReader reader, FPackageFileSummary summary);

		public delegate object EnumerableVisitor(BinaryReader binaryReader, FPackageFileSummary summary, int count);

		public delegate void EnumerableWriter(BinaryWriter binaryWriter, FPackageFileSummary summary, object value, int count);

		public static readonly Dictionary<string, PropertyVisitor> Visitors = new Dictionary<string, PropertyVisitor>();

		public static readonly Dictionary<string, ExportVisitor> ExportVisitors = new Dictionary<string, ExportVisitor>();

		public static readonly Dictionary<string, StructVisitor> StructVisitors = new Dictionary<string, StructVisitor>();

		public static readonly Dictionary<string, EnumerableVisitor> EnumerableVisitors = new Dictionary<string, EnumerableVisitor>
		{
			{
				"StructProperty",
				delegate(BinaryReader r, FPackageFileSummary s, int c)
				{
					long position = r.BaseStream.Position;
					try
					{
						UStructProperty uStructProperty2 = LSerializer.Deserialize<UStructProperty>(r);
						uStructProperty2.Ref(s);
						object[] array = new object[c];
						for (int i = 0; i < c; i++)
						{
							array[i] = (((object)VisitStruct(r, uStructProperty2.StructName, s)) ?? ((object)new UObject(r, s, pad: false)));
						}
						uStructProperty2.Struct = array;
						return uStructProperty2;
					}
					catch
					{
						r.BaseStream.Position = position;
						return new UObject(r, s, pad: false);
					}
				}
			},
			{
				"BoolProprety",
				(BinaryReader r, FPackageFileSummary s, int c) => r.ReadByte() == 1
			},
			{
				"IntProperty",
				(BinaryReader r, FPackageFileSummary s, int c) => r.ReadInt32()
			},
			{
				"UInt32Property",
				(BinaryReader r, FPackageFileSummary s, int c) => r.ReadUInt32()
			},
			{
				"ByteProperty",
				delegate(BinaryReader r, FPackageFileSummary s, int c)
				{
					FName fName3 = LSerializer.Deserialize<FName>(r);
					fName3.Ref(s);
					return fName3;
				}
			},
			{
				"FloatProperty",
				(BinaryReader r, FPackageFileSummary s, int c) => r.ReadSingle()
			},
			{
				"StrProperty",
				(BinaryReader r, FPackageFileSummary s, int c) => LSerializer.FString(r)
			},
			{
				"EnumProperty",
				delegate(BinaryReader r, FPackageFileSummary s, int c)
				{
					FName fName2 = LSerializer.Deserialize<FName>(r);
					fName2.Ref(s);
					return fName2;
				}
			},
			{
				"NameProperty",
				delegate(BinaryReader r, FPackageFileSummary s, int c)
				{
					FName fName = LSerializer.Deserialize<FName>(r);
					fName.Ref(s);
					return fName;
				}
			},
			{
				"TextProperty",
				(BinaryReader r, FPackageFileSummary s, int c) => new UTextProperty
				{
					StreamOffset = r.ReadInt64(),
					Guid = LSerializer.Deserialize<FPropertyGuid>(r),
					Hash = LSerializer.FString(r),
					Value = LSerializer.FString(r)
				}
			},
			{
				"SoftObjectProperty",
				delegate(BinaryReader r, FPackageFileSummary s, int c)
				{
					USoftObjectProperty uSoftObjectProperty2 = new USoftObjectProperty();
					uSoftObjectProperty2.Package = LSerializer.Deserialize<FName>(r);
					uSoftObjectProperty2.Package.Ref(s);
					uSoftObjectProperty2.Path = LSerializer.FString(r);
					return uSoftObjectProperty2;
				}
			},
			{
				"ObjectProperty",
				delegate(BinaryReader r, FPackageFileSummary s, int c)
				{
					FPackageIndex fPackageIndex = LSerializer.Deserialize<FPackageIndex>(r);
					fPackageIndex.Ref(s);
					return fPackageIndex;
				}
			}
		};

		public static readonly Dictionary<string, EnumerableWriter> EnumerableWriters = new Dictionary<string, EnumerableWriter>
		{
			{
				"StructProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					UStructProperty uStructProperty = v as UStructProperty;
					if (uStructProperty != null)
					{
						BSerializer.Serialize<UStructProperty>(w, uStructProperty);
						uStructProperty.BSerialize(w, s);
					}
					else
					{
						UObject uObject = v as UObject;
						if (uObject == null)
						{
							throw new NotImplementedException("Struct Array should be either UStructProperty or UObject");
						}
						uObject.BSerialize(w, s);
					}
				}
			},
			{
				"BoolProprety",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					w.Write(Convert.ToByte(v));
				}
			},
			{
				"IntProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					w.Write(Convert.ToInt32(v));
				}
			},
			{
				"UInt32Property",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					w.Write(Convert.ToUInt32(v));
				}
			},
			{
				"ByteProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					BSerializer.Serialize<FName>(w, v);
				}
			},
			{
				"FloatProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					w.Write(Convert.ToSingle(v));
				}
			},
			{
				"StrProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					BSerializer.WriteFString(w, Convert.ToString(v));
				}
			},
			{
				"EnumProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					BSerializer.Serialize<FName>(w, v);
				}
			},
			{
				"NameProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					BSerializer.Serialize<FName>(w, v);
				}
			},
			{
				"TextProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					UTextProperty uTextProperty = v as UTextProperty;
					w.Write(uTextProperty.StreamOffset);
					BSerializer.Serialize<FPropertyGuid>(w, uTextProperty.Guid);
					BSerializer.WriteFString(w, uTextProperty.Hash);
					BSerializer.WriteFString(w, uTextProperty.Value);
				}
			},
			{
				"SoftObjectProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					USoftObjectProperty uSoftObjectProperty = v as USoftObjectProperty;
					BSerializer.Serialize<FName>(w, uSoftObjectProperty.Name);
					BSerializer.WriteFString(w, uSoftObjectProperty.Path);
				}
			},
			{
				"ObjectProperty",
				delegate(BinaryWriter w, FPackageFileSummary s, object v, int c)
				{
					BSerializer.Serialize<FPackageIndex>(w, v);
				}
			}
		};

		public static FPropertyTag Visit(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
		{
			if (Visitors.TryGetValue(baseTag.Type.Name, out PropertyVisitor value))
			{
				return value(reader, baseTag, summary);
			}
			throw new NotImplementedException("No parser for " + baseTag.Type.Name);
		}

		public static object VisitEnumerable(BinaryReader reader, string type, FPackageFileSummary summary, int count)
		{
			if (EnumerableVisitors.TryGetValue(type, out EnumerableVisitor value))
			{
				return value(reader, summary, count);
			}
			throw new NotImplementedException("No parser for " + type);
		}

		public static void WriteEnumerable(BinaryWriter writer, string type, FPackageFileSummary summary, object value, int count)
		{
			if (EnumerableWriters.TryGetValue(type, out EnumerableWriter value2))
			{
				value2(writer, summary, value, count);
				return;
			}
			throw new NotImplementedException("No parser for " + type);
		}

		public static void SetEnumAsByte()
		{
		}

		internal static IStructObject VisitStruct(BinaryReader reader, string structName, FPackageFileSummary summary)
		{
			if (StructVisitors.TryGetValue(structName, out StructVisitor value))
			{
				return value(reader, summary);
			}
			return null;
		}

		public static IExportObject VisitSubtype(BinaryReader reader, FObjectExport export, FPackageFileSummary summary)
		{
			if (ExportVisitors.TryGetValue(export.ClassIndex.Name, out ExportVisitor value))
			{
				return value(reader, export, summary);
			}
			return null;
		}

		public static void LoadVisitors(string gameType = "Unreal", Assembly loadFrom = null)
		{
			if ((object)loadFrom == null)
			{
				loadFrom = Assembly.GetExecutingAssembly();
			}
			LoadExportVisitors(gameType, loadFrom);
			LoadStructVisitors(gameType, loadFrom);
			Type baseType = typeof(FPropertyTag);
			foreach (Type item in from x in loadFrom.GetTypes()
				where baseType.IsAssignableFrom(x) && x.GetCustomAttribute<CategoryAttribute>()?.Category == gameType
				select x)
			{
				string text = item.GetCustomAttribute<DescriptionAttribute>()?.Description;
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new InvalidDataException(item.FullName + " has no DescriptionAttribute!");
				}
				PropertyVisitor propertyVisitor = (from x in item.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public).Select(CreateDelegate)
					where x != null
					select x).FirstOrDefault();
				Visitors[text] = (propertyVisitor ?? CreateTagDeserializer(item));
			}
		}

		public static void LoadExportVisitors(string gameType = "Unreal", Assembly loadFrom = null)
		{
			if ((object)loadFrom == null)
			{
				loadFrom = Assembly.GetExecutingAssembly();
			}
			Type baseType = typeof(IExportObject);
			foreach (Type item in from x in loadFrom.GetTypes()
				where baseType.IsAssignableFrom(x) && x.GetCustomAttribute<CategoryAttribute>()?.Category == gameType
				select x)
			{
				string text = item.GetCustomAttribute<DescriptionAttribute>()?.Description;
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new InvalidDataException(item.FullName + " has no DescriptionAttribute!");
				}
				ExportVisitor exportVisitor = (from x in item.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public).Select(CreateExportDelegate)
					where x != null
					select x).FirstOrDefault();
				ExportVisitors[text] = (exportVisitor ?? throw new NotImplementedException(item.FullName + " has no Export Delegate!"));
			}
		}

		public static void LoadStructVisitors(string gameType = "Unreal", Assembly loadFrom = null)
		{
			if ((object)loadFrom == null)
			{
				loadFrom = Assembly.GetExecutingAssembly();
			}
			Type baseType = typeof(IStructObject);
			foreach (Type item in from x in loadFrom.GetTypes()
				where baseType.IsAssignableFrom(x) && x.GetCustomAttribute<CategoryAttribute>()?.Category == gameType
				select x)
			{
				string text = item.GetCustomAttribute<DescriptionAttribute>()?.Description;
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new InvalidDataException(item.FullName + " has no DescriptionAttribute!");
				}
				StructVisitor structVisitor = (from x in item.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public).Select(CreateStructDelegate)
					where x != null
					select x).FirstOrDefault();
				StructVisitors[text] = (structVisitor ?? CreateStructDeserializer(item));
			}
		}

		private static PropertyVisitor CreateDelegate(MethodInfo info)
		{
			try
			{
				return (PropertyVisitor)info.CreateDelegate(typeof(PropertyVisitor));
			}
			catch
			{
				return null;
			}
		}

		private static ExportVisitor CreateExportDelegate(MethodInfo info)
		{
			try
			{
				return (ExportVisitor)info.CreateDelegate(typeof(ExportVisitor));
			}
			catch
			{
				return null;
			}
		}

		private static StructVisitor CreateStructDelegate(MethodInfo info)
		{
			try
			{
				return (StructVisitor)info.CreateDelegate(typeof(StructVisitor));
			}
			catch
			{
				return null;
			}
		}

		private static PropertyVisitor CreateTagDeserializer(Type visitor)
		{
			return (BinaryReader r, FPropertyTag b, FPackageFileSummary s) => (FPropertyTag)LSerializer.Deserialize(r, visitor);
		}

		private static StructVisitor CreateStructDeserializer(Type visitor)
		{
			return (BinaryReader r, FPackageFileSummary s) => (IStructObject)LSerializer.Deserialize(r, visitor);
		}
	}
}
