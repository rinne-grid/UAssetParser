using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
	[DataContract]
	[DebuggerDisplay("[FPropertyTag {Name} ({Type})]")]
	public class FPropertyTag
	{
		[DataMember]
		public FName Name
		{
			get;
			set;
		}

		[DataMember]
		public FName Type
		{
			get;
			set;
		}

		[DataMember]
		public int Size
		{
			get;
			set;
		}

		[DataMember]
		public int Index
		{
			get;
			set;
		}

		public virtual void Ref(FPackageFileSummary summary)
		{
			Name.Ref(summary);
			Type.Ref(summary);
		}

		public virtual object GetValue()
		{
			return null;
		}

		public virtual int GetSize()
		{
			throw new NotImplementedException("No GetSize for the caller type!");
		}

		public virtual void UpdateFromJSON(object data, FPackageFileSummary summary)
		{
		}

		public void GenerateNew(string name, string type, FPackageFileSummary summary)
		{
			Name = new FName();
			Type = new FName();
			Name.UpdateName(name, summary);
			Type.UpdateName(type, summary);
		}

		public virtual void BSerialize(BinaryWriter writer, FPackageFileSummary summary)
		{
		}

		public virtual void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			Name.UpdateIndex(names, summary);
			Type.UpdateIndex(names, summary);
		}
	}
}
