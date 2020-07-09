using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using UAssetParser.Extensions;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("TextProperty")]
	[Category("Unreal")]
	public class UTextProperty : FPropertyTag
	{
		[DataMember]
		[Conditional("HasValue")]
		public FPropertyGuid Guid
		{
			get;
			set;
		}

		[DataMember]
		[Conditional("HasValue")]
		public long StreamOffset
		{
			get;
			set;
		}

		[DataMember]
		[Conditional("HasValue")]
		public FPropertyGuid Key
		{
			get;
			set;
		}

		[DataMember]
		[Conditional("HasValue")]
		public string Hash
		{
			get;
			set;
		}

		[DataMember]
		public string Value
		{
			get;
			set;
		}

		[DataMember]
		public byte HasValue
		{
			get
			{
				return Convert.ToByte(base.Size > 7);
			}
			set
			{
			}
		}

		public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
		{
			UTextProperty uTextProperty = LSerializer.Deserialize<UTextProperty>(reader);
			if (baseTag.Size < 8)
			{
				reader.BaseStream.Position++;
			}
			else
			{
				reader.BaseStream.Position--;
			}
			uTextProperty.Ref(summary);
			return uTextProperty;
		}

		public override object GetValue()
		{
			return new
			{
				Hash = Hash,
				Text = Value
			};
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			var anon = JsonConvert.DeserializeAnonymousType(data.ToString(), new
			{
				Hash = string.Empty,
				Text = string.Empty
			});
			if (!string.Equals(Value, anon.Text, StringComparison.OrdinalIgnoreCase))
			{
				Value = anon.Text;
				Hash = (string.IsNullOrEmpty(Value) ? string.Empty : anon.Hash);
				base.Size = ((string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(Hash)) ? 5 : (33 + Value.FLengthWithNull() + 4 + 4 + 8 + 1));
			}
		}

		public override void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
			writer.BaseStream.Position--;
			if (base.Size < 8)
			{
				writer.Write((short)(-256));
			}
		}
	}
}
