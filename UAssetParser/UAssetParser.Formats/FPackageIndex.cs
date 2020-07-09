using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
	[DataContract]
	public class FPackageIndex
	{
		[DataMember]
		public int Index
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public object ObjectResource
		{
			get;
			private set;
		}

		public bool IsImport => Index < 0;

		public bool IsExport => Index > 0;

		public bool IsNull => Index == 0;

		public string Name
		{
			get
			{
				FObjectExport fObjectExport = ObjectResource as FObjectExport;
				if (fObjectExport != null)
				{
					return fObjectExport.ObjectName;
				}
				FObjectImport fObjectImport = ObjectResource as FObjectImport;
				if (fObjectImport != null)
				{
					return fObjectImport.ObjectName;
				}
				return null;
			}
		}

		public string FullName
		{
			get
			{
				FObjectExport fObjectExport = ObjectResource as FObjectExport;
				if (fObjectExport != null)
				{
					return fObjectExport.ObjectName;
				}
				FObjectImport fObjectImport = ObjectResource as FObjectImport;
				if (fObjectImport != null)
				{
					if (!fObjectImport.PackageRef.IsNull)
					{
						return fObjectImport.PackageRef.Name;
					}
					return fObjectImport.ObjectName;
				}
				return null;
			}
		}

		public object Ref(FObjectExport[] exports, FObjectImport[] imports)
		{
			if (ObjectResource != null)
			{
				return ObjectResource;
			}
			int num = -Index - 1;
			int num2 = Index - 1;
			if (IsImport && imports != null && imports.Length > num)
			{
				ObjectResource = imports[num];
			}
			if (IsExport && exports != null && exports.Length > num2)
			{
				ObjectResource = exports[num2];
			}
			return ObjectResource;
		}

		public object Ref(FPackageFileSummary summary)
		{
			if (summary == null)
			{
				return ObjectResource;
			}
			return Ref(summary.Exports, summary.Imports);
		}
	}
}
