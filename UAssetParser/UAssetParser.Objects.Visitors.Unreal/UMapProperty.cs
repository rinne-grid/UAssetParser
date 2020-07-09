using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("MapProperty")]
	[Category("Unreal")]
	public class UMapProperty : FPropertyTag
	{
		[DataMember]
		public FName KeyType
		{
			get;
			set;
		}

		[DataMember]
		public FName ValueType
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
		public List<(object, object)> Entries
		{
			get;
			set;
		} = new List<(object, object)>();


		public static FPropertyTag PropertyVisitor(BinaryReader reader, FPropertyTag baseTag, FPackageFileSummary summary)
		{
			UMapProperty uMapProperty = LSerializer.Deserialize<UMapProperty>(reader);
			uMapProperty.Ref(summary);
			for (int i = 0; i < uMapProperty.Count; i++)
			{
				uMapProperty.Entries.Add((VisitorFactory.VisitEnumerable(reader, uMapProperty.KeyType, summary, 1), VisitorFactory.VisitEnumerable(reader, uMapProperty.ValueType, summary, 1)));
			}
			return uMapProperty;
		}

		public override void Ref(FPackageFileSummary summary)
		{
			base.Ref(summary);
			KeyType.Ref(summary);
			ValueType.Ref(summary);
		}

		public override object GetValue()
		{
			return Entries.Select(((object, object) x) => new KeyValuePair<object, object>(UArrayProperty.Unwrap(x.Item1), UArrayProperty.Unwrap(x.Item2)));
		}
	}
}
