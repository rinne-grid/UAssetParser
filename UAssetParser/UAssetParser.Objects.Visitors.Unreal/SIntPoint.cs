using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("IntPoint")]
	[Category("Unreal")]
	public class SIntPoint : IStructObject
	{
		[DataMember]
		public int X
		{
			get;
			set;
		}

		[DataMember]
		public int Y
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
			return 8;
		}
	}
}
