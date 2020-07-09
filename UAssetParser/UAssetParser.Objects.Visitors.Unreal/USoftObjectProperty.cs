using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using UAssetParser.Extensions;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("SoftObjectProperty")]
	[Category("Unreal")]
	public class USoftObjectProperty : FPropertyTag
	{
		[DataMember]
		public FPropertyGuid Guid
		{
			get;
			set;
		}

		[DataMember]
		public FName Package
		{
			get;
			set;
		}

		[DataMember]
		public string Path
		{
			get;
			set;
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			Package.Ref(summary);
		}

		public override object GetValue()
		{
			bool printInstances = UAsset.Options.PrintInstances;
			return new
			{
				Package = Package.GetValue(printInstances),
				Path = Path
			};
		}

		public override void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
			var anonymousTypeObject = new
			{
				Package = string.Empty,
				Path = string.Empty
			};
			var anon = JsonConvert.DeserializeAnonymousType(data.ToString(), anonymousTypeObject);
			Package.UpdateName(anon.Package, summary);
			if (!string.Equals(Path, anon.Path, StringComparison.OrdinalIgnoreCase))
			{
				Path = anon.Path;
				base.Size = 12 + Path.FLengthWithNull();
			}
		}

		public override void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			base.UpdateIndex(names, summary);
			Package.UpdateIndex(names, summary);
		}
	}
}
