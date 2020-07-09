using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("SetProperty")]
	[Category("Unreal")]
	public class USetProperty : FPropertyTag
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
		public int Zero
		{
			get;
			set;
		}

		[DataMember]
		public int Count
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public List<object> Entries
		{
			get;
			set;
		}

		public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
		{
			USetProperty uSetProperty = LSerializer.Deserialize<USetProperty>(reader);
			uSetProperty.Ref(summary);
			long position = reader.BaseStream.Position;
			uSetProperty.Entries = UArrayProperty.GetEntries(uSetProperty.Count, uSetProperty.ArrayType, reader, summary);
			reader.BaseStream.Position = position + uSetProperty.Size - 8;
			return uSetProperty;
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			ArrayType.Ref(summary);
		}

		public override object GetValue()
		{
			return Entries.Select(UArrayProperty.Unwrap).ToArray();
		}
	}
}
