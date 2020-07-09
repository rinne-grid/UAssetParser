using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using UAssetParser.Extensions;

namespace UAssetParser.Formats
{
	[DataContract]
	[DebuggerDisplay("{Name}")]
	public class FNameEntry : IEquatable<FNameEntry>
	{
		[DataMember]
		public string Name
		{
			get;
			set;
		}

		[DataMember]
		public ushort NonCasePreservingHash
		{
			get;
			set;
		}

		[DataMember]
		public ushort CasePreservingHash
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public bool isWide => Name.isWide();

		[IgnoreDataMember]
		public int RefCountOld
		{
			get;
			set;
		}

		[IgnoreDataMember]
		public int RefCount
		{
			get;
			set;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as FNameEntry);
		}

		public bool Equals(FNameEntry p)
		{
			if (p == null)
			{
				return false;
			}
			if (this == p)
			{
				return true;
			}
			if (GetType() != p.GetType())
			{
				return false;
			}
			return string.Equals(Name, p.Name, StringComparison.OrdinalIgnoreCase);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public int GetLength()
		{
			if (!Name.isWide())
			{
				return Name.Length + 9;
			}
			return Name.Length * 2 + 10;
		}

		public void UpdateHash()
		{
			NonCasePreservingHash = SetNCPHash();
			CasePreservingHash = SetCPHash();
		}

		private ushort SetNCPHash()
		{
			uint num = 0u;
			for (int i = 0; i < Name.Length; i++)
			{
				if (!isWide)
				{
					byte b = Convert.ToByte(char.ToUpper(Name[i]));
					num = (((num >> 8) & 0xFFFFFF) ^ CRC32Hash.DeprecatedTable[(num ^ b) & 0xFF]);
					continue;
				}
				char num2 = Name[i];
				ushort num3 = Convert.ToUInt16(char.ToUpper(num2));
				num = (((num >> 8) & 0xFFFFFF) ^ CRC32Hash.DeprecatedTable[(num ^ num3) & 0xFF]);
				num3 = Convert.ToUInt16((int)num2 >> 8);
				num = (((num >> 8) & 0xFFFFFF) ^ CRC32Hash.DeprecatedTable[(num ^ num3) & 0xFF]);
			}
			return Convert.ToUInt16(num & 0xFFFF);
		}

		private ushort SetCPHash()
		{
			uint num = uint.MaxValue;
			if (!isWide)
			{
				byte[] bytes = Encoding.ASCII.GetBytes(Name);
				for (int i = 0; i < bytes.Length; i++)
				{
					num = ((num >> 8) ^ CRC32Hash.NewTable[bytes[i] ^ (num & 0xFF)]);
					num = ((num >> 8) ^ CRC32Hash.NewTable[num & 0xFF]);
					num = ((num >> 8) ^ CRC32Hash.NewTable[num & 0xFF]);
					num = ((num >> 8) ^ CRC32Hash.NewTable[num & 0xFF]);
				}
			}
			else
			{
				for (int j = 0; j < Name.Length; j++)
				{
					char c = Name[j];
					num = ((num >> 8) ^ CRC32Hash.NewTable[(num ^ c) & 0xFF]);
					num = ((num >> 8) ^ CRC32Hash.NewTable[(num ^ (c = (char)((int)c >> 8))) & 0xFF]);
					num = ((num >> 8) ^ CRC32Hash.NewTable[(num ^ (c = (char)((int)c >> 8))) & 0xFF]);
					num = ((num >> 8) ^ CRC32Hash.NewTable[(num ^ (c = (char)((int)c >> 8))) & 0xFF]);
				}
			}
			return Convert.ToUInt16(~num & 0xFFFF);
		}
	}
}
