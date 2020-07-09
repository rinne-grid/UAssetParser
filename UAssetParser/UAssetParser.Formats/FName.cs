using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
	[DataContract]
	[DebuggerDisplay("[FName {Name} ({Index}|{InNumber})]")]
	public class FName
	{
		[DataMember]
		public int Index
		{
			get;
			set;
		}

		[DataMember]
		public int InNumber
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public string Name
		{
			get;
			set;
		}

		public string Ref(FNameEntry[] names)
		{
			if (Name != null)
			{
				return Name;
			}
			if (names.Length < Index || Index < 0)
			{
				return null;
			}
			Name = names[Index].Name;
			names[Index].RefCountOld++;
			return Name;
		}

		public static implicit operator string(FName @this)
		{
			return @this.Name;
		}

		public string GetValue(bool withInstance = false)
		{
			if ((!withInstance && !UAsset.Options.VeboseInstances) || InNumber <= 0)
			{
				return Name;
			}
			return $"{Name}({InNumber})";
		}

		public void Ref(FPackageFileSummary summary)
		{
			Ref(summary.Names);
		}

		public void UpdateIndex(string[] names, FPackageFileSummary summary)
		{
			Index = Array.BinarySearch(names, Name, StringComparer.OrdinalIgnoreCase);
			summary.Names[Index].RefCount++;
		}

		public void UpdateName(string newName, FPackageFileSummary summary)
		{
			Name = newName;
			if (string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName))
			{
				Name = "None";
				int num3 = Index = (InNumber = 0);
			}
			int num4 = newName.IndexOf('(');
			if (num4 > -1)
			{
				int num5 = newName.IndexOf(')');
				int num3 = 0;
				int length = num4 - num3;
				Name = newName.Substring(num3, length);
				length = ++num4;
				num3 = num5 - length;
				InNumber = Convert.ToInt32(newName.Substring(length, num3));
			}
			FNameEntry fNameEntry = new FNameEntry
			{
				Name = Name
			};
			if (summary.UpdatedNames.Add(fNameEntry))
			{
				fNameEntry.UpdateHash();
				summary.NewEntries.Add(fNameEntry.Name);
			}
		}
	}
}
