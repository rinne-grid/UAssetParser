using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
	[DataContract]
	public class FGenerationInfo
	{
		[DataMember]
		public int ExportCount
		{
			get;
			set;
		}

		[DataMember]
		public int NameCount
		{
			get;
			set;
		}
	}
}
