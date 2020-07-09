using System.Linq;

namespace UAssetParser.Extensions
{
	public static class StringHelper
	{
		public static bool isWide(this string str)
		{
			return str.Any((char c) => c > '\u007f');
		}

		public static int FLength(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			if (!str.isWide())
			{
				return str.Length;
			}
			return str.Length * 2;
		}

		public static int FLengthWithNull(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			if (!str.isWide())
			{
				return str.Length + 1;
			}
			return str.Length * 2 + 2;
		}
	}
}
