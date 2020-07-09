using System;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
	[DataContract]
	public class FPropertyGuid
	{
		[DataMember]
		public byte HasGuid
		{
			get;
			set;
		}

		[DataMember]
		[Conditional("HasGuid")]
		public Guid Guid
		{
			get;
			set;
		} = Guid.Empty;

	}
}
