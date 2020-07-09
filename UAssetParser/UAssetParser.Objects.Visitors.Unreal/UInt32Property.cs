using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("UInt32Property")]
	[Category("Unreal")]
	public class UInt32Property : FPropertyTag
	{
		[DataMember]
		public FPropertyGuid Guid
		{
			get;
			set;
		}

		[DataMember]
		public uint Value
		{
			get;
			set;
		}

		public override object GetValue()
		{
			return Value;
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			Value = Convert.ToUInt32(data);
		}
	}
}
