using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("NameProperty")]
	[Category("Unreal")]
	public class UNameProperty : FPropertyTag
	{
		[DataMember]
		public FPropertyGuid Guid
		{
			get;
			set;
		}

		[DataMember]
		public FName Value
		{
			get;
			set;
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			Value.Ref(summary);
		}

		public override object GetValue()
		{
			return Value.GetValue(UAsset.Options.PrintInstances);
		}

		public override int GetSize()
		{
			if (Guid.HasGuid != 0)
			{
				return 49;
			}
			return 33;
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			string newName = Convert.ToString(data);
			if (Value == null)
			{
				Value = new FName();
			}
			if (Guid == null)
			{
				Guid = new FPropertyGuid
				{
					HasGuid = 0
				};
			}
			Value.UpdateName(newName, summary);
		}

		public override void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			base.UpdateIndex(names, summary);
			Value.UpdateIndex(names, summary);
		}
	}
}
