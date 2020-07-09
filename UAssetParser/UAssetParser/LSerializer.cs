using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace UAssetParser
{
	public static class LSerializer
	{
		public delegate object PrimitiveVisitor(BinaryReader reader);

		private static readonly Dictionary<Type, (PropertyInfo, string, object, object, object)[]> PropertyMap = new Dictionary<Type, (PropertyInfo, string, object, object, object)[]>();

		public static Dictionary<Type, PrimitiveVisitor> PrimitiveMap = new Dictionary<Type, PrimitiveVisitor>
		{
			{
				typeof(sbyte),
				(BinaryReader r) => r.ReadSByte()
			},
			{
				typeof(byte),
				(BinaryReader r) => r.ReadByte()
			},
			{
				typeof(ushort),
				(BinaryReader r) => r.ReadUInt16()
			},
			{
				typeof(short),
				(BinaryReader r) => r.ReadInt16()
			},
			{
				typeof(uint),
				(BinaryReader r) => r.ReadUInt32()
			},
			{
				typeof(int),
				(BinaryReader r) => r.ReadInt32()
			},
			{
				typeof(ulong),
				(BinaryReader r) => r.ReadUInt64()
			},
			{
				typeof(long),
				(BinaryReader r) => r.ReadInt64()
			},
			{
				typeof(bool),
				delegate(BinaryReader r)
				{
					bool num = r.ReadByte() != 0;
					r.BaseStream.Position += 3L;
					return num;
				}
			},
			{
				typeof(char),
				(BinaryReader r) => r.ReadChar()
			},
			{
				typeof(decimal),
				(BinaryReader r) => r.ReadDecimal()
			},
			{
				typeof(double),
				(BinaryReader r) => r.ReadDouble()
			},
			{
				typeof(float),
				(BinaryReader r) => r.ReadSingle()
			},
			{
				typeof(string),
				FString
			},
			{
				typeof(Guid),
				(BinaryReader r) => FGuid(r)
			}
		};

		public static T Deserialize<T>(Stream data, bool leaveOpen = false)
		{
			using (BinaryReader data2 = new BinaryReader(data, Encoding.UTF8, leaveOpen))
			{
				return Deserialize<T>(data2);
			}
		}

		public static T Deserialize<T>(BinaryReader data)
		{
			return (T)Deserialize(data, typeof(T));
		}

		public static object Deserialize(BinaryReader data, Type T, int? countRef = null)
		{
			if (T.IsEnum)
			{
				T = T.GetEnumUnderlyingType();
			}
			if (!PrimitiveMap.TryGetValue(T, out PrimitiveVisitor value))
			{
				return DeserializeComplex(data, T, countRef);
			}
			return value(data);
		}

		private static object DeserializeComplex(BinaryReader data, Type T, int? countRef = null)
		{
			if (T.IsArray)
			{
				if (!countRef.HasValue)
				{
					return FArray(data, T.GetElementType());
				}
				return FArray(data, countRef.Value, T.GetElementType());
			}
			(PropertyInfo Property, string Kind, object OffsetReference, object CountReference, object ConditionReference)[] properties = GetProperties(T);
			object obj = Activator.CreateInstance(T);
			(PropertyInfo, string, object, object, object)[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				(PropertyInfo, string, object, object, object) valueTuple = array[i];
				PropertyInfo item = valueTuple.Item1;
				object item2 = valueTuple.Item3;
				object item3 = valueTuple.Item4;
				PropertyInfo propertyInfo = valueTuple.Item5 as PropertyInfo;
				if ((object)propertyInfo != null && (int)Convert.ChangeType(propertyInfo.GetValue(obj), typeof(int)) != 1)
				{
					continue;
				}
				int? num = item2 as int?;
				int? countRef2 = item3 as int?;
				if (!num.HasValue)
				{
					PropertyInfo propertyInfo2 = item2 as PropertyInfo;
					if ((object)propertyInfo2 != null)
					{
						num = (int)Convert.ChangeType(propertyInfo2.GetValue(obj), typeof(int));
					}
				}
				if (!countRef2.HasValue)
				{
					PropertyInfo propertyInfo3 = item3 as PropertyInfo;
					if ((object)propertyInfo3 != null)
					{
						countRef2 = (int)Convert.ChangeType(propertyInfo3.GetValue(obj), typeof(int));
					}
				}
				long position = data.BaseStream.Position;
				if (num.HasValue)
				{
					data.BaseStream.Position = num.Value;
				}
				int valueOrDefault = (item.GetCustomAttribute<SizeAttribute>()?.Size).GetValueOrDefault(0);
				item.SetValue(obj, Deserialize(data, item.PropertyType, countRef2));
				if (valueOrDefault > 0)
				{
					data.BaseStream.Position = position + valueOrDefault;
				}
				else if (num.HasValue)
				{
					data.BaseStream.Position = position;
				}
			}
			return obj;
		}

		public static (PropertyInfo Property, string Kind, object OffsetReference, object CountReference, object ConditionReference)[] GetProperties(Type T)
		{
			if (!PropertyMap.TryGetValue(T, out (PropertyInfo, string, object, object, object)[] value))
			{
				if (T.GetCustomAttribute<DataContractAttribute>() == null)
				{
					throw new InvalidDataException(T.FullName + " does not have DataContractAttribute, please add data contract attributes or add a custom serializer");
				}
				IEnumerable<(PropertyInfo property, string Name, object descr, object cate, object)> temp = (from x in GetAllProperties(T)
					where x.GetCustomAttribute<DataMemberAttribute>() != null && x.GetCustomAttribute<IgnoreDataMemberAttribute>() == null
					select x).Select((Func<PropertyInfo, (PropertyInfo, string, object, object, object)>)delegate(PropertyInfo property)
				{
					object obj4 = property.GetCustomAttribute<DescriptionAttribute>()?.Description;
					if (obj4 != null && ((string)obj4).StartsWith("0x") && int.TryParse((string)obj4, NumberStyles.HexNumber, null, out int result))
					{
						obj4 = result;
					}
					object obj5 = property.GetCustomAttribute<CategoryAttribute>()?.Category;
					if (obj5 != null && ((string)obj5).StartsWith("0x") && int.TryParse((string)obj5, NumberStyles.HexNumber, null, out int result2))
					{
						obj5 = result2;
					}
					return (property, property.Name, obj4, obj5, property.GetCustomAttribute<ConditionalAttribute>()?.Condition);
				});
				value = temp.Select(delegate((PropertyInfo property, string Name, object descr, object cate, object) tuple)
				{
					(PropertyInfo property, string Name, object descr, object cate, object) valueTuple = tuple;
					PropertyInfo item = valueTuple.property;
					string item2 = valueTuple.Name;
					object obj = valueTuple.descr;
					object obj2 = valueTuple.cate;
					object obj3 = valueTuple.Item5;
					string offsetRef = obj as string;
					if (offsetRef != null)
					{
						obj = temp.FirstOrDefault(((PropertyInfo property, string Name, object descr, object cate, object) x) => x.Name == offsetRef).property;
					}
					string countRef = obj2 as string;
					if (countRef != null)
					{
						obj2 = temp.FirstOrDefault(((PropertyInfo property, string Name, object descr, object cate, object) x) => x.Name == countRef).property;
					}
					string conditionRef = obj3 as string;
					if (conditionRef != null)
					{
						obj3 = temp.FirstOrDefault(((PropertyInfo property, string Name, object descr, object cate, object) x) => x.Name == conditionRef).property;
					}
					return (item, item2, obj, obj2, obj3);
				}).ToArray();
				PropertyMap[T] = value;
			}
			return value;
		}

		private static List<PropertyInfo> GetAllProperties(Type T)
		{
			List<PropertyInfo> list = new List<PropertyInfo>();
			Queue<Type> queue = new Queue<Type>();
			HashSet<Type> hashSet = new HashSet<Type>();
			queue.Enqueue(T);
			while (queue.Count > 0)
			{
				Type type = queue.Dequeue();
				if (hashSet.Add(type))
				{
					if (type.BaseType.FullName != "System.Object")
					{
						queue.Enqueue(type.BaseType);
					}
					list.InsertRange(0, type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				}
			}
			return list;
		}

		public static string FString(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num == 0)
			{
				return string.Empty;
			}
			if (num > 0)
			{
				return Encoding.UTF8.GetString(reader.ReadBytes(num), 0, num - 1);
			}
			return Encoding.Unicode.GetString(reader.ReadBytes(-num * 2), 0, -num * 2 - 2);
		}

		public static Array FArray(BinaryReader reader, Type T)
		{
			int num = reader.ReadInt32();
			if (num == 0)
			{
				return Array.CreateInstance(T, 0);
			}
			return FArray(reader, num, T);
		}

		private static Array FArray(BinaryReader reader, int count, Type T)
		{
			Array array = Array.CreateInstance(T, count);
			for (int i = 0; i < count; i++)
			{
				array.SetValue(Deserialize(reader, T), i);
			}
			return array;
		}

		public static Guid FGuid(BinaryReader r)
		{
			return new Guid(r.ReadBytes(16));
		}
	}
}
