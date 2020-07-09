using System;
using System.Collections.Generic;
using UAssetParser.Formats;

namespace UAssetParser.Extensions
{
	public class FNameByString : IComparer<FNameEntry>
	{
		public int Compare(FNameEntry x, FNameEntry y)
		{
			return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
		}
	}
}
