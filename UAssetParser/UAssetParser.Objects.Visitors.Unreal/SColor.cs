using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("Color")]
	[Category("Unreal")]
	public class SColor : IStructObject
	{
		[DataMember]
		public byte R
		{
			get;
			set;
		}

		[DataMember]
		public byte G
		{
			get;
			set;
		}

		[DataMember]
		public byte B
		{
			get;
			set;
		}

		[DataMember]
		public byte A
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
			return 4;
		}
	}
}
