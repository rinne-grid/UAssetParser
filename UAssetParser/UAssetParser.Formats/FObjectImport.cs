using System.Diagnostics;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
	[DataContract]
	[DebuggerDisplay("[FObjectImport {ObjectName}]")]
	public class FObjectImport
	{
		[DataMember]
		public FName ClassPackage
		{
			get;
			set;
		}

		[DataMember]
		public FName ClassName
		{
			get;
			set;
		}

		[DataMember]
		public FPackageIndex PackageRef
		{
			get;
			set;
		}

		[DataMember]
		public FName ObjectName
		{
			get;
			set;
		}

		internal void Ref(FPackageFileSummary summary)
		{
			ClassPackage.Ref(summary);
			ClassName.Ref(summary);
			PackageRef.Ref(summary);
			ObjectName.Ref(summary);
		}

		public void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			ClassPackage.UpdateIndex(names, summary);
			ClassName.UpdateIndex(names, summary);
			ObjectName.UpdateIndex(names, summary);
		}
	}
}
