using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("ByteProperty")]
	[Category("Unreal")]
	public class UByteProperty : FPropertyTag
	{
		[DataMember]
		public FName EnumName
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
		public object Value
		{
			get;
			set;
		}

		public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
		{
			UByteProperty uByteProperty = LSerializer.Deserialize<UByteProperty>(reader);
			uByteProperty.Ref(summary);
			if (uByteProperty.EnumName.Name == "None")
			{
				uByteProperty.Value = reader.ReadByte();
			}
			else
			{
				uByteProperty.Value = LSerializer.Deserialize<FName>(reader);
				uByteProperty.Ref(summary);
			}
			return uByteProperty;
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			EnumName.Ref(summary);
			(Value as FName)?.Ref(summary);
		}

		public override object GetValue()
		{
			FName fName = Value as FName;
			if (fName != null)
			{
				return fName.GetValue(UAsset.Options.PrintInstances);
			}
			return Value;
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			FName fName = Value as FName;
			if (fName != null)
			{
				fName.UpdateName(data.ToString(), summary);
				return;
			}
			Value = Convert.ToByte(data);
			throw new NotImplementedException("Cannot recalculate actual byte size for ByteProperty");
		}

		public override void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
			if (EnumName.Name == "None")
			{
				writer.Write(Convert.ToByte(Value));
			}
			else
			{
				BSerializer.Serialize(writer, typeof(FName), (FName)Value);
			}
		}

		public override void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			base.UpdateIndex(names, summary);
			EnumName.UpdateIndex(names, summary);
			(Value as FName)?.UpdateIndex(names, summary);
		}
	}
}
