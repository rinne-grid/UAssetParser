using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("EnumProperty")]
	[Category("Unreal")]
	public class UEnumProperty : FPropertyTag
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

		[DataMember]
		public FName Value
		{
			get;
			set;
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			EnumName.Ref(summary);
			Value?.Ref(summary);
		}

		public override object GetValue()
		{
			return Value.GetValue(UAsset.Options.PrintInstances);
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			string newName = data.ToString();
			Value.UpdateName(newName, summary);
		}

		public override void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			base.UpdateIndex(names, summary);
			EnumName.UpdateIndex(names, summary);
			Value.UpdateIndex(names, summary);
		}
	}
}
