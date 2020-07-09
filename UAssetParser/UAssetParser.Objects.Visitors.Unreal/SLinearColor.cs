using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("LinearColor")]
	[Category("Unreal")]
	public class SLinearColor : IStructObject
	{
		[DataMember]
		public float R
		{
			get;
			set;
		}

		[DataMember]
		public float G
		{
			get;
			set;
		}

		[DataMember]
		public float B
		{
			get;
			set;
		}

		[DataMember]
		public float A
		{
			get;
			set;
		}

		public object Serialize()
		{
			return this;
		}

		public int GetSize()
		{
			return 16;
		}
	}
}
