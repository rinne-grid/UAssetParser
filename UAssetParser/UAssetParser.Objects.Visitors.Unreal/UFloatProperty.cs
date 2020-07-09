using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("FloatProperty")]
	[Category("Unreal")]
	public class UFloatProperty : FPropertyTag
	{
		[DataMember]
		public FPropertyGuid Guid
		{
			get;
			set;
		}

		[DataMember]
		public float Value
		{
			get;
			set;
		}

		public override object GetValue()
		{
			return Value;
		}

		public override int GetSize()
		{
			if (Guid.HasGuid != 0)
			{
				return 45;
			}
			return 29;
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			Value = Convert.ToSingle(data);
			if (Guid == null)
			{
				Guid = new FPropertyGuid
				{
					HasGuid = 0
				};
			}
		}
	}
}
