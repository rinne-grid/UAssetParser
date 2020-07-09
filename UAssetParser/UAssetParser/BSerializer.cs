using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UAssetParser.Extensions;
using UAssetParser.Formats;

namespace UAssetParser
{
	public static class BSerializer
	{
		public delegate void PrimitiveVisitor(BinaryWriter writer, object value);

		public static Dictionary<Type, PrimitiveVisitor> PrimitiveMap = new Dictionary<Type, PrimitiveVisitor>
		{
			{
				typeof(sbyte),
				delegate(BinaryWriter r, object v)
				{
					r.Write((sbyte)v);
				}
			},
			{
				typeof(byte),
				delegate(BinaryWriter r, object v)
				{
					r.Write((byte)v);
				}
			},
			{
				typeof(ushort),
				delegate(BinaryWriter r, object v)
				{
					r.Write((ushort)v);
				}
			},
			{
				typeof(short),
				delegate(BinaryWriter r, object v)
				{
					r.Write((short)v);
				}
			},
			{
				typeof(uint),
				delegate(BinaryWriter r, object v)
				{
					r.Write((uint)v);
				}
			},
			{
				typeof(int),
				delegate(BinaryWriter r, object v)
				{
					r.Write((int)v);
				}
			},
			{
				typeof(ulong),
				delegate(BinaryWriter r, object v)
				{
					r.Write((ulong)v);
				}
			},
			{
				typeof(long),
				delegate(BinaryWriter r, object v)
				{
					r.Write((long)v);
				}
			},
			{
				typeof(bool),
				delegate(BinaryWriter r, object v)
				{
					r.Write(Convert.ToUInt32(v));
				}
			},
			{
				typeof(char),
				delegate(BinaryWriter r, object v)
				{
					r.Write((char)v);
				}
			},
			{
				typeof(decimal),
				delegate(BinaryWriter r, object v)
				{
					r.Write((decimal)v);
				}
			},
			{
				typeof(double),
				delegate(BinaryWriter r, object v)
				{
					r.Write((double)v);
				}
			},
			{
				typeof(float),
				delegate(BinaryWriter r, object v)
				{
					r.Write((float)v);
				}
			},
			{
				typeof(string),
				delegate(BinaryWriter r, object v)
				{
					WriteFString(r, (string)v);
				}
			},
			{
				typeof(Guid),
				delegate(BinaryWriter r, object v)
				{
					WriteFGuid(r, (Guid)v);
				}
			}
		};

		public static void Serialize<T>(BinaryWriter data, object instance)
		{
			Serialize(data, typeof(T), instance);
		}

		public static void Serialize(BinaryWriter data, Type T, object instance)
		{
			if (T.IsEnum)
			{
				T = T.GetEnumUnderlyingType();
			}
			if (PrimitiveMap.TryGetValue(T, out PrimitiveVisitor value))
			{
				value(data, instance);
			}
			else
			{
				SerializeComplex(data, T, instance);
			}
		}

		private static void SerializeComplex(BinaryWriter writer, Type T, object instance)
		{
			if (T.IsArray)
			{
				FArray(writer, T, instance);
				return;
			}
			(PropertyInfo, string, object, object, object)[] properties = LSerializer.GetProperties(T);
			for (int i = 0; i < properties.Length; i++)
			{
				(PropertyInfo, string, object, object, object) valueTuple = properties[i];
				PropertyInfo item = valueTuple.Item1;
				object item2 = valueTuple.Item3;
				object item3 = valueTuple.Item4;
				PropertyInfo propertyInfo = valueTuple.Item5 as PropertyInfo;
				if ((object)propertyInfo != null && (int)Convert.ChangeType(propertyInfo.GetValue(instance), typeof(int)) != 1)
				{
					continue;
				}
				int? num = item2 as int?;
				int? num2 = item3 as int?;
				if (!num.HasValue)
				{
					PropertyInfo propertyInfo2 = item2 as PropertyInfo;
					if ((object)propertyInfo2 != null)
					{
						num = (int)Convert.ChangeType(propertyInfo2.GetValue(instance), typeof(int));
					}
				}
				if (!num2.HasValue)
				{
					PropertyInfo propertyInfo3 = item3 as PropertyInfo;
					if ((object)propertyInfo3 != null)
					{
						num2 = (int)Convert.ChangeType(propertyInfo3.GetValue(instance), typeof(int));
					}
				}
				long position = writer.BaseStream.Position;
				int valueOrDefault = (item.GetCustomAttribute<SizeAttribute>()?.Size).GetValueOrDefault(0);
				Serialize(writer, item.PropertyType, item.GetValue(instance));
				if (valueOrDefault > 0)
				{
					writer.BaseStream.Position = position + valueOrDefault;
				}
			}
		}

		public static void WriteFString(BinaryWriter writer, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				string text = value + "\0";
				int length = text.Length;
				byte[] buffer = value.isWide() ? Encoding.Unicode.GetBytes(text) : Encoding.ASCII.GetBytes(text);
				length = (value.isWide() ? (-length) : length);
				writer.Write(length);
				writer.Write(buffer);
			}
			else
			{
				writer.Write(0);
			}
		}

		public static void FArray(BinaryWriter writer, Type T, object instance)
		{
			Array array = instance as Array;
			if (T != typeof(FNameEntry[]) && T != typeof(FObjectImport[]) && T != typeof(FObjectExport[]))
			{
				writer.Write(array.Length);
			}
			for (int i = 0; i < array.Length; i++)
			{
				Serialize(writer, array.GetValue(i).GetType(), array.GetValue(i));
			}
		}

		public static void WriteFGuid(BinaryWriter writer, Guid value)
		{
			writer.Write(value.ToByteArray());
		}
	}
}
