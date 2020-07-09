using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace UAssetParser.Objects.Visitors.Unreal
{
	[DataContract]
	[Description("Guid")]
	[Category("Unreal")]
	public class SGuid : IStructObject
	{
		[DataMember]
		public Guid FGuid
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
