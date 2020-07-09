using System.Diagnostics;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
	[DataContract]
	[DebuggerDisplay("{Major}.{Minor}.{Patch} ({Changeset})")]
	public class FEngineVersion
	{
		[DataMember]
		public uint Major
		{
			get;
			set;
		}

		[DataMember]
		public uint Minor
		{
			get;
			set;
		}

		[DataMember]
		public uint Patch
		{
			get;
			set;
		}

		[DataMember]
		public uint Changeset
		{
			get;
			set;
		}
	}
}
