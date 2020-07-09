using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("IntProperty")]
	[Category("Unreal")]
	public class UIntProperty : FPropertyTag
	{
		[DataMember]
		public FPropertyGuid Guid
		{
			get;
			set;
		}

		[DataMember]
		public int Value
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
			Value = Convert.ToInt32(data);
		}
	}
}
